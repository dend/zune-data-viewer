using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Threading;
using Coding4Fun.ZuneDataViewer.Helpers;
using Coding4Fun.ZuneDataViewer.Models;

namespace Coding4Fun.ZuneDataViewer
{
    public partial class GUIDPage : PhoneApplicationPage
    {
        private bool isNew = true;

        public GUIDPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(GUIDPage_Loaded);
        }

        void GUIDPage_Loaded(object sender, RoutedEventArgs e)
        {
            if ((from c in IsolatedStorageSettings.ApplicationSettings where c.Key == "GUID" select c).Count() != 0)
            {
                isNew = false;
                CheckGuid();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            CheckGuid();
        }

        void CheckGuid()
        {
            Log.Text += "\nTrying to acquire user information...";
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);

            Log.Text += "\nTrying to download...";

            string currentGUID;

            if (isNew)
                currentGUID = txtGuid.Text;
            else
                currentGUID = IsolatedStorageSettings.ApplicationSettings["GUID"].ToString();

            client.DownloadStringAsync(new Uri("http://socialapi.zune.net/en-US/members/" + currentGUID));
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                Log.Text += "\nAttempting to read the result...";
                Log.Text += "\n" + e.Result;
                StartProcess(e.Result);               
            }
            catch (WebException ex)
            {
                isNew = true;
                Log.Text += "\nFAILED! Sorry, seems like there is no such user.";
                Log.Text += "\n" + ex.InnerException;
                Log.Text += "\n" + ex.StackTrace;
            }

        }

        private void StoreGuid()
        {
            Log.Text += "\nSaving...";
            IsolatedStorageSettings.ApplicationSettings.Add("GUID", txtGuid.Text);
            IsolatedStorageSettings.ApplicationSettings.Save();
            Log.Text += "\nGoing home now...";
        }

        private void GoHome()
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void StartProcess(string profileData)
        {
            if (isNew)
            {
                StoreGuid();
            }

            Parsers.ParseProfileInfo(profileData);
            Downloader d = new Downloader();
            d.Download(DownloadType.BackgroundPicture);
            d.Download(DownloadType.ProfilePicture);
             
            GoHome();
        }
    }
}