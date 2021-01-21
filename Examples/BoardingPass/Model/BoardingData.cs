using System;

namespace BoardingPass.Model
{
    public class BoardingData
    {

        public string Flight { get; set; }
        public string DepartureAirport { get; set; }
        public string DepartureAbvr { get; set; }
        public string BoardingGate { get; set; }
        public DateTime BoardingTill { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime Arrival { get; set; }
        public string ArrivalAirport { get; set; }
        public string ArrivalAbvr { get; set; }
        public string Class { get; set; }
        public string ClassAdd { get; set; }
        public string Seat { get; set; }

        public override string ToString() 
        {
            return  "BoardingData{" +  
                    "Flight=" + Flight +
                    ", DepartureAirport=" + DepartureAirport +
                    ", DepartureAbvr=" + DepartureAbvr +
                    ", BoardingGate=" + BoardingGate +
                    ", BoardingTill=" + BoardingTill +
                    ", DepartureTime=" + DepartureTime +
                    ", Arrival=" + Arrival +
                    ", ArrivalAirport=" + ArrivalAirport +
                    ", ArrivalAbvr=" + ArrivalAbvr +
                    ", Class=" + Class +
                    ", ClassAdd=" + ClassAdd +
                    ", Seat=" + Seat +
                     "}";
        }
    }
}