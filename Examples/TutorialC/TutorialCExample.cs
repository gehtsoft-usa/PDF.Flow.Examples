using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Gehtsoft.PDFFlow.Utils;


namespace TutorialC
{
    public static class TutorialCRunner
    {
        public class Keywords
        {
            [DataMember] public string Id { get; set; }
            [DataMember] public string Keyword { get; set; }
            [DataMember] public string Description { get; set; }

            public override string ToString() => $"{Id}\n{Keyword}\n{Description}";
        }

        public static List<Keywords> KeywordsList { get; }

        private static readonly string ProjectDir;
        private static readonly string ImageUrl;
        private static readonly string KeywordsJsonFile;
        private static readonly string KeywordsJsonContent;
        private static readonly FontBuilder DocumentFont;
        private static readonly FontBuilder DocumentFontBrown;
        private static readonly FontBuilder DocumentFontBold;
        private static readonly FontBuilder DocumentFontBoldLarge;
        private static readonly FontBuilder DocumentFontBoldOrange;
        private static readonly FontBuilder ItalicFont;
        private static readonly FontBuilder ItalicBrownFont;
        private static readonly StyleBuilder CodeExamplesStyle;
        private static readonly FontBuilder SmallFont;
        private static readonly FontBuilder DocumentFontRedUnderline;
        private static readonly FontBuilder DocumentFontTitleWhite;
        private static readonly FontBuilder HeaderFontBoldLarge;
        private static readonly Color ColorBackground;
        private static readonly Box PageMargins;

        static TutorialCRunner()
        {
            ProjectDir = Directory.GetCurrentDirectory();
            KeywordsJsonFile = Path.Combine(ProjectDir, "Content", "tutorialc_keywords.json");
            KeywordsJsonContent = File.ReadAllText(KeywordsJsonFile);
            KeywordsList = JsonConvert.DeserializeObject<List<Keywords>>(KeywordsJsonContent);
            ImageUrl = Path.Combine(ProjectDir, "images", "customers");
            FontBuilder documentFont() => Fonts.Times(12f);
            FontBuilder italicFont() => documentFont().SetName("Times-Italic");
            FontBuilder boldFont() => documentFont().SetName("Times-Bold");
            DocumentFont = documentFont();
            DocumentFontBold = boldFont();
            ItalicFont = italicFont();
            ItalicBrownFont = italicFont().SetColor(Color.FromRgba(0.4, 0.3, 0.0));
            CodeExamplesStyle = StyleBuilder.New()
                .SetFont(Fonts.Courier(12f))
                .SetBackColor(Color.FromHtml("#F0F0F0"))
                .SetMarginTop(14f).SetMarginBottom(14f);
            ColorBackground = Color.FromRgba(201.0 / 255, 107.0 / 255, 20.0 / 255);
            DocumentFontBoldOrange = boldFont().SetColor(ColorBackground);
            SmallFont = documentFont().SetSize(10f);
            DocumentFontBrown = documentFont().SetColor(Color.FromRgba(0.4, 0.3, 0.0));
            DocumentFontRedUnderline = boldFont()
                .SetColor(Color.FromRgba(0.4, 0.3, 0.0))
                .SetUnderlineStroke(Stroke.Solid)
                .SetUnderlineColor(Color.FromRgba(201.0 / 255, 107.0 / 255, 20.0 / 255));
            DocumentFontTitleWhite = documentFont().SetSize(16f).SetColor(Color.White);
            DocumentFontBoldLarge = boldFont().SetSize(16f);
            HeaderFontBoldLarge = Fonts.Helvetica(15f).SetBold();
            PageMargins = new Box(40);
        }

        public static DocumentBuilder Run()
        {
            var documentStyle = StyleBuilder.New().SetLineSpacing(1.2f);
            return DocumentBuilder
                .New().ApplyStyle(documentStyle)
                .AddBookCoverSection()
                .AddFirstSection()
                .AddSecondSection()
                .AddThirdSection()
                .AddOutlineSection();
        }

        // Outline
        internal static DocumentBuilder AddOutlineSection(this DocumentBuilder builder)
        {
            builder.AddOutline("CONTENTS", 
                10f, 
                DocumentFontBoldOrange, DocumentFontTitleWhite,
                PaperSize.Letter, PageOrientation.Portrait,
                PageMargins.Left, PageMargins.Top, PageMargins.Right, PageMargins.Bottom)
                .SetCaptionBackColor(ColorBackground)
                .SetSpacingUnderline(Stroke.Dashed, ColorBackground);
            return builder;
        }

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
                            .SetUnderlineColor(ColorBackground);
            section.AddFooterToBothPages(15f)
                .AddParagraph()
                    .AddPageNumberToParagraph(" Page #", 1, SmallFont)
                    .SetAlignment(HorizontalAlignment.Right);
        }

        internal static DocumentBuilder AddBookCoverSection(this DocumentBuilder builder)
        {
            var imageUrl = Path.Combine(ProjectDir, "images", "DocumentExample", "LearnCCoverPage.jpg");
            var fontHidden = Fonts.Courier(.01f).SetColor(Color.White);
            builder
                .AddSection()
                    .SetSize(PaperSize.A4)
                    .SetOrientation(PageOrientation.Portrait)
                    .SetMargins(PageMargins)
                    .AddParagraph("LEARN C PROGRAMMING")
                        .SetFont(fontHidden)
                        .SetBackColor(Color.White).SetOutline().SetOutline(0)
                .ToSection()
                    .AddImage(imageUrl).SetScale(ScalingMode.Stretch);
            return builder;
        }

        internal static DocumentBuilder AddThirdSection(this DocumentBuilder builder)
        {
            var imageUrlLogo = Path.Combine(ProjectDir, "images", "DocumentExample", "LearnCLogo30_30.jpg");
            var imageUrl = Path.Combine(ProjectDir, "images", "DocumentExample", "DemoImg2.jpg");
            var imageExclamationMark =
                Path.Combine(ProjectDir, "images", "DocumentExample", "ExclamationMark_45_45.jpg");
            var imageQuestionMark = Path.Combine(ProjectDir, "images", "DocumentExample", "QuestionMark_45_45.jpg");
            var s = builder.AddSection().SetOrientation(PageOrientation.Portrait)
                .SetMargins(PageMargins)
                .AddParagraph()
                    .AddTextToParagraph("3. Decision making")
                    .SetFont(DocumentFontTitleWhite)
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetBackColor(ColorBackground)
                    .SetBorder(Stroke.Solid, Color.Gray, 3)
                    .SetOutline(1)
            .ToSection()
                .AddParagraph()
                    .AddTextToParagraph("Typical structure")
                    .SetFont(HeaderFontBoldLarge)
                    .SetMargins(0, 10, 0, 0)
                    .SetOutline(2)
            .ToSection()
                .AddLine()
                    .SetColor(Color.Black).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToSection()
                .AddParagraph()
                    .AddInlineImageToParagraph(imageQuestionMark, new XSize(15, 15), ScalingMode.UserDefined)
                    .SetFirstLineIndent(20f)
                    .AddTextToParagraph(@"Decision-making structures require that the programmer specifies one or more 
conditions to be evaluated or tested by the program, along with a statement or 
statements to be executed if the condition is determined to be true, and 
optionally, other statements to be executed if the condition is determined to be 
false.", ignoreNewLineSymbol: true)
                    .SetFont(DocumentFont)
                    .SetMargins(0, 2, 0, 0)
            .ToSection()
                .AddParagraph()
                    .SetFirstLineIndent(20f)
                    .AddTextToParagraph(@"Shown below is the general form of a typical decision-making structure found in 
most of the programming languages:", ignoreNewLineSymbol: true)
                    .SetFont(DocumentFont)
                    .SetMargins(0, 2, 0, 5)
            .ToSection()
                .AddImage(imageUrl)
                    .SetScale(ScalingMode.OriginalSize)
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetBorder(Stroke.Dashed, ColorBackground, 3)
            .ToSection();
            AddFooters(s, " C tutorial. Chapter #3. Decision making. ");
            return builder;
        }

        internal static DocumentBuilder AddFirstSection(this DocumentBuilder builder)
        {
            var imageUrlLogo = Path.Combine(ProjectDir, "images", "DocumentExample", "LearnCLogo30_30.jpg");
            var imageUrl = Path.Combine(ProjectDir, "images", "DocumentExample", "DemoImg1.jpg");
            var imageExclamationMark =
                Path.Combine(ProjectDir, "images", "DocumentExample", "ExclamationMark_45_45.jpg");
            var imageQuestionMark = Path.Combine(ProjectDir, "images", "DocumentExample", "QuestionMark_45_45.jpg");

            var s = builder.AddSection()
            .SetSize(PaperSize.A4).SetOrientation(PageOrientation.Portrait)
            .SetMargins(PageMargins)
                .AddParagraph()
                    .SetFont(DocumentFontTitleWhite)
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetBackColor(ColorBackground)
                    .SetBorder(Stroke.Solid, Color.Gray, 3)
                    .SetOutline(1)
                    .AddText("1. Environment setup")
            .ToSection()
                .AddParagraph()
                    .SetFont(HeaderFontBoldLarge)
                    .SetMargins(0, 10, 0, 0)
                    .SetOutline(2)
                    .AddText("Try it Online")
            .ToSection()
                .AddLine()
                    .SetColor(ColorBackground).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToSection()
                .AddParagraph()
                    .SetFont(DocumentFont)
                    .SetMargins(0, 2, 0, 0)
                    .AddInlineImageToParagraph(imageExclamationMark, new XSize(15, 15), ScalingMode.UserDefined)
                    .AddTextToParagraph("You really ")
                    .AddTextToParagraph("do not need", DocumentFontRedUnderline)
                    .AddText(@" to set up your own environment to start learning C 
programming language. Reason is very simple, we already have set up C 
Programming environment online, so that you can compile and execute all the 
available examples online at the same time when you are doing your theory 
work. This gives you confidence in what you are reading and to check the result 
with different options. Feel free to modify any example and execute it online.",
                            ignoreNewLineSymbol: true)
            .ToSection()
                .AddParagraph()
                    .SetFont(DocumentFont)
                    .SetMargins(0, 0, 0, 5)
                    .AddTextToParagraph("Try following example using our online compiler option available at ")
                    .AddUrlToParagraph("http://www.compileonline.com/")
                    .AddText(".")
            .ToSection()
                .AddParagraph().ApplyStyle(CodeExamplesStyle)
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
            .ToSection()
                .AddParagraph()
                    .AddTextToParagraph("* For most of the examples given in this tutorial, you will find the ", DocumentFontBrown)
                    .AddTextToParagraph("Try it", DocumentFontRedUnderline)
                    .AddTextToParagraph(@" option in 
our website code sections at the top right corner that will take you to the online 
compiler. So just make use of it and enjoy your learning.", DocumentFontBrown,
                                        ignoreNewLineSymbol: true)
            .ToSection()
                .AddImage(imageUrl)
                    .SetPaddings(5)
                    .SetScale(ScalingMode.OriginalSize)
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetBorder(Stroke.Double, ColorBackground, 3)
            .ToSection()
                .AddParagraph("Local Environment Setup")
                    .SetFont(HeaderFontBoldLarge)
                    .SetMargins(0, 10, 0, 0)
                    .SetOutline(2)
            .ToSection()
                .AddLine()
                    .SetColor(ColorBackground).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToSection()
                .AddParagraph()
                    .AddInlineImageToParagraph(imageQuestionMark, new XSize(15, 15), ScalingMode.UserDefined)
                    .AddTextToParagraph(@"If you want to set up your environment for C programming language, you need 
the following two software tools available on your computer: Text Editor and The C Compiler.", ignoreNewLineSymbol: true)
                    .SetFont(DocumentFont)
                    .SetMargins(0, 10, 0, 0)
            .ToSection()
                .AddParagraph("Installation on UNIX/Linux")
                    .SetFont(HeaderFontBoldLarge).SetMargins(0, 10, 0, 0)
                    .SetOutline(2)
            .ToSection()
                .AddLine()
                    .SetColor(ColorBackground).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToSection()
                .AddParagraph().SetMargins(0, 5, 0, 0)
                    .AddInlineImageToParagraph(imageQuestionMark,
                        new XSize(15, 15), ScalingMode.UserDefined)
                    .AddTextToParagraph("If you are using ", DocumentFont)
                    .AddTextToParagraph("Linux or UNIX", DocumentFontBold)
                    .AddTextToParagraph(@", then check whether GCC is installed on your
system by entering the following command from the command line:", DocumentFont, ignoreNewLineSymbol: true)
            .ToSection()
                .AddParagraph().ApplyStyle(CodeExamplesStyle)
                    .AddText("$ gcc -v")
            .ToSection()
                .AddParagraph()
                    .AddInlineImageToParagraph(imageQuestionMark,
                        new XSize(15, 15), ScalingMode.UserDefined)
                    .AddTextToParagraph(@"If you have GNU compiler installed on your machine, then it should print a
message as follows:", ignoreNewLineSymbol: true)
                    .SetFont(DocumentFont)
                    .SetMargins(0, 10, 0, 0)
            .ToSection()
                .AddParagraph().ApplyStyle(CodeExamplesStyle)
                    .AddText(
                        new[]
                        {
                            "", "Using built-in specs.", "Target: i386-redhat-linux",
                            "Configured with: ../configure --prefix=/usr .......", "Thread model: posix",
                            "gcc version 4.1.2 20080704 (Red Hat 4.1.2-46)"
                        })
            .ToSection()
                .AddParagraph()
                    .AddInlineImageToParagraph(imageQuestionMark,
                        new XSize(15, 15), ScalingMode.UserDefined)
                    .SetMargins(0, 5, 0, 0)
                    .AddTextToParagraph(@"If GCC is not installed, then you will have to install it yourself using the detailed instructions available at ", DocumentFont, ignoreNewLineSymbol: true)
                    .AddUrlToParagraph("http://gcc.gnu.org/install/",
                                       "gcc.gnu.org/install/")
                    .AddTextToParagraph(".", DocumentFont)
            .ToSection()
                .AddParagraph("Installation on Mac OS")
                    .SetFont(HeaderFontBoldLarge)
                    .SetMargins(0, 10, 0, 0)
                    .SetOutline(2)
            .ToSection()
                .AddLine()
                    .SetColor(ColorBackground).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToSection()
                .AddParagraph()
                    .AddInlineImageToParagraph(imageQuestionMark,
                        new XSize(15, 15), ScalingMode.UserDefined)
                    .AddTextToParagraph(@"If you use Mac OS X, the easiest way to obtain GCC is to download the Xcode
development environment from Apple's web site and follow the simple
installation instructions. Once you have Xcode setup, you will be able to use GNU
compiler for C/C++.", ignoreNewLineSymbol: true)
                    .SetFont(DocumentFont)
                    .SetMargins(0, 5, 0, 0)
            .ToSection()
                .AddParagraph()
                    .AddTextToParagraph("Installation on Windows")
                    .SetFont(HeaderFontBoldLarge)
                    .SetMargins(0, 10, 0, 0)
                    .SetOutline(2)
            .ToSection()
                .AddLine()
                    .SetColor(ColorBackground).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToSection()
                .AddParagraph()
                    .AddInlineImageToParagraph(imageQuestionMark, new XSize(15, 15), ScalingMode.UserDefined)
                    .AddTextToParagraph(@"To install GCC on Windows, you need to install MinGW. To install MinGW, go to
the MinGW homepage, www.mingw.org, and follow the link to the MinGW 
download page. Download the latest version of the MinGW installation program, 
which should be named MinGW-<version>.exe. 
While installing MinGW, at a minimum, you must install gcc-core, gcc-g++, 
binutils, and the MinGW runtime, but you may wish to install more. 
Add the bin subdirectory of your MinGW installation to your PATH environment 
variable, so that you can specify these tools on the command line by their simple 
names. 
After the installation is complete, you will be able to run gcc, g++, ar, ranlib, 
dlltool, and several other GNU tools from the Windows command line.", ignoreNewLineSymbol: true)
                    .SetFont(DocumentFont)
                    .SetMargins(0, 5, 0, 0)
            .ToSection();
            AddFooters(s, " C tutorial. Chapter #1. Environment setup. ");

            return builder;
        }

        internal static DocumentBuilder AddSecondSection(this DocumentBuilder builder)
        {
            var imagesDir = Directory.GetCurrentDirectory();
            var imageUrlLogo = Path.Combine(imagesDir, "images", "DocumentExample", "LearnCLogo30_30.jpg");
            var imageExclamationMark =
                Path.Combine(ProjectDir, "images", "DocumentExample", "ExclamationMark_45_45.jpg");
            var s = builder.AddSection()
                .SetMargins(PageMargins)
                .SetSize(PaperSize.A4)
                .SetOrientation(PageOrientation.Portrait)
                .SetNumerationStyle(NumerationStyle.Arabic)
                .AddParagraph("2. Basic syntax").SetFont(DocumentFontTitleWhite)
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetBackColor(ColorBackground)
                    .SetBorder(Stroke.Solid, Color.Gray, 3)
                    .SetOutline(1)
            .ToSection()
                .AddParagraph("Identifiers")
                    .SetFont(HeaderFontBoldLarge).SetMargins(0, 10, 0, 0)
                    .SetOutline(2)
            .ToSection()
                .AddLine()
                    .SetColor(ColorBackground).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToSection()
                .AddParagraph().SetMargins(0, 5, 0, 5)
                    .AddInlineImageToParagraph(imageExclamationMark, 
                        new XSize(15, 15), ScalingMode.UserDefined)
                    .AddText(@"A C identifier is a name used to identify a variable, function, or any other user-
defined item. An identifier starts with a letter A to Z, a to z, or an underscore ‘_’ 
followed by zero or more letters, underscores, and digits (0 to 9).", ignoreNewLineSymbol: true)
                        .SetFont(DocumentFont)
            .ToSection()
                .AddParagraph().SetMargins(0, 5, 0, 5)
                    .AddInlineImageToParagraph(imageExclamationMark, new XSize(15, 15), ScalingMode.UserDefined)
                    .AddTextToParagraph("C does not allow punctuation characters such as @, $, and % within identifiers. C is a ", DocumentFont)
                    .AddTextToParagraph("case-sensitive", element => element.SetFont(DocumentFontBold).SetBackColor(Color.Yellow))
                    .AddTextToParagraph(" programming language. Thus, ", DocumentFont)
                    .AddTextToParagraph("Customer", ItalicFont)
                    .AddTextToParagraph(" and ", DocumentFont)
                    .AddTextToParagraph("customer", ItalicFont)
                    .AddText(" are two different identifiers in C. Here are some examples of acceptable identifiers:")
                        .SetFont(DocumentFont)
            .ToSection()
                .AddParagraph().ApplyStyle(CodeExamplesStyle)
                    .SetAlignment(HorizontalAlignment.Center)
                    .AddText(new[] { "Zara", "abc", "move_name", "a_123", "myname50", "_temp", "j", "retVal" })
            .ToSection()
                .AddParagraph("Keywords")
                    .SetFont(HeaderFontBoldLarge)
                    .SetMargins(0, 15, 0, 0)
                    .SetOutline()
                    .SetOutline(2)
            .ToSection()
                .AddLine()
                    .SetColor(ColorBackground).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToSection()
                .AddParagraph().SetMargins(0, 5, 0, 5)
                    .AddInlineImageToParagraph(imageExclamationMark, 
                        new XSize(15, 15), ScalingMode.UserDefined)
                    .AddTextToParagraph("The following list shows the reserved words in C. These reserved words ", DocumentFont)
                    .AddTextToParagraph("may not be used", element => element.SetFont(DocumentFontBold).SetBackColor(Color.Yellow))
                    .AddText(" as constants or variables or any other identifier names. While naming your functions and variables, other than these names, you can choose any names of reasonable length for variables, functions etc.")
                        .SetFont(DocumentFont)
            .ToSection()
                .AddTable()
                    .SetRepeatHeaders(true)
                    .SetContentRowStyleMinHeight(15)
                    .SetDataSource(KeywordsList)
                    .SetContentRowStyleFont(DocumentFont)
                    .SetContentRowStyleBackColor(
                        Color.FromRgba(193.0 / 255, 193.0 / 255, 193.0 / 255))
                    .SetHeaderRowStyleFont(
                        DocumentFontBold)
                    .SetDefaultAltRowStyle()
                    .SetAltRowStyleBackColor(
                        Color.FromRgba(255.0 / 255, 161.0 / 255, 74.0 / 255))
                    .SetAltRowStyleFont(
                        DocumentFont)
                    .SetHeaderRowStyleMinHeight(20)
            .ToSection()
                .AddParagraph("Bitwise Operators")
                    .SetFont(HeaderFontBoldLarge)
                    .SetMargins(0, 15, 0, 0)
                    .SetOutline()
                    .SetOutline(2)
            .ToSection()
                .AddLine()
                    .SetColor(ColorBackground).SetStroke(Stroke.Solid).SetWidth(1f)
            .ToSection()
                .AddParagraph()
                    .SetMargins(0, 5, 0, 0)
                    .SetFont(DocumentFont)
                    .AddInlineImageToParagraph(imageExclamationMark, 
                        new XSize(15, 15), ScalingMode.UserDefined)
                    .AddText("Bitwise operators work on bits and perform bit-by-bit operation. They are &, |, ^ and ~.")
            .ToSection()
                .AddParagraph().SetMargins(0, 5, 0, 0).SetFont(DocumentFont)
                    .AddText("Assume A = 60 and B = 13; in binary format, they will be as follows:")
            .ToSection()
                .AddParagraph().ApplyStyle(CodeExamplesStyle)
                    .AddText(
                        new[]
                        {
                            "A = 0011 1100", "B = 0000 1101", "A&B = 0000 1100", "A|B = 0011 1101", "A^B = 0011 0001",
                            "~A = 1100 0011"
                        })
            .ToSection();
                // Repeating area footers:
            AddFooters(s, " C tutorial. Chapter #2. Basic syntax. ");

            // end of AddSectionToDocument
            return builder;
        }
    }
}
