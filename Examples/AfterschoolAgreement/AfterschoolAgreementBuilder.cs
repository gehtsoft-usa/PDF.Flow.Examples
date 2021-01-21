using System.IO;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;

namespace AfterschoolAgreement
{
    public static class AfterschoolAgreementBuilder
    {
        public static DocumentBuilder AddAfterschoolAgreement(this DocumentBuilder builder)
        {
            var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", "AfterschoolAgreement");
            var checkboxUrl = Path.Combine(imageDir, "Checkbox.png");
            var logoUrl = Path.Combine(imageDir, "SchoolLogo.jpg");
            builder.AddSectionToDocument(s =>
            {
                s.SetSize(PaperSize.Letter).SetOrientation(PageOrientation.Portrait);

                s.AddTable((TableBuilder table) =>
                {
                    table.SetContentRowStyleBorder(borderBuilder => borderBuilder.SetStroke(Stroke.None));

                    table.AddColumn("", null, c =>
                    {
                        c.SetWidth(52);
                    });

                    table.AddColumn("", null, c =>
                    {
                        c.SetWidth(XUnit.FromPercent(40));
                    });

                    table.AddColumn("", null, c =>
                    {
                        c.SetWidth(XUnit.FromPercent(60));
                    });

                    table.AddRow((TableRowBuilder row) =>
                    {

                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Left)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.AddImage(logoUrl,
                                          i =>
                                          {
                                              i.SetWidth(65);
                                              i.SetHeight(45);
                                              i.SetScale(ScalingMode.UserDefined);
                                          });
                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Center)
                                .SetVerticalAlignment(VerticalAlignment.Center)
                                .SetFontSize(12);
                            cell.AddParagraph("AFTERSCHOOL").SetBold();
                            cell.AddParagraph("BANK DRAFT FORM");
                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Center)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.AddParagraphToCell("REGISTRATION PACKET  PG. 4 of 4");
                            cell.SetBackColor(Color.Black);
                            cell.SetFont(Fonts.Courier(12).SetColor(Color.White).SetBold());
                        });
                    });
                });

                var p = s.AddParagraph("BEST SCHOOL of Twin Peaks ")
                    .SetMarginTop(5)
                    .SetBold()
                    .SetFontColor(Color.Gray)
                    .SetFontSize(12);
                p.AddText("Afterschool ").SetFontColor(Color.Black);
                p.AddText("Agreement ACH/CC Automatic Payment Option");

                s.AddParagraph("STEP #1")
                    .SetBackColor(Color.FromRgba(0.3, 0.3, 0.3))
                    .SetFont(Fonts.Courier(16).SetColor(Color.White).SetBold());

                s.AddParagraph()
                   .SetMarginLeft(0)
                   .SetBorderWidth(0)
                   .SetFontSize(4)
                   .AddTabSymbol().AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
                s.AddParagraph()
                    .SetFontSize(6)
                        .AddTextToParagraph("CHILD'S FIRST NAME")
                    .AddTabSymbol()
                        .AddTextToParagraph("MIDDLE INITIAL")
                    .AddTabSymbol()
                        .AddTextToParagraph("LAST NAME")
                    .AddTabulationInPercent(35, TabulationType.Left)
                    .AddTabulationInPercent(65, TabulationType.Left);

                s.AddParagraph()
                   .SetMarginLeft(0)
                   .SetMarginTop(15)
                   .SetBorderWidth(0)
                   .SetFontSize(6)
                   .AddTabSymbol().AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
                s.AddParagraph()
                    .SetFontSize(6)
                        .AddTextToParagraph("PHONE NUMBER (DAY)")
                    .AddTabSymbol()
                        .AddTextToParagraph("PHONE NUMBER (EVENING)")
                    .AddTabulationInPercent(65, TabulationType.Left);

                s.AddParagraph()
                    .SetMarginLeft(0)
                    .SetMarginTop(15)
                    .SetBorderWidth(0)
                    .SetFontSize(6)
                    .AddTabSymbol().AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
                s.AddParagraph()
                    .SetFontSize(6)
                        .AddTextToParagraph("CHILD'S SCHOOL")
                    .AddTabSymbol()
                        .AddTextToParagraph("CHILD'S ADDRESS")
                    .AddTabSymbol()
                        .AddTextToParagraph("CITY")
                    .AddTabSymbol()
                        .AddTextToParagraph("STATE")
                    .AddTabSymbol()
                        .AddTextToParagraph("ZIP")
                    .AddTabulationInPercent(35, TabulationType.Left)
                    .AddTabulationInPercent(65, TabulationType.Left)
                    .AddTabulationInPercent(77, TabulationType.Left)
                    .AddTabulationInPercent(90, TabulationType.Left);

                s.AddParagraph()
                   .SetFontSize(6)
                   .SetMarginLeft(0)
                   .SetMarginTop(15)
                   .SetMarginBottom(3)
                   .SetBorderWidth(0)
                   .AddTabSymbol().AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);

                s.AddTable((TableBuilder table) =>
                {
                    table.SetContentRowStyleBorder(borderBuilder => borderBuilder.SetStroke(Stroke.None));

                    table.AddColumn("", null, c =>
                    {
                        c.SetWidth(XUnit.FromPercent(49));
                    });

                    table.AddColumn("", null, c =>
                    {
                        c.SetWidth(XUnit.FromPercent(2));
                    });

                    table.AddColumn("", null, c =>
                    {
                        c.SetWidth(XUnit.FromPercent(49));
                    });

                    table.AddRow((TableRowBuilder row) =>
                    {
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Left)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.AddParagraphToCell("STEP #2");
                            cell.SetPadding(2, 0, 0, 0);
                            cell.SetBackColor(Color.FromRgba(0.3, 0.3, 0.3));
                            cell.SetFont(Fonts.Courier(16).SetColor(Color.White).SetBold());
                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {

                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Left)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.AddParagraphToCell("STEP #3");
                            cell.SetPadding(2, 0, 0, 0);
                            cell.SetBackColor(Color.FromRgba(0.3, 0.3, 0.3));
                            cell.SetFont(Fonts.Courier(16).SetColor(Color.White).SetBold());
                        });
                    });

                    table.AddRow((TableRowBuilder row) =>
                    {
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetBorder(Stroke.Solid, Color.Black, StyleSheet.DefaultBorderWidth);
                            cell.SetHorizontalAlignment(HorizontalAlignment.Center)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.AddParagraphToCell("Begin Draft Date:")
                                .AddParagraph("_______ / _______ / _______");
                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {

                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Center)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.AddTable(t =>
                            {
                                t.SetHeaderRowStyleBackColor(Color.Gray);
                                //t.SetContentRowStyleFont(f => f.SetSize(8));
                                //t.AddColumn("DRAFT DATES", null, c =>
                                //{
                                //    c.SetWidth(XUnit.FromPercent(65));
                                //});

                                //t.AddColumn("AMOUNT", null, c =>
                                //{
                                //    c.SetWidth(XUnit.FromPercent(35));
                                //});

                                t.AddColumn("", null, c =>
                                {
                                    c.SetWidth(XUnit.FromPercent(70));
                                });

                                t.AddColumn("", null, c =>
                                {
                                    c.SetWidth(XUnit.FromPercent(30));
                                });
                                t.AddRow(r =>
                                {
                                    r.AddCell("DRAFT DATES").SetBackColor(Color.Gray).SetFontColor(Color.White);
                                    r.AddCell("AMOUNT").SetBackColor(Color.Gray).SetFontColor(Color.White);
                                });
                                t.AddRow(r =>
                                {
                                    r.SetVerticalAlignment(VerticalAlignment.Bottom);
                                    r.AddCell("Monthly on the 1st").SetFontSize(10);
                                    r.AddCell("$").SetBold();
                                });
                                t.AddRow(r =>
                                {
                                    r.SetVerticalAlignment(VerticalAlignment.Bottom);
                                    r.AddCell("Semi-Monthly on the 1st & 15th").SetFontSize(10);
                                    r.AddCell("$").SetBold();
                                });
                            });
                        });
                    });
                    table.SetMarginBottom(5);
                });

                s.AddParagraph("STEP #4")
                    .SetBackColor(Color.FromRgba(0.3, 0.3, 0.3))
                    .SetFont(Fonts.Courier(16).SetColor(Color.White).SetBold());

                s.AddTable((TableBuilder table) =>
                {
                    table.SetMarginTop(2);

                    table.SetContentRowStyleBorder(borderBuilder => borderBuilder.SetStroke(Stroke.None));

                    table.AddColumn("", null, c =>
                    {
                        c.SetWidth(XUnit.FromPercent(49));
                    });

                    table.AddColumn("", null, c =>
                    {
                        c.SetWidth(XUnit.FromPercent(2));
                    });

                    table.AddColumn("", null, c =>
                    {
                        c.SetWidth(XUnit.FromPercent(49));
                    });

                    table.AddRow((TableRowBuilder row) =>
                    {
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Left)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.SetPadding(2, 0, 0, 0);
                            cell.AddParagraph("")
                                .AddInlineImageToParagraph(checkboxUrl, 11, 11, ScalingMode.UserDefined)
                                .AddText(" OPTION 1: CREDIT/DEBIT CARD");
                            cell.SetBackColor(Color.Gray);
                            cell.SetFont(Fonts.Courier(14).SetColor(Color.White).SetBold());
                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {

                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Left)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.SetPadding(2, 0, 0, 0);
                            cell.AddParagraph("")
                                .AddInlineImageToParagraph(checkboxUrl, 11, 11, ScalingMode.UserDefined)
                                .AddText(" OPTION 2: BANK DRAFT");
                            cell.SetBackColor(Color.Gray);
                            cell.SetFont(Fonts.Courier(14).SetColor(Color.White).SetBold());
                        });
                    });

                    table.AddRow((TableRowBuilder row) =>
                    {
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Center)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.SetFont(Fonts.Courier(6));
                            cell.AddTable(t =>
                            {
                                t.SetContentRowStyleFont(Fonts.Courier(6));
                                t.SetDefaultAltRowStyle();

                                t.AddColumn("", null, c =>
                                {
                                    c.SetWidth(XUnit.FromPercent(65));
                                });

                                t.AddColumn("", null, c =>
                                {
                                    c.SetWidth(XUnit.FromPercent(35));
                                });
                                t.AddRow(r =>
                                {
                                    var c = r.AddCell().SetColSpan(2)
                                        .SetFontSize(8)
                                        .SetVerticalAlignment(VerticalAlignment.Center);
                                    c.AddParagraph("Check one:  ")
                                        .AddInlineImageToParagraph(checkboxUrl, 8, 8, ScalingMode.UserDefined)
                                        .AddTextToParagraph(" Visa  ")
                                        .AddInlineImageToParagraph(checkboxUrl, 8, 8, ScalingMode.UserDefined)
                                        .AddTextToParagraph(" Mastercard  ")
                                        .AddInlineImageToParagraph(checkboxUrl, 8, 8, ScalingMode.UserDefined)
                                        .AddTextToParagraph(" Discover  ")
                                        .AddInlineImageToParagraph(checkboxUrl, 8, 8, ScalingMode.UserDefined)
                                        .AddTextToParagraph(" AmEx");
                                });
                                t.AddRow(r =>
                                {
                                    r.AddCell("CREDIT/DEBIT CARD #").AddParagraph("FILL HERE").SetFontColor(Color.White).SetFontSize(16);
                                    r.AddCell("EXP. DATE").AddParagraph("FILL HERE").SetFontColor(Color.White).SetFontSize(16);
                                });
                                t.AddRow(r =>
                                {
                                    r.AddCell("CARDHOLDER NAME").AddParagraph("FILL HERE").SetFontColor(Color.White).SetFontSize(16);
                                    r.AddCell("CVV").AddParagraph("FILL HERE").SetFontColor(Color.White).SetFontSize(16);
                                });
                            });
                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {

                        });
                        row.AddCell((TableCellBuilder cell) =>
                        {
                            cell.SetHorizontalAlignment(HorizontalAlignment.Center)
                                .SetVerticalAlignment(VerticalAlignment.Center);
                            cell.SetFontSize(6);
                            cell.AddTable(t =>
                            {
                                t.SetContentRowStyleFont(Fonts.Courier(6));
                                t.SetDefaultAltRowStyle();

                                t.AddColumn("", null, c =>
                                {
                                    c.SetWidth(XUnit.FromPercent(60));
                                });

                                t.AddColumn("", null, c =>
                                {
                                    c.SetWidth(XUnit.FromPercent(40));
                                });
                                t.AddRow(r =>
                                {
                                    r.AddCell("ACCOUNT HOLDER NAME").AddParagraph("FILL HERE").SetFontColor(Color.White).SetFontSize(16);
                                    r.AddCell("BANK NAME").AddParagraph("FILL HERE").SetFontColor(Color.White).SetFontSize(16);
                                });
                                t.AddRow(r =>
                                {
                                    r.AddCell("ROUTING/TRANSIT #").AddParagraph("FILL HERE").SetFontColor(Color.White).SetFontSize(16);
                                    r.AddCell("BANK ACCOUNT #").AddParagraph("FILL HERE").SetFontColor(Color.White).SetFontSize(16);
                                });
                            });
                        });
                    });
                });

                s.AddParagraph("AUTOMATED CLEARINGHOUSE(ACH) DRAFTS ARE REQUIRED TO HAVE A VOIDED CHECK. DEBIT CARDS ARE NOT ACCEPTED. MUST BE ACH OR CREDIT CARDS ONLY.")
                    .SetMarginTop(10)
                    .SetBold()
                    .SetFontSize(8);

                s.AddParagraph("Only 1 Form of Draft Payment can be entered per person.").SetListBulleted(ListBullet.Bullet).SetFontSize(8);
                s.AddParagraph("Children enrolled in Summer Camp may have a larger draft amount on May 15 & Aug 1.").SetListBulleted(ListBullet.Bullet).SetFontSize(8);
                s.AddParagraph();
                s.AddParagraph("I understand that this transfer will occur monthly on the 1st. First draft begins Aug. 1.").SetListNumbered(NumerationStyle.Arabic, 0, 0).SetFontSize(8);
                s.AddParagraph("I understand that should I choose to change a Bank Account I must provide a school with at least a 2 week notice.").SetListNumbered(NumerationStyle.Arabic, 0, 0).SetFontSize(8);
                s.AddParagraph("I understand that the information above will be used to transfer payment from my account.").SetListNumbered(NumerationStyle.Arabic, 0, 0).SetFontSize(8);
                s.AddParagraph("I understand that if my payment is returned for non-sufficient funds I will be charged a $30 fee.").SetListNumbered(NumerationStyle.Arabic, 0, 0).SetFontSize(8);
                s.AddParagraph("BEST SCHOOL only accepts Visa, MasterCard, Discover, and American Express.").SetListNumbered(NumerationStyle.Arabic, 0, 0).SetFontSize(8);
                s.AddParagraph("I understand that after three returned items, I will be ineligible to use the automatic payment option.").SetListNumbered(NumerationStyle.Arabic, 0, 0).SetFontSize(8);

                s.AddParagraph("ACCOUNT HOLDER ACKNOWLEDGMENT").SetMarginTop(5).SetBold();

                s.AddParagraph()
                    .SetMarginTop(8)
                    .SetMarginLeft(0)
                    .SetBorderWidth(0)
                    .AddTabSymbol().AddTabulationInPercent(50, TabulationType.Right, TabulationLeading.BottomLine)
                    .AddTabSymbol().AddTabSymbol()
                    .AddTabulationInPercent(70, TabulationType.Left)
                    .AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
                s.AddParagraph()
                    .SetMarginTop(0)
                    .SetFontSize(10)
                    .AddTabSymbol().AddTextToParagraph("Account Holder Signature")
                    .AddTabulationInPercent(25, TabulationType.Center)
                    .AddTabSymbol()
                    .AddTextToParagraph("Date")
                    .AddTabulationInPercent(85, TabulationType.Center);

                s.AddTable(t =>
                {
                    t.SetAlignment(HorizontalAlignment.Center);
                    t.SetWidth(XUnit.FromPercent(80));
                    t.SetMarginTop(5);
                    t.SetBorderStroke(Stroke.Dotted);
                    t.AddColumnToTable("", XUnit.FromPercent(10))
                        .AddColumnToTable("", XUnit.FromPercent(80))
                        .AddColumn("", XUnit.FromPercent(10));
                    var r = t.AddRow().SetHorizontalAlignment(HorizontalAlignment.Center).SetVerticalAlignment(VerticalAlignment.Center);
                    r.AddCell()
                        .SetBorderStroke(Stroke.Dotted, Stroke.Dotted, Stroke.None, Stroke.Dotted)
                        .SetFontSize(6)
                        .AddParagraphToCell("Please").AddParagraphToCell("Staple").AddParagraphToCell("Here");
                    r.AddCell()
                        .SetBorderStroke(Stroke.None, Stroke.Dotted, Stroke.None, Stroke.Dotted)
                        .SetPadding(0, 68, 0, 64)
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .AddParagraph("STAPLE VOIDED CHECK HERE").SetBold().SetFontSize(24);
                    r.AddCell()
                        .SetBorderStroke(Stroke.None, Stroke.Dotted, Stroke.Dotted, Stroke.Dotted)
                        .SetFontSize(6).AddParagraphToCell("Please").AddParagraphToCell("Staple").AddParagraphToCell("Here");
                });
                s.SetMargins(20, 20, 20, 0);
            });
            return builder;
        }
    }
}