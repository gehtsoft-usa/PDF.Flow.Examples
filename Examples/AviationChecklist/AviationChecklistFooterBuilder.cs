using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using System;
using static AviationChecklist.AviationChecklistBuilder;

namespace AviationChecklist
{
    internal static class AviationChecklistFooterBuilder
    {
        internal static void Build(SectionBuilder section)
        {
            section.AddFooterToBothPages(30)
                   .AddTable()
                       .SetWidth(XUnit.FromPercent(100))
                       .AddColumnPercentToTable("", 33)
                       .AddColumnPercentToTable("", 33)
                       .AddColumnPercentToTable("", 34)
                       .AddRow()
                           .SetBorderWidth(0, 2, 0, 0)
                           .SetFont(FNT8)
                           .AddCellToRow("Version 1.02 March 2021")
                           .AddCell("Copyright © 2021 gsairlines.com")
                               .SetHorizontalAlignment(HorizontalAlignment.Center)
                       .ToRow()
                           .AddCell()
                               .SetHorizontalAlignment(HorizontalAlignment.Right)
                               .AddParagraph()
                                   .AddPageNumberToParagraph("Page ");
        }
    }
}