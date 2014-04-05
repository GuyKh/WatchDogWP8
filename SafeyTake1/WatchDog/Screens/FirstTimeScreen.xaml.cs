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
using System.Threading.Tasks;


namespace WatchDOG.Screens
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public partial class FirstTime : PhoneApplicationPage
    {
        private Settings _newSettings;
        private bool register = true;
        private MobileServiceCollection<Driver, Driver> drivers;
        private IMobileServiceTable<Driver> driversTable = App.MobileService.GetTable<Driver>();

        public FirstTime()
        {
            InitializeComponent();
            getUsersFromServer();

        }

        private async void getUsersFromServer()
        {
            drivers = await driversTable.ToCollectionAsync();
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

                txtName.Visibility = Visibility.Visible;
                txtboxName.Visibility = Visibility.Visible;
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
                    Settings.CurrentDriverSetting = LoadDriver(txtboxUser.Text);
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
                else
                {
                    MessageBox.Show("Error inserting the User to the Database. Try again");
                    return;
                }
            }
            StartScreen.isFirstTime = false;

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }

        private Driver LoadDriver(string username)
        {
            Driver driver = drivers.Where(_driver => _driver.Username == username).FirstOrDefault();
            if (driver != null)
                return driver;
            else throw new InvalidOperationException("Non existing driver");
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
            
            _driver = null;
            
            if (drivers == null)
                getUsersFromServer();

            var _existing = drivers.Where(driver => driver.Username == username);
            if (_existing == null || !_existing.Any())
            {
                var tempDriver = new Driver(name, username, password);
                try
                {
                    _driver = tempDriver;
                    await driversTable.InsertAsync(tempDriver);               
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                return;
            }
            else
            {
                MessageBox.Show("User name already exists");

            }
            
        }



        /// <summary>
        /// Validate login credentials with the DB
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool ValidateCredentials(string username, string password)
        {
            var _driver = drivers.Where(driver => driver.Username == username).FirstOrDefault();
            if (_driver != null)
                return _driver.Password == password;
            return false;
        }
    }
}