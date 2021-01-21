using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using static MedicalBill.MedicalBillBuilder;

namespace MedicalBill
{
    internal class MedicalBillBodyBuilder
    {
        private readonly List<RowData> rowsData;

        public MedicalBillBodyBuilder(Params ps)
        {
            this.rowsData = ps.RowsData;
        }

        internal void Build(SectionBuilder sectionBuilder)
        {
            BuildBody(sectionBuilder);
        }

        private void BuildBody(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercentToTable("", 47)
                .AddColumnPercentToTable("", 12)
                .AddColumnPercentToTable("", 12)
                .AddColumnPercent("", 12);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .SetBackColor(BLUE_COLOR)
                .SetFont(FNT9B_W)
                .SetHorizontalAlignment(HorizontalAlignment.Center);
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .AddParagraphToCell("Date of Service");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .AddParagraphToCell("Description");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .AddParagraphToCell("Charges");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .AddParagraphToCell("Payment/ Adjustments");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .AddParagraphToCell("Patient Balance");
            int i = rowsData.Count;
            foreach (RowData rowData in rowsData)
            {
                FontBuilder curFont = (--i) == 0 ? FNT10B : FNT10;
                rowBuilder = tableBuilder.AddRow();
                rowBuilder
                    .SetFont(curFont)
                    .SetBorder(BorderBuilder.New()
                        .SetRightWidth(0)
                        .SetTopWidth(0)
                        .SetLeftWidth(0)
                        .SetBottomWidth(0.5f)
                        .SetBottomStroke(Stroke.Solid)
                    );
                cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .SetPadding(4)
                    .AddParagraphToCell(rowData.Date);
                cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .SetPadding(4)
                    .AddParagraphToCell(rowData.Description);
                cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .SetPadding(4)
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .AddParagraphToCell(rowData.Charges);
                cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .SetPadding(4)
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .AddParagraphToCell(rowData.Payment);
                cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .SetPadding(4)
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .AddParagraphToCell(rowData.Balance);
            }
        }
    }
}