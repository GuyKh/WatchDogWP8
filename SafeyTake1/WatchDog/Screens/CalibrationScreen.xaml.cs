using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Media;
using FaceDetectionWinPhone;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using WatchDOG.Helpers;
using System.Device.Location;
using Windows.Devices.Geolocation;
using WatchDOG.Alerters;


namespace WatchDOG.Screens
{
    public partial class CalibrationScreen : PhoneApplicationPage
    {
        #region Private Properties
        private PhotoCamera cam;
        private MediaLibrary library = new MediaLibrary();
        private Detector detector;
        private DateTime frameStart;
        private Boolean GPSEnabled = true;
        #endregion

        #region Constructor
        public CalibrationScreen()
        {
            InitializeComponent();
            detector = Detector.Create(EyeDetectorAlerter.MODEL_XML);
            GPSEnabled = isGPSEnabled();
            setGPSToggle(GPSEnabled);
        }
        #endregion


        private void setGPSToggle(bool ticked)
        {
            checkGPS.IsChecked = ticked;
        }

        private Boolean isGPSEnabled()
        {
            Geolocator geolocator = new Geolocator();
            return !(geolocator.LocationStatus == PositionStatus.Disabled);
        }

        #region Navigation Methods
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                // Try going to the previous screen first
                this.NavigationService.GoBack();
            else
                NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            cam = new Microsoft.Devices.PhotoCamera(CameraType.FrontFacing);
            if (cam == null)
            {
                btnPassText.Text = "Failed";
                btnPass.Foreground = new SolidColorBrush(Colors.Red);
                MessageBox.Show("ERROR: Front Camera Not Available", "Error", MessageBoxButton.OK);

            }
            cam.Initialized += new EventHandler<Microsoft.Devices.CameraOperationCompletedEventArgs>(cam_Initialized);
            viewfinderCanvas.Width = 400*(1.3333);
            
            overlayCanvas.Width = viewfinderCanvas.Width;
            overlayCanvas.Height = viewfinderCanvas.Height;

            viewfinderBrush.SetSource(cam);
        }


        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (cam != null)
            {
                // Dispose camera to minimize power consumption and to expedite shutdown.
                cam.Dispose();

                // Release memory, ensure garbage collection.
                cam.Initialized -= cam_Initialized;
            }
        }
        #endregion

        #region Camera Methods
        private void cam_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            if (!e.Succeeded)
            {
                cam = null;
                return;
            }

            try
            {
                cam = (PhotoCamera)sender;
                cam.Resolution = cam.AvailableResolutions.First();
            }
            catch (Exception)
            {
                cam = null;
                return;
            }

            this.Dispatcher.BeginInvoke(delegate()
            {
                if (cam == null)
                    return;

                WriteableBitmap bitmap = new WriteableBitmap((int)cam.PreviewResolution.Width,
                                                             (int)cam.PreviewResolution.Height);
                frameStart = DateTime.Now;
                cam.GetPreviewBufferArgb32(bitmap.Pixels);

                bitmap = WatchDogHelper.rotateImage(bitmap, 270);
                detectFaces(bitmap);
            });
        }
        
        private void detectFaces(WriteableBitmap bitmap)
        {
            // The user sees a transposed image in the viewfinder, transpose the image for face detection as well.
            WriteableBitmap detectorBitmap = (new WriteableBitmap(bitmap));
            var thread = new System.Threading.Thread(delegate()
            {
                List<NativeFaceDetector.Rectangle> rectangles = detector.getFaces(bitmap,
                    EyeDetectorAlerter.BASE_SCALE, EyeDetectorAlerter.SCALE_INC,
                    EyeDetectorAlerter.INCREMENT, EyeDetectorAlerter.MIN_NEIGHBORS);


                // Print Text
                this.Dispatcher.BeginInvoke(delegate()
                {
                    double milliseconds = (DateTime.Now - frameStart).TotalMilliseconds;
                    // Cannot capture an image until the previous capture has completed.

                    /* Dani comment - txtDebug made error prevented running the app.
                    txtDebug.Text = (rectangles != null) ? "No. of Eyes: " + rectangles.Count + ". It took me: " + milliseconds + " ms." : "Null Eyes";
                    */
                    frameStart = DateTime.Now;
                });

                this.Dispatcher.BeginInvoke(delegate()
                {
                    overlayCanvas.Children.Clear();
                    foreach (var r in rectangles)
                    {
                        Rectangle toAdd = new Rectangle();
                        TranslateTransform loc = new TranslateTransform();
                        loc.X = r.x() + 40 > 0 ? r.x() + 40 : r.x();
                        loc.Y = r.y() - 50 > 0 ? r.y() - 50 : r.y();
                        toAdd.RenderTransform = loc;
                        toAdd.Width = r.width();
                        toAdd.Height = r.height();
                        toAdd.Stroke = new SolidColorBrush(Colors.Red);
                        toAdd.StrokeThickness = 5;
                        overlayCanvas.Children.Add(toAdd);
                    }

                    bool eyesDistanceOK = false;
                    for (int i = 0; i < rectangles.Count; i++)
                    {
                        for (int j = i + 1; j < rectangles.Count; j++)
                        {
                            if (Math.Abs(rectangles[i].y() - rectangles[j].y()) < 100)
                                eyesDistanceOK = true;
                        }
                    }

                    if (rectangles.Count >= 2 && eyesDistanceOK)
                    {

                        WatchDogHelper.ShowToastMessage("Success", "Two Eyes were detedted");
                        btnPassText.Text = "Pass";
                        btnPass.Foreground = new SolidColorBrush(Colors.White);
                        btnPass.Background = new SolidColorBrush(Colors.Green);
                    }
                });


                this.Dispatcher.BeginInvoke(delegate()
                {
                    if (cam == null)
                        return;
                    try
                    {
                        cam.GetPreviewBufferArgb32(bitmap.Pixels);
                        bitmap = WatchDogHelper.rotateImage(bitmap, 270);
                        detectFaces(bitmap);
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                });

            });
            thread.Start();
        }
        

        #endregion


        private void setInternetToggle(bool ticked)
        {
            checkInternet.IsChecked = ticked;
        }



       

    }
}