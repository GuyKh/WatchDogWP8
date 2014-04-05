using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using NativeFaceDetector;
using WatchDOG.DataStructures;
using WatchDOG.Helpers;
using Detector = FaceDetectionWinPhone.Detector;

namespace WatchDOG.Alerters
{
    class EyeDetectorAlerter : FrontCameraAlerterAbstract
    {
        #region Model Configuration
        public const string MODEL_XML = "models\\haarcascade_eye.xml";

        //The initial ratio between the size of your image and the size of the sliding window (default: 2)
        //public const float BASE_SCALE = 8.0f;   
        public const float BASE_SCALE = 4.0f;  

        //How much to increment your window for every iteration (default:1.25)
        //public const float SCALE_INC = 1.1f;
        public const float SCALE_INC = 1.55f;

        // How much to shif the window at each step, in terms of the % of the window size
        //public const float INCREMENT = 0.05f;
        public const float INCREMENT = 0.08f;

        // Minimum number of overlapping face rectangles to be considered a valid face (default: 1)
        public const int MIN_NEIGHBORS = 2;
        #endregion 

        #region Private Properties
        private readonly Detector _detector;
        private Detector _faceDetector;
        #endregion 

        #region Constructor
        public EyeDetectorAlerter()
        {
            _faceDetector = Detector.Create("models\\haarcascade_frontalface_alt.xml");
            _detector = Detector.Create(MODEL_XML);
        }

        #endregion

        #region Public Methods
        public override double ProcessData(WriteableBitmap bitmap)
        {
            if (_detector == null){
                //Fail in initializing the Detector
                return -1;
            }


            //WriteableBitmap detectorBitmap = (new WriteableBitmap(bitmap));

            // Should Use different thread????
            List<NativeFaceDetector.Rectangle> rectangles = Detect(bitmap);

            if (rectangles.Any())
            {
                if (rectangles.Count >= 2)
                    return 0;
                return 50;
            }
            return 100;

        }

        public override EAlertType GetAlerterType()
        {
            return EAlertType.EyeDetectionAlert;
        }
        #endregion

        public List<Rectangle> Detect(WriteableBitmap bitmap)
        {
            return DetectEyes(bitmap);
        }

        private List<Rectangle> DetectEyes(WriteableBitmap bitmap)
        {
           return _detector.getFaces(bitmap, BASE_SCALE, SCALE_INC, INCREMENT, MIN_NEIGHBORS);
        }

        //public List<Rectangle> Detect(WriteableBitmap bitmap)
        //{
        //    var faces = DetectFaces(bitmap);

        //    var eyes = new List<Rectangle>();
        //    foreach (Rectangle face in faces)
        //    {
        //        WriteableBitmap faceBitmap = WatchDogHelper.CropImage(bitmap, (int)face.x(), (int)face.y(),
        //            (int)face.width(), (int)face.height());
        //        eyes.AddRange(DetectEyes(faceBitmap));
        //    }
        //    return eyes;
        //}

        //internal List<Rectangle> DetectEyes(WriteableBitmap bitmap)
        //{
        //    return _detector.getFaces(bitmap, BASE_SCALE, SCALE_INC, INCREMENT, MIN_NEIGHBORS);
        //}

        //internal List<Rectangle> DetectFaces(WriteableBitmap bitmap)
        //{
        //    return _faceDetector.getFaces(bitmap, 3.0f, 1.15f, 0.08f, 2);
        //}

    }
}
