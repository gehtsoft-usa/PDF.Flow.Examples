namespace BoardingPass.Model
{
    public class TicketData
    {

        public string Company { get; set; }
        public string Site { get; set; }
        public string SiteTitle { get; set; }
        public string Passenger { get; set; }
        public string ETK { get; set; }
        public string RegNo { get; set; }

        public override string ToString() 
        {
            return  "BoardingData{" +  
                    "Company=" + Company +
                    ", Site=" + Site +
                    ", SiteTitle=" + SiteTitle +
                    ", Passenger=" + Passenger +
                    ", ETK=" + ETK +
                    ", RegNo=" + RegNo +
                     "}";
        }
    }
}