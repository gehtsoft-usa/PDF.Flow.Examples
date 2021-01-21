using System.IO;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static MedicalBill.MedicalBillBuilder;

namespace MedicalBill
{
    internal class MedicalBillBottomBuilder : FormBuilder
    {
        private readonly Params ps;

        public MedicalBillBottomBuilder(Params ps)
        {
            this.ps = ps;
        }

        internal void Build(SectionBuilder sectionBuilder)
        {
            BuildBottom(sectionBuilder);
        }

        private void BuildBottom(SectionBuilder sectionBuilder)
        {
            BuildBeforeCut(sectionBuilder);
            BuildCut(sectionBuilder);
            BuildAfterCut(sectionBuilder);
        }

        private void BuildAfterCut(SectionBuilder sectionBuilder)
        {
            BuildPayment(sectionBuilder);
            BuildPostNet(sectionBuilder);
        }

        private void BuildPostNet(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 50)
                .AddColumnPercent("", 50);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell().SetFont(FNT12);
            var paragraphBuilder = cellBuilder.AddParagraph((ps.GuarantorName + "\n" +
                    ps.CenterAddress).ToUpper());
            paragraphBuilder.SetMarginTop(10).SetMarginBottom(10);
            cellBuilder.AddImageToCell(Path.Combine(ps.ImageDir,
                    "postnet1_2x.png"), new XSize(124, 11));
            cellBuilder = rowBuilder.AddCell().SetFont(FNT12);
            paragraphBuilder = cellBuilder.AddParagraph((ps.CenterName + "\n" +
                    ps.CenterAddress).ToUpper());
            paragraphBuilder.SetMarginTop(10).SetMarginBottom(10);
            cellBuilder.AddImageToCell(Path.Combine(ps.ImageDir,
                    "postnet2_2x.png"), new XSize(124, 11));
        }

        private void BuildPayment(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 50)
                .AddColumnPercent("", 50);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .AddCellToRow(BuildCheck)
                .AddCellToRow(BuilCard);
        }

        private void BuilCard(TableCellBuilder cellBuilder)
        {
            if (ps.CardNames.Count > 0)
            {
                AddCardHead(cellBuilder);
                AddForm(cellBuilder, MedicalBillBuilder.CARD_TEXTS1, 
                    new int[] { 50, 25, 25 });
                AddForm(cellBuilder, MedicalBillBuilder.CARD_TEXTS2, new int[] { 50, 50 });
                AddForm(cellBuilder, MedicalBillBuilder.CARD_TEXTS3, 
                    new int[] { 33, 34, 33 }, false, 
                    new string[] { ps.StatementDate, ps.GuarantorNumber, ps.Balance });
                AddForm(cellBuilder, MedicalBillBuilder.CARD_TEXTS4, 
                    new int[] { 50, 50 }, true);
            }
        }

        private void AddCardHead(TableCellBuilder outerCellBuilder)
        {
            outerCellBuilder.SetPadding(0, 6, 0, 0);
            var paragraphBuilder = outerCellBuilder.AddParagraph();
            paragraphBuilder
                .SetFont(FNT7B)
                .AddTextToParagraph("IF PAYING BY VISA, MASTERCARD, DISCOVER OR AMEX, FILL OUT BELOW");
            outerCellBuilder.AddTable(FillCardNames);
        }

        private void FillCardNames(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0, 0, 0, 0);
            foreach (string cardName in ps.CardNames)
            {
                tableBuilder
                    .AddColumnPercent("", 100 / ps.CardNames.Count);
            };
            var rowBuilder = tableBuilder.AddRow();
            foreach (string cardName in ps.CardNames)
            {
                var cellBuilder = rowBuilder.AddCell().SetPadding(0, 0, 0, 8);
                cellBuilder.AddParagraph("o").SetFont(FNTZ16);
                cellBuilder.AddParagraph(cardName).SetFont(FNT10);
            }
        }

        private void BuildCheck(TableCellBuilder outerCellBuilder)
        {
            outerCellBuilder.AddTable(FillCheckTable);
        }

        private void FillCheckTable(TableBuilder tableBuilder)
        {
            tableBuilder.SetWidth(PageWidth / 2 - 8).SetBorder(Stroke.None)
                .AddColumnPercent("", 100);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.SetBorder(BorderBuilder.New()
                .SetRightWidth(0)
                .SetTopWidth(0)
                .SetLeftWidth(0)
                .SetBottomWidth(0.5f)
                .SetBottomStroke(Stroke.Solid)
            );
            var cellBuilder = rowBuilder.AddCell().SetPadding(0, 4, 0, 4);
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .AddTextToParagraph("o", FNTZ12)
                .AddTextToParagraph(" Please check box if address is incorrect or insurance information has changed, and indicate change(s) on reverse side.",
                    FNT7);
            rowBuilder = tableBuilder.AddRow();
            rowBuilder.SetBorder(borderBuilder =>
            {
                borderBuilder.SetWidth(0);
            });
            cellBuilder = rowBuilder.AddCell().SetPadding(0, 4, 0, 0);
            paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetFont(FNT8B)
                .AddTextToParagraph("MAKE CHECKS PAYABLE TO");
            paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetFont(FNT9B_B)
                .AddTextToParagraph(ps.CenterName + "\n" + ps.CenterAddress);
            paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginTop(20).SetFont(FNT8B)
                .AddTextToParagraph("CHANGE SERVICE REQUESTED");
            paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetFont(FNT9)
                .AddTextToParagraph("For Billing inquries: " +
                ps.CenterPhone + "\nPatent Name: " +  ps.CenterPatent);
        }

        private void BuildCut(SectionBuilder sectionBuilder)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetAlignment(HorizontalAlignment.Center)
                .SetFont(FNT9B).SetMarginTop(20)
                .AddTextToParagraph("Please retain statement for your records");
            sectionBuilder.AddLine(PageWidth, 0.5f, Stroke.Dashed);
        }
        private void BuildBeforeCut(SectionBuilder sectionBuilder)
        {
            var outerTableBuilder = sectionBuilder.AddTable();
            outerTableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 50)
                .AddColumnPercent("", 50);
            var rowBuilder = outerTableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetFont(FNT9).SetPadding(0, 8, 0, 0)
                .AddParagraphToCell("MESSAGES:\nWe have filed the medical claims with your insurance.They have\nindicated the balance is your responsibility. To pay your DIN online,\nplease visit www.ourwebsite.com.\n\nIf you have questions regarding your bill, or for payment arrangements, please call 123 - 456 - 78 or send an email inquiry to aboutmybill@ourwebsite.com");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder.AddTable(FillBottomBalanceTable);
        }

        private void FillBottomBalanceTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100)).SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0, 0, 0, 0)
                .AddColumnPercentToTable("", 74)
                .AddColumnPercent("", 26);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetPadding(4)
                .SetBackColor(MedicalBillBuilder.BLUE_COLOR)
                .SetFont(FNT9B_W)
                .AddParagraphToCell("Current Balance");
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                    .SetPadding(4)
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetBackColor(MedicalBillBuilder.BLUE_COLOR)
                    .SetFont(FNT9B_W)
                    .AddParagraphToCell(ps.Balance);
            rowBuilder = tableBuilder.AddRow();
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetColSpan(2)
                .SetFont(FNT9B_B)
                .AddParagraphToCell("This is your first notice for the visit above, which includes a list of itemized services rendered.");
            rowBuilder = tableBuilder.AddRow();
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .SetColSpan(2)
                .SetFont(FNT9)
                .AddParagraphToCell("We offer a Financial Aid program for qualified applicants. For more information, please call 123-456-7890 or visit our website at www.ourwebsite.com for more information.");
        }
    }
}