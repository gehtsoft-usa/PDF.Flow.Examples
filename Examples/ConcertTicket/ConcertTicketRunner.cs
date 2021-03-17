using ConcertTicketData.Model;
using Gehtsoft.PDFFlow.Builder;
using Newtonsoft.Json;
using System.IO;

namespace ConcertTicket
{
    public static class ConcertTicketRunner
    {
        public static DocumentBuilder Run()
        {
            string ticketJsonFile = CheckFile(Path.Combine("Content", "concert-ticket-data.json"));
            string ticketJsonContent = File.ReadAllText(ticketJsonFile);
            TicketData ticketData =
               JsonConvert.DeserializeObject<TicketData>(ticketJsonContent);

            string jsonFile = CheckFile(Path.Combine("Content", "concert-data.json"));
            string jsonContent = File.ReadAllText(jsonFile);

            ConcertData concertData =
               JsonConvert.DeserializeObject<ConcertData>(jsonContent);

            ConcertTicketBuilder ConcertTicketBuilder = 
                new ConcertTicketBuilder();

           ConcertTicketBuilder.TicketData = ticketData;
           ConcertTicketBuilder.ConcertData = concertData;

           return ConcertTicketBuilder.Build();
        }

        private static string CheckFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new IOException("File not found: " + Path.GetFullPath(file));
            }
            return file;
        }
    }
}