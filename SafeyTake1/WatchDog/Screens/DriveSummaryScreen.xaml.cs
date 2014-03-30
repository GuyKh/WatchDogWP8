using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WatchDOG.DataStructures;
using WatchDOG.Helpers;

namespace WatchDOG.Screens
{
    public partial class DriveSummaryScreen : PhoneApplicationPage
    {
        public DriveSummaryScreen()
        {
            InitializeComponent();
        }

        private void populateFields(Drive drive)
        {
            txtDrivingTime.Text = drive.EndTime.Subtract(drive.StartTime).ToString("g");
            txtDriverAVGScore.Text = drive.Driver.AverageScore.ToString();
            AlertEvent commonAlert = drive.Events.GroupBy(evnt => evnt.AlertType)
                .OrderByDescending(type => type.Count()).SelectMany(g => g).FirstOrDefault();

            txtCommonHazzard.Text = commonAlert != null ? 
                WatchDogHelper.GetEnumDescription(commonAlert.AlertType) : 
                "--";
            txtSafetyScore.Text = drive.Events.Count() > 0 ? drive.Events.Average(evnt => evnt.AlertLevel).ToString() : "0";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
                NavigationService.GoBack();
            }
            else 
                Application.Current.Terminate();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Drive lastDrive = (Drive)PhoneApplicationService.Current.State["CurrentDrive"];
            populateFields(lastDrive);
            
        }


    }
}