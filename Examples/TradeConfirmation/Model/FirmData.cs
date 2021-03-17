using System.Collections.Generic;

namespace TradeConfirmationData.Model
{
    public class FirmData
    {
        public string DocumentDate { get; set; }
        public List<string> FirmContact { get; set; }
        public string TaxInfo { get; set; }
        public string ExpensesInfo { get; set; }
        public string MoreInfo { get; set; }

        public override string ToString()
        {
            return "{" + ", taxinfo=" + TaxInfo +
                    ", expensesinfo=" + ExpensesInfo +
                    ", moreinfo=" + MoreInfo +
                    ", firmcontact: [" + FirmContact.ToString() + "]" +
                     "}";
        }
    }
}


