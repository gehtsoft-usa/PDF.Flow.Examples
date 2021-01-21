using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static TravelInsurance.TravelInsuranceFormBuilder;
using static TravelInsurance.TravelInsuranceBuilder;

namespace TravelInsurance
{
    internal class TravelInsuranceGeneralInfoBuilder
    {
        internal void Build(SectionBuilder sectionBuilder)
        {
            var tableBuilder = CreateTable(sectionBuilder, 3);

            AddTitle(tableBuilder, "General Information");

            tableBuilder
                .AddRow()
                    .AddCell()
                        .SetColSpan(3)
                        .SetPadding(0, 7)
                        .AddParagraphToCell("Documents required\nFor all travel claims please " +
                            "submit air tickets and boarding pass.\nFor annual plans, please " +
                            "provide a copy of the passport showing duration of trip.");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Policyholder", 1 }, 
                    {"Claimant (if it differs from the above)", 1 },
                    {"Insurance Policy No.", 1}
                });

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Address", 3 }
                });

            tableBuilder
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 17)
                        .AddParagraph("Occupation")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 0)
                        .AddParagraph("Date of Birth")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 0)
                        .AddParagraphToCell("Sex")
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Male", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Female")
                                .SetFont(FNT12);

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"TelephoneNo.", 1 },
                    {"HP No.", 1 },
                    {"Email Address:", 1}
                });

            tableBuilder
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph("Travel companion(s) is/are insured" +
                                      "\nWith AHA? If yes, please provide details.")
                .ToRow()
                    .AddCell()
                        .SetColSpan(2)
                        .SetPadding(1, 10, 0, 0)
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Yes", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(25, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" No")
                                .SetFont(FNT12)
            .ToTable()
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
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
                        .SetPadding(1, 1, 0, 17)
                        .AddParagraph("Registration No.")
                .ToRow()
                    .AddCell()
                        .SetPadding(1, 1, 0, 0)
                        .AddParagraphToCell("Purpose of Trip")
                        .AddParagraph()
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Business", addTabulationSymbol: true)
                                .SetFont(FNT12)
                        .ToParagraph()
                            .AddTabulationInPercent(50, TabulationType.Left)
                            .AddInlineImage(CheckboxPath)
                                .SetSize(16, 16)
                        .ToParagraph()
                            .AddText(" Vacation")
                                .SetFont(FNT12);

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Place where accident, loss or illness occurred", 1 },
                    {"Time", 1 },
                    {"Date", 1}
                });

            tableBuilder
                .AddRow()
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(Stroke.Solid)
                    .AddCell()
                        .SetPadding(1, 1, 0, 10)
                        .AddParagraph("Are there any other Policies of insurance in force " +
                                      "covering\nyou in respect of this event?")
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
                        .AddParagraph("If yes, please specify:");

            CreateRow(tableBuilder, new Dictionary<string, int>()
                {
                    {"Description of the incident, loss or illness", 3 }
                });
        }

        
    }
}