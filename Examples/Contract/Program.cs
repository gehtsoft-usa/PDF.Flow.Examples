using System;

namespace Gehtsoft.PDFFlow.Contract.ConsoleDemos
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Gehtsoft.PDFFlow.Demos");
            Console.WriteLine("----------------------");
            Console.WriteLine("CONTRACT");
            Console.WriteLine("----------------------");
            GenerateExample();
            Console.WriteLine("");
            Console.WriteLine("Press any key for exit...");
            Console.ReadKey();
        }        
        private static void GenerateExample() => ContractExample.GenerateExample();
    }
}