using AirplaneTicket.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System;
using Gehtsoft.PDFFlow.UserUtils;

namespace AirplaneTicket
{
    internal class AirplaneTicketBuilder
    { 
        internal static readonly CultureInfo DocumentLocale  
            = new CultureInfo("en-US");
        internal const PageOrientation Orientation 
            = PageOrientation.Portrait;
        internal static readonly Box Margins  = new Box(29, 20, 29, 20);
        internal static readonly XUnit PageWidth = 
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));

        internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7f);
        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9f);
        internal static readonly FontBuilder FNT9B_G = 
            Fonts.Helvetica(9f).SetBold().SetColor(Color.Gray);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11f);
        internal static readonly FontBuilder FNT11_B = 
            Fonts.Helvetica(11f).SetBold();
        internal static readonly FontBuilder FNT20 = Fonts.Helvetica(20f);
        internal static readonly FontBuilder FNT20B = 
            Fonts.Helvetica(20f).SetBold();
        private static readonly string[] ROUTE_HEADS = {
                "FLIGHT", "DEPARTURE", "ARRIVAL", "CLASS", "BAGGAGE", "CHECK-IN"
            };

        public TicketData TicketData { get; internal set; }
        public List<RouteData> RouteData { get; internal set; }
        public List<string> TripData { get; internal set; }
        public List<FareData> FareData { get; internal set; }
        public List<HelpData> HelpData { get; internal set; }

        internal DocumentBuilder Build()
        {
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            sectionBuilder.AddHeaderToBothPages(80, BuildHeader);
            FillTicketInfoTable(sectionBuilder.AddTable(), GetTicketData());
            BuildRouteInfo(sectionBuilder);
            BuildAboutTrip(sectionBuilder);
            BuildFareAndHelp(sectionBuilder);
            return documentBuilder;
        }

        private void BuildHeader(RepeatingAreaBuilder builder)
        {
            var tableBuilder = builder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(50))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 25).AddColumnPercent("", 75);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder.AddImage(Path.Combine("images", "AT_Logo_2x.png"),
                XSize.FromHeight(80));
            cellBuilder = rowBuilder.AddCell()
                .SetVerticalAlignment(VerticalAlignment.Center)
                .SetPadding(17, 0, 0, 0);
            cellBuilder
                .AddParagraph(TicketData.Company + " company").SetFont(FNT20B);
            cellBuilder
                .AddParagraph("E-ticket").SetFont(FNT20);
        }

        private void FillTicketInfoTable(TableBuilder tableBuilder, string[,] ticketData)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetContentRowStyleFont(FNT10)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercent("", 25);
            for(int i = 0, len = (ticketData.Length >> 2), j = len + i; i < len; i++, j++)
            {
                var rowBuilder = tableBuilder.AddRow();
                var cellBuilder = rowBuilder.AddCell(ticketData[i, 0]);
                cellBuilder
                    .SetPadding(0, 3.5f, 0, 8.5f)
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(
                        Stroke.None, Stroke.None, Stroke.None, Stroke.Solid);
                cellBuilder = rowBuilder.AddCell(ticketData[i, 1]);
                cellBuilder
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(0, 3.5f, 0, 8.5f)
                    .SetBorderWidth(0, 0, 10, 0.5f)
                    .SetBorderColor(
                        Color.Black, Color.Black, Color.White, Color.Black)
                    .SetBorderStroke(
                        Stroke.None, Stroke.None, Stroke.Solid, Stroke.Solid);
                cellBuilder = rowBuilder.AddCell(ticketData[j, 0]);
                cellBuilder
                    .SetPadding(0, 3.5f, 0, 8.5f)
                    .SetBorderWidth(10, 0, 0, 0.5f)
                    .SetBorderColor(
                        Color.White, Color.Black, Color.Black, Color.Black)
                    .SetBorderStroke(
                        Stroke.Solid, Stroke.None, Stroke.None, Stroke.Solid);
                cellBuilder = rowBuilder.AddCell(ticketData[j, 1]);
                cellBuilder
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(0, 3.5f, 0, 8.5f)
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(
                        Stroke.None, Stroke.None, Stroke.None, Stroke.Solid);
            }
        }

        private string[,] GetTicketData()
        {
            return new string[,]
            {
                {"Passenger:", TicketData.Passenger},
                {"Document:", TicketData.Document},
                {"Ticket No:", TicketData.TicketNo},
                {"Order:", TicketData.Order},
                {"Issued:", TicketData.Issued.ToString(
                                "dd MMMM yyyy", DocumentLocale)},
                {"Status:", TicketData.Status}
            };
        }

        private void BuildRouteInfo(SectionBuilder sectionBuilder)
        {
            sectionBuilder.AddParagraph("Route")
                .SetFont(FNT11_B).SetMarginTop(22);
            sectionBuilder.AddLine(PageWidth, 2f, Stroke.Solid);
            FillRouteInfoTable(sectionBuilder.AddTable());
        }

        private void FillRouteInfoTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercentToTable("", 16)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercent("", 16);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .ApplyStyle(
                    StyleBuilder.New()
                        .SetFont(FNT9B_G)
                        .SetPaddingTop(4.8f)
                        .SetPaddingBottom(8.2f)
                );
            foreach (string headName in ROUTE_HEADS)  
            {
                rowBuilder.AddCell(headName);
            }
            foreach(RouteData rd in RouteData)
            {
                rowBuilder = tableBuilder.AddRow();
                rowBuilder
                    .ApplyStyle(
                        StyleBuilder.New()
                            .SetFont(FNT10)
                            .SetPaddingTop(3.5f)
                            .SetPaddingBottom(7.5f)
                    );
                foreach (string cellValue in FirstRouteRow(rd))
                {
                    rowBuilder.AddCell(cellValue);
                }
                rowBuilder = tableBuilder.AddRow();
                rowBuilder
                    .ApplyStyle(
                        StyleBuilder.New()
                            .SetFont(FNT7)
                            .SetPaddingBottom(5.5f)
                            .SetBorderBottom(0.5f, Stroke.Solid, Color.Black)
                    );
                foreach (string cellValue in SecondRouteRow(rd))
                {
                    rowBuilder.AddCell(cellValue);
                }
            }
        }

        private string[] FirstRouteRow(RouteData rd)
        {
            return new string[]
            {
                rd.Flight,
                rd.Departure.ToString("dd MMMM", DocumentLocale),
                rd.Arrival.ToString("dd MMMM", DocumentLocale),
                rd.Class,
                rd.Baggage,
                rd.CheckIn.ToString("HH:mm", DocumentLocale),
            };

        }

        private string[] SecondRouteRow(RouteData rd)
        {
            return new string[]
            {
                rd.FlightCompany + "\n" + rd.FlightPlaner,
                rd.Departure.ToString("HH:mm", DocumentLocale) + 
                    "\n" + rd.DepartureAirport,
                rd.Arrival.ToString("HH:mm", DocumentLocale) + 
                    "\n" + rd.ArrivalAirport,
                rd.ClassAdd,
                rd.BaggageAdd,
                rd.CheckInAirport,
            };
        }
        private void BuildAboutTrip(SectionBuilder sectionBuilder)
        {
            sectionBuilder.AddParagraph("About your trip")
               .SetFont(FNT11_B).SetMarginTop(14);
            var lineBuilder = sectionBuilder.AddLine(PageWidth, 2f, Stroke.Solid);
            lineBuilder.SetMarginBottom(10);
            BuildAboutList(sectionBuilder);
        }

        private void BuildAboutList(SectionBuilder sectionBuilder)
        {
            foreach (String text in TripData)
            {
                var paragraphBuilder = sectionBuilder.AddParagraph();
                paragraphBuilder
                    .SetMarginLeft(8)
                    .SetFont(FNT9)
                    .SetListBulleted()
                    .AddTextToParagraph(
                        text.Replace("{company.name}", TicketData.Company));
            }
        }

        private void BuildFareAndHelp(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetMarginTop(30)
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 51)
                .AddColumnPercent("", 49);
            var tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder.AddCellToRow(BuildFare);
            tableRowBuilder.AddCellToRow(BuildHelp);

        }

        private void BuildFare(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph("Fare breakdown")
               .SetFont(FNT11_B).SetMarginBottom(2);
            cellBuilder.AddTable(FillFareTable);
        }

        private void BuildHelp(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph("Need help?")
               .SetFont(FNT11_B).SetMarginBottom(2);
            cellBuilder.AddTable(FillHelpTable);
        }

        private void FillFareTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(95))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 60)
                .AddColumnPercent("", 40);
            var tableRowStyle = StyleBuilder.New()
                    .SetBorderTop(2, Stroke.Solid, Color.Black);
            TableRowBuilder tableRowBuilder;
            TableCellBuilder cellBuilder;
            double sum = 0;
            foreach(FareData fd in FareData)
            {
                tableRowStyle
                    .SetBorderBottom(0.5f, Stroke.Solid, Color.Black)
                    .SetFont(FNT10)
                    .SetPaddingTop(3.5f)
                    .SetPaddingBottom(8.5f);
                tableRowBuilder = 
                    tableBuilder.AddRow().ApplyStyle(tableRowStyle);
                tableRowBuilder.AddCell(fd.Name);
                cellBuilder = tableRowBuilder.AddCell(String
                    .Format(CultureInfo.InvariantCulture, "{0:0,0.00}", fd.Fare) +
                    " USD");
                cellBuilder.SetHorizontalAlignment(HorizontalAlignment.Right);
                //cellBuilder.SetPadding(0, 3.5f, 0, 8.5f);
                sum += fd.Fare;
                tableRowStyle = StyleBuilder.New();
            }
            tableRowStyle
                .SetBorderBottom(0.5f, Stroke.Solid, Color.Black)
                .SetFont(FNT10)
                .SetPaddingTop(3.5f)
                .SetPaddingBottom(8.5f);
            tableRowBuilder =
                tableBuilder.AddRow().ApplyStyle(tableRowStyle);
            tableRowBuilder.AddCell("Total fare:");
            cellBuilder = tableRowBuilder.AddCell(String
                .Format(CultureInfo.InvariantCulture, "{0:0,0.00}", sum) +
                " USD");
            cellBuilder.SetHorizontalAlignment(HorizontalAlignment.Right);
            //cellBuilder.SetPadding(0, 3.5f, 0, 8.5f);
        }

        private void FillHelpTable(TableBuilder tableBuilder)
        {
            var endIndex = HelpData.Count - 1;
            if (endIndex < 0)
            {
                return;
            }
            tableBuilder
                .SetWidth(XUnit.FromPercent(95))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 60)
                .AddColumnPercent("", 40);
            var tableRowStyle = StyleBuilder.New()
                    .SetBorderTop(2, Stroke.Solid, Color.Black);
            for (int i = 0; ; i++)
            {
                var hd = HelpData[i];
                tableRowStyle
                    .SetBorderBottom(0.5f, Stroke.Solid, Color.Black)
                    .SetFont(FNT10)
                    .SetPaddingTop(3.5f)
                    .SetPaddingBottom(8.5f);
                var tableRowBuilder =
                    tableBuilder.AddRow().ApplyStyle(tableRowStyle);
                tableRowBuilder
                    .AddCell(hd.Name.Replace("{company.name}", 
                        TicketData.Company));
                var cellBuilder = tableRowBuilder.AddCell(hd.Value);
                cellBuilder.SetHorizontalAlignment(HorizontalAlignment.Right);
                //cellBuilder.SetPadding(0, 3.5f, 0, 8.5f);
                if (i == endIndex)
                {
                    break;
                }
                tableRowStyle = StyleBuilder.New();
            }
        }
    }
}