using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static TravelInsurance.TravelInsuranceFormBuilder;


namespace TravelInsurance
{
    internal class TravelInsuranceSectionCBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 2, 0);

            AddTitle(tableBuilder, "Section C - Luggage & Personal Effects", 2);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 7)
                        .AddParagraphToCell("Documents required for Section C\n• Police Report " +
                            "and original purchase receipts and/or warranty cards")
            .ToTable()
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 17)
                        .AddParagraph()
                            .AddText("Item", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(30, TabulationType.Left)
                            .AddText("Description")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph()
                            .AddText("When and where", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(25, TabulationType.Left)
                            .AddText("Original purchased", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddText("Depreciation of wear", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(75, TabulationType.Left)
                            .AddText("Amount Claimed")
                    .ToCell()
                        .AddParagraph()
                            .AddText("purchased", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(25, TabulationType.Left)
                            .AddText("price", addTabulationSymbol: true)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddText("and tear");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"", 2 }
                }, 10);

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"", 2 }
                }, 10);
        }
    }
}