using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeyTake1.DataStructures
{
    class Drive
    {
        #region Private Properties
        
        private List<AlertEvent> _events;
        private DateTime _startTime;
        private DateTime _endTime;
        private Driver _driver;

        #endregion 

        #region Constructor
        public Drive(Driver driver)
        {
            _events = new List<AlertEvent>();
            _startTime = DateTime.Now;
            _driver = driver;
        }
        public Drive(Driver driver, DateTime startTime)
        {
            this(driver);
            _startTime = startTime;

        }
        #endregion

        #region Public Properties

        public List<AlertEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public Driver Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }
        #endregion
    }
}
