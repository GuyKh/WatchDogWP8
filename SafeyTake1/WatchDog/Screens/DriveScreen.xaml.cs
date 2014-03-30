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
        private const string ALARM_SOUND_FILENAME = "alarm.wav";

        /// <summary>
        /// How long will an alarm take (+- FLICKERING_WAIT duration)
        /// </summary>
        private const int ALERT_DURATION = 500;

        #endregion

        #region Private Properties

        private double _safetyLevel = 0;
        private Drive _currentDrive; 
        private DriveLogic _driveLogic;
        private Driver _currentDriver;
        #endregion

        #region Constructor
        public DriveScreen()
        {
            _currentDriver = new Driver("Name", "User", "Pass");
            _driveLogic = new DriveLogic(_currentDriver);
            InitializeComponent();

            }

        #endregion

       

        private void StartDrive()
        {
            pumpARGBFrames = true;
            ARGBFramesThread = new System.Threading.Thread(PumpARGBFrames);

            wb = new WriteableBitmap((int)cam.PreviewResolution.Width, (int)cam.PreviewResolution.Height);
            // Start pump.
            ARGBFramesThread.Start();
        }

        private void EndDrive()
        {
            pumpARGBFrames = false;
            _currentDrive = _driveLogic._currentDrive;
            _currentDrive.EndTime = DateTime.Now;

            PhoneApplicationService.Current.State["CurrentDrive"] = _currentDrive;
            NavigationService.Navigate(new Uri("/Screens/DriveSummaryScreen.xaml", UriKind.Relative));
        }

      

        

      

      
        #region UI Special Behaviors

        /// <summary>
        /// Perform updates to the screen: Update the UI and alert, if needed.
        /// </summary>
        /// <param name="level">Safety Meter score</param>
        /// <param name="args"></param>
        private void UpdateScreen(double score, string message)
        {

            _safetyLevel = score;

            this.Dispatcher.BeginInvoke(delegate()
            {
                dangerDescription.Text = message;
            });

            UpdateSafetyMeter(score);

            if (score >= DriveLogic.ALERT_THRESHOLD)
                {
                    Alert(ALERT_DURATION);
                    
                }

        }

        /// <summary>
        /// Perform alert - Play sound and flicker the background
        /// </summary>
        /// <param name="duration">Duration of the flickering</param>
        private void Alert(int duration)
        {
            this.Dispatcher.BeginInvoke(playSound);

            //this.Dispatcher.BeginInvoke(() => startflickeringBackground(FLICKERING_WAIT));
            //Thread.Sleep(duration);
            //stopFlickeringBackground();


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
        [ThreadStatic]
        private static bool flickering;
        

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
            Stream stream = TitleContainer.OpenStream(@"Sounds/"+ALARM_SOUND_FILENAME);
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
            // Check to see if the camera is available on the phone.
            if (PhotoCamera.IsCameraTypeSupported(CameraType.FrontFacing) == true)
            {
                // Initialize the default camera.
                cam = new Microsoft.Devices.PhotoCamera(CameraType.FrontFacing);
                
                cam.Initialized += CamOnInitialized;

                viewfinderCanvas.Visibility = Visibility.Collapsed;
                //Set the VideoBrush source to the camera
                viewfinderBrush.SetSource(cam);

            }
        }

        private void CamOnInitialized(object sender, CameraOperationCompletedEventArgs cameraOperationCompletedEventArgs)
        {
            Deployment.Current.Dispatcher.BeginInvoke(StartDrive);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (cam != null)
            {
                // Dispose camera to minimize power consumption and to expedite shutdown.
                cam.Dispose();


                // Release memory, ensure garbage collection.
                cam.Initialized -= CamOnInitialized;
            }

            

            // Text is param, you can define anything instead of Text 
            // but remember you need to further use same param.
            PhoneApplicationService.Current.State["CurrentDrive"] = _currentDrive;
        }

        #endregion


        #region Camera
        // Variables
        PhotoCamera cam = new PhotoCamera();
        private static ManualResetEvent pauseFramesEvent = new ManualResetEvent(true);
        private WriteableBitmap wb;
        private Thread ARGBFramesThread;
        private bool pumpARGBFrames;


        // ARGB frame pump
        void PumpARGBFrames()
        {
            // Create capture buffer.
            int[] ARGBPx = new int[(int)cam.PreviewResolution.Width * (int)cam.PreviewResolution.Height];

            try
            {
                PhotoCamera phCam = (PhotoCamera)cam;

                while (pumpARGBFrames)
                {
                    pauseFramesEvent.WaitOne();

                    // Copies the current viewfinder frame into a buffer for further manipulation.
                    phCam.GetPreviewBufferArgb32(ARGBPx);


                    pauseFramesEvent.Reset();
                   
                    // Copy to WriteableBitmap.
                    ARGBPx.CopyTo(wb.Pixels, 0);

                    double score = _driveLogic.AnalyzeFrontPicture(wb);

                    UpdateScreen(score, _driveLogic.AlertMessage);
                    pauseFramesEvent.Set();
                    
                }

            }
            catch (Exception)
            {

            }
        }

        #endregion


        #region Helpers



        #endregion
    }
}