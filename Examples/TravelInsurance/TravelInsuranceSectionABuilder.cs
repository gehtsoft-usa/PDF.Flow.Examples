using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static TravelInsurance.TravelInsuranceFormBuilder;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionABuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 3);

            AddTitle(tableBuilder, "Section A -  Personal Accident/Illness – " +
                "Medical And Additional Expenses");

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(3)
                        .SetPadding(0, 2, 0, 5)
                        .AddParagraphToCell("Documents required for Section A\n• original " +
                            "medical receipts and copy of discharge summary or available " +
                            "medical report")
            .ToTable()
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph("Have you suffered this illness or injury or a similar " +
                            "condition or a recurrence of a previous illness or injury? ")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 10, 0, 0)
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Yes", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" No")
                                .SetFont(FNT12)
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph("If yes, please specify:")
            .ToTable()
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 0)
                        .AddParagraphToCell("State amount claimed:")
                        .AddParagraph("$")
                            .SetFont(FNT12)
                .ToRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(1, 1, 0, 17)
                        .AddParagraph("Give name and address of your usual attending Doctor");
        }
    }
}