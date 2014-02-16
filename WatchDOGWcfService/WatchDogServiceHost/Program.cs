using System;
using System.ServiceModel;
using WatchDogService;

namespace WatchDogServiceHost
{
    class Program
    {
        static ServiceHost _host;


        static void StartService()
        {
            _host = new ServiceHost(typeof(WatchDogWcfService));
            /***********
             * if you don't want to use App.Config for the web service host, 
                 * just uncomment below:
             ***********
                 host.AddServiceEndpoint(new ServiceEndpoint(
                 ContractDescription.GetContract(typeof(IStudentEnrollmentService)),
                 new WSHttpBinding(), 
                 new EndpointAddress("http://localhost:8732/awesomeschoolservice"))); 
             **********/
            _host.Open();
        }


        static void CloseService()
        {
            if (_host.State != CommunicationState.Closed)
            {
                _host.Close();
            }
        }

        static void Main()
        {

            Console.WriteLine("Starting Watch DOG Service ....");
            StartService();
            Console.WriteLine("Watch DOG Service is running....");
            foreach (var channelDispatcher in _host.ChannelDispatchers)
            {
                if (channelDispatcher.Listener != null) 
                    Console.WriteLine(channelDispatcher.Listener.Uri.ToString());
            }
            Console.ReadKey();

            CloseService();
        }
    
    }
}
