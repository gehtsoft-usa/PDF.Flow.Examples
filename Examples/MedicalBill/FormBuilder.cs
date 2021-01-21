using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using static MedicalBill.MedicalBillBuilder;
using Gehtsoft.PDFFlow.Utils;

namespace MedicalBill
{
    internal class FormBuilder
    {
        protected void AddRadioButtons(TableCellBuilder cellBuilder, string label, 
            string[] choices, int[] widths, bool bottomBorder = false)
        {
            string[] labels = choices;
            if (label != null)
            {
                labels = new string[choices.Length + 1];
                labels[0] = label;
                for (int i = 1, l = labels.Length; i < l; i++)
                {
                    labels[i] = choices[i - 1];
                }
            }
            int wl = widths.Length;
            if (labels.Length > 0 && wl > 0)
            {
                int lastWidth = widths[widths.Length - 1];
                cellBuilder.AddTable(tableBuilder =>
                {
                    tableBuilder
                        .SetWidth(XUnit.FromPercent(100))
                        .SetBorder(Stroke.None);
                        
                    for (int i = 0, l = labels.Length; i < l; i++)
                    {
                        tableBuilder.AddColumnPercent("", i < wl ? widths[i] : lastWidth);
                    }
                    var rowBuilder = tableBuilder.AddRow();
                    rowBuilder
                        .SetBorder(borderBuilder =>
                        {
                            borderBuilder
                                .SetRightWidth(0)
                                .SetTopBorder(0.5f, Stroke.Solid, null)
                                .SetLeftWidth(0);
                            if (bottomBorder)
                            {
                                borderBuilder.SetTopBorder(0.5f, Stroke.Solid, null);
                            }
                            else
                            {
                                borderBuilder.SetBottomWidth(0);
                            }
                        });
                    for (int i = 0, l = labels.Length; i < l; i++)
                    {
                        AddRadioCellsToRow(rowBuilder, labels, i);
                    }
                });
            }
        }

        private void AddRadioCellsToRow(TableRowBuilder rowBuilder, string[] labels, int i)
        {
            var cellBuilder = rowBuilder.AddCell();
            string text = labels[i];
            if (i == 0)
            {
                cellBuilder.AddParagraph(text).SetFont(FNT7);
            }
            else
            {
                cellBuilder.AddParagraph("o").SetFont(FNTZ12);
                cellBuilder.AddParagraph(text).SetFont(FNT10);
            }
        }

        protected void AddForm(TableCellBuilder cellBuilder, string[] labels, 
            int[] widths, bool bottomBorder = false, 
            string[] values = null, float topBorderDepth = 0.5f, 
            float bottomBorderDepth = 0.5f, float valueFontSize = 12f)
        {
            int wl = widths.Length;
            if (labels.Length > 0 && wl > 0)
            {
                int lastWidth = widths[widths.Length - 1];
                cellBuilder.AddTable(tableBuilder =>
                {
                    tableBuilder
                        .SetWidth(XUnit.FromPercent(100))
                        .SetBorder(Stroke.None);
                    for (int i = 0, l = labels.Length; i < l; i++)
                    {
                        tableBuilder.AddColumnPercent("", i < wl ? widths[i] : lastWidth);
                    }
                    var rowBuilder = tableBuilder.AddRow();
                    rowBuilder
                        .SetBorder(borderBuilder =>
                        {
                            borderBuilder
                                .SetRightWidth(0)
                                .SetTopBorder(topBorderDepth, Stroke.Solid, null)
                                .SetLeftWidth(0);
                            if (bottomBorder)
                            {
                                borderBuilder.SetBottomBorder(bottomBorderDepth, 
                                    Stroke.Solid, null);
                            }
                            else
                            {
                                borderBuilder.SetBottomWidth(0);
                            }
                        });
                    for (int i = 0, l = labels.Length; i < l; i++)
                    {
                        AddCellsToFormRow(rowBuilder, labels, values, valueFontSize, i);
                    }
                });
            }
        }

        private void AddCellsToFormRow(TableRowBuilder rowBuilder, string[] labels, 
            string[] values, float valueFontSize, int i)
        {
            var valueFont = Fonts.Helvetica(values == null ? 8f : valueFontSize);
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder.AddParagraph(labels[i]).SetMarginTop(0).SetFont(FNT7);
            var text = values != null && i < values.Length ? values[i] : " ";
            cellBuilder.AddParagraph(text).SetFont(valueFont);
        }
    }
}