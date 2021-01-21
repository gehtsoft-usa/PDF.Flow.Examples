using BankAccountStatement.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using static BankAccountStatement.BankAccountStatementBuilder;
using static BankAccountStatement.Helper;

namespace BankAccountStatement
{ 
    internal class BankAccountStatementSecondPageBuilder : 
        BankAccountStatementPageBuilder
    {
        private List<Statement> statements;
        public BankAccountStatementSecondPageBuilder(StatementInfo statementInfo, 
            List<Statement> statements) : base(statementInfo)
        {
            this.statements = statements;
        }

        internal void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder.SetOrientation(Orientation).SetMargins(Margins);
            var headerBuilder = sectionBuilder.AddHeaderToBothPages(80);
            AddTitle(headerBuilder.AddTable()
                , 20f, 4, AddCommonPageTitle);
            headerBuilder.AddLine(PageWidth, 2f, Stroke.Solid);
            AddOverdraftProtection(sectionBuilder, 32f);
            AddTransactionHistory(sectionBuilder, 12f);
            AddServiceFeeSummary(sectionBuilder);
            AddTransactionFeeSummary(sectionBuilder);
        }

        private void AddOverdraftProtection(SectionBuilder sectionBuilder, 
            float bottomMargin)
        {
            AddParagraph(
                sectionBuilder,
                4.8f,
                "Overdraft Protection",
                FNT7_9B,
                3.6f
            );
            AddParagraph(
                sectionBuilder,
                "Your account is linked to the following for Overdraft Protection:\n" + 
                "Savings - 000001234567890",
                FNT7_2,
                bottomMargin
            );
        }
        private void AddTransactionHistory(SectionBuilder sectionBuilder, 
            float bottomMargin)
        {
            AddStatementsHeadLine(sectionBuilder);
            AddStatementsTable(sectionBuilder);
            AddEndingDayBalanceComment(sectionBuilder, bottomMargin);
        }

        private void AddServiceFeeSummary(SectionBuilder sectionBuilder)
        {
            AddServiceFeeSummaryParagrpaph(sectionBuilder);
            AddServiceFeeSummaryTable(sectionBuilder);
        }

        private void AddTransactionFeeSummary(SectionBuilder sectionBuilder)
        {
            AddTransactionFeeSummaryParagrpaph(sectionBuilder);
            AddTransactionFeeSummaryTable(sectionBuilder);
            sectionBuilder
                .AddLine(PageWidth, 0.5f, Stroke.Solid);
            AddYourFeedBack(sectionBuilder);
        }

        private void AddTransactionFeeSummaryParagrpaph(SectionBuilder sectionBuilder)
        {
            //AddEmptyParagraph(sectionBuilder, FNT5);
            sectionBuilder
                .AddLine(PageWidth, 0.5f, Stroke.Solid).SetMarginBottom(6);
            AddParagraph(
                sectionBuilder,
                "Account transaction fees summary",
                FNT7_9B
            );
        }
        private void AddServiceFeeSummaryParagrpaph(SectionBuilder sectionBuilder)
        {
            AddParagraph(sectionBuilder, "Monthly service fee summary", FNT7_9B, 4.8f);
            var paragraphBuilder = sectionBuilder.AddParagraph(); 
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFont(FNT7_2URL)
                        .SetBorder(0, Stroke.None, Color.White)
                    )
                .SetFont(FNT7_2)
                .AddTextToParagraph(
                    "For a complete list of fees and detailed account information, please see the " + 
                    statementInfo.BankName + 
                    " Fee and Information Schedule and Account Agreement applicable to your account or talk to a banker. Go to "
                    )
                .AddUrlToParagraph("https://" + 
                    statementInfo.Online + 
                    "/feefaq", statementInfo.Online + "/feefaq")
                .AddTextToParagraph(
                    " to find answers to common questions about the monthly service fee on your account.");
        }
        private void AddServiceFeeSummaryTable(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable(); 
            tableBuilder
                .SetBorder(Stroke.None)
                .ApplyStyle(
                    StyleBuilder.New()
                    .SetFont(FNT7_2)
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                )
                .SetMarginTop(20)
                .SetWidth(XUnit.FromPercent(92))
                .AddColumnPercentToTable("", 51)
                .AddColumnPercentToTable("", 30)
                .AddColumnPercentToTable("", 16)
                .AddColumnPercent("", 3);
            AddTableRows(tableBuilder, GetServiceFeeSummaryRowData(), 4, 0, 2);
        }

        private void AddTransactionFeeSummaryTable(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable(); 
            tableBuilder
                .SetBorder(Stroke.None)
                .ApplyStyle(
                    StyleBuilder.New()
                    .SetFont(FNT7_2)
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    )
                .SetMarginTop(6)
                .SetMarginBottom(60)
                .SetWidth(XUnit.FromPercent(92))
                .AddColumnPercentToTable("", 37)
                .AddColumnPercentToTable("", 10)
                .AddColumnPercentToTable("", 10)
                .AddColumnPercentToTable("", 11)
                .AddColumnPercentToTable("", 13)
                .AddColumnPercent("", 18);
            AddTableRows(tableBuilder, GetTransactionFeeSummaryRowData(), 6, 0);
        }
        private void AddYourFeedBack(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0, 0, 0, 0)
                .ApplyStyle(
                    StyleBuilder.New()
                    .SetHorizontalAlignment(HorizontalAlignment.Left)
                    )
                .SetMarginTop(10)
                .SetWidth(XUnit.FromPercent(54))
                .AddColumnPercentToTable("", 7)
                .AddColumnPercent("", 93);
            var tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder.AddCell(CHECK_BOX).SetFont(FNTZ8);
            var cellBuilder = tableRowBuilder.AddCell();
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New().SetFont(FNT7_5URL)
                 )
                .AddTextToParagraph(
                    "Your feedback matters\n",FNT7_9B
                )
                .AddTextToParagraph(
                    "Share your compliments and complaints so we can better serve you.\n" + 
                    "Call us at " + 
                    statementInfo.FeedBackPhone + 
                    "(" + statementInfo.Phone + 
                    ") or visit ",
                    FNT7_5
                )
                .AddUrlToParagraph(
                    "https://" + 
                    statementInfo.Online + 
                    "/feedback", 
                    statementInfo.Online + "/feedback"
                    )
                .AddTextToParagraph(
                    ".",
                    FNT7_5
                );
        }
        private void AddTableRows(TableBuilder tableBuilder, RowData[] rows, 
            int columnCount, params int[] underlines)
        {
            for(int i = 0, l = rows.Length; i < l; i++) 
            {
                RowData row = rows[i];
                var tableRowBuilder = tableBuilder.AddRow();
                tableRowBuilder
                    .SetBorder(borderBuilder =>
                    {
                        borderBuilder
                            .SetLeftBorder(0, Stroke.None, Color.None)
                            .SetTopBorder(0, Stroke.None, Color.None)
                            .SetRightBorder(0, Stroke.None, Color.None);
                        if (underlines.Contains(i))
                        {
                            borderBuilder
                                .SetBottomBorder(0.5f, Stroke.Solid, Color.Black);
                        } else
                        {
                            borderBuilder
                                .SetBottomBorder(row.BottomBorderWidth(), 
                                    row.BottomBorderStroke(), Color.Black);
                        }
                    });
                for(int j = 0; j < columnCount; j++)
                {
                    var cellBuilder = tableRowBuilder.AddCell();
                    cellBuilder
                        .SetHorizontalAlignment(j > 0 ? 
                            HorizontalAlignment.Right : 
                            HorizontalAlignment.Left)
                        .SetVerticalAlignment(VerticalAlignment.Bottom);
                    cellBuilder
                        .SetPadding(0, 0, 0, 0);
                    if (j < row.GetLength())
                    {
                        row.AddTextToParagraph(cellBuilder.AddParagraph(), j);
                    } else
                    {
                        cellBuilder
                            .SetFont(row.GetLastFont())
                            .AddParagraphToCell(" ");
                    }
                }
            }
        }
        private RowData[] GetServiceFeeSummaryRowData()
        {
            return new RowData[]
                {
                    new RowData(
                        "Fee period " + 
                        statementInfo.DateBegin.ToString("MM/dd/yyyy", DocumentLocale) + 
                        " - " + 
                        statementInfo.DateEnd.ToString("MM/dd/yyyy", DocumentLocale),
                        "Standard monthly service fee " + 
                        ReplaceZeroDouble(statementInfo.StandartServiceFee, "$0.00", "$"),
                        "You paid " + 
                        ReplaceZeroDouble(statementInfo.ServiceFee, "$0.00", "$")
                    ),
                    new RowData(
                        new FontBuilder[] {
                            FNT7_2B,
                            FNT7_2
                        },
                        "How to reduce the monthly service fee by " + 
                        ReplaceZeroDouble(statementInfo.ServiceDiscount, "$0.00", "$"),
                        "Minimum required",
                        "This fee period"
                    ),
                    new ComplexRowData(
                        new RowData (
                                new FontBuilder[] {
                                    FNT7_2,
                                    FNT7_2B,
                                    FNT7_2
                                },
                                "Have any ",
                                "ONE",
                                " of the following account requirements\n" + 
                                "Average ledger balance"
                        ),
                        new FontBuilder[] {
                            FNT7_2,
                            FNT7_2,
                            FNTZ8
                        },
                        ReplaceZeroDouble(statementInfo.MinimumRequired, "$0.00", "$"),
                        ReplaceZeroDouble(Math.Round(statementInfo.AverageBalance, 0, 
                        MidpointRounding.ToEven), "$0.00", "$"),
                        CHECK_BOX
                    ),
                    new RowData(
                        new FontBuilder[] {
                            FNT7_2B
                        },
                        "Monthly service fee discount(s) (applied when box is checked)"
                    ),
                    new ComplexRowData(
                        new RowData (
                                new FontBuilder[] {
                                    FNTZ8,
                                    FNT7_2
                                },
                                CHECK_BOX,
                                " Online only statements (" +
                                ReplaceZeroDouble(statementInfo.ServiceDiscount, "$0.00", "$") + 
                                "  discount)"
                        ),
                        ReplaceZeroDouble(statementInfo.MinimumRequired, "$0.00", "$"),
                        ReplaceZeroDouble(Math.Round(statementInfo.AverageBalance, 0, 
                            MidpointRounding.ToEven), "$0.00", "$")
                    )
                };
        }
        private RowData[] GetTransactionFeeSummaryRowData()
        {
            return new RowData[]
                {
                    new RowData(
                        "Service charge description",
                        "Units used",
                        "Units included",
                        "Excess units",
                        "Service charge per\nexcess units ($)",
                        "Total service\ncharge ($)"
                    ),
                    new BottomBorderRowData(
                        2f,
                        Stroke.Solid,
                        "Transactions",
                        ReplaceZeroInt(statementInfo.TransactionUnits, "0"),
                        ReplaceZeroInt(statementInfo.TransactionUnitsIncluded, "0"),
                        ReplaceZeroInt(statementInfo.TransactionExcessUnits, "0"),
                        ReplaceZeroDouble(statementInfo.ServiceCharge, "0.00"),
                        ReplaceZeroDouble(statementInfo.TotalServiceCharge, "0.00")
                    ),
                    new RowData(
                        new FontBuilder[] {
                            FNT7_2B
                        },
                        "Total service charges",
                        " ",
                        " ",
                        " ",
                        " ",
                        ReplaceZeroDouble(statementInfo.TotalServiceCharge, "$0.00", "$")
                    )
                };
        }
        private void AddStatementsHeadLine(SectionBuilder sectionBuilder)
        {
            sectionBuilder
                .AddLine(PageWidth, 0.5f, Stroke.Solid);
            AddParagraph(
                sectionBuilder,
                "Transaction history",
                FNT10_5B,
                12
            );
        }

        private void AddStatementsTable(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .ApplyStyle(
                    StyleBuilder.New()
                    .SetFont(FNT7_2)
                    )
                .SetMarginBottom(4)
                .AddColumnPercentToTable("", 4)
                .AddColumnPercentToTable("", 11)
                .AddColumnPercentToTable("", 48)
                .AddColumnPercentToTable("", 13)
                .AddColumnPercentToTable("", 13)
                .AddColumnPercent("", 11);
            AddStatementsHead(tableBuilder);
            Statement total = new Statement();
            AddStatements(tableBuilder, total);
            AddStatementsTotal(tableBuilder, total);
        }

        private void AddEndingDayBalanceComment(SectionBuilder sectionBuilder, float bottomMargin)
        {
            AddParagraph(
                sectionBuilder,
                "The Ending Daily Balance does not reflect any pending withdrawals or holds on deposited funds that may have been outstanding on your account when your transactions posted. If you had insufficient available funds when a transaction posted, fees may have been assessed.",
                FNT7,
                bottomMargin
            );
        }

        private void AddStatementsHead(TableBuilder tableBuilder)
        {
            var tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder
                .SetFont(FNT7_2)
                .SetVerticalAlignment(VerticalAlignment.Bottom)
                .SetBorder(borderBuilder =>
                {
                    borderBuilder
                        .SetLeftBorder(0, Stroke.None, Color.None)
                        .SetTopBorder(0, Stroke.None, Color.None)
                        .SetRightBorder(0, Stroke.None, Color.None)
                        .SetBottomBorder(0.5f, Stroke.Solid, Color.Black);
                });
            tableRowBuilder.AddCell("Date").SetPadding(0, 0, 4, 4);
            tableRowBuilder.AddCell("Check\nNumber")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 4);
            tableRowBuilder.AddCell("Description").SetPadding(4, 0, 4, 4);
            tableRowBuilder.AddCell("Deposits/\nCredits")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 4);
            tableRowBuilder.AddCell("Withdrawals/\nDebits")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 4);
            tableRowBuilder.AddCell("Ending daily\nbalance")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 0, 4);
        }

        private void AddStatementsTotal(TableBuilder tableBuilder, Statement total)
        {
            var tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder
                .SetFont(FNT7_2B)
                .SetVerticalAlignment(VerticalAlignment.Top)
                .SetBorder(borderBuilder =>
                {
                    borderBuilder
                        .SetLeftStroke(Stroke.None)
                        .SetTopStroke(Stroke.None)
                        .SetRightStroke(Stroke.None)
                        .SetBottomBorder(2f, Stroke.Solid, Color.Black);
                });
            tableRowBuilder
                .AddCell("Ending balance on " +
                            statementInfo.DateEnd.ToString("M/d", DocumentLocale))
                .SetPadding(0, 0, 4, 0)
                .SetColSpan(5);
            tableRowBuilder
                .AddCell(ReplaceZeroDouble(statementInfo.EndingBalance, " "))
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 0, 0);
            tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder
                .SetFont(FNT7_2B)
                .SetVerticalAlignment(VerticalAlignment.Top);
            tableRowBuilder.AddCell("Totals").SetPadding(0, 0, 4, 0).SetColSpan(3);
            tableRowBuilder
                .AddCell(ReplaceZeroDouble(total.Deposits, " ", "$"))
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 0);
            tableRowBuilder
                .AddCell(ReplaceZeroDouble(total.Withdrawals, " ", "$"))
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 0);
            tableRowBuilder
                .AddCell(" ")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 0, 0);
        }

        private void AddStatements(TableBuilder tableBuilder, Statement total)
        {
            double deposits = 0;
            double withdrawals = 0;
            foreach (Statement statement in statements)
            {
                deposits += statement.Deposits;
                withdrawals += statement.Withdrawals;
                var tableRowBuilder = tableBuilder.AddRow();
                tableRowBuilder
                    .SetFont(FNT7_2)
                    .SetVerticalAlignment(VerticalAlignment.Top)
                    .SetBorder(borderBuilder =>
                    {
                        borderBuilder
                            .SetLeftStroke(Stroke.None)
                            .SetTopStroke(Stroke.None)
                            .SetRightStroke(Stroke.None)
                            .SetBottomBorder(0.5f, Stroke.Solid, Color.Black);
                    });
                tableRowBuilder
                    .AddCell(ReplaceNullDate(statement.Date, "M/d", " "))
                    .SetPadding(0, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceNullStr(statement.Check, " "))
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(4, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceNullStr(statement.Details, " "))
                    .SetPadding(4, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceZeroDouble(statement.Deposits, " "))
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(4, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceZeroDouble(statement.Withdrawals, " "))
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(4, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceZeroDouble(statement.EndingDailyBalance, " "))
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(4, 0, 0, 0);
            }
            total.Deposits = Math.Round(deposits, 2, MidpointRounding.ToEven);
            total.Withdrawals = Math.Round(withdrawals, 2, MidpointRounding.ToEven);
        }

        private class RowData
        {
            internal string[] data;
            internal FontBuilder[] font;
            internal RowData(params string[] data) {
                this.font = new FontBuilder[] { FNT7_2 };
                this.data = data;
            }
            internal RowData(FontBuilder[] font, params string[] data)
            {
                this.font = font.Length > 0 ? font : new FontBuilder[] { FNT7_2 };
                this.data = data;
            }
            internal FontBuilder GetLastFont()
            {
                return font[font.Length - 1];
            }
            internal virtual int GetLength()
            {
                return data.Length;
            }
            internal virtual void AddTextToParagraph(ParagraphBuilder paragraphBuilder, 
                int index)
            {
                var textBuilder = paragraphBuilder.AddText(
                    index < data.Length ? data[index] : " ");
                textBuilder.SetFont(index < font.Length ? font[index] : GetLastFont());
            }

            internal virtual XUnit BottomBorderWidth()
            {
                return 0;
            }

            internal virtual Stroke BottomBorderStroke()
            {
                return Stroke.None;
            }
        }

        private class BottomBorderRowData : RowData
        {
            XUnit bottomBorderWidth;
            Stroke bottomBorderStroke;
            internal BottomBorderRowData(XUnit bottomBorderWidth, 
                Stroke bottomBorderStroke, params string[] data) : base(data)
            {
                this.bottomBorderWidth = bottomBorderWidth;
                this.bottomBorderStroke = bottomBorderStroke;
            }
            internal BottomBorderRowData(XUnit bottomBorderWidth, 
                Stroke bottomBorderStroke, FontBuilder[] font, params string[] data) : 
                base(font, data)
            {
                this.bottomBorderWidth = bottomBorderWidth;
                this.bottomBorderStroke = bottomBorderStroke;
            }
            internal override XUnit BottomBorderWidth()
            {
                return bottomBorderWidth;
            }

            internal override Stroke BottomBorderStroke()
            {
                return bottomBorderStroke;
            }
        }

        private class ComplexRowData : RowData
        {
            RowData firstCell;
            internal ComplexRowData(RowData firstCell, params string[] data) : base(data)
            {
                this.firstCell = firstCell;
            }
            internal ComplexRowData(RowData firstCell, 
                FontBuilder[] font, params string[] data) : base(font, data)
            {
                this.firstCell = firstCell;
            }
            internal override int GetLength()
            {
                return base.GetLength() + 1;
            }
            internal override void AddTextToParagraph(ParagraphBuilder paragraphBuilder, 
                int index)
            {
                if (index > 0)
                {
                    base.AddTextToParagraph(paragraphBuilder, index - 1);
                } else
                {
                    for (int i = 0, l = firstCell.GetLength(); i < l; i++)
                    {
                        firstCell.AddTextToParagraph(paragraphBuilder, i);
                    }
                }
            }
        }
    }
}