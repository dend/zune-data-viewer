using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using Coding4Fun.ZuneDataViewer.Models;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using Coding4Fun.ZuneDataViewer.Helpers;
using System.Windows.Data;
using Coding4Fun.ZuneDataViewer.ViewModel;
using System.Diagnostics;


namespace Coding4Fun.ZuneDataViewer
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void mainPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (mainPanel.SelectedIndex)
            {
                case 1:
                    {
                        if ((ProfileViewModelLocator.ProfileModel.Badges == null) ||
                            (ProfileViewModelLocator.ProfileModel.Badges.Count == 0))
                        {
                            ProfileViewModelLocator.ProfileModel.Badges = new ObservableCollection<Badge>();
                            Downloader d = new Downloader();
                            d.Download(DownloadType.BadgeList);
                        }
                        break;
                    }
                case 2:
                    {
                        if ((ProfileViewModelLocator.ProfileModel.RecentPlaylist == null) ||
                            (ProfileViewModelLocator.ProfileModel.RecentPlaylist.Count == 0))
                        {
                            ProfileViewModelLocator.ProfileModel.RecentPlaylist = new ObservableCollection<RecentSong>();
                            Downloader d = new Downloader();
                            d.Download(DownloadType.RecentPlaylist);
                        }
                        break;
                    }
                case 3:
                    {
                        if ((ProfileViewModelLocator.ProfileModel.MostPlayed == null) || 
                            (ProfileViewModelLocator.ProfileModel.MostPlayed.Count == 0))
                        {
                            ProfileViewModelLocator.ProfileModel.MostPlayed = new ObservableCollection<Artist>();
                            Downloader d = new Downloader();
                            d.Download(DownloadType.MostPlayedPlaylist);
                        }
                        break;
                    }
                case 4:
                    {
                        if ((ProfileViewModelLocator.ProfileModel.Favorites == null) ||
                            (ProfileViewModelLocator.ProfileModel.Favorites.Count == 0))
                        {
                            ProfileViewModelLocator.ProfileModel.Favorites = new ObservableCollection<Favorite>();
                            Downloader d = new Downloader();
                            d.Download(DownloadType.FavoritePlaylist);
                        }
                        break;
                    }
                case 5:
                    {
                        if ((ProfileViewModelLocator.ProfileModel.Friends == null) ||
                            (ProfileViewModelLocator.ProfileModel.Friends.Count == 0))
                        {
                            ProfileViewModelLocator.ProfileModel.Friends = new ObservableCollection<Friend>();
                            Downloader d = new Downloader();
                            d.Download(DownloadType.FriendList);
                        }
                        break;
                    } 
            }
        }
    }
}