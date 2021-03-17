##### Example: AfterschoolAgreement

**AfterschoolAgreement** project is an example showing how to generate an Afterschool Bank Draft Form document using the PDFFlow library. This example demonstrates how to create a single page document that contains multiple tables, paragraphs, and fill-in forms with various styling. The example demonstrates how to add bulleted and numbered lists to documents and use tabulation methods to insert spaces between content.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/AfterschoolAgreement). 

**Table of Content**  

- [Prerequisites](#prerequisites)
- [Purpose](#purpose)
- [Description](#description)
- [Writing the source code](#writing-the-source-code)
  - [Create a blank single page document](#1-create-a-blank-single-page-document)
  - [Build a header](#2-build-a-header)
  - [Build "Step #1" section](#3-build-step-1-section)
  - [Build "Step #2" and "Step #3" sections](#4-build-step-2-and-step-3-section)
  - [Build "Step #4" section](#5-build-step-4-section)
  - [Build Information and Signature section](#6-build-information-and-signature-section)
  - [Build Staple section](#7-build-staple-section)
- [Summary](#summary)

# Prerequisites
1. **Visual Studio 2017** or later is installed.
  To install a community version of Visual Studio, use the following link: 
  https://visualstudio.microsoft.com/vs/community/.
  Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.
2. **.NET Core Framework SDK 2.1** or later is installed.
To install the framework, use the following link: https://dotnet.microsoft.com/download.
3. **PDFFlow Library**.
  The instructions on how to install the library are available here: [PDFFlow documentation](https://demo.gehtsoft.com/pdfflowdoc/web-content.html#InstallingPDFFlowLibrary.html).

# Purpose
The example shows how to create an “Afterschool Bank Draft Form”, which is a complex single page document.

The page consists of the following parts (see figure below):
* A header
* "Step #1" section
* "Step #2" section
* "Step #3" section
* "Step #4" section
* The Information and Signature section
* The Staple section

![Fig. 1](../Articles%20Images/AfterschoolAgreement_1.png)

Step-by-step instructions on how to build each part of the document are provided in this article.



# Description

#### Image of a school logo

The logo image of a sample school is located in the **Content/images/AfterschoolAgreement/SchoolLogo.png** directory.

#### Image of a checkbox

Image of an unsigned checkbox is located in the **Content/images/AfterschoolAgreement/Checkbox.png** directory.

#### Output file
The example creates the **AfterschoolAgreement.pdf** file in the output **bin/(debug|release)/netcoreapp2.1** folder, unless specified otherwise in the command line.

# Writing the source code

## 1. Create a blank single page document.

1.1. Run Visual Studio, select **File** > **Create** > **Console Application (.Net Core)**, specify the project name as **AfterschoolAgreement**.

1.2. Create internal class `Parameters`  inside class `Program` of the new project:

```c#
        internal class Parameters
        {
            public string appToView;
            public string file;
            public Parameters(string appToView, string file)
            {
                this.appToView = appToView;
                this.file = file;
            }
        }
```

1.3. Add a private method `Start`  to the class `Program`. This method opens the output file in the specified application:

```c#
        private static void Start(string file, string appToView)
        {
            ProcessStartInfo psi;
            psi = new ProcessStartInfo("cmd", @"/c start " + appToView + " " + file);
            Process.Start(psi);
        }
```

Remove errors by adding namespace to the top of class `Program`:

```c#
using System.Diagnostics
using System.IO;
```

1.4. Add a private method `PrepareParameters` to class `Program`, this method checks and processes optional parameters that may be passed when the project is executed:

```c#
        private static bool PrepareParameters(Parameters parameters, string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("?")
                    || args[0].Equals("-h")
                    || args[0].Equals("-help")
                    || args[0].Equals("--h")
                    || args[0].Equals("--help")
                    )
                {
                    Usage();
                    return false;
                }
                parameters.file = args[0];
                if (args.Length > 1)
                {
                    parameters.appToView = args[1];
                }
            }
            return true;
        }
```

1.5. Add output file check logic to method `PrepareParameters` :

```c#
            if (File.Exists(parameters.file))
            {
                try
                {
                    File.Delete(parameters.file);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Can't delete file: " + 
                        Path.GetFullPath(parameters.file));
                    Console.Error.WriteLine(e.Message);
                    return false;
                }
            }
```

1.6. Add method `Usage` to class `Program`, that displays information about optional parameters in the console:

```c#
        private static void Usage()
        {
            Console.WriteLine("Usage: dotnet run [fullPathToOutFile] [appToView]");
            Console.WriteLine("fullPathToOutFile - a path to the result file, ");
            Console.Write("'AirplaneTicket.pdf' by default");
            Console.WriteLine("appToView - the name of an application to view the file immediately"); 
            Console.Write(" after preparing by default none app starts");
        }
```

1.7. Modify method `Main` in class `Program`:

```c#
        static int Main(string[] args)
        {
            Parameters parameters = new Parameters(null, "AfterschoolAgreement.pdf");

            if (!PrepareParameters(parameters, args))
            {
                return 1;
            }

            try {
                AfterschoolAgreementRunner.Run().Build(parameters.file);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                return 1;
            }
            Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                + "\" document has been successfully built");
            if (parameters.appToView != null)
            {
                Start(parameters.file, parameters.appToView);
            }
            return 0;
        }
```

1.8. Add new class `AfterschoolAgreementRunner` to the project:

```c#
using Gehtsoft.PDFFlow.Builder;
using System.Collections.Generic;
using System.IO;

namespace AfterschoolAgreement
{
    public static class AfterschoolAgreementRunner
    {
        public static DocumentBuilder Run()
        {
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            documentBuilder.AddSection();

            return documentBuilder;
        }
    }
}
```

1.9. There are two ways to run the project:

* From Visual Studio by clicking **F5**.

* From the command line: in the directory with **AfterschoolAgreement.csproj**, run the command: 

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
Where: fullPathToOutFile - a path to the result file, 'AfterschoolAgreement.pdf' by default
appToView - the name of an application to view the file immediately after preparing, by default none app starts
```

You can set these optional parameters, for example, if you want to place output file in the root directory of the project and view the file in **Microsoft Edge** browser, write:

```
dotnet run ../../../AfterschoolAgreement.pdf msedge
```

After execution of the project, **AfterschoolAgreement.pdf** file is generated. You can open it to see that it contains a blank page.



## 2. Build a header.

2.1. Modify method `Run` in class `AfterschoolAgreement`, set default line spacing for the document:

```c#
        public static DocumentBuilder Run()
        {
            return DocumentBuilder.New()
                .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
                .AddAfterschoolAgreement();
        }
```

2.2. Add images to the project. In Solution Explorer window right-click on the `AfterschoolAgreement` project name and select **Add** > **New Folder**, copy all contents of **Content** folder in the [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/AfterschoolAgreement) and paste to the **New Folder**. 

Rename **New Folder** as **Content**.

2.3. Create a new class `AfterschoolAgreementBuilder` in the project:

```c#
using System.IO;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;

namespace AfterschoolAgreement
{
    public static class AfterschoolAgreementBuilder
    {
        public static DocumentBuilder AddAfterschoolAgreement(this DocumentBuilder builder)
        {
            return builder;
        }
    }
}
```

2.4. Create variables that define image paths inside `AddAfterSchoolAgreement`:

```c#
            var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", "AfterschoolAgreement");
            var checkboxUrl = Path.Combine(imageDir, "Checkbox.png");
            var logoUrl = Path.Combine(imageDir, "SchoolLogo.jpg");
```

All parts of the document will be defined inside the method `AddAfterschoolAgreement`, so all the code snippets presented below should be added to this method before the line `return builder`.

2.5. Set the document's page size and orientation:

```c#
            SectionBuilder section = builder.AddSection()
                .SetSize(PaperSize.Letter)
                .SetOrientation(PageOrientation.Portrait);
```

2.6. Create a new table with no borders, define widths of its columns:

```c#
            section.AddTable()
                .SetContentRowStyleBorder(borderBuilder => borderBuilder.SetStroke(Stroke.None))
                .AddColumnToTable("", 52)
                .AddColumnToTable("", XUnit.FromPercent(40))
                .AddColumnToTable("", XUnit.FromPercent(60))
```

Borderless tables are commonly used when building documents with the PDFFlow library.  They allow to lay out different elements of pages properly, each element can then be placed into a separate cell and formatted individually.

2.7. Add a row to the table, the first cell in the row contains the logo of the school:

```c#
                    .AddRow()
                        .AddCell()
                        .SetHorizontalAlignment(HorizontalAlignment.Left)
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .AddImageToCell(logoUrl, 65, 45, ScalingMode.UserDefined)
```

2.8. Add a second cell with the document name:

```c#
                    .ToRow()
                        .AddCell()
                        .SetHorizontalAlignment(HorizontalAlignment.Center)
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .SetFontSize(12)
                            .AddParagraph("AFTERSCHOOL")
                            .SetBold()
                        .ToCell()
                        .AddParagraphToCell("BANK DRAFT FORM")
```

2.9. Add a third cell with the information about the packet. Set the cell's background color to black:

```c#
                    .ToRow()
                        .AddCell()
                        .SetHorizontalAlignment(HorizontalAlignment.Center)
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .AddParagraphToCell("REGISTRATION PACKET  PG. 4 of 4")
                        .SetBackColor(Color.Black)
                        .SetFont(Fonts.Courier(12).SetColor(Color.White).SetBold())
                .ToTable();
```

2.10. Insert the title of the document as a separate paragraph:

```c#
            section.AddParagraph("BEST SCHOOL of Twin Peaks ")
                .SetMarginTop(5)
                .SetBold()
                .SetFontColor(Color.Gray)
                .SetFontSize(12)
                    .AddText("Afterschool ")
                    .SetFontColor(Color.Black)
                .ToParagraph()
                .AddTextToParagraph("Agreement ACH/CC Automatic Payment Option");
```

The PDFFlow library allows to format parts of the text differently, even if they are on the same line. When you run the project you should see this at the top of the page:

![Fig. 2](../Articles%20Images/AfterschoolAgreement_2.png)



## 3. Build the "Step #1" section.

3.1. Add a new paragraph, specify its font size and color: 

```c#
            section.AddParagraph("STEP #1")
                .SetBackColor(Color.FromRgba(0.3f, 0.3f, 0.3f))
                .SetFont(Fonts.Courier(16).SetColor(Color.White).SetBold());       
```

3.2. Add a horizontal line using tabulation methods functionality, and insert the child name form:

```c#
            section.AddParagraph()
                .SetMarginLeft(0)
                .SetBorderWidth(0)
                .SetFontSize(4)
                .AddTabSymbol()
                .AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
            section.AddParagraph()
                .SetFontSize(6)
                .AddTextToParagraph("CHILD'S FIRST NAME")
                .AddTabSymbol()
                .AddTextToParagraph("MIDDLE INITIAL")
                .AddTabSymbol()
                .AddTextToParagraph("LAST NAME")
                .AddTabulationInPercent(35, TabulationType.Left)
                .AddTabulationInPercent(65, TabulationType.Left);
```

3.3. Add the phone number form:

```c#
            section.AddParagraph()
                .SetMarginLeft(0)
                .SetMarginTop(15)
                .SetBorderWidth(0)
                .SetFontSize(6)
                .AddTabSymbol()
                .AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
            section.AddParagraph()
                .SetFontSize(6)
                .AddTextToParagraph("PHONE NUMBER (DAY)")
                .AddTabSymbol()
                .AddTextToParagraph("PHONE NUMBER (EVENING)")
                .AddTabulationInPercent(65, TabulationType.Left);
```

3.4. Similarly, add the address form by using tabulation to add spaces between the text properly:

```c#
            section.AddParagraph()
                .SetMarginLeft(0)
                .SetMarginTop(15)
                .SetBorderWidth(0)
                .SetFontSize(6)
                .AddTabSymbol()
                .AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
            section.AddParagraph()
                .SetFontSize(6)
                .AddTextToParagraph("CHILD'S SCHOOL")
                .AddTabSymbol()
                .AddTextToParagraph("CHILD'S ADDRESS")
                .AddTabSymbol()
                .AddTextToParagraph("CITY")
                .AddTabSymbol()
                .AddTextToParagraph("STATE")
                .AddTabSymbol()
                .AddTextToParagraph("ZIP")
                .AddTabulationInPercent(35, TabulationType.Left)
                .AddTabulationInPercent(65, TabulationType.Left)
                .AddTabulationInPercent(77, TabulationType.Left)
                .AddTabulationInPercent(90, TabulationType.Left);
            section.AddParagraph()
                .SetFontSize(6)
                .SetMarginLeft(0)
                .SetMarginTop(15)
                .SetMarginBottom(3)
                .SetBorderWidth(0)
                .AddTabSymbol()
                .AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
```


When you run the program, the result should be as below:

![Fig. 3](../Articles%20Images/AfterschoolAgreement_3.png)



## 4. Build the "Step #2" and "Step #3" sections.

4.1. Add a table with three columns, the second column should be empty and it serves as a separator between the two other columns:

```c#
            TableBuilder step2And3Table = section.AddTable()
                .SetContentRowStyleBorder(borderBuilder => borderBuilder.SetStroke(Stroke.None))
                .AddColumnToTable("", XUnit.FromPercent(49))
                .AddColumnToTable("", XUnit.FromPercent(2))
                .AddColumnToTable("", XUnit.FromPercent(49));
```

4.2. Add a header row to the table, which contains the name of the form sections:

```c#
            step2And3Table
                .AddRow()
                    .AddCell()
                    .SetHorizontalAlignment(HorizontalAlignment.Left)
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddParagraphToCell("STEP #2")
                    .SetPadding(2, 0, 0, 0)
                    .SetBackColor(Color.FromRgba(0.3f, 0.3f, 0.3f))
                    .SetFont(Fonts.Courier(16).SetColor(Color.White).SetBold())
                .ToRow()
                .AddCellToRow()
                    .AddCell()
                    .SetHorizontalAlignment(HorizontalAlignment.Left)
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddParagraphToCell("STEP #3")
                    .SetPadding(2, 0, 0, 0)
                    .SetBackColor(Color.FromRgba(0.3f, 0.3f, 0.3f))
                    .SetFont(Fonts.Courier(16).SetColor(Color.White).SetBold());
```

4.3. Add a new row to the table, the first cell of this row has a form for the date:

```c#
            step2And3Table
                .AddRow()
                    .AddCell()
                    .SetBorder(Stroke.Solid, Color.Black, StyleSheet.DefaultBorderWidth)
                    .SetHorizontalAlignment(HorizontalAlignment.Center)
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddParagraphToCell("Begin Draft Date:")
                    .AddParagraphToCell("_______ / _______ / _______")
                .ToRow()
                .AddCellToRow()
```

4.4. Insert another table inside the third cell of the row:

```c#
                    .AddCell()
                    .SetHorizontalAlignment(HorizontalAlignment.Center)
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddTable()
                    .SetHeaderRowStyleBackColor(Color.Gray)
                    .AddColumnToTable("", XUnit.FromPercent(70))
                    .AddColumnToTable("", XUnit.FromPercent(30))
```

4.5. This nested table has three rows, first one contains headers. Insert it as below:

```c#
                        .AddRow()
                            .AddCell("DRAFT DATES")
                            .SetBackColor(Color.Gray)
                            .SetFontColor(Color.White)
                        .ToRow()
                            .AddCell("AMOUNT")
                            .SetBackColor(Color.Gray)
                            .SetFontColor(Color.White)
                    .ToTable()
```

4.6. Add other rows to the nested table:

```c#
                        .AddRow()
                        .SetVerticalAlignment(VerticalAlignment.Bottom)
                            .AddCell("Monthly on the 1st")
                            .SetFontSize(10)
                        .ToRow()
                            .AddCell("$")
                            .SetBold()
                    .ToTable()
                        .AddRow()
                        .SetVerticalAlignment(VerticalAlignment.Bottom)
                            .AddCell("Semi-Monthly on the 1st & 15th")
                            .SetFontSize(10)
                        .ToRow()
                            .AddCell("$")
                            .SetBold()
                    .ToTable()
                    .SetMarginBottom(5);
```

Now, run the program one more time to view the result:

![Fig. 4](../Articles%20Images/AfterschoolAgreement_4.png)



## 5. Build the "Step #4" section.

5.1. Insert a new paragraph with the name of the section:

```c#
            section.AddParagraph("STEP #4")
                .SetBackColor(Color.FromRgba(0.3f, 0.3f, 0.3f))
                .SetFont(Fonts.Courier(16).SetColor(Color.White).SetBold());
```

5.2. Add a table, separate it from the element above by setting a top margin:

```c#
            section.AddTable()
                .SetMarginTop(2)
                .SetContentRowStyleBorder(borderBuilder => borderBuilder.SetStroke(Stroke.None))
                .AddColumnToTable("", XUnit.FromPercent(49))
                .AddColumnToTable("", XUnit.FromPercent(2))
                .AddColumnToTable("", XUnit.FromPercent(49))
```

5.3. Add the first row to the table, insert the unchecked checkbox image in the first and third cells of this row, and adjust the text position inside the cells:

```c#
                    .AddRow()
                        .AddCell()
                        .SetHorizontalAlignment(HorizontalAlignment.Left)
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .SetPadding(2, 0, 0, 0)
                            .AddParagraph("")
                            .AddInlineImageToParagraph(checkboxUrl, 11, 11, ScalingMode.UserDefined)
                            .AddTextToParagraph(" OPTION 1: CREDIT/DEBIT CARD")
                        .ToCell()
                        .SetBackColor(Color.Gray)
                        .SetFont(Fonts.Courier(14).SetColor(Color.White).SetBold())
                    .ToRow()
                    .AddCellToRow()
                        .AddCell()
                        .SetHorizontalAlignment(HorizontalAlignment.Left)
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .SetPadding(2, 0, 0, 0)
                            .AddParagraph("")
                            .AddInlineImageToParagraph(checkboxUrl, 11, 11, ScalingMode.UserDefined)
                            .AddTextToParagraph(" OPTION 2: BANK DRAFT")
                        .ToCell()
                        .SetBackColor(Color.Gray)
                        .SetFont(Fonts.Courier(14).SetColor(Color.White).SetBold())
```

5.4. Add another row to the table, and specify a font for its first cell content:

```c#
                .ToTable()
                    .AddRow()
                        .AddCell()
                        .SetHorizontalAlignment(HorizontalAlignment.Center)
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .SetFont(Fonts.Courier(6))
```

5.5. Add a new table to this cell, the first row of this table should contain the credit card options and span two columns:

```c#
                            .AddTable()
                            .SetContentRowStyleFont(Fonts.Courier(6))
                            .SetDefaultAltRowStyle()
                            .AddColumnToTable("", XUnit.FromPercent(65))
                            .AddColumnToTable("", XUnit.FromPercent(35))
                                .AddRow()
                                    .AddCell()
                                    .SetColSpan(2)
                                    .SetFontSize(8)
                                    .SetVerticalAlignment(VerticalAlignment.Center)
                                    .AddParagraph("Check one:  ")
                                        .AddInlineImageToParagraph(checkboxUrl, 8, 8, ScalingMode.UserDefined)
                                        .AddTextToParagraph(" Visa  ")
                                        .AddInlineImageToParagraph(checkboxUrl, 8, 8, ScalingMode.UserDefined)
                                        .AddTextToParagraph(" Mastercard  ")
                                        .AddInlineImageToParagraph(checkboxUrl, 8, 8, ScalingMode.UserDefined)
                                        .AddTextToParagraph(" Discover  ")
                                        .AddInlineImageToParagraph(checkboxUrl, 8, 8, ScalingMode.UserDefined)
                                        .AddTextToParagraph(" AmEx")
```

5.6. Fill the other rows with the forms for the credit card information. Pay attention to the fact that a single cell may contain multiple paragraphs with different styling:

```c#
                            .ToTable()
                                .AddRow()
                                    .AddCell("CREDIT/DEBIT CARD #")
                                        .AddParagraph("FILL HERE")
                                        .SetFontColor(Color.White)
                                        .SetFontSize(16)
                                .ToRow()
                                    .AddCell("EXP. DATE")
                                        .AddParagraph("FILL HERE")
                                        .SetFontColor(Color.White)
                                        .SetFontSize(16)
                            .ToTable()
                                .AddRow()
                                    .AddCell("CARDHOLDER NAME")
                                        .AddParagraph("FILL HERE")
                                        .SetFontColor(Color.White)
                                        .SetFontSize(16)
                                .ToRow()
                                    .AddCell("CVV")
                                        .AddParagraph("FILL HERE")
                                        .SetFontColor(Color.White)
                                        .SetFontSize(16)
                            .ToTable()
                        .ToCell()
                    .ToRow()
```

5.7. Similarly, add a second form for the payment option to the initial table:

```c#
                    .AddCellToRow()
                        .AddCell()
                        .SetHorizontalAlignment(HorizontalAlignment.Center)
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .SetFontSize(6)
                            .AddTable()
                            .SetContentRowStyleFont(Fonts.Courier(6))
                            .SetDefaultAltRowStyle()
                            .AddColumnToTable("", XUnit.FromPercent(60))
                            .AddColumnToTable("", XUnit.FromPercent(40))
                                .AddRow()
                                    .AddCell("ACCOUNT HOLDER NAME")
                                        .AddParagraph("FILL HERE")
                                        .SetFontColor(Color.White)
                                        .SetFontSize(16)
                                .ToRow()
                                    .AddCell("BANK NAME")
                                        .AddParagraph("FILL HERE")
                                        .SetFontColor(Color.White)
                                        .SetFontSize(16)
                            .ToTable()
                                .AddRow()
                                    .AddCell("ROUTING/TRANSIT #")
                                        .AddParagraph("FILL HERE")
                                        .SetFontColor(Color.White)
                                        .SetFontSize(16)
                                .ToRow()
                                    .AddCell("BANK ACCOUNT #")
                                        .AddParagraph("FILL HERE")
                                        .SetFontColor(Color.White)
                                        .SetFontSize(16)
                            .ToTable();
```

When you run the project, you should see the completed Step #4 section as below:

![Fig. 5](../Articles%20Images/AfterschoolAgreement_5.png)



## 6. Build the Information and Signature section.

6.1. Add a new paragraph with text highlighted in bold:

```c#
            section.AddParagraph("AUTOMATED CLEARINGHOUSE(ACH) DRAFTS ARE REQUIRED TO HAVE " +
                                 "A VOIDED CHECK. DEBIT CARDS ARE NOT ACCEPTED. MUST BE ACH" +
                                 "OR CREDIT CARDS ONLY.")
                .SetMarginTop(10)
                .SetBold()
                .SetFontSize(8);
```

6.2. Insert a bulleted list. It is done the same way as when adding paragraphs, so you just have to call the method `SetListBulleted` to the definition of paragraphs: 

```c#
            section.AddParagraph("Only 1 Form of Draft Payment can be entered per person.")
                .SetListBulleted()
                .SetFontSize(8);
            section.AddParagraph("Children enrolled in Summer Camp may have a larger draft amount on May 15 & Aug 1.")
                .SetListBulleted()
                .SetFontSize(8);
```

6.3. Add a numbered list, the method `SetListNumbered` automatically handles numeration of the items:

```c#
            section.AddParagraphToSection()
                .AddParagraph( "I understand that this transfer will occur monthly on the 1st. First draft begins Aug. 1.")
                .SetListNumbered(NumerationStyle.Arabic, 0, 0)
                .SetFontSize(8);
            section.AddParagraph("I understand that should I choose to change a Bank Account " + 
                                 "I must provide a school with at least a 2 week notice.")
                .SetListNumbered(NumerationStyle.Arabic, 0, 0)
                .SetFontSize(8);
            section.AddParagraph("I understand that the information above will be used to " +
                                 "transfer payment from my account.")
                .SetListNumbered(NumerationStyle.Arabic, 0, 0)
                .SetFontSize(8);
            section.AddParagraph("I understand that if my payment is returned for " + 
                                 "non-sufficient funds I will be charged a $30 fee.")
                .SetListNumbered(NumerationStyle.Arabic, 0, 0)
                .SetFontSize(8);
            section.AddParagraph("BEST SCHOOL only accepts Visa, MasterCard, Discover, and American Express.")
                .SetListNumbered(NumerationStyle.Arabic, 0, 0)
                .SetFontSize(8);
            section.AddParagraph("I understand that after three returned items, " + 
                                 "I will be ineligible to use the automatic payment option.")
                .SetListNumbered(NumerationStyle.Arabic, 0, 0)
                .SetFontSize(8);
```

6.4. Add a line where a signature and date can be handwritten:

```c#
            section.AddParagraph("ACCOUNT HOLDER ACKNOWLEDGMENT")
                .SetMarginTop(5)
                .SetBold();
            section.AddParagraph()
                .SetMarginTop(8)
                .SetMarginLeft(0)
                .SetBorderWidth(0)
                .AddTabSymbol().AddTabulationInPercent(50, TabulationType.Right, TabulationLeading.BottomLine)
                .AddTabSymbol().AddTabSymbol()
                .AddTabulationInPercent(70, TabulationType.Left)
                .AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
            section.AddParagraph()
                .SetMarginTop(0)
                .SetFontSize(10)
                .AddTabSymbol().AddTextToParagraph("Account Holder Signature")
                .AddTabulationInPercent(25, TabulationType.Center)
                .AddTabSymbol()
                .AddTextToParagraph("Date")
                .AddTabulationInPercent(85, TabulationType.Center);
```

 Now, run the project one more time to view the lists:

![Fig. 6](../Articles%20Images/AfterschoolAgreement_6.png)



## 7. Build the Staple section.

7.1. Insert a table with three columns:

```c#
            TableBuilder pleaseStapleHereTable = section.AddTable()
                .SetAlignment(HorizontalAlignment.Center)
                .SetWidth(XUnit.FromPercent(80))
                .SetMarginTop(5)
                .SetBorderStroke(Stroke.Dotted)
                .AddColumnToTable("", XUnit.FromPercent(10))
                .AddColumnToTable("", XUnit.FromPercent(80))
                .AddColumnToTable("", XUnit.FromPercent(10));
```

7.2. Add a row to the table, each cell of this row should have dotted borders:

```c#
            pleaseStapleHereTable.AddRow()
                .SetHorizontalAlignment(HorizontalAlignment.Center)
                .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddCell()
                    .SetBorderStroke(Stroke.Dotted, Stroke.Dotted, Stroke.None, Stroke.Dotted)
                    .SetFontSize(6)
```

7.3. Add the text about stapling a check to the cells:

```c#
                    .AddParagraphToCell("Please").AddParagraphToCell("Staple").AddParagraphToCell("Here")
                .ToRow()
                    .AddCell()
                    .SetBorderStroke(Stroke.None, Stroke.Dotted, Stroke.None, Stroke.Dotted)
                    .SetPadding(0, 68, 0, 64)
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddParagraph("STAPLE VOIDED CHECK HERE").SetBold().SetFontSize(24)
                .ToRow()
                    .AddCell()
                    .SetBorderStroke(Stroke.None, Stroke.Dotted, Stroke.Dotted, Stroke.Dotted)
                    .SetFontSize(6).AddParagraphToCell("Please")
                    .AddParagraphToCell("Staple").AddParagraphToCell("Here")
                .ToSection()
                .SetMargins(20, 20, 20, 0);
```

After executing the project, you should see this at the bottom of the page:

![Fig. 7](../Articles%20Images/AfterschoolAgreement_7.png)

# Summary
The example above showed how to create a complex single-page document that includes tables, nested tables, images, and lists.

The resulting **AfterschoolAgreement.pdf** document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/AfterschoolAgreement.pdf).

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/AfterschoolAgreement).
