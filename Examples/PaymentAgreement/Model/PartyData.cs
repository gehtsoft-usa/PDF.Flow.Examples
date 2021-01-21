namespace PaymentAgreement.Model
{
    public class PartyData
    {

        public string Party { get; set; } = "";
        public string Name { get; set; } = "";
        public string MailAddress { get; set; } = "";

        public override string ToString() 
        {
            return "AgreementText{" +  
                   "Party=" + Party +
                   ", Name=" + Name +
                   ", MailAddress=" + MailAddress +
                   "}";
        }
    }
}