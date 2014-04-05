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
        #region Private Properties
        public static bool isFirstTime=true;
        #endregion

        #region Constructor
        public StartScreen()
        {
            InitializeComponent();
        }
        #endregion

        #region Navigation Methods
        private void openSettingsScreen() {
            NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }

        private void OpenDriverHistory() {
            NavigationService.Navigate(new Uri("/Screens/DriverHistoryScreen.xaml", UriKind.Relative));
        }

        private void showFirstTimeConfiguration() {
            NavigationService.Navigate(new Uri("/Screens/FirstTimeScreen.xaml", UriKind.Relative));
        }

        private void startDriving()
        {
            NavigationService.Navigate(new Uri("/Screens/DriveScreen.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (e.NavigationMode == NavigationMode.Back)
            {
                if (Settings.CurrentDriverSetting == null){
                    MessageBox.Show("Failed creating a driver, Login / Sign Up again");
                    showFirstTimeConfiguration();
                }
            }
        }
        #endregion

        #region Button Behaviors
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
        #endregion

        #region Overriding Functions 
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.CurrentDriverSetting == null)
                showFirstTimeConfiguration();
        }
        #endregion
    }
}