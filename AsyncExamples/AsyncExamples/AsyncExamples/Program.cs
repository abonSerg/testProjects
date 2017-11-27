using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine($"Start main. Thread {Thread.CurrentThread.ManagedThreadId}");

            var list = Enumerable.Range(1, 10).ToList();

           list.AsParallel().ForAll(i=>AsyncMethod(i));

          // var t = list.Select(AsyncMetho2d);


       //    var mainTAsk = Task.WhenAll(t);
        //   mainTAsk.GetAwaiter().GetResult();

            Console.ReadKey();
        }

        private static async Task AsyncMethod(int i)
        {
            //Console.WriteLine($"Start async operation. Thread {Thread.CurrentThread.ManagedThreadId}");

            await Task.Run(() =>
            {
                Console.WriteLine($"Task run, {Thread.CurrentThread.ManagedThreadId}");
            });

            //Console.WriteLine($"End async operation. Thread {Thread.CurrentThread.ManagedThreadId}");
        }

        private static Task AsyncMetho2d(int i)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Task run, {Thread.CurrentThread.ManagedThreadId}");
            });
        }

    }
}
