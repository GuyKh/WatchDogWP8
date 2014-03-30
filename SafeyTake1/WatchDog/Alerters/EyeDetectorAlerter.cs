﻿using FaceDetectionWinPhone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WatchDOG.DataStructures;

namespace WatchDOG.Alerters
{
    class EyeDetectorAlerter : FrontCameraAlerterAbstract
    {
        #region Model Configuration
        public const string MODEL_XML = "models\\haarcascade_eye.xml";

        //The initial ratio between the size of your image and the size of the sliding window (default: 2)
        public const float BASE_SCALE = 8.0f;   

        //How much to increment your window for every iteration (default:1.25)
        public const float SCALE_INC = 1.1f;

        // How much to shif the window at each step, in terms of the % of the window size
        public const float INCREMENT = 0.05f;

        // Minimum number of overlapping face rectangles to be considered a valid face (default: 1)
        public const int MIN_NEIGHBORS = 2;
        #endregion 

        #region Private Properties
        private Detector detector;
        #endregion 

        #region Constructor
        public EyeDetectorAlerter()
        {
            detector = Detector.Create(MODEL_XML);
        }
        #endregion

        #region Public Methods
        public override double ProcessData(WriteableBitmap bitmap)
        {
            if (detector == null){
                //Fail in initializing the Detector
                return -1;
            }


            //WriteableBitmap detectorBitmap = (new WriteableBitmap(bitmap));

            // Should Use different thread????
            List<NativeFaceDetector.Rectangle> rectangles = detector.getFaces(bitmap, BASE_SCALE, SCALE_INC, INCREMENT, MIN_NEIGHBORS);

            if (rectangles.Any())
            {
                if (rectangles.Count >= 2)
                    return 100;
                return 50;
            }
            return 1;

        }

        public override EAlertType GetAlerterType()
        {
            return EAlertType.EyeDetectionAlert;
        }
        #endregion



        
    }
}
