using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Utils;
using Receipt.Model;

namespace Receipt
{
    public static class ReceiptRunner
    {
        private static readonly string ProjectDir;
        private static readonly string ReceiptJsonFile;
        private static readonly string ReceiptJsonContent;
        private static readonly string ImageUrl;
        public static List<ReceiptData> ReceiptData { get; }
        private static readonly string[] ReceiptText;
        private static readonly FontBuilder DocumentFont;
        private static readonly FontBuilder ItalicFont;
        private static readonly FontBuilder FooterFont;
        private static readonly FontBuilder BoldFont;
        private static readonly FontBuilder TitleFont;
        private static readonly FontBuilder HiddenFont;

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

        public static DocumentBuilder Run()
        {
            return DocumentBuilder
                .New()
                .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
                .AddReceiptSection();
        }

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

        private static SectionBuilder SetSectionSettings(this SectionBuilder s)
        {
            s.SetMargins(30).SetSize(PaperSize.A4).SetOrientation(PageOrientation.Portrait).SetNumerationStyle();
            return s;
        }

        private static SectionBuilder AddReceiptTitle(this SectionBuilder s)
        {
            s.AddImage(ImageUrl).SetScale(ScalingMode.OriginalSize).SetAlignment(HorizontalAlignment.Center);
            s.AddParagraph("Receipt").SetMargins(0, 20, 0, 10).SetFont(TitleFont).SetAlignment(HorizontalAlignment.Center);
            s.AddLine().SetColor(Color.FromRgba(106.0 / 255.0, 85.0 / 255.9, 189.0 / 255.0)).SetStroke(Stroke.Solid).SetWidth(2);
            return s;
        }

        private static SectionBuilder AddReceiptText(this SectionBuilder s)
        {
            ParagraphBuilder p = s.AddParagraph()
                .SetMargins(0, 20, 0, 10)
                .SetFont(DocumentFont);
            p.AddText(ReceiptText);
            p.AddUrl("support@yourhomeprovider.com");
            return s;
        }

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
                .AddTextToParagraph("200 John Doe Street")
                .AddTabSymbol()
                .AddUrlToParagraph("https://yourhomeprovider.com")
                .AddTabulation(280);
            footer
                .AddParagraph()
                .SetMargins(0, 0, 0, 0)
                .SetFont(FooterFont)
                .AddText(new[] { "Santa Barbara, CA 000001", "United States" });
            footer
                .AddParagraph()
                .SetMargins(0, 0, 0, 0)
                .SetFont(FooterFont)
                .AddText("*VAT / GST paid directly by Your Home Provider, Inc., where applicable");
            
            return s;
        }
    }
}
