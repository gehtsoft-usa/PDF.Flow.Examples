using System;
using System.Diagnostics;
using System.IO;

namespace AviationChecklist
{
    class Program
    {
        static int Main(string[] args)
        {
            Parameters parameters = new Parameters(null, "AviationChecklist.pdf");

            PrepareParameters(parameters, args);

            try
            {
                AviationChecklistRunner.Run().Build(parameters.file);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
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

        private static void Start(string file, string appToView)
        {
            ProcessStartInfo psi = new ProcessStartInfo
                                    ("cmd", @"/c start " + appToView + " " + file);
            Process.Start(psi);
        }

        private static void PrepareParameters(Parameters parameters, string[] args)
        {
            if (args.Length > 0)
            {
                parameters.file = args[0];
                if (args.Length > 1)
                {
                    parameters.appToView = args[1];
                }
            }
        }
    }
}
