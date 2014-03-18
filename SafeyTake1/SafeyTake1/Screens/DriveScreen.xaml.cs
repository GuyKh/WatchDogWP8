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
using WatchDOG.Alerters;
using System.ComponentModel;
using Microsoft.Devices;
using System.Windows.Media.Imaging;
using Coding4Fun.Toolkit.Controls;
using WatchDOG.DataStructures;

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

        /// <summary>
        /// Threshold from whatlevel up to alarm.
        /// </summary>
        private const double ALERT_THRESHOLD = 50;

        #endregion


        #region Private Properties

        private double _safetyLevel = 0;
        private DateTime _driveBeginingTime;
        private DateTime _driveEndingTime;
        private List<FrontCameraAlerterAbstract> frontAlerters;
        private PhotoCamera frontCam;
        private Drive _currentDrive;
        
        #endregion

        #region Constructor
        public DriveScreen()
        {
            InitializeComponent();

            initCameras();
            frontAlerters = new List<FrontCameraAlerterAbstract>();
            frontAlerters.Add(new EyeDetectorAlerter());
            }

        #endregion

        #region Initialization
        /// <summary>
        /// Initilialize Available Cameras
        /// </summary>
        private void initCameras()
        {
            frontCam = new Microsoft.Devices.PhotoCamera(CameraType.FrontFacing);
            if (frontCam == null)
            {
                ShowToastMessage("Error Starting Front Camera", "Front Camera could not be initialized"); 
                //MessageBox.Show("ERROR: Front Camera Not Available", "Error", MessageBoxButton.OK);
            }

            frontCam.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(frontCam_Initialized);
            frontCam.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(frontCam_ThumbnailAvailable);
            
        }

        private void startDrive()
        {
            _currentDrive = new Drive(new Driver("Name", "User", "Pass"), DateTime.Now);
            _driveBeginingTime = DateTime.Now;
        }

        private void endDrive()
        {
            _currentDrive.EndTime = DateTime.Now;
            _driveEndingTime = DateTime.Now;
            NavigationService.Navigate(new Uri("/Screens/DriveSummaryScreen.xaml", UriKind.Relative));
        }

        #endregion

        #region Front Camera Event Handlers
        private void frontCam_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            if (!e.Succeeded)
            {
                frontCam = null;
                ShowToastMessage("Error Starting Front Camera", "Front Camera could not be initialized"); 
                //MessageBox.Show("ERROR: Camera is not available", "ERROR", MessageBoxButton.OK);
                return;
            }

            try
            {
                frontCam = (PhotoCamera)sender;
                frontCam.Resolution = frontCam.AvailableResolutions.First();
            }
            catch (Exception)
            {
                frontCam = null;
                ShowToastMessage("Front Camera Error", "Front Camera is not avaliable"); 
               // MessageBox.Show("ERROR: Camera is not available", "ERROR", MessageBoxButton.OK);
                return;
            }
        }
        
        
        private void frontCam_ThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {

            this.Dispatcher.BeginInvoke(delegate()
            {
                if (frontCam == null)
                {
                    //MessageBox.Show("ERROR: Camera is not available", "ERROR", MessageBoxButton.OK);
                    ShowToastMessage("Error Starting Front Camera", "Front Camera could not be initialized"); 
                    return;
                }
                
                // Initialize a new Bitmap image
                WriteableBitmap bitmap = new WriteableBitmap((int)frontCam.PreviewResolution.Width,
                                                             (int)frontCam.PreviewResolution.Height);
                
                // Get the bitmap from the camera
                frontCam.GetPreviewBufferArgb32(bitmap.Pixels);

                // Foreach Alerter, try to analize the value.
                List<double> alerterValues = new List<Double>();
                foreach (IAlerter alerter in frontAlerters)
                {
                    alerter.GetData();
                    
                    double alertSafetyLevel = alerter.ProcessData(bitmap);



                    if (alertSafetyLevel >= ALERT_THRESHOLD)
                        _currentDrive.Events.Add(new AlertEvent(){ 
                            AlertLevel = alertSafetyLevel, AlertTime=DateTime.Now, AlertType=alerter.GetAlerterType(), Driver = _currentDrive.Driver
                        });
                

                    alerterValues.Add(alertSafetyLevel);
                    
                }

                // Calculate the safety score (for all valid values).
                double res = calculateSafetyScore(alerterValues.Where(w => w != -1));

                // Update the UI
                updateScreen(res);
            });
        }

        private double calculateSafetyScore(IEnumerable<double> alerterValues)
        {
            double ret = 0;
            foreach (var value in alerterValues)
            {
                ret += value / alerterValues.Count();
            }
            return ret;
        }

        #endregion
        
        #region UI Special Behaviors
        /// <summary>
        /// Perform updates to the screen: Update the UI and alert, if needed.
        /// </summary>
        /// <param name="level">Safety Meter score</param>
        private void updateScreen(double level)
        {
            _safetyLevel = level;

            this.Dispatcher.BeginInvoke(delegate()
            {
                updateSafetyMeter((double)level);

                if (level >= ALERT_THRESHOLD)
                    alert(ALERT_DURATION);
            });

        }

        /// <summary>
        /// Perform alert - Play sound and flicker the background
        /// </summary>
        /// <param name="duration">Duration of the flickering</param>
        private void alert(int duration)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                playSound();
            });

            this.Dispatcher.BeginInvoke(delegate()
            {
                startflickeringBackground(FLICKERING_WAIT);
            });
            Thread.Sleep(duration);
            stopFlickeringBackground();


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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Logic.DriveLogic.StartLoop();
        }
        #endregion

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
                endDrive();
            }

        }


  
        #endregion

        #region Navigation Event Handlers Override
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            initCameras();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Dispose camera
            if (frontCam != null)
            {
                // Dispose camera to minimize power consumption and to expedite shutdown.
                frontCam.Dispose();

                // Release memory, ensure garbage collection.
                frontCam.Initialized -= frontCam_Initialized;
                frontCam.CaptureThumbnailAvailable -= frontCam_ThumbnailAvailable;
            }

            // Text is param, you can define anything instead of Text 
            // but remember you need to further use same param.
            PhoneApplicationService.Current.State["CurrentDrive"] = _currentDrive;
        }

        #endregion

        #region Helpers

        private void ShowToastMessage(String title, String message)
        {
            ToastPrompt tp = new ToastPrompt();

            tp.Title = title;
            tp.Message = message;
            tp.ImageSource = new BitmapImage(new Uri("/Assets/AlignmentGrid.png", UriKind.Relative));
            tp.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            tp.TextWrapping = TextWrapping.Wrap;

            tp.Show();
        }

        #endregion
    }
}