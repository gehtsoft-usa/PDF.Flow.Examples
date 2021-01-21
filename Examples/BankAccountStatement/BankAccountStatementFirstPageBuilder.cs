using System;
using BankAccountStatement.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using static BankAccountStatement.BankAccountStatementBuilder;
using static BankAccountStatement.Helper;

namespace BankAccountStatement
{
    internal class BankAccountStatementFirstPageBuilder : BankAccountStatementPageBuilder
    {
        public BankAccountStatementFirstPageBuilder(StatementInfo statementInfo) : 
            base(statementInfo)
        {
        }

        internal void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            AddTitle(sectionBuilder.AddHeaderToBothPages(136).AddTable(), 
                20f, 4, AddTitleWithBankName);
            AddTwoPanels(sectionBuilder, 30f, AddCompanyName, AddQuestions);
            AddOptions(sectionBuilder, 30f);
            AddAdvt(sectionBuilder, 12f);
            AddTwoPanels(sectionBuilder, 0f, AddActivitySummary, AddAccountInfo);
        }

        private void AddOptions(SectionBuilder sectionBuilder, float bottomMargin)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .SetWidth(PageWidth)
                .AddColumnPercentToTable("", 63).AddColumnPercentToTable("", 37)
                .SetMarginBottom(bottomMargin);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .ApplyStyle(StyleBuilder.New()
                    .SetBorderTop(2f, Stroke.Solid, Color.Black));
            AddYourBusiness(cellBuilder);
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .ApplyStyle(StyleBuilder.New()
                    .SetBorderLeft(10f, Stroke.Solid, Color.White)
                    .SetBorderTop(2f, Stroke.Solid, Color.Black))
                .SetPadding(3.5f, 0, 0, 0);
            AddOptionsToCell(cellBuilder);
        }

        private void AddAdvt(SectionBuilder sectionBuilder, float bottomMargin)
        {
            sectionBuilder
                .AddLine(PageWidth, 0.5f, Stroke.Solid).SetMarginBottom(12);
            AddParagraph(sectionBuilder, statementInfo.Advt, FNT7_5, bottomMargin);
        }

        private void AddActivitySummary(TableCellBuilder outerCellBuilder)
        {
            outerCellBuilder.AddTable(FillActivitySummaryTable);
        }

        private void FillActivitySummaryTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(80))
                .SetBorder(Stroke.None)
                .SetAltRowStyleBackColor(Color.White)
                .AddColumnPercentToTable("", 75).AddColumnPercent("", 25);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .ApplyStyle(
                    StyleBuilder.New()
                        .SetBorderLeftStroke(Stroke.None)
                        .SetBorderTop(
                            0.5f, Stroke.Solid, Color.Black)
                        .SetBorderRightStroke(Stroke.None)
                        .SetBorderBottomStroke(Stroke.None)
                        .SetFont(FNT10_5B)
                )
                .SetColSpan(2)
                .AddParagraphToCell("Activity summary");
            FillActivitySummaryTableRow(tableBuilder, new Field[] {
                        new Field("Beginning balance on " +
                            statementInfo.DateBegin.ToString(
                                "MMMM dd, yyyy", DocumentLocale),
                            "$" + StatementInfo.ToString(statementInfo.BeginningBalance)),
                        new Field("Deposits/Credits",
                            StatementInfo.ToString(statementInfo.Deposits)),
                        new Field("Withdrawals/Debits",
                            StatementInfo.ToString(statementInfo.Withdrawals)),
                        new Field("Ending balance on " +
                            statementInfo.DateEnd.ToString("MMMM dd, yyyy",
                            DocumentLocale),
                            "$" + StatementInfo.ToString(statementInfo.EndingBalance)),
                        new Field("Average ledger balance this period",
                            "$" + StatementInfo.ToString(statementInfo.AverageBalance))
                    });
        }

        private void FillActivitySummaryTableRow(TableBuilder tableBuilder, Field[] fields)
        {
            StyleBuilder style1l = StyleBuilder.New().SetBorderStroke(Stroke.None);
            StyleBuilder style1r = StyleBuilder.New().SetBorderStroke(Stroke.None);
            StyleBuilder style2l = StyleBuilder.New()
                .SetBorderRightStroke(Stroke.None)
                .SetBorderTop(2f, Stroke.Solid, Color.Black)
                .SetBorderLeftStroke(Stroke.None)
                .SetBorderBottomStroke(Stroke.None);
            StyleBuilder style2r = StyleBuilder.New()
                .SetBorderRightStroke(Stroke.None)
                .SetBorderTop(2f, Stroke.Solid, Color.Black)
                .SetBorderLeftStroke(Stroke.None)
                .SetBorderBottomStroke(Stroke.None);
            for (int i = 0, l = fields.Length; i < l; i++)
            {
                StyleBuilder stylel = i != 3 ? style1l : style2l;
                StyleBuilder styler = i != 3 ? style1r : style2r;
                var font = i != 3 ? FNT7_2 : FNT7_2B;
                Field field = fields[i];
                var rowBuilder = tableBuilder.AddRow();
                var cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .ApplyStyle(stylel)
                    .SetFont(font);
                if (i == l - 1)
                {
                    cellBuilder.SetPadding(0, 15, 0, 0);
                }
                cellBuilder
                    .AddParagraph(field.name);
                cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .ApplyStyle(styler)
                    .SetFont(font)
                    .SetHorizontalAlignment(HorizontalAlignment.Right);
                if (i == l - 1)
                {
                    cellBuilder.SetPadding(0, 16, 0, 0);
                }
                cellBuilder
                    .AddParagraph(field.value);
            }
        }

        private void AddAccountInfo(TableCellBuilder outerCellBuilder)
        {
            AddParagraph(outerCellBuilder, "Account number:", FNT7_2, 6);
            AddParagraph(outerCellBuilder, statementInfo.CompanyName, FNT7_2B);
            AddParagraph(outerCellBuilder, "New York account terms and conditions apply", 
                FNT7, 6);
            AddParagraph(outerCellBuilder, "For Direct Deposit use", FNT7_2);
            AddParagraph(outerCellBuilder, "Routing Number (RTN): " +
                            statementInfo.DepositRTN, FNT7, 6);
            AddParagraph(outerCellBuilder, "For Wire Transfers use", FNT7_2);
            AddParagraph(outerCellBuilder, "Routing Number(RTN): " + 
                            statementInfo.WireRTN, FNT7_2);
        }

        private void AddTwoPanels (SectionBuilder sectionBuilder, float bottomMargin, 
            Action<TableCellBuilder> LeftPanelAction, 
            Action<TableCellBuilder> RightPanelAction)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .SetWidth(PageWidth)
                .AddColumnPercentToTable("", 62).AddColumnPercentToTable("", 38)
                .SetMarginBottom(bottomMargin);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .ApplyStyle(StyleBuilder.New()
                    .SetBorderRight(2f, Stroke.Solid, Color.Black));
            LeftPanelAction(cellBuilder);
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                    .SetBorderStroke(Stroke.None)
                    .SetPadding(8, 0, 0, 0);
            RightPanelAction(cellBuilder);
        }

        private void AddYourBusiness(TableCellBuilder cellBuilder)
        {
            AddParagraph(cellBuilder, "Your Business and "
                            + statementInfo.BankName, FNT13B);
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFontColor(Color.Blue)
                        .SetFontName("Helvetica")
                        .SetFontSize(7.5f)
                        .SetFontUnderline(Stroke.Solid, Color.Blue)
                        .SetBorder(0, Stroke.None, Color.White)
                    )
                .SetFont(FNT7_5)
                .AddTextToParagraph("The plans you establish today will shape your business far into the future. The heart of the planning process is your business plan. Take the time now to build a strong foundation.\nFind out more at ")
                .AddUrlToParagraph("https://" + statementInfo.BusinessPlan, 
                    statementInfo.BusinessPlan)
                .AddTextToParagraph(".");
        }
        private void AddOptionsToCell(TableCellBuilder cellBuilder)
        {
            AddParagraph(cellBuilder, "Account options", FNT10_5B);
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFontColor(Color.Blue)
                        .SetFontName("Helvetica")
                        .SetFontSize(7f)
                        .SetFontUnderline(Stroke.Solid, Color.Blue)
                    )
                .SetFont(FNT7)
                .AddTextToParagraph(
                    "A check mark in the box indicates you have these convenient services with your account(s). Go to ")
                .AddUrlToParagraph("https://" + 
                statementInfo.AccountOptions, statementInfo.AccountOptions)
                .AddTextToParagraph(
                    " or call the number above if you have questions or if you would like to add new services");
            Field[] fields = new Field[]
            {
                new Field("Business Online Banking", "true"),
                new Field("Online Statements", "true"),
                new Field("Business Bill Pay", "true"),
                new Field("Business Spending Report", "true"),
                new Field("Overdraft Protection", "true"),
            };
            AddCheckBoxes(cellBuilder, fields);
        }

        private void AddCheckBoxes(TableCellBuilder outerCellBuilder, Field[] fields)
        {
            outerCellBuilder.AddTable(tableBuilder => 
            {
                tableBuilder
                    .SetBorder(Stroke.None)
                    .SetWidth(XUnit.FromPercent(100))
                    .SetMarginLeft(4.5f)
                    .SetMarginTop(10)
                    .AddColumnPercentToTable("", 60).AddColumnPercent("", 40);
                foreach(Field field in fields)
                {
                    var tableRowBuilder = tableBuilder.AddRow();
                    tableRowBuilder
                        .SetBorderStroke(Stroke.None);
                    var cellBuilder = tableRowBuilder.AddCell();
                    cellBuilder.SetFont(FNT7_2).AddParagraph(field.name);
                    cellBuilder = tableRowBuilder.AddCell();
                    cellBuilder.SetHorizontalAlignment(HorizontalAlignment.Right);
                    cellBuilder.SetFont(FNTZ8).AddParagraphToCell(CHECK_BOX);
                }
            });
        }

        private void AddCompanyName(TableCellBuilder cellBuilder)
        {
            AddParagraph(cellBuilder, 19.2f, statementInfo.CompanyName +
                    "\n" + statementInfo.CompanyAddress, FNT8_9);
        }

        private void AddQuestions(TableCellBuilder cellBuilder)
        {
            AddParagraph(cellBuilder, "Questions?", FNT10_5B, 6f);
            AddParagraph(cellBuilder, "Available by phone 24 hours a day, 7 days a week:\n" +
                "Telecommunications Relay Services calls accepted", FNT7_9);
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder.SetMarginBottom(6);
            paragraphBuilder.AddText(statementInfo.PhoneFree).SetFont(FNT9_8B);
            paragraphBuilder.AddText("(" + statementInfo.Phone + ")").SetFont(FNT7_5);
            cellBuilder.AddTable(FillCallCentersTable);
        }

        private void FillCallCentersTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetBorder(Stroke.None)
                .SetWidth(XUnit.FromPercent(70))
                .AddColumnPercentToTable("", 40).AddColumnPercent("", 60);
            AddCallCenters(tableBuilder, new Field[] {
                        new Field("TTY:", statementInfo.TTY)
                        , new Field("Online:", statementInfo.Online)
                        , new Field("Write:", statementInfo.White)
                    });
        }

        private void AddCallCenters(TableBuilder tableBuilder, Field[] fields)
        {
            for (int i = 0, l = fields.Length; i < l; i++)
            {
                Field field = fields[i];
                var rowBuilder = tableBuilder.AddRow();
                rowBuilder
                    .SetFont(FNT7_9)
                    .AddCellToRow(field.name)
                    .AddCellToRow(field.value);
                if (i == l - 1)
                {
                    break;
                }
                rowBuilder = tableBuilder.AddRow();
                rowBuilder.SetFont(FNT7_9);
                var cellBuilder = rowBuilder.AddCell(" ");
                cellBuilder.SetColSpan(2);
            }
        }

        private class Field
        {
            public String name;
            public String value;
            public Field(String name, String value)
            {
                this.name = name;
                this.value = value;
            }
        }
    }
}