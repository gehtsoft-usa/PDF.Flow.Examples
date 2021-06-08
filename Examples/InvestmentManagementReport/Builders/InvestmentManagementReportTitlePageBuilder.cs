using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;
using InvestmentManagementReport.Model;
using InvestmentManagementReport.Widgets;

namespace InvestmentManagementReport
{
    internal static class InvestmentManagementReportTitlePageBuilder
    {
        private static readonly Color GrayColor = Color.FromHtml("#4D4E4C");
        private static readonly FontBuilder FNT_7_5I = Fonts.Helvetica(7.5f).SetOblique();
        private static readonly FontBuilder FNT_9 = Fonts.Helvetica(9f);
        private static readonly FontBuilder FNT_9B_GRAY = Fonts.Helvetica(9f).SetBold().SetColor(GrayColor);
        private static readonly FontBuilder FNT_11 = Fonts.Helvetica(11f);
        private static readonly FontBuilder FNT_11_75B = Fonts.Helvetica(11.75f).SetBold();
        private static readonly FontBuilder FNT_11_75B_GRAY = Fonts.Helvetica(11.75f).SetBold().SetColor(GrayColor);
        private static readonly FontBuilder FNT_10B = Fonts.Helvetica(10f).SetBold();
        private static readonly FontBuilder FNT_18_25B = Fonts.Helvetica(18.25f).SetBold();

        public static void AddTitlePage(this SectionBuilder sectionBuilder, InvestmentManagementReportTitlePage data)
        {
            var layout = TwoColumnLayoutWidget.AddTo(sectionBuilder);
            layout.LeftColumn.FillLeftColumn(data);
            layout.RightColumn.FillRightColumn(data);
        }

        private static void FillLeftColumn(this TableCellBuilder cellBuilder, InvestmentManagementReportTitlePage data)
        {
            TableBuilder leftColumnLayoutTable = cellBuilder
                .AddTable()
                .AddColumnPercentToTable("", 100f);
            
            leftColumnLayoutTable.AddRow().AddCell().AddEnvelopeNumber(data);
            leftColumnLayoutTable.AddRow().AddCell().AddAddressAndName(data);
            leftColumnLayoutTable.AddRow().AddCell().AddContactInformation(data);
            leftColumnLayoutTable.AddRow().AddCell().AddWelcomeBlock(data);
        }

        private static void AddEnvelopeNumber(this TableCellBuilder cellBuilder,
            InvestmentManagementReportTitlePage data)
        {
            cellBuilder
                .SetHorizontalAlignment(HorizontalAlignment.Center)
                    .AddParagraph($"Envelope # {data.EnvelopeNumber}")
                    .SetFont(FNT_9)
                    .SetMarginTop(35f);
        }

        private static void AddAddressAndName(this TableCellBuilder cellBuilder,
            InvestmentManagementReportTitlePage data)
        {
            cellBuilder
                .SetPadding(30f, 16f, 0f, 0f)
                .SetFont(FNT_9);
            cellBuilder.AddParagraph(data.FullName);
            cellBuilder.AddParagraph(data.AddressFirstLine);
            cellBuilder.AddParagraph(data.AddressSecondLine);
        }

        private static void AddContactInformation(this TableCellBuilder cellBuilder,
            InvestmentManagementReportTitlePage data)
        {
            cellBuilder
                .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.Solid)
                .SetBorderWidth(2f)
                .AddParagraph("Contact Information")
                .SetFont(FNT_11)
                .SetMarginTop(125f);
            
            TableBuilder contactInformationTable = cellBuilder
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .AddTable()
                            .SetBorderStroke(Stroke.None)
                            .AddColumnPercentToTable("", 50f)
                            .AddColumnPercentToTable("", 50f)
                            .SetMarginTop(7f);
            
            TableRowBuilder rowBuilder = contactInformationTable
                .AddRow()
                .SetFont(FNT_9);
            TableCellBuilder leftColumnCell = rowBuilder.AddCell();
            TableCellBuilder rightColumnCell = rowBuilder.AddCell().SetHorizontalAlignment(HorizontalAlignment.Right);

            foreach (var (name, value) in data.ContactInformation)
            {
                leftColumnCell.AddParagraph(name);
                rightColumnCell.AddParagraph(value);
            }
        }

        private static void AddWelcomeBlock(this TableCellBuilder cellBuilder,
            InvestmentManagementReportTitlePage data)
        {
            cellBuilder
                .AddParagraph(data.WelcomeTitle)
                .SetMarginTop(61f)
                .SetFont(FNT_10B);
            cellBuilder
                .AddParagraph(data.WelcomeText)
                .SetFont(FNT_9);
        }
        
        private static void FillRightColumn(this TableCellBuilder cellBuilder,
            InvestmentManagementReportTitlePage data)
        {
            TableBuilder rightColumnLayoutTable = cellBuilder
                .AddTable()
                .AddColumnPercentToTable("", 100f);
            
            rightColumnLayoutTable.AddRow().AddCell().AddPortfolioValueBlock(data);
            rightColumnLayoutTable.AddRow().AddCell().AddChangeFromLastPeriodBlock(data);
            rightColumnLayoutTable.AddRow().AddCell().AddPortfolioTable(data);
            if (data.TableNotes != null && data.TableNotes.Count > 0)
                rightColumnLayoutTable.AddRow().AddCell().AddPortfolioTableNotes(data);
        }

        private static void AddPortfolioValueBlock(this TableCellBuilder cellBuilder,
            InvestmentManagementReportTitlePage data)
        {
            cellBuilder
                .AddTable()
                .SetBorderStroke(Stroke.None)
                .AddColumnPercentToTable("", 50f)
                .AddColumnPercentToTable("", 50f)
                    .AddRow()
                    .SetVerticalAlignment(VerticalAlignment.Bottom)
                        .AddCell("Your Portfolio Value:")
                        .SetFont(FNT_11_75B)
                    .ToRow()
                        .AddCell(data.PortfolioValue)
                        .SetFont(FNT_18_25B)
                        .SetHorizontalAlignment(HorizontalAlignment.Right);
        }
        
        private static void AddChangeFromLastPeriodBlock(this TableCellBuilder cellBuilder,
            InvestmentManagementReportTitlePage data)
        {
            cellBuilder
                .AddTable()
                .SetBorderStroke(Stroke.None)
                .AddColumnPercentToTable("", 50f)
                .AddColumnPercentToTable("", 50f)
                    .AddRow()
                    .SetVerticalAlignment(VerticalAlignment.Bottom)
                        .AddCell("Change from Last Period:")
                        .SetFont(FNT_9B_GRAY)
                    .ToRow()
                        .AddCell(data.PortfolioValue)
                        .SetPadding(0f, 1.5f, 0f, 0f)
                        .SetFont(FNT_11_75B_GRAY)
                        .SetHorizontalAlignment(HorizontalAlignment.Right);
        }

        private static void AddPortfolioTable(this TableCellBuilder cellBuilder,
            InvestmentManagementReportTitlePage data)
        {
            cellBuilder.SetPadding(0f, 20f, 0f, 0f);
            var table = GreenTableWidget.AddTo(cellBuilder);
            table.FillData(data.PortfolioTable);
        }

        private static void AddPortfolioTableNotes(this TableCellBuilder cellBuilder,
            InvestmentManagementReportTitlePage data)
        {
            NotesWidget.AddTo(cellBuilder).FillData(data.TableNotes);
        }
    }
}