using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace WatchDOG.DataStructures
{
    public class AlertEvent
    {
        
        private DateTime _alertTime;
        private EAlertType _alertType;
        private Driver _driver;
        private Geocoordinate _alertLocation;
        private double _alertLevel;

        public string id { get; set; }

        [JsonProperty(PropertyName = "AlertTime")]
        public DateTime AlertTime
        {
            get { return _alertTime; }
            set { _alertTime = value; }
        }

        [JsonProperty(PropertyName = "AlertType")]
        public int AlertTypeInt
        {
            get { return (int)AlertType; }
            set { AlertType = (EAlertType)value; }
        }

        [JsonIgnore]
        public EAlertType AlertType
        {
            get { return _alertType; }
            set { _alertType = value; }
        }

        [JsonIgnore()]
        public Driver Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        [JsonProperty(PropertyName = "AlertLocation")]
        public Geocoordinate AlertLocation
        {
            get { return _alertLocation; }
            set { _alertLocation = value; }
        }

        
        //public double AlertLevelFloat
        //{
        //    get { return (float)AlertLevel; }
        //    set { AlertLevel = (double)AlertLevel; }
        //}

        [JsonProperty(PropertyName = "AlertLevel")]
        public double AlertLevel
        {
            get { return _alertLevel; }
            set { _alertLevel = value; }
        }

        [JsonProperty(PropertyName = "DriverId")]
        public string DriverID
        {
            get { return Driver.id; }
        }
    }
}
