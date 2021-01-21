using System.IO;
using Gehtsoft.PDFFlow.Builder;

namespace MedicalBill
{
    public static class MedicalBillRunner
    {
        public static DocumentBuilder Run()
        {
            string imageDir = Path.Combine(Directory.GetCurrentDirectory(), "images");
            MedicalBillBuilder medicalBillBuilder = new MedicalBillBuilder(imageDir)
            {
                //Real values
                GuarantorNumber = "2nnnnn",
                StatementDate = "07/10/2020",
                CenterName = "Sample Medical Center",
                CenterAddress = "123 Main Street\nAnywhere, NY 12345 - 6789",
                CenterPhone = "123 - 456 - 7890",
                CenterPatent = "Sample Patent",
                FinServAddress = "Main Street 123",
                FinServUrl = "http://www.ourwebsite.com/PatientFinancialServices.aspx"
            };
            DocumentBuilder doc = medicalBillBuilder
                .AddRowData("07/01/2020 to 07/01/2020", "Visit #123 Sample Patient")
                .AddRowData("", "Pharmacy" , "60.53")
                .AddRowData("", "Treatment or Observation Room", "588.00")
                .AddRowData("", "Insurance Payment", "", "-598.53")
                .AddRowData("", "Total Hospital Charges", "638.53")
                .AddRowData("", "Total Payments", "", "-598.53")
                .AddRowData("", "Total Adjustments", "", "0.00")
                .AddRowData("", "Patient Due", "", "", "40.00")
                .AddCardName("Visa")
                .AddCardName("MasterCard")
                .AddCardName("Discover")
                .AddCardName("Amex")
                .CreateDocumentBuilder();
            return doc;
        }
    }
}