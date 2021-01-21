using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static TravelInsurance.TravelInsuranceFormBuilder;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceSectionFBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 2);

            AddTitle(tableBuilder, "Section F - Others", 2);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 6, 0, 0)
                        .AddParagraphToCell("In respect of any other claim, which does not fall " +
                            "within the sections stated above, please provide details of the " +
                            "claim you are submitting.  If the space below is insufficient for " +
                            "such details,");

            CreateRow(tableBuilder, new Dictionary<string, int>() { { "", 2 } }, 10);

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(0, 4, 0, 25)
                        .AddParagraph(SECTIONF_LONG_TEXT)
                            .SetLineSpacing(0.9f);

            sectionBuilder
                .AddTable()
                    .SetBorder(Stroke.None)
                    .AddColumnToTable("", 178)
                    .AddColumnToTable("", 104)
                    .AddColumnToTable("", 178)
                    .AddRow()
                        .AddCell()
                            .SetBorderWidth(0.58f)
                            .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.None)
                            .SetPadding(0, 2, 0, 16)
                            .AddParagraphToCell("Date")
                    .ToRow()
                        .AddCell()
                            .AddParagraphToCell("")
                    .ToRow()
                        .AddCell()
                            .SetBorderWidth(0.58f)
                            .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.None)
                            .SetPadding(0, 2, 0, 16)
                            .AddParagraphToCell("Signed here (Claimant)")
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .SetBorderWidth(0.58f)
                            .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.None)
                            .SetPadding(0, 2, 0, 16)
                            .AddParagraphToCell("Date")
                    .ToRow()
                        .AddCell()
                            .AddParagraphToCell("")
                    .ToRow()
                        .AddCell()
                            .SetBorderWidth(0.58f)
                            .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.None)
                            .SetPadding(0, 2, 0, 16)
                            .AddParagraphToCell("Signed here (Policyholder)")
            .ToSection()
                .AddTable()
                    .SetBorder(Stroke.Solid)
                    .AddColumnToTable("", 178)
                    .AddColumnToTable("", 187)
                    .AddColumnToTable("", 187)
                    .AddRow()
                        .AddCell()
                            .SetPadding(1, 10.5f, 0, 9.5f)
                            .SetBorderWidth(0, 2, 0.5f, 0)
                            .AddParagraphToCell("Particulars of Agent Name")
                    .ToRow()
                        .AddCell()
                            .SetPadding(6.5f, 10.5f, 0, 9.5f)
                            .SetBorderWidth(0, 2, 0.5f, 0)
                            .AddParagraphToCell("Mobile")
                    .ToRow()
                        .AddCell()
                            .SetPadding(4.5f, 10.5f, 0, 9.5f)
                            .SetBorderWidth(0, 2, 0, 0)
                            .AddParagraphToCell("Email Address");
        }
    }
}