using System;
using System.Collections.Generic;
using System.IO;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace LogBook
{
    public static class LogBookRunner
    {
        private static string ProjectDir;
        private static string OperationJsonFile;
        private static string OperationJsonContent;
        private static IEnumerable<Operation> OperationData;
        private static PDFSettings Options;

        #region Constructors

        static LogBookRunner()
		{
            ProjectDir = Directory.GetCurrentDirectory();
            OperationJsonFile = Path.Combine(ProjectDir, "Content", "operations_log.json");
            OperationJsonContent = File.ReadAllText(OperationJsonFile);
            OperationData = JsonConvert.DeserializeObject<List<Operation>>(OperationJsonContent);
            int _id = 1;
            DateTime? maxDate = DateTime.ParseExact("1/1/1970", "d/M/yyyy", null);
            DateTime? minDate = DateTime.ParseExact("1/1/3000", "d/M/yyyy", null);
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
            Options = new PDFSettings
            {
                SectionMargins = new Box(20, 20, 20, 20),
                PaperSize = PaperSize.A4,
                Orientation = PageOrientation.Landscape,
                HeaderOdd = "Operations Log Book Printout",                
                HeaderEven = "Operations Log Book Printout",
                BookName = "Operations Log Book",
                DateOfPrint = DateTime.ParseExact("1/1/2021", "d/M/yyyy", null),
                DateRangeStart = minDate,
                DateRangeEnd = maxDate,
                NumberOfRecords = _id-1,
                DateFormat = "MM/dd/yyyy",
                DiscardedStrikethroughStroke = Stroke.Solid,
                DiscardedStrikethroughMode = StrikethroughMode.TextOnly,
                DiscardedStrikethroughColor = Color.Red,
                StartingPage = 1,
                DisplayDateRange = true
            };			
		}
		
		#endregion Constructors

        public static void Run(string pdfFileName)
        { 
            using (IStreamCoordinator coordinator = new LogBookCoordinator(pdfFileName, Options, startingPage: 0))
            {                
                coordinator.Input(OperationData);
            }
        }       
    }
}