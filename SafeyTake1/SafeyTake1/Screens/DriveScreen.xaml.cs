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
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace SafeyTake1.Screens
{
    public partial class DriveScreen : PhoneApplicationPage
    {
        private double _safetyLevel = 0;
        private const int FLICKERING_WAIT = 100;
        private const string ALARM_SOUND = "Sounds\alarm.mp3";

        public DriveScreen()
        {
            InitializeComponent();
        }

        private void endDrive()
        {
            NavigationService.Navigate(new Uri("/Screens/DriveSummaryScreen.xaml", UriKind.Relative));
        }

        // Volatile (one for all threads) boolean indicating if now flickering.
        private volatile bool flickering;

        /// <summary>
        /// Start flickering - set flickering - true and call flicker()
        /// </summary>
        private void startflickeringBackground()
        {
            flickering = true;
            flicker();
        }

        /// <summary>
        /// Stop flickering (set flickering to false)
        /// </summary>
        private void stopFlickeringBackground()
        {
            flickering = false;
        }

        /// <summary>
        /// Perform flickeing effect (to White) background, while "flickering" var is on.
        /// </summary>
        private void flicker()
        {
            Brush prevBackground = pageGrid.Background;
            while (flickering)
            {
                pageGrid.Background = new SolidColorBrush(Colors.White);
                Thread.Sleep(FLICKERING_WAIT);
                pageGrid.Background = prevBackground;
                Thread.Sleep(FLICKERING_WAIT);
            }
        }

        /// <summary>
        /// Return the current Safety level
        /// </summary>
        /// <returns>SafetyLevel (double)</returns>
        public double getCurrentSafetyLevel()
        {
            return _safetyLevel;
        }

        private void openSettings()
        {
            NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Play the alarm sound once.
        /// </summary>
        private void playSound()
        {
            Stream stream = TitleContainer.OpenStream(ALARM_SOUND);
            SoundEffect effect = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();
            effect.Play();

        }

        private void sendLocationToWeb()
        {

        }

        /// <summary>
        /// Show the End Drive popup, if user answers "Yes", it'll call endDrive method.
        /// </summary>
        private void showPopupEndDrive()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to end the drive?",
                "End Drive", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                endDrive();
            }

        }

        /// <summary>
        /// Update the Safety Gauge.
        /// </summary>
        /// <param name="value">Safety Level (0-100)</param>
        public void updateSafetyMeter(double value)
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
            openSettings();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            showPopupEndDrive();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Logic.DriveLogic.StartLoop();
        }
    }
}