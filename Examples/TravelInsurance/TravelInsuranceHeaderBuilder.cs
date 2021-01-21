using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceHeaderBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            sectionBuilder
                .AddTable()
                    .SetContentRowStyleFont(FNT20B)
                    .SetBorder(Stroke.None)
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("", 50)
                    .AddColumnPercentToTable("", 50)
                    .AddRow()
                        .AddCell()
                            .SetPadding(2, 4, 0, 0)
                            .AddParagraph("Travel Insurance")
                                .SetLineSpacing(0.8f)
                                .AddText("\nClaim Form")
                                    .SetFontColor(Color.Gray)
                    .ToRow()
                        .AddCell()
                            .AddImage(LogoPath)
                            .SetAlignment(HorizontalAlignment.Right)
                            .SetScale(ScalingMode.UserDefined)
                            .SetWidth(250)
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .SetColSpan(2)
                            .SetPadding(0, 13, 0, 8)
                            .SetFont(FNT7)
                            .AddParagraphToCell("PLEASE COMPLETE ALL SECTIONS TO FACILITATE " +
                                                "THE PROCESSING OF YOUR APPLICATION ")
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .SetColSpan(2)
                            .SetBorder(Stroke.Solid, Color.Black, 0.5f)
                            .SetPadding(8)
                            .SetFont(FNT8)
                            //.AddParagraph()
                            //    .AddTextToParagraph(HEADER_BOX_TEXT);
                            .AddParagraphToCell(HEADER_BOX_TEXT);

        }
    }
}
