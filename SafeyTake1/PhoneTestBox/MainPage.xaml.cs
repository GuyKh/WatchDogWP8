using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneTestBox.Resources;
using Windows.Phone.Media.Capture;   // For advanced capture APIs
using Microsoft.Xna.Framework.Media; // For the media library
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;                     // For the memory streams


namespace PhoneTestBox
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Create a capture sequence object with 1 frame.
        CameraCaptureSequence seq;
        PhotoCaptureDevice cam;


        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

   


        private void btnClick_Click(object sender, RoutedEventArgs e)
        {
            MainPage_Loaded(sender, e);

        }



        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            using (MemoryStream stream = new MemoryStream())
            using (var camera = await PhotoCaptureDevice.OpenAsync(CameraSensorLocation.Back,
                    PhotoCaptureDevice.GetAvailableCaptureResolutions(CameraSensorLocation.Back).First()))
            {
                var sequence = camera.CreateCaptureSequence(1);
                sequence.Frames[0].CaptureStream = stream.AsOutputStream();
                camera.PrepareCaptureSequenceAsync(sequence);
                await sequence.StartCaptureAsync();

                stream.Seek(0, SeekOrigin.Begin);

                using (var library = new MediaLibrary())
                {
                    library.SavePictureToCameraRoll("currentImage.jpg", stream);

                    BitmapImage licoriceImage = new BitmapImage(new Uri("currentImage.jpg", UriKind.Relative));
                    imgPlace.Source = licoriceImage;
                }
            }
        }
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}