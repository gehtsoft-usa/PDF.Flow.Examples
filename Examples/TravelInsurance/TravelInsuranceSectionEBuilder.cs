using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using static TravelInsurance.TravelInsuranceFormBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionEBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 2);

            AddTitle(tableBuilder, "Section E - Baggage Delay", 2);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 5, 0, 6)
                        .AddParagraphToCell("Documents required for Section E\nBoarding Pass, " +
                            "Baggage Irregularity Report, Baggage acknowledgement slip and any" +
                            " other correspondence from the Airlines.");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Arrival Date", 1 },
                    {"Date ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Arrival Time", 1 },
                    {"Time ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Place of Departure", 1 },
                    {"Place", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Flight No.", 2 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Name of Airline", 2 }
                });
        }
    }
}