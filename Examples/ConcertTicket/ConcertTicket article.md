##### Example: ConcertTicket

# Purpose
The ConcertTicket project is an example of generation of a Concert Ticket document. The example demonstrates how to create a document based on several tables built with using `colSpan` and `rowSpan` parameters.
The example also demonstrates how to locate images and data from a data source in the table cells.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/ConcertTicket).

![](../Articles%20Images/ConcertTicket.png "The page")

# Prerequisites
1) **Visual Studio 2017** or later is installed.
   To install a community version of Visual Studio, use the following link: https://visualstudio.microsoft.com/vs/community/.
   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or later is installed.
   To install the framework, use the following link: https://dotnet.microsoft.com/download.

# Description

### Table data
The values for the ticket details and personal data fields are located in the **Content/ticket-data.json** file:
```json
{
  "Eticket": "000385724",
  "Admission": "General admission",
  "TicketType": "Adult",
  "Price": "$45",
  "Name": "John Smith",
  "Venue": "Concert Hall",
  "Address": "205 1st St."
}

```

The data for the rules, event description, and location instructions fields are located in the **Content/concert-data.json** file:
```json
{
    "titlerulesofpurchase": "Rules of purchase",
    "titlerulesofattendance": "Rules of attendance",
    "rulesofpurchase": "No refunds or exchanges. Unused tickets may be converted to a tax - deductible donation. All photography, video and audio recording is prohibited. Cameras and tape or video recorders are not allowed in the theater due to copyright law.",
    "rulesofattendance": [
        "1. Do not stand up in front of people who are sitting down",
        "2. Do not take up too much space in a pit/GA section",
        "3. DO NOT force your way to the front of the stage",
        "4. Let people pass through if they are trying to exit",
        "5. Make friends with those around you",
        "6. Let shorter people stand in front of you",
        "7. Sharing is caring!",
        "8. Do not interrupt those sitting near you with a drink run every other song",
        "9. Do not talk loudly during the concert",
        "10. Rock out and have fun!"
    ],

    "titlebandslist": "Program/Bands List/Event Description",
    "bandslist": "A City literally built on Rock n' Roll in Cadott WI, Rock Fest is the true Rock experience you can't miss. In its 26th year, it is THE top venue for people of all ages to come together for one common purpose: to congregate with other rock fans from across the world, in a place where rock music still matters. Featuring the very best of active and classic rock and legendary names in Rock Music, entertainment and experience are the first priority. Aerosmith, Iron Maiden, Avenged Sevenfold, Kiss, Motley Crue, Fleetwood Mac, Tom Petty, Kid Rock, Shinedown, Five Finger Death Punch, Rob Zombie, Korn & many more rock legends have graced this permanent Main Stage over the course of the last two and a half decades.",

    "titlehowtofind": "How to find us",
    "howtofind": "Concert Hall is located three blocks west of the park and three blocks north the library. Free parking is available nearby, which fills up close to showtime, and on the streets around. Bike racks are located outside the main entrance to the Hall.",

    "titlelearn": "Learn more at",
    "facebook": "Facebook /ConcertHall",
    "twitter": "Twitter @ConcertHall",
    "instagram": "Instagram @ConcertHall",
    "counterfoil": "This is your ticket. Print this Entire page, fold it and bring it with you to the event. Please make sure the QR code is visible."
}
```

### Concert poster
The concert poster is located in the **images/CT_Poster.png** file.

### QR code
The ticket QR code is located in the **images/Qr_code.png** file.

### Output file
The example creates the **ConcertTicket.pdf** file in the output **bin/(Debug|Release)/netcoreapp2.1** folder.


# Writing the source code

#### 1. Create a new console application.
1.1.    Run Visual Studio.  
1.2.    Select **File** > **Create** > **Console Application (.Net Core)**.

#### 2. Modify the class Program.
2.1. Set the path to the output PDF file in the `Main()` function:

```c#
	Parameters parameters = new Parameters(null, "ConcertTicket.pdf")
```
2.2. To generate the Concert Ticket document and to build it into the output file, call the `Run()` and `Build()` methods:

```c#
	ConcertTicket.Run().Build(parameters.file);
```
2.3. After the generation and building is completed, notify the user about it:

```c#
Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                        + "\" document has been successfully built");
```
#### 3. Create a class for running the document generation.

3.1. Create a `ConcertTicketRunner` class:

```c#
public static class ConcertTicketRunner	
{
}
```

3.2. Add the `CheckFile` method to the `ConcertTicketRunner` class:

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

This method is responsible for checking if the file exists. If not, an instance of `IOException` is thrown.

3.3. Add the `Run` method to the `ConcertTicketRunner` class:

```c#
public static DocumentBuilder Run()
    {
        // The method flow is described below
    }
```

3.3.1. First, we read the JSON data from the files:

```c#
string ticketJsonFile = CheckFile(Path.Combine(
    "Content", "ticket-data.json"));
string ticketJsonContent = File.ReadAllText(ticketJsonFile);
TicketData ticketData =
    JsonConvert.DeserializeObject<TicketData>(ticketJsonContent);
string jsonFile = CheckFile(Path.Combine(
    "Content", "concert-data.json"));
string jsonContent = File.ReadAllText(jsonFile);
ConcertData concertData =
    JsonConvert.DeserializeObject<ConcertData>(jsonContent);
```

3.3.2. Then, we create an instance of the `ConcertTicketBuilder` class and pass the data to it:

```c#
ConcertTicketBuilder ConcertTicketBuilder =
    new ConcertTicketBuilder();
ConcertTicketBuilder.TicketData = ticketData;
ConcertTicketBuilder.ConcertData = concertData;
```

3.3.3 Finally, we return the instance of the `ConcertTicketBuilder` class so that we can save the document later:

```c#
return ConcertTicketBuilder.Build();
```

#### 4. Build the document structure in the ConcertTicketBuilder class.

4.1. Define the culture info, page orientation, and margins variables:

```c#
        internal static readonly CultureInfo DocumentLocale  
            = new CultureInfo("en-US");
        internal const PageOrientation Orientation 
            = PageOrientation.Portrait;
        internal static readonly Box Margins  = new Box(30, 27, 30, 27);
        internal static readonly XUnit PageWidth = 
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));
```

4.2. Define the font variables:

```c#
        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9f);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12f);
        internal static readonly FontBuilder FNT12B = Fonts.Helvetica(12f).SetBold(true);
        internal static readonly FontBuilder FNT20 = Fonts.Helvetica(20f);
        internal static readonly FontBuilder FNT19B = Fonts.Helvetica(19f).SetBold();
```

4.3.  Define the data properties:

```c#
		public TicketData TicketData { get; internal set; }
		public ConcertData ConcertData { get; internal set; }
```

4.4. Build the document into an instance of the `ConcertTicketBuilder` class:

```c#
        internal DocumentBuilder Build()
        {
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            var concertSection = documentBuilder.AddSection();
            concertSection
                 .SetOrientation(Orientation)
                 .SetMargins(Margins);
            return documentBuilder;
        }
```

Here we create an instance of the `DocumentBuilder` class, add a section to it,
set the section margins and page orientation,
and fill it with the content.

4.5. To fill the header bar, we need to add the following:

```c#
      private void concertTable(SectionBuilder section)
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
            var row2Builder = concertTable.AddRow();
```
Here we add four columns to the table, 20%, 30%, 20%, and 30% in width respectively. 
Then we add two rows that will make our header.

4.6. In the first cell of the first row, we add the concert poster image with `rowSpan` = 2 because the cell spans two rows.

```c#
            var row1Builder = concertTable.AddRow();
            AddLogoImage(row1Builder.AddCell("", 0, 2));
```

4.7. The poster image is loaded from the local file **images/CT_Poster.png**:

```c#
        private void AddLogoImage(TableCellBuilder cellBuilder)
        {
            cellBuilder
                .SetPadding(2, 2, 2, 0);
            cellBuilder
                .AddImage(Path.Combine("images",
                     "СT_Poster.png")).SetHeight(340); 
        }
```
4.8. In the second cell of the first row, we place the concert name, date, and a line. We set `colSpan`= 3 here because the cell spans three columns.

We also set paddings to create an empty space after the poster on the left and between the table and the ticket data at the bottom.

```c#
            var row1Builder = concertTable.AddRow();
            AddLogoImage(row1Builder.AddCell("", 0, 2));
            AddConcertData(row1Builder.AddCell("", 3, 0)
                .SetPadding(32, 0, 0, 8));
```

4.9. Here we create two paragraphs with the necessary font and create a line by configuring the table border.

To create the line in the table cell, we set the stroke style and width for the lower border of the cell using the methods `.SetBorderStroke()` and `.SetBorderWidth()`

```c#
        private void AddConcertData(TableCellBuilder cellBuilder)
        {
            cellBuilder
                .AddParagraph("Nick Cave and the Bad Seeds").SetFont(FNT19B);
            cellBuilder
                .AddParagraph("25.05.2021  7:30PM").SetFont(FNT12)
                .SetBorderStroke(strokeLeft: Stroke.None, strokeTop: Stroke.None, strokeRight: Stroke.None, strokeBottom: Stroke.Solid)
                .SetBorderWidth(2);
            cellBuilder
                .SetBorderWidth(widthLeft: 1, widthBottom: 2, 
                                widthRight: 1, widthTop: 1);
        }
```

4.10. Now we fill the concert data in the second row. For correct generation of the table, as we use the row span, we add an empty cell to have four cells in the row.

The second cell will contain the ticket number and the QR code. The third cell will contain the field names for the ticket properties, and the fourth cell - the personal data.

```c#
            var row2Builder = concertTable.AddRow();
            row2Builder.AddCell();
            No(row2Builder.AddCell("").SetFont(FNT10)
                .SetPadding(32, 0, 0, 0));
            FillTicketData(row2Builder.AddCell());
            FillPersonalInfo(row2Builder.AddCell());
```

4.11. To add the number, we take the data from `TicketData.Eticket`. To add the QR code, we import the **images/Qr_code.png** image. Do not forget to set the image height.

```c#
        private void No(TableCellBuilder cellBuilder)
        {
            cellBuilder
               .AddParagraph("E - ticket");
            cellBuilder
                .AddParagraph(TicketData.Eticket).SetLineSpacing(1.5f); 
            cellBuilder
                .AddImage(Path.Combine("images", "Qr_Code.png")).SetHeight(100);
        }
```

4.12. Then we add the ticket properties to the third column.

```c#
        private void FillTicketData(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph("Admission").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Ticket type").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Price").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Name").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Venue").SetLineSpacing(1.4f);
            cellBuilder.AddParagraph("Address").SetLineSpacing(1.4f);
        }
```

4.13. Add the relevant information to the adjacent column in the same order. We configure the same line spacing everywhere.

```
        private void FillPersonalInfo(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(TicketData.Admission).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.TicketType).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Price).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Name).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Venue).SetLineSpacing(1.4f);
            cellBuilder.AddParagraph(TicketData.Address).SetLineSpacing(1.4f);
        }
```

4.14. Now as the header bar is completed, our next step is to create a table with the concert rules and location instructions.

This is what the concert table consists of. 

![](../Articles%20Images/ConcertTicketHeader.png "The page")

4.15. We create the infoTable. We set the top margin to create an empty space under the header. We also set the width for three columns (50%, 25%, and 25%), and add two rows.

```c#
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
            var row4Builder = infoTable.AddRow();

        }
```

4.16. Two cells of the first row will contain the rules of attendance and purchase.

First, we add the title from `ConcertData.TitleRulesOfAttendance`.

To represent the rules of attendance as a list, we create a cycle adding each item of the list from `start` to `end`.

We also set the borders stroke style making the right border invisible and set margins to create an empty space.

```c#
       private void FillRuleA(int start, int end, TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(ConcertData.TitleRulesOfAttendance)
            		.SetFont(FNT12B).SetMargins(10, 10, 1, 4);
            cellBuilder.SetBorderStroke(strokeLeft: Stroke.Solid, 
            		strokeTop: Stroke.Solid, strokeRight: Stroke.None, strokeBottom: Stroke.Solid);

            foreach (var item in ConcertData.RulesOfAttendance)
            {
                cellBuilder.AddParagraph(item).SetFont(FNT9).SetMargins(20, 0, 10, 2);
            }
```

4.17. Now we add the title from `ConcertData.TitleRulesOfPurchase`, set the border stroke style to make the left border invisible, and add the rules of purchase from `ConcertData.RulesOfPurchase`.

```c#
        private void FillRuleP(TableCellBuilder cellBuilder)
        {
           cellBuilder.AddParagraph(ConcertData.TitleRulesOfPurchase)
                	.SetFont(FNT12B).SetMargins(10, 10, 1, 4);
           cellBuilder.SetBorderStroke(strokeLeft: Stroke.None, 
                    strokeTop: Stroke.Solid, strokeRight: Stroke.Solid, strokeBottom: Stroke.Solid);            		
           cellBuilder.AddParagraph(ConcertData.RulesOfPurchase)
                .SetFont(FNT9).SetLineSpacing(1.2f).SetMargins(10, 0, 10, 4);    
        }
```

4.18. Add the rules to the table and set `colSpan` for the second cell to span two 25% columns.

```c#
            var row3Builder = infoTable.AddRow();
            FillRuleA(start: 0, end: 10, row3Builder.AddCell("").SetFont(FNT10));
            FillRuleP(row3Builder.AddCell("", 2, 0).SetFont(FNT10));
```

4.19. Now we configure the cell with the program description: add the title and the text, and set margins.

```c#
        private void FillBandlist(TableCellBuilder cellBuilder)
        {
            cellBuilder.SetBorderStroke(Stroke.None);
            cellBuilder.AddParagraph(ConcertData.TitleBandsList).SetFont(FNT12B).SetMargins(0, 20, 1, 4);
            cellBuilder.AddParagraph(ConcertData.BandsList).SetFont(FNT9).SetLineSpacing(1.2f).SetMargins(0, 0, 30, 4);
            cellBuilder.AddParagraph("");
        }
```

4.20. Then we create the third cell. Here we add the info from `ConcertData`: the title, information, and social media.

```c#
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
```

4.21. Now we should complete the row in the infoTable and add the image from the file **images/CT_Location.png** to the second cell.

```c#
			var row4Builder = infoTable.AddRow();
            FillBandlist(row4Builder.AddCell("").SetFont(FNT12));
            row4Builder.AddCell("")
                .AddImage(Path.Combine("images", "CT_Location.png")).SetHeight(400)
                .SetMarginTop(9);
            AddContactInfo(row4Builder.AddCell("").SetFont(FNT12));
```

This is how the infoTable looks.

![](../Articles%20Images/ConcertTicketBody.png "The page")


4.22. To complete the Concert Ticket document, we create the counterFoil table. We set the top margin and the width for four columns. The first cell is a half of the page width, it contains some data and an image, the other three columns are similar to those in the header.

```c#
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
            var row6Builder = counterFoil.AddRow();
            var row7Builder = counterFoil.AddRow();
        }
```

4.23. The first row contains a cell with a paragraph and a cell with the concert name as in the header.

To create the first cell, we take data from **ConcertData.json**.

```c#
        private void YourTicket(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph(ConcertData.CounterFoil).SetFont(FNT9).SetMarginRight(30);
        }
```

4.24. To complete the row, we simply use the existing `AddConcertData`. We also set the top padding to put the paragraph on the same level with the concert name. 

```c#
            var row5Builder = counterFoil.AddRow();
            YourTicket(row5Builder.AddCell("")
                .SetPadding(0, 2, 0, 0));
            AddConcertData(row5Builder.AddCell("", 3, 0));
```

4.25. Now we configure the cells with information about the ticket owner as we did in the header.

```c#
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
```

4.26. Finally, we complete the row. 

To do this, we add the information to the cells, the scheme from **images/CT_Scheme.png**, then cells with the ticket properties and ticket info, and the last cell contains the QR code from the **images/Qr_code.png** file.

```c#
            var row6Builder = counterFoil.AddRow();
            row6Builder.AddCell()
                .AddImage(Path.Combine("images", "CT_Scheme.png")).SetHeight(100);
            FillTicketDataCounterFoil(row6Builder.AddCell());
            FillPersonalInfoCounterFoil(row6Builder.AddCell());
            row6Builder.AddCell()
                .AddImage(Path.Combine("images", "Qr_Code.png")).SetWidth(153);
```

4.27. In the last 7th row, we have only the ticket number under the QR code image. We create three empty cells and one cell with the ticket number.

```c#
            var row7Builder = counterFoil.AddRow();
            row7Builder.AddCell();
            row7Builder.AddCell();
            row7Builder.AddCell();
            row7Builder.AddCell(TicketData.Eticket).SetFont(FNT10);
```

This is what we get in the counterFoil table.

![](../Articles%20Images/ConcertTicketFooter.png "The page")

#### 5.  The generated **PDF file** should look as shown below:

The resulting **ConcertTicket.pdf** document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/ConcertTicket.pdf).