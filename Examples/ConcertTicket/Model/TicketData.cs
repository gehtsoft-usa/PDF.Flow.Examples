namespace ConcertTicketData.Model
{
    public class TicketData
    {
        public string Eticket { get; set; }

        public string Admission { get; set; }
        public string TicketType { get; set; }
        public string Price { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public string Address { get; set; }



        public override string ToString()
        {
            return "TicketData{" +
                    "Eticket=" + Eticket +
                     ", Admission=" + Admission +
                    ", TicketType=" + TicketType +
                    ", Price=" + Price +
                    ", Name=" + Name +
                    ", Venue=" + Venue +
                    ", Address=" + Address +
                     "}";
        }


    }


}


