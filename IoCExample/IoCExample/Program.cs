using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using IoCExample.Core;

namespace IoCExample
{
   class Program
    {
        public static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            ConfigContainer();

            var printerProvider = Container.Resolve<IPrinterProvider>();

            printerProvider.Print();
            Console.Read();



            var publicPrinterProvider = Container.Resolve<PublicPrinterProvider>();

            publicPrinterProvider.Print();
            Console.ReadKey();
        }

        private static void ConfigContainer()
        {
            var isLocalMachine = Convert.ToBoolean(ConfigurationManager.AppSettings["isLocalMachine"]);

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies());

            //Injection depends on predefined parameters
            builder.Register<IPrinter>(x =>
            {
                if (isLocalMachine)
                {
                    return x.Resolve<LocalPrinter>();
                }

                return x.Resolve<PublicPrinter>();
            });


            builder.Register<IPrinterProvider>(x => x.Resolve<CustomPrinterProvider>());

            builder.RegisterType<LocalPrinter>().As<IPrinter>();
            builder.RegisterType<PublicPrinter>().Named<IPrinter>("publicPrinter");

            //Injection depends on what type it was injected into - ...WhenInjectedInto
            builder.RegisterType<PublicPrinterProvider>()
                .WithParameter(ResolvedParameter.ForNamed<IPrinter>("publicPrinter"));

            Container = builder.Build();
        }
    }
}
