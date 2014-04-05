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


namespace WatchDOG.Screens
{
    public partial class FirstTime : PhoneApplicationPage
    {
        private Settings _newSettings;
        private bool register = true;
        private MobileServiceCollection<Driver, Driver> Drivers;
        private IMobileServiceTable<Driver> DriversTable = App.MobileService.GetTable<Driver>();

        public FirstTime()
        {
            InitializeComponent();
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            SwitchStatus();
        }

        /// <summary>
        /// Switch UI statuses between Login and Register
        /// </summary>
        private void SwitchStatus()
        {
            if (register)
            {
                headline.Text = "Login";
                txtVerify.Visibility = Visibility.Collapsed;
                txtboxVerify.Visibility = Visibility.Collapsed;
                txtName.Visibility = Visibility.Collapsed;
                txtboxName.Visibility = Visibility.Collapsed;
                btnState.Content = "Sign Up Mode";
                register = false;
            }
            else
            {
                headline.Text = "Register";
                txtVerify.Visibility = Visibility.Visible;
                txtboxVerify.Visibility = Visibility.Visible;
                btnState.Content = "Login Mode";
                register = true;
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Terminate();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (!register)
                if (!ValidateCredentials(txtboxUser.Text, txtboxPassword.Text))
                {
                    MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK);
                    return;
                }
                else
                {
                    Driver driver = LoadDriver(txtboxUser.Text);
                }
            else
            {
                if (txtboxPassword.Text != txtboxVerify.Text)
                {
                    MessageBox.Show("Both passwords do not match", "Error", MessageBoxButton.OK);
                    return;
                }
                if (txtboxPassword.Text == "")
                {
                    MessageBox.Show("Password cannot be empty", "Error", MessageBoxButton.OK);
                    return;
                }
                
                CreateNewDriver(txtboxName.Text, txtboxUser.Text, txtboxPassword.Text);
                
                if (_driver != null)
                    Settings.CurrentDriverSetting = _driver; 
            }
            StartScreen.isFirstTime = false;

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }

        private Driver LoadDriver(string username)
        {
            return null;
        }

        private async void InsertDriver(Driver todoItem)
        {
            //// This code inserts a new Drivers into the database. When the operation completes
            //// and Mobile Services has assigned an Id, the item is added to the CollectionView
            await DriversTable.InsertAsync(todoItem);

            Drivers.Add(todoItem);
        }

        private Driver _driver;

        /// <summary>
        /// Create a new user from the username and password.   
        /// </summary>
        /// <param name="name">Driver's Name</param>
        /// <param name="username">Driver's Username</param>
        /// <param name="password">Driver's Password</param>
        private async void CreateNewDriver(string name, string username, string password)
        {
            // ToDo: Insert to the DB.

            _driver = null;
            var _existing = await DriversTable.Where(driver => driver.Username == username).ToListAsync();

            if (!_existing.Any())
            {
                _driver = new Driver(name, username, password);

                InsertDriver(_driver);
                
                return;
            }
            else
            {
                MessageBox.Show("User name already exists");

            }
            return;
        }

        /// <summary>
        /// Validate login credentials with the DB
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool ValidateCredentials(string username, string password)
        {
            //ToDo: Check with the DB for existance of a username with this specific password.

            return true;
        }
    }
}