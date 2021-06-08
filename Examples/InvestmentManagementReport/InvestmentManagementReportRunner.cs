using Gehtsoft.PDFFlow.Builder;

namespace InvestmentManagementReport
{
    public static class InvestmentManagementReportRunner
    {
        public static DocumentBuilder Run()
        {
            return DocumentBuilder.New()
                .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
                .AddInvestmentManagementReport();
        }
    }
}