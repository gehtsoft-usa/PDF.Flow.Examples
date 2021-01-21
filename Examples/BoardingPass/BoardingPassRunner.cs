using System.Collections.Generic;
using System.IO;
using BoardingPass.Model;
using Gehtsoft.PDFFlow.Builder;
using Newtonsoft.Json;

namespace BoardingPass
{
    public static class BoardingPassRunner
    {
        public static DocumentBuilder Run()
        {
            string ticketJsonFile = CheckFile(
                Path.Combine("Content", "bp-ticket-data.json"));
            string boardingJsonFile = CheckFile(
                Path.Combine("Content", "boarding-data.json"));
            string whatsNextJsonFile = CheckFile(
                Path.Combine("Content", "whats-next.json"));
            string ticketJsonContent = File.ReadAllText(ticketJsonFile);
            string boardingJsonContent = File.ReadAllText(boardingJsonFile);
            string whatsNextJsonContent = File.ReadAllText(whatsNextJsonFile);
            TicketData ticketData = 
                JsonConvert.DeserializeObject<TicketData>(ticketJsonContent);
            BoardingData boardingData = 
                JsonConvert.DeserializeObject<BoardingData>(boardingJsonContent);
            List<string> whatsNextData = 
                JsonConvert.DeserializeObject<List<string>>(whatsNextJsonContent);
            BoardingPassBuilder boardingPassBuilder = 
                new BoardingPassBuilder();
            boardingPassBuilder.TicketData = ticketData;
            boardingPassBuilder.BoardingData = boardingData;
            boardingPassBuilder.WhatsNextData = whatsNextData;
            return boardingPassBuilder.Build();
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