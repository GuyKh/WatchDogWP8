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
using Microsoft.WindowsAzure.MobileServices;
using WatchDog;
using WatchDOG.Helpers;

namespace WatchDOG.Screens
{
    public partial class DriverHistoryScreen : PhoneApplicationPage
    {
        private MobileServiceCollection<Drive, Drive> drives;
        private IMobileServiceTable<Drive> drivesTable = App.MobileService.GetTable<Drive>();

        public DriverHistoryScreen()
        {
            getDrivesFromServer();
            InitializeComponent();
            populateTable();
        }

        private async void getDrivesFromServer()
        {
            if (Settings.CurrentDriverSetting == null)
                MessageBox.Show("No User configured. Restart the application please");
            else drives = await drivesTable.Where(drv => drv.DriverID == Settings.CurrentDriverSetting.id).ToCollectionAsync();
        }

        private void populateTable()
        {
            if (Settings.CurrentDriverSetting == null)
            {
                MessageBox.Show("No User configured. Restart the application please");
                return;
            }

            if (drives == null)
            {
                MessageBox.Show("No Drives");
                return;
            }

            List<string> driveList = new List<string>();
            foreach (Drive drive in drives)
            {
                driveList.Add(string.Format("Start Time: {0}, End Time: {1}, Average Score {2}", drive.StartTime.ToShortTimeString(), drive.EndTime.ToShortTimeString(), drive.AverageScore));
            }

            List<AlphaKeyGroup<Drive>> DataSource = AlphaKeyGroup<Drive>.CreateGroups(drives,
            System.Threading.Thread.CurrentThread.CurrentUICulture,
            (Drive drive) => { return drive.DriverID; }, true);


            historyListSelector.ItemsSource = DataSource;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
                // Try going to the previous screen first
                this.NavigationService.GoBack();
            else
                NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }
    }
}