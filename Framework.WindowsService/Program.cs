using Autofac;
using Autofac.Extras.Quartz;
using log4net;
using log4net.Config;
using MyJobs;
using Topshelf;

namespace Framework.WindowsService
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            ContainerBuilder containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<MyService>()
                .AsSelf()
                .InstancePerLifetimeScope();

            containerBuilder.Register(c => LogManager.GetLogger(typeof(Program)))
                .As<ILog>();

            containerBuilder.RegisterModule(new QuartzAutofacFactoryModule());

            //TODO: Verify the typeof() argument once
            containerBuilder.RegisterModule(new QuartzAutofacJobsModule(typeof(MyJob).Assembly));

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
