using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCExample.Core
{
    public class LocalPrinter : IPrinter
    {
        public void Print()
        {
            Console.WriteLine("Printing on the LOCAL prtinter...");
        }
    }
}
