##### Example: WarehouseShipmentReport
 
# Purpose
WarehouseShipmentReport project is an example of generation of the report of warehouse shipments. The example demonstrates creation of a multilevel list and working with barcodes. Example shows how to set hierarchic numeration of list, levels, left indents, bulleted and numbered list items. Also different fonts are applied. Additionally, this example demonstrates how add repeating header and how to work with JSON as an input data.

**to be changed to proper URL** The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/WarehouseShipmentReport).

# Prerequisites
1) **Visual Studio 2017** or above is installed.
To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### Report input data
The list of warehouse shipments for the report are located in the file **Content/WarehouseShipmentReport.json**:
```json
    "Shipment": "2020-01-23 10:13AM",
    "Orders": 
	[
	  {
	    "Order": "2019-10-08 11:14AM",
	    "Customer": "John Smith",	
	    "Products": 
	    	[
	    		{
					"Barcode": "840749284261",
	    			"Code": "559555",
	    			"Name": "Samsung 128GB 100MB/s (U3) MicroSDXC EVO Select Memory Card"	
	    		},
	    		{
					"Barcode": "140749244261",
	    			"Code": "626666",
	    			"Name": "Logitech MK270 Wireless Keyboard"
	    		}
	    	]
	  },
```

### Output file
The example creates the file **WarehouseShipmentReport.pdf** in the output **bin/(debug|release)/netcoreapp2.1** folder.


# Writing the source code

#### 1. Create new console application.
1.1.	Run Visual Studio
1.2.	File -> Create -> Console Application (.Net Core)

#### 2. Modify class Program.
2.1. In the function **Main()** set path to output PDF-file:
```c#
    Parameters parameters = new Parameters(null, "WarehouseShipmentReport.pdf");
```
2.2. Call method Run() for house rental contract generation and Build() for building a document into an output PDF-file:
```c#
    WarehouseShipmentReportRunner.Run().Build(parameters.file);
```
2.3. After the file is generated, notify the user about it:
```c#
    Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                        + "\" document has been successfully built");
```


#### 3. Create class for running document generation

Create new document by calling method New() of the DocumentBuilder. Then call AddWarehouseShipmentReport() method for adding content to the document:
```c#
public static class WarehouseShipmentReportRunner
{
    public static DocumentBuilder Run()
    {
        return DocumentBuilder.New()
            .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
            .AddWarehouseShipmentReport();
    }
}
```


#### 4. Build document structure

4.1. Create class **WarehouseShipmentReportBuilder** which will contain method for building document structure
```c#
public static class WarehouseShipmentReportBuilder
```

4.2. Under the Model subfolder create **class WarehouseShipmentReportData**:
```c#
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WarehouseShipmentReport.Model
{
    [DataContract]
    public class WarehouseShipmentReportData
    {
        [DataMember]
        public string Shipment { get; set; }
        [DataMember]
        public List<OrderData> Orders { get; set; }
    }

    [DataContract]
    public class OrderData
    {
        [DataMember]
        public string Order { get; set; }
        [DataMember]
        public string Customer { get; set; }
        [DataMember]
        public List<ProductData> Products { get; set; }
    }

    [DataContract]
    public class ProductData
    {
        [DataMember]
        public string Barcode { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }        
    }
}
```

4.3. **Define fields** of the WarehouseShipmentReportBuilder class.

Path to the project directory:
```c#
private static readonly string ProjectDir;
```

Path to the input template text of the contract:
```c#
private static readonly string ContractTextFile
```

Path to the JSON file, containing input data:
```c#
private static readonly string WarehouseShipmentReportJsonFile;
```

JSON content:
```c#
private static readonly string WarehouseShipmentReportJsonContent;
```

Different fonts:
```c#
private static readonly FontBuilder DocumentFont;
private static readonly FontBuilder BoldFont;
private static readonly FontBuilder TitleFont;
private static readonly FontBuilder OrangeFont;
```

Dictionary data:
```c#
public static List<WarehouseShipmentReportData> WarehouseShipmentReportData { get; }
```


4.4. **Create constructor** static WarehouseShipmentReportBuilder() and **initialize fields**:

Initialize path variables:
```c#
ProjectDir = Directory.GetCurrentDirectory();
WarehouseShipmentReportJsonFile = Path.Combine(ProjectDir, "Content", "WarehouseShipmentReport.json");
```

Read JSON data:
```c#
WarehouseShipmentReportJsonContent = File.ReadAllText(WarehouseShipmentReportJsonFile);
WarehouseShipmentReportData = JsonConvert.DeserializeObject<List<WarehouseShipmentReportData>>(WarehouseShipmentReportJsonContent);
```

Initialize fonts:
```c#
FontBuilder documentFont() => StyleSheet.DefaultFont().SetSize(14);
FontBuilder boldFont() => documentFont().SetBold();
DocumentFont = documentFont();
BoldFont = boldFont();
TitleFont = boldFont().SetSize(24).SetColor(Color.FromRgba(60.0/255.0, 29.0/255.0, 0.0));
OrangeFont = boldFont().SetSize(24).SetColor(Color.FromRgba(245.0 / 255.0, 166.0 / 255.0, 35.0/255.0));
 ```       

4.5. Create method **AddWarehouseShipmentReport()**.

This method will create a section by calling AddSection() method on DocumentBuilder, set needed paper size, orientation and margins and call the methods for adding content to the section. All these methods must be the extension methods of SectionBuilder and return SectionBuilder, so we can call them in an uninterrupted fluent chain:
```c#
        internal static DocumentBuilder AddWarehouseShipmentReport(this DocumentBuilder builder)
        {
            builder
                .AddSection()
                    .SetSectionSettings()
                    .AddRepeatingHeader()
                    .AddWarehouseShipmentReportData();                                
            return builder;
        }
```


4.6. **Set Section Settings**. 

```c#
        internal static SectionBuilder SetSectionSettings(this SectionBuilder s)
        {
            return s.SetMargins(30).SetSize(PaperSize.A4).SetOrientation(PageOrientation.Portrait).SetNumerationStyle(NumerationStyle.Arabic);
        }
```

4.6. **Add repeating header** to every page of the document.
Method AddHeaderToBothPages() of the SectionBuilder adds a header to every page. You could also call AddHeaderToOddPage() to add this header only to odd pages, or AddHeaderToEvenPage() for even pages only. You can add as many header as you need, and they will be placed below each other. We need one here.
```c#
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
```

4.8. **Add report data** with warehouse shipments. 
Here we need a small local sub-method AddLevel() which will add corresponding data and make list bulleted or numbered. 

On the level of Product (level 2) we need to generate barcode image, so we call method AddBarcode() of ParagraphBuilder and pass barcode input string from JSON field Barcode and the type of the Barcode Standart (BarcodeType.UPC_A in this case).

The data from JSON is stored in WarehouseShipmentReportData. So we will loop through it and call AddLevel().
```c#
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
                        p.AddBarcode(barcode, BarcodeType.UPC_A);
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
                        foreach (var product in order.Products)
                        {
                            if (product.Barcode != null)
                                AddLevel(2, "", product.Barcode);
                            AddLevel(3, "Product Code: " + product.Code);
                            AddLevel(3, "Product Name: " + product.Name);
                        }
                    }
            }
            return s;
        }       
```

#### 5. Generated **PDF file** should look as shown below:
The resulting WarehouseShipmentReport.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/WarehouseShipmentReport.pdf).