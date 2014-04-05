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
using WatchDOG.Helpers;

namespace WatchDOG.Screens
{
    public partial class SettingsScreen : PhoneApplicationPage
    {
        public SettingsScreen()
        {
            InitializeComponent();
            if (Settings.CurrentDriverSetting != null)
            {
                txtDriversName.Text = Settings.CurrentDriverSetting.Name;
                PopulateSettings();
            }
            else WatchDogHelper.ShowToastMessage("Error", "No driver loaded, please restart the application");

            PopulateSettings();
        }


        /// <summary>
        /// Populate Settings to UI controls according to the Settings class
        /// </summary>
        private void PopulateSettings()
        {
            if (Settings.UnitsSetting == Units.Metric)
            {
                radioMetric.IsChecked = true;
                radioMiles.IsChecked = false;
            }
            else
            {

                radioMetric.IsChecked = false;
                radioMiles.IsChecked = true;
            }

            checkGps.IsChecked = Settings.IsGpsEnabledSettings;
        }

        private void btnCalibrate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/CalibrationScreen.xaml", UriKind.Relative));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (radioMetric.IsChecked.HasValue && radioMetric.IsChecked == true)
                Settings.UnitsSetting = Units.Metric;
            else if (radioMiles.IsChecked.HasValue && radioMiles.IsChecked == true)
                Settings.UnitsSetting = Units.Imperial;

            if (checkGps.IsChecked.HasValue && checkGps.IsChecked == true)
                Settings.IsGpsEnabledSettings = true;
            else Settings.IsGpsEnabledSettings = false;

            Settings.Save();

            if (this.NavigationService.CanGoBack)
                // Try going to the previous screen first
                this.NavigationService.GoBack();
            else
                NavigationService.Navigate(new Uri("/Screens/StartScreen.xaml", UriKind.Relative));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                // Try going to the previous screen first
                this.NavigationService.GoBack();
            else
                NavigationService.Navigate(new Uri("/Screens/StartScreen.xaml", UriKind.Relative));
            
        }
    }
}