using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using MedicationSchedule.Model;
using Newtonsoft.Json;
using System.IO;
using static MedicationSchedule.MedicationScheduleBuilder;

namespace MedicationSchedule
{
    internal class MedicationScheduleFooterBuilder
    {

        FooterInfoData footerInfoData = JsonConvert.DeserializeObject<FooterInfoData>
            (File.ReadAllText(Path.Combine("Content", "footer-info.json")));
        internal void Build(SectionBuilder sectionBuilder)
        {
            sectionBuilder.AddFooterToBothPages(15)
                          .AddTable()
                              .SetBorderStroke(Stroke.None)
                              .AddColumnPercentToTable("", 46)
                              .AddColumnPercentToTable("", 20)
                              .AddColumnPercentToTable("", 17)
                              .AddColumnPercentToTable("", 17)
                              .AddRow()
                                  .SetFont(FNT9)
                                  .AddCell()
                                      .AddParagraph()
                                          .AddPageNumber("Page ")
                              .ToRow()
                                  .AddCell()
                                      .AddParagraph(footerInfoData.Treatment)
                                          .AddTabSymbol()
                                          .AddTabulationInPercent(42)
                                          .AddTextToParagraph("Form")
                                          .AddTabSymbol()
                                          .AddTabulationInPercent(60)
                                          .AddTextToParagraph(footerInfoData.Form)
                              .ToRow()
                                  .AddCell()
                                      .AddParagraph("Permanent Record ")
                                          .SetAlignment(HorizontalAlignment.Right)
                                           .AddTextToParagraph(footerInfoData.PermanentRecord)
                              .ToRow()
                                  .AddCell()
                                      .AddParagraph("Medication Record ")
                                          .SetAlignment(HorizontalAlignment.Right)
                                          .AddTextToParagraph(footerInfoData.MedicationRecord);
        }
    }
}