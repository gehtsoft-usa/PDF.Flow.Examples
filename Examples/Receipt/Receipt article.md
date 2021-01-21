##### Example: Receipt

# Purpose
The Receipt project is an example of the receipt generation. The example demonstrates a simple one-page document that includes a footer and a document flow area above it. This example also demonstrates the usage of an image, horizontal lines, a table with transparent borders and clickable URLs / emails.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/Receipt).

# Prerequisites
1) **Visual Studio 2017** or above is installed.
   To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
   To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### Table data
The values for the receipt table fields are located in the **Content/receipt.json** file:
```json
[
  {
    "Field": "Date",
    "Value": "2019-03-23 11:15AM PDT"
  },
  {
    "Field": "Account billed",
    "Value": "john-doe (John.Doe@gmail.com)"
  },
  {
    "Field": "Tariff plan",
    "Value": "40 Mbps"
  },
    {
    "Field": "Total",
    "Value": "$230.00 USD*"
  },
  {
    "Field": "Charged to",
    "Value": "American Express (7*** ****** *1234)"
  },
  {
    "Field": "Transaction ID",
    "Value": "QW3RT8UI"
  },
    {
    "Field": "For service through",
    "Value": "2019-09-22"
  }
]
```

### Provider logo
The picture of the provider logo is located in the **images/ReceiptExample/receipt.jpg** file.

### Output file
The example creates the **Receipt.pdf** file in the output **bin/(debug|release)/netcoreapp2.1** folder.


# Writing the source code

#### 1. Create new console application.
1.1.	Run Visual Studio  
1.2.	File -> Create -> Console Application (.Net Core)

#### 2. Modify class Program.
2.1. Set the path to the output PDF-file in the **Main()** function:

```c#
    Parameters parameters = new Parameters(null, "Receipt.pdf");
```
2.2. Call the **Run()** method for receipt generation and the **Build()** for building a document into the output PDF-file:

```c#
    ReceiptRunner.Run().Build(parameters.file);
```
2.3. After generation is completed, notify user regarding successful generation:
```c#
    Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                        + "\" document has been successfully built");
```


#### 3. Create class for running document generation

3.1. Create new document by calling **New()**  method of the DocumentBuilder. Then call the **AddReceipt()** method for adding the content to the document:

```c#
public static class ReceiptRunner
{
    public static DocumentBuilder Run()
    {
        return DocumentBuilder
            .New()
            .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
            .AddReceiptSection();
    }
}
```

3.2. Define the variables in **ReceiptRunner** class:

Path to the project root directory:
```c#
    private static readonly string ProjectDir;
```

Path to the `receipt.json` file:
```c#
    private static readonly string ReceiptJsonFile;
```

Content that was read from the`receipt.json`:
```c#
    private static readonly string ReceiptJsonContent;
```

Path to the `receipt.jpg` file:
```c#
    private static readonly string ImageUrl;
```

Data that was parsed from the `ReceiptJsonContent` variable:
```c#
    public static List<ReceiptData> ReceiptData { get; }
```

Manually defined text of the receipt:
```c#
    private static readonly string[] ReceiptText;
```

Different fonts used to create the document:
```c#
    private static readonly FontBuilder DocumentFont;
    private static readonly FontBuilder ItalicFont;
    private static readonly FontBuilder FooterFont;
    private static readonly FontBuilder BoldFont;
    private static readonly FontBuilder TitleFont;
    private static readonly FontBuilder HiddenFont;
```

3.3. Initialize those variables:
```c#
    static ReceiptRunner()
    {
        ProjectDir = Directory.GetCurrentDirectory();
        ReceiptJsonFile = Path.Combine(ProjectDir, "Content", "receipt.json");
        ReceiptJsonContent = File.ReadAllText(ReceiptJsonFile);
        ReceiptData = JsonConvert.DeserializeObject<List<ReceiptData>>(ReceiptJsonContent);
        ImageUrl = Path.Combine(ProjectDir, "images", "ReceiptExample", "receipt.jpg");

        DocumentFont = Fonts.Courier(14f);
        ItalicFont = FontBuilder.New().SetSize(14f).SetName("Times").SetItalic();
        FooterFont = Fonts.Times(12f).SetItalic().SetColor(Color.FromRgba(106.0 / 255.0, 85.0 / 255.9, 189.0 / 255.0));
        BoldFont = FontBuilder.New().SetSize(14f).SetName("Courier").SetBold();
        TitleFont = FontBuilder.New().SetSize(28f).SetName("Courier").SetBold();
        HiddenFont = Fonts.Courier(0.01f).SetColor(Color.White);

        ReceiptText = new[]
        {
            "We received payment for your subscription.",
            "Thanks for staying with us!",
            "Questions? Please contact "
        };
    }
```


#### 4. Build the document structure in the ResumeRunner class

4.1. Create a general method to add the receipt section

```c#
    private static DocumentBuilder AddReceiptSection(this DocumentBuilder builder)
    {
        builder
            .AddSection()
            .SetSectionSettings()
            .AddReceiptTitle()
            .AddReceiptText()
            .AddReceiptTable()
            .AddFooter();
        return builder;
    }
```

4.2. Set the section parameters (margins, the page size, the orientation, the numeration style)

```c#
    private static SectionBuilder SetSectionSettings(this SectionBuilder s)
    {
        s.SetMargins(30).SetSize(PaperSize.A4).SetOrientation(PageOrientation.Portrait).SetNumerationStyle();
        return s;
    }
```

4.3. Add the title block (which contains the centered image loaded from the local file, the title itself, and the  horizontal separation line with custom RGB color)

```c#
    private static SectionBuilder AddReceiptTitle(this SectionBuilder s)
    {
        s.AddImage(ImageUrl).SetScale(ScalingMode.OriginalSize).SetAlignment(HorizontalAlignment.Center);
        s.AddParagraph("Receipt").SetMargins(0, 20, 0, 10).SetFont(TitleFont).SetAlignment(HorizontalAlignment.Center);
        s.AddLine().SetColor(Color.FromRgba(106.0 / 255.0, 85.0 / 255.9, 189.0 / 255.0)).SetStroke(Stroke.Solid).SetWidth(2);
        return s;
    }
```

4.4. Add the receipt description block with the clickable email address

```c#
    private static SectionBuilder AddReceiptText(this SectionBuilder s)
    {
        ParagraphBuilder p = s.AddParagraph()
            .SetMargins(0, 20, 0, 10)
            .SetFont(DocumentFont);
        p.AddText(ReceiptText);
        p.AddUrl("support@yourhomeprovider.com");
        return s;
    }
```

4.5. Add the receipt table with zero borders and the data loaded from the `receipt.json` file (the receipt date, the account, the tariff plan, and etc.)

```c#
    private static SectionBuilder AddReceiptTable(this SectionBuilder s)
    {
        s.AddTable()
            .SetHeaderRowStyleBorder(builder => builder.SetStroke(Stroke.None).SetWidth(.0f))
            .SetHeaderRowStyleFont(HiddenFont)
            .SetContentRowStyleBorder(builder => builder.SetStroke(Stroke.None).SetWidth(.0f))
            .SetContentRowStyleFont(ItalicFont)
            .SetDataSource(ReceiptData);
        
        return s;
    }
```

4.6. Add the footer block with the clickable email support, the URL, the provider address, and the company name

```c#
    private static SectionBuilder AddFooter(this SectionBuilder s)
    {
        RepeatingAreaBuilder footer = s.AddFooterToBothPages(160);
        
        footer
            .AddLine()
            .SetColor(Color.FromRgba(106.0 / 255.0, 85.0 / 255.0, 189.0 / 255.0))
            .SetStroke(Stroke.Solid)
            .SetWidth(2);
        footer
            .AddParagraph()
            .SetMargins(0, 20, 0, 10)
            .AddText("Your Home Provider")
            .SetFont(BoldFont);
        footer
            .AddParagraph()
            .SetMargins(0, 0, 0, 0)
            .SetFont(FooterFont)
            .AddTextToParagraph("Your Home Provider, Inc.")
            .AddTabSymbol()
            .AddUrlToParagraph("support@yourhomeprovider.com")
            .AddTabulation(280);
        footer
            .AddParagraph()
            .SetMargins(0, 0, 0, 0)
            .SetFont(FooterFont)
            .AddTextToParagraph("123 John Doe Street")
            .AddTabSymbol()
            .AddUrlToParagraph("https://yourhomeprovider.com")
            .AddTabulation(280);
        footer
            .AddParagraph()
            .SetMargins(0, 0, 0, 0)
            .SetFont(FooterFont)
            .AddText(new[] { "Santa Barbara, CA 99999", "United States" });
        footer
            .AddParagraph()
            .SetMargins(0, 0, 0, 0)
            .SetFont(FooterFont)
            .AddText("*VAT / GST paid directly by Your Home Provider, Inc., where applicable");
        
        return s;
    }
```


#### 5. The generated **PDF file** must look as shown below:
The resulting Receipt.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/Receipt.pdf).