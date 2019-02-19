using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using log4net.Config;
using Topshelf;

namespace Framework.WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            ContainerBuilder containerBuilder= new ContainerBuilder();

            containerBuilder.RegisterType<MyService>()
                .AsSelf()
                .InstancePerLifetimeScope();

            IContainer container = containerBuilder.Build();

            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.SetServiceName("MyService");
                hostConfigurator.SetDisplayName("My Service");
                hostConfigurator.SetDescription("Does custom logic.");

                hostConfigurator.RunAsLocalSystem();
                hostConfigurator.UseLog4Net();

                hostConfigurator.Service<MyService>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(hostsetting => container.Resolve<MyService>());

                    serviceConfigurator.WhenStarted(service => service.OnStart());
                    serviceConfigurator.WhenStopped(service => service.OnStop());
                });
            });
        }
    }
}
