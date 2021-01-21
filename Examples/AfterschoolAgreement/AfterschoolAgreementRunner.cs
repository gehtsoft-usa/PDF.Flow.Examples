using Gehtsoft.PDFFlow.Builder;

namespace AfterschoolAgreement
{
    public static class AfterschoolAgreementRunner
    {
        public static DocumentBuilder Run()
        {
            return DocumentBuilder.New()
                .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
                .AddAfterschoolAgreement();
        }
    }
}