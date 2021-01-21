namespace RentalAgreement.Model
{
    public class PartyData
    {

        public string Party { get; set; } = "";
        public string KnownAs { get; set; } = "";
        public string Name { get; set; } = "";
        public string NameExt { get; set; } = "";
        public string MailAddress { get; set; } = "";
        public string Phone { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string Signer { get; set; } = "";


        public override string ToString() 
        {
            return "AgreementText{" +  
                   "Party=" + Party +
                    ", KnownAs=" + KnownAs +
                    ", Name=" + Name +
                    ", NameExt=" + NameExt +
                    ", MailAddress=" + MailAddress +
                    ", Phone=" + Phone +
                    ", EmailAddress=" + EmailAddress +
                    ", Signer=" + EmailAddress +
                   "}";
        }
    }
}