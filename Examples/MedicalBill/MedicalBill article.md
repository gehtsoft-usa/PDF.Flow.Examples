##### Example: MedicalBill

**MedicalBill** project is an example of creating a **“Medical Bill”** document. The example shows how to create a complex document that includes tables, nested tables, and lists. Also it shows one of the ways to display forms for manual filling of a printed document.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/MedicalBill).


**Table of Content**  

- [Prerequisites](#prerequisites)
- [Purpose](#purpose)
- [Description](#description)
- [Output file](#output-file)
- [Open the project in Visual Studio](#1-open-the-project-in-visual-studio)
- [Run the sample application](#2-run-the-sample-application)
- [Source code structure](#3-source-code-structure)
- [The Program class](#4-class-program)
- [The MedicalBillRunner class](#5-the-medicalbillrunner-class)
- [The MedicalBillBuilder class](#6-the-medicalbillbuilder-class)
- [The MedicalBillFrontBuilder class](#7-the-medicalbillfrontbuilder-class)
- [The MedicalBillBackBuilder class](#8the-medicalbillbackbuilder-class)
- [The MedicalBillHeadBuilder class](#9-the-medicalbillheadbuilder-class)
- [The MedicalBillBodyBuilder class](#10-the-medicalbillbodybuilder-class)
- [The MedicalBillBottomBuilder class](#11-the-medicalbillbottombuilder-class)
- [The MedicalBillBackTextBuilder class](#12-the-medicalbillbacktextbuilder-class)
- [The MedicalBillBackClientInfoBuilder class](#13-the-medicalbillbackclientinfobuilder-class)
- [Summary](#summary)

# Prerequisites
1. **Visual Studio 2017** or later is installed.
To install a community version of Visual Studio, use the following link: https://visualstudio.microsoft.com/vs/community/.
Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2. **.NET Core Framework SDK 2.1** or later is installed.
To install the framework, use the following link: https://dotnet.microsoft.com/download.

# Purpose
The example shows how to create a “Medical Bill” that is a complex two-page document. See Fig. 1.

The images below show the main blocks of the document in blue rectangles.

The first, or the front, page is the bill itself that consists of:
* A header
* A body (bill data table)
* A bottom part

The second, or the back, page is a form for the clients to fill in the information about themselves and their insurance agent. The page consists of: 

* A text
* A form

![Fig. 1](../Articles%20Images/Medical_Bill_Ill.png "The  pages")

Fig. 1

# Description

#### Output file
The example creates the **Medical.pdf** file in the output **bin/(debug|release)/netcoreapp2.1** folder, unless specified otherwise in the command line.

#### 1. Open the project in Visual Studio
There are two ways to open the project:

* After installing Visual Studio, double-click the project file **MedicalBill.csproj**.

* In Visual Studio, select **File** > **Open** > **Project/Solution** > **MedicalBill.csproj**. 

#### 2. Run the sample application
There are two ways to run the sample application:
* From Visual Studio by clicking **F5**.

* From a command line: in the directory with **MedicalBill.csproj**, run the command: 

```
dotnet run
```

You can get optional parameters of the command line using the command:

```
dotnet run -help
```

that shows the specification of the command line options:

```
Usage: dotnet run [fullPathToOutFile] [appToView]
Where: fullPathToOutFile - a path to the result file, 'MedicalBill.pdf' by default
appToView - the name of an application to view the file immediately after preparing, by default none app starts
```

## 3. Source code structure

The source code consists of several files (classes).

The reasons to write several classes are the following (in descending order of priority):

* Separation of responsibility. The class `Program` processes the command line options, any possible errors during the document creation, and displays the error report. The other classes do not process the command line options and errors. They are used to build the document itself.
* Reduction of responsibility of each class. This helps to limit the size of code for each class, which improves understanding of the code example.
* Independent blocks of content. The document consists of several content blocks which are independent, so for each class you can select its own block.

## 4. The Program class
Responsibility:
* Parse command line options.
You can process the command line options using the `PrepareParameters` that prepares the `Parameters` structure.
* Display prompts to the user when one of the following options is present in the command line: `“?”`, `“-h”`, `“-help”`, `“--h”`, `“--help”`. It is processed by `Usage`.
* Build the document. It delegates the document building to the `MedicalBillRunner` class with the name of the resulting document file.
* Run the application to demonstrate the resulting document if specified in the command line options.

## 5. The MedicalBillRunner class
Responsibility:
* Create an object of the `MedicalBillBuilder` class.
* Initialize its properties with data of Medical Bill. The properties of the `MedicalBillBuilder` class have the default values, which are as specified in the following lines:

```csharp
                GuarantorNumber = "2nnnnn",
                StatementDate = "07/10/2020",
                CenterName = "Sample Medical Center",
                CenterAddress = "123 Main Street\nAnywhere, NY 12345 - 6789",
                CenterPhone = "123 - 456 - 7890",
                CenterPatent = "Sample Patent",
                FinServAddress = "Main Street 123",
                FinServUrl = "http://www.ourwebsite.com/PatientFinancialServices.aspx"
```

These lines are not required to run the sample application. 
But these lines and subsequent lines must contain real information about the medical entity and data of account calculations in a real application.

* Build the document. It delegates the document building to the `MedicalBillBuilder` class using the `CreateDocumentBuilder` method.

## 6. The MedicalBillBuilder class
Responsibility:
* Save the properties with data of the medical entity in the `Params` structure. The classes for creating the document blocks will use this structure. 
* Save the calculation data in the `RowData` structure. The class for creating the calculation block will use this structure.

The method `CreateDocumentBuilder` has the following responsibilities:

* Create an object of the `Gehtsoft.PDFFlow.Builder.DocumentBuilder` class.
* Create an object of the `MedicalBillFrontBuilder` class and build the front page using the `Build` method of the created object.
* Create an object of the `MedicalBillBackBuilder` class and build the back page using the `Build` method of the created object
* Return the created object of the `Gehtsoft.PDFFlow.Builder.DocumentBuilder` class.

## 7. The MedicalBillFrontBuilder class
Responsibility:
* Create a page and set its parameters:

```csharp
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins)            
```
* Create the footer: 

```csharp
                .AddFooterToOddPage(12, BuildFooter);
```
* Create the first (front) page of the document.
This is done by the `Build` method. It creates objects one by one and runs the `Build` methods for the following classes:


* `MedicalBillHeadBuilder`
* `MedicalBillBodyBuilder`
* `MedicalBillBottomBuilder`


The footer is created by the `BuildFooter` method. The link to the method is passed to the `Gehtsoft.PDFFlow.Builder.SectionBuilder.AddFooterToOddPage` method. 

```csharp
        private void BuildFooter(RepeatingAreaBuilder footerBuilder)
        {
            var paragraphBuilder = footerBuilder.AddParagraph();
            paragraphBuilder
                .SetAlignment(HorizontalAlignment.Right)
                .SetFont(FNT7)
                .AddTextToParagraph("1nnnnn-XX-XXX-00");
        }
```

## 8.The MedicalBillBackBuilder class

Responsibility:
* Create the page and set its parameters:

```csharp
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder.SetOrientation(Orientation).SetMargins(Margins);
```
* Create the second (back) page of the document.
This is done by the `Build` method. It creates objects one by one and runs the `Build` methods for the following classes:


* `MedicalBillBackTextBuilder`
* `MedicalBillBackClientInfoBuilder`

## 9. The **MedicalBillHeadBuilder** class

Responsibility: 
* Create the header of the page. See Fig. 2.

![Fig. 2](../Articles%20Images/MedicalBillFrontHead.png "The front side header")

Fig. 2

This is done by the `BuildHead` method.

The header consists of three logos of the medical center in the top row and four blocks of text information in the bottom row. 
In this case, two right blocks of text information are located under one picture of the top row. 
So, this part of the document is implemented as a table consisting of two rows and four columns, where the last cell of the first row spans two columns.

Though the PDFFlow library supports adding headers using the methods `Gehtsoft.PDFFlow.Builder.Gehtsoft.PDFFlow.SectionBuilder.AddHeader ... `, it does not give any advantage in this particular case. So, here we add the table directly to the section:

```csharp
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetContentRowStyleFont(FNT9)
                .SetBorder(Stroke.None)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 33)
                .AddColumnPercentToTable("", 33)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercent("", 17);
```
Add cells with images to the first row of the table that is created as follows: 
```csharp
            var rowBuilder = tableBuilder.AddRow();
```
To make the cell span two columns, use the `TableCellBuilder.SetColSpan` method:
```csharp
            cellBuilder = rowBuilder.AddCell();
            cellBuilder.SetColSpan(2).SetPadding(4, 0, 0, 0);
            paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .AddInlineImage(Path.Combine(ps.ImageDir, "Healthcare_2x.png"))
                .SetAlignment(HorizontalAlignment.Right);
```
Add the cells with texts to the second row of the table:
```csharp
            rowBuilder = tableBuilder.AddRow();
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .AddParagraphToCell(ps.CenterName + "\n" + 
                    ps.CenterAddress);
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .AddParagraphToCell("To Contact Us Call:  " + 
                    ps.CenterPhone + 
                    "\n\nPhone representatives are available:\n8am to 8pm Monday - Thursday\nand 8am to 4:30pm Friday");
etc.
```
## 10. The MedicalBillBodyBuilder class

Responsibility:
* Create a table of the Medical Bill calculation block. See Fig. 3

![Fig. 3](../Articles%20Images/MedicalBillFrontBody.png "The front side of the table of calculation")

Fig. 3

This is done by the `BuildBody` method.

We implement this block as a table with five columns:

```csharp
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercentToTable("", 47)
                .AddColumnPercentToTable("", 12)
                .AddColumnPercentToTable("", 12)
                .AddColumnPercent("", 12);
```

Set a blue background for the first row: 

```csharp
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .SetBackColor(BLUE_COLOR)
```
This row contains the titles, for example:
```csharp
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .AddParagraphToCell("Date of Service");
etc.
```
We configure the rows with calculation data in the `foreach` loop:
```csharp
            foreach (RowData rowData in rowsData)
            {
                FontBuilder curFont = (--i) == 0 ? FNT10B : FNT10;
                rowBuilder = tableBuilder.AddRow();
                rowBuilder
                    .SetFont(curFont)
                    .SetBorder(BorderBuilder.New()
                        .SetRightWidth(0)
                        .SetTopWidth(0)
                        .SetLeftWidth(0)
                        .SetBottomWidth(0.5f)
                        .SetBottomStroke(Stroke.Solid)
                    );
                cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .SetPadding(4)
                    .AddParagraphToCell(rowData.Date);
etc.
```
## 11. The MedicalBillBottomBuilder class

Responsibility:
* Create the bottom part of Medical Bill. See Fig. 4.

![Fig. 4](../Articles%20Images/MedicalBillFrontBottom.png "The front side of the table of calculation")

Fig. 4

This is done by the `BuildBottom` method.

The bottom part of Medical Bill consists of the following blocks:
* Block before the **Cut** line
* **Cut** line
* Block after the **Cut**

These blocks are created in the following methods respectively:
* `BuildBeforeCut`
* `BuildCut`
* `BuildAfterCut`



###### The BuildBeforeCut method

There are two columns before the **Cut** line in the document. So we implement the block before the **Cut** line as a one-row table with two columns of the same width. 

* The left column with the text "*MESSAGES: ...*"
* The right column with the table "*Current Balance ...*"
```csharp
        private void BuildBeforeCut(SectionBuilder sectionBuilder)
        {
            var outerTableBuilder = sectionBuilder.AddTable();
            outerTableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 50)
                .AddColumnPercent("", 50);
            var rowBuilder = outerTableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetFont(FNT9).SetPadding(0, 8, 0, 0)
                .AddParagraphToCell("MESSAGES:\nWe have filed the medical claims with your insurance.They have\nindicated the balance is your responsibility. To pay your DIN online,\nplease visit www.ourwebsite.com.\n\nIf you have questions regarding your bill, or for payment arrangements, please call 123 - 456 - 78 or send an email inquiry to aboutmybill@ourwebsite.com");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder.AddTable(FillBottomBalanceTable);
        }
```
The `FillBottomBalanceTable` method fills the  "*Current Balance ...*" table. The link to the method is passed to the `Gehtsoft.PDFFlow.Builder.TableCellBuilder.AddTable` method.
The `FillBottomBalanceTable` method creates a table with two columns. Only the first row of the table is divided into two cells, the cells of the second and the third rows span two columns. This is done using the `Gehtsoft.PDFFlow.Builder.TableCellBuilder.SetColSpan (2)` method.

```csharp
        private void FillBottomBalanceTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0, 0, 0, 0)
                .AddColumnPercentToTable("", 74)
                .AddColumnPercent("", 26);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .SetBackColor(MedicalBillBuilder.BLUE_COLOR)
                .SetFont(FNT9B_W)
                .AddParagraphToCell("Current Balance");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                    .SetPadding(4)
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetBackColor(MedicalBillBuilder.BLUE_COLOR)
                    .SetFont(FNT9B_W)
                    .AddParagraphToCell(ps.Balance);
            rowBuilder = tableBuilder.AddRow();
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetColSpan(2)
                .SetFont(FNT9B_B)
                .AddParagraphToCell("This is your first notice for the visit above, which includes a list of itemized services rendered.");
            rowBuilder = tableBuilder.AddRow();
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetColSpan(2)
                .SetFont(FNT9)
                .AddParagraphToCell("We offer a Financial Aid program for qualified applicants. For more information, please call 123-456-7890 or visit our website at www.ourwebsite.com for more information.");
        }
    }
```
###### The BuildCut method

The **Cut** line is a horizontal dashed line with the text above it. 
```csharp
        private void BuildCut(SectionBuilder sectionBuilder)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetAlignment(HorizontalAlignment.Center)
                .SetFont(FNT9B).SetMarginTop(20)
                .AddTextToParagraph("Please retain statement for your records");
            sectionBuilder.AddLine(PageWidth, 0.5f, Stroke.Dashed);
        }
```

###### The BuildAfterCut method

The block after the **Cut** line is divided into two parts vertically.

* The information about two payment methods for the Medical Bill is located at the top.
* The information on how to send the Medical Bill document by mail is located at the bottom. 

Accordingly, these parts are created by the `BuildPayment` and `BuildPostNet` methods. 


###### The BuildPayment method

The method creates a block with information about two payment methods for the Medical Bill:

* Payment by check. Information about the payment must be located on the left.
* Payment by credit card. Information about the payment is a form for the credit card data located on the right. This form is expected to be filled manually by the patient on the printed document.

For this, we create a table with one row and two columns:
```csharp
        private void BuildPayment(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 50)
                .AddColumnPercent("", 50);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .AddCellToRow(BuildCheck)
                .AddCellToRow(BuilCard);
        }
```

The `BuildCheck` and `BuildCard` methods create information about the two payment methods.

###### The BuildCheck method

The method creates information about payment by check in the document.
To get a line before the phrase "MAKE CHECKS PAYABLE TO", we create the table with a visible bottom border in the first row.
The method creates the table and delegates its filling to the `FillCheckTable` method.


###### The FillCheckTable method

The method fills the table with information for payment by check and sets the visible bottom border in the first row:

```csharp
            tableBuilder.SetWidth(PageWidth / 2 - 8).SetBorder(Stroke.None)
                .AddColumnPercent("", 100);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.SetBorder(BorderBuilder.New()
                .SetRightWidth(0)
                .SetTopWidth(0)
                .SetLeftWidth(0)
                .SetBottomWidth(0.5f)
                .SetBottomStroke(Stroke.Solid)
            );
```
###### The BuildCard method

The method creates information about payment by credit card.
The information consists of: 

* A form for choosing card types.
* A form with four rows for filling credit card details.

```csharp
        private void BuilCard(TableCellBuilder cellBuilder)
        {
            if (ps.CardNames.Count > 0)
            {
                AddCardHead(cellBuilder);
                AddForm(cellBuilder, MedicalBillBuilder.CARD_TEXTS1, 
                    new int[] { 50, 25, 25 });
                AddForm(cellBuilder, MedicalBillBuilder.CARD_TEXTS2, new int[] { 50, 50 });
                AddForm(cellBuilder, MedicalBillBuilder.CARD_TEXTS3, 
                    new int[] { 33, 34, 33 }, false, 
                    new string[] { ps.StatementDate, ps.GuarantorNumber, ps.Balance });
                AddForm(cellBuilder, MedicalBillBuilder.CARD_TEXTS4, 
                    new int[] { 50, 50 }, true);
            }
        }
```

###### The AddCardHead method

The method creates the form for choosing card types.
It creates a paragraph with the phrase "* IF PAYING BY VISA, MASTERCARD, DISCOVER OR AMEX, FILL OUT BELOW *",
creates a table for checkboxes of each card type, and delegates filling of the table to the `FillCardNames` method.


###### The FillCardNames method

The method creates columns for `CardNames` in the `foreach` loop for each card type and then fills the cells with a checkbox and the name of the card type in another `foreach` cycle.
```csharp
        private void FillCardNames(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0, 0, 0, 0);
            foreach (string cardName in ps.CardNames)
            {
                tableBuilder
                    .AddColumnPercent("", 100 / ps.CardNames.Count);
            };
            var rowBuilder = tableBuilder.AddRow();
            foreach (string cardName in ps.CardNames)
            {
                var cellBuilder = rowBuilder.AddCell().SetPadding(0, 0, 0, 8);
                cellBuilder.AddParagraph("o").SetFont(FNTZ16);
                cellBuilder.AddParagraph(cardName).SetFont(FNT10);
            }
        }
```

###### The FormBuilder.AddForm method

This method is defined in the `FormBuilder` class and is used to create forms on both front and back pages.

The method accepts the following parameters: 
* `cellBuilder` — an object of the `Gehtsoft.PDFFlow.Builder.TableCellBuilder` class of the table cell where you want to create the form. 
* `labels` — a `string` array that must be printed as label of the field values.
* `widths` — a `int` array. Each element of the array determines the width of the column in % of the width of the nested table to display a label and a blank space for the field value. 
* `bottomBorder` — an optional `bool` flag value. It determines whether to print the bottom border of the row or not. The default value is `false`.
* `values` — an optional `string` array of field values. If it is specified and not null, values are printed instead of the blank space. It is used to display the string "* Statement Date ...*"
* `topBorderDepth` — an optional width, `float`, of the top border. The default value is `0.5`.
* `bottomBorderDepth` — an optional width, `float`, of the bottom border. The default value is `0.5`.
* `valueFontSize` — an optional font size, `float`, for printing elements of the `values` array. The default value is `12`.

The method creates a nested table with one row of cells. Each cell contains:
* The field label
* A space to be filled by the client


###### The BuildPostNet method

The method creates information about sending Medical Bill by mail.

This information represents two identical blocks at the very bottom of the front page, starting with the strings "* SAMPLE GUARANTOR *" and "*SAMPLE MEDICAL CENTER*". We create it as a table with one row and two columns:

```csharp
        private void BuildPostNet(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 50)
                .AddColumnPercent("", 50);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell().SetFont(FNT12);
            var paragraphBuilder = cellBuilder.AddParagraph((ps.GuarantorName + "\n" +
                    ps.CenterAddress).ToUpper());
            paragraphBuilder.SetMarginTop(10).SetMarginBottom(10);
            cellBuilder.AddImageToCell(Path.Combine(ps.ImageDir,
                    "postnet1_2x.png"), new XSize(124, 11));
            cellBuilder = rowBuilder.AddCell().SetFont(FNT12);
            paragraphBuilder = cellBuilder.AddParagraph((ps.CenterName + "\n" +
                    ps.CenterAddress).ToUpper());
            paragraphBuilder.SetMarginTop(10).SetMarginBottom(10);
            cellBuilder.AddImageToCell(Path.Combine(ps.ImageDir,
                    "postnet2_2x.png"), new XSize(124, 11));
        }

```

## 12. The **MedicalBillBackTextBuilder** class

Responsibility: 
* Create a block of text on the second (back) page of the document.

![Fig. 5](../Articles%20Images/MedicalBillBackText.png "The text of the back page")

Fig. 5. 

This is done by the `AddTexts` method.

###### The AddTexts method

The method creates text information on the second (back) page of the document.

The information consists of several distinct blocks of text:
* The first block with the title which format differs from that of the other blocks "* The Sample Medical Center financial assistance policy plain language summary *".
* The second and fourth blocks with titles.
* The third block that includes a list and links.

Accordingly, the following methods are called to create these blocks:

* `AddFirstBlock` 
* `AddNextBlock`
* `AddListBlock` 
* `AddNextBlock` 

As the third block includes links, when calling the listed methods, we pass the `StringWithUrl` parameter defined in the **MedicalBillBackTextBuilder.cs** file.


###### The StringWithUrl class

Responsibility: 
* Store information about the created text consisting of several elements, regardless of whether the elements are texts or links. 

The class implements this responsibility by using one-dimensional array of objects of the `Text` type defined in the **MedicalBillBackTextBuilder.cs** file.

```csharp
    internal class StringWithUrl
    {
        private Text[] texts;

        public StringWithUrl(string v) : this(new Text[] { new Text(v) }) { }

        public StringWithUrl(Text[] vs)
        {
            this.texts = vs;
        }

        public Text[] Texts
        {
            get { return texts; }
        }
    }

```

###### The Text class

Responsibility: 
* Store information about a text element with indication that this element is not a link.
The feature of the class is a possibility to be extended with overriding the indication that the piece of the text is a link.

Using this indication, the calling code can determine how to work with this element, either as with a plain text or as with a link.

```csharp
    internal class Text
    {
        private string content;
        public String Content
        {
            get { return content; }
        }
        public Text(string content)
        {
            this.content = content;
        }
        public virtual bool IsUrl()
        {
            return false;
        }

        public override string ToString()
        {
            return content;
        }
    }

```

###### The Link class

It is an extension of the `Text` class.

Responsibility: 
* Store information about a text element with indication that this element is a link.

```csharp
    internal class Link : Text
    {
        private string href;
        public String Title
        {
            get { return Content; }
        }
        public String Href
        {
            get { return href; }
        }
        public Link(string href) : base(href)
        {
            this.href = href;
        }
        public Link(string title, string href) : base(title)
        {
            this.href = href;
        }
        public override bool IsUrl()
        {
            return true;
        }
    }

```

###### The AddFirstBlock method

The code of the `AddFirstBlock` method is trivial because the data is already prepared in the `AddTexts` method.
What is different in this method:

* A larger title margin than in the other blocks. We set it using the `Gehtsoft.PDFFlow.Builder.ParagraphBuilder.SetMarginBottom` method.
* A larger font size for the title than in the other blocks. We set it using the `Gehtsoft.PDFFlow.Builder.ParagraphBuilder.SetFont` method.

```csharp
        private void AddFirstBlock(SectionBuilder sectionBuilder, 
            string title, StringWithUrl stringWithUrl)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder.SetMarginBottom(17).SetFont(FNT20B).AddText(title);
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder.SetFont(FNT11);
            foreach (Text text in stringWithUrl.Texts)
            {
                paragraphBuilder.AddText(text.Content);
            }
        }

```
###### The AddNextBlock method

The method creates the title of the block. After the margin, it creates the text of the block.
Creation of the title and of the text paragraphs for this block is the same as creation of the title and of the text paragraphs for the block with a list. 

So, the method delegates all work to the following methods:

* `AddBlockTitle` method
* `AddBlockText` method

###### The AddBlockTitle method

The method creates a paragraph for the title, sets its top and bottom margins, sets its font size, and adds the title text to the paragraph. 

```csharp
        private void AddParagraphTitle(SectionBuilder sectionBuilder, string title)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginTop(12).SetMarginBottom(4).SetFont(FNT12B).AddText(title);
        }

```
###### The AddBlockText method
The code of the `AddBlockText` is trivial because the data is already prepared in the `AddTexts` method.
The method creates a paragraph, sets the top margin, and adds the text elements to the paragraph in the `foreach` loop. 

```
        private void AddArticleText(SectionBuilder sectionBuilder, 
            StringWithUrl stringWithUrl, FontBuilder font, float topMargin = 0f)
        {
            var paragraphBuilder =
                sectionBuilder.AddParagraph().SetFont(font).SetMarginTop(topMargin);
            foreach (Text text in stringWithUrl.Texts)
            {
                paragraphBuilder
                    .AddText(text.Content);
            }
        }

```

###### The AddListBlock method

This method creates: 

* The title of the block.
* The first paragraph of the block after the margin.
* The list.
* The last paragraph of the block.

The method reduces all the work to the sequential calling of of the following methods:

* `AddBlockTitle` 
* `AddBlockText` 
* `AddBlockList` 
* `AddBlockText` 

#### The AddBlockList method

During processing of objects of the `StringWithUrl` array in a loop, the method adds the paragraphs to the section. For each paragraph it sets:

* The left margin using the `Gehtsoft.PDFFlow.Builder.ParagraphBuilder.SetMarginLeft` method.
* The font using the `Gehtsoft.PDFFlow.Builder.ParagraphBuilder.SetFont` method.
* The type of the list using the `Gehtsoft.PDFFlow.Builder.ParagraphBuilder.SetListBulleted` method.
* All objects of the `StringWithUrl` type. It delegates determining whether the text is a plain text or a link to the `AddTextOrUrl` method. 

#### The AddTextOrUrl method

The method displays either a plain text or link. It delegates the work to: 

* The `AddLink` method, if the `text` is a link.
* The `AddText` method, if the `text` is not a link.

#### The AddLink method

The method sets the style of the link display for the paragraph and adds the URL and the text of the link to the paragraph. 

```csharp
        private void AddLink(ParagraphBuilder paragraphBuilder, Link link)
        {
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFontColor(Color.Blue)
                        .SetFontName("Helvetica")
                        .SetFontSize(11)
                        .SetFontUnderline(Stroke.Solid, Color.Blue))
                .AddUrlToParagraph(link.Href, link.Title);
        }

```
#### The AddText method

The method adds a text element to the paragraph.
```csharp
        private void AddText(ParagraphBuilder paragraphBuilder, Text text)
        {
            paragraphBuilder.AddTextToParagraph(text.Content);
        }

```

## 13. The **MedicalBillBackClientInfoBuilder** class


Responsibility: 
* Build a block of forms on the second (back) page of the document.
 
![Fig. 6](../Articles%20Images/MedicalBillBackForm.png "The back side of the Form")

Fig. 6. 

This is done by the `Build` method.

The block of forms consists of the following parts:

* The text "*If any of this following has changed since your last statement, please indicate…*"
* A table of forms with client's information

To create these parts, the method calls the following methods:

* `AddInfoTitle`
* `AddClientTable`

#### The AddInfoTitle method

The method creates a paragraph, sets the top margin, sets the font, and adds the text to the paragraph:

```csharp
        private void AddInfoTitle(SectionBuilder sectionBuilder)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginTop(17)
                .SetFont(FNT9)
                .AddTextToParagraph("If any of this following has changed since your last statement, please indicate…");
        }

```

#### The AddClientTable method

The block of forms at the bottom of the Medical Bill back page represents two columns with forms: 
* The left one for client's information to be filled by the client. 
* The right one for information about client's insurance agent to be filled by the client. 


Accordingly, the method creates a table with two columns and one row which contains:
* The left cell with the client's information form in the nested table. The `FillAboutYouTable` method fills the nested table.
* The right cell with the client's insurance agent information form in the nested table. The `FillInsuranceTable` method fills the nested table.

```csharp
        private void AddClientTable(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 50).AddColumnPercent("", 50);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.AddCell().AddTable(FillAboutYouTable);
            rowBuilder.AddCell().AddTable(FillInsuranceTable);
        }

```

#### The FillAboutYouTable method

The method fills the table with client's information form with the following: 
* Seven forms of text data using the `AddForm` method described above.
* One form for choosing one of several options delegating its creation to the `AddRadioButtons` method.
* One line to display the "* Comments *:" label.
* One blank line for client's comment. 

```csharp
        private void FillAboutYouTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(PageWidth / 2 - 8)
                .SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0f, 0f, 0f, 0f)
                .AddColumnPercent("", 100);
            var rowBuilder = tableBuilder.AddRow();
            AddTitle(rowBuilder, "About you:");
            rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            AddForm(cellBuilder, MedicalBillBuilder.CLIENT_NAME_TEXT, new int[] { 100 });
            AddForm(cellBuilder, MedicalBillBuilder.CLIENT_ADDRESS_TEXT, new int[] { 100 });
            AddForm(cellBuilder, MedicalBillBuilder.ADDRESS_FIELDS_TEXT,
                new int[] { 50, 25, 25 });
            AddForm(cellBuilder, MedicalBillBuilder.CLIENT_PHONE_TEXT, new int[] { 100 });
            AddRadioButtons(cellBuilder,
                MedicalBillBuilder.MARITAL_STATUS_TEXT,
                MedicalBillBuilder.MARITAL_STATUS_VALUES_TEXT,
                new int[] { 14, 17, 17, 17, 17, 17 });
            AddForm(cellBuilder, MedicalBillBuilder.EMPL_PHONE_TEXT, new int[] { 50, 50 });
            AddForm(cellBuilder, MedicalBillBuilder.EMPL_ADDRESS_TEXT, new int[] { 100 });
            AddForm(cellBuilder, MedicalBillBuilder.ADDRESS_FIELDS_TEXT, 
                new int[] { 50, 25, 25 }, true);
            rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .SetBorder(borderBuilder =>
                {
                    borderBuilder
                        .SetRightWidth(0).SetTopWidth(0).SetLeftWidth(0).SetBottomWidth(2)
                        .SetBottomStroke(Stroke.Solid);
                });
            cellBuilder = rowBuilder.AddCell().SetPadding(0, 7, 0, 0);
            cellBuilder.AddParagraph().SetFont(FNT11).AddText("Comments:");
            AddRowForClient(tableBuilder);
        }

```

#### The FormBuilder.AddRadioButtons method

The method is defined in the `FormBuilder` class.

The method accepts the following parameters:
* `cellBuilder` – an object of `Gehtsoft.PDFFlow.Builder.TableCellBuilder` class
of the table cell where you want to create the form.
* `label` – a common `string` label for all choices.
* `choices` – a `string` array of values of each choice.
* `widths` – an `int` array. Each element of the array determines the width of the column in % of the width of the nested table to display the common label and a label of each choice. The number of elements in the `widths` array must be one more than the number of elements in the `choices` array. 
* `bottomBorder` – an optional `bool` flag value. It determines whether to print the bottom border of the row or not. The default value is `false`.

The method creates a nested table with one row and several cells. The first cell of the row contains the common explanation of the selection purpose. The other cells contain checkboxes with selection options.

#### The FillInsuranceTable method

The method fills the table with client's insurance agent information form consisting of ten forms of text data using the `AddForm` method described above
```csharp
        private void FillInsuranceTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0, 0, 0, 0)
                .AddColumnPercent("", 100);
            var rowBuilder = tableBuilder.AddRow();
            AddTitle(rowBuilder, "About your insurance:");
            rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            AddForm(cellBuilder, MedicalBillBuilder.ENS_PR_NAME_TEXT, new int[] { 60, 40 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_PR_ADDRESS_TEXT,
                new int[] { 60, 40 });
            AddForm(cellBuilder, MedicalBillBuilder.ADDRESS_FIELDS_TEXT,
                new int[] { 50, 25, 25 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_POLICY_TEXT, new int[] { 50, 50 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_RELATION_TEXT,
                new int[] { 100 }, true,
                new string[] { " " }, 0.5f, 2.0f, 13f);
            AddForm(cellBuilder, MedicalBillBuilder.ENS_SC_NAME_TEXT, new int[] { 60, 40 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_SC_ADDRESS_TEXT,
                new int[] { 60, 40 });
            AddForm(cellBuilder, MedicalBillBuilder.ADDRESS_FIELDS_TEXT,
                new int[] { 50, 25, 25 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_POLICY_TEXT, new int[] { 50, 50 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_RELATION_TEXT, 
                new int[] { 100 }, true);
        }
```

# Summary
The above example showed how to create a complex document that includes tables, nested tables, images, and lists.

The resulting **MedicalBill.pdf** document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/MedicalBill.pdf).

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/MedicalBill).
