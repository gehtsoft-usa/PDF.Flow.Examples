##### Example: TutorialC

# Purpose
TutorialC project is an example of а programming tutorial book. The example demonstrates creation of a multi-page text document with mixed content. The document includes several sections, and a repeating footer with automatic page numeration. Additionally, this example shows how to use images and how to work with tables.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/TutorialC).

# Prerequisites
1) **Visual Studio 2017** or above is installed.
To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### Image files

The document image files is located in the folder **Images\TutorialC**.

### Additional data

The document additional data is located in the file **Content/tutorialc_keywords.json**. The file contains a description of the C keywords. This data is used to build the table. The file is a standard JSON.
```json
[
  {
    "Id": "1",
    "Keyword": "auto",
    "Description": "The auto keyword declares automatic variables"
  },
  {
    "Id": "2",
    "Keyword": "break",
    "Description": "The break statement makes program jump out of the innermost enclosing loop (while, do, for or switch statements) explicitly."
  },

]
//etc.
```

### Output file

The example creates the file **TutorialC.pdf** in the output **bin/(debug|release)/netcoreapp2.1** folder.

# Writing the source code

## Create project

#### 1. Create new console application.
1.1.	Run Visual Studio  
1.2.	File -> Create -> Console Application (.Net Core)

#### 2. Modify class Program.
2.1. In the function **Main()** set path to output the PDF file:
```c#
    var pathDocument = "TutorialC.pdf";
```
2.2. Call method **Run()** for resume generation and **Build()** for building a document into output PDF:
```c#
    TutorialCRunner.Start(pathDocument);
```
2.3. After generation is completed, notify user of successful generation:
```c#
    Console.WriteLine("document has been successfully built");
```

## Sources structure

The document consists of a few logical parts, and we use Create methods to generate each logical part:
- AddBookCoverSection() - create the cover image of the book
- AddFirstSection() - create the first section of the book
- AddSecondSection() - create the 'Basic syntax' section of the book
- AddThirdSection() - create the 'Desicion making' section of the book
- AddOutlineSection() - add an outline (Table of Contents) to the book

## Global definitions (TutorialCRunner() constructor)

Let's define the necessary paths.
```c#
ProjectDir = Directory.GetCurrentDirectory();
```
Let's define the required data from the JSON data file.
```c#
KeywordsJsonFile = Path.Combine(ProjectDir, "Content", "tutorialc_keywords.json");
KeywordsJsonContent = File.ReadAllText(KeywordsJsonFile);
KeywordsList = JsonConvert.DeserializeObject<List<Keywords>>(KeywordsJsonContent);
```
Let's define the path for the image files.
```c#
ImageUrl = Path.Combine(ProjectDir, "images", "TutorialC", "customers");
```
Let's define the fonts.
```c#
DocumentFont = Fonts.Times(12f);
//etc.
```

## Optimization of the code.

To add footers several times with a different text, we added a new method. Defining methods for common operations is a good optimization practice to achieve better code size.

```c#
    internal static void AddFooters(SectionBuilder section, string text)
    {
        var imageUrlLogo = Path.Combine(ProjectDir, "images", "DocumentExample", "LearnCLogo30_30.jpg");
        section.AddFooterToBothPages(40)
            .AddLine()
                .SetColor(ColorBackground).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToArea()
                .AddParagraph()
                    .SetMargins(0, 5, 0, 0).SetFont(DocumentFontBoldOrange)
                    .AddInlineImageToParagraph(imageUrlLogo,
                        new XSize(15, 15), ScalingMode.UserDefined)
                    .AddTextToParagraph(text)
                    .AddUrl("http://www.gehtsoftusa.com/", "GEHTSOFT USA LLC.")
                        .SetFont(DocumentFontBoldOrange)
                        .SetUnderline(ColorBackground);
        section.AddFooterToBothPages(15f)
            .AddParagraph()
                .AddPageNumberToParagraph(" Page #", 1, SmallFont)
                .SetAlignment(HorizontalAlignment.Right);
    }
```

## Cover section (AddBookCoverSection() method)
We used an image for the cover of the book. Let's define the path to the image file.
```c#
var imageUrl = Path.Combine(ProjectDir, "images", "TutorialC", "DocumentExample", "LearnCCoverPage.jpg");
```
We also want to add a link from the table of contents. To do this, let's define a hidden font.
```c#
var fontHidden = Fonts.Courier(.01f).SetColor(Color.White);
```
Let's add a hidden label for the table of contents.
```c#
builder
    .AddSection()
        .SetSize(PaperSize.A4)
        .SetOrientation(PageOrientation.Portrait)
        .AddParagraph("LEARN C PROGRAMMING")
            .SetFont(fontHidden)
            .SetBackColor(Color.White).SetOutline().SetOutline(0)
```
Let's add an image to decorate the book cover.
```c#
    .ToSection()
        .AddImage(imageUrl).SetScale(ScalingMode.Stretch);
```
## First section 'Environment setup' (AddFirstSection() method)
Let's define the necessary paths to the images: imageUrlLogo, imageUrl, imageExclamationMark, imageQuestionMark.
Let's create a title that will be added to the table of contents.
```c#
    .AddParagraph()
        .SetFont(DocumentFontTitleWhite)
        .SetAlignment(HorizontalAlignment.Center)
        .SetBackColor(ColorBackground)
        .SetBorder(Stroke.Solid, Color.Gray, 3)
        .SetOutline(1)
        .AddText("1. Environment setup")
```

Next we will add paragraphs and content to the section. We add images to paragraphs so that the reader's attention is drawn to the important information.
```c#
    .AddInlineImageToParagraph(imageExclamationMark, new XSize(15, 15), ScalingMode.UserDefined)
```

If you need to skip line breaks in the text, you can specify the ignoreNewLineSymbol parameter:
```c#
    .AddText(@" to set up your own environment to start learning C 
programming language. Reason is very simple, we already have set up C 
Programming environment online, so that you can compile and execute all the 
available examples online at the same time when you are doing your theory 
work. This gives you confidence in what you are reading and to check the result 
with different options. Feel free to modify any example and execute it online.",
            ignoreNewLineSymbol: true)
```

Let's add a hyperlink to indicate the useful source of information.
```c#
    .AddUrlToParagraph("http://www.compileonline.com/")
```

To accurately convey the text of the program, we used the @ strings and passed the strings as an array.
```c#
    .AddText(
    new[]
    {
        "#include <stdio.h>",
        " ",
        "int main()",
        "{",
        @"    /* my first program in C */",
        @"    printf(""Hello, World! \n"");",
        @"    return 0;",
        "}"
    })
```

## Second section 'Basic syntax' (AddSecondSection() method)
Let's define the necessary paths to the images: imageUrlLogo, imageExclamationMark. 
Let's create a title that will be added to the table of contents.
```c#
    .AddParagraph("2. Basic syntax").SetFont(DocumentFontTitleWhite)
        .SetAlignment(HorizontalAlignment.Center)
        .SetBackColor(ColorBackground)
        .SetBorder(Stroke.Solid, Color.Gray, 3)
        .SetOutline(1)
```

Add paragraphs and content to the section.

To accurately convey the text of the bitwise operators, we used strings as an array.
```c#
    .AddText(
        new[]
        {
            "A = 0011 1100", "B = 0000 1101", "A&B = 0000 1100", "A|B = 0011 1101", "A^B = 0011 0001",
            "~A = 1100 0011"
        })
```

We have added a footer to this section to show the title of the chapter.
```c#
    AddFooters(s, " C tutorial. Chapter #2. Basic syntax. ");
```

## Third section 'Desicion making' (AddThirdSection() method)
Let's define the necessary paths to the images: imageUrlLogo, imageUrl.  
Let's create a title that will be added to the table of contents.
```c#
    .AddParagraph()
            .AddTextToParagraph("3. Desicion making")
            .SetFont(DocumentFontTitleWhite)
            .SetAlignment(HorizontalAlignment.Center)
            .SetBackColor(ColorBackground)
            .SetBorder(Stroke.Solid, Color.Gray, 3)
            .SetOutline(1)
```

Add paragraphs and content to the section.

If you need to skip line breaks in the text, you can specify the ignoreNewLineSymbol parameter:
```c#
    .AddTextToParagraph(@"Decision-making structures require that the programmer specifies one or more 
conditions to be evaluated or tested by the program, along with a statement or 
statements to be executed if the condition is determined to be true, and 
optionally, other statements to be executed if the condition is determined to be 
false.", ignoreNewLineSymbol: true)
```

The **fluent style** allows us to switch after the paragraph to the section context (ToSection). After that, we add an image. 
```c#
.ToSection()
    .AddImage(imageUrl)
        .SetScale(ScalingMode.OriginalSize)
        .SetAlignment(HorizontalAlignment.Center)
        .SetBorder(Stroke.Dashed, ColorBackground, 3)
```

We have added a footer to this section to show the title of the chapter.
```c#
    AddFooters(s, " C tutorial. Chapter #3. Decision making. ");
```

## Outline section (AddOutlineSection() method)
We have added a title to the page.
```c#
    .AddParagraph()
        .AddTextToParagraph("CONTENTS")
        .SetFont(DocumentFontTitleWhite)
        .SetAlignment(HorizontalAlignment.Center)
        .SetBackColor(ColorBackground)
        .SetBorder(Stroke.Solid, Color.Gray, 3)
```
To add a table of contents, just call the appropriate method: builder.AddOutline().
You can add parameters.
```c#
    builder.AddOutline()
           .SetSpacingUnderline(Stroke.Dashed, ColorBackground)
           .SetLevelLeftIndent(10f)
           .SetFont(DocumentFontBoldOrange);
```
We have added a footer to this section to show the title.
```c#
    AddFooters(s, " C tutorial. Contents. ");
```

# Summary

Following the above steps you have learned how to make a real reference book, which is a a multi-page text document with mixed content. This article showed how to add a table of contents structure, explained how to add images to the book and how to create tables with data from an external JSON file. In addtion, creation of a key element of a comprehensive document - the repeating footer with automatic page numeration - was demonstrated.

The resulting TutorialC.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/TutorialC.pdf).