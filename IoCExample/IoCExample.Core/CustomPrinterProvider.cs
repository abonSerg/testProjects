using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCExample.Core
{
    public class CustomPrinterProvider : IPrinterProvider
    {
        private readonly IPrinter printer;

        public CustomPrinterProvider(IPrinter printer)
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
}
