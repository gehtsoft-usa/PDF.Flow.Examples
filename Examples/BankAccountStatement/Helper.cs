using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using System;
using System.Globalization;
using static BankAccountStatement.BankAccountStatementBuilder;

namespace BankAccountStatement
{
    public static class Helper
    {
        public static string ReplaceNullDate(DateTime date, string format, string forNull)
        {
            return date != null ? date.ToString(format,
                                                DocumentLocale)
                                : forNull;
        }

        public static string ReplaceNullStr(string value, string forNull)
        {
            return value != null ? value : forNull;
        }

        public static string ReplaceZeroDouble(double value, string forZero, 
            string currencyPrefix = "")
        {
            return !Double.IsNaN(value) && 0.0 != value ? 
                currencyPrefix + DoubleToString(value) : 
                forZero;
        }

        public static string ReplaceZeroInt(int value, string forZero)
        {
            return 0 != value ? value.ToString() : forZero;
        }

        public static ParagraphBuilder AddFormRow(TableCellBuilder cellBuilder, 
            string text = "$")
        {
            return cellBuilder.AddParagraph()
                .AddTabSymbol()
                .SetFont(FNT7_2)
                .AddTabulation(30, TabulationType.Right, TabulationLeading.None)
                .AddTextToParagraph(text, addTabulationSymbol: true)
                .AddTabulation(96, TabulationType.Right, TabulationLeading.BottomLine);
        }
        public static TableCellBuilder AddFormRowBox(TableCellBuilder outerCellBuilder, 
            string text = "$")
        {
            return outerCellBuilder.AddTableToCell(tableBuilder => 
            {
                tableBuilder
                    .SetWidth(XUnit.FromPercent(76))
                    .SetContentRowStyleMinHeight(25)
                    .SetAlignment(HorizontalAlignment.Right)
                    .AddColumnPercentToTable("", 100)
                    .SetBorderStroke(Stroke.Solid)
                    .SetBorderColor(Color.Black)
                    .SetBorderWidth(1.5f);
                var cellBuilder = tableBuilder.AddRow().SetMinHeight(25).AddCell();
                cellBuilder
                    .SetVerticalAlignment(VerticalAlignment.Bottom)
                    .SetPadding(0, 0, 0, 4)
                    .AddParagraph()
                    .AddTabSymbol()
                    .SetFont(FNT7_2)
                    .AddTabulation(6, TabulationType.Right, TabulationLeading.None)
                    .AddTextToParagraph(text, addTabulationSymbol: true)
                    .AddTabulation(72, TabulationType.Right, TabulationLeading.BottomLine);
            });
        }

        public static void AddParagraph(SectionBuilder sectionBuilder, string text, 
            FontBuilder paragrpaphFont, float bottomMagin = 0.0f)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder.AddTextToParagraph(text)
                .SetMarginBottom(bottomMagin)
                .SetFont(paragrpaphFont);
        }


        public static void AddParagraph(SectionBuilder sectionBuilder, float topMagin, 
            string text, FontBuilder paragrpaphFont, float bottomMagin = 0.0f)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder.AddTextToParagraph(text)
                .SetMarginTop(topMagin)
                .SetMarginBottom(bottomMagin)
                .SetFont(paragrpaphFont);
        }

        public static void AddParagraph(TableCellBuilder cellBuilder, string text,
            FontBuilder paragrpaphFont, float bottomMagin = 0.0f)
        {
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder.AddTextToParagraph(text)
                .SetMarginBottom(bottomMagin)
                .SetFont(paragrpaphFont);
        }

        public static void AddParagraph(TableCellBuilder cellBuilder, float topMagin, 
            string text,
            FontBuilder paragrpaphFont, float bottomMagin = 0.0f)
        {
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder.AddTextToParagraph(text)
                .SetMarginTop(topMagin)
                .SetMarginBottom(bottomMagin)
                .SetFont(paragrpaphFont);
        }

        public static void AddParagraph(RepeatingAreaBuilder builder, string text,
            FontBuilder paragrpaphFont, float bottomMagin = 0.0f)
        {
            var paragraphBuilder = builder.AddParagraph();
            paragraphBuilder.AddTextToParagraph(text)
                .SetMarginBottom(bottomMagin)
                .SetFont(paragrpaphFont);
        }

        public static void AddParagraph(RepeatingAreaBuilder builder, float topMagin, 
            string text,
            FontBuilder paragrpaphFont, float bottomMagin = 0.0f)
        {
            var paragraphBuilder = builder.AddParagraph();
            paragraphBuilder.AddTextToParagraph(text)
                .SetMarginTop(topMagin)
                .SetMarginBottom(bottomMagin)
                .SetFont(paragrpaphFont);
        }

        public static void AddNumberedListToParagraph(RepeatingAreaBuilder builder, 
            string[] items, FontBuilder font, int leftMargin)
        {
            foreach (String text in items)
            {
                var paragraphBuilder = builder.AddParagraph();
                paragraphBuilder
                    .SetMarginLeft(leftMargin)
                    .SetFont(font)
                    .SetListNumbered(NumerationStyle.Arabic)
                    .AddTextToParagraph(text);
            }
        }
        private static string DoubleToString(double value)
        {
            if (value < 10)
            {
                return String
                    .Format(CultureInfo.InvariantCulture, "{0:,0.00}", value);
            }
            return String
                .Format(CultureInfo.InvariantCulture, "{0:0,0.00}", value);
        }

    }
}