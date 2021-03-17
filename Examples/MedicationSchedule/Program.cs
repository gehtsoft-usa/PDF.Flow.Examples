using System;
using System.Diagnostics;
using System.IO;

namespace MedicationSchedule
{
    class Program
    {
        static int Main(string[] args)
        {
            Parameters parameters = new Parameters(null, "MedicationSchedule.pdf");

            if (args.Length > 0)
            {
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
                    return 1;
                }
            }

            try
            {
                MedicationScheduleRunner.Run().Build(parameters.file);
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
                var psi = new ProcessStartInfo("cmd", @"/c start " + 
                    parameters.appToView + " " + parameters.file);
                Process.Start(psi);
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
    }
}
