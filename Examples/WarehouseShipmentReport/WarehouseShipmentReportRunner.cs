using Gehtsoft.PDFFlow.Builder;

namespace WarehouseShipmentReport
{
    public static class WarehouseShipmentReportRunner
    {
        public static DocumentBuilder Run()
        {
            return DocumentBuilder.New()
                .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
                .AddWarehouseShipmentReport();
        }
    }
}