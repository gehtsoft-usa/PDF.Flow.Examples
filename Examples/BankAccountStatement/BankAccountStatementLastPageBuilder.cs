using BankAccountStatement.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using System;
using static BankAccountStatement.Helper;
using static BankAccountStatement.BankAccountStatementBuilder;

namespace BankAccountStatement
{
    internal class BankAccountStatementLastPageBuilder : BankAccountStatementPageBuilder
    {
        public BankAccountStatementLastPageBuilder(StatementInfo statementInfo) : 
            base(statementInfo)
        {
        }

        internal void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            var headerBuilder = sectionBuilder.AddHeaderToBothPages(80);
            AddTitle(headerBuilder.AddTable(), 20f, 4, AddCommonPageTitle);
            headerBuilder.AddLine(PageWidth, 2f, Stroke.Solid);
            var footerBuilder = sectionBuilder.AddFooterToBothPages(8);
            var paragraphBuilder = footerBuilder.AddParagraph();
            paragraphBuilder
                .SetAlignment(HorizontalAlignment.Left)
                .SetFont(FNT5_6)
                .AddTextToParagraph("©2020 " + statementInfo.BankName + 
                    " , " + statementInfo.BankNameState + 
                    " All rights reserved. Member FDIC.");
            headerBuilder = sectionBuilder.AddHeaderToBothPages(90);
            AddGeneralStatementPolicies(headerBuilder);
            headerBuilder.AddLine(PageWidth, 0.5f, Stroke.Solid);
            AddAccountBalanceCalculationWorksheet(sectionBuilder);
        }

        private void AddGeneralStatementPolicies(RepeatingAreaBuilder builder)
        {
            AddParagraph(
                builder,
                "General statement policies for " + statementInfo.BankName,
                FNT11_B
            );
            builder
                .AddTableToRepeatingArea(FillGeneralStatementPoliciesTable);
        }

        private void FillGeneralStatementPoliciesTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetBorder(Stroke.None)
                .SetMarginTop(10)
                .SetMarginBottom(6)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 50)
                .AddColumnPercent("", 50);
            var tableRowBuilder = tableBuilder.AddRow();
            var paragraphBuilder = tableRowBuilder.AddCell().AddParagraph();
            paragraphBuilder
                .AddTextToParagraph("Notice", FNT7_2B)
                .AddTextToParagraph(": " +
                statementInfo.BankName + ", " +
                statementInfo.BankNameState +
                " may furnish information about accounts belonging to individuals, including sole proprietorships, to consumer reporting agencies.  If this applies to you, you have the right to dispute the accuracy of information that we have reported by writing to us at: "
                + statementInfo.ReportAddress + ".",
                FNT7_2);
            tableRowBuilder
                .AddCell("You must describe the specific information that is inaccurate or in dispute and the basis for any dispute with supporting documentation.  In the case of information that relates to an identity theft, you will need to provide us with an identity theft report.")
                .SetFont(FNT7_2)
                .SetPadding(8, 0, 0, 0);
        }

        private void AddAccountBalanceCalculationWorksheet(SectionBuilder sectionBuilder)
        {
            float repAreaWidth = PageWidth * 0.5f - 1;
            sectionBuilder.AddRptAreaLeftToBothPages(repAreaWidth, AddInstructions);
            sectionBuilder.AddRptAreaRightToBothPages(repAreaWidth, AddBalanceCalc);
        }

        private void AddInstructions(RepeatingAreaBuilder builder)
        { 
            AddAboutAccountBalanceCalculationWorksheet(builder);
            AddInstuctionAboutENTER(builder);
            AddInstuctionAboutADD(builder);
            AddInstuctionAboutSUBTRACT(builder);
        }

        private void AddAboutAccountBalanceCalculationWorksheet(
            RepeatingAreaBuilder builder)
        {
            AddParagraph(
                builder,
                "Account Balance Calculation Worksheet",
                FNT11_B
            );
            string[] items = GetAboutAccountBalanceCalculationItems();
            AddNumberedListToParagraph(
                builder,
                items,
                FNT7_2,
                14
            );
        }

        private void AddInstuctionAboutENTER(RepeatingAreaBuilder builder)
        {
            AddInstructionTitle(builder, 19.2f, "ENTER");
            var tableBuilder = builder.AddTable();
            ConfigureDescriptionFormTable(tableBuilder);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .AddCellToRow(FillEndingBalanceENTER)
                .AddCellToRow(AddEndingBalanceForm);
        }

        private void AddInstuctionAboutADD(RepeatingAreaBuilder builder)
        {
            AddInstructionTitle(builder, 6, "ADD");
            var tableBuilder = builder.AddTable();
            ConfigureDescriptionFormTable(tableBuilder);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .AddCellToRow(FillInstructionAnyDeposits)
                .AddCellToRow(AddAnyDepositsForm);
        }

        private void AddInstuctionAboutSUBTRACT(RepeatingAreaBuilder builder)
        {
            AddInstructionTitle(builder, 6, "SUBTRACT");
            var tableBuilder = builder.AddTable();
            ConfigureDescriptionFormTable(tableBuilder);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .AddCellToRow(FillInstructionAnyWithdrawals)
                .AddCellToRow(AddAnyWithdrawalsForm);
        }

        private string[] GetAboutAccountBalanceCalculationItems()
        {
            return new string[]
            {
                "Use the following worksheet to calculate your overall account balance.",
                "Go through your register and mark each check, withdrawal, ATM transaction, payment, deposit or other credit listed on your statement. Be sure that your register shows any interest paid into your account and any service charges, automatic payments or ATM transactions withdrawn from your account during this statement period.",
                "Use the chart to the right to list any deposits, transfers to your account, outstanding checks, ATM withdrawals, ATM payments or any other withdrawals (including any from previous months) which are listed in your register but not shown on your statement."
            };
        }

        private void AddBalanceCalc(RepeatingAreaBuilder builder)
        {
            var tableBuilder = builder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(90))
                .SetBorder(Stroke.Solid, Color.Black, 0.5f)
                .SetAltRowStyleBackColor(Color.White)
                .AddColumnPercentToTable("", 20)
                .AddColumnPercentToTable("", 60)
                .AddColumnPercentToTable("", 20)
                .SetAlignment(HorizontalAlignment.Right);
            var rowBuilder = tableBuilder.AddRow();
            foreach (string head in new string[] {
                                                    "Number",
                                                    "Items Outstanding",
                                                    "Amount"
                                                    })
            {
                rowBuilder
                    .AddCell(head).ApplyStyle(
                                StyleBuilder.New()
                                    .SetBorderStroke(Stroke.None)
                                    .SetFont(FNT7B)
                                    .SetHorizontalAlignment(
                                        HorizontalAlignment.Center)
                            );
            }
            for(int i = 0; i < 26; i++)
            {
                rowBuilder = tableBuilder.AddRow();
                rowBuilder.AddCellToRow(" ").AddCellToRow(" ").AddCellToRow(" ");
            }
            rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder
                .AddCell("Total amount  $");
            cellBuilder
                .ApplyStyle(
                    StyleBuilder.New()
                        .SetBorderStroke(Stroke.None)
                        .SetFont(FNT7B)
                        .SetHorizontalAlignment(
                            HorizontalAlignment.Right)
                        .SetVerticalAlignment(
                            VerticalAlignment.Bottom)
                )
                .SetColSpan(2)
                .SetPadding(0, 0, 4, 0);
            rowBuilder
                .AddCellToRow(" ");
        }
        private void AddInstructionTitle(RepeatingAreaBuilder builder, float topMargin, string title)
        {
            AddParagraph(builder, topMargin, title, FNT7_2B);
        }

        private void ConfigureDescriptionFormTable(TableBuilder tableBuilder, 
            float descriptionColumnPercent = 64, float formColumnPercent = 36)
        {
            tableBuilder
                .SetMarginTop(1f)
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetAltRowStyleBackColor(Color.White)
                .AddColumnPercentToTable("", descriptionColumnPercent)
                .AddColumnPercentToTable("", formColumnPercent)
                .SetAlignment(HorizontalAlignment.Left);
        }

        private void FillEndingBalanceENTER(TableCellBuilder cellBuilder)
        {
            var tableBuilder = cellBuilder.AddTable();
            ConfigureDescriptionTable(tableBuilder, 6, 94);
            AddDescriptionRow(
                tableBuilder, 
                16, 
                "A.", 
                new string[] { 
                    "The ending balance shown on your statement" 
                }, 
                AddDescriptionToCell
            );
        }

        private void FillInstructionAnyDeposits(TableCellBuilder cellBuilder)
        {
            var tableBuilder = cellBuilder.AddTable();
            ConfigureDescriptionTable(tableBuilder, 6, 94);
            AddDescriptionRow(
                tableBuilder,
                64,
                "B.",
                new string[] {
                    "Any deposits listed in your register or transfers into your account which are not shown on your statement."
                },
                AddDescriptionToCell
            );
            AddDescriptionRow(
                tableBuilder,
                46,
                " ",
                new string[] {
                    "CALCULATE THE SUBTOTAL",
                    "(Add Parts A and B)"
                },
                AddCalculateDescriptionToCell
            );
        }

        private void FillInstructionAnyWithdrawals(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddTable(tableBuilder =>
            {
                ConfigureDescriptionTable(tableBuilder, 6, 94);
                AddDescriptionRow(
                    tableBuilder,
                    24,
                    "C.",
                    new string[] {
                        "The total outstanding checks and withdrawals from the chart above .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  .  ."
                    },
                    AddWithdrawalsDescriptionToCell
                );
                AddDescriptionRow(
                    tableBuilder,
                    36,
                    " ",
                    new string[] {
                        "CALCULATE THE ENDING BALANCE",
                        "(Part A + Part B - Part C)",
                        "This amount should be the same as the current balance shown in your check register .  .  .  .  .  .  .  .  .  ."
                    },
                    AddCalculateEndingBalanceDescriptionToCell
                );
            });
        }

        private void ConfigureDescriptionTable(TableBuilder tableBuilder, 
            float firstColumnPercent, float secondColumnPercent)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetAltRowStyleBackColor(Color.White)
                .AddColumnPercentToTable("", firstColumnPercent)
                .AddColumnPercentToTable("", secondColumnPercent)
                .SetAlignment(HorizontalAlignment.Left);
        }

        private void AddDescriptionRow(TableBuilder tableBuilder, float minHeight, 
            string listIndex, string[] text, 
            Action<TableCellBuilder, string[]> addTextAction)
        {
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .ApplyStyle(
                    StyleBuilder.New()
                        .SetTableCellMinHeight(minHeight)
                );
            rowBuilder.AddCell(listIndex).SetFont(FNT7_2B);
            addTextAction(rowBuilder.AddCell(), text);
        }

        private void AddDescriptionToCell(TableCellBuilder cellBuilder, string[] text)
        {
            cellBuilder
                .SetFont(FNT7_2)
                .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.Dotted)
                .SetBorderColor(Color.Black);
            foreach (string item in text)
            {
                cellBuilder.AddParagraphToCell(item);
            }
        }

        private void AddWithdrawalsDescriptionToCell(TableCellBuilder cellBuilder, 
            string[] text)
        {
            cellBuilder
                .SetFont(FNT7_2)
                .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.None);
            foreach (string item in text)
            {
                ParagraphBuilder paragraphBuilder = cellBuilder.AddParagraph();
                paragraphBuilder.AddText(item);
            }
        }

        private void AddCalculateDescriptionToCell(TableCellBuilder cellBuilder, 
            string[] text)
        {
            cellBuilder
                .SetPadding(0, 10, 0, 0)
                .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.Dotted)
                .SetBorderColor(Color.Black);
            for (int i = 0, l = text.Length; i < l; i++)
            {
                string item = text[i];
                var font = i == 0 ? FNT7_2B : FNT7_2;
                ParagraphBuilder paragraphBuilder = cellBuilder.AddParagraph();
                paragraphBuilder
                    .SetFont(font)
                    .AddText(item);
            }
        }

        private void AddCalculateEndingBalanceDescriptionToCell(
            TableCellBuilder cellBuilder, string[] text)
        {
            cellBuilder
                .SetPadding(0, 10, 0, 0)
                .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.None);
            for (int i = 0, l = text.Length; i < l; i++)
            {
                string item = text[i];
                var font = i == 0 ? FNT7_2B : FNT7_2;
                ParagraphBuilder paragraphBuilder = cellBuilder.AddParagraph();
                paragraphBuilder
                    .SetFont(font)
                    .AddText(item);
            }
        }

        private void AddEndingBalanceForm(TableCellBuilder cellBuilder)
        {
            AddFormRow(cellBuilder).SetMarginTop(10);
        }

        private void AddAnyDepositsForm(TableCellBuilder cellBuilder)
        {
            AddFormRow(cellBuilder).SetMarginBottom(3);
            AddFormRow(cellBuilder).SetMarginBottom(3);
            AddFormRow(cellBuilder).SetMarginBottom(3);
            AddFormRow(cellBuilder, "+ $").SetMarginBottom(10);
            AddFormRow(cellBuilder, "TOTAL $").SetMarginBottom(38);
            AddFormRow(cellBuilder, "TOTAL $");
        }

        private void AddAnyWithdrawalsForm(TableCellBuilder cellBuilder)
        {
            AddFormRow(cellBuilder, "- $").SetMarginTop(10).SetMarginBottom(25);
            AddFormRowBox(cellBuilder); 
        }
    }
}