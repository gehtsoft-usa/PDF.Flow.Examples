using System;

namespace TutorialC
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Gehtsoft.PDFFlow.Demos");
            Console.WriteLine("----------------------");
            Console.WriteLine("C TUTORIAL");
            Console.WriteLine("----------------------");
            GenerateExample();
            Console.WriteLine("");
            Console.WriteLine("Press any key for exit...");
            Console.ReadKey();
        }        
        private static void GenerateExample() => TutorialCRunner.Run().Build("TutorialC.pdf");
    }
}
