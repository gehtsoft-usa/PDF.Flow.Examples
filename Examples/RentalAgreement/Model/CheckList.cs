namespace RentalAgreement.Model
{
    public class CheckList
    {

        public string Name { get; set; } = "";
        public string[] Items { get; set; } = { };

        public override string ToString() 
        {
            return "CheckList{" +  
                   "Name=" + Name +
                    ", Items=" + Items +
                   "}";
        }
    }
}