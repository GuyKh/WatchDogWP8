using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WatchDOG.DataStructures
{
    public class Settings
    {
        #region Constants

        readonly string SETTINGS_FILEPATH = "";

        #endregion 

        #region Private Properties

        private static Color _lowAlertColor;
        private static Color _medAlertColor;
        private static Color _highAlertColor;


        private static string _medAlertSoundFilePath;
        private static string _highAlertSoundFilePath;

        private static Driver _currentDriver;

        private static bool _isGpsEnabled;


        private static bool _isLaneDetectionEnabled;       // Maybe have the enabled within the type itself


        private static bool _isDistanceDetectionEnabled;


        private static bool _isEyeDetectionEnabled;


        private static Units _unit;

        
        #endregion 

        #region Public Properties

        public static Color LowAlertColor
        {
            get { return _lowAlertColor; }
            set { _lowAlertColor = value; }
        }

        public static Color MedAlertColor
        {
            get { return _medAlertColor; }
            set { _medAlertColor = value; }
        }

        public static Color HighAlertColor
        {
            get { return _highAlertColor; }
            set { _highAlertColor = value; }
        }

        public static string MedAlertSoundFilePath
        {
            get { return _medAlertSoundFilePath; }
            set { _medAlertSoundFilePath = value; }
        }
        public static string HighAlertSoundFilePath
        {
            get { return _highAlertSoundFilePath; }
            set { _highAlertSoundFilePath = value; }
        }

        internal static Driver CurrentDriver
        {
            get { return _currentDriver; }
            set { _currentDriver = value; }
        }

        public static bool IsGpsEnabled
        {
            get { return _isGpsEnabled; }
            set { _isGpsEnabled = value; }
        }
        public static bool IsLaneDetectionEnabled
        {
            get { return _isLaneDetectionEnabled; }
            set { _isLaneDetectionEnabled = value; }
        }
        public static bool IsDistanceDetectionEnabled
        {
            get { return _isDistanceDetectionEnabled; }
            set { _isDistanceDetectionEnabled = value; }
        }
        public static bool IsEyeDetectionEnabled
        {
            get { return _isEyeDetectionEnabled; }
            set { _isEyeDetectionEnabled = value; }
        }
        internal static Units Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
        #endregion

        #region Constructor
        public Settings()
        {
            _unit = Units.Metric;
            
            _lowAlertColor = Colors.Green;
            _medAlertColor = Colors.Yellow;

        }

        #endregion

        #region Public Methods
        public static void LoadSettingsFromDisk()
        {

        }

        public static void SaveSettingsToDisk()
        {

            Loaded = true;
        }

        #endregion

        public static bool Loaded { get; set; }
    }

    enum Units{
        Metric,     // Meters
        Imperial    // Miles
    }
}
