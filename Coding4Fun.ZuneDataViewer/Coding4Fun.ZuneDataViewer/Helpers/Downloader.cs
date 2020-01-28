using System;
using System.Net;
using Coding4Fun.ZuneDataViewer.Helpers;
using Coding4Fun.ZuneDataViewer.Models;
using System.Windows.Media.Imaging;

namespace Coding4Fun.ZuneDataViewer.Helpers
{
    public class Downloader
    {
        public void Download(DownloadType type)
        {           
            switch (type)
            {
                case DownloadType.ProfilePicture:
                    {
                        Profile.ProfileImage = new BitmapImage(new Uri(Profile.ProfileImageURL,UriKind.Absolute));
                        break;
                    }
                case DownloadType.BackgroundPicture:
                    {
                        Profile.BackgroundImage = new BitmapImage(new Uri(Profile.BackgroundImageURL, UriKind.Absolute));
                        break;
                    }
                case DownloadType.RecentPlaylist:
                   {
                       WebClient client = new WebClient();
                       client.DownloadStringCompleted += (s,e) => HandleCompletedDownload(s,e,type);
                       client.DownloadStringAsync(new Uri(Profile.RecentPlaylistURL));
                       break;
                   }
                case DownloadType.MostPlayedPlaylist:
                   {
                       WebClient client = new WebClient();
                       client.DownloadStringCompleted += (s, e) => HandleCompletedDownload(s, e, type);
                       client.DownloadStringAsync(new Uri(Profile.MostPlayedPlaylistURL));
                       break;
                   }
                case DownloadType.FavoritePlaylist:
                   {
                       WebClient client = new WebClient();
                       client.DownloadStringCompleted += (s, e) => HandleCompletedDownload(s, e, type);
                       client.DownloadStringAsync(new Uri(Profile.FavortiesPlaylistURL));
                       break;
                   }
                case DownloadType.BadgeList:
                   {
                       WebClient client = new WebClient();
                       client.DownloadStringCompleted += (s, e) => HandleCompletedDownload(s, e, type);
                       client.DownloadStringAsync(new Uri(Profile.BadgeUrl));
                       break;
                   }
                case DownloadType.FriendList:
                   {
                       WebClient client = new WebClient();
                       client.DownloadStringCompleted += (s, e) => HandleCompletedDownload(s, e, type);
                       client.DownloadStringAsync(new Uri(Profile.FriendsUrl));
                       break;
                   }
            }
        }

        void HandleCompletedDownload(object sender, DownloadStringCompletedEventArgs e, DownloadType type)
        {
            switch (type)
            {
                case DownloadType.RecentPlaylist:
                    {
                        Parsers.ParseRecentPlaylist(e.Result);
                        break;
                    }
                case DownloadType.MostPlayedPlaylist:
                    {
                        Parsers.ParseMostPlayed(e.Result);
                        break;
                    }
                case DownloadType.FavoritePlaylist:
                    {
                        Parsers.ParseFavorites(e.Result);
                        break;
                    }
                case DownloadType.BadgeList:
                    {
                        Parsers.ParseBadges(e.Result);
                        break;
                    }
                case DownloadType.FriendList:
                    {
                        Parsers.ParseFriends(e.Result);
                        break;
                    }
            }
        }
    }
}
