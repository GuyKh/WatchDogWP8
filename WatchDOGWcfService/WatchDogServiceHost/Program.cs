using System;
using System.Collections.Generic;
using System.Linq;
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

                 host.AddServiceEndpoint(new ServiceEndpoint(
                 ContractDescription.GetContract(typeof(IWatchDogService)),
                 new WSHttpBinding(), 
                 new EndpointAddress("http://localhost:8765/WatchDOG"))); 

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
            StartService();

            Console.WriteLine("Watch DOG Service is running....");
            Console.WriteLine("At - " + host.BaseAddresses.ToString());
            Console.WriteLine("Press any key to close");
            Console.ReadKey();

            CloseService();
        }
    
    }
}
