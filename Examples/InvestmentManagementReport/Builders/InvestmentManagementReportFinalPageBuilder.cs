using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using InvestmentManagementReport.Model;
using InvestmentManagementReport.Utils;
using InvestmentManagementReport.Widgets;

namespace InvestmentManagementReport
{
    internal static class InvestmentManagementReportFinalPageBuilder
    {
        public static void AddFinalPage(this SectionBuilder sectionBuilder, InvestmentManagementReportAdditionalInfo dataPart1, InvestmentManagementReportAdditionalInfo dataPart2)
        {
            sectionBuilder
                .InsertPageBreak()
                .AddPart(dataPart1)
                .AddLineToSection(XUnit.FromPercent(100), 2)
                .AddPart(dataPart2);
        }

        private static SectionBuilder AddPart(this SectionBuilder sectionBuilder, InvestmentManagementReportAdditionalInfo data)
        {
            sectionBuilder                
                .AddParagraph(data.Caption)
                    .SetHelveticaOfSize(data.CaptionFontSize);

            var layout = TwoColumnLayoutWidget.AddTo(sectionBuilder);
            layout.LeftColumn.FillLeftColumn(data);
            layout.RightColumn.FillRightColumn(data);
            return sectionBuilder;
        }

        private static void FillLeftColumn(this TableCellBuilder cellBuilder, InvestmentManagementReportAdditionalInfo data)
        {
            cellBuilder
                .SetHelveticaOfSize(data.TextFontSize)
                .AddParagraphToCell(data.LeftColumnParagraph1 + "\n")
                .AddParagraphToCell(data.LeftColumnParagraph2 + "\n")
                .AddParagraphToCell(data.LeftColumnParagraph3);
        }

        private static void FillRightColumn(this TableCellBuilder cellBuilder, InvestmentManagementReportAdditionalInfo data)
        {
            cellBuilder
                .SetHelveticaOfSize(data.TextFontSize)
                .AddParagraphToCell(data.RightColumnParagraph1 + "\n")
                .AddParagraphToCell(data.RightColumnParagraph2 + "\n")
                .AddParagraphToCell(data.RightColumnParagraph3 + "\n")
                .AddParagraphToCell(data.RightColumnParagraph4);
        }
    }
}
