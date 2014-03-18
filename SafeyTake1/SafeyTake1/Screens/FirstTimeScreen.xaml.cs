using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WatchDOG.Screens
{
    public partial class FirstTime : PhoneApplicationPage
    {
        private int _newSettings;

        public FirstTime()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            StartScreen.isFirstTime = false;
            NavigationService.Navigate(new Uri("/Screens/SettingsScreen.xaml", UriKind.Relative));
        }
    }
}