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

namespace WatchDOG.Screens
{
    public partial class CalibrationScreen : PhoneApplicationPage
    {
        PhotoCamera cam;
        MediaLibrary library = new MediaLibrary();
        bool PhotoTaken = false;
        Detector detector;
        DateTime frameStart;

        

        public CalibrationScreen()
        {
            InitializeComponent();
            detector = Detector.Create("models\\haarcascade_eye.xml");
        }

        private void setGPSToggle(bool ticked)
        {
            checkGPS.IsChecked = ticked;
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
            cam.CaptureThumbnailAvailable += new EventHandler<ContentReadyEventArgs>(cam_ThumbnailAvailable);
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
                cam.CaptureThumbnailAvailable -= cam_ThumbnailAvailable;
            }
        }

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
                detectFaces(bitmap);
            });
        }

        private void cam_ThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void setInternetToggle(bool ticked)
        {
            checkInternet.IsChecked = ticked;
        }



        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }

        private void detectFaces(WriteableBitmap bitmap)
        {
            // The user sees a transposed image in the viewfinder, transpose the image for face detection as well.
            WriteableBitmap detectorBitmap = (new WriteableBitmap(bitmap));
            var thread = new System.Threading.Thread(delegate()
            {
                List<NativeFaceDetector.Rectangle> rectangles = detector.getFaces(detectorBitmap, 4.0f, 1.55f, 0.08f, 2);

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


                Dispatcher.BeginInvoke(delegate()
                {
                    overlayCanvas.Children.Clear();
                    foreach (var r in rectangles)
                    {
                        Rectangle toAdd = new Rectangle();
                        TranslateTransform loc = new TranslateTransform();
                        loc.X = r.x();
                        loc.Y = r.y();
                        toAdd.RenderTransform = loc;
                        toAdd.Width = r.width();
                        toAdd.Height = r.height();
                        toAdd.Stroke = new SolidColorBrush(Colors.Red);
                        toAdd.StrokeThickness = 5;
                        overlayCanvas.Children.Add(toAdd);
                    }
                });

                this.Dispatcher.BeginInvoke(delegate()
                {

                    if (cam == null)
                        return;

                    cam.GetPreviewBufferArgb32(bitmap.Pixels);
                    detectFaces(bitmap);
                });
            });
            thread.Start();
        }

        // Assumes that the bitmapcontext is active
        private void drawHeaderForBox(WriteableBitmap bitmap, int x, int y, int width, String text)
        {
            const int BOX_HEIGHT = 40;
            const int BOX_WIDTH = 120;

            TranslateTransform transform = new TranslateTransform();
            transform.X = x + (width - BOX_WIDTH) / 2;
            transform.Y = y;

            Border border = new Border();
            border.Height = BOX_HEIGHT;
            border.Width = BOX_WIDTH;
            border.Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));

            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 36;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.Text = text;

            border.Child = textBlock;
            border.Arrange(new Rect(0.0, 0.0, border.Width, border.Height));
            border.UpdateLayout();

            bitmap.Render(border, transform);
        }

    }
}