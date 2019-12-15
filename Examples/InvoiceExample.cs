using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Gehtsoft.PDFFlow.Invoice.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Content;
using Gehtsoft.PDFFlow.Models.Document;

namespace Gehtsoft.PDFFlow.Invoice
{
    internal static class InvoiceExample
    {
        #region Fields

        private static readonly string PdfFile;
        private static readonly string ProjectDir;
        private static readonly string InvoiceJsonFile;
        private static readonly string InvoiceJsonContent;
        private static readonly string ImageUrl;
        public static List<InvoiceData> InvoiceData { get; }
        private static readonly string[] InvoiceText;
        private static readonly Font DocumentFont;
        private static readonly Font ItalicFont;
        private static readonly Font FooterFont;
        private static readonly Font BoldFont;
        private static readonly Font UrlFont;
        private static readonly Font TitleFont;
        private static readonly Font HiddenFont;

        #endregion Fields

        #region Constructors

        static InvoiceExample()
        {
            PdfFile = Path.Combine(Environment.CurrentDirectory, "Invoice.pdf");
            ProjectDir = Directory.GetCurrentDirectory();
            InvoiceJsonFile = Path.Combine(ProjectDir, "Content", "invoice.json");
            InvoiceJsonContent = File.ReadAllText(InvoiceJsonFile);
            InvoiceData = JsonConvert.DeserializeObject<List<InvoiceData>>(InvoiceJsonContent);
            ImageUrl = Path.Combine(ProjectDir, "images", "InvoiceExample", "invoice.jpg");
            DocumentFont = new Font { Name = FontNames.Courier, Size = 14f, Color = Color.Black };
            ItalicFont = DocumentFont.Clone(); ItalicFont.Name = "Times-Italic";
            FooterFont = DocumentFont.Clone(); FooterFont.Color = Color.FromRgba(106.0 / 255.0, 85.0 / 255.9, 189.0 / 255.0); FooterFont.Size = 12f;
            BoldFont = DocumentFont.Clone(); BoldFont.Name = "Courier-Bold";
            TitleFont = BoldFont.Clone(); TitleFont.Size = 28f;
            HiddenFont = DocumentFont.Clone(); HiddenFont.Color = Color.White; HiddenFont.Size = 0.01f;
            UrlFont = DocumentFont.Clone(); UrlFont.Color = Color.Blue; UrlFont.Underline = Stroke.Solid; UrlFont.UnderlineColor = Color.Blue;
            InvoiceText = new[]
                {
                    "We received payment for your subscription.",
                    "Thanks for staying with us!",
                    "Questions? Please contact "
                };
        }

        #endregion Constructors

        #region Methods

        internal static void GenerateExample()
        {
            Console.WriteLine("Generating file " + PdfFile);
            if (File.Exists(PdfFile))
                File.Delete(PdfFile);
            DocumentBuilder
                .New()
                .AddInvoiceSection()
                .Build(PdfFile);
        }

        internal static DocumentBuilder AddInvoiceSection(this DocumentBuilder builder)
        {
            builder.AddSection(s =>
            {
                SetSectionSettings(s);

                AddInvoiceTitle(s);

                AddInvoiceText(s);

                AddInvoiceTable(s);

                AddFooter(s);
            });
            return builder;
        }

        internal static void SetSectionSettings(Section s)
        {
            s.SetMargins(30).SetSize(PaperSize.A4).SetOrientation(PageOrientation.Portrait).SetNumerationStyle(NumerationStyle.Arabic);
        }

        internal static void AddInvoiceTitle(Section s)
        {
            s.AddImage(ImageUrl).SetScale(ScalingMode.AutoSize).SetAlignment(HorizontalAlignment.Center);
            s.AddParagraph("Receipt").SetMargins(20, 0, 0, 10).SetFont(TitleFont).SetAlignment(HorizontalAlignment.Center);
            s.AddLine().SetColor(Color.FromRgba(106.0 / 255.0, 85.0 / 255.9, 189.0 / 255.0)).SetStyle(Stroke.Solid).SetWidth(2);
        }

        internal static void AddInvoiceText(Section s)
        {
            Paragraph p = s.AddParagraph().SetMargins(20, 0, 0, 10).SetFont(DocumentFont);
            p.AddText(InvoiceText);
            p.AddUrl("support@yourhomeprovider.com");
        }

        internal static void AddInvoiceTable(Section s)
        {
            Table t = s.AddTable();
            t.HeaderRowStyle.SetBorderStyle(Stroke.None).SetFont(HiddenFont).Border.SetWidth(0.0f);
            t.ContentRowStyle.SetBorderStyle(Stroke.None).SetFont(ItalicFont).Border.SetWidth(0.0f);
            t.SetDataSource(InvoiceData);
        }

        internal static void AddFooter(Section s)
        {
            s.Layout.AddSinglePage().AddRepeatingArea(s.Page, 160, true, area =>
            {
                area.AddLine().SetColor(Color.FromRgba(106.0 / 255.0, 85.0 / 255.9, 189.0 / 255.0)).SetStyle(Stroke.Solid).SetWidth(2);                
                area.AddItem<Paragraph>(p =>
                {                    
                    p.SetMargins(20, 0, 0, 10).AddText("Your Home Provider").SetFont(BoldFont);                   
                });
                area.AddItem<Paragraph>(p =>
                {                    
                    p.SetMargins(0, 0, 0, 0).SetFont(FooterFont).AddTextToParagraph("Your Home Provider, Inc.")
                        .AddTabSymbol().AddUrlToParagraph("support@yourhomeprovider.com").AddTabulation(280, TabulationType.Left);
                });
                area.AddItem<Paragraph>(p =>
                {
                    p.SetMargins(0, 0, 0, 0).SetFont(FooterFont).AddTextToParagraph("666 John Doe Street")
                        .AddTabSymbol().AddUrlToParagraph("https://yourhomeprovider.com").AddTabulation(280, TabulationType.Left);
                });
                area.AddItem<Paragraph>(p =>
                {
                    p.SetMargins(0, 0, 0, 0).SetFont(FooterFont).AddText(new[] { "Santa Barbara, CA 99999", "United States" });                    
                });

                area.AddItem<Paragraph>(p =>
                {
                    p.SetMargins(0, 0, 0, 0).SetFont(FooterFont).AddText("*VAT / GST paid directly by Your Home Provider, Inc., where applicable");                    
                });

            });
        }

        #endregion Methods
    }
}
