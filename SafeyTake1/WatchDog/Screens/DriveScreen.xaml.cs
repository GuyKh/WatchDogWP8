using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
using WatchDOG.Alerters;
using System.ComponentModel;
using Microsoft.Devices;
using System.Windows.Media.Imaging;
using Coding4Fun.Toolkit.Controls;
using WatchDOG.DataStructures;
using WatchDOG.Logic;

namespace WatchDOG.Screens
{
    public partial class DriveScreen : PhoneApplicationPage
    {

        #region Constants

        /// <summary>
        /// How long to wait between changing background colors
        /// </summary>
        private const int FLICKERING_WAIT = 100;

        /// <summary>
        /// Which sound file to play on alarm
        /// </summary>
        private const string ALARM_SOUND = "Sounds\alarm.mp3";

        /// <summary>
        /// How long will an alarm take (+- FLICKERING_WAIT duration)
        /// </summary>
        private const int ALERT_DURATION = 500;

        #endregion

        #region Private Properties

        private double _safetyLevel = 0;
        private Drive _currentDrive; 
        private DriveLogic _driveLogic;
        #endregion

        #region Constructor
        public DriveScreen()
        {
            _driveLogic = new DriveLogic(new Driver("Name", "User", "Pass"));
            _driveLogic.Alert += new AlertEventHandler(UpdateScreen);
            _driveLogic.dispatcher = this.Dispatcher;
            InitializeComponent();

            }

        #endregion

       

        private void StartDrive()
        {
 
        }

        private void EndDrive()
        {
            _currentDrive = _driveLogic._currentDrive;
            _currentDrive.EndTime = DateTime.Now;
            NavigationService.Navigate(new Uri("/Screens/DriveSummaryScreen.xaml", UriKind.Relative));
        }

      

        

      

      
        #region UI Special Behaviors

        /// <summary>
        /// Perform updates to the screen: Update the UI and alert, if needed.
        /// </summary>
        /// <param name="level">Safety Meter score</param>
        /// <param name="args"></param>
        private void UpdateScreen(object sender, AlertEventHandlerArgs args)
        {

            _safetyLevel = args.Level;

            dangerDescription.Text = args.Message;

            Dispatcher.BeginInvoke(delegate()
            {
                UpdateSafetyMeter(args.Level);

                if (args.Level >= DriveLogic.ALERT_THRESHOLD)
                {
                    Alert(ALERT_DURATION);
                    
                }
            });

        }

        /// <summary>
        /// Perform alert - Play sound and flicker the background
        /// </summary>
        /// <param name="duration">Duration of the flickering</param>
        private void Alert(int duration)
        {
            this.Dispatcher.BeginInvoke(playSound);

            this.Dispatcher.BeginInvoke(() => startflickeringBackground(FLICKERING_WAIT));
            Thread.Sleep(duration);
            stopFlickeringBackground();


        }

        /// <summary>
        /// Update the Safety Gauge.
        /// </summary>
        /// <param name="value">Safety Level (0-100)</param>
        public void UpdateSafetyMeter(double value)
        {
            // Update the safety meter 
            this.Dispatcher.BeginInvoke(delegate()
            {
                safetyMeter.Value = value;

                // Update the trail color
                if (value < 33)
                    safetyMeter.TrailBrush = new SolidColorBrush(Colors.Green);
                else if (value < 66)
                    safetyMeter.TrailBrush = new SolidColorBrush(Colors.Yellow);
                else
                    safetyMeter.TrailBrush = new SolidColorBrush(Colors.Red);
            });
        }


        #region Flicker Region
        // Volatile (one for all threads) boolean indicating if now flickering.
        private volatile bool flickering;
        

        /// <summary>
        /// Start flickering - set flickering - true and call flicker()
        /// </summary>
        private void startflickeringBackground(int flickerInterval)
        {
            flickering = true;
            flicker(flickerInterval);
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
        private void flicker(int flickerInterval)
        {
            Brush prevBackground = pageGrid.Background;
            while (flickering)
            {
                pageGrid.Background = new SolidColorBrush(Colors.White);
                Thread.Sleep(flickerInterval);
                pageGrid.Background = prevBackground;
                Thread.Sleep(flickerInterval);
            }
        }
        #endregion

        #region Sound Playing Region
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
        #endregion

        #endregion
          
        private void sendLocationToWeb()
        {

        }     
        
        #region Button Behaviors
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            openSettings();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            showPopupEndDrive();
        }
        #endregion

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
        }
        

        #region Navigation
        /// <summary>
        /// Open Settings page
        /// </summary>
        private void openSettings()
        {
            NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
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
                EndDrive();
            }

        }


  
        #endregion

        #region Navigation Event Handlers Override
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Task init = new Task(_driveLogic.InitCameras);
            init.Start();
            init.Wait();

            _driveLogic.StartLoop();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _driveLogic.StopLoop();
            _driveLogic.DisposeCameras();

            

            // Text is param, you can define anything instead of Text 
            // but remember you need to further use same param.
            PhoneApplicationService.Current.State["CurrentDrive"] = _currentDrive;
        }

        #endregion

        #region Helpers



        #endregion
    }
}