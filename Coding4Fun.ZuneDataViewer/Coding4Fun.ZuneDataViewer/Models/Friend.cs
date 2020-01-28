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
    public class Friend
    {
        public string Name { get; set; }
        public string Playcount { get; set; }
        public BitmapImage ProfileImage { get; set; }
        public BitmapImage BackgroundImage { get; set; }
    }
}
