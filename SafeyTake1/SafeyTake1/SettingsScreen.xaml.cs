using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Reflection;


namespace SafeyTake1
{
    public partial class SettingsScreen : PhoneApplicationPage
    {
        List<String> colorList;
        string colorString = "";
        public SettingsScreen()
        {


            InitializeComponent();

            var colors = new List<ColorSelectModel>();
            var accentColor = (Color)Resources["PhoneAccentColor"];
            colors.Add(new ColorSelectModel("Accent Color", accentColor));
            colors.AddRange(
                    typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                        .Where(p => p.PropertyType == typeof(Color))
                        .Select(p => new ColorSelectModel(p.Name, (Color)p.GetValue(typeof(Colors), null))));
            colorListPicker.ItemsSource = colorListPicker.ItemsSource ??
                new ObservableCollection<ColorSelectModel>(colors);

            
        }
    }

    public class ColorSelectModel
    {
        public ColorSelectModel(string text, Color color)
        {
            this.Text = text;
            this.Color = color;
            this.ColorBrush = new SolidColorBrush(color);
        }
        public string Text { get; set; }
        public Color Color { get; set; }
        public SolidColorBrush ColorBrush { get; set; }
    }
}