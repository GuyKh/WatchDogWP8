using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using WatchDogWcf;

namespace WatchDogServiceHost
{
    class Program
    {
        static ServiceHost host = null;


        static void StartService()
        {
            host = new ServiceHost(typeof(WatchDogService));
            /***********
             * if you don't want to use App.Config for the web service host, 
                 * just uncomment below:
             ***********
                 host.AddServiceEndpoint(new ServiceEndpoint(
                 ContractDescription.GetContract(typeof(IStudentEnrollmentService)),
                 new WSHttpBinding(), 
                 new EndpointAddress("http://localhost:8732/awesomeschoolservice"))); 
             **********/
            host.Open();
        }


        static void CloseService()
        {
            if (host.State != CommunicationState.Closed)
            {
                host.Close();
            }
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Starting Watch DOG Service ....");
            StartService();
            Console.WriteLine("Watch DOG Service is running....");
            foreach (var channelDispatcher in host.ChannelDispatchers)
            {
                Console.WriteLine(channelDispatcher.Listener.Uri.ToString());
            }
            Console.ReadKey();

            CloseService();
        }
    
    }
}
