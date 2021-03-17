using TradeConfirmationData.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;
using System.IO;
using System.Globalization;
using Gehtsoft.PDFFlow.UserUtils;

namespace TradeConfirmation
{
    internal class TradeConfirmationBuilder
    {
        public FirmData FirmData { get; internal set; }
        public TradeData TradeData { get; internal set; }

        internal static readonly CultureInfo DocumentLocale
            = new CultureInfo("en-US");
        internal static readonly XUnit PageWidth =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));
        internal const PageOrientation Orientation
            = PageOrientation.Portrait;
        internal static readonly Box Margins = new Box(30, 27, 30, 27);

        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9f);
        internal static readonly FontBuilder FNT9B = Fonts.Helvetica(9f).SetBold();
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT11B = Fonts.Helvetica(11f).SetBold();
        internal static readonly FontBuilder FNT24B = Fonts.Helvetica(24f).SetBold();

        internal DocumentBuilder Build()
        {
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            var Section = documentBuilder.AddSection();
            Section.SetOrientation(PageOrientation.Portrait);

            AddTradeHeader(Section);
            Section
                .AddLine()
                .SetWidth(2)
                .SetStroke(Stroke.Solid)
                .SetMargins(0, 10, 0, 14)
                .SetAlignment(HorizontalAlignment.Left).ToSection();
            AddTradeBody(Section);
            AddTradeFooter(Section);

            return documentBuilder;
        }

        public void AddTradeHeader(SectionBuilder section)
        {
            var headerTable = section.AddTable()
                .SetContentRowStyleBorder(borderBuilder =>
                   borderBuilder.SetStroke(Stroke.None));

            headerTable
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 50)
                .AddColumnPercentToTable("", 50);

            var row1Builder = headerTable.AddRow();
            row1Builder.AddCell("Financial Firm ABC®", 2, 0).SetFont(FNT24B);

            var row2Builder = headerTable.AddRow();
            row2Builder.AddCell(FirmData.DocumentDate, 2, 0).SetFont(FNT9)
                .SetHorizontalAlignment(HorizontalAlignment.Right);

            var row3Builder = headerTable.AddRow();
            AddAccount(row3Builder.AddCell("", 2, 0).SetFont(FNT9)
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(0, 0, 0, 10)
                .SetBorderStroke(Stroke.None));

            var row4Builder = headerTable.AddRow();
            AddCustomer(row4Builder.AddCell("").SetFont(FNT9));
            AddFirm(row4Builder.AddCell("").SetFont(FNT9)
                .SetHorizontalAlignment(HorizontalAlignment.Right));

            var row5Builder = headerTable.AddRow();
            AddAccount(row5Builder.AddCell("", 2, 0).SetFont(FNT9).SetBold(true)
                .SetHorizontalAlignment(HorizontalAlignment.Left)
                .SetBorderStroke(Stroke.None));

            var row6Builder = headerTable.AddRow();
            row6Builder.AddCell("We are pleased to confirm the below transaction:", 2, 0).SetFont(FNT9);
        }
        
        public void AddTradeBody(SectionBuilder section)
        {
            var bodyTable = section.AddTable()
                .SetContentRowStyleBorder(borderBuilder =>
                borderBuilder.SetStroke(Stroke.None));

            bodyTable
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 15)
               .AddColumnPercentToTable("", 45)
                .AddColumnPercentToTable("", 3)
                .AddColumnPercentToTable("", 37);

            var row7Builder = bodyTable.AddRow();
            row7Builder.AddCell()
                .AddImage(Path.Combine("images", "TC_Image.png"), 100, 100);
            BodyInfo(row7Builder.AddCell("", 3, 0));

            var row8Builder = bodyTable.AddRow();
            row8Builder.AddCell("Trade").SetFont(FNT9)
                .SetHorizontalAlignment(HorizontalAlignment.Center)
                .SetPadding(0, 0, 5, 0);
            row8Builder.AddCell("", 3, 0)
                .AddParagraph("Order number: ").SetFont(FNT9).AddText(TradeData.Order).SetFont(FNT9);

            var row9Builder = bodyTable.AddRow();
            row9Builder.AddCell();
            row9Builder.AddCell("Trade Calculation", 3, 0).SetFont(FNT11B)
                .SetBorderWidth(0, 0, 0, 2)
                .SetBorderStroke(strokeLeft: Stroke.None, strokeTop: Stroke.None,
                    strokeRight: Stroke.None, strokeBottom: Stroke.Solid);
            
            var row10Builder = bodyTable.AddRow();
            row10Builder.AddCell();
            FillLeftBlock(a: "Principal Amount*:", b: TradeData.PrincipalAmount, 
                row10Builder.AddCell("").SetPadding(0, 4, 0, 4));
            row10Builder.AddCell();
            FillRightBlock(a: "Trade Date:", b: TradeData.TradeDate, 
                row10Builder.AddCell("").SetPadding(0, 4, 0, 4));

            var row11Builder = bodyTable.AddRow();
            row11Builder.AddCell();
            FillLeftBlock(a: "Accrued Interest:", b: TradeData.AccruedInterest,
                row11Builder.AddCell("").SetPadding(0, 4, 0, 4));
            row11Builder.AddCell();
            FillRightBlock(a: "Trade Time", b: TradeData.TradeTime,
                row11Builder.AddCell("").SetPadding(0, 4, 0, 4));

            var row12Builder = bodyTable.AddRow();
            row12Builder.AddCell();
            FillLeftBlock(a: "Transaction Fee:", b: TradeData.TransactionFee,
                row12Builder.AddCell("").SetPadding(0, 4, 0, 4).SetBorderWidth(0, 0, 0, 2));
            row12Builder.AddCell();
            FillRightBlock(a: "Settlement Date:", b: TradeData.SettlementDate,
                row12Builder.AddCell("").SetPadding(0, 4, 0, 4));

            var row13Builder = bodyTable.AddRow();
            row13Builder.AddCell();
            FillLeftBlock(a: "Total:", b: TradeData.Total,
                row13Builder.AddCell("").SetPadding(0, 4, 0, 4));
            row13Builder.AddCell();
            row13Builder.AddCell();

            var row14Builder = bodyTable.AddRow();
            row14Builder.AddCell();
            FillLeftBlock(a: "Bank Qualified:", b: TradeData.BankQualified,
                row14Builder.AddCell("").SetPadding(0, 4, 0, 4).SetBorderWidth(0, 0, 0, 2));
            row14Builder.AddCell();
            row14Builder.AddCell();
            
            var row15Builder = bodyTable.AddRow();
            row15Builder.AddCell();
            FillLeftBlock(a: "State:", b: TradeData.State,
                row15Builder.AddCell("").SetPadding(0, 4, 0, 4));
            row15Builder.AddCell();
            row15Builder.AddCell();
            
            var row16Builder = bodyTable.AddRow();
             row16Builder.AddCell();
             FillLeftBlock(a: "Bank Qualified:", b: TradeData.BankQualified2,
                 row16Builder.AddCell("").SetPadding(0, 4, 0, 4));
             row16Builder.AddCell();
             row16Builder.AddCell();

             var row17Builder = bodyTable.AddRow();
             row17Builder.AddCell();
             FillLeftBlock(a: "Dated to Date:", b: TradeData.DatedDate,
                 row17Builder.AddCell("").SetPadding(0, 4, 0, 4));
             row17Builder.AddCell();
             row17Builder.AddCell();

             var row18Builder = bodyTable.AddRow();
              row18Builder.AddCell();
              FillLeftBlock(a: "Yield to Maturity:", b: TradeData.YieldtoMaturity,
                  row18Builder.AddCell("").SetPadding(0, 4, 0, 4));
              row18Builder.AddCell();
              row18Builder.AddCell();

              var row19Builder = bodyTable.AddRow();
              row19Builder.AddCell();
              FillLeftBlock(a: "Yield to Call:", b: TradeData.YieldtoCall,
                  row19Builder.AddCell("").SetPadding(0, 4, 0, 4));
              row19Builder.AddCell();
              row19Builder.AddCell();

              var row20Builder = bodyTable.AddRow();
              row20Builder.AddCell();
              Exempt(row20Builder.AddCell("").SetPadding(0, 4, 0, 4));
              row20Builder.AddCell();
              row20Builder.AddCell();

              var row21Builder = bodyTable.AddRow();
              row21Builder.AddCell();
              FillLeftBlock(a: "Capacity:", b: TradeData.Capacity,
                  row21Builder.AddCell("").SetPadding(0, 4, 0, 4));
              row21Builder.AddCell();
              row21Builder.AddCell();

              var row22Builder = bodyTable.AddRow();
              row22Builder.AddCell();
              FillLeftBlock(a: "Bond Form:", b: TradeData.BondForm,
                  row22Builder.AddCell("").SetPadding(0, 4, 0, 4));
              row22Builder.AddCell();
              row22Builder.AddCell();
        }
        public void AddTradeFooter(SectionBuilder section)
        {
            var footerTable = section.AddTable()
                .SetContentRowStyleBorder(borderBuilder =>
                borderBuilder.SetStroke(Stroke.None));

            footerTable
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 15)
                .AddColumnPercentToTable("", 85);

            var row23Builder = footerTable.AddRow();
            row23Builder.AddCell();
            Info(row23Builder.AddCell("").SetFont(FNT9)
                .SetPadding(0, 18, 0, 0));
        }
   
        private void AddCustomer(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(TradeData.CustomerName);
            foreach (var item in TradeData.CustomerAddress)
            {
                cellBuilder.AddParagraph(item);
            }
        }

        private void AddFirm(TableCellBuilder cellBuilder)
        {
            foreach (var item in FirmData.FirmContact)
            {
                cellBuilder.AddParagraph(item);
            }
        }

        void AddAccount(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph("Trade Confirmation - Account ")
                .AddTabSymbol().AddText(TradeData.Account);
        }

        void BodyInfo(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(FirmData.TaxInfo).SetMarginBottom(13).SetFont(FNT9);
            cellBuilder.AddParagraph("You bought:").SetFont(FNT9).AddTabSymbol()
                .AddTabulation(300,TabulationType.Right).AddText("Price:").SetFont(FNT9);
            cellBuilder.AddParagraph(TradeData.Bought).SetFont(FNT9B).AddTabSymbol()
                .AddTabulation(300, TabulationType.Right).AddText(TradeData.Price).SetFont(FNT9B);
        }
 
        void FillLeftBlock(string a, string b, TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(a).SetFont(FNT10).SetLineSpacing(1.2f).AddTabSymbol()
                .AddTabulation(252, TabulationType.Right).AddText(b).SetFont(FNT10);
            cellBuilder
                .SetBorderStroke(strokeLeft: Stroke.None, strokeTop: Stroke.None,
                strokeRight: Stroke.None, strokeBottom: Stroke.Solid);
        }
        void FillRightBlock(string a, string b, TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(a).SetFont(FNT10).SetLineSpacing(1.2f).AddTabSymbol()
                .AddTabulation(207, TabulationType.Right).AddText(b).SetFont(FNT10);
            cellBuilder
                .SetBorderStroke(strokeLeft: Stroke.None, strokeTop: Stroke.None,
                strokeRight: Stroke.None, strokeBottom: Stroke.Solid);
        }
        void Exempt(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph("Callable 04 / 01 / 27 @100").SetFont(FNT10).SetLineSpacing(1.2f).AddTabSymbol()
                .AddTabulation(252, TabulationType.Right).AddText(TradeData.TaxExempt).SetFont(FNT10);
            cellBuilder.AddParagraph("Federally Tax Exempt").SetFont(FNT10);
            cellBuilder
                .SetBorderStroke(strokeLeft: Stroke.None, strokeTop: Stroke.None, 
                strokeRight: Stroke.None, strokeBottom: Stroke.Solid);
        }
        void Info(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(FirmData.ExpensesInfo);
            cellBuilder.AddParagraph(" ");
            cellBuilder.AddParagraph(FirmData.MoreInfo);
        }
    }

}
