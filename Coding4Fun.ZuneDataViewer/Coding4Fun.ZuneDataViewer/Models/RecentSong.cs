using System;
using System.Windows.Media.Imaging;

namespace Coding4Fun.ZuneDataViewer.Models
{
    public class RecentSong
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string AlbumID { get; set; }
        public BitmapImage AlbumArt { get; set; }
    }
}
