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
    public class Artist
    {
        public string ArtistID { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Playcount { get; set; }
        public bool HasRadio { get; set; }
        public BitmapImage ArtistImage { get; set; }
    }
}
