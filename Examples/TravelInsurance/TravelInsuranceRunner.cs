using Gehtsoft.PDFFlow.Builder;

namespace TravelInsurance
{
    public static class TravelInsuranceRunner
    {
        public static DocumentBuilder Run()
        {
            var travelInsuranceBuilder = new TravelInsuranceBuilder() { };

            return travelInsuranceBuilder.CreateDocumentBuilder();
        }
    }
}