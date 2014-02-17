using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WatchDogWebApplication.Models
{
    public class EyeDetector
    {
        /// <summary>
        /// Returns true if there are atleast one face and two eyes in the picture.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static bool ContainsFaces(Image image)
        {
            FacesWithEyes faces = GetFaces(image);
            return (faces.Faces.Count > 0 && faces.Eyes.Count > 1);
        }

        /// <summary>
        /// Detects faces and eyes and retuns rectangles with the face/eyes areas.
        /// Uses default values.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static FacesWithEyes GetFaces(Image image)
        {
            string facesXmlModel = System.Configuration.ConfigurationManager.AppSettings["DefaultFacesXMLModel"];
            string eyesXmlModel = System.Configuration.ConfigurationManager.AppSettings["DefaultEyesXMLModel"];
            double faceRatio = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["DefaultFacesRatio"]);
            double eyesRatio = Double.Parse(System.Configuration.ConfigurationManager.AppSettings["DefaultEyesRatio"]);
            return GetFacesWithCustomData(image, facesXmlModel, eyesXmlModel, faceRatio, eyesRatio);
        }

        /// <summary>
        /// Detects faces and eyes and retuns rectangles with the face/eyes areas.
        /// Uses custom values.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="facesXmlModel"></param>
        /// <param name="eyesXmlModel"></param>
        /// <param name="faceRatio"></param>
        /// <param name="eyeRatio"></param>
        /// <returns></returns>
        public static FacesWithEyes GetFacesWithCustomData(Image image, string facesXmlModel, string eyesXmlModel, double faceRatio, double eyeRatio)
        {
            long detectionTime = 0;
            return DetectFace.Detect(image, facesXmlModel, eyesXmlModel, out detectionTime, faceRatio, eyeRatio);
        }
    }

    

    public class FacesWithEyes
    {
        public List<Rectangle> Faces { get; set; }

        public List<Rectangle> Eyes { get; set; }
    }
}