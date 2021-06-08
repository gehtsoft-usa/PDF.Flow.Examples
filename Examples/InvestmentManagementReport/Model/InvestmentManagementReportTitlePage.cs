using System.Collections.Generic;

namespace InvestmentManagementReport.Model
{
    public class InvestmentManagementReportTitlePage
    {
        public string EnvelopeNumber { get; set; }
        public string AddressFirstLine { get; set; }
        public string AddressSecondLine { get; set; }
        public string FullName { get; set; }
        public string WelcomeTitle { get; set; }
        public string WelcomeText { get; set; }
        public string PortfolioValue { get; set; }
        public string ChangeFromLastPeriod { get; set; }
        public List<NameValuePair> ContactInformation { get; set; }
        public GreenTable PortfolioTable { get; set; }
        public List<NameValuePair> TableNotes { get; set; }
    }
}