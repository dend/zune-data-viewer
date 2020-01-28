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
using System.Windows.Media.Imaging;

namespace Coding4Fun.ZuneDataViewer.Models
{
    public class Favorite
    {
        public string Title { get; set; }
        public string Length { get; set; }
        public bool IsExplicit { get; set; }
        public string ArtistName { get; set; }
        public BitmapImage ArtistImage { get; set; }
        public string AlbumName { get; set; }
        public BitmapImage AlbumImage { get; set; }
        public string ArtistID { get; set; }
        public string AlbumID { get; set; }
    }
}
