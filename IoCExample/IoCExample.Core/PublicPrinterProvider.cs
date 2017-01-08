namespace IoCExample.Core
{
    public class PublicPrinterProvider : IPrinterProvider
    {
        private readonly IPrinter printer;

        public PublicPrinterProvider(IPrinter printer)
        {
            this.printer = printer;
        }

        public void Print()
        {
            printer.Print();
        }
    }
}