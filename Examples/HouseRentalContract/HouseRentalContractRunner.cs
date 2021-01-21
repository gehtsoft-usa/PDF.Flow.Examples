using Gehtsoft.PDFFlow.Builder;

namespace HouseRentalContract
{
    public static class HouseRentalContractRunner
    {
        public static DocumentBuilder Run()
        {
            return DocumentBuilder.New()
                .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
                .AddHouseRentalContract();
        }
    }
}