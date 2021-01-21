using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static MedicalBill.MedicalBillBuilder;

namespace MedicalBill
{
    internal class MedicalBillFrontBuilder
    {
        private readonly Params ps;

        public MedicalBillFrontBuilder(Params ps)
        {
            this.ps = ps;
        }

        public void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins)
                .AddFooterToOddPage(12, BuildFooter);
            new MedicalBillHeadBuilder(
                ps
            ).Build(sectionBuilder);

            new MedicalBillBodyBuilder(
                ps
            ).Build(sectionBuilder);

            new MedicalBillBottomBuilder(
                ps
            ).Build(sectionBuilder);
        }

        private void BuildFooter(RepeatingAreaBuilder footerBuilder)
        {
            var paragraphBuilder = footerBuilder.AddParagraph();
            paragraphBuilder
                .SetAlignment(HorizontalAlignment.Right)
                .SetFont(FNT7)
                .AddTextToParagraph("1nnnnn-XX-XXX-00");
        }
    }
}