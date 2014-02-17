using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using Emgu.CV;
using Emgu.CV.GPU;
using Emgu.CV.Structure;

namespace WatchDogWebApplication.Models
{
    public class DetectFace
    {
        public static FacesWithEyes Detect(Image img, string faceFileName, String eyeFileName, out long detectionTime, double faceRatio, double eyeRatio)
        {
            string tempFilename = DateTime.Now.Ticks + ".jpg";
            img.Save(tempFilename);
            Image<Bgr, Byte> image = new Image<Bgr, byte>(tempFilename); //Read the files as an 8-bit Bgr image  

            Stopwatch watch = Stopwatch.StartNew();
            List<Rectangle> faces = new List<Rectangle>();
            List<Rectangle> eyes = new List<Rectangle>();

            if (GpuInvoke.HasCuda)
            {
                using (GpuCascadeClassifier face = new GpuCascadeClassifier(faceFileName))
                using (GpuCascadeClassifier eye = new GpuCascadeClassifier(eyeFileName))
                {

                    using (GpuImage<Bgr, Byte> gpuImage = new GpuImage<Bgr, byte>(image))
                    using (GpuImage<Gray, Byte> gpuGray = gpuImage.Convert<Gray, Byte>())
                    {
                        Rectangle[] faceRegion = face.DetectMultiScale(gpuGray, faceRatio, 10, Size.Empty);
                        faces.AddRange(faceRegion);
                        foreach (Rectangle f in faceRegion)
                        {
                            using (GpuImage<Gray, Byte> faceImg = gpuGray.GetSubRect(f))
                            {
                                //For some reason a clone is required.
                                //Might be a bug of GpuCascadeClassifier in opencv
                                using (GpuImage<Gray, Byte> clone = faceImg.Clone(null))
                                {
                                    Rectangle[] eyeRegion = eye.DetectMultiScale(clone, eyeRatio, 10, Size.Empty);

                                    foreach (Rectangle e in eyeRegion)
                                    {
                                        Rectangle eyeRect = e;
                                        eyeRect.Offset(f.X, f.Y);
                                        eyes.Add(eyeRect);
                                    }
                                }
                            }
                        }
                    }
                    watch.Stop();
                }




            }
            else
            {

                //Read the HaarCascade objects
                using (CascadeClassifier face = new CascadeClassifier(faceFileName))
                using (CascadeClassifier eye = new CascadeClassifier(eyeFileName))
                {

                    using (Image<Gray, Byte> gray = image.Convert<Gray, Byte>()) //Convert it to Grayscale
                    {
                        //normalizes brightness and increases contrast of the image
                        gray._EqualizeHist();

                        //Detect the faces  from the gray scale image and store the locations as rectangle
                        //The first dimensional is the channel
                        //The second dimension is the index of the rectangle in the specific channel
                        Rectangle[] facesDetected = face.DetectMultiScale(
                            gray,
                            faceRatio,
                            10,
                            new Size(20, 20),
                            Size.Empty);
                        faces.AddRange(facesDetected);

                        foreach (Rectangle f in facesDetected)
                        {
                            //Set the region of interest on the faces
                            gray.ROI = f;
                            Rectangle[] eyesDetected = eye.DetectMultiScale(
                                gray,
                                eyeRatio,
                                10,
                                new Size(20, 20),
                                Size.Empty);
                            gray.ROI = Rectangle.Empty;

                            foreach (Rectangle e in eyesDetected)
                            {
                                Rectangle eyeRect = e;
                                eyeRect.Offset(f.X, f.Y);
                                eyes.Add(eyeRect);
                            }
                        }
                    }
                    watch.Stop();
                }


            }
            detectionTime = watch.ElapsedMilliseconds;

            return new FacesWithEyes { Faces = faces, Eyes = eyes };
        }
    }
}