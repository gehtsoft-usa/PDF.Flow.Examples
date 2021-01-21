using Gehtsoft.PDFFlow.Builder;
using static MedicalBill.MedicalBillBuilder;

namespace MedicalBill
{
    internal class MedicalBillBackBuilder
    {
        private readonly Params ps;

        public MedicalBillBackBuilder(Params ps)
        {
            this.ps = ps;
        }

        internal void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder.SetOrientation(Orientation).SetMargins(Margins);
            new MedicalBillBackTextBuilder(ps).Build(sectionBuilder);
            new MedicalBillBackClientInfoBuilder(ps).Build(sectionBuilder);
        }
    }
}