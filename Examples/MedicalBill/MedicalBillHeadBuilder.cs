using System.IO;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using static MedicalBill.MedicalBillBuilder;

namespace MedicalBill
{
    internal class MedicalBillHeadBuilder
    {
        private readonly Params ps;
        public MedicalBillHeadBuilder(Params ps)
        {
            this.ps = ps;
        }

        internal void Build(SectionBuilder sectionBuilder)
        {
            BuildHead(sectionBuilder);
        }

        private void BuildHead(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetContentRowStyleFont(FNT9)
                .SetBorder(Stroke.None)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 33)
                .AddColumnPercentToTable("", 33)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercent("", 17);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder.SetPadding(0, 0, 4, 0);
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .AddInlineImage(Path.Combine(ps.ImageDir, "MB_Logo_2x.png"));
            cellBuilder = rowBuilder.AddCell();
            cellBuilder.SetPadding(4, 0, 4, 0);
            paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .AddInlineImage(Path.Combine(ps.ImageDir, "Clinicare_2x.png"))
                .SetAlignment(HorizontalAlignment.Center);
            cellBuilder = rowBuilder.AddCell();
            cellBuilder.SetColSpan(2).SetPadding(4, 0, 0, 0);
            paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .AddInlineImage(Path.Combine(ps.ImageDir, "Healthcare_2x.png"))
                .SetAlignment(HorizontalAlignment.Right);
            rowBuilder = tableBuilder.AddRow();
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .AddParagraphToCell(ps.CenterName + "\n" + 
                    ps.CenterAddress);
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .AddParagraphToCell("To Contact Us Call:  " + 
                    ps.CenterPhone + 
                    "\n\nPhone representatives are available:\n8am to 8pm Monday - Thursday\nand 8am to 4:30pm Friday");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .AddParagraphToCell("Guarantor Number:\nGuarantor Name:\nStatement Date:\nDue Date:");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .AddParagraphToCell(
                    ps.GuarantorNumber + "\n" + 
                    ps.GuarantorName + "\n" + 
                    ps.StatementDate + "\nUpon Receipt")
                .SetHorizontalAlignment(HorizontalAlignment.Right);
        }
    }
}