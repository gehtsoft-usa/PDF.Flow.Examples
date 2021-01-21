using System;

namespace AirplaneTicket.Model
{
    public class TicketData
    {
        public string Company { get; set; }
        public string Passenger { get; set; }
        public string Document { get; set; }
        public string TicketNo { get; set; }
        public string Order { get; set; }
        public DateTime Issued { get; set; }
        public string Status { get; set; }


        public override string ToString() 
        {
            return  "TicketData{" + 
                    "Company=" + Company +
                    ", Passenger=" + Passenger +
                    ", Document=" + Document +
                    ", TicketNo=" + TicketNo +
                    ", Order=" + Order +
                    ", Issued=" + Issued +
                    ", Status=" + Status +
                     "}";

        }
    }
}