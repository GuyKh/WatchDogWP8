using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeyTake1.DataStructures
{
    class Speed
    {
        private static float _currentSpeed;

        public float CurrentSpeed
        {
            get { return _currentSpeed; }
            set { _currentSpeed = value; }
        }

        /// <summary>
        /// this function returns the current 
        /// </summary>
        /// <returns></returns>
        public bool GetSpeed()
        {
            
            return true;
        }

        public bool IsMoving()
        {
            return true;
        }
    }
}
