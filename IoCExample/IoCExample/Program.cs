using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace IoCExample
{
    public interface IPrinter
    {
        void Print();
    }

    public class LocalPrinter : IPrinter
    {
        public void Print()
        {
            Console.WriteLine("Printing on the LOCAL prtinter...");
        }
    }

    public class PublicPrinter : IPrinter
    {
        public void Print()
        {
            Console.WriteLine("Printing on the PUBLIC prtinter...");
        }
    }

    public class PrinterProvider : IPrinterProvider
    {
        private readonly IPrinter printer;

        public PrinterProvider(IPrinter printer)
        {
            this.printer = printer;
        }

        public void Print()
        {
            printer.Print();
        }
    }

    public interface IPrinterProvider
    {
        void Print();
    }


    class Program
    {
        public static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            ConfigContainer();

            var printerProvider = Container.Resolve<IPrinterProvider>();

            printerProvider.Print();
            Console.Read();
        }

        private static void ConfigContainer()
        {
            var isLocalMachine = Convert.ToBoolean(ConfigurationManager.AppSettings["isLocalMachine"]);

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies());

            builder.Register<IPrinter>(x =>
            {
                if (isLocalMachine)
                {
                    return x.Resolve<LocalPrinter>();
                }

                return x.Resolve<PublicPrinter>();
            });

            builder.Register<IPrinterProvider>(x => x.Resolve<PrinterProvider>());

            Container = builder.Build();
        }
    }
}
