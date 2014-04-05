using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDOG.DataStructures
{
    class Drive
    {
        #region Private Properties
        
        private List<AlertEvent> _events;
        private DateTime _startTime;
        private DateTime _endTime;
        private Driver _driver;
        private float _averageScore;
        #endregion 

        #region Constructor
        public Drive(Driver driver)
        {
            _events = new List<AlertEvent>();
            _startTime = DateTime.Now;
            _driver = driver;
        }
        public Drive(Driver driver, DateTime startTime) : this(driver)
        {
            _startTime = startTime;

        }
        #endregion

        #region Public Properties
        public string id { get; set; }

        [JsonIgnore()]
        public List<AlertEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        [JsonProperty(PropertyName = "StartTime")]
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        [JsonProperty(PropertyName = "EndTime")]
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        [JsonProperty(PropertyName = "DriverID")]
        public string DriverID
        {
            get { return Driver.id; }
        }

        public Driver Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

         [JsonProperty(PropertyName = "AverageScore")]
        public float AverageScore
        {
            get {
                if (Events == null || !Events.Any())
                    return (float)-1;
                return (float)Events.Average(evnt => evnt.AlertLevel);
            }
            set { _averageScore = value; } 
        }
        #endregion
    }
}
