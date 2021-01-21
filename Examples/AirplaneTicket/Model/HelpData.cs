namespace AirplaneTicket.Model
{
    public class HelpData
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString() 
        {
            return  "HelpData{" +  
                    ", Name=" + Name +
                    ", Value=" + Value +
                     "}";
        }
    }
}