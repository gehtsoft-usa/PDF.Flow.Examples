using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using static TravelInsurance.TravelInsuranceFormBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionBBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 3);

            AddTitle(tableBuilder, "Section B - Cancellation/Curtailment/Postponement");

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(3)
                        .SetPadding(0, 2, 0, 5)
                        .AddParagraphToCell("Documents required for Section B\n• documents from " +
                            "carrier/travel agent and any relevant documents to support your claim");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"When and where was the trip booked?", 1 },
                    {"Intended Departure Date", 1 },
                    {"Date of cancellation ", 1}
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Why was the trip cancelled?", 3 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Amount paid by you", 1 },
                    {"Amount recovered from other sources", 1 },
                    {"Amount Claimed", 1}
                });
        }
    }
}