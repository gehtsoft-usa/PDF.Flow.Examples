namespace AirplaneTicket.Model
{
    public class FareData
    {
        public string Name { get; set; }
        public double Fare { get; set; }

        public override string ToString() 
        {
            return  "FareData{" +  
                    ", Name=" + Name +
                    ", Fare=" + Fare +
                     "}";
        }
    }
}