using Gehtsoft.PDFFlow.Builder;

namespace MedicationSchedule
{
    public class MedicationScheduleRunner
    {
        public static DocumentBuilder Run()
        {
            MedicationScheduleBuilder docBuilder = new MedicationScheduleBuilder(); 

            return docBuilder.Build();
        }
    }
}