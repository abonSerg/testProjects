using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoCExample.Core;
using Ninject;

namespace IoCExample.Ninject
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel(new CustomNinjectModule());

            var printerProvider = kernel.Get<CustomPrinterProvider>();
            var publicPrinterProvider = kernel.Get<PublicPrinterProvider>();
            var privatePrinterProvider = kernel.Get<PrivatePrinterProvider>();

            printerProvider.Print(); //Local
            Console.ReadKey(); 

            publicPrinterProvider.Print();
            Console.ReadKey(); //Public

            privatePrinterProvider.Print();
            Console.ReadKey(); //Local
        }
    }
}
