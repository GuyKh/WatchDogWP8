using WatchDOG.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDOG.Logic
{
    class DriveSummary
    {

        private List<AlertEvent> _events;
        private float _score;

        internal List<AlertEvent> Events
        {
            get { return _events; }
            set { _events = value; }
        }
        

        public float Score
        {
            get { return _score; }
            set { _score = value; }
        }

    }

}
