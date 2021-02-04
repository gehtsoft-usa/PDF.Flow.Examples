using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using LabResults.Model;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using static LabResults.LabResultsBuilder;

namespace LabResults
{
    internal class LabResultsTestDetailsBuilder
    {
        LabInfoData labInfoData = JsonConvert.DeserializeObject<LabInfoData>
            (File.ReadAllText(Path.Combine("Content", "lab-info.json")));

        internal void Build(SectionBuilder sectionBuilder, TestDetailsData testData)
        {
            var resultColor = Color.FromHtml("#417505");

            if (testData.ClinicalInfo == "INDETERMINATE" || testData.ClinicalInfo == "INSUFFICIENT")
                resultColor = Color.FromHtml("#F5A623");
            else if (testData.ClinicalInfo == "DETECTED" || testData.ClinicalInfo == "HIGH")
                resultColor = Color.Red;

            sectionBuilder
                .AddParagraph("General Comments & Additional Information")
                    .SetMarginTop(14)
                    .SetFont(FNT9B)
                    .SetFontColor(blueGreen)
                    .AddTabSymbol()
                    .AddTabulationInPercent(51)
                    .AddText("Ordered Items")
            .ToSection()
                .AddParagraph("Clinical Info:")
                    .SetFont(FNT9B)
                    .AddTabSymbol()
                    .AddTabulationInPercent(10)
                    .AddText(testData.ClinicalInfo)
                        .SetFontColor(resultColor)
                .ToParagraph()
                    .AddTabSymbol()
                    .AddTabulationInPercent(51)
                    .AddText(testData.OrderedItems)
                        .SetFont(FNT9)
            .ToSection()
                .AddTable()
                    .SetMarginTop(11)
                    .SetBorderWidth(0, 0, 0, 1)
                    .SetBorderColor(blueGreen)
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("TESTS", 22)
                    .AddColumnPercentToTable("RESULT", 22)
                    .AddColumnPercentToTable("FLAG", 12)
                    .AddColumnPercentToTable("UNITS", 9)
                    .AddColumnPercentToTable("REFERENCE INTERVAL", 29)
                    .AddColumnPercentToTable("LAB", 6)
                    .SetHeaderRowStyleBackColor(blueGreen)
                    .SetHeaderRowStyleHorizontalAlignment(HorizontalAlignment.Center)
                    .SetHeaderRowStyleFont(FNT10.SetColor(Color.White))
                    .AddRow()
                        .SetFont(FNT11)
                        .SetHorizontalAlignment(HorizontalAlignment.Center)
                        .AddCellToRow(testData.Tests)
                        .AddCellToRow(testData.Result)
                        .AddCellToRow(testData.Flag)
                        .AddCellToRow(testData.Units)
                        .AddCellToRow(testData.ReferenceInterval)
                        .AddCellToRow(testData.Lab)
            .ToSection()
                .AddParagraph()
                    .SetMarginTop(12)
                    .SetFont(FNT11)
                    .SetLineSpacing(1)
                    .AddTextToParagraph(testData.Description)
            .ToSection()
                .AddLine(PageWidth, 1)
                    .SetMarginTop(17)
            .ToSection()
                .AddParagraph(labInfoData.Index)
                    .SetFont(FNT9)
                    .SetMarginLeft(30)
                    .SetLineSpacing(0.8f)
                    .AddTabSymbol()
                    .AddTabulationInPercent(5)
                    .AddText(labInfoData.Code)
                .ToParagraph()
                    .AddTabSymbol()
                    .AddTabulationInPercent(12)
                    .AddText(labInfoData.Name)
                .ToParagraph()
                    .AddTabSymbol()
                    .AddTabulationInPercent(61)
                    .AddText(labInfoData.Director)
            .ToSection()
                .AddParagraph(labInfoData.Address)
                    .SetFont(FNT9)
                    .SetMarginLeft(91.5f)
            .ToSection()
                .AddLine(PageWidth, 1)
            .ToSection()
                .AddParagraph("For inquiries, the physician may contact branch: " +
                            labInfoData.BranchPhone + " Lab: " + labInfoData.LabPhone)
                    .SetFont(FNT7)
                    .SetMarginTop(3)
                    .SetAlignment(HorizontalAlignment.Right);

        }
    }
}