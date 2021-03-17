using System.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using PaymentAgreement.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;
using System.Text.RegularExpressions;
using System.Text;
namespace PaymentAgreement
{
    internal class PaymentAgreementBuilder
    {

        internal static readonly CultureInfo DocumentLocale
            = new CultureInfo("en-US");
        internal const PageOrientation Orientation
            = PageOrientation.Portrait;
        internal static readonly Box Margins = new Box(29, 20, 29, 20);
        internal static readonly XUnit PageWidth =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));

        internal static readonly FontBuilder FNT9 =
            Fonts.Times(9f);
        internal static readonly FontBuilder FNT11 =
            Fonts.Times(11f);
        internal static readonly FontBuilder FNT11B =
            Fonts.Times(11f).SetBold();
        internal static readonly FontBuilder FNT20B =
            Fonts.Times(20f).SetBold();
        
        internal const string MAIN_TITLE = "PAYMENT AGREEMENT";

        internal Dictionary<string, string> dict;

        private List<PartyData> partyData;
        private PartyData debtor = new PartyData();
        private PartyData cosigner = new PartyData();
        private PartyData creditor = new PartyData();

        public List<AgreementText> AgreementText { get; internal set; }
        public AgreementData Agreement { get; internal set; }
        public List<PartyData> PartyData
        {
            get { return partyData; }
            internal set
            {
                partyData = value;
                SyncPartyData();
            }
        }

        internal DocumentBuilder Build()
        {            
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            BuildHeader(sectionBuilder.AddHeaderToBothPages(80));
            BuildFooter(sectionBuilder.AddFooterToBothPages(70));
            //BuildBarcodeFooter(sectionBuilder.AddFooterToBothPages(70));
            sectionBuilder
                .AddParagraph(MAIN_TITLE)
                .SetAlignment(HorizontalAlignment.Center)
                .SetFont(FNT20B)
                .SetMarginBottom(30);
            BuildTextsPages(sectionBuilder);
            BuildSignatures(sectionBuilder, GetSignData());
            return documentBuilder;
        }

        private void BuildHeader(RepeatingAreaBuilder builder)
        {
            builder
                .AddImage(Path.Combine("images", "pa-logo.png"), 
                    XSize.FromHeight(75));
            builder.AddLine(PageWidth, 1.5f, Stroke.Solid, Color.Gray)
                .SetMarginTop(5);

        }

        private void BuildFooter(RepeatingAreaBuilder builder)
        {
            builder
                .AddQRCodeUrl("http://www.besthospital.com/payments", 2,
                              Color.Black, Color.White, false).SetHeight(50);
            ParagraphBuilder paragraphBuilder = builder
                .AddParagraph();
            FontBuilder font = Fonts.Helvetica(7f).SetBold().SetUnderline(Stroke.Solid, Color.Blue);
            paragraphBuilder
                .AddTabSymbol()
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFont(font))
                .AddUrlToParagraph("http://www.besthospital.com/payments")
                .AddTabulation(2, TabulationType.Left,
                    TabulationLeading.None)
                .AddTabulation(550, TabulationType.Right,
                    TabulationLeading.None)
                .AddTextToParagraph(" ", FNT11, true)
                .AddTextToParagraph("Page ", FNT9)
                .AddPageNumber().SetFont(FNT9);
        }

        private void BuildTextsPages(SectionBuilder sectionBuilder)
        {
            InitDictionary();
            foreach (AgreementText article in AgreementText) {
                var paragraphBuilder = sectionBuilder.AddParagraph();
                paragraphBuilder.AddText(article.Header).SetFont(FNT11B);
                paragraphBuilder.SetMarginBottom(16);
                int last = article.Text.Length - 1;
                if (last < 0)
                {
                    continue;
                }
                paragraphBuilder.SetFont(FNT11).AddText(" ");
                for (int i = 0; ; i++)
                {
                    paragraphBuilder.AddText(SubsPlaceholders(article.Text[i]));
                    if (i == last)
                    {
                        break;
                    }
                    paragraphBuilder = sectionBuilder.AddParagraph().SetFont(FNT11);
                    paragraphBuilder.SetMarginBottom(16);
                }
            }
        }

        private string SubsPlaceholders(string text)
        {
            Regex regex = new Regex(@"\{\s*([^\s\}]+)\s*\}");
            MatchCollection matches = regex.Matches(text);
            if (matches.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                int end = 0;
                foreach(Match match in matches)
                {
                    int start = match.Index;
                    sb.Append(text.Substring(end, start - end));
                    sb.Append(SubsPlaceholder(match));
                    end = start + match.Length;

                }
                sb.Append(text.Substring(end));
                text = sb.ToString();
            }
            return text;
        }

        private void BuildSignatures(SectionBuilder sectionBuilder, Sign[] data)
        {

            foreach(Sign sign in data)
            {
                sectionBuilder.AddParagraph()
                   .SetMarginBottom(20)
                   .AddTextToParagraph(sign.name, FNT11B, true)
                   .AddTextToParagraph(" Date: ", FNT11, true)
                   .AddTabulation(250, TabulationType.Left,
                        TabulationLeading.BottomLine)
                   .AddTabulation(500, TabulationType.Right,
                        TabulationLeading.BottomLine);
                sectionBuilder.AddParagraph().SetMarginBottom(20)
                    .AddText(sign.value);
            }
        }

        private Sign[] GetSignData()
        {
            Sign[] result = new Sign[PartyData.Count];
            for (int i = 0, l = PartyData.Count; i < l; i++)
            {
                PartyData party = PartyData[i];
                result[i] = new Sign(party.Party + "’s Signature:", party.Name);
            }
            return result;
        }

        private void SyncPartyData()
        {
            foreach (PartyData party in partyData)
            {
                switch (party.Party)
                {
                    case "Debtor":
                        debtor = party;
                        break;
                    case "Co-Signer":
                        cosigner = party;
                        break;
                    case "Creditor":
                        creditor = party;
                        break;
                }
            }
        }

        private void InitDictionary()
        {
            this.dict = new Dictionary<string, string>
            {
                ["agreement.date"] = DateToString(Agreement.Date),
                ["parties.creditor.name"] = creditor.Name,
                ["parties.creditor.mailaddress"] = creditor.MailAddress,
                ["parties.debtor.name"] = debtor.Name,
                ["parties.debtor.mailAddress"] = debtor.MailAddress,
                ["parties.co-signer.name"] = cosigner.Name,
                ["agreement.balance"] = FundToString(Agreement.Balance),
                ["agreement.discountedBalance"] = FundToString(Agreement.DiscountedBalance),
                ["agreement.downPayment"] = FundToString(Agreement.DownPayment),
                ["agreement.interestRate"] = PercentsToString(Agreement.InterestRate),
                ["agreement.repaymentFrequency"] = Agreement.RepaymentFrequency,
                ["agreement.repaymentPeriodBegins"] = DateToString(Agreement.RepaymentPeriodBegins),
                ["agreement.repaymentPeriodEnds"] = DateToString(Agreement.RepaymentPeriodEnds),
                ["agreement.extensionPeriodOne"] = Agreement.ExtensionPeriodOne.ToString(),
                ["agreement.extensionPeriod"] = Agreement.ExtensionPeriod.ToString(),
                ["agreement.brokenPayPeriod"] = Agreement.BrokenPayPeriod,
                ["agreement.governingLawState"] = Agreement.GoverningLawState,
                ["agreement.governingLaw"] = Agreement.GoverningLaw
            };
        }

        private string SubsPlaceholder(Match match)
        {
            if (dict.TryGetValue(match.Groups[1].ToString(), out string result))
                return result;
            return match.Groups[0].ToString();
        }

        private string DateToString(DateTime date)
        {
            return date.ToString(
                        "MMMM dd yyyy", DocumentLocale);
        }

        private string FundToString(double fund)
        {
            return "$" + String.Format(
                DocumentLocale, "{0:0,0.00}", fund);
        }

        private string PercentsToString(double procents)
        {
            if (procents < 10)
            {
                return 
                    String.Format(DocumentLocale, "{0:,0.00}", procents) + "%";
            }
            return String.Format(DocumentLocale, "{0:0,0.00}", procents) + "%";
        }

        internal struct Sign
        {
            public string name;
            public string value;
            public Sign(string name, string value)
            {
                this.name = name;
                this.value = value;
            }

        }
    }
}