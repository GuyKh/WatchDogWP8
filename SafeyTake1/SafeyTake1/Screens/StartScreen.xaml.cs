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
    public partial class StartScreen : PhoneApplicationPage
    {

        private static Settings _mySettings;

        public static Settings MySettings
        {
            get { return _mySettings; }
            set { _mySettings = value; }
        }

        public StartScreen()
        {
            InitializeComponent();
        }

        private void exitApp() { }

        private void openSettingsScreen() { }

        private void showDriverHistory() { }

        private void showFirstTimeConfiguration() { }

        private void startDriving() { }

        private void testFeatures() { }
    }
}