using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace WatchDogWebApplicationHost
{
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:13407");
        static readonly Uri _address = new Uri(_baseAddress, "/api/eyedetect");


        static void Main(string[] args)
        {
            HttpSelfHostServer server = null;

            try
            {
                // Configure Server Settings
                HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);

                config.Routes.MapHttpRoute(
                    "API Default", "api/{controller}/{id}",
                    defaults: new {id = RouteParameter.Optional});

                // Create Server
                server = new HttpSelfHostServer(config);

                //Start Listening
                server.OpenAsync().Wait();
                Console.WriteLine("Listening on " + _baseAddress);

                
                Console.WriteLine("Press Enter to exit...");
                Console.WriteLine();

                // Call the web Api and display the result
                HttpClient client = new HttpClient();
                client.GetStringAsync(_baseAddress).ContinueWith(
                    getTask =>
                    {
                        if (getTask.IsCanceled)
                            Console.WriteLine("Request was canceled");

                        else if (getTask.IsFaulted)
                            Console.WriteLine("Request failed: {0}", getTask.Exception);

                        else
                            Console.WriteLine("Client reieved: {0}", getTask.Result);
                    });

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couln't start Server: {0}", ex.GetBaseException().Message);
                Console.WriteLine("Press Enter to exit..");
                Console.ReadLine();
            }
            finally
            {
                if (server != null)
                {
                    // Stop Listening
                    server.CloseAsync().Wait();
                }
            }
        }
    }
}
