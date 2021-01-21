using Gehtsoft.PDFFlow.Builder;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceFrontBuilder
    {
        internal void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder.AddSection()
                .SetOrientation(Orientation)
                .SetMargins(Margins)
                .SetStyleFont(FNT7);

            new TravelInsuranceHeaderBuilder().Build(sectionBuilder);
            new TravelInsuranceGeneralInfoBuilder().Build(sectionBuilder);
            new TravelInsuranceSectionABuilder().Build(sectionBuilder);
            new TravelInsuranceSectionBBuilder().Build(sectionBuilder);
        }
    }
}