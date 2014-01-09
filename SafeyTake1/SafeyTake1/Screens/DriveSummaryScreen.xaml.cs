using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SafeyTake1.DataStructures;

namespace SafeyTake1.Screens
{
    public partial class DriveSummaryScreen : PhoneApplicationPage
    {
        public DriveSummaryScreen()
        {
            InitializeComponent();
        }

        private void populateFields(Drive drive)
        {
            txtDrivingTime.Text = drive.EndTime.Subtract(drive.StartTime).ToString("HH:MM");
            txtDriverAVGScore.Text = drive.Driver.AverageScore.ToString();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate();
        }
    }
}