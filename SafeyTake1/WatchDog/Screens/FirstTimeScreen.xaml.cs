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

namespace WatchDOG.Screens
{
    public partial class FirstTime : PhoneApplicationPage
    {
        private Settings _newSettings;
        private bool register = true;

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

                Settings.CurrentDriver = CreateNewDriver(txtboxName.Text, txtboxUser.Text, txtboxPassword.Text);
                Settings.Loaded = true;
            }

            Settings.SaveSettingsToDisk();

            StartScreen.isFirstTime = false;
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }

        private Driver LoadDriver(string username)
        {
            return null;
        }

        /// <summary>
        /// Create a new user from the username and password.
        /// </summary>
        /// <param name="name">Driver's Name</param>
        /// <param name="username">Driver's Username</param>
        /// <param name="password">Driver's Password</param>
        private Driver CreateNewDriver(string name, string username, string password)
        {
            // ToDo: Insert to the DB.

            Driver _driver = new Driver(name, username, password);
            return _driver;
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