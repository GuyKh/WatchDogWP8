using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WatchDOG.Screens
{
    public partial class SettingsScreen : PhoneApplicationPage
    {
        public SettingsScreen()
        {
            InitializeComponent();
        }

        private void btnCalibrate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/CalibrationScreen.xaml", UriKind.Relative));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
                NavigationService.Navigate(new Uri("/Screens/StartScreen.xaml", UriKind.Relative));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                // Try going to the previous screen first
                this.NavigationService.GoBack();
            
        }
    }
}