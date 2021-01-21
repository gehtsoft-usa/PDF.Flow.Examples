using System;

namespace AirplaneTicket.Model
{
    public class RouteData
    {

        public string Flight { get; set; }
        public string FlightCompany { get; set; }
        public string FlightPlaner { get; set; }
        public DateTime Departure { get; set; }
        public string DepartureAirport { get; set; }
        public DateTime Arrival { get; set; }
        public string ArrivalAirport { get; set; }
        public string Class { get; set; }
        public string ClassAdd { get; set; }
        public string Baggage { get; set; }
        public string BaggageAdd { get; set; }
        public DateTime CheckIn { get; set; }
        public string CheckInAirport { get; set; }


        public override string ToString()
        {
            return  "RouteData{"+
                    "Flight=" + Flight +
                    ", FlightCompany=" + FlightCompany +
                    ", FlightPlaner=" + FlightPlaner +
                    ", Departure=" + Departure +
                    ", DepartureAirport=" + DepartureAirport +
                    ", Arrival=" + Arrival +
                    ", ArrivalAirport=" + ArrivalAirport +
                    ", Class=" + Class +
                    ", ClassAdd=" + ClassAdd +
                    ", Baggage=" + Baggage +
                    ", BaggageAdd=" + BaggageAdd +
                    ", CheckIn=" + CheckIn +
                    ", CheckInAirport=" + CheckInAirport +
                    "}";
        }
    }
}
