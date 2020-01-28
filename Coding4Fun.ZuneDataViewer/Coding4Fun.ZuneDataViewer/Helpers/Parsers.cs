using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Linq;
using Coding4Fun.ZuneDataViewer.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Coding4Fun.ZuneDataViewer.ViewModel;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.IO.IsolatedStorage;

namespace Coding4Fun.ZuneDataViewer.Helpers
{
    public static class Parsers
    {
        public static void ParseProfileInfo(string xml)
        {
            XDocument doc = XDocument.Parse(xml);

            Profile.ZuneName = doc.Root.Element("{http://schemas.zune.net/profiles/2008/01}zunetag").Value.ToString();
            Profile.RealName = doc.Root.Element("{http://schemas.zune.net/profiles/2008/01}displayname").Value.ToString();
            Profile.Location = doc.Root.Element("{http://schemas.zune.net/profiles/2008/01}location").Value.ToString();
            Profile.PlayCount = doc.Root.Element("{http://schemas.zune.net/profiles/2008/01}playcount").Value.ToString();
            Profile.Status = doc.Root.Element("{http://schemas.zune.net/profiles/2008/01}status").Value.ToString();
            Profile.Bio = doc.Root.Element("{http://schemas.zune.net/profiles/2008/01}bio").Value.ToString();

            string date = doc.Root.Element("{http://www.w3.org/2005/Atom}updated").Value.ToString();
            DateTime time = DateTime.Parse(date);
            Profile.LastUpdate = time.ToLongDateString() + " " + time.ToLongTimeString();

            XElement images = doc.Root.Element("{http://schemas.zune.net/profiles/2008/01}images");
            Profile.ProfileImageURL = (from c in images.Elements() where c.Attribute("title").Value == "usertile" select c).First().Attribute("href").Value.ToString();
            Profile.BackgroundImageURL = (from c in images.Elements() where c.Attribute("title").Value == "background" select c).First().Attribute("href").Value.ToString();

            XElement playlists = doc.Root.Element("{http://schemas.zune.net/profiles/2008/01}playlists");
            Profile.RecentPlaylistURL = (from c in playlists.Elements() where c.Attribute("title").Value == "BuiltIn-RecentTracks" select c).First().Attribute("href").Value.ToString();
            Profile.MostPlayedPlaylistURL = (from c in playlists.Elements() where c.Attribute("title").Value == "BuiltIn-MostPlayedArtists" select c).First().Attribute("href").Value.ToString();
            Profile.FavortiesPlaylistURL = (from c in playlists.Elements() where c.Attribute("title").Value == "BuiltIn-FavoriteTracks" select c).First().Attribute("href").Value.ToString();
            Profile.BadgeUrl = string.Format("http://socialapi.zune.net/en-US/members/{0}/badges", IsolatedStorageSettings.ApplicationSettings["GUID"].ToString());

            Profile.FriendsUrl = (from c in doc.Root.Elements() where c.Attribute("title") != null && c.Attribute("title").Value == "friends" select c)
                .FirstOrDefault().Attribute("href").Value;
        }

        public static void ParseRecentPlaylist(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            foreach (XElement element in doc.Root.Elements())
            {
                XElement x = element.Element("{http://schemas.zune.net/catalog/music/2007/10}track");
                if (x != null)
                {
                    RecentSong song = new RecentSong();
                    song.Title = x.Element("{http://schemas.zune.net/catalog/music/2007/10}title").Value;
                    song.Album = x.Element("{http://schemas.zune.net/catalog/music/2007/10}album").
                        Element("{http://schemas.zune.net/catalog/music/2007/10}title").Value;
                    song.Artist = x.Element("{http://schemas.zune.net/catalog/music/2007/10}primaryArtist").
                        Element("{http://schemas.zune.net/catalog/music/2007/10}name").Value;

                    song.AlbumID = x.Element("{http://schemas.zune.net/catalog/music/2007/10}album")
                        .Element("{http://schemas.zune.net/catalog/music/2007/10}id").Value.Remove(0,9);

                    
                    WebClient client = new WebClient();
                    client.DownloadStringAsync(new Uri("http://catalog.zune.net/v3.2/en-US/music/album/" + song.AlbumID));
                    client.DownloadStringCompleted += (s, e) => HandleDownload(s, e, song);     
                }
            }
        }

        public static void ParseMostPlayed(string xml)
        {

            XDocument doc = XDocument.Parse(xml);
            XElement x = doc.Root.Element("{http://schemas.zune.net/profiles/2008/01}artists");
            foreach (XElement artist in x.Elements())
            {
                Artist _artist = new Artist();
                _artist.Name = artist.Element("{http://schemas.zune.net/profiles/2008/01}name").Value;
                _artist.ArtistID = artist.Element("{http://schemas.zune.net/profiles/2008/01}id").Value.Remove(0, 9);

                WebClient client = new WebClient();
                client.DownloadStringAsync(new Uri("http://catalog.zune.net/v3.2/en-US/music/artist/" + _artist.ArtistID));
                client.DownloadStringCompleted += (s, e) => HandleArtistDownload(s, e, _artist);    
            }
        }

        public static void ParseFavorites(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            XElement[] elements = (from c in doc.Root.Elements() where c.Name == "{http://www.w3.org/2005/Atom}entry" select c).ToArray();

            foreach (XElement fav in elements)
            {
                XElement trackRoot = fav.Element("{http://schemas.zune.net/catalog/music/2007/10}track");
                Favorite favorite = new Favorite();
                favorite.Title = fav.Element("{http://www.w3.org/2005/Atom}title").Value;
                favorite.Length = trackRoot.Element("{http://schemas.zune.net/catalog/music/2007/10}length").Value.Remove(0, 2)
                    .Replace('H',':').Replace('M',':').Replace("S",string.Empty);
                favorite.IsExplicit = Convert.ToBoolean(trackRoot.Element("{http://schemas.zune.net/catalog/music/2007/10}isExplicit").Value);
                favorite.ArtistName = trackRoot.Element("{http://schemas.zune.net/catalog/music/2007/10}primaryArtist").
                    Element("{http://schemas.zune.net/catalog/music/2007/10}name").Value;
                favorite.ArtistID = trackRoot.Element("{http://schemas.zune.net/catalog/music/2007/10}primaryArtist").
                    Element("{http://schemas.zune.net/catalog/music/2007/10}id").Value.Remove(0,9);
                favorite.AlbumName = trackRoot.Element("{http://schemas.zune.net/catalog/music/2007/10}album").
                    Element("{http://schemas.zune.net/catalog/music/2007/10}title").Value;
                favorite.AlbumID = trackRoot.Element("{http://schemas.zune.net/catalog/music/2007/10}album").
                    Element("{http://schemas.zune.net/catalog/music/2007/10}id").Value.Remove(0,9);

                WebClient client = new WebClient();
                client.DownloadStringAsync(new Uri("http://catalog.zune.net/v3.2/en-US/music/artist/" + favorite.ArtistID));
                client.DownloadStringCompleted += (s, e) => HandleFArtistDownload(s, e, favorite);
            }
        }

        public static void ParseBadges(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            XElement[] elements = (from c in doc.Root.Elements() where c.Name == "{http://www.w3.org/2005/Atom}entry" select c).ToArray();

            foreach (XElement badge in elements)
            {
                Badge b = new Badge();

                b.Type = badge.Element("{http://www.w3.org/2005/Atom}title").Value;
                string imageUrl = (from c in badge.Elements()
                                   where c.Name == "{http://www.w3.org/2005/Atom}link" && c.Attribute("rel").Value == "enclosure"
                                   select c).FirstOrDefault().Attribute("href").Value;

                b.Image = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));

                string artistID = badge.Element("{http://schemas.zune.net/profiles/2008/01}media")
                    .Element("{http://schemas.zune.net/profiles/2008/01}id").Value;

                WebClient client = new WebClient();
                client.DownloadStringAsync(new Uri("http://catalog.zune.net/v3.2/en-US/music/artist/" + artistID));
                client.DownloadStringCompleted += (s, e) => HandleBArtistDownload(s, e, b);
            }
        }

        public static void ParseFriends(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            XElement[] elements = (from c in doc.Root.Elements() where c.Name == "{http://www.w3.org/2005/Atom}entry" select c).ToArray();

            foreach (XElement friend in elements)
            {
                Friend f = new Friend();
                f.Name = friend.Element("{http://www.w3.org/2005/Atom}title").Value;
                f.Playcount = friend.Element("{http://schemas.zune.net/profiles/2008/01}playcount").Value + " plays";

                string profileImageUrl = (from c in friend.Element("{http://schemas.zune.net/profiles/2008/01}images").Elements() 
                                          where c.Attribute("title").Value == "usertile"
                                          select c).FirstOrDefault().Attribute("href").Value;
                string backgroundImageUrl = (from c in friend.Element("{http://schemas.zune.net/profiles/2008/01}images").Elements() 
                                          where c.Attribute("title").Value == "background"
                                          select c).FirstOrDefault().Attribute("href").Value;

                f.ProfileImage = new BitmapImage(new Uri(profileImageUrl, UriKind.Absolute));
                f.BackgroundImage = new BitmapImage(new Uri(backgroundImageUrl, UriKind.Absolute));
                ProfileViewModelLocator.ProfileModel.Friends.Add(f);
            }
        }

        private static void HandleBArtistDownload(object sender, DownloadStringCompletedEventArgs e, Badge badge)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(e.Result);
                string name = xdoc.Root.Element("{http://www.w3.org/2005/Atom}title").Value;

                badge.Artist = name;

            }
            catch
            {
                badge.Artist = string.Empty;
            }

            ProfileViewModelLocator.ProfileModel.Badges.Add(badge);

        }

        private static void HandleFArtistDownload(object sender, DownloadStringCompletedEventArgs e, Favorite fav)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(e.Result);
                string imageId = xdoc.Root.Element("{http://schemas.zune.net/catalog/music/2007/10}image")
                    .Element("{http://schemas.zune.net/catalog/music/2007/10}id").Value.Remove(0, 9);

                BitmapImage image = new BitmapImage(new Uri(string.Format("http://catalog.zune.net/v3.2/image/{0}?width=300&height=300", imageId)));

                fav.ArtistImage = image;

            }
            catch
            {
                fav.ArtistImage = new BitmapImage(new Uri("Images/unknown.png", UriKind.Relative));
            }

            WebClient client = new WebClient();
            client.DownloadStringAsync(new Uri("http://catalog.zune.net/v3.2/en-US/music/album/" + fav.AlbumID));
            client.DownloadStringCompleted += (s, ev) => HandleFDownload(s, ev, fav);

        }

        private static void HandleFDownload(object sender, DownloadStringCompletedEventArgs ev, Favorite fav)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(ev.Result);
                string imageId = xdoc.Root.Element("{http://schemas.zune.net/catalog/music/2007/10}image")
                    .Element("{http://schemas.zune.net/catalog/music/2007/10}id").Value.Remove(0, 9);

                BitmapImage image = new BitmapImage(new Uri(string.Format("http://catalog.zune.net/v3.2/image/{0}?width=100&height=100", imageId)));
                fav.AlbumImage = image;
            }
            catch
            {
                fav.AlbumImage = new BitmapImage(new Uri("Images/unknown.png", UriKind.Relative));
            }

            ProfileViewModelLocator.ProfileModel.Favorites.Add(fav);
        }

        private static void HandleDownload(object sender, DownloadStringCompletedEventArgs e, RecentSong song)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(e.Result);
                string imageId = xdoc.Root.Element("{http://schemas.zune.net/catalog/music/2007/10}image")
                    .Element("{http://schemas.zune.net/catalog/music/2007/10}id").Value.Remove(0, 9);
                
                BitmapImage image = new BitmapImage(new Uri(string.Format("http://catalog.zune.net/v3.2/image/{0}?width=100&height=100", imageId)));
                song.AlbumArt = image;
            }
            catch
            {
                song.AlbumArt = new BitmapImage(new Uri("Images/unknown.png", UriKind.Relative));
            }

            ProfileViewModelLocator.ProfileModel.RecentPlaylist.Add(song);
        }

        private static void HandleArtistDownload(object sender, DownloadStringCompletedEventArgs e, Artist artist)
        {

            try
            {
                XDocument xdoc = XDocument.Parse(e.Result);
                string imageId = xdoc.Root.Element("{http://schemas.zune.net/catalog/music/2007/10}image")
                    .Element("{http://schemas.zune.net/catalog/music/2007/10}id").Value.Remove(0, 9);

                artist.Playcount = "Global plays: " + xdoc.Root.Element("{http://schemas.zune.net/catalog/music/2007/10}playCount").Value;
                artist.HasRadio = Convert.ToBoolean(xdoc.Root.Element("{http://schemas.zune.net/catalog/music/2007/10}hasRadioChannel").Value);
                artist.Genre = xdoc.Root.Element("{http://schemas.zune.net/catalog/music/2007/10}primaryGenre")
                    .Element("{http://schemas.zune.net/catalog/music/2007/10}title").Value;

                BitmapImage image = new BitmapImage(new Uri(string.Format("http://catalog.zune.net/v3.2/image/{0}?width=100&height=100", imageId)));
                artist.ArtistImage = image;
            }
            catch
            {
                artist.ArtistImage = new BitmapImage(new Uri("Images/unknown.png", UriKind.Relative));
            }

            ProfileViewModelLocator.ProfileModel.MostPlayed.Add(artist);
        }
    }
}
