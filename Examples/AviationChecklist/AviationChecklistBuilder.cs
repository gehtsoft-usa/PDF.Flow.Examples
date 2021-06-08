using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;

namespace AviationChecklist
{
    internal class AviationChecklistBuilder
    {
        
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8f);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT10BI = Fonts.Helvetica(10f)
            .SetBold().SetOblique();
        internal static readonly FontBuilder FNT12B = Fonts.Helvetica(12f).SetBold();
        internal static readonly FontBuilder FNT13 = Fonts.Helvetica(13f);
        internal static readonly FontBuilder FNT19B = Fonts.Helvetica(19f).SetBold();

        internal static Color DARKBLUE = Color.FromHtml("#000066");

        internal DocumentBuilder Build()
        {
            var document = DocumentBuilder.New();
            var section = document.AddSection();
            section.SetOrientation(PageOrientation.Portrait)
                   .SetSize(PaperSize.A4)
                   .SetMargins(29, 21, 29, 12);

            AviationChecklistHeaderBuilder.Build(section);
            AviationChecklistPage1Builder.Build(section);
            AviationChecklistPage2Builder.Build(section);
            AviationChecklistFooterBuilder.Build(section);

            return document;
        }

        internal static TableBuilder CreateMainTable(SectionBuilder section)
        {
            var mainTable = section.AddTable();

            mainTable
                .AddColumnPercentToTable("", 49)
                .AddColumnPercentToTable("", 2)
                .AddColumnPercentToTable("", 49)
                .SetBorderStroke(Stroke.None)
                .SetContentRowStyleFont(FNT10);

            return mainTable;
        }

        internal static void CreateSectionHeader(TableBuilder table, string title)
        {
            table
                .AddRow()
                    .AddCell(title)
                    .SetPadding(1, 0, 1, 0)
                    .SetBorderStroke(Stroke.Solid)
                    .SetBackColor(DARKBLUE)
                    .SetFont(FNT12B)
                    .SetFontColor(Color.White);
        }

        internal static TableCellBuilder CreateSectionCell(TableBuilder table)
        {
            var sectionCell = table.AddRow().AddCell();
            sectionCell
                .SetBorderStroke(Stroke.Solid)
                .SetPadding(1, 0, 0, 0);

            return sectionCell;
        }

        internal static void CreateChecklistItem(TableBuilder table, string itemName, 
            string itemValue, bool isFirst = false, bool isLast = false)
        {
            var sectionCell = table.AddRow().AddCell();

            if (isFirst == true)
            {
                sectionCell
                    .SetBorderWidth(0.5f, 0.5f, 0.5f, 0)
                    .SetPadding(1, 0, 0, 0);
            }
            else if (isLast == true)
            {
                sectionCell
                    .SetBorderWidth(0.5f, 0, 0.5f, 0.5f)
                    .SetPadding(1, 0, 0, 2);
            }
            else
            {
                sectionCell
                    .SetBorderWidth(0.5f, 0, 0.5f, 0)
                    .SetPadding(1, 0, 0, 0);
            }

            sectionCell
                .SetBorderStroke(Stroke.Solid)
                .AddParagraph()
                .SetLineSpacing(0.9f)
                .AddTextToParagraph(itemName, addTabulationSymbol: true)
                .AddTabulationInPercent(99, TabulationType.Right, TabulationLeading.DotBottom)
                .AddTextToParagraph(itemValue);
        }

        internal static void CreateBoldParagraph(TableBuilder table, string boldText, 
            bool isFirst = false, bool isLast = false)
        {
            var sectionCell = table.AddRow().AddCell();

            if (isFirst == true)
            {
                sectionCell.SetBorderWidth(0.5f, 0.5f, 0.5f, 0);
            }
            else if (isLast == true)
            {
                sectionCell.SetBorderWidth(0.5f, 0, 0.5f, 0.5f);
            }
            else
            {
                sectionCell.SetBorderWidth(0.5f, 0, 0.5f, 0);
            }

            sectionCell
                .SetBorderStroke(Stroke.Solid)
                .AddParagraph(boldText)
                .SetFont(FNT10BI)
                .SetAlignment(HorizontalAlignment.Center);
        }

        internal static void CreateEmptyCell(TableBuilder table)
        {
            table
                .AddRow()
                    .AddCell();
        }
    }
}
