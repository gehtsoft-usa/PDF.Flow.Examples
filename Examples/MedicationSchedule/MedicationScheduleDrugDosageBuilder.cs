using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using MedicationSchedule.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static MedicationSchedule.MedicationScheduleBuilder;

namespace MedicationSchedule
{
    internal class MedicationScheduleDrugDosageBuilder
    {
        List<DrugInfoData> drugInfoData = JsonConvert.DeserializeObject<List<DrugInfoData>>
            (File.ReadAllText(Path.Combine("Content", "drug-info.json")));
        ScheduleInfoData scheduleInfoData = JsonConvert.DeserializeObject<ScheduleInfoData>
            (File.ReadAllText(Path.Combine("Content", "schedule-info.json")));

        internal readonly string checkmarkPath = 
            Path.Combine("images", "MedicationSchedule_Checkmark.png");
        internal readonly string emptymarkPath = 
            Path.Combine("images", "MedicationSchedule_Emptymark.png");

        internal void Build(SectionBuilder sectionBuilder)
        {
            var table = sectionBuilder.AddTable();
            var days = new List<string>() { "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa",
                                            "Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"};

            table.SetMarginTop(14)
                 .SetBorderStroke(Stroke.None)
                 .AddColumnPercentToTable("", 28.2f)
                 .AddColumnPercentToTable("", 6);

            for (int i = 0; i < 14; i++)
            {
                table.AddColumnPercentToTable("", 4.7f);
            };

            var headerRow = table.AddHeaderRow();
            headerRow.SetBorderStroke(Stroke.Solid)
                     .SetBorderWidth(0, 0, 0, 2)
                     .SetFont(FNT11B)
                     .AddCell("Drug & Dosage")
                         .SetPadding(0, 0, 0, 11)
            .ToRow()
                    .AddCellToRow("Time");

            foreach (var day in days)
            {
                headerRow.AddCell(day);
            };

            foreach (var drug in drugInfoData)
            {
                var drugTimeCount = 1;

                foreach (var drugTime in drug.Time)
                {
                    var contentRow = table.AddRow();
                    contentRow.SetFont(FNT9)
                              .SetBorderWidth(0, 0, 0, 0.5f)
                              .SetBorderStroke(Stroke.Solid);

                    if (drug.Time.Count == 1)
                    {
                        var cell = contentRow.AddCell();

                        cell.SetPadding(0, 3.5f, 0, 3)
                            .AddParagraph(drug.Name)
                                .SetFont(FNT12)
                                .SetLineSpacing(1.5f);

                        if (drug.Type != "")
                        {
                            cell.AddParagraph(drug.Type)
                                    .SetLineSpacing(1.1f);
                        }

                        cell.AddParagraph(drug.Dosage);
                    }
                    else if (drugTimeCount == 1)
                    {
                        contentRow.AddCell()
                                  .SetPadding(0, 3.5f, 0, 0)
                                  .SetBorderStroke(Stroke.None)
                                  .AddParagraph(drug.Name)
                                      .SetFont(FNT12);
                    }
                    else if (drugTimeCount == 2)
                    {
                        var cell = contentRow.AddCell();


                        if (drug.Type != "")
                        {
                            cell.AddParagraph(drug.Type)
                                    .SetLineSpacing(1.1f);
                        }

                        cell.AddParagraph(drug.Dosage);

                        if (drugTimeCount != drug.Time.Count)
                            cell.SetBorderStroke(Stroke.None);
                    }
                    else
                    {
                        var cell = contentRow.AddCell();
                        if (drugTimeCount != drug.Time.Count)
                            cell.SetBorderStroke(Stroke.None);
                    }


                    contentRow.AddCell(drugTime);
                    drugTimeCount++;

                    foreach (var take in drug.TakeDrug)
                    {
                        var cell = contentRow.AddCell();
                        cell.SetBorderWidth(0.5f, 0.5f, 0, 0.5f)
                            .SetPadding(0, 0, 0, 27);

                        if (!take)
                        {
                            cell.SetBackColor(Color.FromHtml("#D3D3D3"));
                        }
                    }
                }

            }

            sectionBuilder.AddTable()
                          .SetMarginTop(11)
                          .SetBorderStroke(Stroke.None)
                          .AddColumnPercentToTable("", 75)
                          .AddColumnPercentToTable("", 9)
                          .AddColumnPercentToTable("", 7)
                          .AddColumnPercentToTable("", 3)
                          .AddColumnPercentToTable("", 6)
                          .AddRow()
                              .SetFont(FNT9)
                              .AddCell()
                                  .SetPadding(223, 0, 0, 0)
                                  .AddParagraph(scheduleInfoData.Instructions)
                                      .SetLineSpacing(1)
                          .ToRow()
                              .AddCell()
                                  .AddImage(checkmarkPath)
                                  .SetAlignment(HorizontalAlignment.Right)
                          .ToRow()
                              .AddCell()
                                  .SetPadding(3, 0, 0, 0)
                                  .AddParagraph(scheduleInfoData.Take)
                          .ToRow()
                              .AddCell()
                                  .AddImage(emptymarkPath)
                                  .SetAlignment(HorizontalAlignment.Right)
                          .ToRow()
                              .AddCell()
                                  .SetPadding(3, 0, 0, 0)
                                  .AddParagraph(scheduleInfoData.Skip);
        }
    }
}