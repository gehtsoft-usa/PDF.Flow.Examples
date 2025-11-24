namespace ConcertTicketData.Model
{
    public class TicketData
    {
        public string Eticket { get; set; } = null!;

        public string Admission { get; set; } = null!;
        public string TicketType { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Venue { get; set; } = null!;
        public string Address { get; set; } = null!;



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


