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
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Coding4Fun.ZuneDataViewer.Models
{
    public static class Profile
    {
        // Info
        public static string ZuneName { get; set; }
        public static string RealName { get; set; }
        public static string Location { get; set; }
        public static string PlayCount { get; set; }
        public static string LastUpdate { get; set; }
        public static string Status { get; set; }
        public static string Bio { get; set; }

        // Image URLs
        public static string ProfileImageURL { get; set; }
        public static string BackgroundImageURL { get; set; }

        // Playlist URLs
        public static string RecentPlaylistURL { get; set; }
        public static string MostPlayedPlaylistURL { get; set; }
        public static string FavortiesPlaylistURL { get; set; }

        // Images
        public static BitmapImage ProfileImage { get; set; }
        public static BitmapImage BackgroundImage { get; set; }

        // Misc
        public static string BadgeUrl { get; set; }
        public static string FriendsUrl { get; set; }
    }
}
