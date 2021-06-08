##### Example: MedicationSchedule

**MedicationSchedule** project is an example of generation of a **"Medication Schedule Form"** document. This example demonstrates creation of a PDF document that contains nested tables, fill-in forms and repeating areas. The example also shows how to add barcodes of the GS1-128 standard to PDF documents and how to style paragraphs and cells in the tables individually. 

The example's source code is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/MedicationSchedule). 

**Table of Content**  

- [Prerequisites](#prerequisites)
- [Purpose](#purpose)
- [Description](#description)
- [Writing the source code](#writing-the-source-code)
  - [Create a blank single page document](#1-create-a-new blank-PDF-document)
  - 
  - [Build a header](#3-build-a-header)
  - [Build "Drug and Dosage" table](#4-build-drug-&-dosage-table)
  - [Build a repeating footer](#5-build-a-repeating-footer)
- [Summary](#summary)

# Prerequisites
1. **Visual Studio 2017** or later is installed.
    To install a community version of Visual Studio, use the following link: 
    https://visualstudio.microsoft.com/vs/community/.
    Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.
2. **.NET Core Framework SDK 2.1** or later is installed.
To install the framework, use the following link: https://dotnet.microsoft.com/download.

# Purpose
The example shows how to create a Medication Schedule form, which is a complex two page document.

This document consists of the following parts (see figure below):
* Header
* "Drug and Dosage"  
* Repeating footer

![Fig. 1a](../Articles%20Images/MedicationSchedule_1a.png)

![Fig. 1b](../Articles%20Images/MedicationSchedule_1b.png)

Step-by-step instructions on how to build each of those parts are provided in this article.

# Description

#### Image of a hospital logo

Image of the logo of a fictitious health center is located in the **images/MedicationSchedule_Checkmark.png** directory.

#### Images of the checkmarks

Images of the checked and empty round checkmarks are located inside the **images** folder.

#### Output file
The example creates the **MedicationSchedule.pdf** file in the output **bin/debug/netcoreapp2.1** folder, unless specified otherwise in the command line.

# Writing the source code

## 1. Create a blank document.

We will start with creating a new blank PDF document. This should help you to understand how to set up the environment and run the project in order to view the resulting PDF file.

1.1. Open Visual Studio, select **File** > **Create** > **Console Application (.Net Core)**, specify the project name as **MedicationSchedule**.

1.2. Add the libraries Gehtsoft.PDFFlowLib and Gehtsoft.PDFFlowLib.Barcodes to the project, you can read about how to do it here: [Installing PDFFlow library](https://demo.gehtsoft.com/pdfflowdoc/web-content.html#InstallingPDFFlowLibrary.html).

1.3. Create an internal class `Parameters`  inside the class `Program` of the new project:

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

The class `Parameters` allows us to define the name of the PDF file that this project will generate. This class also allows to specify the application in which this file will be opened automatically after the project `MedicationSchedule` is run.   

1.4. Add a private method `Start`  to the `Program` class , this method opens output file in the specified application:

```c#
        private static void Start(string file, string appToView)
        {
            ProcessStartInfo psi = new ProcessStartInfo
                                    ("cmd", @"/c start " + appToView + " " + file);
            Process.Start(psi);
        }
```

Remove the errors by adding following namespaces to the top of the `Program` class:

```c#
using System.Diagnostics;
using System.IO;
```

1.4. Add another private method named `PrepareParameters` which is defined below:

```c#
        private static void PrepareParameters(Parameters parameters, string[] args)
        {
            if (args.Length > 0)
            {
                parameters.file = args[0];
                if (args.Length > 1)
                {
                    parameters.appToView = args[1];
                }
            }
        }
```

This method checks and processes optional parameters that may be passed to the `Main` method in the command line when the project is executed.

1.4. Now let's modify the `Main` method in the `Program` class. First it should create an instance of the `Parameters` class:

```c#
        static int Main(string[] args)
        {
            Parameters parameters = new Parameters(null, "MedicationSchedule.pdf");
        }
```

As you can see, we set the default value for the name of the output PDF file of the project to be **MedicationSchedule.pdf** . Next, the `Main` method calls the `PrepareParameters` method in order to process the optional parameters passed from the command line:

```c#
			PrepareParameters(parameters, args);
```

After that, it tries to build the PDF file and shows the message about successful building in the terminal window: 

```c#
            try {
                MedicationScheduleRunner.Run().Build(parameters.file);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                return 1;
            }
            Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                + "\" document has been successfully built");
```

Otherwise, in case the building of the document was unsuccessful, an error message should appear there. Finally, the `Start` method is called, if an application that opens the output PDF file is specified:

```c#
            if (parameters.appToView != null)
            {
                Start(parameters.file, parameters.appToView);
            }
            return 0;
```

Our `Program` class is ready, now let's move on directly to the building of an empty PDF document. 

1.6. Create a new `MedicationScheduleRunner` class  in our project:

```c#
using Gehtsoft.PDFFlow.Builder;
using System.Collections.Generic;
using System.IO;

namespace MedicationSchedule
{
    public static class MedicationScheduleRunner
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

This class contains the `Run` method which creates an instance of the `DocumentBuilder` class. Any PDF document that the PDFFlow library generates must have at least one section, so a section is added to the `documentBuilder`.  

1.7. Run the project, there are two ways to do this:

* From Visual Studio by clicking **F4**.

* From a command line: in the directory with the **MedicationSchedule.csproj**, run the command: 

```
dotnet run
```

You can set the optional parameters, for example, if you want to place the output file in the root directory of the project and view the file in **Microsoft Edge** browser, write:

```
dotnet run ../../../MedicationSchedule.pdf msedge
```

After the execution of the project, **MedicationSchedule.pdf** file is generated, open it to see that it contains a blank page.



## 2. Set page properties and prepare page contents.

2.1. Modify the class `MedicationScheduleRunner.cs`:

```c#
using Gehtsoft.PDFFlow.Builder;

namespace MedicationSchedule
{
    public class MedicationScheduleRunner
    {
        public static DocumentBuilder Run()
        {
            MedicationScheduleBuilder docBuilder = new MedicationScheduleBuilder(); 

            return docBuilder.Build();
        }
    }
}
```

We get an error that `MedicationScheduleBuilder` is not defined. To resolve it add a new internal class `MedicationScheduleBuilder.cs` to the project:

```c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Utils;

namespace MedicationSchedule
{
    internal class MedicationScheduleBuilder
    {

    }
}
```

This class will be responsible for setting the structure and the main parameters of the document pages. 

2.2. Define the fonts that will be used in the document as the fields of the `MedicationScheduleBuilder` class:

```c#
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8);
        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10);
        internal static readonly FontBuilder FNT10B = Fonts.Helvetica(10).SetBold();
        internal static readonly FontBuilder FNT11B = Fonts.Helvetica(11).SetBold();
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12);
```

These fonts are the instances of the `FontBuilder` class of the PDFFlow library.

2.3. Add an internal method `Build` to the class:

```c#
        internal DocumentBuilder Build()
        {
            var documentBuilder = DocumentBuilder.New();
            var sectionBuilder = documentBuilder.AddSection();

            return documentBuilder;
        }
```

The section `sectionBuilder` will contain all the pages of our document since those pages are of the same size and orientation. Before adding any content to the document, we should define those page parameters:

```c#
            sectionBuilder
                .SetOrientation(PageOrientation.Landscape)
                .SetMargins(36, 20, 37, 20)
                .SetSize(PaperSize.Letter);
```

As you can see pages will be standard Letter size with landscape orientation, also we set custom values for margins of the pages. 

Page properties are now set, let's add some exterior files to the project that we will use to fill our document with content. 

2.4. Copy the folders **images**, **Content** and **Model** from the [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/MedicationSchedule) to our **MedicationSchedule** project's root directory. 

- The **images** folder has PNG images of the logo of a fictitious health center and of the round checkmarks

- The **Content** folder has JSON files with most of the text used in the document. For example, **patient-info.json** file contains:

  ````json
  {
    "PatientName": "John Smith",
    "SSN": "123-45-6789",
    "DOB": "03/22/1985",
    "From": "02/14/2021",
    "To": "02/28/2021"
  }
  ````

  You may write your own data into the values of these JSON files.

- The **Model** folder contains C# classes that define C# objects which will be used to read data from those JSON files. For instance, the **PatientInfoData** class looks as follows:

  ```c#
  
  namespace MedicationSchedule.Model
  {
      public class PatientInfoData
      {
          public string PatientName { get; set; }
          public string SSN { get; set; }
          public string DOB { get; set; }
          public string From { get; set; }
          public string To { get; set; }
      }
  }
  ```

It should be mentioned that the PDFFlow library allows to write all the text of the documents directly inside the C# code. But in this example we parse the text from JSON files in order to make the process of adding or editing the information in the document easy.     

2.5. To deserialize data from JSON in this example we need methods of the `Newtonsoft.Json` library. 
In the Visual Studio select  **Project** > **Manage NuGet packages**. Type library's name and install latest version. Now we can access its methods by adding namespace `Newtonsoft.Json` to our project's classes. 



## 3. Build a header.

The header part of the final document is shown below:

![Fig. 1a](../Articles%20Images/MedicationSchedule_2.png)

This part has form fields that contain information about the patient who should take the medication as well as the health professionals who prescribed and approved those drugs. Also on the right side there is a separate section with additional notes and contact information. Let's add the header to our document. 

2.2. Modify the file `MedicationScheduleBuilder.cs`. 

Insert at the top of the file:

```c#
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Utils;
using System.IO;
```

Introduce constants to use them for reducing the size of the code.

```c#
    internal class MedicationScheduleBuilder
    {
        internal const PageOrientation Orientation = PageOrientation.Portrait;
        internal static readonly Box Margins = new Box(29, 21, 29, 0);

        internal static readonly string CheckboxPath = Path.Combine(
            Directory.GetCurrentDirectory(), "images", "MedicationSchedule_Checkbox.png");
        internal static readonly string LogoPath = Path.Combine(Directory.GetCurrentDirectory(),
                                        "images", "MedicationSchedule_Logo.png");

        internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7f);
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8f);
        internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11f);
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12f);
        internal static readonly FontBuilder FNT20B = Fonts.Helvetica(20f).SetBold();
        
```

Rewrite the `CreateDocumentBuilder()` method:

```C#
        internal DocumentBuilder CreateDocumentBuilder()
        {
            var documentBuilder = DocumentBuilder.New();
            new MedicationScheduleFrontBuilder().Build(documentBuilder);
            new MedicationScheduleBackBuilder().Build(documentBuilder);
            return documentBuilder;
        }

```

2.3. After that, create two new classes: `MedicationScheduleFrontBuilder.cs` and `MedicationScheduleBackBuilder.cs`. 
Both files should have at the top:

~~~c#
using Gehtsoft.PDFFlow.Builder;
using static MedicationSchedule.MedicationScheduleBuilder;

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

![Fig. 1a](../Articles%20Images/MedicationSchedule_4.png)

2.4. Add this line to the `Build()` method of `MedicationScheduleFrontBuilder.cs` class:

```c#
           new MedicationScheduleHeaderBuilder().Build(sectionBuilder);
```

Create `MedicationScheduleHeaderBuilder.cs` file and paste following code snippet into it:

 ```c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using static MedicationSchedule.MedicationScheduleBuilder;

namespace MedicationSchedule
{
    internal class MedicationScheduleHeaderBuilder
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

![Fig. 1a](../Articles%20Images/MedicationSchedule_5.png)

2.5. Let's add more text to the table

Remove last semicolon in the `Build` method and paste this code snippet on the new line:

```c#
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .SetColSpan(2)
                            .SetPadding(0, 13, 0, 8)
                            .SetFont(FNT7)
                            .AddParagraphToCell(ASTERISK_TEXT1)
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .SetColSpan(2)
                            .SetBorder(Stroke.Solid, Color.Black, 0.5f)
                            .SetPadding(8)
                            .SetFont(FNT8)
                            .AddParagraph()
                                .AddTextToParagraph(ASTERISK_TEXT2);

```

 We added two more rows to the table, each has a cell that spans two columns. These cells have different formatting and contain text of varying size. Check the changes by executing the project, you should get:

  ![Fig.6](../Articles%20Images/MedicationSchedule_6.png)

It should be mentioned that although expected design of the documents may not look like table, broad functionality of PDFFlow library allows to achieve required placement of elements on the pages for most documents simply by creating tables and configuring them.    



## 4. Build "Drug & Dosage" table.

![Fig.7](../Articles%20Images/MedicationSchedule_7.png)

3.1. Rewrite `Build()` method in the `MedicationScheduleHeaderBuilder.cs`, insert table with two columns with no borders, width of each column is 50% of width of the table:

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

![Fig.8](../Articles%20Images//MedicationSchedule_8.png)

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

![Fig.8](../Articles%20Images//MedicationSchedule_8.png)

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

![Fig.9](../Articles%20Images//MedicationSchedule_9.png)

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
                        .ToCell()
                            .AddParagraph("NPI:")
                                .ApplyStyle(styleParagraph2)
                                .AddTabSymbol()
                                .AddTabulationInPercent(44)
                                .AddText(physicianDetailsData.NPI)
                                    .SetFont(FNT9);
```

Run the project to view the table:

3.9. Add a horizontal 1px line of a custom color under the table:

```c#
                .ToSection()
                    .AddLine(PageWidth, 1, Stroke.Solid, blueGreen)
                        .SetMarginTop(5);
```



## 5. Build a repeating footer.

4.1. Rewrite `MedicationScheduleTestDetailsBuilder.cs` class:

```c#
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using MedicationSchedule.Model;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using static MedicationSchedule.MedicationScheduleBuilder;

namespace MedicationSchedule
{
    internal class MedicationScheduleTestDetailsBuilder
    {
        LabInfoData labInfoData = JsonConvert.DeserializeObject<LabInfoData>
            (File.ReadAllText(Path.Combine("Content", "lab-info.json")));

        internal void Build(SectionBuilder sectionBuilder, TestDetailsData testData)
        {
            var resultColor = Color.FromHtml("#417404");

            if (testData.ClinicalInfo == "INDETERMINATE" || testData.ClinicalInfo == "INSUFFICIENT")
                resultColor = Color.FromHtml("#F4A623");
            else if (testData.ClinicalInfo == "DETECTED" || testData.ClinicalInfo == "HIGH")
                resultColor = Color.Red;

        }
    }
}
```

We added logic to highlight value of Clinical info in different colors.

4.2. Insert two paragraphs, in order to separate text into two columns use tabulation methods of the library:

```c#
            sectionBuilder
                .AddParagraph("General Comments & Additional Information")
                    .SetMarginTop(14)
                    .SetFont(FNT9B)
                    .SetFontColor(blueGreen)
                    .AddTabSymbol()
                    .AddTabulationInPercent(41)
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
                    .AddTabulationInPercent(41)
                    .AddText(testData.OrderedItems)
                        .SetFont(FNT9);
```

Run the project, now the result of the tests is in different color on each page:

4.3. Add a new table with multiple columns as written below:

```c#
            .ToSection()
                .AddTable()
                    .SetMarginTop(11)
                    .SetBorderWidth(0, 0, 0, 1)
                    .SetBorderColor(blueGreen)
                    .SetWidth(XUnit.FromPercent(100))
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

![Fig.10](../Articles%20Images/MedicationSchedule_10.png)

4.4. Add description of the test result in a separate paragraph:

```c#
            .ToSection()
                .AddParagraph()
                    .SetMarginTop(12)
                    .SetFont(FNT11)
                    .SetLineSpacing(1)
                    .AddTextToParagraph(testData.Description)
```

4.4. Insert two horizontal lines with 1px width and paragraphs with multiple columns of text between the lines:

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
                    .AddTabulationInPercent(4)
                    .AddText(labInfoData.Code)
            .ToSection()
                .AddParagraph(labInfoData.Address)
                    .SetFont(FNT9)
                    .SetMarginLeft(91.4f)
            .ToSection()
                .AddLine(PageWidth, 1)
```

We could also use a table here, but since only one line of text requires tabulation in it, addition of simple paragraphs does the job.

4.6. Add a thin line of text with contact phone numbers aligned to the right side of the page:

```c#
            .ToSection()
                .AddParagraph("For inquiries, the physician may contact branch: " +
                            labInfoData.BranchPhone + " Lab: " + labInfoData.LabPhone)
                    .SetFont(FNT7)
                    .SetMarginTop(3)
                    .SetAlignment(HorizontalAlignment.Right);
                    
```

It should look on the second page as in the following picture:

![Fig.11](../Articles%20Images/MedicationSchedule_11.png)

# Summary
The example above showed how to create a complex two page document that includes tables, nested tables, images, and barcodes.

The resulting **MedicationSchedule.pdf** document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/MedicationSchedule.pdf).

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/MedicationSchedule).
