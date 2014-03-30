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
    public partial class DriverHistoryScreen : PhoneApplicationPage
    {
        public DriverHistoryScreen()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                // Try going to the previous screen first
                this.NavigationService.GoBack();
            else
                NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }
    }
}