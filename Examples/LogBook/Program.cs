using System;
using System.Diagnostics;

namespace Gehtsoft.PDFFlow.LogBook.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.WriteLine("Gehtsoft.PDFFlow.Demos");
            Console.WriteLine("----------------------");
            Console.WriteLine("OPERATIONS LOG BOOK");
            Console.WriteLine("----------------------");
            ConsoleDemo.Start("LogBook.pdf");
            Console.WriteLine("100% done");
            Console.WriteLine("");
            sw.Stop();
            Console.WriteLine("Time taken: {0} seconds", Math.Round(sw.Elapsed.TotalSeconds,2));
            Console.WriteLine("");
            Console.WriteLine("Press any key for exit...");
            Console.ReadKey();
        }
    }
}
