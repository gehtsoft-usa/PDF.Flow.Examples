using System;
using System.Collections.Generic;
using System.IO;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Newtonsoft.Json;

namespace Gehtsoft.PDFFlow.LogBook.ConsoleDemo
{
    public class ConsoleDemo
    {
        private static string ProjectDir;
        private static string OperationJsonFile;
        private static string OperationJsonContent;

        public static void Start(string fileName)
        {
            var pdfFile = Path.Combine(Environment.CurrentDirectory, fileName);
            ProjectDir = Directory.GetCurrentDirectory();
            OperationJsonFile = Path.Combine(ProjectDir, "Content", "operations_log.json");
            OperationJsonContent = File.ReadAllText(OperationJsonFile);
            IEnumerable<Operation> OperationData = JsonConvert.DeserializeObject<List<Operation>>(OperationJsonContent);
            int _id = 1;
            DateTime? maxDate = DateTime.ParseExact("1/1/1970", "d/M/yyyy", null);
            DateTime? minDate = DateTime.ParseExact("1/1/3000", "d/M/yyyy", null);
            bool flag_25_done = false;
            bool flag_50_done = false;
            bool flag_75_done = false;
            foreach (Operation item in OperationData)
            {
                item.Code = _id.ToString();
                _id++;
                if ((item.Received != null) && (item.Received < minDate))
                {
                    minDate = item.Received;
                }
                if ((item.Sent != null) && (item.Sent < minDate))
                {
                    minDate = item.Sent;
                }
                if ((item.Received != null) && (item.Received > maxDate))
                {
                    maxDate = item.Received;
                }
                if ((item.Sent != null) && (item.Sent > maxDate))
                {
                    maxDate = item.Sent;
                }
            }
            var options = new PDFSettings
            {
                Padding = new Box(20, 40, 30, 0),
                PaperSize = PaperSize.A4,
                Orientation = PageOrientation.Landscape,
                HeaderLineStyle = Stroke.Solid,
                HeaderOdd = "Operations Log Book Printout",                
                HeaderEven = "Operations Log Book Printout",
                PageChanged = args =>
                {
                    var pageNumber = args.PageNumber;
                },
                RowLayout = args =>
                {
                    if ((!flag_25_done) && (args.Row.Index / (_id - 1)) >= 0.25)
                    {
                        flag_25_done = true;
                        Console.WriteLine("25% done");
                    }
                    if ((!flag_50_done) && (args.Row.Index / (_id - 1)) >= 0.5)
                    {
                        flag_50_done = true;
                        Console.WriteLine("50% done");
                    }
                    if ((!flag_75_done) && (args.Row.Index / (_id - 1)) >= 0.75)
                    {
                        flag_75_done = true;
                        Console.WriteLine("75% done");
                    }
                },
                BookName = "Operations Log Book",
                DateOfPrint = DateTime.Now,
                DateRangeStart = minDate,
                DateRangeEnd = maxDate,
                NumberOfRecords = _id-1,
                DateFormat = "MM/dd/yyyy",
                DiscardedLineStyle = Stroke.Solid,
                DiscardedLineDrawMode = LinethroughMode.TextOnly,
                DiscardedLineColor = Color.Black,
                StartingPage = 1,
                DisplayDateRange = true
            };

            using (IStreamCoordinator coordinator = new LogBookCoordinator(pdfFile, options, startingPage: 0))
            {                
                if (File.Exists(pdfFile))
                    File.Delete(pdfFile);
                coordinator.Input(OperationData);
                Console.WriteLine("Generating file "+pdfFile);
                Console.WriteLine("");
            }
            
        }       
    }
}