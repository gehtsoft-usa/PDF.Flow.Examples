##### Example: Resume

# Purpose
The resume project is an example of the resume/CV generation. The example demonstrates a simple multi-page document that includes right repeating area on both pages (even and odd) with the same content and document flow area on the left side of page. This example also demonstrates the usage of the vertical line (between the document flow and the repeating area) and clickable URLs.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/Resume).

# Prerequisites
1) **Visual Studio 2017** or above is installed.
   To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
   To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### Output file
The example creates the file **Resume.pdf** in the output **bin/(debug|release)/netcoreapp2.1** folder.


# Writing the source code

#### 1. Create new console application.
1.1.	Run Visual Studio  
1.2.	File -> Create -> Console Application (.Net Core)

#### 2. Modify class Program.
2.1. Set the path to the output PDF-file In the **Main()** function:

```c#
    Parameters parameters = new Parameters(null, "Resume.pdf");
```
2.2. Call the **Run()** method  for the resume generation and the **Build()** to build a document in the output PDF-file:

```c#
    ResumeRunner.Run().Build(parameters.file);
```
2.3. After generation is completed, notify user regarding successful generation::

```c#
    Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                        + "\" document has been successfully built");
```


#### 3. Create class for running document generation

Create new document by calling the **New()** method of the DocumentBuilder. Then call the **AddResume()** method to add the content to the document:
```c#
public static class ResumeRunner
{
    public static DocumentBuilder Run()
    {
        return DocumentBuilder.New()
            .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
            .AddResume();
    }
}
```


#### 4. Build document structure

4.1. Create the **ResumeBuilder** class which will contain the method to build the document structure

```c#
public static class ResumeBuilder
```

4.2. Define the fields of the **HouseRentalContractBuilder** class.

Path to the Bill Gates image file:

```c#
private static readonly string BillGatesPath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", "DocumentFlowArea", "Bill_Gates.jpg");
```

4.3. Create the **AddResume()** method.

This method creates one section by calling the **AddSection()** method of the **DocumentBuilder**, sets the orientation and margins. Also it calls the methods to add the content to the sections. All these methods must be extension methods of the **SectionBuilder** and they must return the **SectionBuilder**, so we can call them in an uninterrupted chain:

```c#
    public static DocumentBuilder AddResume(this DocumentBuilder builder)
    {
        builder.ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f));
        var section = builder.AddSection().SetOrientation(PageOrientation.Portrait).SetMargins(20f, 30f);
        
        section
            .AddRightArea()
            .AddSeparationArea()
            .AddMainDocumentFlow();
        
        return builder;
    }
```

4.4. **Add the right repeating area** on both pages (even and odd).

We call the `AddRptAreaRightToBothPages` method passing the width of this area as a parameter. Here we add some paragraphs with the text or the URLs and we apply custom formatting for them, such as the font size or the font color.

```c#
    internal static SectionBuilder AddRightArea(this SectionBuilder s)
    {
        RepeatingAreaBuilder rightAreaBuilder = s.AddRptAreaRightToBothPages(200f);
        rightAreaBuilder
            .AddLine()
            .SetWidth(2f);
        rightAreaBuilder
            .AddParagraph()
            .SetMarginTop(10f)
                .AddText(" Contact")
                .SetFontSize(16f);
        rightAreaBuilder
            .AddParagraph()
            .SetMarginTop(7f)
            .SetFontSize(14f)
            .AddTextToParagraph("+1-202-555-0163 ")
                .AddText("(Mobile)")
                .SetFontColor(Color.Gray);
        rightAreaBuilder
            .AddParagraph()
            .SetFontSize(14f)
            .AddTextToParagraph("email@example.com");
        rightAreaBuilder
            .AddParagraph()
            .SetMarginTop(10f)
            .SetFontSize(14f)
            .SetTextOverflowAction(TextOverflowAction.Ellipsis)
            .AddUrlToParagraph("https://www.linkedin.com/in/williamhgates", "www.linkedin.com/in/williamhgates")
                .AddText(" (LinkedIn)")
                .SetFontColor(Color.Gray);
        rightAreaBuilder
            .AddParagraph()
            .SetFontSize(14f)
            .SetTextOverflowAction(TextOverflowAction.Ellipsis)
            .AddUrlToParagraph("https://www.gatesnotes.com/", "www.gatesnotes.com")
                .AddText(" (Site)")
                .SetFontColor(Color.Gray);
        return s;
    }
```

4.5. **Add the vertical separation line** by using a thin right repeating area, 20 points in width, that will be added before the previous right repeating area (this is a line whose 

* width equals to the page's height subtracted by vertical margins, 

* length is 0.5 points.

  By the way of explanation, we swapped the parameters of width and length so that the horizontal line becomes the vertical one:

```c#
    internal static SectionBuilder AddSeparationArea(this SectionBuilder s)
    {
        s.AddRptAreaRightToBothPages(20f)
            .AddLine(length: 0.5f, width: s.PageSize.Height - s.Margins.Vertical)
            .SetMarginLeft(9f)
            .SetColor(Color.Gray);
        return s;
    }
```

4.6. **Add general document flow** to the current section (with the original-sized image, the text paragraph and the horizontal lines separating the headers)

```c#
    internal static SectionBuilder AddMainDocumentFlow(this SectionBuilder s)
    {
        s.AddLine()
            .SetWidth(2f);
        s.AddImage(BillGatesPath, ScalingMode.OriginalSize)
            .SetMarginLeft(1.5f)
            .SetMarginTop(7f);
        s.AddParagraph("Bill Gates").SetMarginTop(6.5f).SetFontSize(28f);
        s.AddParagraph("Co-chair, Bill & Melinda Gates Foundation").SetFontSize(14f).SetMarginTop(5f);
        s.AddParagraph("Seattle").SetFontSize(14f).SetFontColor(Color.Gray);

        s.AddParagraph("Summary").SetFontSize(18f).SetMarginTop(20f);
        s.AddParagraph(
            "Co-chair of the Bill & Melinda Gates Foundation. Microsoft Co-founder. Voracious reader. Avid traveler. Active blogger.")
            .SetFontSize(14f)
            .SetMarginTop(10f);

        s.AddLine(40f).SetColor(Color.Gray).SetMarginTop(20f);

        s.AddParagraph("Experience").SetFontSize(18f).SetMarginTop(10f);
        s.AddParagraph("Bill & Melinda Gates Foundation").SetFontSize(14f).SetMarginTop(15f);
        s.AddParagraph("Co-chair").SetFontSize(12f);
        s.AddParagraph("2000 - Present (20 years)").SetFontSize(12f);
        s.AddParagraph("Microsoft").SetFontSize(14f).SetMarginTop(20f);
        s.AddParagraph("Co-founder").SetFontSize(12f);
        s.AddParagraph("1975 - Present (45 years)").SetFontSize(12f);

        s.AddLine(40f).SetColor(Color.Gray).SetMarginTop(20f);

        s.AddParagraph("Education").SetFontSize(18f).SetMarginTop(10f);
        s.AddParagraph("Harvard University").SetFontSize(14f).SetMarginTop(15f);
        s.AddParagraph(" · (1973 - 1975)").SetFontSize(12f);
        s.AddParagraph("Lakeside School, Seattle").SetFontSize(14f).SetMarginTop(20f);
         
        return s;
    }
```


#### 5. Generated **PDF file** must look as shown below:
The resulting Resume.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/Resume.pdf).