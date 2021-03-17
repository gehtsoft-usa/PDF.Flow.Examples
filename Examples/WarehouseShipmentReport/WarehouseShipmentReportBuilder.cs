using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WarehouseShipmentReport.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.Barcodes.Enums;

namespace WarehouseShipmentReport
{
    public static class WarehouseShipmentReportBuilder
    {
        #region Fields

        private static readonly string ProjectDir;
        private static readonly string WarehouseShipmentReportJsonFile;
        private static readonly string WarehouseShipmentReportJsonContent;
        public static List<WarehouseShipmentReportData> WarehouseShipmentReportData { get; }
        private static readonly FontBuilder DocumentFont;
        private static readonly FontBuilder BoldFont;
        private static readonly FontBuilder TitleFont;
        private static readonly FontBuilder OrangeFont;
        private static readonly string LogoUrl;

        #endregion Fields

        #region Constructors

        static WarehouseShipmentReportBuilder()
        {
            ProjectDir = Directory.GetCurrentDirectory();
            WarehouseShipmentReportJsonFile = Path.Combine(ProjectDir, "Content", "WarehouseShipmentReport.json");
            WarehouseShipmentReportJsonContent = File.ReadAllText(WarehouseShipmentReportJsonFile);
            WarehouseShipmentReportData = JsonConvert.DeserializeObject<List<WarehouseShipmentReportData>>(WarehouseShipmentReportJsonContent);

            FontBuilder documentFont() => StyleSheet.DefaultFont().SetSize(14);
            FontBuilder boldFont() => documentFont().SetBold();
            DocumentFont = documentFont();
            BoldFont = boldFont();
            TitleFont = boldFont().SetSize(24).SetColor(Color.FromRgba(60.0/255.0, 29.0/255.0, 0.0));
            OrangeFont = boldFont().SetSize(24).SetColor(Color.FromRgba(245.0 / 255.0, 166.0 / 255.0, 35.0/255.0));

            LogoUrl = Path.Combine(ProjectDir, "Images", "Logo.png");

        }

        #endregion Constructors

        #region Methods

        internal static DocumentBuilder AddWarehouseShipmentReport(this DocumentBuilder builder)
        {
            builder
                .AddSection()
                    .SetSectionSettings()
                    .AddRepeatingHeader()
                    .AddWarehouseShipmentReportData();                                
            return builder;
        }

        internal static SectionBuilder SetSectionSettings(this SectionBuilder s)
        {
            return s.SetMargins(30).SetSize(PaperSize.A4).SetOrientation(PageOrientation.Portrait).SetNumerationStyle(NumerationStyle.Arabic);
        }

        internal static SectionBuilder AddRepeatingHeader(this SectionBuilder s)
        {
            s.AddHeaderToBothPages(100)
                .AddTable()
                    .SetBorderStroke(Stroke.None)
                    .AddColumn()
                        .SetWidth(100)
                .ToTable()
                    .AddColumn()
                        .SetWidth(XUnit.FromPercent(70))
                .ToTable()
                    .AddColumn()
                        .SetWidth(XUnit.FromPercent(30))
                .ToTable()
                    .AddRow()
                        .AddCell()
                            .AddImage(LogoUrl)
                                .SetSize(100, 100)
                    .ToRow()
                        .AddCell()
                            .AddParagraph("Sample company")
                                .SetFont(TitleFont)
                        .ToCell()
                            .AddParagraph("Warehouse shipments")
                                .SetFont(OrangeFont)
                    .ToRow()
                        .AddCell()
                            .SetFontSize(10)
                            .AddParagraphToCell("Sample company GmbH")
                            .AddParagraphToCell("P.O.BOX 13248")
                            .AddParagraphToCell("Adenauerstrasse 10")
                            .AddParagraphToCell("Hellesdorf, Bayern, Germany")
                .ToArea()
                    .AddLine()
                        .SetColor(Color.FromRgba(0.6, 0.3, 0.0))
                        .SetWidth(2)
                        .SetMarginTop(10);
            return s;
        }

        internal static void AddWarehouseShipmentReportTitle(SectionBuilder s)
        {            
            s.AddParagraph("Warehouse shipments").SetMargins(20, 0, 0, 10).SetFont(TitleFont).SetAlignment(HorizontalAlignment.Center);
            s.AddLine().SetColor(Color.FromRgba(0.6, 0.3, 0.0)).SetStroke(Stroke.Solid).SetWidth(2);
        }

        internal static SectionBuilder AddWarehouseShipmentReportData(this SectionBuilder s)
        {
            void AddLevel(uint level, string text, string barcode = "")
            {
                FontBuilder font = DocumentFont;
                ListBullet listBullet = ListBullet.Bullet;
                switch (level)
                {
                    case 0:
                        font = TitleFont;
                        break;
                    case 1:
                        font = BoldFont;                      
                        break;
                    case 2:                     
                        break;
                    case 3:
                        listBullet = ListBullet.Dash;
                        break;
                    default:
                        return;
                }
                s.AddParagraph(p =>
                {
                    p.SetListBulleted(listBullet, level, 20f);
                    p
                        .SetMarginBottom(5)
                        .AddText(text)
                        .SetFont(font);
                    if (level == 1)
                        p.SetListNumbered(NumerationStyle.Arabic, level, 20f, true);
                    if (level == 2)
                    {
                        p.SetListNumbered(NumerationStyle.Arabic, level, 20f, true);
                        p.AddBarcode(barcode, BarcodeType.GS1_128C, 186f, 7.9f, hasQuiteZones: false)
                            .SetMarginLeft(5f); 
                    }

                    if (level == 3)
                    {
                        p.SetListBulleted(listBullet, level, 28f);
                    }
                });                
            }

            foreach (var shipment in WarehouseShipmentReportData)
            {
                AddLevel(0, "Shipment "+shipment.Shipment);
                if (shipment.Orders != null)
                    foreach (var order in shipment.Orders)
                    {
                        AddLevel(1, "Order " + order.Order + ", " + order.Customer);
                        if (order.Products != null)
                        {
                            foreach (var product in order.Products)
                            {
                                if (product.Barcode != null)
                                    AddLevel(2, "", product.Barcode);
                                AddLevel(3, "Product Code: " + product.Code);
                                AddLevel(3, "Product Name: " + product.Name);
                            }
                        }
                    }
            }
            return s;
        }       

        #endregion Methods
    }
}
