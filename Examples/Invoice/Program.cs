using System;

namespace Gehtsoft.PDFFlow.Invoice.ConsoleDemos
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Gehtsoft.PDFFlow.Demos");
            Console.WriteLine("----------------------");
            Console.WriteLine("INVOICE");
            Console.WriteLine("----------------------");
            GenerateExample();
            Console.WriteLine("");
            Console.WriteLine("Press any key for exit...");
            Console.ReadKey();
        }        
        private static void GenerateExample() => InvoiceExample.GenerateExample();
    }
}