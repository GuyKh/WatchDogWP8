using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace SafeyTake1.Screens
{
    public partial class DriveScreen : PhoneApplicationPage
    {
        private double _safetyLevel = 0;


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

        private double getCurrentSafetyLevel()
        {
            return _safetyLevel;
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

        private void updateSafetyMeter(double value)
        {
            // Update the safety meter 
            this.Dispatcher.BeginInvoke(delegate()
            {
                safetyMeter.Value = value;
                _safetyLevel = value;

                // Update the trail color
                if (value < 33)
                    safetyMeter.TrailBrush = new SolidColorBrush(Colors.Green);
                else if (value < 66)
                    safetyMeter.TrailBrush = new SolidColorBrush(Colors.Yellow);
                else 
                    safetyMeter.TrailBrush = new SolidColorBrush(Colors.Red);
            });
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