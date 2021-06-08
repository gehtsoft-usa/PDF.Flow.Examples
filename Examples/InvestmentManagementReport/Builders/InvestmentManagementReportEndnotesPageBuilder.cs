using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Utils;
using InvestmentManagementReport.Model;
using InvestmentManagementReport.Widgets;

namespace InvestmentManagementReport
{
    internal static class InvestmentManagementReportEndnotesPageBuilder
    {
        public static void AddEndnotesPage(this SectionBuilder sectionBuilder, InvestmentManagementReportAdditionalInfo data)
        {
            sectionBuilder
                .InsertPageBreak()
                .AddParagraph(data.Caption)
                    .SetFont(Fonts.Helvetica(float.Parse(data.CaptionFontSize)));

            var layout = TwoColumnLayoutWidget.AddTo(sectionBuilder);
            layout.LeftColumn.FillLeftColumn(data);
            layout.RightColumn.FillRightColumn(data);

        }

        private static void FillLeftColumn(this TableCellBuilder cellBuilder, InvestmentManagementReportAdditionalInfo data)
        {
            cellBuilder
                .SetFont(Fonts.Helvetica(float.Parse(data.TextFontSize)))
                .AddParagraphToCell(data.LeftColumnParagraph1 + "\n")
                .AddParagraphToCell(data.LeftColumnParagraph2 + "\n")
                .AddParagraphToCell(data.LeftColumnParagraph3);
        }

        private static void FillRightColumn(this TableCellBuilder cellBuilder, InvestmentManagementReportAdditionalInfo data)
        {
            cellBuilder
                .SetFont(Fonts.Helvetica(float.Parse(data.TextFontSize)))
                .AddParagraphToCell(data.RightColumnParagraph1 + "\n")
                .AddParagraphToCell(data.RightColumnParagraph2 + "\n")
                .AddParagraphToCell(data.RightColumnParagraph3 + "\n")
                .AddParagraphToCell(data.RightColumnParagraph4);
        }
    }
}
