using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WatchDOG.DataStructures;

namespace WatchDOG.Screens
{
    public partial class StartScreen : PhoneApplicationPage
    {
        // New Comment.
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

        private void openSettingsScreen() {
            NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }

        private void OpenDriverHistory() {
            NavigationService.Navigate(new Uri("/Screens/DriverHistoryScreen.xaml", UriKind.Relative));
        }

        private void showFirstTimeConfiguration() {
            NavigationService.Navigate(new Uri("/Screens/FirstTimeScreen.xaml", UriKind.Relative));
        }

        private void startDriving() {
            NavigationService.Navigate(new Uri("/Screens/DriveScreen.xaml", UriKind.Relative));
        }

        private void testFeatures() { }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            openSettingsScreen();
        }

        private void btnStartDriving_Click(object sender, RoutedEventArgs e)
        {
            startDriving();
        }

        private void btnMyHistory_Click(object sender, RoutedEventArgs e)
        {
            OpenDriverHistory();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

            if (isFirstTime)
                showFirstTimeConfiguration();
        }
    }
}