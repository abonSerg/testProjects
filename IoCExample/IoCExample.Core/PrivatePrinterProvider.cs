namespace IoCExample.Core
{
    public class PrivatePrinterProvider : IPrinterProvider
    {
        private readonly IPrinter printer;

        public PrivatePrinterProvider(IPrinter printer)
        {
            this.printer = printer;
        }

        public void Print()
        {
            printer.Print();
        }
    }
}