﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WatchDOG.Alerters
{
    abstract class BackCameraAlerterAbstract : IAlerter
    {
        private void GetBackCameraPhoto()
        {

        }

        public bool GetData()
        {
            return false;
        }




        abstract public float ProcessData(WriteableBitmap picture);
        
    }
}
