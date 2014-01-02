using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SafeyTake1
{
    class Settings
    {
        #region Constants

        readonly string SETTINGS_FILEPATH = "";

        #endregion 

        #region Private Properties

        private Color _lowAlertColor;
        private Color _medAlertColor;
        private Color _highAlertColor;


        private string _medAlertSoundFilePath;
        private string _highAlertSoundFilePath;
        
        private Driver _currentDriver;

        private bool _isGpsEnabled;

        
        private bool _isLaneDetectionEnabled;       // Maybe have the enabled within the type itself

        
        private bool _isDistanceDetectionEnabled;

       
        private bool _isEyeDetectionEnabled;

        
        private Units _unit;

        
        #endregion 

        #region Public Properties
        public Color LowAlertColor
        {
            get { return _lowAlertColor; }
            set { _lowAlertColor = value; }
        }

        public Color MedAlertColor
        {
            get { return _medAlertColor; }
            set { _medAlertColor = value; }
        }

        public Color HighAlertColor
        {
            get { return _highAlertColor; }
            set { _highAlertColor = value; }
        }

        public string MedAlertSoundFilePath
        {
            get { return _medAlertSoundFilePath; }
            set { _medAlertSoundFilePath = value; }
        }
        public string HighAlertSoundFilePath
        {
            get { return _highAlertSoundFilePath; }
            set { _highAlertSoundFilePath = value; }
        }

        internal Driver CurrentDriver
        {
            get { return _currentDriver; }
            set { _currentDriver = value; }
        }

        public bool IsGpsEnabled
        {
            get { return _isGpsEnabled; }
            set { _isGpsEnabled = value; }
        }
        public bool IsLaneDetectionEnabled
        {
            get { return _isLaneDetectionEnabled; }
            set { _isLaneDetectionEnabled = value; }
        }
        public bool IsDistanceDetectionEnabled
        {
            get { return _isDistanceDetectionEnabled; }
            set { _isDistanceDetectionEnabled = value; }
        }
        public bool IsEyeDetectionEnabled
        {
            get { return _isEyeDetectionEnabled; }
            set { _isEyeDetectionEnabled = value; }
        }
        internal Units Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }
        #endregion

        #region Constructor
        public Settings()
        {
            _unit = Units.Metric;
        }

        #endregion

        #region Public Methods
        public void LoadSettingsFromDisk()
        {

        }

        public void SaveSettingsToDisk()
        {

        }

        public void RunTest()
        { }
        #endregion
    }

    enum Units{
        Metric,     // Meters
        Imperial    // Miles
    }
}
