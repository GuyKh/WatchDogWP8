﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDOG.DataStructures
{
    class Driver
    {

        #region Private Properties
        private string _name;
        private string _username;
        private string _password;
        private float _avgScore;
        private List<AlertEvent> _driversEvents;

        #endregion 
        
        #region Public Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public float AverageScore
        {
            get { return _avgScore; }
            set { _avgScore = value; }
        }

        public List<AlertEvent> DriversEvents
        {
            get { return _driversEvents; }
            set { _driversEvents = value; }
        }
        #endregion

        #region Constructor
        public Driver(string name, string username, string password)
        {
            _name = name;
            _username = username;
            _password = password;
            _avgScore = 0;
            _driversEvents = new List<AlertEvent>();
        }
        #endregion 

        #region Private Methods
        private void recalculateAverage()
        {
            _avgScore = 0;
            foreach(AlertEvent ae in _driversEvents)
            {
                _avgScore += ae.AlertLevel / _driversEvents.Count;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new Alert Event to this Driver's History.
        /// </summary>
        /// <param name="ae"></param>
        /// <returns>New Average Score of the driver</returns>
        public float AddEvent(AlertEvent ae)
        {
            _driversEvents.Add(ae);
            recalculateAverage();
            return _avgScore;
        }

  
        #endregion
    }
}
