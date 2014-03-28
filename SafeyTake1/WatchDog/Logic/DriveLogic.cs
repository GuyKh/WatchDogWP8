using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Windows.Foundation;
using Windows.Phone.Media.Capture;
using Microsoft.Devices;
using WatchDOG.Alerters;
using WatchDOG.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using WatchDOG.Helpers;
using CameraExplorer;


namespace WatchDOG.Logic
{
    class DriveLogic
    {
        
        #region Constants
        /// <summary>
        /// Threshold from whatlevel up to alarm.
        /// </summary>
        public const double ALERT_THRESHOLD = 50;

        #endregion


        #region Private Properties
        private int[] _continousSafeTime;
        private List<AlertEvent> _eventsList;
        private Speed _speed;
        private float _score;
        private List<FrontCameraAlerterAbstract> frontAlerters;
        //internal PhotoCamera frontCam;
        internal Drive _currentDrive;
        private Driver _currentDriver;
        private string _alertMessage;
        public Dispatcher dispatcher;
        internal PhotoCaptureDevice photoCaptureDevice;

        private CameraExplorer.DataContext _dataContext = CameraExplorer.DataContext.Singleton;

        
        #endregion 

        #region Constructor
        public DriveLogic(Driver driver)
        {
            _currentDriver = new Driver("A", "B", "C");
            frontAlerters = new List<FrontCameraAlerterAbstract>();
            frontAlerters.Add(new EyeDetectorAlerter());

            _currentDrive = new Drive(_currentDriver, DateTime.Now);
        }

        #endregion


        #region Public Properties
        public float Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public int[] ContinousSafeTime
        {
            get { return _continousSafeTime; }
            set { _continousSafeTime = value; }
        }


        internal List<AlertEvent> EventsList
        {
            get { return _eventsList; }
            set { _eventsList = value; }
        }


        public Speed Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        public event AlertEventHandler Alert;
        #endregion



        public void StartLoop()
        {
            while (true)
            {
                _dataContext.ImageStream.Position = 0;

                WriteableBitmap frontCamBitmap = new WriteableBitmap((int)_dataContext.Device.CaptureResolution.Width, 
                    (int)_dataContext.Device.CaptureResolution.Height);
                frontCamBitmap.LoadJpeg(_dataContext.ImageStream);

                // Foreach Alerter, try to analize the value.
                List<AlertEvent> alertEvents = new List<AlertEvent>();
                foreach (IAlerter alerter in frontAlerters)
                {
                    alerter.GetData();

                    AlertEvent alertEvent = new AlertEvent()
                    {
                        AlertLevel = alerter.ProcessData(frontCamBitmap),
                        AlertTime = DateTime.Now,
                        AlertType = alerter.GetAlerterType(),
                        Driver = _currentDriver
                    };



                    if (alertEvent.AlertLevel >= ALERT_THRESHOLD)
                        _currentDrive.Events.Add(alertEvent);


                    alertEvents.Add(alertEvent);
                }

                // Calculate the safety score (for all valid values).
                double res = calculateSafetyScore(alertEvents.Where(_event => _event.AlertLevel != -1));

                // Update the UI
                if (Alert != null)
                    Alert(this, new AlertEventHandlerArgs(res, _alertMessage));

            }
        }

        
       
        //public void detect()
        //{
            
        //    if (working)
        //        return;
        //    dispatcher.BeginInvoke(delegate()
        //    {
        //        working = true;
        //        // Initialize a new Bitmap image
        //        WriteableBitmap bitmap = new WriteableBitmap((int) frontCam.PreviewResolution.Width,
        //            (int) frontCam.PreviewResolution.Height);

        //        // Get the bitmap from the camera
        //        frontCam.GetPreviewBufferArgb32(bitmap.Pixels);

        //        bitmap = WatchDogHelper.rotateImage(bitmap, 270);

        //        // Foreach Alerter, try to analize the value.
        //        List<AlertEvent> alertEvents = new List<AlertEvent>();
        //        foreach (IAlerter alerter in frontAlerters)
        //        {
        //            alerter.GetData();

        //            AlertEvent alertEvent = new AlertEvent()
        //            {
        //                AlertLevel = alerter.ProcessData(bitmap),
        //                AlertTime = DateTime.Now,
        //                AlertType = alerter.GetAlerterType(),
        //                Driver = _currentDriver
        //            };



        //            if (alertEvent.AlertLevel >= ALERT_THRESHOLD)
        //                _currentDrive.Events.Add(alertEvent);


        //            alertEvents.Add(alertEvent);
        //        }

        //        // Calculate the safety score (for all valid values).
        //        double res = calculateSafetyScore(alertEvents.Where(_event => _event.AlertLevel != -1));

        //        // Update the UI
        //        if (Alert != null)
        //            Alert(this, new AlertEventHandlerArgs(res, _alertMessage));

        //        working = false;
        //    }
        //);

       
        //}


        public void StopLoop()
        {
           
        }

        
        
        #region Safety Calculation Functions
        private double calculateSafetyScore(IEnumerable<AlertEvent> alertEvents)
        {
            // Alert level is the Average of all alerters score
            double ret = alertEvents.Average(alert => alert.AlertLevel); 
            
            if (ret > ALERT_THRESHOLD)
            {
                EAlertType highestAlertType = alertEvents.OrderByDescending(_event => _event.AlertLevel).FirstOrDefault().AlertType;
                updateTheMessageByType(highestAlertType);
            }
            else
            {
                _alertMessage = "Drive Safely";
            }
            return ret;
        }

        private void updateTheMessageByType(EAlertType highestAlertType)
        {
            switch (highestAlertType)
            {
                case EAlertType.EyeDetectionAlert:
                    _alertMessage = "Open Your Eyes!";
                    break;
                case EAlertType.DistanceAlert:
                    _alertMessage = "Keep Your Distance";
                    break;
                case EAlertType.LaneCrossingAlert:
                    _alertMessage = "Keep in Your Lane";
                    break;
                default:
                    _alertMessage = "Drive Safely";
                    break;
            }
        }
        #endregion

        #region Camera
        [ThreadStatic]
        private bool working = false;

        #region Initialization and Disposal

        /// <summary>
        /// Initilialize Available Cameras
        /// </summary>
        internal async void InitCameras()
        {
            //if (Camera.IsCameraTypeSupported(CameraType.FrontFacing))
            //{

            //    frontCam = new Microsoft.Devices.PhotoCamera(CameraType.FrontFacing);
            //    if (frontCam == null)
            //    {
            //        WatchDogHelper.ShowToastMessage("Error Starting Front Camera",
            //            "Front Camera could not be initialized");
            //        //MessageBox.Show("ERROR: Front Camera Not Available", "Error", MessageBoxButton.OK);
            //    }

            //    //frontCam.Initialized += frontCam_Initialized;
            //}
            //else
            //{
            //    WatchDogHelper.ShowToastMessage("Error Starting Front Camera",
            //            "Front Camera is not supported");
            //}
            if (_dataContext != null)
            {
                await InitializeCamera(CameraSensorLocation.Front);
                
            }

            if (PhotoCaptureDevice.AvailableSensorLocations.Contains(CameraSensorLocation.Front))
            {
                System.Collections.Generic.IReadOnlyList<Windows.Foundation.Size> SupportedResolutions =
                    PhotoCaptureDevice.GetAvailableCaptureResolutions(CameraSensorLocation.Front);
                Windows.Foundation.Size res = PhotoCaptureDevice.GetAvailableCaptureResolutions(CameraSensorLocation.Front).First();
                
                photoCaptureDevice = await PhotoCaptureDevice.OpenAsync(CameraSensorLocation.Front, res);
                await photoCaptureDevice.SetPreviewResolutionAsync(res);
                photoCaptureDevice.PreviewFrameAvailable += photoCaptureDevice_PreviewFrameAvailable;
            }
        }

        private async Task InitializeCamera(CameraSensorLocation sensorLocation)
        {
            Windows.Foundation.Size initialResolution = new Windows.Foundation.Size(640, 480);

            PhotoCaptureDevice d = await PhotoCaptureDevice.OpenAsync(sensorLocation, initialResolution);

            d.SetProperty(KnownCameraGeneralProperties.EncodeWithOrientation,
                          d.SensorLocation == CameraSensorLocation.Back ?
                          d.SensorRotationInDegrees : -d.SensorRotationInDegrees);

            _dataContext.Device = d;
        }
        

        internal void DisposeCameras()
        {
            //// Dispose camera
            //if (frontCam != null)
            //{
            //    // Dispose camera to minimize power consumption and to expedite shutdown.
            //    frontCam.Dispose();

            //    // Release memory, ensure garbage collection.
            //    //frontCam.Initialized -= frontCam_Initialized;
            //    //frontCam.CaptureThumbnailAvailable -= frontCam_ThumbnailAvailable;
            //}

            if (photoCaptureDevice != null)
            {
                photoCaptureDevice.Dispose();
                photoCaptureDevice.PreviewFrameAvailable -= photoCaptureDevice_PreviewFrameAvailable;
            }
        }
        #endregion

        #region Front Camera Event Handlers
        private void photoCaptureDevice_PreviewFrameAvailable(ICameraCaptureDevice sender, object args)
        {
            if (working)
                return;
            working = true;
            dispatcher.BeginInvoke(delegate()
            {
                // Initialize a new Bitmap image

                WriteableBitmap bitmap = new WriteableBitmap((int)sender.PreviewResolution.Width,
                    (int)sender.PreviewResolution.Height);

                // Get the bitmap from the camera
                sender.GetPreviewBufferArgb(bitmap.Pixels);

                bitmap = WatchDogHelper.rotateImage(bitmap, 270);

                // Foreach Alerter, try to analize the value.
                List<AlertEvent> alertEvents = new List<AlertEvent>();
                foreach (IAlerter alerter in frontAlerters)
                {
                    alerter.GetData();

                    AlertEvent alertEvent = new AlertEvent()
                    {
                        AlertLevel = alerter.ProcessData(bitmap),
                        AlertTime = DateTime.Now,
                        AlertType = alerter.GetAlerterType(),
                        Driver = _currentDriver
                    };



                    if (alertEvent.AlertLevel >= ALERT_THRESHOLD)
                        _currentDrive.Events.Add(alertEvent);


                    alertEvents.Add(alertEvent);
                }

                // Calculate the safety score (for all valid values).
                double res = calculateSafetyScore(alertEvents.Where(_event => _event.AlertLevel != -1));

                // Update the UI
                if (Alert != null)
                    Alert(this, new AlertEventHandlerArgs(res, _alertMessage));

                working = false;
            }

        );
        }


        /* Doesn't Work :(
        private void frontCam_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            if (!e.Succeeded)
            {
                frontCam = null;
                WatchDogHelper.ShowToastMessage("Error Starting Front Camera", "Front Camera could not be initialized");
                //MessageBox.Show("ERROR: Camera is not available", "ERROR", MessageBoxButton.OK);
                return;
            }

            try
            {
                frontCam = (PhotoCamera)sender;
                frontCam.Resolution = frontCam.AvailableResolutions.First();
            }
            catch (Exception)
            {
                frontCam = null;
                WatchDogHelper.ShowToastMessage("Front Camera Error", "Front Camera is not avaliable");
                // MessageBox.Show("ERROR: Camera is not available", "ERROR", MessageBoxButton.OK);
                return;
            }

            detect();

            
        }


        private void frontCam_ThumbnailAvailable(object sender, ContentReadyEventArgs e)
        {
            if (frontCam == null)
            {
                //MessageBox.Show("ERROR: Camera is not available", "ERROR", MessageBoxButton.OK);
                WatchDogHelper.ShowToastMessage("Error Starting Front Camera", "Front Camera could not be initialized");
                return;
            }

            // Initialize a new Bitmap image
            WriteableBitmap bitmap = new WriteableBitmap((int)frontCam.PreviewResolution.Width,
                                                            (int)frontCam.PreviewResolution.Height);

            // Get the bitmap from the camera
            frontCam.GetPreviewBufferArgb32(bitmap.Pixels);
            
            bitmap = WatchDogHelper.rotateImage(bitmap, 270);

            // Foreach Alerter, try to analize the value.
            List<AlertEvent> alertEvents = new List<AlertEvent>();
            foreach (IAlerter alerter in frontAlerters)
            {
                alerter.GetData();

                AlertEvent alertEvent = new AlertEvent()
                {
                    AlertLevel = alerter.ProcessData(bitmap),
                    AlertTime = DateTime.Now,
                    AlertType = alerter.GetAlerterType(),
                    Driver = _currentDriver
                };



                if (alertEvent.AlertLevel >= ALERT_THRESHOLD)
                    _currentDrive.Events.Add(alertEvent);


                alertEvents.Add(alertEvent);

            }

            // Calculate the safety score (for all valid values).
            double res = calculateSafetyScore(alertEvents.Where(_event => _event.AlertLevel != -1));

            // Update the UI
            if (Alert != null) 
                Alert(this, new AlertEventHandlerArgs(res, _alertMessage));
        }
          
         */
        #endregion
        #endregion


    }

    internal delegate void AlertEventHandler(object sender, AlertEventHandlerArgs args);

    public class AlertEventHandlerArgs
    {
        public AlertEventHandlerArgs(double level, string msg)
        {
            Level = level;
            Message = msg;
        }

        public double Level { get; set; }
        public string Message { get; set; }
    }
}

