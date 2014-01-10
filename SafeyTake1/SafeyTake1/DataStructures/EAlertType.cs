using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeyTake1.DataStructures
{
    enum EAlertType
    {
        // Using binary values (001, 010, 100) for combinations.

        EyeDetectionAlert = 1,
        DistanceAlert = 2,
        LaneCrossingAlert = 4
    }
}
