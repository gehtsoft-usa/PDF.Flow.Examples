using BankAccountStatement.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using System;
using System.IO;
using static BankAccountStatement.BankAccountStatementBuilder;
using static BankAccountStatement.Helper;

namespace BankAccountStatement
{
    internal class BankAccountStatementPageBuilder
    {
        protected const string CHECK_BOX = "o";
        protected StatementInfo statementInfo;

        public BankAccountStatementPageBuilder(StatementInfo statementInfo)
        {
            this.statementInfo = statementInfo;
        }
        protected void AddTitle(TableBuilder tableBuilder, float bottomMargin, 
            int pageCount, Action<TableCellBuilder, int> AddTitleAction)
        {
            tableBuilder
                .SetBorder(Stroke.None)
                .SetWidth(PageWidth)
                .AddColumnPercentToTable("", 90).AddColumnPercentToTable("", 10)
                .SetMarginBottom(bottomMargin);
            var rowBuilder = tableBuilder.AddRow();
            AddTitleAction(rowBuilder.AddCell(), pageCount);
            AddLogo(rowBuilder.AddCell());
        }

        protected void AddTitleWithBankName(TableCellBuilder cellBuilder, 
            int pageCount)
        {
            AddParagraph(cellBuilder, 10f, statementInfo.BankName +
                            " Simple Business Checking", FNT18_3B);
            AddTextTitle(cellBuilder, pageCount, 0);
        }

        protected void AddCommonPageTitle(TableCellBuilder cellBuilder, int pageCount)
        {
            AddTextTitle(cellBuilder, pageCount, 10);
        }

        protected void AddTextTitle(TableCellBuilder cellBuilder, int pageCount, float topMargin)
        {
            var tableBuilder = cellBuilder.AddTable();
            tableBuilder
                .SetMarginTop(topMargin)
                .SetBorder(Stroke.None)
                .SetWidth(XUnit.FromPercent(70))
                .AddColumnPercentToTable("", 23)
                .AddColumnPercentToTable("", 18)
                .AddColumnPercentToTable("", 42)
                .AddColumnPercent("", 17);
            AddStatementInfoToTitle(tableBuilder.AddRow(), pageCount);
        }

        private void AddStatementInfoToTitle(TableRowBuilder tableRowBuilder, int pageCount)
        {
            tableRowBuilder.AddCell("Account number:").SetFont(FNT8_9);
            tableRowBuilder.AddCell(statementInfo.AccountNumber + " ").SetFont(FNT9B);
            tableRowBuilder.AddCell(statementInfo.DateBegin.ToString(
               "MMMM dd, yyyy", DocumentLocale) +
                " - " +
                statementInfo.DateEnd.ToString(
                "MMMM dd, yyyy", DocumentLocale)).SetFont(FNT8_9);
            var paragraphBuilder = tableRowBuilder.AddCell().SetFont(FNT8_9).AddParagraph();
            paragraphBuilder
                .AddTextToParagraph("Page ")
                .AddPageNumberToParagraph()
                .AddTextToParagraph(" of " + pageCount);
        }
        
        private void AddLogo(TableCellBuilder cellBuilder)
        {
            cellBuilder
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .AddImageToCell(Path.Combine("images", "BS_Logo_2x.png"));
        }
    }
}