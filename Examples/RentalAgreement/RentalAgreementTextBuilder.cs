using System;
using System.Collections.Generic;
using RentalAgreement.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using System.Text.RegularExpressions;
using System.Text;
using static RentalAgreement.RentalAgreementBuilder;
using static RentalAgreement.RentalAgreementHelper;

namespace RentalAgreement
{
    internal class RentalAgreementTextBuilder
    {

        internal static readonly Box Margins = new Box(25, 16, 60, 16);
        internal static readonly XUnit PageWidth =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));

        internal static readonly XUnit PageHeight =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Height -
                (Margins.Top + Margins.Bottom));

        private PartyData landlord = new PartyData();
        private PartyData manager = new PartyData();
        private List<PartyData> tenant = new List<PartyData>();
        private List<PartyData> tenantAdd = new List<PartyData>();
        private AgreementData agreement;

        internal Dictionary<string, string> dict;
        private List<AgreementText> agreementText;

        internal RentalAgreementTextBuilder SetLandord(PartyData landlord)
        {
            this.landlord = landlord;
            return this;
        }

        internal RentalAgreementTextBuilder SetManager(PartyData manager)
        {
            this.manager = manager;
            return this;
        }

        internal RentalAgreementTextBuilder SetTenant(List<PartyData> tenant)
        {
            this.tenant = tenant;
            return this;
        }

        internal RentalAgreementTextBuilder SetTenantAdd(
            List<PartyData> tenantAdd)
        {
            this.tenantAdd = tenantAdd;
            return this;
        }

        internal RentalAgreementTextBuilder SetAgreement(
            AgreementData agreement)
        {
            this.agreement = agreement;
            return this;
        }

        internal RentalAgreementTextBuilder SetAgreementText(
            List<AgreementText> agreementText)
        {
            this.agreementText = agreementText;
            return this;
        }

        internal void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            sectionBuilder.SetRepeatingAreaPriority(
                RepeatingAreaPriority.Vertical);
            BuildHeader(
                sectionBuilder.AddHeaderToBothPages(80), PageWidth);
            BuildFooterBar(sectionBuilder.AddFooterToBothPages(90), 86);
            BuildEqualHousingOpportunity(
                sectionBuilder.AddRptAreaLeftToBothPages(65), PageHeight);
            sectionBuilder
                .AddParagraph(MAIN_TITLE)
                .SetAlignment(HorizontalAlignment.Center)
                .SetFont(MAIN_TITLE_FONT)
                .SetMarginBottom(MAIN_TITLE_BOTTOM_MARGIN);
            BuildTextsPages(sectionBuilder);
            BuildSignatures(sectionBuilder, GetSignData());
        }


        private void BuildTextsPages(SectionBuilder sectionBuilder)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder.SetFont(TEXT_FONT)
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("This agreement, dated December 9 2020, by and between a business entity known as ")
                .AddTextToParagraph(landlord.Name)
                .AddTextToParagraph(" of ")
                .AddTextToParagraph(landlord.MailAddress)
                .AddTextToParagraph(", hereinafter known as the “")
                .AddTextToParagraph(landlord.KnownAs)
                .AddTextToParagraph("”.");
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetFont(HEADER_FONT).SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("AND");
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder.SetFont(TEXT_FONT)
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("2 individuals known as ")
                .AddTextToParagraph(AsList(tenant, KnownAsField))
                .AddTextToParagraph(", hereinafter known as the “Tenant(s)”, agree to the following: ");
            InitDictionary();
            foreach (AgreementText article in agreementText)
            {
                paragraphBuilder = sectionBuilder.AddParagraph();
                paragraphBuilder.AddText(article.Header).SetFont(HEADER_FONT);
                paragraphBuilder.SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN);
                int last = article.Text.Length - 1;
                if (last < 0)
                {
                    continue;
                }
                paragraphBuilder.SetFont(TEXT_FONT).AddText(" ");
                for (int i = 0; ; i++)
                {
                    paragraphBuilder.AddText(
                        SubsPlaceholders(article.Text[i]));
                    if (i == last)
                    {
                        break;
                    }
                    paragraphBuilder = sectionBuilder.AddParagraph()
                        .SetFont(TEXT_FONT)
                        .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN);
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
                foreach (Match match in matches)
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

        private void BuildSignatures(SectionBuilder sectionBuilder, 
            Sign[] data)
        {
            for (int i = 0, l = data.Length; i < l; i++)
            {
                Sign sign = data[i];
                var paragraphBuilder = sectionBuilder.AddParagraph();
                if (i == 0)
                {
                    paragraphBuilder.SetMarginTop(SIGNATURES_TOP_MARGIN);
                }
                if (i < 2)
                {
                    paragraphBuilder
                        .SetFont(HEADER_FONT)
                        .AddTextToParagraph(
                            i == 0 ?
                            "LANDLORD(S) SIGNATURE" :
                            "TENANT(S) SIGNATURE"
                        )
                        .SetMarginBottom(SIGNATURE_BOTTOM_MARGIN);
                    paragraphBuilder = sectionBuilder.AddParagraph();
                }
                paragraphBuilder
                    .AddTextToParagraph(sign.name + " ", HEADER_FONT, true)
                    .AddTabulation(380, TabulationType.Left,
                        TabulationLeading.BottomLine);
                if (sign.value != null)
                {
                    paragraphBuilder = sectionBuilder.AddParagraph()
                        .AddTextToParagraph(sign.value);
                }
                paragraphBuilder.SetMarginBottom(SIGNATURE_BOTTOM_MARGIN);
            }
        }

        private Sign[] GetSignData()
        {
            Sign[] result = new Sign[tenant.Count + 1];
            result[0] = new Sign(landlord.Party + "’s Signature:", 
                landlord.Signer);
            for (int i = 0, j = 1, l = tenant.Count; i < l; i++, j++)
            {
                PartyData party = tenant[i];
                result[j] = new Sign(party.Party + "’s Signature:", null);
            }
            return result;
        }

        private void InitDictionary()
        {
            this.dict = new Dictionary<string, string>
            {
                ["agreement.date"] = DateToString(agreement.Date),
                ["tenantAdd.knownAs.asList"] = AsList(tenantAdd, 
                    KnownAsField),
                ["agreement.aptAddress"] = agreement.AptAddress,
                ["agreement.aptFeatures"] = agreement.AptFeaturesBath + 
                    " and " + agreement.AptFeatures,
                ["agreement.aptFurnishing"] = agreement.AptFurnishing,
                ["agreement.appliances"] = agreement.Appliances,
                ["agreement.dateBegin"] = DateToString(agreement.DateBegin),
                ["agreement.dateEnd"] = DateToString(agreement.DateEnd),
                ["agreement.cancelNoticePeriodDays"] =
                    agreement.CancelNoticePeriodDays,
                ["agreement.continueNoticePeriondDays"] =
                    agreement.ContinueNoticePeriondDays,
                ["agreement.monthPayment"] =
                    FundToString(agreement.MonthPayment),
                ["agreement.nonsufficientFundsFee"] =
                    FundToString(agreement.NonsufficientFundsFee),
                ["agreement.lateFee"] = FundToString(agreement.LateFee),
                ["agreement.securityDeposit"] =
                    FundToString(agreement.SecurityDeposit),
                ["agreement.rightToBuyPrice"] =
                    FundToString(agreement.RightToBuyPrice),
                ["agreement.rightToBuyDeposit"] =
                    FundToString(agreement.RightToBuyDeposit),
                ["agreement.abandonmentPeriod"] = agreement.AbandonmentPeriod,
                ["agreement.parkingSpace"] = agreement.ParkingSpace,
                ["agreement.parkingSpaceDescription"] =
                    agreement.ParkingSpaceDescription,
                ["agreement.terminationPeriod"] =
                    agreement.TerminationPeriod.ToString(),
                ["agreement.terminationFee"] =
                    FundToString(agreement.TerminationFee),
                ["agreement.petsAllowed"] = agreement.PetsAllowed,
                ["agreement.petsFee"] = FundToString(agreement.PetsFee),
                ["landlord.name"] = landlord.Name,
                ["landlord.nameExt"] = landlord.NameExt,
                ["landlord.mailAddress"] = landlord.MailAddress,
                ["tenant.knownAs.asList"] = AsList(tenant, KnownAsField),
                ["tenant.mailAddress"] = AsList(tenant, MailAddressField),
                ["manager.name"] = manager.Name,
                ["manager.knownAs"] = manager.KnownAs,
                ["manager.mailAddress"] = manager.MailAddress,
                ["manager.phone"] = manager.Phone,
                ["manager.emailAddress"] = manager.EmailAddress,
                ["agreement.armedForcesNotice"] = agreement.ArmedForcesNotice,
                ["agreement.lawsState"] = agreement.LawsState
            };
        }

        private string KnownAsField(PartyData party)
        {
            return party.KnownAs;
        }

        private string MailAddressField(PartyData party)
        {
            return party.MailAddress;
        }

        private string AsList(List<PartyData> parties, 
            Func<PartyData, string> getFieldValue)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (PartyData party in parties)
            {
                String value = getFieldValue(party);
                if (!string.IsNullOrEmpty(value))
                {
                    if (i++ > 0)
                    {
                        sb.Append(" and ");
                    }
                    sb.Append(value);
                }
            }
            return sb.ToString();
        }

        private string SubsPlaceholder(Match match)
        {
            if (dict.TryGetValue(match.Groups[1].ToString(), 
                    out string result))
                return result;
            return match.Groups[0].ToString();
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