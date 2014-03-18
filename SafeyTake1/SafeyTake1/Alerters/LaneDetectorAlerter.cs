using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WatchDOG.Alerters
{
    class LaneDetectorAlerter : BackCameraAlerterAbstract
    {
        public override float ProcessData(WriteableBitmap picture)
        {
            return -1;
        }
    }
}
