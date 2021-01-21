using Gehtsoft.PDFFlow.Builder;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using System.Linq;
using Gehtsoft.PDFFlow.Utils;
using HouseRentalContract.Model;

namespace HouseRentalContract
{
    public static class HouseRentalContractBuilder
    {
        #region Fields

        private static readonly string PdfFile;
        private static readonly string ProjectDir;
        private static readonly string ContractTextFile;
        private static readonly string LastPageTextFile;
        private static readonly string DictionaryJsonFile;
        private static readonly string DictionaryJsonContent;
        private static readonly FontBuilder DocumentFont;
        private static readonly FontBuilder TitleFont;
        public static List<string> ContractContent { get; set; }
        public static List<string> LastPageContent { get; set; }
        public static List<ContractDictionary> DictionaryData { get; }

        #endregion Fields

        #region Constructors

        static HouseRentalContractBuilder()
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
            DocumentFont = Fonts.Helvetica(16f);
            TitleFont = Fonts.Times(16f).SetName("Times-Bold").SetSize(28f);
        }

        #endregion Constructors

        #region Methods

        public static DocumentBuilder AddHouseRentalContract(this DocumentBuilder builder)
        {
            builder.AddSection()
                .SetSize(PaperSize.Letter)
                .SetOrientation(PageOrientation.Portrait)
                .SetMargins(new Box(40, 80, 40, 40))
                .AddContractTitle()
                .ReplaceKeyWordsFromDictionary()
                .AddContractText()
                .AddFooter();

            builder.AddSection()
                .SetSize(PaperSize.Letter)
                .SetOrientation(PageOrientation.Portrait)
                .SetMargins(new Box(40, 80, 40, 40))
                .AddSignatures()
                .AddLastFooter();
            return builder;
        }

        internal static SectionBuilder AddFooter(this SectionBuilder s)
        {
            s.AddFooterToBothPages(50)
                .AddParagraph()
                    .SetMargins(0, 18, 0, 0)
                    .AddPageNumberToParagraph()
                    .AddTabSymbol()
                    .AddTabSymbol()
                    .AddTabulationInPercent(60, TabulationType.Left)
                    .AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine)
            .ToArea()
                .AddParagraph()
                    .AddTabSymbol()
                    .AddTextToParagraph("(Initials)")
                    .AddTabulationInPercent(80, TabulationType.Center);
            return s;
        }

        internal static SectionBuilder AddLastFooter(this SectionBuilder s)
        {
            s.AddFooterToBothPages(45)
                .AddParagraph()
                .AddPageNumber();
            return s;
        }

        internal static SectionBuilder AddContractTitle(this SectionBuilder s)
        {
            s.AddParagraph("House Rental Contract")
                .SetAlignment(HorizontalAlignment.Center)
                .SetMargins(0, 0, 0, 20)
                .SetFont(TitleFont);
            return s;
        }

        internal static SectionBuilder AddContractText(this SectionBuilder s)
        {
            foreach (var paragraph in ContractContent)
                s.AddParagraph(paragraph).SetFont(DocumentFont).SetJustifyAlignment(true);
            return s;
        }

        internal static SectionBuilder AddSignatures(this SectionBuilder s)
        {
            foreach (var paragraph in LastPageContent)
                s.AddParagraph(paragraph).SetFont(DocumentFont).SetJustifyAlignment(true);

            s.AddParagraph()
                .SetMargins(0, 40, 0, 0)
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

            return s;
        }

        internal static SectionBuilder ReplaceKeyWordsFromDictionary(this SectionBuilder s)
        {
            ContractContent = ContractContent.ConvertAll(c => c.Replace("{paragraphbreak}", " "));
            foreach (ContractDictionary item in DictionaryData)
            {
                ContractContent = ContractContent.ConvertAll(c => c.Replace("{" + item.Key + "}", item.Value));
            }
            return s;
        }

        #endregion Methods
    }
}