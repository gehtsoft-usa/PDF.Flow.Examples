using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using System.IO;
using static AviationChecklist.AviationChecklistBuilder;

namespace AviationChecklist
{
    internal static class AviationChecklistHeaderBuilder
    {
        internal static void Build(SectionBuilder section)
        {
            section
                .AddHeaderToBothPages(50)
                    .AddTable()
                    .SetBorderStroke(Stroke.None)
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("", 60)
                    .AddColumnPercentToTable("", 40)
                        .AddRow()
                            .AddCell()
                                .AddParagraph("SamplePlane 123 - For Illustration Purposes")
                                .SetFont(FNT13)
                            .ToCell()
                                .AddParagraph("Normal Operating Checklist")
                                .SetFont(FNT19B)
                        .ToRow()
                            .AddCell()
                            .SetHorizontalAlignment(HorizontalAlignment.Right)
                                .AddImage(Path.Combine("images", "AC_Image.png"))
                                .SetScale(ScalingMode.UserDefined)
                                .SetWidth(200);
        }
    }
}