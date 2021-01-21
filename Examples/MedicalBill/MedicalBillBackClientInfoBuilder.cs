using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using static MedicalBill.MedicalBillBuilder;
using Gehtsoft.PDFFlow.Utils;

namespace MedicalBill
{
    internal class MedicalBillBackClientInfoBuilder : FormBuilder
    {
        private Params ps;

        public MedicalBillBackClientInfoBuilder(Params ps)
        {
            this.ps = ps;
        }

        internal void Build(SectionBuilder sectionBuilder)
        {
            AddInfoTitle(sectionBuilder);
            AddClientTable(sectionBuilder);
        }

        private void AddInfoTitle(SectionBuilder sectionBuilder)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginTop(17)
                .SetFont(FNT9)
                .AddTextToParagraph("If any of this following has changed since your last statement, please indicate…");
        }

        private void AddClientTable(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 50).AddColumnPercent("", 50);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.AddCell().AddTable(FillAboutYouTable);
            rowBuilder.AddCell().AddTable(FillInsuranceTable);
        }

        private void FillAboutYouTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(PageWidth / 2 - 8)
                .SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0f, 0f, 0f, 0f)
                .AddColumnPercent("", 100);
            var rowBuilder = tableBuilder.AddRow();
            AddTitle(rowBuilder, "About you:");
            rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            AddForm(cellBuilder, MedicalBillBuilder.CLIENT_NAME_TEXT, new int[] { 100 });
            AddForm(cellBuilder, MedicalBillBuilder.CLIENT_ADDRESS_TEXT, new int[] { 100 });
            AddForm(cellBuilder, MedicalBillBuilder.ADDRESS_FIELDS_TEXT,
                new int[] { 50, 25, 25 });
            AddForm(cellBuilder, MedicalBillBuilder.CLIENT_PHONE_TEXT, new int[] { 100 });
            AddRadioButtons(cellBuilder,
                MedicalBillBuilder.MARITAL_STATUS_TEXT,
                MedicalBillBuilder.MARITAL_STATUS_VALUES_TEXT,
                new int[] { 14, 17, 17, 17, 17, 17 });
            AddForm(cellBuilder, MedicalBillBuilder.EMPL_PHONE_TEXT, new int[] { 50, 50 });
            AddForm(cellBuilder, MedicalBillBuilder.EMPL_ADDRESS_TEXT, new int[] { 100 });
            AddForm(cellBuilder, MedicalBillBuilder.ADDRESS_FIELDS_TEXT, 
                new int[] { 50, 25, 25 }, true);
            rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .SetBorder(borderBuilder =>
                {
                    borderBuilder
                        .SetRightWidth(0).SetTopWidth(0).SetLeftWidth(0).SetBottomWidth(2)
                        .SetBottomStroke(Stroke.Solid);
                });
            cellBuilder = rowBuilder.AddCell().SetPadding(0, 7, 0, 0);
            cellBuilder.AddParagraph().SetFont(FNT11).AddText("Comments:");
            AddRowForClient(tableBuilder);
        }

        private void FillInsuranceTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0, 0, 0, 0)
                .AddColumnPercent("", 100);
            var rowBuilder = tableBuilder.AddRow();
            AddTitle(rowBuilder, "About your insurance:");
            rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            AddForm(cellBuilder, MedicalBillBuilder.ENS_PR_NAME_TEXT, new int[] { 60, 40 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_PR_ADDRESS_TEXT,
                new int[] { 60, 40 });
            AddForm(cellBuilder, MedicalBillBuilder.ADDRESS_FIELDS_TEXT,
                new int[] { 50, 25, 25 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_POLICY_TEXT, new int[] { 50, 50 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_RELATION_TEXT,
                new int[] { 100 }, true,
                new string[] { " " }, 0.5f, 2.0f, 13f);
            AddForm(cellBuilder, MedicalBillBuilder.ENS_SC_NAME_TEXT, new int[] { 60, 40 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_SC_ADDRESS_TEXT,
                new int[] { 60, 40 });
            AddForm(cellBuilder, MedicalBillBuilder.ADDRESS_FIELDS_TEXT,
                new int[] { 50, 25, 25 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_POLICY_TEXT, new int[] { 50, 50 });
            AddForm(cellBuilder, MedicalBillBuilder.ENS_RELATION_TEXT, 
                new int[] { 100 }, true);
        }

        private void AddTitle(TableRowBuilder rowBuilder, string text)
        {
            rowBuilder.SetBorder(borderBuilder =>
            {
                borderBuilder
                    .SetRightWidth(0).SetTopWidth(0).SetLeftWidth(0).SetBottomWidth(2)
                    .SetStroke(Stroke.Solid);
            });
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder.AddParagraph().SetFont(FNT11).AddTextToParagraph(text);
        }

        private void AddRowForClient(TableBuilder tableBuilder, float fontSize = 14)
        {
            FontBuilder font = Fonts.Helvetica(fontSize);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.SetBorder(borderBuilder =>
            {
                borderBuilder
                    .SetRightWidth(0).SetTopWidth(0).SetLeftWidth(0)
                    .SetBottomBorder(0.5f, Stroke.Solid, null);
            });
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder.AddParagraph().SetFont(font).AddTextToParagraph(" ");
        }
    }
}