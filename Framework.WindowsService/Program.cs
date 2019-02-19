using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Framework.WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.SetServiceName("MyService");
                hostConfigurator.SetDisplayName("My Service");
                hostConfigurator.SetDescription("Does custom logic.");

                hostConfigurator.RunAsLocalSystem();

                hostConfigurator.Service<MyService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(() => new MyService());

                    serviceConfigurator.WhenStarted(service => service.OnStart());
                    serviceConfigurator.WhenStopped(service => service.OnStop());
                });
            });
        }
    }
}
