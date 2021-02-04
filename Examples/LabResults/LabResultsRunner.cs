using Gehtsoft.PDFFlow.Builder;

namespace LabResults
{
    public class LabResultsRunner
    {
        public static DocumentBuilder Run()
        {
            LabResultsBuilder labResultsBuilder = new LabResultsBuilder(); 

            return labResultsBuilder.Build();
        }
    }
}