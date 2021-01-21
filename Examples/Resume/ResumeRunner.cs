using Gehtsoft.PDFFlow.Builder;

namespace Resume
{
    public static class ResumeRunner
    {
        public static DocumentBuilder Run()
        {
            return DocumentBuilder.New()
                .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
                .AddResume();
        }
    }
}