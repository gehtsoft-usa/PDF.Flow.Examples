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
    internal class RentalAgreementCheckListBuilder
    {

        private const float MAIN_TITLE_BOTTOM_MARGIN = 10;
        const float RAGRAPH_BOTTOM_MARGIN = 16;
        const float ROW_BOTTOM_MARGIN = 1;

        internal static readonly Box Margins = new Box(60, 16, 60, 16);
        internal static readonly XUnit PageWidth =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));

        internal static readonly XUnit PageHeight =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Height -
                (Margins.Top + Margins.Bottom));

        private AgreementData agreement;
        private List<CheckList> checkList;

        internal RentalAgreementCheckListBuilder SetAgreement(
            AgreementData agreement)
        {
            this.agreement = agreement;
            return this;
        }

        internal RentalAgreementCheckListBuilder SetCheckList(
            List<CheckList> checkList)
        {
            this.checkList = checkList;
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
            BuildHeader(sectionBuilder.AddHeaderToBothPages(80), PageWidth);
            BuildFooter(sectionBuilder.AddFooterToBothPages(40));
            //BuildFooterQqualHousingOpportunity(sectionBuilder.AddFooterToBothPages(85));
            //BuildQqualHousingOpportunity(sectionBuilder.AddRptAreaLeftToBothPages(65), PageHeight);
            sectionBuilder.AddParagraph()
                .SetFont(MAIN_TITLE_FONT)
                .SetAlignment(HorizontalAlignment.Center)
                .SetMarginBottom(MAIN_TITLE_BOTTOM_MARGIN)
                .AddText("Move-in Checklist");
            sectionBuilder.AddParagraph()
                .SetFont(TEXT_FONT)
                .SetMarginBottom(ROW_BOTTOM_MARGIN)
                .AddTextToParagraph("Property Address: ")
                .AddTextToParagraph(agreement.AptAddress);
            sectionBuilder.AddParagraph()
                .SetFont(TEXT_FONT)
                .SetMarginBottom(ROW_BOTTOM_MARGIN)
                .AddTextToParagraph("Unit Size: ")
                .AddTextToParagraph(agreement.AptFeatures);
            sectionBuilder.AddParagraph()
                .SetMarginBottom(RAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("Move-in Inspection Date: ", TEXT_FONT, 
                    true)
                .AddTabulation(220, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTabulation(460, TabulationType.Left,
                    TabulationLeading.BottomLine)
                .AddTextToParagraph("Move-out Inspection Date: ", TEXT_FONT, 
                    true);
            sectionBuilder.AddParagraph()
                .SetFont(TEXT_FONT)
                .SetMarginBottom(RAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("Write the condition of the space along with any specific damage or repairs needed. Be sure to write" +
                    " any repair needed such as paint chipping, wall damage, or any lessened area that could be" +
                    " considered maintenance needed at the end of the lease, and therefore, be deducted at the end of the" +
                    " Lease Term.");
            foreach(CheckList room in checkList)
            {
                sectionBuilder.AddParagraph()
                    .SetFont(MAIN_TITLE_FONT)
                    .SetMarginBottom(RAGRAPH_BOTTOM_MARGIN)
                    .AddTextToParagraph(room.Name);
                for (int i = room.Items.Length - 1, j = 0; i >= 0; i--, j++)
                {
                    string item = room.Items[j];
                    sectionBuilder.AddParagraph()
                        .SetMarginBottom(i > 0 ? 
                                    ROW_BOTTOM_MARGIN : 
                                    RAGRAPH_BOTTOM_MARGIN)
                        .AddTextToParagraph(item + " ", TEXT_FONT, true)
                        .AddTabulation(220, TabulationType.Left,
                            TabulationLeading.BottomLine)
                        .AddTabulation(460, TabulationType.Left,
                            TabulationLeading.BottomLine)
                        .AddTextToParagraph(" Specific Damage ", TEXT_FONT, 
                        true);

                }

            }
            sectionBuilder.AddParagraph()
                .SetFont(TEXT_FONT)
                .SetMarginBottom(RAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("I, a Tenant on this Lease, have sufficiently inspected the Premises and confirm above-stated" +
                    " information. (only 1 Tenant required)");
            sectionBuilder.AddParagraph()
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("Tenant's Signature ", HEADER_FONT, true)
                .AddTabulation(250, TabulationType.Left,
                    TabulationLeading.BottomLine);
            sectionBuilder.AddParagraph()
                .SetFont(TEXT_FONT)
                .SetMarginBottom(RAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("I, the Landlord on this Lease, have sufficiently inspected the Premises and confirm above-stated" +
                    "information.");
            sectionBuilder.AddParagraph()
                .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                .AddTextToParagraph("Landlord's Signature ", HEADER_FONT, 
                    true)
                .AddTabulation(250, TabulationType.Left,
                    TabulationLeading.BottomLine);
        }
    }
}