using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SafeyTake1.DataStructures;

namespace SafeyTake1.Screens
{
    public partial class StartScreen : PhoneApplicationPage
    {

        private static Settings _mySettings;
        public static bool isFirstTime=true;

        public static Settings MySettings
        {
            get { return _mySettings; }
            set { _mySettings = value; }
        }

        public StartScreen()
        {
            InitializeComponent();
        }

        public static void exitApp() { }

        private void openSettingsScreen() { }

        private void showDriverHistory() { }

        private void showFirstTimeConfiguration() { }

        private void startDriving() { }

        private void testFeatures() { }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }

        private void btnStartDriving_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/DriveScreen.xaml", UriKind.Relative));
        }

        private void btnMyHistory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/DriverHistoryScreen.xaml", UriKind.Relative));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (isFirstTime)
                NavigationService.Navigate(new Uri("/Screens/FirstTimeScreen.xaml", UriKind.Relative));
        }
    }
}