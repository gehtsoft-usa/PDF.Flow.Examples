using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using static RentalAgreement.RentalAgreementBuilder;
using static RentalAgreement.RentalAgreementHelper;

namespace RentalAgreement
{
    internal class RentalAgreementDepositBuilder
    {
        internal static readonly Box Margins = new Box(60, 16, 60, 16);
        internal static readonly XUnit PageWidth =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));

        internal static readonly XUnit PageHeight =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Height -
                (Margins.Top + Margins.Bottom));

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
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder.SetFont(MAIN_TITLE_FONT)
                .SetAlignment(HorizontalAlignment.Center)
                .SetMarginBottom(MAIN_TITLE_BOTTOM_MARGIN)
                .AddText("Security Deposit Receipt");
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("Date:", TEXT_FONT, true)
                .AddTabulation(150, TabulationType.Left,
                    TabulationLeading.BottomLine);
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("Dear", TEXT_FONT, true)
                .AddTabulation(150, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTextToParagraph("[Tenant(s)],", TEXT_FONT, true);
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetFont(TEXT_FONT)
                .SetMarginTop(PARAGRAPH_TOP_MARGIN)
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph(
                    "The Landlord shall hold the Security Deposit in a separate account at a bank"
                 );
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("located at", TEXT_FONT, true)
                .AddTabulation(280, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTextToParagraph(" [Street Address] in", TEXT_FONT, true);
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("the City of", TEXT_FONT, true)
                .AddTabulation(200, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTabulation(375, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTextToParagraph(", State of", TEXT_FONT, true)
                .AddTextToParagraph(".");
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginBottom(PARAGRAPH_ROW_DIST)
                .AddTextToParagraph(
                    "The Security Deposit in the amount of $", TEXT_FONT, 
                     true)
                .AddTabulation(300, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTextToParagraph(
                    " (US Dollars) has been deposited in", TEXT_FONT, true);
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginBottom(PARAGRAPH_ROW_DIST)
                .AddTextToParagraph(" ", TEXT_FONT, true)
                .AddTabulation(120, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTabulation(434, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTextToParagraph(
                    "[Bank Name] with the Account Number of", TEXT_FONT, true)
                .AddTextToParagraph(" for the full");
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph(
                    "performance of the Lease executed on the ", TEXT_FONT, 
                    true)
                .AddTabulation(220, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTextToParagraph(" day of ", TEXT_FONT, true)
                .AddTabulation(400, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTextToParagraph(", 20", TEXT_FONT, true)
                .AddTabulation(434, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTextToParagraph(".");
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .SetFont(TEXT_FONT)
                .AddTextToParagraph("Sincerely,");
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("Landlord’s Signature ", HEADER_FONT, 
                    true)
                .AddTabulation(250, TabulationType.Left,
                    TabulationLeading.BottomLine);
        }
    }
}