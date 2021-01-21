using System;
using System.IO;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using static RentalAgreement.RentalAgreementBuilder;

namespace RentalAgreement
{
    internal class RentalAgreementHelper
    {
        internal static void BuildHeaderWithBar(RepeatingAreaBuilder builder, 
            float pageWidth)
        {
            var tableBuilder = builder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 50)
                .AddColumnPercentToTable("", 50);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.AddCell()
                .AddImage(Path.Combine("images", "ra-logo-2x.png"),
                    XSize.FromHeight(120));
            rowBuilder.AddCell()
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .AddImage(Path.Combine("images", "ra-barcode.png"),
                    XSize.FromHeight(120));
            builder.AddParagraph()
                .SetAlignment(HorizontalAlignment.Right)
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFont(URL_FONT))
                .AddUrlToParagraph("http://www.bestlandlords.com/billing");
            builder.AddLine(pageWidth, 1.5f, Stroke.Solid, Color.Gray)
                .SetMarginTop(5);

        }

        internal static void BuildHeader(RepeatingAreaBuilder builder, 
            float pageWidth)
        {
            builder
                .AddImage(Path.Combine("images", "ra-logo-2x.png"),
                    XSize.FromHeight(60));
            builder.AddLine(pageWidth, 1.5f, Stroke.Solid, Color.Gray)
                .SetMarginTop(5);

        }

        internal static void BuildFooterEqualHousingOpportunity(
            RepeatingAreaBuilder builder)
        {
            builder
                .AddImage(Path.Combine("images",
                            "equal-housing-opportunity-logo-1200w.png"),
                    XSize.FromHeight(80))
                .SetAlignment(HorizontalAlignment.Left);
            BuildFooter(builder);
        }

        internal static void BuildFooterBar(RepeatingAreaBuilder builder, float barImageHeight)
        {
            var tableBuilder = builder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 40)
                .AddColumnPercentToTable("", 42)
                .AddColumnPercentToTable("", 10)
                .AddColumnPercentToTable("", 8);
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder.SetVerticalAlignment(VerticalAlignment.Bottom);
            rowBuilder.AddCell()
                .SetPadding(40, 0, 0, 0)
                .SetFont(FNT11)
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .AddParagraph()
                .SetAlignment(HorizontalAlignment.Left)
                .AddTextToParagraph("Initials ", INITALS_FONT,
                    true)
                .AddTabulation(140, TabulationType.Left,
                    TabulationLeading.BottomLine);
            rowBuilder
                .AddCell()
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(0, 0, 5, 0)
                .AddParagraph()
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFontColor(Color.Red)
                        .SetFont(URL_FONT))
                .AddUrlToParagraph("http://www.bestlandlords.com/billing");
            rowBuilder
                .AddCell()
                .AddImage(Path.Combine("images", "ra-barcode.png"),
                    XSize.FromHeight(barImageHeight));
            rowBuilder.AddCell().AddParagraph()
                .SetAlignment(HorizontalAlignment.Right)
                .AddTextToParagraph(" ", PAGE_NUMBER_FONT, true)
                .AddTextToParagraph("Page ", PAGE_NUMBER_FONT)
                .AddPageNumber().SetFont(PAGE_NUMBER_FONT);
        }

        internal static void BuildFooter(RepeatingAreaBuilder builder)
        {
            ParagraphBuilder paragraphBuilder = builder
                .AddParagraph();
            paragraphBuilder
                .SetAlignment(HorizontalAlignment.Right)
                .AddTextToParagraph(" ", PAGE_NUMBER_FONT, true)
                .AddTextToParagraph("Page ", PAGE_NUMBER_FONT)
                .AddPageNumber().SetFont(PAGE_NUMBER_FONT);
        }

        internal static void BuildEqualHousingOpportunity(RepeatingAreaBuilder builder,
            float pageHeight)
        {
            builder
                .AddImage(Path.Combine("images",
                    "equal-housing-opportunity-logo-160w.png"),
                    XSize.FromHeight(64))
                    .SetMarginTop(pageHeight - 66f);
        }

        internal static string DateToString(DateTime date)
        {
            return date.ToString(
                        "MMMM dd yyyy", DocumentLocale);
        }

        internal static string FundToString(double fund)
        {
            return "$" + String.Format(
                DocumentLocale, "{0:0,0.00}", fund);
        }

        internal static string PercentsToString(double procents)
        {
            if (procents < 10)
            {
                return
                    String.Format(DocumentLocale, "{0:,0.00}", procents) 
                        + "%";
            }
            return String.Format(DocumentLocale, "{0:0,0.00}", procents) 
                    + "%";
        }

    }
}