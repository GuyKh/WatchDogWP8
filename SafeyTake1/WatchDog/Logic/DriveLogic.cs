using System.Threading;
using System.Windows;
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
using WatchDOG.Screens;


namespace WatchDOG.Logic
{
    class DriveLogic
    {
        
        #region Constants
        /// <summary>
        /// Threshold from whatlevel up to alarm.
        /// </summary>
        public const double ALERT_THRESHOLD = 50;

        public const int DURATION_INTERVAL = 1000;

        #endregion


        #region Private Properties

        private List<FrontCameraAlerterAbstract> frontAlerters;
        internal Drive _currentDrive;
        private Driver _currentDriver;
        private string _alertMessage;


        
        #endregion 

        #region Constructor
        public DriveLogic(Driver driver)
        {
            _currentDriver = driver;
            frontAlerters = new List<FrontCameraAlerterAbstract>();
            frontAlerters.Add(new EyeDetectorAlerter());

            _currentDrive = new Drive(_currentDriver, DateTime.Now);
        }

        #endregion


        #region Public Properties

        public float Score { get; set; }

        public int[] ContinousSafeTime { get; set; }


        internal List<AlertEvent> EventsList { get; set; }


        public Speed Speed { get; set; }

        public string AlertMessage
        {
            get { return _alertMessage; }
        }

        #endregion






        public double AnalyzeFrontPicture(WriteableBitmap bitmap)
        {
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
            return calculateSafetyScore(alertEvents.Where(_event => _event.AlertLevel >= 0));

            
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

