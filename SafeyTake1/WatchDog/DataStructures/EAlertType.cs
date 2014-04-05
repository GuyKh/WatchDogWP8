using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDOG.DataStructures
{
    public enum EAlertType
    {
        // Using binary values (001, 010, 100) for combinations.
        [Description("Eyes Closed")]
        EyeDetectionAlert = 1,
        
        [Description("Too Close")]
        DistanceAlert = 2,

        [Description("Lane Crossing")]
        LaneCrossingAlert = 4
    }
}
