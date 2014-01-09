using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace SafeyTake1.Screens
{
    public partial class DriverHistoryScreen : PhoneApplicationPage
    {
        public DriverHistoryScreen()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Screens/StartScreen.xaml", UriKind.Relative));
        }
    }
}