##### Example: LabResults

# Purpose
The LabResults project is an example of generation of a Laboratory Test Results Report document. The example demonstrates how to create a multi-page document that contains repeating headers, footers and images. This example also shows how to divide page into tables and paragraphs, parse document contents from json file and add tabulation to separate text parts on the same line.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/LabResults). The document consists of three pages, each containing

* Header
* Test Details
* Footer

Process of building each of these parts is described in this article step-by-step.

**Table of Contents**

- [Prerequisites](#prerequisites)
- [Resources](#resources)
    + [Images](#images)
    + [JSON](#json)
    + [Models](#model)
    + [Output file](#output-file)
- [Writing the source code](#writing-the-source-code)
    + [1. Create new console application.](#1-create-new-console-application)
    + [2. Create LabResultsBuilder class.](#2Create-LabResultsBuilder-class)
    + [3. Create three-page document.](#3-create-two-page-document)
    + [4. Build Header](#4-build-header)
    + [5. Build Test Details section](#5-build-test-details-section)
    + [6. Build Footer](#8-build-footer)
- [Summary](#summary)

# Prerequisites
1) **Visual Studio 2017** or above is installed.
   To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
   To install the framework use the following link: https://dotnet.microsoft.com/download

3) **Adobe Acrobat Reader** and/or **Google Chrome** (or any other Chromium based browser) to view PDF document.

# Resources

### Images

Images of sample company logo and barcode are in **images**  folder. 

### JSON

All json files used in this example are in **Content** folder.

### Models

Models are created in order to parse data from json files to the code. Each model is a c# public class that has public properties with names that match name of keys of key-value pairs written in respective json files. All models used in this example are in **Model** folder.

### Output file
The example creates the **LabResults.pdf** file in the output **bin/(Debug|Release)/netcoreapp2.1** folder by default.


# Writing the source code

#### 1. Create an empty document.
1.1. Run Visual Studio, create a new console application: **File -> Create -> Console Application (.Net Core)**.

1.2. Empty `Main()` method in the `Program.cs`, then set the path to the output PDF-file:

```c#
    string file = "LabResults.pdf";
```
1.2. Call the `Run()` method of a "runner" class for the document generation and the `Build()` method for building a document into an output PDF-file:

```c#
    LabResultsRunner.Run().Build(file);
```
1.3. We have an error that  `LabResultsRunner` is not defined. To resolve it, add a new class `LabResultsRunner.cs` and define the `Run()` method in it:

```c#
using Gehtsoft.PDFFlow.Builder;

namespace LabResults
{
    internal class LabResultsRunner
    {
        internal static DocumentBuilder Run()
        {
            return DocumentBuilder.New().AddSectionToDocument();
        }
    }
}
```

1.4. After running the project we get **LabResults.pdf** file in the output **bin/(Debug|Release)/netcoreapp2.1** folder. Open it with **Adobe Acrobat Reader** , it is empty since we haven't added anything to it yet. 
It might be inconvenient to close **Adobe Acrobat Reader** every time before running `LabResults` project and reopen it to view generated document. So let's make some changes in `Program.cs` that allow us to specify the options of the command line:

~~~c#
using System;
using System.Diagnostics;
using System.IO;

namespace LabResults
{
    class Program
    {
        static int Main(string[] args)
        {
            Parameters parameters = new Parameters(null, "LabResults.pdf");

            if (args.Length > 0)
            {
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
                    return 1;
                }
            }

            try
            {
                LabResultsRunner.Run().Build(parameters.file);
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
                var psi = new ProcessStartInfo("cmd", @"/c start " + 
                    parameters.appToView + " " + parameters.file);
                Process.Start(psi);
            }

            return 0;
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

As you can see, we added internal class `Parameters`, which will be used to parse the command line options to its properties values. Now each time we run the project, `Main()` method 

* First, fills  instance of `Parameters`type with property values: `appToView` and `file`.
* Second, deletes an existing `LabResults.pdf` file and displays an error message in case deletion cannot be done.
* Then, tries to generate the PDF document, shows a message if an attempt is successful.
* Finally, opens created document in the application, that was defined in the command line options. 

1.5. Open **Debug -> LabResults Debug Properties**, to specify where you want to place generated PDF file and what application will be used to view it. For example, to create the file in the project's root directory and view it in Google Chrome, write in **Application arguments** section **../../../LabResults.pdf chrome**. 

1.6. Run the project again. Now an empty document **LabResults.pdf** is automatically opened in your specified application.

#### 2. Create LabResultsBuilder class. 

2.1. Open Solution Explorer window: **View -> Solution Explorer**. In this window, right-click on **LabResults** project name and select **Open Folder in File Explorer**. 

2.1. Paste **Content**, **images** and **Model** folders into opened **LabResults** folder, after that you should see that they appear in the Solution Explorer window inside the project. Now we can use their contents in building the document.

2.3. Modify `LabResultsRunner.cs` class:

```c#
using Gehtsoft.PDFFlow.Builder;

namespace LabResults
{
    internal class LabResultsRunner
    {
        internal static DocumentBuilder Run()
        {
            LabResultsBuilder labResultsBuilder = new LabResultsBuilder(); 

            return labResultsBuilder.Build();
        }
    }
}
```

2.4. Create `LabResultsBuilder.cs` class and write implementation for the `Build()` method to remove error messages:

```c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;
using LabResults.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LabResults
{
    internal class LabResultsBuilder
    {
        internal DocumentBuilder Build()
        {
            var documentBuilder = DocumentBuilder.New();
            documentBuilder
                .AddSection()
                    .SetOrientation(Orientation)
                    .SetMargins(Margins);

            return documentBuilder;
        }
    }
}
```

2.5. Add constants and fields to this class which will be used throughout the project to optimize code size. Those constants define orientation, margins and width of pages as well as text fonts and color:

```c#
        internal const PageOrientation Orientation
            = PageOrientation.Portrait;
        internal static readonly Box Margins = new Box(29, 49, 29, 35);
        internal static readonly XUnit PageWidth = (PredefinedSizeBuilder.ToSize
     		(PaperSize.Letter).Width - (Margins.Left + Margins.Right));

        internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7);
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8);
        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(10);
        internal static readonly FontBuilder FNT9B = Fonts.Helvetica(9).SetBold();
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10);
        internal static readonly FontBuilder FNT10B = Fonts.Helvetica(10).SetBold();
        internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11);

        internal static readonly Color blueGreen = Color.FromHtml("#1B7A98");

```

 2.6. If you try to run the project now, you might get a `FILE_NOT_FOUND` error. In order to fix this, double-click on `LabResults` project name in Solution Explorer window and paste these settings just above `</Project>` tag:

```html
 <ItemGroup>
	<None Remove="Content\**\*.*" />
	<None Remove="images\**\*.*" />
 </ItemGroup>

 <ItemGroup>
	<Content Include="Content\**\*.*">
		<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
	</Content>
	<Content Include="images\**\*.*">
		<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
	</Content>
 </ItemGroup>
```



#### 3. Create three-page document. 

3.1. Rewrite `Build()` method in the `LabResultsBuilder.cs` file:

```c#
        internal DocumentBuilder Build()
        {
            var documentBuilder = DocumentBuilder.New();

            foreach (var testData in testDetailsData)
            {
                var sectionBuilder = documentBuilder.AddSection();
                sectionBuilder
                    .SetOrientation(Orientation)
                    .SetMargins(Margins);

                new LabResultsHeaderBuilder().Build(sectionBuilder);
                new LabResultsTestDetailsBuilder().Build(sectionBuilder, testData);
                new LabResultsFooterBuilder().Build();
            }

            return documentBuilder;
        }
```

3.2. Insert this code snippet just above `Build()` method:

```c#
        List<TestDetailsData> testDetailsData =
            JsonConvert.DeserializeObject<List<TestDetailsData>>
            (File.ReadAllText(Path.Combine("Content", "test-details.json")));
```

`test-details.json` file contains results of 3 tests, so we deserialize contents of the file by using methods of  `JsonConvert`class of `Newtonsoft.Json` namespace and store them in the `List<TestDetailsData>` collection. Then we enumerate through this list, as a result details of each test will be displayed on separate page. 

3.3. Let's get rid of error messages and try to build the document. First, create `LabResultsHeaderBuilder.cs` file, add image to the top of the pages:

``` c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using LabResults.Model;
using Newtonsoft.Json;
using System.IO;
using static LabResults.LabResultsBuilder;

namespace LabResults
{
    internal class LabResultsHeaderBuilder
    {

        PatientDetailsData patientDetailsData = JsonConvert.DeserializeObject<PatientDetailsData>
            (File.ReadAllText(Path.Combine("Content", "patient-details.json")));
        SpecimenDetailsData specimenDetailsData = JsonConvert.DeserializeObject<SpecimenDetailsData>
            (File.ReadAllText(Path.Combine("Content", "specimen-details.json")));
        PhysicianDetailsData physicianDetailsData =     JsonConvert.DeserializeObject<PhysicianDetailsData>
            (File.ReadAllText(Path.Combine("Content", "physician-details.json")));
        TestInfoData testInfoData = JsonConvert.DeserializeObject<TestInfoData>
            (File.ReadAllText(Path.Combine("Content", "test-info.json")));
        SpecimenInfoData specimenInfoData = JsonConvert.DeserializeObject<SpecimenInfoData>
            (File.ReadAllText(Path.Combine("Content", "specimen-info.json")));

        string logoPath = Path.Combine("images", "LabResults_Logo.png");
        string codePath = Path.Combine("images", "LabResults_Code.png");

        internal void Build(SectionBuilder sectionBuilder)
        {
            sectionBuilder
                .AddImage(logoPath);
        }
    }
}
```

3.4. Next, create `LabResultsTestDetailsBuilder.cs` file, contents of it are displayed below:

```c#
using Gehtsoft.PDFFlow.Builder;
using LabResults.Model;
using Newtonsoft.Json;
using System.IO;

namespace LabResults
{
    internal class LabResultsTestDetailsBuilder
    {
        LabInfoData labInfoData = JsonConvert.DeserializeObject<LabInfoData>
            (File.ReadAllText(Path.Combine("Content", "lab-info.json")));


        internal void Build(SectionBuilder sectionBuilder, TestDetailsData testData)
        {
        }
    }
}
```

3.5. Finally, create `LabResultsFooter.cs` class, implementation of the `Build()` method will be written later:

```c#
using LabResults.Model;
using Newtonsoft.Json;
using System.IO;

namespace LabResults
{
    internal class LabResultsFooterBuilder
    {
        ReportInfoData reportInfoData = JsonConvert.DeserializeObject<ReportInfoData>
            (File.ReadAllText(Path.Combine("Content", "report-info.json")));

        internal void Build()
        {
        }
    }
}
```

Run the project, you should get three-page document, each page contains an image of a logo:

![pic-1](../Articles%20Images/LabResults-1.png)



#### 4. Build Header. 

3.1. Rewrite `Build()` method in the `LabResultsHeaderBuilder.cs`, insert table with two columns with no borders, width of each column is 50% of width of the table:

```c#
        internal void Build(SectionBuilder sectionBuilder)
        {
            sectionBuilder
                .AddTable()
                    .SetBorderStroke(Stroke.None)
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("", 50)
                    .AddColumnPercentToTable("", 50)
                    .AddRow()
                        .AddCell()
                            .AddImageToCell(logoPath, 210, 37, ScalingMode.UserDefined)
                    .ToRow()
                        .AddCell()
                            .SetPadding(5, 8, 0, 0)
                            .AddParagraph(testInfoData.Report)
                                .SetFont(FNT10B);

        }
```

We added image of the logo with specified size to the left column of the table and a paragraph with text to the right column of the table. 

3.2. Add a 2px bold line below this table (from now on remove extra semicolons if they are highlighted as syntax errors):

```c#
            .ToSection()
                .AddLineToSection(PageWidth, 2);
```

Run the project to view the result:

![pic-2](../Articles%20Images/LabResults-2.png)

3.3. Add a new table with two columns to section, left column contains general information about specimen:

```c#
                .AddTable()
                    .SetBorderStroke(Stroke.None)
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("", 50)
                    .AddColumnPercentToTable("", 50)
                    .AddRow()
                        .AddCell()
                            .SetFont(FNT9B)
                            .SetFontColor(blueGreen)
                            .AddParagraph("Specimen ID:")
                                .SetMarginTop(2)
                                .AddTabSymbol()
                                .AddTabulationInPercent(25)
                                .AddText(specimenInfoData.SpecimenID)
                                    .SetFont(FNT9)
                         .ToCell()
                            .AddParagraph("Control ID:")
                                .SetMarginTop(2)
                                .AddTabSymbol()
                                .AddTabulationInPercent(25)
                                .AddText(specimenInfoData.ControlID)
                                    .SetFont(FNT9)
                         .ToCell()
                            .AddParagraph("Acct #:")
                                .SetMarginTop(0.5f)
                                .AddTabSymbol()
                                .AddTabulationInPercent(25)
                                .AddText(specimenInfoData.Acct)
                                    .SetFont(FNT9)
                         .ToCell()
                            .AddParagraph("Phone:")
                                .SetMarginTop(0.5f)
                                .AddTabSymbol()
                                .AddTabulationInPercent(25)
                                .AddText(specimenInfoData.Phone)
                                    .SetFont(FNT9)
                         .ToCell()
                            .AddParagraph("Rte:")
                                .SetMarginTop(0.5f)
                                .AddTabSymbol()
                                .AddTabulationInPercent(25)
                                .AddText(specimenInfoData.Rte)
                                    .SetFont(FNT9)
                    .ToRow()
                        .AddCell();
```

All the text is inside single cell of the column, each new line of text is added with `Addparagraph()` method, also tabulation is set using `AddTabSymbol()` and `AddTabulationInPercent()` methods called consecutively. Run the project, you should see:

![pic-3](../Articles%20Images/LabResults-3.png)

3.4. Add information about test to the right column of the table, insert image of the barcode in the end:

```c#
                            .SetPadding(5, 0, 0, 0)
                            .SetFont(FNT10)
                            .AddParagraph(testInfoData.Master)
                                .SetMarginTop(2)
                        .ToCell()
                            .AddParagraphToCell(testInfoData.Account)
                            .AddParagraphToCell(testInfoData.Address1)
                            .AddParagraphToCell(testInfoData.Address2)
                            .AddImage(codePath)
                                .SetMarginTop(10);
```

Run the project to view the result:

![pic-4](../Articles%20Images/LabResults-4.png)

 3.5. Add another table that has three columns, only left border of 0.5px of each cell is set to be visible:

```c#
            .ToSection()
                .AddTable()
                    .SetMarginTop(15)
                    .SetBorderWidth(0.5f, 0, 0, 0)
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("", 31)
                    .AddColumnPercentToTable("", 32)
                    .AddColumnPercentToTable("", 37)
                
```

3.6. Insert these two variables to the beginning of `Build()` method, use of `StyleBuilder` class is very convenient since it allows to define certain style only once and apply it repeatedly throughout the document:

```c#
            var styleParagraph1 = StyleBuilder.New().SetLineSpacing(1)
                                .SetFont(FNT9B).SetFontColor(blueGreen);

            var styleParagraph2 = StyleBuilder.New().SetLineSpacing(0.5f)
                                .SetFont(FNT9B).SetFontColor(blueGreen);

```

3.7. Add a row to the last table, first cell of the row contains details about patient:

```c#
                    .AddRow()
                        .SetFont(FNT10B)
                        .AddCell()
                            .SetPadding(12, 0, 0, 0)
                            .AddParagraph("Patient Details")
                                .SetLineSpacing(1)
                        .ToCell()
                            .AddParagraph("DOB:")
                                .ApplyStyle(styleParagraph1)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(patientDetailsData.DOB)
                                    .SetFont(FNT9)
                        .ToCell()
                            .AddParagraph("Age(y/m/d)")
                                .ApplyStyle(styleParagraph1)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(patientDetailsData.Age)
                                    .SetFont(FNT9)
                        .ToCell()
                            .AddParagraph("Gender:")
                                .ApplyStyle(styleParagraph1)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(patientDetailsData.Gender)
                                    .SetFont(FNT9)
                        .ToCell()
                            .AddParagraph("Patient ID:")
                                .ApplyStyle(styleParagraph2)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(patientDetailsData.PatientID)
                                    .SetFont(FNT9)
                    .ToRow()
                    	.AddCellToRow()
                    	.AddCellToRow();

```

Now run the project, the table should look like this:

![pic-5](../Articles%20Images/LabResults-5.png)

3.8. Remove last two line of code in `Build()`method and add two cells to the row of the table, these cells contain details about specimen and physician:

```c#
                        .SetFont(FNT10B)
                        .AddCell()
                            .SetPadding(12, 0, 0, 0)
                            .AddParagraph("Specimen Details")
                                .SetLineSpacing(1)
                        .ToCell()
                            .AddParagraph("Date collected:")
                                .ApplyStyle(styleParagraph1)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(specimenDetailsData.Collected)
                                    .SetFont(FNT9)
                        .ToCell()
                            .AddParagraph("Date received:")
                                .ApplyStyle(styleParagraph1)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(specimenDetailsData.Received)
                                    .SetFont(FNT9)
                        .ToCell()
                            .AddParagraph("Date entered:")
                                .ApplyStyle(styleParagraph1)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(specimenDetailsData.Entered)
                                    .SetFont(FNT9)
                        .ToCell()
                            .AddParagraph("Date reported:")
                                .ApplyStyle(styleParagraph2)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(specimenDetailsData.Reported)
                                    .SetFont(FNT9)
                    .ToRow()
                        .SetFont(FNT10B)
                        .AddCell()
                            .SetPadding(12, 0, 0, 0)
                            .AddParagraph("Physician Details")
                                .SetLineSpacing(1)
                        .ToCell()
                            .AddParagraph("Ordering:")
                                .ApplyStyle(styleParagraph1)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(physicianDetailsData.Ordering)
                                    .SetFont(FNT9)
                        .ToCell()
                            .AddParagraph("Referring:")
                                .ApplyStyle(styleParagraph1)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(physicianDetailsData.Referring)
                                    .SetFont(FNT9)
                        .ToCell()
                            .AddParagraph("ID:")
                                .ApplyStyle(styleParagraph1)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(physicianDetailsData.ID)
                                    .SetFont(FNT9)
                        .ToCell()
                            .AddParagraph("NPI:")
                                .ApplyStyle(styleParagraph2)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(physicianDetailsData.NPI)
                                    .SetFont(FNT9);
```

Run the project to view the table:

![pic-6](../Articles%20Images/LabResults-6.png)

3.9. Add a horizontal 1px line of a custom color under the table:

```c#
                .ToSection()
                    .AddLine(PageWidth, 1, Stroke.Solid, blueGreen)
                        .SetMarginTop(5);
```

Now each page of the document has this header: 

![pic-7](../Articles%20Images/LabResults-7.png)



#### 5. Build Test Details section. 

5.1. Rewrite  `LabResultsTestDetailsBuilder.cs` class:

```c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using LabResults.Model;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using static LabResults.LabResultsBuilder;

namespace LabResults
{
    internal class LabResultsTestDetailsBuilder
    {
        LabInfoData labInfoData = JsonConvert.DeserializeObject<LabInfoData>
            (File.ReadAllText(Path.Combine("Content", "lab-info.json")));

        internal void Build(SectionBuilder sectionBuilder, TestDetailsData testData)
        {
            var resultColor = Color.FromHtml("#417505");

            if (testData.ClinicalInfo == "INDETERMINATE")
                resultColor = Color.FromHtml("#F5A623");
            else if (testData.ClinicalInfo == "DETECTED")
                resultColor = Color.Red;

        }
    }
}
```

We added logic to highlight value of Clinical info in different colors. 

5.2. Insert two paragraphs, in order to separate text into two columns use tabulation methods of the library:

```c#
            sectionBuilder
                .AddParagraph("General Comments & Additional Information")
                    .SetMarginTop(14)
                    .SetFont(FNT9B)
                    .SetFontColor(blueGreen)
                    .AddTabSymbol()
                    .AddTabulationInPercent(51)
                    .AddText("Ordered Items")
            .ToSection()
                .AddParagraph("Clinical Info:")
                    .SetFont(FNT9B)
                    .AddTabSymbol()
                    .AddTabulationInPercent(10)
                    .AddText(testData.ClinicalInfo)
                        .SetFontColor(resultColor)
                .ToParagraph()
                    .AddTabSymbol()
                    .AddTabulationInPercent(51)
                    .AddText(testData.OrderedItems)
                        .SetFont(FNT9);
```

Run the project, now the result of the test is in different color on each page:

![pic-8](../Articles%20Images/LabResults-8a.png)

![pic-8b](../Articles%20Images/LabResults-8b.png)

![pic-8c](../Articles%20Images/LabResults-8c.png)

5.3. Add a new table with multiple columns as written below:

```c#
            .ToSection()
                .AddTable()
                    .SetMarginTop(11)
                    .SetBorderWidth(0, 0, 0, 1)
                    .SetBorderColor(blueGreen)
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("TESTS", 22)
                    .AddColumnPercentToTable("RESULT", 22)
                    .AddColumnPercentToTable("FLAG", 12)
                    .AddColumnPercentToTable("UNITS", 9)
                    .AddColumnPercentToTable("REFERENCE INTERVAL", 29)
                    .AddColumnPercentToTable("LAB", 6)
                    .SetHeaderRowStyleBackColor(blueGreen)
                    .SetHeaderRowStyleHorizontalAlignment(HorizontalAlignment.Center)
                    .SetHeaderRowStyleFont(FNT10.SetColor(Color.White))
                    .AddRow()
                        .SetFont(FNT11)
                        .SetHorizontalAlignment(HorizontalAlignment.Center)
                        .AddCellToRow(testData.Tests)
                        .AddCellToRow(testData.Result)
                        .AddCellToRow(testData.Flag)
                        .AddCellToRow(testData.Units)
                        .AddCellToRow(testData.ReferenceInterval)
                        .AddCellToRow(testData.Lab);
                        
```

This table has two rows, one of them is a header row, we set its background color, text font and alignment with `SetHeaderRowStyle()` methods. Run the project one more time to view the table:

![pic-9](../Articles%20Images/LabResults-9.png)

5.4. Add description of the test result in a separate paragraph:

```c#
            .ToSection()
                .AddParagraph()
                    .SetMarginTop(12)
                    .SetFont(FNT11)
                    .SetLineSpacing(1)
                    .AddTextToParagraph(testData.Description)
```

 5.5. Insert two horizontal lines with 1px width and paragraphs with multiple columns of text between the lines:

```c#
            .ToSection()
                .AddLine(PageWidth, 1)
                    .SetMarginTop(17)
            .ToSection()
                .AddParagraph(labInfoData.Index)
                    .SetFont(FNT9)
                    .SetMarginLeft(30)
                    .SetLineSpacing(0.8f)
                    .AddTabSymbol()
                    .AddTabulationInPercent(5)
                    .AddText(labInfoData.Code)
                .ToParagraph()
                    .AddTabSymbol()
                    .AddTabulationInPercent(12)
                    .AddText(labInfoData.Name)
                .ToParagraph()
                    .AddTabSymbol()
                    .AddTabulationInPercent(61)
                    .AddText(labInfoData.Director)
            .ToSection()
                .AddParagraph(labInfoData.Address)
                    .SetFont(FNT9)
                    .SetMarginLeft(91.5f)
            .ToSection()
                .AddLine(PageWidth, 1)
```

We could also use a table here, but since only one line of text requires tabulation in it, addition of simple paragraphs does the job. 

5.6. Add a thin line of text with contact phone numbers aligned to the right side of the page:

```c#
            .ToSection()
                .AddParagraph("For inquiries, the physician may contact branch: " +
                            labInfoData.BranchPhone + " Lab: " + labInfoData.LabPhone)
                    .SetFont(FNT7)
                    .SetMarginTop(3)
                    .SetAlignment(HorizontalAlignment.Right);
                    
```

Test Results section is ready, it should look as in the following picture:

![pic-10](../Articles%20Images/LabResults-10.png)



#### 6. Build Footer. 

6.1. Rewrite `Build()` method in the `LabResultsFooterBuilder.cs`, we can use special `.AddFooterToBothPages()` method of PDFFlow library, it automatically aligns the footer at the bottom of each page:

```c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using LabResults.Model;
using Newtonsoft.Json;
using System.IO;
using static LabResults.LabResultsBuilder;

namespace LabResults
{
    internal class LabResultsFooterBuilder
    {
        ReportInfoData reportInfoData = JsonConvert.DeserializeObject<ReportInfoData>
            (File.ReadAllText(Path.Combine("Content", "report-info.json")));

        internal void Build(SectionBuilder sectionBuilder)
        {
            sectionBuilder
                .AddFooterToBothPages(45)
        }
    }
}

```

6.2. Add following code snippet to `Build()` method  to fill footer with contents:

```c#
                    .AddLine(PageWidth, 2)
                .ToArea()
                    .AddTable()
                        .SetBorderStroke(Stroke.None)
                        .SetWidth(XUnit.FromPercent(100))
                        .AddColumnPercentToTable("", 65)
                        .AddColumnPercentToTable("", 35)
                        .AddRow()
                            .AddCell()
                                .SetPadding(0, 1, 0, 0)
                                .AddParagraph("FINAL REPORT")
                                    .SetMarginTop(1)
                                    .SetFont(FNT10B)
                                    .AddTabSymbol()
                                    .AddTabulationInPercent(62)
                                    .AddText("Date Issued: " + reportInfoData.Date)
                                        .SetFont(FNT8)
                         .ToRow()
                            .AddCell()
                                .AddParagraph("Page 1 of 1")
                                    .SetFont(FNT8)
                                    .SetAlignment(HorizontalAlignment.Right)
                    .ToTable()
                        .AddRow()
                            .AddCell()
                                .SetFont(FNT7Half)
                                .AddParagraph("This document contains private and confidential " +
                                        "health information protected by state and federal law.")
                                    .SetMarginTop(3)
                                    .SetLineSpacing(1)
                             .ToCell()
                                .AddParagraph("If you have received this document in error, " +
                                        "please call " + reportInfoData.Phone)
                        .ToRow()
                            .AddCell()
                                .SetHorizontalAlignment(HorizontalAlignment.Right)
                                .SetFont(FNT7Half)
                                .AddParagraph(reportInfoData.Copyright)
                            .ToCell()
                                .AddParagraph(reportInfoData.Version);

```

First, we insert a 2px horizontal line to distinguish start of the footer. Then, we added a table to put text with tabulation on the same lines into columns. Two parts of the text at the bottom have different horizontal alignment so we can't just add text into paragraphs as we did above. But we can easily specify alignment of the text inside each cell of the table.

Run the project to view the footer at the bottom of each page of our document:

![pic-12](../Articles%20Images/LabResults-12.png) 

# Summary

The above example shows how to create a multi-page document that contains tables and paragraphs of various styles,  repeating headers, footers and images. 

The resulting LabResults.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/LabResults.pdf).
