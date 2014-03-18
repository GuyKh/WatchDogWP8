using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WatchDOG.DataStructures;

namespace WatchDOG.Alerters
{
    class LaneDetectorAlerter : BackCameraAlerterAbstract
    {
        public override double ProcessData(WriteableBitmap picture)
        {
            return -1;
        }

        public override EAlertType GetAlerterType()
        {
            return EAlertType.LaneCrossingAlert;
        }
    }
}
