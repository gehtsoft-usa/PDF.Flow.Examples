namespace PaymentAgreement.Model
{
    public class AgreementText
    {

        public string Header { get; set; }
        public string[] Text { get; set; }

        public override string ToString() 
        {
            return  "AgreementText{" +  
                    "Header=" + Header +
                    ", Text=" + Text +
                     "}";
        }
    }
}