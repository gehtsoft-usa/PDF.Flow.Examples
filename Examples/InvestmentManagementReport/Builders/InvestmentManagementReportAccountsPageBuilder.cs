using Gehtsoft.PDFFlow.Builder;
using InvestmentManagementReport.Model;
using InvestmentManagementReport.Utils;
using InvestmentManagementReport.Widgets;

namespace InvestmentManagementReport.Builders
{
    internal static class InvestmentManagementReportAccountsPageBuilder
    {
        public static void AddAccountsPage(this SectionBuilder sectionBuilder, InvestmentManagementReportAccountsPage data)
        {
            sectionBuilder
                .InsertPageBreak()
                .AddParagraph(data.Caption)
                    .SetHelveticaOfSize(data.CaptionFontSize)
            .ToSection()
                .AddParagraph(data.SubCaption)
                    .SetHelveticaOfSize(data.SubCaptionFontSize)
            .ToSection()
                .AddAccountTable(data);

            var layout = TwoColumnLayoutWidget.AddTo(sectionBuilder);
            layout.LeftColumn.FillLeftColumn(data);

            NotesWidget.AddTo(sectionBuilder).FillData(data.TableNotes);
        }

        private static void FillLeftColumn(this TableCellBuilder cellBuilder, InvestmentManagementReportAccountsPage data)
        {
            cellBuilder
                .SetHelveticaOfSize(data.BottomBlockFontSize)
                .AddParagraph(data.BottomBlockTitle+"\n\n")
                    .SetBold()
            .ToCell()
                .AddParagraph(data.BottomBlockText);
        }

        private static void AddAccountTable(this SectionBuilder sectionBuilder,
            InvestmentManagementReportAccountsPage data)
        {
            var table = GreenTableWidget.AddTo(sectionBuilder);
            table.FillData(data.AccountsTable);
        }
    }
}
