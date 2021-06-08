using System.IO;
using System.Text.Json;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using InvestmentManagementReport.Builders;
using InvestmentManagementReport.Model;

namespace InvestmentManagementReport
{
    internal static class InvestmentManagementReportBuilder
    {
        public static DocumentBuilder AddInvestmentManagementReport(this DocumentBuilder documentBuilder)
        {
            string headersFile = Path.Combine("Content", "investment_management_report_headers.json");
            string titlePageFile = Path.Combine("Content", "investment_management_report_title_page.json");
            string accountsPageFile = Path.Combine("Content", "investment_management_report_accounts_page.json");
            string blankPageFile = Path.Combine("Content", "investment_management_report_blank_page.json");
            string endnotesPageFile = Path.Combine("Content", "investment_management_report_endnotes_page.json");
            string finalPagePart1File = Path.Combine("Content", "investment_management_report_final_page_part1.json");
            string finalPagePart2File = Path.Combine("Content", "investment_management_report_final_page_part2.json");
            var headers = 
                JsonSerializer.Deserialize<InvestmentManagementReportHeaders>(File.ReadAllText(headersFile));
            var titlePageData =
                JsonSerializer.Deserialize<InvestmentManagementReportTitlePage>(File.ReadAllText(titlePageFile));
            var accountsPageData =
                JsonSerializer.Deserialize<InvestmentManagementReportAccountsPage>(File.ReadAllText(accountsPageFile));
            var blankPageData =
                JsonSerializer.Deserialize<InvestmentManagementReportBlankPage>(File.ReadAllText(blankPageFile));
            var endnotesPageData =
                JsonSerializer.Deserialize<InvestmentManagementReportAdditionalInfo>(File.ReadAllText(endnotesPageFile));
            var finalPagePart1Data =
                JsonSerializer.Deserialize<InvestmentManagementReportAdditionalInfo>(File.ReadAllText(finalPagePart1File));
            var finalPagePart2Data =
                JsonSerializer.Deserialize<InvestmentManagementReportAdditionalInfo>(File.ReadAllText(finalPagePart2File));

            SectionBuilder section = documentBuilder
                .AddSection()
                .SetSize(PaperSize.Letter)
                .SetMargins(36f, 38f, 36f, 28f)
                .SetOrientation(PageOrientation.Landscape);
            
            section.AddRepeatingAreas(headers);
            section.AddTitlePage(titlePageData);
            section.AddAccountsPage(accountsPageData);           
            section.AddBlankPage(blankPageData, 3);
            section.AddEndnotesPage(endnotesPageData);
            section.AddFinalPage(finalPagePart1Data, finalPagePart2Data);

            return documentBuilder;
        }
    }
}