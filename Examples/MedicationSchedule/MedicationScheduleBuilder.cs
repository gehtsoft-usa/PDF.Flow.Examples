using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Utils;

namespace MedicationSchedule
{
    internal class MedicationScheduleBuilder
    {
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8);
        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10);
        internal static readonly FontBuilder FNT10B = Fonts.Helvetica(10).SetBold();
        internal static readonly FontBuilder FNT11B = Fonts.Helvetica(11).SetBold();
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12);

        internal DocumentBuilder Build()
        {
            var documentBuilder = DocumentBuilder.New();
            var sectionBuilder = documentBuilder.AddSection();

            sectionBuilder
                .SetOrientation(PageOrientation.Landscape)
                .SetMargins(36, 20, 37, 20)
                .SetSize(PaperSize.Letter);

            new MedicationScheduleHeaderBuilder().Build(sectionBuilder);
            new MedicationScheduleDrugDosageBuilder().Build(sectionBuilder);
            new MedicationScheduleFooterBuilder().Build(sectionBuilder);

            return documentBuilder;
        }
    }
}