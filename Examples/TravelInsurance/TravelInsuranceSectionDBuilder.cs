using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using static TravelInsurance.TravelInsuranceFormBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionDBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 2);

            AddTitle(tableBuilder, "Section D - Flight Delayed/Misconnection", 2);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 10, 0, 0)
                        .AddParagraphToCell("Documents required for Section D\n• letter from " +
                            "Airlines/Carrier stating the reason and duration of delay");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Date", 1 },
                    {"Date ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Time", 1 },
                    {"Time ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Place of Departure", 1 },
                    {"Place of Departure ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Flight No.", 1 },
                    {"Flight No. ", 1 }
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Name of Airline", 1 },
                    {"Name of Airline ", 1 }
                });
        }
    }
}