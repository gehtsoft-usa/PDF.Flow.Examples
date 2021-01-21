using AirplaneTicket.Model;
using Gehtsoft.PDFFlow.Builder;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AirplaneTicket
{
    public static class AirplaneTicketRunner
    {
        public static DocumentBuilder Run()
        {
            string ticketJsonFile = CheckFile(
                Path.Combine("Content", "ticket-data.json"));
            string routeJsonFile = CheckFile(
                Path.Combine("Content", "route-data.json"));
            string tripJsonFile = CheckFile(
                Path.Combine("Content", "about-trip.json"));
            string fareJsonFile = CheckFile(
                Path.Combine("Content", "fare-breakdown.json"));
            string helpJsonFile = CheckFile(
                Path.Combine("Content", "help-list.json"));
            string ticketJsonContent = File.ReadAllText(ticketJsonFile);
            string routeJsonContent = File.ReadAllText(routeJsonFile);
            string tripJsonContent = File.ReadAllText(tripJsonFile);
            string fareJsonContent = File.ReadAllText(fareJsonFile);
            string helpJsonContent = File.ReadAllText(helpJsonFile);
            TicketData ticketData = 
                JsonConvert.DeserializeObject<TicketData>(ticketJsonContent);
            List<RouteData> routeData = 
                JsonConvert.DeserializeObject<List<RouteData>>(routeJsonContent);
            List<string> tripData = 
                JsonConvert.DeserializeObject<List<string>>(tripJsonContent);
            List<FareData> fareData =
                JsonConvert.DeserializeObject<List<FareData>>(fareJsonContent);
            List<HelpData> helpData =
                JsonConvert.DeserializeObject<List<HelpData>>(helpJsonContent);
            AirplaneTicketBuilder airplaneTicketBuilder = 
                new AirplaneTicketBuilder();
            airplaneTicketBuilder.TicketData = ticketData;
            airplaneTicketBuilder.RouteData = routeData;
            airplaneTicketBuilder.TripData = tripData;
            airplaneTicketBuilder.FareData = fareData;
            airplaneTicketBuilder.HelpData = helpData;
            return airplaneTicketBuilder.Build();
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