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
    public partial class DriveScreen : PhoneApplicationPage
    {
        public DriveScreen()
        {
            InitializeComponent();
        }

        private void endDrive()
        {

        }

        private void flickerBackground()
        {

        }

        private float getCurrentSafetyLevel()
        {
            return 0;
        }

        private void openSettings()
        {

        }

        private void playSound()
        {

        }

        private void sendLocationToWeb()
        {

        }

        private void showPopupEndDrive()
        {

        }

        private void updateSafetyMeter()
        {

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/DriveSummaryScreen.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Logic.DriveLogic.StartLoop();
        }
    }
    //Omer commit
}