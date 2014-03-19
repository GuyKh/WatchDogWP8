using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WatchDOG.DataStructures;

namespace WatchDOG.Alerters
{
    interface IAlerter
    {
        /// <summary>
        /// Method gets the data from the relevant sensor to local variables for future work.
        /// </summary>
        /// <returns></returns>
        bool GetData();

        /// <summary>
        /// Processes the data in the local variables, makes a safety analysis and returns the safety level (0-100).
        /// Will return -1 if analysis fails.
        /// </summary>
        /// <returns>Calculated value of the safety level based on this alerter</returns>
        double ProcessData(WriteableBitmap picture);

        /// <summary>
        /// Which type is this alerter.
        /// </summary>
        EAlertType GetAlerterType();
    }
}
