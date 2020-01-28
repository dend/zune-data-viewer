using GalaSoft.MvvmLight;
using Coding4Fun.ZuneDataViewer.Models;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Coding4Fun.ZuneDataViewer.Helpers;
using System;

namespace Coding4Fun.ZuneDataViewer.ViewModel
{
    public class ProfileViewModel : ViewModelBase
    {
        private ObservableCollection<Badge> badges;
        private ObservableCollection<RecentSong> recentPlaylist;
        private ObservableCollection<Artist> mostPlayed;
        private ObservableCollection<Favorite> favs;
        private ObservableCollection<Friend> friends;

        public ProfileViewModel()
        {
            RecentPlaylist = new ObservableCollection<RecentSong>();
            mostPlayed = new ObservableCollection<Artist>();
            favs = new ObservableCollection<Favorite>();
            badges = new ObservableCollection<Badge>();
        }

        public string ZuneName
        {
            get
            {
                return Profile.ZuneName;
            }
        }

        public string RealName
        {
            get
            {
                return Profile.RealName;
            }
        }

        public string Location
        {
            get
            {
                return Profile.Location;
            }
        }

        public string PlayCount
        {
            get
            {
                return Profile.PlayCount + " plays";
            }
        }

        public string LastUpdate
        {
            get
            {
                return Profile.LastUpdate;
            }
        }

        public BitmapImage ProfileImage
        {
            get
            {
                return Profile.ProfileImage;
            }
        }

        public BitmapImage BackgroundImage
        {
            get
            {
               return Profile.BackgroundImage;
            }
        }

        public ObservableCollection<RecentSong> RecentPlaylist
        {
            get
            {
                return recentPlaylist;
            }
            set
            {
                recentPlaylist = value;
                RaisePropertyChanged("RecentPlaylist");
            }
            
        }

        public ObservableCollection<Artist> MostPlayed
        {
            get
            {
                return mostPlayed;
            }
            set
            {
                mostPlayed = value;
                RaisePropertyChanged("MostPlayed");
            }

        }

        public ObservableCollection<Favorite> Favorites
        {
            get
            {
                return favs;
            }
            set
            {
                favs = value;
                RaisePropertyChanged("Favorites");
            }
        }

        public ObservableCollection<Badge> Badges
        {
            get
            {
                return badges;
            }
            set
            {
                badges = value;
                RaisePropertyChanged("Badges");
            }
        }

        public ObservableCollection<Friend> Friends
        {
            get
            {
                return friends;
            }
            set
            {
                friends = value;
                RaisePropertyChanged("Friends");
            }
        }

        public string Status
        {
            get
            {
                return Profile.Status;
            }
        }

        public string Bio
        {
            get
            {
                return Profile.Bio;
            }
        }
    }
}