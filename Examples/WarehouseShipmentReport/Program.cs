using System;
using System.Diagnostics;
using System.IO;

namespace WarehouseShipmentReport
{
    class Program
    {
        static int Main(string[] args)
        {
            Parameters parameters = new Parameters(null, "WarehouseShipmentReport.pdf");

            if (!PrepareParameters(parameters, args))
            {
                return 1;
            }
            
            try
            {
                WarehouseShipmentReportRunner.Run().Build(parameters.file);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 1;
            }
            
            Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                                   + "\" document has been successfully built");
            if (parameters.appToView != null)
            {
                Start(parameters.file, parameters.appToView);
            }
            return 0;
        }
        
        private static void Start(string file, string appToView)
        {
            ProcessStartInfo psi;
            psi = new ProcessStartInfo("cmd", @"/c start " + appToView + " " + file);
            Process.Start(psi);
        }

        private static bool PrepareParameters(Parameters parameters, string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("?")
                    || args[0].Equals("-h")
                    || args[0].Equals("-help")
                    || args[0].Equals("--h")
                    || args[0].Equals("--help")
                    )
                {
                    Usage();
                    return false;
                }
                parameters.file = args[0];
                if (args.Length > 1)
                {
                    parameters.appToView = args[1];
                }
            }

            if (File.Exists(parameters.file))
            {
                try
                {
                    File.Delete(parameters.file);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Can't delete file: " + 
                        Path.GetFullPath(parameters.file));
                    Console.Error.WriteLine(e.Message);
                    return false;
                }
            }
            return true;
        }
        private static void Usage()
        {
            Console.WriteLine("Usage: dotnet run [fullPathToOutFile] [appToView]");
            Console.WriteLine(
                "fullPathToOutFile - a path to the result file, 'WarehouseShipmentReport.pdf' by default");
            Console.WriteLine(
                "appToView - the name of an application to view the file immediately after preparing, by default none app starts");
        }

        internal class Parameters
        {
            public string appToView;
            public string file;
            public Parameters(string appToView, string file)
            {
                this.appToView = appToView;
                this.file = file;
            }
        }
    }
}