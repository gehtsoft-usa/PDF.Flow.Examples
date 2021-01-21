using System.Collections.Generic;
using System.Globalization;
using System.IO;
using BoardingPass.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;

namespace BoardingPass
{
    internal class BoardingPassBuilder
    { 
        internal static readonly CultureInfo DocumentLocale  
            = new CultureInfo("en-US");
        internal const PageOrientation Orientation 
            = PageOrientation.Portrait;
        internal static readonly Box Margins  = new Box(29, 20, 29, 20);
        internal static readonly XUnit PageWidth = 
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));



        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8f);
        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9f);
        internal static readonly FontBuilder FNT8_G =
            Fonts.Helvetica(8f).SetColor(Color.Gray);
        internal static readonly FontBuilder FNT9B = 
            Fonts.Helvetica(9f).SetBold();
        internal static readonly FontBuilder FNT11B = 
            Fonts.Helvetica(11f).SetBold();
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12f);
        internal static readonly FontBuilder FNT12B = 
            Fonts.Helvetica(12f).SetBold();
        internal static readonly FontBuilder FNT15 = Fonts.Helvetica(15f);
        internal static readonly FontBuilder FNT16 = Fonts.Helvetica(16f);
        internal static readonly FontBuilder FNT16_R = 
            Fonts.Helvetica(16f).SetColor(Color.Red);
        internal static readonly FontBuilder FNT17 = Fonts.Helvetica(17f);
        internal static readonly FontBuilder FNT18 = Fonts.Helvetica(18f);
        internal static readonly FontBuilder FNT20 = Fonts.Helvetica(20f);

        internal static readonly BoardingCell EMPTY_ITEM = new BoardingCell("", new FontText[0]);


        public BoardingData BoardingData { get; internal set; }
        public TicketData TicketData { get; internal set; }
        public List<string> WhatsNextData { get; internal set; }

        internal DocumentBuilder Build()
        {
            BoardingCell[,] boardingItems = GetBoardingItems();
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            FillTopLogoBarTable(sectionBuilder.AddTable());
            FillBoardingHandBugTable(sectionBuilder.AddTable(), boardingItems);
            AddWhatsNext(sectionBuilder);
            FillBottomLogoBarBoardingTable(
                sectionBuilder.AddTable(), boardingItems);
            return documentBuilder;
        }

        private void FillTopLogoBarTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 75)
                .AddColumnPercent("", 25);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.AddCell().AddTable(FillLogoTable);
            rowBuilder.AddCell(FillBarTableCell);
        }

        private void FillLogoTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(40))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercent("", 75);
            var rowBuilder = tableBuilder.AddRow();
            AddLogoImage(rowBuilder.AddCell());
            AddCompanyName(rowBuilder.AddCell());
        }

        private void AddCompanyName(TableCellBuilder cellBuilder)
        {
            cellBuilder
                .SetVerticalAlignment(VerticalAlignment.Center)
                .SetPadding(13, 0, 0, 0);
            cellBuilder
                .AddParagraph(TicketData.Company + " company").SetFont(FNT9);
            cellBuilder
                .AddParagraph("Boarding pass").SetFont(FNT15);
            cellBuilder
                .AddParagraph("Passenger's coupon").SetFont(FNT8_G);
        }

        private void AddLogoImage(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddImage(Path.Combine("images", "BP_Logo_2x.png"),
                XSize.FromHeight(50));
        }

        private void FillBarTableCell(TableCellBuilder cellBuilder)
        {
            AddETK(cellBuilder);
            cellBuilder
                .AddImage(Path.Combine("images", "BP_barcode_2x.png"));
        }

        private void AddETK(TableCellBuilder cellBuilder)
        {
            cellBuilder
                .SetFont(FNT9)
                .AddParagraphToCell("ETK: " + TicketData.ETK);
            var paragraphBuilder =
                cellBuilder.AddParagraph("Reg No: " + TicketData.RegNo);
            paragraphBuilder.SetMarginBottom(4);
        }

        private void FillBoardingHandBugTable(TableBuilder tableBuilder, BoardingCell[,] boardingItems)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnToTable("", 415.5f)
                .AddColumn("", 138.5f);
            FillBoardingTableFirstRow(tableBuilder, boardingItems[0, 0]);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.AddCell().AddTable(builder => 
            {
                builder.SetWidth(415.5f);
                FillBoardingTable(builder, boardingItems, 1);
            });
            rowBuilder.AddCell(FillHandBugTableCell);
        }

        private void FillBoardingTableFirstRow(TableBuilder tableBuilder,
                BoardingCell bi)
        {
            for (int k = 0; k < 2; k++)
            {
                var rowBuilder = tableBuilder.AddRow();
                if (k == 1)
                { 
                    rowBuilder.ApplyStyle(
                        StyleBuilder.New()
                            .SetBorderBottom(0.5f, Stroke.Solid, Color.Black)
                            .SetPaddingBottom(6)
                        );
                }
                var cellBuilder = rowBuilder.AddCell();
                cellBuilder.SetColSpan(2);
                if (k == 0)
                {
                    cellBuilder.SetFont(FNT9).AddParagraph(bi.name);
                }
                else
                {
                    if (bi.image != null)
                    {
                        cellBuilder.AddTable(builder => {
                            ImageThenText(builder, bi);
                        });
                    }
                    else
                    {
                        TextOnly(cellBuilder.AddParagraph(), bi);
                    }
                }
            }
        }

        private void FillBoardingTable(TableBuilder tableBuilder,
                BoardingCell[,] boardingItems, int startRow = 0)
        {
            tableBuilder
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercent("", 25);
            int rows = boardingItems.GetLength(0);
            int columns = boardingItems.GetLength(1);
            for (int i = startRow; i < rows; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    var rowBuilder = tableBuilder.AddRow();
                    if (k == 0)
                    {
                        rowBuilder.ApplyStyle(
                            StyleBuilder.New()
                                .SetPaddingTop(4)
                            );
                    }
                    else if (i < rows - 1)
                    {
                        rowBuilder.ApplyStyle(
                            StyleBuilder.New()
                                .SetBorderBottom(0.5f, 
                                    Stroke.Solid, Color.Black)
                                .SetPaddingBottom(4)
                            );
                    }                    
                    for (int j = 0; j < columns; j++)
                    {
                        BoardingCell bi = boardingItems[i, j];
                        if (!bi.isEmpty())
                        {
                            var cellBuilder = rowBuilder.AddCell();
                            if (bi.colSpan > 1)
                            {
                                cellBuilder.SetColSpan(bi.colSpan);
                            }
                            if (k == 0)
                            {
                                cellBuilder
                                    .AddParagraph(bi.name).SetFont(FNT9);
                            }
                            else
                            {
                                if (bi.image != null)
                                {
                                    cellBuilder.AddTable(builder => {
                                        ImageThenText(builder, bi);
                                    });
                                }
                                else
                                {
                                    TextOnly(cellBuilder.AddParagraph(), bi);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ImageThenText(TableBuilder tableBuilder, BoardingCell bi)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnToTable("", 13)
                .AddColumn("");
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.AddCell()
                .SetPadding(0, 4, 0, 0)
                //.SetVerticalAlignment(VerticalAlignment.Bottom)
                .AddImage(Path.Combine("images", bi.image),
                    XSize.FromWidth(11));
            TextOnly(rowBuilder.AddCell().AddParagraph(), bi);
        }

        private void TextOnly(ParagraphBuilder paragraphBuilder, BoardingCell bi)
        {
            foreach(FontText ft in bi.fontTexts)
            {
                paragraphBuilder.AddText(ft.text).SetFont(ft.font);
            }
        }

        private void FillHandBugTableCell(TableCellBuilder cellBuilder)
        {
            cellBuilder
                .SetPadding(19, 6, 0, 0)
                .AddParagraph("Hand baggage allowance")
                .SetFont(FNT9)
                .SetMarginBottom(19);
            cellBuilder.AddImage(
                Path.Combine("images", "BP_handbag_2x.png"),
                XSize.FromHeight(108));
        }

        private void AddWhatsNext(SectionBuilder sectionBuilder)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph("What's next?");
            paragraphBuilder.SetMarginTop(42).SetFont(FNT11B);
            sectionBuilder.AddLine(PageWidth, 2, Stroke.Solid);
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 50)
                .AddColumnPercent("", 50);
            int halfSize = WhatsNextData.Count - WhatsNextData.Count / 2;
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder.SetPadding(0, 6, 4, 0).SetFont(FNT8);
            FillWhatNextHalf(0, halfSize, cellBuilder);
            cellBuilder = rowBuilder.AddCell();
            cellBuilder.SetPadding(4, 6, 0, 0).SetFont(FNT8);
            FillWhatNextHalf(halfSize, WhatsNextData.Count, cellBuilder);
            paragraphBuilder = 
                sectionBuilder.AddParagraph("Have a good flight!");
            paragraphBuilder
                .SetAlignment(HorizontalAlignment.Center)
                .SetMarginTop(20)
                .SetMarginBottom(30)
                .SetFont(FNT17);
            sectionBuilder
                .AddLine(PageWidth, 0.5f, Stroke.Dashed).SetMarginBottom(24);
        }

        private void FillWhatNextHalf(int start, 
            int end, TableCellBuilder cellBuilder)
        {
            for (int i = start; i < end; i++)
            {
                cellBuilder.AddParagraph(WhatsNextData[i]).SetMarginBottom(8f);
            }
        }


        private void FillBottomLogoBarBoardingTable(TableBuilder tableBuilder,
            BoardingCell[,] boardingItems)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 89.5f)
                .AddColumnPercent("", 10.5f);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder.AddTable(FillBottomLogoETKTable);
            cellBuilder.AddTable(builder =>
            {
                builder.SetWidth(415.5f);
                FillBoardingTable(builder, boardingItems, 0);
            });
            rowBuilder.AddCell(FillBottomBarTableCell);
        }

        private void FillBottomLogoETKTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(96))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 8)
                .AddColumnPercentToTable("", 72)
                .AddColumnPercent("", 20);
            var rowBuilder = tableBuilder.AddRow();
            AddLogoImage(rowBuilder.AddCell());
            AddCompanyName(rowBuilder.AddCell());
            AddETK(rowBuilder
                .AddCell().SetHorizontalAlignment(HorizontalAlignment.Right));
        }

        private void FillBottomBarTableCell(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddImage(Path.Combine("images", "BP_barcode_vert_2x.png"),
                XSize.FromHeight(276));
        }

        private BoardingCell[,] GetBoardingItems()
        {
            BoardingCell[,] result =
            {
                {
                new BoardingCell("Passenger name", FNT18,
                    TicketData.Passenger, 4),
                    EMPTY_ITEM,
                    EMPTY_ITEM,
                    EMPTY_ITEM
                },
                {
                new BoardingCell("From", new FontText[] {
                    new FontText (FNT12, BoardingData.DepartureAirport + " / "),
                    new FontText (FNT12B, BoardingData.DepartureAbvr)
                }, 2),
                EMPTY_ITEM,
                new BoardingCell("To", new FontText[] {
                    new FontText (FNT12, BoardingData.ArrivalAirport + " / "),
                    new FontText (FNT12B, BoardingData.ArrivalAbvr)
                }, 2),
                EMPTY_ITEM
                },
                {
                new BoardingCell("Flight", FNT16_R, BoardingData.Flight),
                new BoardingCell("Gate", FNT16, BoardingData.BoardingGate),
                new BoardingCell("Class", FNT16, BoardingData.Class + " "
                    +  BoardingData.ClassAdd),
                new BoardingCell("Seat", FNT16_R, BoardingData.Seat,
                    "BP_seat_2x.png")
                },
                {
                new BoardingCell("Date", FNT16,
                    BoardingData.DepartureTime.ToString(
                                "dd MMMM", DocumentLocale)),
                new BoardingCell("Boarding time till", FNT16,
                    BoardingData.BoardingTill.ToString(
                                "HH:mm", DocumentLocale)),
                new BoardingCell("Departure time", FNT16,
                    BoardingData.DepartureTime.ToString(
                                "HH:mm", DocumentLocale)),
                new BoardingCell("Arrive", FNT16,
                    BoardingData.Arrival.ToString(
                                "HH:mm", DocumentLocale))
                }
            };
            return result;
        }

        internal struct BoardingCell
        {
            internal string name;
            internal FontText[] fontTexts;
            internal string image;
            internal int colSpan;
            internal BoardingCell(string name, FontBuilder font, string text, int colSpan = 1) : this (name, font, text, null, colSpan)
            {
            }
            internal BoardingCell(string name, FontBuilder font, string text, string image, int colSpan = 1)
            {
                this.name = name;
                fontTexts = new FontText[] { new FontText(font, text) };
                this.image = image;
                this.colSpan = colSpan;
            }
            internal BoardingCell(string name, FontText[] fontTexts, int colSpan = 1)
            {
                this.name = name;
                this.fontTexts = fontTexts;
                this.image = null;
                this.colSpan = colSpan;
            }
            internal bool isEmpty()
            {
                return fontTexts.Length == 0;
            }
        }

        internal struct FontText
        {
            internal FontBuilder font;
            internal string text;
            internal FontText(FontBuilder font, string text)
            {
                this.font = font;
                this.text = text;
            }
        }
    }
}