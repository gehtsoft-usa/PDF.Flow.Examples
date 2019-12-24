using Gehtsoft.PDFFlow.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Gehtsoft.PDFFlow.Contract.Model;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Document;
using Gehtsoft.PDFFlow.Models.Content;
using System.Linq;

namespace Gehtsoft.PDFFlow.Contract.ConsoleDemos
{
    internal static class ContractExample
    {
        #region Fields

        private static readonly string PdfFile;
        private static readonly string ProjectDir;
        private static readonly string ContractTextFile;
        private static readonly string LastPageTextFile;
        private static readonly string DictionaryJsonFile;
        private static readonly string DictionaryJsonContent;
        private static readonly Font DocumentFont;
        private static readonly Font ItalicFont;
        private static readonly Font BoldFont;
        private static readonly Font TitleFont;
        public static List<string> ContractContent { get; set; }
        public static List<string> LastPageContent { get; set; }
        public static List<ContractDictionary> DictionaryData { get; }

        #endregion Fields

        #region Constructors

        static ContractExample()
        {
            PdfFile = Path.Combine(Environment.CurrentDirectory, "Contract.pdf");
            ProjectDir = Directory.GetCurrentDirectory();
            ContractTextFile = Path.Combine(ProjectDir, "Content", "contract.txt");
            LastPageTextFile = Path.Combine(ProjectDir, "Content", "lastpage.txt");
            ContractContent = File.ReadAllLines(ContractTextFile).ToList();
            LastPageContent = File.ReadAllLines(LastPageTextFile).ToList();
            DictionaryJsonFile = Path.Combine(ProjectDir, "Content", "contract_dictionary.json");
            DictionaryJsonContent = File.ReadAllText(DictionaryJsonFile);
            DictionaryData = JsonConvert.DeserializeObject<List<ContractDictionary>>(DictionaryJsonContent);
            DocumentFont = new Font
            {
                Name = FontNames.Helvetica,
                Size = 16f,
                Color = Color.Black
            };
            ItalicFont = DocumentFont.Clone(); ItalicFont.Name = "Times-Italic";
            BoldFont = DocumentFont.Clone(); BoldFont.Name = "Times-Bold";
            TitleFont = BoldFont.Clone(); TitleFont.Size = 28f;
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
                .AddContractSection()
                .Build(PdfFile);
        }

        internal static DocumentBuilder AddContractSection(this DocumentBuilder builder)
        {
            builder.AddDefaultSection(s =>
            {
                s.SetSize(PaperSize.Letter);
                s.SetOrientation(PageOrientation.Portrait);
                s.SetMargins(new Box(40, 80, 40, 40));
                AddContractTitle(s);
                ReplaceKeyWordsFromDictionary();
                AddContractText(s);
                AddFooter(s);
            });
            builder.AddDefaultSection(s =>
            {
                s.SetSize(PaperSize.Letter);
                s.SetOrientation(PageOrientation.Portrait);
                s.SetMargins(new Box(40, 80, 40, 40));
                AddSignatures(s);
                AddLastFooter(s);
            });
            return builder;
        }

        internal static void AddFooter(Section s)
        {
            s.Layout.AddSinglePage().AddRepeatingArea(s.Page, 45, true, areaConfig: (area) =>
            {
                area.AddItem<Paragraph>(p =>
                {
                    p.SetMargins(20, 0, 0, 0).AddPageNumber().AddTabSymbol().AddTabSymbol().AddTabulationInPercent(60, TabulationType.Left).AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
                });
                area.AddItem<Paragraph>(p =>
                {
                    p.AddTabSymbol().AddTextToParagraph("(Initials)").AddTabulationInPercent(80, TabulationType.Center);
                });
            });
        }

        internal static void AddLastFooter(Section s)
        {
            s.Layout.AddSinglePage().AddRepeatingArea(s.Page, 45, true, areaConfig: (area) =>
            {
                area.AddItem<Paragraph>(p =>
                {
                    p.SetMargins(20, 0, 0, 0).AddPageNumber();
                });
            });
        }

        internal static void AddContractTitle(Section s)
        {
            s.AddParagraph("House Rental Contract").SetAlignment(HorizontalAlignment.Center).SetMargins(0, 0, 0, 20).SetFont(TitleFont);
        }

        internal static void AddContractText(Section s)
        {
            foreach (var paragraph in ContractContent)
                s.AddParagraph(paragraph).SetFont(DocumentFont).SetJustifyAlignment(true);
        }

        internal static void AddSignatures(Section s)
        {
            foreach (var paragraph in LastPageContent)
                s.AddParagraph(paragraph).SetFont(DocumentFont).SetJustifyAlignment(true);

            s.AddParagraph()
                .SetMargins(30, 0, 0, 0)
                .AddTabSymbol().AddTabulationInPercent(20, TabulationType.Right, TabulationLeading.BottomLine)
                .AddTabSymbol().AddTabSymbol().AddTabulationInPercent(60, TabulationType.Left).AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
            s.AddParagraph()
                .AddTabSymbol().AddTextToParagraph("(Date)").AddTabulationInPercent(10, TabulationType.Center)
                .AddTabSymbol().AddTextToParagraph("(Renter)").AddTabulationInPercent(80, TabulationType.Center);
            s.AddParagraph(" ");
            s.AddParagraph()
                .AddTabSymbol().AddTabulationInPercent(20, TabulationType.Right, TabulationLeading.BottomLine)
                .AddTabSymbol().AddTabSymbol().AddTabulationInPercent(60, TabulationType.Left).AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
            s.AddParagraph()
                .AddTabSymbol().AddTextToParagraph("(Date)").AddTabulationInPercent(10, TabulationType.Center)
                .AddTabSymbol().AddTextToParagraph("(Landlord)").AddTabulationInPercent(80, TabulationType.Center);
        }

        internal static void ReplaceKeyWordsFromDictionary()
        {
            ContractContent = ContractContent.ConvertAll(s => s.Replace("{paragraphbreak}", " "));
            foreach (ContractDictionary item in DictionaryData)
            {
                ContractContent = ContractContent.ConvertAll(s => s.Replace("{" + item.Key + "}", item.Value));
            }
        }

        #endregion Methods
    }
}