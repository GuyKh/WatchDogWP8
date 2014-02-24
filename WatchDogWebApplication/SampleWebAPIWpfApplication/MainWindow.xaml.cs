using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SampleWebAPIWpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _filename;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void selectBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = ofd.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filepathTextBox.Text = ofd.FileName;
                _filename = ofd.FileName;
            }

        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            _filename = filepathTextBox.Text;
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                // HTTP POST
                var image = Image.FromFile(_filename);

                HttpResponseMessage response = await client.PostAsJsonAsync("api/EyeDetect", image);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("OK!");

                }
            }
        }
    }
}
