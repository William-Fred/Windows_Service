using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
namespace Grade_B_Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string [] args)
        {
            //Used the commented code to debug the service here in visual studio.


            //if (Environment.UserInteractive)
            //{
            //    CopyService service1 = new CopyService();
            //    service1.TestStartupAndStop(args);
            //}
            //else
            //{
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new CopyService()
                };
                ServiceBase.Run(ServicesToRun);
            //}

        }
        }
}
