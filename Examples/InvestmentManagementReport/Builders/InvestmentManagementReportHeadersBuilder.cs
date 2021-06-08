using System.IO;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;
using InvestmentManagementReport.Model;

namespace InvestmentManagementReport
{
    internal static class InvestmentManagementReportHeadersBuilder
    {
        private static readonly FontBuilder FNT_17 = Fonts.Helvetica(17f);
        private static readonly FontBuilder FNT_17B = Fonts.Helvetica(17f).SetBold();
        private static readonly FontBuilder FNT_10 = Fonts.Helvetica(10f);
        private static readonly FontBuilder FNT_8 = Fonts.Helvetica(8f);
        private static readonly FontBuilder FNT_8B = Fonts.Helvetica(8f).SetBold();
        private static readonly FontBuilder FNT_7_5I = Fonts.Helvetica(7.5f).SetOblique();
        private static readonly FontBuilder FNT_7_75 = Fonts.Helvetica(7.75f);
        private static readonly Color GreenColor = Color.FromHtml("#3F7600");
        
        public static void AddRepeatingAreas(this SectionBuilder sectionBuilder, InvestmentManagementReportHeaders headers)
        {
            sectionBuilder.AddHeaderToBothPages(80f).AddHeader(headers);
            sectionBuilder.AddFooterToBothPages(20f).AddFooter(headers);
        }

        private static void AddHeader(this RepeatingAreaBuilder areaBuilder, InvestmentManagementReportHeaders headers)
        {
            TableRowBuilder rowBuilder = areaBuilder
                .AddTable()
                .AddColumnToTable("", 63f)
                .AddColumnToTable("", 310f)
                .AddColumnPercentToTable("", 100f)
                .SetBorderStroke(Stroke.None)
                .SetBorderWidth(1f)
                .SetMargins(9f)
                    .AddRow()
                    .SetHeight(45f);
            // Add logo image
            rowBuilder
                .AddCell()
                .SetHorizontalAlignment(HorizontalAlignment.Center)
                .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddImage(Path.Combine(Directory.GetCurrentDirectory(), "Images", "InvestmentManagementReportLogo.png"), 45f, 45f, ScalingMode.UserDefined);
            // Add company name and document name
            rowBuilder
                .AddCell()
                .SetVerticalAlignment(VerticalAlignment.Center)
                .SetPadding(3f, 0f, 0f, 0f)
                    .AddParagraph(headers.CompanyName)
                    .SetFont(FNT_17B)
                .ToCell()
                    .AddParagraph(headers.DocumentName)
                    .SetFont(FNT_17);
            // Add trust block
            rowBuilder
                .AddCell()
                .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddTable()
                    .AddTrustBlock(headers);

            // Add bottom separator line to the header
            areaBuilder.AddLine(720f, 1f);
        }

        private static void AddTrustBlock(this TableBuilder tableBuilder, InvestmentManagementReportHeaders headers)
        {
            TableRowBuilder rowBuilder = tableBuilder
                .SetBorderStroke(Stroke.Solid, Stroke.None, Stroke.None, Stroke.None)
                .SetBorderWidth(1f)
                .AddColumnToTable("", 171f)
                .AddColumnToTable("", 90f)
                .AddColumnPercentToTable("", 100f)
                    .AddRow()
                    .SetHeight(36f);
            // Add name and U/A DTD block
            rowBuilder
                .AddCell()
                .SetPadding(8.5f, 0f, 0f, 0f)
                .SetVerticalAlignment(VerticalAlignment.Bottom)
                .SetFont(FNT_10)
                .AddParagraphToCell(headers.FullNameShort)
                .AddParagraphToCell(headers.FullNameTrust)
                .AddParagraphToCell(headers.UnderAgreementDated);
            // Add account number block
            rowBuilder
                .AddCell()
                .SetPadding(8.5f, 0f, 0f, 0f)
                    .AddParagraph("Account Number")
                    .SetFont(FNT_8)
                    .SetMarginTop(2.5f)
                .ToCell()
                    .AddParagraph(headers.AccountNumber)
                    .SetFont(FNT_8B)
                    .SetMarginTop(1f);
            // Add statement period block
            rowBuilder
                .AddCell()
                .SetPadding(8.5f, 2f, 0f, 0f)
                    .AddParagraph("Statement Period")
                    .SetFont(FNT_8)
                    .SetMarginBottom(1f)
                .ToCell()
                    .AddParagraph(headers.StatementPeriodStart)
                    .SetFont(FNT_8B)
                .ToCell()
                    .AddParagraph(headers.StatementPeriodEnd)
                    .SetFont(FNT_8B);
        }

        private static void AddFooter(this RepeatingAreaBuilder areaBuilder, InvestmentManagementReportHeaders headers)
        {
            areaBuilder.AddLine(720f, 2f, Stroke.Solid, GreenColor).SetMarginBottom(4f);
            areaBuilder
                .AddTable()
                .SetBorderStroke(Stroke.None)
                .AddColumnPercentToTable("", 100f)
                .AddColumnToTable("", 35f)
                    .AddRow()
                    .SetVerticalAlignment(VerticalAlignment.Bottom)
                        .AddCell()
                        .SetFont(FNT_7_5I)
                        .AddParagraphToCell(headers.FooterText)
                    .ToRow()
                        .AddCell()
                        .SetHorizontalAlignment(HorizontalAlignment.Right)
                        .SetFont(FNT_7_75)
                            .AddParagraph()
                            .AddPageNumberToParagraph();
        }
    }
}