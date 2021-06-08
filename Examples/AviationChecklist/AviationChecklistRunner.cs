using Gehtsoft.PDFFlow.Builder;

namespace AviationChecklist
{
    public static class AviationChecklistRunner
    {
        public static DocumentBuilder Run()
        {
            AviationChecklistBuilder docBuilder = new AviationChecklistBuilder();

            return docBuilder.Build();
        }
    }
}