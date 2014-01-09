using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace SafeyTake1.DataStructures
{
    class AlertEvent
    {
        
        private DateTime _alertTime;
        private EAlertType _alertType;
        private Driver _driver;
        private Geocoordinate _alertLocation;
        private float _alertLevel;

        public DateTime AlertTime
        {
            get { return _alertTime; }
            set { _alertTime = value; }
        }

        public EAlertType AlertType
        {
            get { return _alertType; }
            set { _alertType = value; }
        }

        public Driver Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }
        public Geocoordinate AlertLocation
        {
            get { return _alertLocation; }
            set { _alertLocation = value; }
        }

        public float AlertLevel
        {
            get { return _alertLevel; }
            set { _alertLevel = value; }
        }
    }
}
