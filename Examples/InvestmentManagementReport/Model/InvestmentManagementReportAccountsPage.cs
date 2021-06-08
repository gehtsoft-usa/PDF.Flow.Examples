
using System.Collections.Generic;

namespace InvestmentManagementReport.Model
{
    //public class Account
    //{
    //  public int Page,
    //  public string AccountType,
    //  public string AccountName,
    //  public string AccountNumber,
    //  public string BeginningValue,
    //  public string EndingValue
    //}

    public class InvestmentManagementReportAccountsPage
    {
        public string Caption { get; set; }
        public float CaptionFontSize { get; set; }

        public string SubCaption { get; set; }
        public float SubCaptionFontSize { get; set; }

        public string TotalBeginningValue { get; set; }
        public string TotalEndingValue { get; set; }

        //public List<Account> Accounts { get; set; }
        public GreenTable AccountsTable { get; set; }

        public List<NameValuePair> TableNotes { get; set; }

        public string BottomBlockTitle { get; set; }
        public string BottomBlockText { get; set; }
        public float BottomBlockFontSize { get; set; }
    }
}
