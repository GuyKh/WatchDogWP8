using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SafeyTake1.Screens
{
    public partial class CalibrationScreen : PhoneApplicationPage
    {
        public CalibrationScreen()
        {
            InitializeComponent();
        }

        private void setGPSToggle(bool ticked)
        {
            checkGPS.IsChecked = ticked;
        }

        private void setInternetToggle(bool ticked)
        {
            checkInternet.IsChecked = ticked;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }
    
        
    }
}