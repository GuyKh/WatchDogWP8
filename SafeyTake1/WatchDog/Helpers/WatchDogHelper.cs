using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Shell;
using System.Reflection;
using System.ComponentModel;

namespace WatchDOG.Helpers
{
    public class WatchDogHelper
    {
        public static void ShowToastMessage(String title, String message)
        {

            ToastPrompt tp = new ToastPrompt();

            tp.Title = title;
            tp.Message = message;
            tp.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            tp.TextWrapping = TextWrapping.Wrap;

            tp.Show();
        }

        public static WriteableBitmap rotateImage(WriteableBitmap wbSource, int angle)
        {
            WriteableBitmap wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);

            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }

            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }

            return wbTarget;

        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static WriteableBitmap CropImage(WriteableBitmap source,
                                                         int xOffset, int yOffset,
                                                         int width, int height)
        {
            // Get the width of the source image
            var sourceWidth = source.PixelWidth;

            // Get the resultant image as WriteableBitmap with specified size
            WriteableBitmap result = null;
            
            EventWaitHandle Wait = new AutoResetEvent(false);
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                result = new WriteableBitmap(width, height);
                Wait.Set();
            });
            // wait while item is added on UI
            Wait.WaitOne();
            

            // Create the array of bytes
            for (var x = 0; x <= height - 1; x++)
            {
                var sourceIndex = xOffset + (yOffset + x)*sourceWidth;
                var destinationIndex = x*width;

                Array.Copy(source.Pixels, sourceIndex, result.Pixels, destinationIndex, width);
            }
            return result;



            
            
        }
    }
}
