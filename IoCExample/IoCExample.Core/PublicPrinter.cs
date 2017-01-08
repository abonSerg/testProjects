using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCExample.Core
{
    public class PublicPrinter : IPrinter
    {
        public void Print()
        {
            Console.WriteLine("Printing on the PUBLIC prtinter...");
        }
    }

    public interface IPrinter
    {
        void Print();
    }
}
