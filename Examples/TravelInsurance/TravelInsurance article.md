##### Example: TravelInsurance

# Purpose
The TravelInsurance project is an example of generation of a Travel Insurance Claim Form document. The example demonstrates how to create a two-page document that contains many fill-in forms and images. This example also shows how to divide page into tables, configure their titles and borders, add multiple paragraphs and checkboxes into cells.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/TravelInsurance). The document consists of 

* Header
* General Information Section
* Section A
* Section B
* Section C
* Section D
* Section E
* Section F

Process of building each of these parts is described in this article step-by-step.

**Table of Contents**

- [Prerequisites](#prerequisites)
- [Description](#description)
    + [Company logo](#company-logo)
    + [Checkbox](#checkbox)
    + [Output file](#output-file)
- [Writing the source code](#writing-the-source-code)
    + [1. Create new console application.](#1-create-new-console-application)
    + [2. Modify class Program.](#2-modify-class-program)
    + [3. Create two-page document.](#3-create-two-page-document)
    + [4. Build header of the front page](#4-build-header-of-the-front-page)
    + [5. Build General Information section](#5-build-general-information-section)
    + [6. Build Sections A and B](#6-build-sections-a-and-b)
    + [7. Build Sections C, D, E](#7-build-sections-c,-d,-e)
    + [8. Build Section F and the footer of the back page](#8-build-section-f-and-the-footer-of-the-back-page)
- [Summary](#summary)

# Prerequisites
1) **Visual Studio 2017** or above is installed.
   To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
   To install the framework use the following link: https://dotnet.microsoft.com/download

3) **Adobe Acrobat Reader** and/or **Google Chrome** (or any other Chromium based browser) to view PDF document.

# Description

### Company logo

The sample company's logo is in the `images/TravelInsurance_Logo.png` file.

### Checkbox 

A picture of the unchecked checkbox is in the `images/TravelInsurance_Checkbox.png` file.

### Output file
The example creates the **TravelInsurance.pdf** file in the output **bin/(Debug|Release)/netcoreapp2.1** folder.


# Writing the source code

#### 1. Create a new console application.
1.1.    Run Visual Studio  
1.2.    File -> Create -> Console Application (.Net Core)

#### 2. Modify the class Program.
2.1. Set the path to the output PDF-file in the `Main()` method:

```c#
    string file = "TravelInsurance.pdf";
```
2.2. Call the `Run()` method of a "runner" class for the document generation and the `Build()` method for building a document into an output PDF-file:

```c#
    TravelInsuranceRunner.Run().Build(file);
```
2.3. After the generation is completed, notify the user about the successful generation:

At first add at the top of the file:

```c#
using System.IO;
```
Then write before the end of the method `Main`:
```c#
    Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                        + "\" document has been successfully built");
```
We have an error that  `TravelInsuranceRunner` is not defined. To resolve it, add a new class `TravelInsuranceRunner.cs` and define the `Run()` method in it:

```c#
using Gehtsoft.PDFFlow.Builder;

namespace TravelInsurance
{
    internal class TravelInsuranceRunner
    {
        internal static DocumentBuilder Run()
        {
            return DocumentBuilder.New().AddSectionToDocument();
        }
    }
}
```

2.4 After running the project we get **TravelInsurance.pdf** file in the output **bin/(Debug|Release)/netcoreapp2.1** folder. Open it in **Adobe Acrobat**. It is empty and it's normal, because we haven't added anything to it yet. 

Now, run the project again. If you didn't close **Adobe Acrobat** before running you would get an error: **FILE_OPEN_ERROR**.

This occurs because **Adobe Acrobat** has locked the file.  This is not convenient for development, and it is better to use a viewer that shows a document without locking it.

 Also it isn't too convenient to have the document in in the output **bin/(Debug|Release)/netcoreapp2.1** folder.

Let's make little changes in `Program.cs` to allow us to specify the options of the command line:

* full path to the result file of the document.
* the name of a desired application to view the resulting document.

~~~c#
using System;
using System.Diagnostics;
using System.IO;

namespace TravelInsurance
{
    class Program
    {
        static int Main(string[] args)
        {
            Parameters parameters = new Parameters(null, "TravelInsurance.pdf");

            if (!PrepareParameters(parameters, args))
            {
                return 1;
            }

            try {
                TravelInsuranceRunner.Run().Build(parameters.file);
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

        private static void Start(string file, string appToView)
        {
            ProcessStartInfo psi;
            psi = new ProcessStartInfo("cmd", @"/c start " + appToView + " " + file);
            Process.Start(psi);
        }

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
            return true;
        }
        private static void Usage()
        {
            Console.WriteLine("Usage: dotnet run [fullPathToOutFile] [appToView]");
            Console.WriteLine(
                "fullPathToOutFile - a path to the result file, 'TravelInsurance.pdf' by default");
            Console.WriteLine(
                "appToView - the name of an application to view the file immediately after preparing, by default none app starts");
        }

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
    }
}

~~~



The changes we made:

* Added `internal class Parameters`. We need it to parse the command line options to its properties values.
* Implemented  method `PrepareParameters()` that:
  * Analyzes the command line options.
  * If an option is set for the popular **help** options (?, -h, --h, -help, --help) then the list of the command  line options is shown.
  * Fills `Parameters` properties: `appToView` and `file`
* Added `try-catch` for error processing to make the error message clearer than just **FILE_OPEN_ERROR**
* Added an ability to run a desired PDF viewer, if it is specified in the command line options.

To specify the options in Visual Studio open Menu->Project->TravelInsurance properties->Debug and specify **Application arguments:** ../../../TravelInsurance.pdf chrome.

Now, run the project again. Now the empty document **TravelInsurance.pdf** is shown in the Chrome, assuming you have it installed.

#### 3. Create two-page document.

3.1. Modify the class `TravelInsuranceRunner.cs`:

```c#
using Gehtsoft.PDFFlow.Builder;

namespace TravelInsurance
{
    public static class TravelInsuranceRunner
    {
        public static DocumentBuilder Run()
        {
            var travelInsuranceBuilder = new TravelInsuranceBuilder() { };

            return travelInsuranceBuilder.CreateDocumentBuilder();
        }
    }
}
```

We get an error that `TravelInsuranceBuilder` is not defined. To solve it add a new class `TravelInsuranceBuilder.cs`, and define the `CreateDocumentBuilder` method:

```c#
using Gehtsoft.PDFFlow.Builder;

namespace TravelInsurance
{
    internal class TravelInsuranceBuilder
    {

        internal DocumentBuilder CreateDocumentBuilder()
        {
            return DocumentBuilder.New().AddSectionToDocument();
        }
    }
}
```

After running the project we get an empty document, as expected.

3.2. Modify the file `TravelInsuranceBuilder.cs`. 

Insert at the top of the file:

```c#
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Utils;
using System.IO;
```

Introduce constants to use them for reducing the size of the code.

```c#
    internal class TravelInsuranceBuilder
    {
        internal const PageOrientation Orientation = PageOrientation.Portrait;
        internal static readonly Box Margins = new Box(29, 21, 29, 0);

        internal static readonly string CheckboxPath = Path.Combine(
            Directory.GetCurrentDirectory(), "images", "TravelInsurance_Checkbox.png");
        internal static readonly string LogoPath = Path.Combine(Directory.GetCurrentDirectory(),
                                        "images", "TravelInsurance_Logo.png");

        internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7f);
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8f);
        internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11f);
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12f);
        internal static readonly FontBuilder FNT20B = Fonts.Helvetica(20f).SetBold();

        internal const string HEADER_BOX_TEXT =
            "Required documents – For all travel claims please submit air tickets and boarding " +
            "pass. For annual plans, please provide a copy of the passport showing duration of " +
            "trip. We reserve the right to request for additional information. To enable us to " +
            "process your claim expeditiously, please return the duly completed Claim Form with" +
            " supporting documents.\nPlease direct the claim form and all correspondence to:\n" +
            "Sample Company Travel Claims Unit \nc/o Sample Company Ltd, No. 5 Streenname #33-" +
            "01, Sample city 12345\n\nThe acceptance of this Form is NOT an admission of " +
            "liability on the part of Sample Company. Any documentary proof or report required " +
            "by the Company shall be furnished at the expense of the Policyholder or Claimant.\n";

        internal const string SECTIONF_LONG_TEXT =
            "I declare that to the best of my knowledge and belief that the above particulars " +
            "are true and accurate. If I made or shall make any false or fraudulent statements," +
            " or withhold material facts whatsoever in respect of this claim, the Policy shall " +
            "be void and I shall forfeit all rights to recover therein.\n\nI authorise any " +
            "hospital doctor, other person who has attended or examined me, to furnish to the " +
            "Company, and/ or its authorised representatives, any and all information relating " +
            "to any illness or injury, medical history, consultation, prescription or treatment," +
            " and copies of all hospital or medical records.  A photocopy of this authorisation " +
            "shall be considered as effective and valid as the original.";
        
```

Rewrite the `CreateDocumentBuilder()` method:

```C#
        internal DocumentBuilder CreateDocumentBuilder()
        {
            var documentBuilder = DocumentBuilder.New();
            new TravelInsuranceFrontBuilder().Build(documentBuilder);
            new TravelInsuranceBackBuilder().Build(documentBuilder);
            return documentBuilder;
        }

```

3.3. After that, create two new classes: `TravelInsuranceFrontBuilder.cs` and `TravelInsuranceBackBuilder.cs`. 
Both files should have at the top:

~~~c#
using Gehtsoft.PDFFlow.Builder;
using static TravelInsurance.TravelInsuranceBuilder;

~~~

Also, add `Build()` method to remove errors, for now its implementation in both classes will look the same:

```c#
        internal void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder.AddSection()
                .SetOrientation(Orientation)
                .SetMargins(Margins)
                .SetStyleFont(FNT7);
        }
                            
```
Try running the program, you should see that a two-page blank document is generated. We have set orientation, margins of the pages and the default font of the text that will be used on them.   

#### 4. Build the header of the front page

4.1. Add this line to the `Build()` method of `TravelInsuranceFrontBuilder.cs` class:

```c#
           new TravelInsuranceHeaderBuilder().Build(sectionBuilder);
```

Create `TravelInsuranceHeaderBuilder.cs` file and paste following code snippet into it:

 ```c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceHeaderBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            sectionBuilder
                .AddTable()
                    .SetContentRowStyleFont(FNT20B)
                    .SetBorder(Stroke.None)
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("", 50)
                    .AddColumnPercentToTable("", 50)
                    .AddRow()
                        .AddCell()
                            .SetPadding(2, 4, 0, 0)
                            .AddParagraph("Travel Insurance")
                            	.SetLineSpacing(0.8f)
                                .AddText("\nClaim Form")
                                    .SetFontColor(Color.Gray)
                    .ToRow()
                        .AddCell()
                            .AddImage(LogoPath)
                            .SetAlignment(HorizontalAlignment.Right)
                            .SetScale(ScalingMode.UserDefined)
                            .SetWidth(250);

        }
    }
}

 ```

What have we done in this method:

* Created a table.
* Set the default font that will be used in it, removed borders of the table.
* Divided table into two columns, each one have width that equals to 50% of the table width.
* Added row to the table, added cell to this row, set padding of the cell.
* Created a paragraph with text, set indentation between the lines of text to be 0.5 px.
* In order to add more text to the same paragraph, but with different formatting used `AddText` method.
* Set the color of the new line of text to be gray.
* Added another cell to the row, put an image inside of it.
* Set image alignment and size.

Notice how easy to chain and follow methods of PDFFlow library using fluent syntax. Now run the project again, you should see following picture at the top of the first page:

![Fig.1](../Articles%20Images/TravelInsurance-1.png)

4.2. Let's add more text to the table

Remove last semicolon in the `Build` method and paste this code snippet on the new line:

```c#
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .SetColSpan(2)
                            .SetPadding(0, 13, 0, 8)
                            .SetFont(FNT7)
                            .AddParagraphToCell("PLEASE COMPLETE ALL SECTIONS TO FACILITATE " +
                                                "THE PROCESSING OF YOUR APPLICATION ")
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .SetColSpan(2)
                            .SetBorder(Stroke.Solid, Color.Black, 0.5f)
                            .SetPadding(8)
                            .SetFont(FNT8)
                            .AddParagraph()
                                .AddTextToParagraph(HEADER_BOX_TEXT);

```

 We added two more rows to the table, each has a cell that spans two columns. These cells have different formatting and contain text of varying size. Check the changes by executing the project, you should get:

  ![Fig.2](../Articles%20Images/TravelInsurance-2.png)

It should be mentioned that although expected design of the documents may not look like table, broad functionality of PDFFlow library allows to achieve required placement of elements on the pages for most documents simply by creating tables and configuring them.    

#### 5. Build General Information Section.

5.1. Add this line to the `Build()` method of `TravelInsuranceBuilder.cs` class:

```c#
           new TravelInsuranceGeneralInfoBuilder().Build(sectionBuilder);
```

Create `TravelInsuranceGeneralInfoBuilder.cs` file and add `Build()` method to it:

```c#
using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceGeneralInfoBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
        }
    }
}
```

Other sections of the document will look similar to this one, so to not repeat ourselves let's create helper class `TravelInsuranceFormBuilder.cs` that will contain most of the necessary formatting for tables and rows. Contents of this class are following:

```c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using System.Collections.Generic;
using System.Linq;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    static class TravelInsuranceFormBuilder
    {
        public static TableBuilder CreateTable(SectionBuilder sectionBuilder, 
            int columnNumber, float topMargin = 13)
        {
            return sectionBuilder
                .AddTable(Enumerable.Repeat(XUnit.FromPercent(100f / columnNumber), 
                columnNumber).ToArray())
                    .SetMarginTop(topMargin)
                    .SetBorder(Stroke.None)
                    .SetContentRowBorderWidth(0, 0, 0, 0);
        }

        public static void AddTitle(TableBuilder tableBuilder, string title, int columnNumber = 3)
        {
            tableBuilder
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 2)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetColSpan(columnNumber)
                        .AddParagraph(title)
                            .SetFont(FNT11)
                            .SetBold()
                            .SetLineSpacing(0.95f);
        }

        public static void CreateRow(TableBuilder tableBuilder, 
            Dictionary<string, int> rowContents, float bottomPadding = 17)
        {
            var rowBuilder = tableBuilder.AddRow()
                .SetBorderWidth(0, 0, 0, 0.5f)
                .SetBorderStroke(Stroke.Solid);

            foreach (KeyValuePair<string, int> cellContent in rowContents)
            {
                rowBuilder
                    .AddCell()
                        .SetColSpan(cellContent.Value)
                        .SetPadding(1, 1, 0, bottomPadding)
                        .AddParagraph(cellContent.Key)
                            .SetLineSpacing(0.95f);
                    
            }
        }
    }
}

```

What does each method do:

* `CreateTable()` method creates custom table with given number of columns and value of top margin.
  * First, it adds table and divides into columns of same width.
  * Secondly, sets top margin for this table, in other words, distance from the element above.
  * After that defines default borders for rows of the table.

* `AddTitle()` method adds header for the given table and sets bold border under it.
  * To achieve that, first new row added to the table with a bottom border of width 2px.
  * Then a cell is added to this row that spans the whole row.
  * Finally a paragraph is added with the title of this table.

* `CreateRow()` method adds to table a custom row with given text
  * The row will have a thin 0.5px solid border at the bottom.
  * Then method sets padding and column span for each cell of the row and fills with text. 

5.2. Insert following line to the top of  `TravelInsuranceGeneralInfoBuilder.cs` file:

```c#
using static TravelInsurance.TravelInsuranceFormBuilder;

```

Now we can use aforementioned methods to fill this section of the front page of our document with content, just copy this to the `Build()` method:  

```c#
            var tableBuilder = CreateTable(sectionBuilder, 3);

            AddTitle(tableBuilder, "General Information");

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(3)
                        .SetPadding(0, 7)
                        .AddParagraphToCell("Documents required\nFor all travel claims please " +
                            "submit air tickets and boarding pass.\nFor annual plans, please " +
                            "provide a copy of the passport showing duration of trip.");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Policyholder", 1 }, 
                    {"Claimant (if it differs from the above)", 1 },
                    {"Insurance Policy No.", 1}
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Address", 3 }
                });
```

Run the program, the result should be as in this figure:

![Fig.3](../Articles%20Images/TravelInsurance-3.png)

5.3. Now let's add some rows with checkboxes, inside the `Build()` method write:

```c#
            tableBuilder
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 17)
                        .AddParagraph("Occupation")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 0)
                        .AddParagraph("Date of Birth")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 0)
                        .AddParagraphToCell("Sex")
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Male", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Female")
                                .SetFont(FNT12);
```

This code snippet 

* creates a new row in the table,
* adds two cells with text,
* adds one more cell with a text and two checkboxes below the text.
  * To put checkboxes in separate line below they are added to new paragraph.
  * To set two checkboxes apart on the same line tabulation is used by calling `addTabulationInPercent()` method.
  * `AddInlineImage()` method is used to add images to the paragraph.

After running the project you will be able to view this new row:

![Fig.4](../Articles%20Images/TravelInsurance-4.png)

5.4. Insert the code to add rest of the rows of the table:

```c#
            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"TelephoneNo.", 1 },
                    {"HP No.", 1 },
                    {"Email Address:", 1}
                });

            tableBuilder
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph("Travel companion(s) is/are insured" +
                                      "\nWith AHA? If yes, please provide details.")
                .ToRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(1, 10, 0, 0)
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Yes", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(25, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" No")
                                .SetFont(FNT12)
            .ToTable()
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 10, 0, 0)
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Yes", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" No")
                                .SetFont(FNT12)
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 17)
                        .AddParagraph("Registration No.")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 0)
                        .AddParagraphToCell("Purpose of Trip")
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Business", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Vacation")
                                .SetFont(FNT12);

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Place where accident, loss or illness occurred", 1 },
                    {"Time", 1 },
                    {"Date", 1}
                });

            tableBuilder
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph("Are there any other Policies of insurance in force " +
                                      "covering\nyou in respect of this event?")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 10, 0, 0)
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Yes", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" No")
                                .SetFont(FNT12)
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph("If yes, please specify:");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Description of the incident, loss or illness", 3 }
                });
```

The idea here is similar to what was discussed above,  `CreateRow()` method was used to add generic rows that contain only text, but when checkboxes needed to be inserted into different places inside the rows, such rows were formatted manually with fluent syntax. 
Now General Information Section is ready:

![Fig.5](../Articles%20Images/TravelInsurance-5.png)



#### 6. Build Sections A and B.

6.1. Open `TravelInsuranceFrontBuilder.cs` class and add these lines to `Build()` method:

```c#
                new TravelInsuranceSectionABuilder().Build(sectionBuilder);
                new TravelInsuranceSectionBBuilder().Build(sectionBuilder);
```

Create `TravelInsuranceSectionABuilder.cs` file with this content:

```c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static TravelInsurance.TravelInsuranceFormBuilder;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionABuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 3);

            AddTitle(tableBuilder, "Section A -  Personal Accident/Illness – " +
                "Medical And Additional Expenses");

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(3)
                        .SetPadding(0, 2, 0, 5)
                        .AddParagraphToCell("Documents required for Section A\n• original " +
                            "medical receipts and copy of discharge summary or available " +
                            "medical report")
            .ToTable()
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph("Have you suffered this illness or injury or a similar " +
                            "condition or a recurrence of a previous illness or injury? ")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 10, 0, 0)
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Yes", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" No")
                                .SetFont(FNT12)
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph("If yes, please specify:")
            .ToTable()
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 0)
                        .AddParagraphToCell("State amount claimed:")
                        .AddParagraph("$")
                            .SetFont(FNT12)
                .ToRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(1, 1, 0, 17)
                        .AddParagraph("Give name and address of your usual attending Doctor");
        }
    }
}
```

6.2. Create `TravelInsuranceSectionBBuilder.cs` class with this implementation:

```c#
using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using static TravelInsurance.TravelInsuranceFormBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionBBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 3);

            AddTitle(tableBuilder, "Section B - Cancellation/Curtailment/Postponement");

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(3)
                        .SetPadding(0, 2, 0, 5)
                        .AddParagraphToCell("Documents required for Section B\n• documents from " +
                            "carrier/travel agent and any relevant documents to support your claim");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"When and where was the trip booked?", 1 },
                    {"Intended Departure Date", 1 },
                    {"Date of cancellation ", 1}
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Why was the trip cancelled?", 3 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Amount paid by you", 1 },
                    {"Amount recovered from other sources", 1 },
                    {"Amount Claimed", 1}
                });
        }
    }
}
```

Contents of these sections are similar to contents of General Information section, they just have different texts, paddings in the cells and placement of the checkboxes. Using methods that were defined in `TravelInsuranceFormBuilder.cs` class are practical here. Run the program one more time to view the result:

![Fig.6](../Articles%20Images/TravelInsurance-6.png)    

Now first, front page of the document is ready.

#### 7. Build Sections C, D, E.

7.1. Insert these lines into `Build()` method of `TravelInsuranceBackBuilder.cs` class:

```c#
            new TravelInsuranceSectionCBuilder().Build(sectionBuilder);
            new TravelInsuranceSectionDBuilder().Build(sectionBuilder);
            new TravelInsuranceSectionEBuilder().Build(sectionBuilder);
```

Create `TravelInsuranceSectionCBuilder.cs` file:

```c#
using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static TravelInsurance.TravelInsuranceFormBuilder;


namespace TravelInsurance
{
    internal class TravelInsuranceSectionCBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 2, 0);

            AddTitle(tableBuilder, "Section C - Luggage & Personal Effects", 2);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 7)
                        .AddParagraphToCell("Documents required for Section C\n• Police Report " +
                            "and original purchase receipts and/or warranty cards")
            .ToTable()
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 17)
                        .AddParagraph()
                            .AddText("Item", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(30, TabulationType.Left)
                            .AddText("Description")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph()
                            .AddText("When and where", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(25, TabulationType.Left)
                            .AddText("Original purchased", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddText("Depreciation of wear", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(75, TabulationType.Left)
                            .AddText("Amount Claimed")
                    .ToCell()
                        .AddParagraph()
                            .AddText("purchased", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(25, TabulationType.Left)
                            .AddText("price", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddText("and tear");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"", 2 }
                }, 10);

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"", 2 }
                }, 10);
        }
    }
}
```

 What does this method do:

* Creates table with 2 columns with `CreateTable()` method.
* Adds title to this table using `AddTitle()` method.
* Adds a row with a cell that spans two columns, this cell contains text "Document...cards".
* Adds another row with 2 cells that have multiple paragraphs.
  * First cell has single paragraph with 2 pieces of texts separated by tabulation.
  * Second cell contains 2 paragraphs with 4 pieces of text separated by tabulation.
  * Tabulation is performed by using `addTabulationSymbol:true` parameter and `AddTabulationInPercent()`method.
* Adds two more rows that does not have any text.

7.2. Similarly, we can build Sections D and E.

Create `TravelInsuranceSectionDBuilder.cs` class and copy following code there:

```c#
using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using static TravelInsurance.TravelInsuranceFormBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionDBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 2);

            AddTitle(tableBuilder, "Section D - Flight Delayed/Misconnection", 2);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 10, 0, 0)
                        .AddParagraphToCell("Documents required for Section D\n• letter from " +
                            "Airlines/Carrier stating the reason and duration of delay");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Date", 1 },
                    {"Date ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Time", 1 },
                    {"Time ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Place of Departure", 1 },
                    {"Place of Departure ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Flight No.", 1 },
                    {"Flight No. ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Name of Airline", 1 },
                    {"Name of Airline ", 1 }
                });
        }
    }
}
```

Create `TravelInsuranceSectionEBuilder.cs` class and insert code below to the class:

```c#
using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using static TravelInsurance.TravelInsuranceFormBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionEBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 2);

            AddTitle(tableBuilder, "Section E - Baggage Delay", 2);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 5, 0, 6)
                        .AddParagraphToCell("Documents required for Section E\nBoarding Pass, " +
                            "Baggage Irregularity Report, Baggage acknowledgement slip and any" +
                            " other correspondence from the Airlines.");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Arrival Date", 1 },
                    {"Date ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Arrival Time", 1 },
                    {"Time ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Place of Departure", 1 },
                    {"Place", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Flight No.", 2 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Name of Airline", 2 }
                });
        }
    }
}
```

Run the project and on the second page of the document you should see:

![Fig.7](../Articles%20Images/TravelInsurance-7.png)



#### 8. Build Section F and the footer of the back page.

Insert a following line to `Build()` method of `TravelInsuranceBackBuilder.cs`:

```C#
            new TravelInsuranceSectionFBuilder().Build(sectionBuilder);
```

 Create `TravelInsuranceSectionFBuilder.cs` class and fill it with the code shown below:

 ```C#
using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static TravelInsurance.TravelInsuranceFormBuilder;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionFBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 2);

            AddTitle(tableBuilder, "Section F - Others", 2);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 6, 0, 0)
                        .AddParagraphToCell("In respect of any other claim, which does not fall " +
                            "within the sections stated above, please provide details of the " +
                            "claim you are submitting.  If the space below is insufficient for " +
                            "such details,");

            CreateRow(tableBuilder, new Dictionary<string, int>() { { "", 2 } }, 10);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 4, 0, 25)
                        .AddParagraph(SECTIONF_LONG_TEXT)
                            .SetLineSpacing(0.9f);
//line37
            sectionBuilder
                .AddTable()
                    .SetBorder(Stroke.None)
                    .AddColumnToTable("", 178)
                    .AddColumnToTable("", 104)
                    .AddColumnToTable("", 178)
                    .AddRow()
                        .AddCell()
                            .SetBorderWidth(0.58f)
                            .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.None)
                            .SetPadding(0, 2, 0, 16)
                            .AddParagraphToCell("Date")
                    .ToRow()
                        .AddCell()
                            .AddParagraphToCell("")
                    .ToRow()
                        .AddCell()
                            .SetBorderWidth(0.58f)
                            .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.None)
                            .SetPadding(0, 2, 0, 16)
                            .AddParagraphToCell("Signed here (Claimant)")
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .SetBorderWidth(0.58f)
                            .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.None)
                            .SetPadding(0, 2, 0, 16)
                            .AddParagraphToCell("Date")
                    .ToRow()
                        .AddCell()
                            .AddParagraphToCell("")
                    .ToRow()
                        .AddCell()
                            .SetBorderWidth(0.58f)
                            .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.None)
                            .SetPadding(0, 2, 0, 16)
                            .AddParagraphToCell("Signed here (Policyholder)")
            
        }
    }
}

 ```

This section starts like other sections of the document, but it has places for the signatures. Let's see how it is built (follow from the line 37):

* New table is created with `AddTable()` method, with 3 columns of widths 178px, 104px and 178px.
* A row is added to the table with 3 cells.
  * First cell has a paragraph with text and 0.58px top border.
  * Second cell is basically empty with no borders.
  * Third cell contains another paragraph with text and 0.58px top border.
* Another row is added with the same format as the first one.

After running the program here is what you get:

![Fig.8](../Articles%20Images/TravelInsurance-8.png)

To add a footer to the Section F where contact information of the person can be written, add following lines of code to the  end of `Build()` method

```c#
            .ToSection()
                .AddTable()
                    .SetBorder(Stroke.Solid)
                    .AddColumnToTable("", 178)
                    .AddColumnToTable("", 187)
                    .AddColumnToTable("", 187)
                    .AddRow()
                        .AddCell()
                            .SetPadding(1, 10.5f, 0, 9.5f)
                            .SetBorderWidth(0, 2, 0.5f, 0)
                            .AddParagraphToCell("Particulars of Agent Name")
                    .ToRow()
                        .AddCell()
                            .SetPadding(6.5f, 10.5f, 0, 9.5f)
                            .SetBorderWidth(0, 2, 0.5f, 0)
                            .AddParagraphToCell("Mobile")
                    .ToRow()
                        .AddCell()
                            .SetPadding(4.5f, 10.5f, 0, 9.5f)
                            .SetBorderWidth(0, 2, 0, 0)
                            .AddParagraphToCell("Email Address");
                            
```

Instead of drawing vertical lines, a new table is added that contains single row with 3 cells and only two borders between cells are set to be visible. The result can be viewed after executing the project:

![Fig.9](../Articles%20Images/TravelInsurance-9.png)

Thus, second page of the document is successfully built. 

# Summary
The above example shows how to create a two-page document that includes fill-in forms and images. 

The resulting TravelInsurance.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/TravelInsurance.pdf).

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/TravelInsurance).
