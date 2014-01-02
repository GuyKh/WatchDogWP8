using SafeyTake1.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeyTake1.Logic
{
    class DriveLogic{
   
        private int[] _continousSafeTime;
        private List<AlertEvent> _eventsList;
        private Speed _speed;
        private float _score;

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

     
        private float checkDistance()
        {
            return 0;
        }
      
        private float checkLane()
        {
            return 0;
        }
       
        private float checkEyes()
        {
            return 0;
        }
     
        private void createAlert() { }

        public void StartLoop()
        {

        }

        public void StopLoop() { }
     }
}
