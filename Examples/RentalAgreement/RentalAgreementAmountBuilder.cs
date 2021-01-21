using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using RentalAgreement.Model;
using static RentalAgreement.RentalAgreementBuilder;
using static RentalAgreement.RentalAgreementHelper;

namespace RentalAgreement
{
    internal class RentalAgreementAmountBuilder
    {
        private const float MAIN_TITLE_BOTTOM_MARGIN = 40;
        internal static readonly Box Margins = new Box(60, 16, 60, 16);
        internal static readonly XUnit PageWidth =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));

        internal static readonly XUnit PageHeight =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Height -
                (Margins.Top + Margins.Bottom));

        private AgreementData agreement;

        internal RentalAgreementAmountBuilder SetAgreement(
            AgreementData agreement)
        {
            this.agreement = agreement;
            return this;
        }

        internal void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder
                .AddSection()
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            //sectionBuilder.SetRepeatingAreaPriority(RepeatingAreaPriority.Vertical);
            BuildHeader(sectionBuilder.AddHeaderToBothPages(140), PageWidth);
            BuildFooter(sectionBuilder.AddFooterToBothPages(40));
            //BuildFooterQqualHousingOpportunity(sectionBuilder.AddFooterToBothPages(85));
            //BuildQqualHousingOpportunity(sectionBuilder.AddRptAreaLeftToBothPages(85));
            sectionBuilder.AddParagraph()
                .SetAlignment(HorizontalAlignment.Center)
                .SetMarginBottom(MAIN_TITLE_BOTTOM_MARGIN)
                .AddText("AMOUNT ($) DUE AT SIGNING");
            sectionBuilder.AddParagraph()
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("Security Deposit: ", HEADER_FONT)
                .AddTextToParagraph(FundToString(agreement.SecurityDeposit), 
                    TEXT_FONT);
            sectionBuilder.AddParagraph()
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("First (1st) Month's Rent: ", HEADER_FONT)
                .AddTextToParagraph(FundToString(agreement.MonthPayment), 
                    TEXT_FONT);
            sectionBuilder.AddParagraph()
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("Pet Fee(s): ", HEADER_FONT)
                .AddTextToParagraph(FundToString(agreement.PetsFee), 
                    TEXT_FONT)
                .AddTextToParagraph(" for all the Pet(s)", TEXT_FONT);
        }
    }
}