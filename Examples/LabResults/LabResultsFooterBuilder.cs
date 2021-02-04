using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using LabResults.Model;
using Newtonsoft.Json;
using System.IO;
using static LabResults.LabResultsBuilder;

namespace LabResults
{
    internal class LabResultsFooterBuilder
    {
        ReportInfoData reportInfoData = JsonConvert.DeserializeObject<ReportInfoData>
            (File.ReadAllText(Path.Combine("Content", "report-info.json")));

        internal void Build(SectionBuilder sectionBuilder, int pageCount)
        {
            sectionBuilder
                .AddFooterToBothPages(45)
                    .AddLine(PageWidth, 2)
                .ToArea()
                    .AddTable()
                        .SetBorderStroke(Stroke.None)
                        .SetWidth(XUnit.FromPercent(100))
                        .AddColumnPercentToTable("", 65)
                        .AddColumnPercentToTable("", 35)
                        .AddRow()
                            .AddCell()
                                .SetPadding(0, 1, 0, 0)
                                .AddParagraph("FINAL REPORT")
                                    .SetMarginTop(1)
                                    .SetFont(FNT10B)
                                    .AddTabSymbol()
                                    .AddTabulationInPercent(62)
                                    .AddText("Date Issued: " + reportInfoData.Date)
                                        .SetFont(FNT8)
                         .ToRow()
                            .AddCell()
                                .AddParagraph()
                                    .SetFont(FNT8)
                                    .SetAlignment(HorizontalAlignment.Right)
                                    .AddPageNumber("Page ")
                                .ToParagraph()
                                    .AddText(" of " + pageCount)
                    .ToTable()
                        .AddRow()
                            .AddCell()
                                .SetFont(FNT7Half)
                                .AddParagraph("This document contains private and confidential " +
                                        "health information protected by state and federal law.")
                                    .SetMarginTop(3)
                                    .SetLineSpacing(1)
                             .ToCell()
                                .AddParagraph("If you have received this document in error, " +
                                        "please call " + reportInfoData.Phone)
                        .ToRow()
                            .AddCell()
                                .SetHorizontalAlignment(HorizontalAlignment.Right)
                                .SetFont(FNT7Half)
                                .AddParagraph(reportInfoData.Copyright)
                            .ToCell()
                                .AddParagraph(reportInfoData.Version);

        }
    }
}