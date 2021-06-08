using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Utils;
using InvestmentManagementReport.Model;

namespace InvestmentManagementReport
{
    internal static class InvestmentManagementReportBlankPageBuilder
    {
        public static void AddBlankPage(this SectionBuilder sectionBuilder, InvestmentManagementReportBlankPage data, int timesRepeat = 1)
        {
            for (int i = 0; i < timesRepeat; i++)
                sectionBuilder
                    .InsertPageBreak()
                    .AddParagraph(data.Caption)
                        .SetFont(Fonts.Helvetica(float.Parse(data.CaptionFontSize)));
        }
    }
}
