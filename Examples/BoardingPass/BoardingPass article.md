##### Example: Receipt

# Purpose
The BoardingPass project is an example of the boarding pass generation. The example demonstrates a simple one-page document that includes a lot of small tables, images and paragraphs. This example also demonstrates the usage of margins and lines.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/BoardingPass).

# Prerequisites
1) **Visual Studio 2017** or above is installed.
   To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
   To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### Table data
The values for the passenger table fields are located in the **Content/boarding-data.json** file:
```json
{
  "Flight": "S7 168",
  "DepartureAirport": "Istanbul",
  "DepartureAbvr": "SAW",
  "BoardingGate": "G21",
  "BoardingTill": "2020-06-16T06:35:00",
  "DepartureTime": "2020-06-16T07:05:00",
  "Arrival": "2020-06-16T07:30:00",
  "ArrivalAirport": "New York",
  "ArrivalAbvr": "JFK",
  "Class": "Econom",
  "ClassAdd": "(T)",
  "Seat": "12D"
}
```

The values for the company table fields are located in the **Content/bp-ticket-data.json** file:
```json
{
  "Company": "Sample",
  "Site": "https:\\sampleairlines.com",
  "SiteTitle": "sampleairlines.com",
  "Passenger": "PAVEL REMPEL",
  "ETK": "1234567890/1",
  "RegNo": "123"
}
```

The values for the "What's next" table fields are located in the **Content/whats-next.json** file:
```json
[
  "Please, arrive at the airport in advance, taking into account the time required for baggage check-in, preflight screening, passport and customs control.",
  "You may check-in your baggage at the check-in counter at the airport. Please note the baggage transportation rules. If the weight, width or height of your baggage exceeds the free baggage allowance, you have to pay for excess baggage.",
  "If you carry cabin baggage only, please apply to the check-in counter to get a cabin baggage tag.",
  "If after online check-in you decide to change or return your ticket, you have to cancel your check-in via the web-site at least 1 hours prior to the departure and apply to the place of purchase of your ticket.",
  "To ensure flight safety the airline reserves right to change your seat onboard if required so by the pilot in command.",
  "If you don't have an opportunity to print it out, you may apply to the check-in counters at the airport. For domestic flights departing from JFK airport you may show your boarding pass on the screen of an electronic device.",
  "The current status of any flight is stated on the online timetable at sampleairlines.com",
  "The boarding closes on time stated on your boarding pass. Late passengers are not accepted for transportation."
]
```

### Barcodes
The pictures of the boarding pass barcodes are located in the `images/BP_barcode_2x.png` and the `images/BP_barcode_vert_2x.png` files.

### Company logo
The picture of the company's logo is located in the `images/BP_Logo_2x.png` file.

### Hand baggage allowance
The picture of the company's hand baggage allowance is located in the `images/BP_handbag_2x.png` file.

### Seat icon
The picture of the seat icon is located in the `images/BP_seat_2x.png` file.

### Output file
The example creates the **BoardingPass.pdf** file in the output **bin/(Debug|Release)/netcoreapp2.1** folder.


# Writing the source code

#### 1. Create new console application.
1.1.	Run Visual Studio  
1.2.	File -> Create -> Console Application (.Net Core)

#### 2. Modify class Program.
2.1. Set the path to the output PDF-file in the **Main()** function:

```c#
    Parameters parameters = new Parameters(null, "BoardingPass.pdf");
```
2.2. Call the **Run()** method for the boarding pass generation and the **Build()** for building a document into the output PDF-file:

```c#
    BoardingPassRunner.Run().Build(parameters.file);
```
2.3. After the generation is completed, notify user regarding successful generation:
```c#
    Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                        + "\" document has been successfully built");
```


#### 3. Create class for running document generation

3.1. Create the boarding pass runner:

```c#
public static class BoardingPassRunner
{
}
```

3.2. Add the `CheckFile` method to the runner class:
```c#
    private static string CheckFile(string file)
    {
        if (!File.Exists(file))
        {
            throw new IOException("File not found: " + Path.GetFullPath(file));
        }
        return file;
    }
```
This method is responsible for checking if the file exists. If not - an instance of `IOException` will be thrown.

3.3. Add the `Run` method to the runner class:
```c#
    public static DocumentBuilder Run()
    {
        // Method flow is described below
    }
```

3.3.1. Firstly, we read the JSON data from the files:
```c#
string ticketJsonFile = CheckFile(
    Path.Combine("Content", "bp-ticket-data.json"));
string boardingJsonFile = CheckFile(
    Path.Combine("Content", "boarding-data.json"));
string whatsNextJsonFile = CheckFile(
    Path.Combine("Content", "whats-next.json"));
string ticketJsonContent = File.ReadAllText(ticketJsonFile);
string boardingJsonContent = File.ReadAllText(boardingJsonFile);
string whatsNextJsonContent = File.ReadAllText(whatsNextJsonFile);
TicketData ticketData = 
    JsonConvert.DeserializeObject<TicketData>(ticketJsonContent);
BoardingData boardingData = 
    JsonConvert.DeserializeObject<BoardingData>(boardingJsonContent);
List<string> whatsNextData = 
    JsonConvert.DeserializeObject<List<string>>(whatsNextJsonContent);
```

3.3.2. Secondly, we create an instance of the `BoardingPassBuilder` class and pass the data into it:
```c#
BoardingPassBuilder boardingPassBuilder = 
    new BoardingPassBuilder();
boardingPassBuilder.TicketData = ticketData;
boardingPassBuilder.BoardingData = boardingData;
boardingPassBuilder.WhatsNextData = whatsNextData;
```

3.3.3. In the end, we return an instance of the `DocumentBuilder` class so we can save a document later:
```c#
return boardingPassBuilder.Build();
```

#### 4. Build the document structure in the BoardingPassBuilder class

4.1. Define the font variables:

```c#
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
```

4.2. Define the other variables (culture info, page orientation, margins, page width and empty cell template):

```c#
        internal static readonly CultureInfo DocumentLocale  
            = new CultureInfo("en-US");
        internal const PageOrientation Orientation 
            = PageOrientation.Portrait;
        internal static readonly Box Margins  = new Box(29, 20, 29, 20);
        internal static readonly XUnit PageWidth = 
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));
        internal static readonly BoardingCell EMPTY_ITEM = new BoardingCell("", new FontText[0]);
```

4.3. Define the data properties:

```c#
        public BoardingData BoardingData { get; internal set; }
        public TicketData TicketData { get; internal set; }
        public List<string> WhatsNextData { get; internal set; }
```

4.4. Build the document into an instance of the DocumentBuilder class:

```c#
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
```
Here we create a DocumentBuilder instance, add a section into it,
set the margins and orientation of section,
and fill it with the content.

4.5. To fill the header bar, we need to add the following:

```c#
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
```
Here we setup the table without borders and with 2 columns, 25% and 75% in width respectively.
In the document, this block will be represented as logo picture on the top left and a bar-code on the top right.

4.6. In the first cell of the table-in-table created above, we create a boarding pass logo alongside with a company name.

```c#
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
```
To place the logo picture, we take the first column, 25% in width.
To place the company name and the document name, we take the second column, 75% in width.

4.7. Logo picture is loaded from the local file named `BP_Logo_2x.png`:

```c#
        private void AddLogoImage(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddImage(Path.Combine("images", "BP_Logo_2x.png"),
                XSize.FromHeight(50));
        }
```

4.8. Company name is taken from the `TicketData.Company` property that was loaded before from the JSON file:

```c#
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
```

4.9. To fill the second cell of the header bar, 25% in width, we use the following method:

```c#
        private void FillBarTableCell(TableCellBuilder cellBuilder)
        {
            AddETK(cellBuilder);
            cellBuilder
                .AddImage(Path.Combine("images", "BP_barcode_2x.png"));
        }
```

4.10. There are two text labels above the bar code. They are taken from the `TicketData.ETK` and the `TicketData.RegNo` properties respectively:

```c#
        private void AddETK(TableCellBuilder cellBuilder)
        {
            cellBuilder
                .SetFont(FNT9)
                .AddParagraphToCell("ETK: " + TicketData.ETK);
            var paragraphBuilder =
                cellBuilder.AddParagraph("Reg No: " + TicketData.RegNo);
            paragraphBuilder.SetMarginBottom(4);
        }
```

4.11. Header bar is completed. The next step is to create a boarding table.

![Fig. 1](../Articles%20Images/BoardingTableSchema.png "Boarding Table Schema")
This is what the boarding table consists of. This is exactly the green table,
that has the first row as a col-span and the second row with 2 cells,
each of cell contains an inner table (so-called "table-in-table").

```c#
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
```

4.12. To fill the first row of the boarding table, we create the following method:

```c#
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
```

4.13. To fill the rest of the boarding table, we create the following:

```c#
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
```
Here we create the red table without borders and with 4 columns of 25% width each.
Then we dynamically fill the table with the data from the JSON files.
We also use the col-spans functionality to merge some cells.

4.14. To fill the cell, where we expect to have an image before a text, we create:

```c#
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
```
On the inside, this method setups a table with 2 columns, the first of which contains an image and the second one contains a text label.
We don't want to use the inline image functionality because we won't be able to control the vertical alignment of the text and the image separately.

4.15. The following method is responsible only for placement of a text in a cell:

```c#
        private void TextOnly(ParagraphBuilder paragraphBuilder, BoardingCell bi)
        {
            foreach(FontText ft in bi.fontTexts)
            {
                paragraphBuilder.AddText(ft.text).SetFont(ft.font);
            }
        }
```
We can have multiple formatting options for the same paragraph instance, that's why we can have both the regular and bold text in the same text line, not only in the separate paragraph object.
As you can see, we call the `.AddText(...)` method that returns the `FormattedTextElementBuilder`,
on which we can call the formatting methods, such as `.SetFont(...)`.

4.16. To create the "Hand baggage allowance" block on the right side, we have this method:

```c#
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
```
Let's remember, what we did in 4.11 section. We finished the creation of the red table. The next step was to create a dark yellow table. That's what we did in the example above.
Note that you can use the margins inside a cell to move the text block or any other element:
here we used the bottom margin to move the "Hand baggage allowance" text label.

4.17. To create the "What's next?" block, we use the following method:

```c#
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
            cellBuilder.SetPadding(0, 6, 5, 0).SetFont(FNT8);
            FillWhatNextHalf(0, halfSize, cellBuilder);
            cellBuilder = rowBuilder.AddCell();
            cellBuilder.SetPadding(5, 6, 0, 0).SetFont(FNT8);
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
```
The "What's next" block also consists of a table without borders. In the end,
we create a dashed line.

4.18. We use this method to separate the "What's next" content with "empty lines":
```c#
        private void FillWhatNextHalf(int start, 
            int end, TableCellBuilder cellBuilder)
        {
            for (int i = start; i < end; i++)
            {
                cellBuilder.AddParagraph(WhatsNextData[i]).SetMarginBottom(8f);
            }
        }
```

4.19. To fill the vertical bar code, we create one more table with the fixed width, 415.5pt, and
with 2 columns, 89.5% and 10.5% in width:
```c#
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
```
Here we use the action-style method `.AddTable` with lambda expression to be able to call
the `FillBoardingTable(...)` method. This is one of purposes why you may need to use the
action-style methods.

4.20. Then, to fill the "ETK" and "Reg No" text blocks, we use the following method:
```c#
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
```
We set the right alignment, so our text blocks are aligned to the right, just before the
vertical bar code picture.

4.21. The last method we use in this document is:
```c#
        private void FillBottomBarTableCell(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddImage(Path.Combine("images", "BP_barcode_vert_2x.png"),
                XSize.FromHeight(276));
        }
```
It just fills the bar code cell created in 4.19 with the corresponding image. The image's height
is set to 276px.

#### 5. The generated **PDF file** must look as shown below:
The resulting BoardingPass.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/BoardingPass.pdf).