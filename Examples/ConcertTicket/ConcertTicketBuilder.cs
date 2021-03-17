using ConcertTicketData.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;
using System.IO;
using System.Globalization;
using Gehtsoft.PDFFlow.UserUtils;

namespace ConcertTicket
{
    internal class ConcertTicketBuilder
    {
        public TicketData TicketData { get; internal set; }
        public ConcertData ConcertData { get; internal set; }

        internal static readonly CultureInfo DocumentLocale  
            = new CultureInfo("en-US");
        internal const PageOrientation Orientation 
            = PageOrientation.Portrait;
        internal static readonly Box Margins  = new Box(30, 27, 30, 27);
        internal static readonly XUnit PageWidth = 
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));

        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9f);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12f);
        internal static readonly FontBuilder FNT12B = Fonts.Helvetica(12f).SetBold(true);
        internal static readonly FontBuilder FNT20 = Fonts.Helvetica(20f);
        internal static readonly FontBuilder FNT19B = Fonts.Helvetica(19f).SetBold();



        internal DocumentBuilder Build()
        {
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            var concertSection = documentBuilder.AddSection();

            concertSection
                 .SetOrientation(Orientation)
                 .SetMargins(Margins);

            addConcertTable(concertSection);
            addInfoTable(concertSection);
            addCounterFoil(concertSection);

            return documentBuilder;
        }

        public void addConcertTable(SectionBuilder section)
        {
            var concertTable = section.AddTable()
                .SetContentRowStyleBorder(borderBuilder =>
                    borderBuilder.SetStroke(Stroke.None));

            concertTable
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 20)
                .AddColumnPercentToTable("", 30)
                .AddColumnPercentToTable("", 20)
                .AddColumnPercentToTable("", 30);

            var row1Builder = concertTable.AddRow();
            AddLogoImage(row1Builder.AddCell("", 0, 2));
            AddConcertData(row1Builder.AddCell("", 3, 0)
                .SetPadding(32, 0, 0, 0));

            var row2Builder = concertTable.AddRow();
            row2Builder.AddCell();
            No(row2Builder.AddCell("").SetFont(FNT10)
                .SetPadding(32, 0, 0, 0));
            FillTicketData(row2Builder.AddCell());
            FillPersonalInfo(row2Builder.AddCell());
        }
        public void addInfoTable(SectionBuilder section)
        {
            var infoTable = section.AddTable()
                .SetContentRowStyleBorder(borderBuilder =>
                    borderBuilder.SetStroke(Stroke.None));

            infoTable
                .SetMarginTop(9f)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 50)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25);

            var row3Builder = infoTable.AddRow();
            FillRuleA(start: 0, end: 10, row3Builder.AddCell("").SetFont(FNT10));
            FillRuleP(row3Builder.AddCell("", 2, 0).SetFont(FNT10));

            var row4Builder = infoTable.AddRow();
            FillBandlist(row4Builder.AddCell("").SetFont(FNT12));
            row4Builder.AddCell("")
                .AddImage(Path.Combine("images", "CT_Location.png")).SetHeight(400)
                .SetMarginTop(9);
            AddContactInfo(row4Builder.AddCell("").SetFont(FNT12));
        }
        private void addCounterFoil(SectionBuilder section)
        {
            var counterFoil = section.AddTable()
                .SetContentRowStyleBorder(borderBuilder =>
                    borderBuilder.SetStroke(Stroke.None));

            counterFoil
                .SetMarginTop(10f)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 50)
                .AddColumnPercentToTable("", 16)
                .AddColumnPercentToTable("", 20)
                .AddColumnPercentToTable("", 14);

            var row5Builder = counterFoil.AddRow();
            YourTicket(row5Builder.AddCell("")
                .SetPadding(0, 2, 0, 0));
            AddConcertData(row5Builder.AddCell("", 3, 0));

            var row6Builder = counterFoil.AddRow();
            row6Builder.AddCell()
                .AddImage(Path.Combine("images", "CT_Scheme.png")).SetHeight(100);
            FillTicketDataCounterFoil(row6Builder.AddCell());
            FillPersonalInfoCounterFoil(row6Builder.AddCell());
            row6Builder.AddCell()
                .AddQRCodeUrl("https://gehtsoftusa.com/", 4, Color.Black, Color.White, false).SetWidth(153);

            var row7Builder = counterFoil.AddRow();
            row7Builder.AddCell();
            row7Builder.AddCell();
            row7Builder.AddCell();
            row7Builder.AddCell(TicketData.Eticket).SetFont(FNT10);
        }

        private void AddConcertData(TableCellBuilder cellBuilder)
        {
            cellBuilder
                .AddParagraph("Epic Rock Concert").SetFont(FNT19B);
            cellBuilder
                .AddParagraph("07/28/2021  9:00PM").SetFont(FNT12)
                .SetBorderStroke(strokeLeft: Stroke.None, strokeTop: Stroke.None, strokeRight: Stroke.None, strokeBottom: Stroke.Solid)
                .SetBorderWidth(2);
        }

        private void AddLogoImage(TableCellBuilder cellBuilder)
        {
            cellBuilder
                .SetPadding(2, 2, 2, 0);
            cellBuilder
                .AddImage(Path.Combine("images", "СT_Poster.png")).SetHeight(340); 
        }

        private void No(TableCellBuilder cellBuilder)
        {
            cellBuilder
               .AddParagraph("E - ticket");
            cellBuilder
                .AddParagraph(TicketData.Eticket).SetLineSpacing(1.5f); 
            cellBuilder
                .AddQRCodeUrl("https://gehtsoftusa.com/", 4, 
                              Color.Black, Color.White, false).SetHeight(100);
        }

       private void FillRuleA(int start, int end, TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(ConcertData.TitleRulesOfAttendance).SetFont(FNT12B).SetMargins(10, 10, 1, 4);
            cellBuilder.SetBorderStroke(strokeLeft: Stroke.Solid, strokeTop: Stroke.Solid, strokeRight: Stroke.None, strokeBottom: Stroke.Solid);

            foreach (var item in ConcertData.RulesOfAttendance)
            {
                cellBuilder.AddParagraph(item).SetFont(FNT9).SetMargins(20, 0, 10, 2);
            }
        }

        private void FillRuleP(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(ConcertData.TitleRulesOfPurchase).SetFont(FNT12B).SetMargins(10, 10, 1, 4);
            cellBuilder.SetBorderStroke(strokeLeft: Stroke.None, strokeTop: Stroke.Solid,
                    strokeRight: Stroke.Solid, strokeBottom: Stroke.Solid);
            cellBuilder.AddParagraph(ConcertData.RulesOfPurchase).SetFont(FNT9).SetLineSpacing(1.2f).SetMargins(10, 0, 10, 4);    
        }

        private void FillBandlist(TableCellBuilder cellBuilder)
        {
            cellBuilder.SetBorderStroke(Stroke.None);
            cellBuilder.AddParagraph(ConcertData.TitleBandsList).SetFont(FNT12B).SetMargins(0, 20, 1, 4);
            cellBuilder.AddParagraph(ConcertData.BandsList).SetFont(FNT9).SetLineSpacing(1.2f).SetMargins(0, 0, 30, 4);
            cellBuilder.AddParagraph("");
        }

        private void AddContactInfo(TableCellBuilder cellBuilder)
        {
            cellBuilder.SetBorderStroke(Stroke.None).SetPadding(11,11,0,0);  
            cellBuilder.AddParagraph(ConcertData.TitleHowtoFind).SetFont(FNT12B).SetMargins(0, 9, 1, 4);
            cellBuilder.AddParagraph(ConcertData.HowToFind).SetFont(FNT9);
            cellBuilder.AddParagraph(ConcertData.TitleLearn).SetFont(FNT12B).SetMarginTop(10);
            cellBuilder.AddParagraph(ConcertData.Facebook).SetFont(FNT9)
                .SetAlignment(HorizontalAlignment.Left);
            cellBuilder.AddParagraph(ConcertData.Twitter).SetFont(FNT9)
                .SetAlignment(HorizontalAlignment.Left);
            cellBuilder.AddParagraph(ConcertData.Instagram).SetFont(FNT9)
                .SetAlignment(HorizontalAlignment.Left);
        }

        private void FillTicketData(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph("Admission").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Ticket type").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Price").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Name").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Venue").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Address").SetLineSpacing(1.4f);
        }
        private void FillPersonalInfo(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(TicketData.Admission).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.TicketType).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Price).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Name).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Venue).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Address).SetLineSpacing(1.4f);
        }
        private void YourTicket(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(ConcertData.CounterFoil).SetFont(FNT9).SetMarginRight(30);
        }
        private void FillTicketDataCounterFoil(TableCellBuilder cellBuilder)
        {
            cellBuilder.SetBorderStroke(Stroke.None);
            cellBuilder.AddParagraph("Admission").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Ticket type").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Price").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Name").SetLineSpacing(1.4f);
        }
        private void FillPersonalInfoCounterFoil(TableCellBuilder cellBuilder)
        {
            cellBuilder.SetBorderStroke(Stroke.None).SetBold(true);
            cellBuilder.AddParagraph(TicketData.Admission).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.TicketType).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Price).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Name).SetLineSpacing(1.4f);
        }
    }

}
