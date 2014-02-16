using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Configuration;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WatchDogWcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class WatchDogService : IWatchDogService
    {
        /// <summary>
        /// Returns true if there are atleast one face and two eyes in the picture.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public bool IsThereFaces(Image<Bgr, byte> image)
        {
            FacesWithEyes _faces = GetFaces(image);
            return (_faces.Faces.Count > 0 && _faces.Eyes.Count > 1);
        }

        /// <summary>
        /// Detects faces and eyes and retuns rectangles with the face/eyes areas.
        /// Uses default values.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public FacesWithEyes GetFaces(Image<Bgr, byte> image)
        {
            string facesXmlModel = WebConfigurationManager.AppSettings["DefaultFacesXMLModel"];
            string eyesXmlModel = WebConfigurationManager.AppSettings["DefaultEyesXMLModel"];
            double faceRatio = Double.Parse(WebConfigurationManager.AppSettings["DefaultFacesRatio"]);
            double eyesRatio = Double.Parse(WebConfigurationManager.AppSettings["DefaulteyesRatio"]);

            return GetFaces(image, facesXmlModel, eyesXmlModel, faceRatio, eyesRatio);
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
        public FacesWithEyes GetFaces(Image<Bgr, byte> image, string facesXmlModel, string eyesXmlModel, double faceRatio, double eyeRatio)
        {
            long detectionTime = 0;
            return DetectFace.Detect(image, facesXmlModel, eyesXmlModel, out detectionTime, faceRatio, eyeRatio);
        }
    }
}
