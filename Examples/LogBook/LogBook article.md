##### Example: LogBook
 
# Purpose
The example demonstrates a multi-page document that consists of a complex table
that spreads into two-pages layout (i.e. half of table columns is printed on the left page
and half of table columns is printed on the right page).

Pay attention that the developer just defines the table data, all complex work related
to layouting the data, including a complex job of making rows on the left and right sides
of the table matching by the position and height, is made by the library.

Your will not find this mechanism of table splitting in any other PDF library.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/LogBook).

# Prerequisites
1) **Visual Studio 2017** or above is installed.
To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core Framework SDK 2.1** or above is installed.
To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### Input data
The mock data used to generate the table is located in the file **Content/operations_log.json**. It contains a log of sent/received operations with the information about the seller and purchaser.

### Output file
The example creates the file **LogBook.pdf** in the output **bin/(debug|release)/netcoreapp2.1** folder.

# Writing the source code

Main work will be made in a class LogBookCoordinator. It will process each operation from input file and render it as a row of a table. Six columns will be printed at the even page, and the rest four columns will be printed at the odd page. Class Operation will describe one operation from the input file. To create the book we will read the log into collection of operations and pass this collection to the Input method of LogBookCoordinator.

#### 1. Create new console application.
1.1.	Run Visual Studio
1.2.	File -> Create -> Console Application (.Net Core)

#### 2. Modify class Program.
2.1. Modify method **Main()**. 

Prepare parameters and filename:
```csharp
            Parameters parameters = new Parameters(null, "LogBook.pdf");

            if (!PrepareParameters(parameters, args))
            {
                return 1;
            }
```

Call Run method of LogBookRunner class to start LogBook generation:
```csharp
LogBookRunner.Run(parameters.file);
```
After generation is completed, notify user about this and open generated PDf, if needed:
```csharp
            Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                                   + "\" document has been successfully built");
            if (parameters.appToView != null)
            {
                Start(parameters.file, parameters.appToView);
            }
            return 0;
```

#### 3. Describe model:

Abstract class IEntity:
```csharp
public interface IEntity { }
public abstract class Entity : IEntity
{
}
```
Class describing one operation from operation log:
```csharp
    [DataContract]
    public class Operation:Entity
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string VIN { get; set; }
        [DataMember]
        public DateTime? Received { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public DateTime? Sent { get; set; }
        [DataMember]
        public string Buyer_Name { get; set; }
        [DataMember]
        public string Buyer_Address { get; set; }
        [DataMember]
        public string Buyer_State { get; set; }
        [DataMember]
        public string Buyer_Zip { get; set; }
        [DataMember]
        public bool Discarded{ get; set; }
        [DataMember]
        public string DiscardReason { get; set; }
        public string FullAddress
        {
            get
            {
                return Buyer_Address + (String.IsNullOrEmpty(Buyer_State) ? "" : (", " + Buyer_State)) +
                    (String.IsNullOrEmpty(Buyer_Zip) ? "" : (", " + Buyer_Zip));                    
            }
        }
    }
```
Options for customizing output:
```csharp
    public class PDFSettings
    {
        public Box SectionMargins { get; set; }
        public PaperSize PaperSize { get; set; } = PaperSize.A4;
        public PageOrientation Orientation { get; set; } = PageOrientation.Landscape;
        public string HeaderOdd { get; set; }
        public string HeaderEven { get; set; }
        public string BookName { get; set; }
        public DateTime? DateOfPrint { get; set; }
        public DateTime? DateRangeStart { get; set; }
        public DateTime? DateRangeEnd { get; set; }
        public long NumberOfRecords { get; set; }
        public string DateFormat { get; set; } = "d/M/yyyy";
        public TextOverflowAction TextOverflowAction { get; set; } = TextOverflowAction.Ellipsis;
        public Stroke DiscardedStrikethroughStroke { get; set; } = Stroke.Solid;
        public Color DiscardedStrikethroughColor { get; set; } = Color.Black;
        public StrikethroughMode DiscardedStrikethroughMode { get; set; } = StrikethroughMode.TextOnly;
        public uint StartingPage { get; set; }
        public bool DisplayDateRange { get; set; } = false;
    }
```
Library allows to strike out a text in a table cell or the whole cell. So we have two options to choose from:
```csharp
public enum StrikethroughMode
{
    TextOnly,
    FullRow,
}
```

#### 4. Implement class LogBookRunner.

4.1. Create class:
```csharp
namespace LogBook
{
    public static class LogBookRunner
    {
```

4.2. **Declare fields**:
Path to the project directory:
```csharp
private static string ProjectDir;
```
Path to input file:
```csharp
private static string OperationJsonFile;
```
Input directory:
```csharp
private static string OperationJsonContent;
```
Collection of operations:
```csharp
private static IEnumerable<Operation> OperationData;
```
Options for customizing PDF:
```csharp
private static PDFSettings Options;
```

4.3. Create **constructor**:
4.3.1. Constructor name is the same as the name of the class:
```csharp
        static LogBookRunner()
		{
```
4.3.2. **Initialize variables** and fields:
```csharp
ProjectDir = Directory.GetCurrentDirectory();
int _id = 1;
DateTime? maxDate = DateTime.ParseExact("1/1/1970", "d/M/yyyy", null);
DateTime? minDate = DateTime.ParseExact("1/1/3000", "d/M/yyyy", null);
```
Read log content:
```csharp
OperationJsonFile = Path.Combine(ProjectDir, "Content", "operations_log.json");
OperationJsonContent = File.ReadAllText(OperationJsonFile);
OperationData = JsonConvert.DeserializeObject<List<Operation>>(OperationJsonContent);
```

Calculate maximum and minimum dates of operations for showing at the last page of PDF file:
```csharp
foreach (Operation item in OperationData)
{
    item.Code = _id.ToString();
    _id++;
    if ((item.Received != null) && (item.Received < minDate))
    {
        minDate = item.Received;
    }
    if ((item.Sent != null) && (item.Sent < minDate))
    {
        minDate = item.Sent;
    }
    if ((item.Received != null) && (item.Received > maxDate))
    {
        maxDate = item.Received;
    }
    if ((item.Sent != null) && (item.Sent > maxDate))
    {
        maxDate = item.Sent;
    }
}
```
4.3.3. **Initialize settings** for customizing output.
Here we can set some custom settings describing how our book will look like. For example, you can change DateOfPrint to DateTime.Now(), if you want.
```csharp:
Options = new PDFSettings
{
    SectionMargins = new Box(20, 20, 20, 20),
    PaperSize = PaperSize.A4,
    Orientation = PageOrientation.Landscape,
    HeaderOdd = "Operations Log Book Printout",                
    HeaderEven = "Operations Log Book Printout",
    BookName = "Operations Log Book",
    DateOfPrint = DateTime.ParseExact("1/1/2021", "d/M/yyyy", null),
    DateRangeStart = minDate,
    DateRangeEnd = maxDate,
    NumberOfRecords = _id-1,
    DateFormat = "MM/dd/yyyy",
    DiscardedStrikethroughStroke = Stroke.Solid,
    DiscardedStrikethroughMode = StrikethroughMode.TextOnly,
    DiscardedStrikethroughColor = Color.Red,
    StartingPage = 1,
    DisplayDateRange = true
};
```
4.3.4. Write method **Run**. Create an instanse of **LogBookCoordinator** (this class will be described below) and call it's **Input method** passing a collection of operations as a parameter:
```csharp
public static void Run(string pdfFileName)
{ 
    using (IStreamCoordinator coordinator = new LogBookCoordinator(pdfFileName, Options, startingPage: 0))
    {                
        coordinator.Input(OperationData);
    }
}       
```

#### 5. Implement a class LogBookCoordinator

5.1. Create interface with three methods for processing entity or collection of entities:
```csharp
public interface IStreamCoordinator:IDisposable
{
    void Input<T>(T data) where T:IEntity;
    void Input<T>(IEnumerable<T> data) where T:IEntity;
    void Input(IDictionary<string, object> data);
}
```

5.2. Create class, **declare fields and initialize them** in the constructor:
```csharp
    internal class LogBookCoordinator : IStreamCoordinator
    {
        #region Fields
        private readonly DocumentBuilder _builder;
        private readonly PDFSettings _options;
        private readonly IStream _stream;
        private bool _disposed;
        private readonly bool _continuous;
        private TableBuilder _table;
        private readonly string DateFormat = @"MM\/dd\/yyyy";
        private FontBuilder exoRegular;
        private FontBuilder exoItalic;
        private FontBuilder exoBold;
        private readonly IEnumerable<SectionBuilder> _sections;
        #endregion Fields

        #region Constructors
        public LogBookCoordinator(IStream stream, bool continuous = false)
        {
            _stream = stream;
            _builder = stream.Builder;
            _options = stream.Options;
            DateFormat = _options.DateFormat ?? DateFormat;
            stream.Coordinator = this;
            _continuous = continuous;
            exoRegular = FontBuilder.New().FromFile(stream.exoRegularFile, 10);
            exoItalic = FontBuilder.New().FromFile(stream.exoItalicFile, 10);
            exoBold = FontBuilder.New().FromFile(stream.exoBoldFile, 10);
            _sections = CreateSections();
            if (continuous)
            {
                BuildSections();
            }
        }
        public LogBookCoordinator(string file, PDFSettings options, int startingPage = 0, bool continuous = false) : this(new DocumentStream(file, options) { StartingPage = startingPage }, continuous) { }
        public LogBookCoordinator(Stream stream, PDFSettings options, int startingPage = 0, bool continuous = false) : this(new DocumentStream(stream, options) { StartingPage = startingPage }, continuous) { }
        #endregion Constructors
```

5.3. Implement methods **Input**:
For these methods we need helper-method GetProperties:
```csharp
internal static class ReflectionHelper
{
    internal static IEnumerable<PropertyInfo> GetProperties(this object obj)
    {
        var propList = obj.GetType().GetProperties();
        return propList;
    }
}
```

If data parameter is single entity, call Input for one operation, otherwise call input for dictionary:
```csharp
public void Input<T>(T data) where T : IEntity
{
    if (data is Operation entity)
    {
        Input(entity);
    }
    else
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        foreach (PropertyInfo info in data.GetProperties())
        {
            properties.Add(info.Name, info.GetValue(data));
        }
        Input((IDictionary<string, object>)properties);
    }
}
```

Method which gets enumerable collection of operations and then calls Input method for every single operation:
```csharp
public void Input<T>(IEnumerable<T> data) where T : IEntity
{
    foreach (var item in data)
    {
        Input(item);
    }
}
```

Method gets dictionary and then calls Input method for every single operation: 
```csharp
public void Input(IDictionary<string, object> data)
{
    var entity = new Operation();
    foreach (var pair in data)
    {
        PropertyInfo property = typeof(Operation).GetProperty(pair.Key);
        if (property.CanWrite)
            property.SetValue(entity, pair.Value);
        }
        Input(entity);
    }
```
Method gets one operation, adds a row to the table and writes text to the cells of this row. First 6 columns of a table ("Received" information) will be rendered at the first page and the last 4 columns ("Sent" information) will be rendered at the second page.
If operation is discarded, we strike out the whole row or only letters in cells depending on the setting DiscardedStrikethroughMode and add a row with the discard reason.
Add row to the table and then add cells with the text to the row by calling AddCellToRow() method returning TableRowBuilder, or AddCell() method returning TableCellBuilder:
```csharp
public void Input(Operation book)
{
    TableRowBuilder row = _table.AddRow();
    // Received                              
    row
        .AddCellToRow(book.Code)
        .AddCellToRow(book.Manufacturer)
        .AddCellToRow(book.Model)
        .AddCellToRow(book.VIN)
        .AddCellToRow(book.Received.HasValue ? book.Received.Value.ToString(DateFormat) : "")
        .AddCell(book.Source);
    // Sent
    row
        .AddCellToRow(book.Code)
        .AddCellToRow(book.Sent.HasValue ? book.Sent.Value.ToString(DateFormat) : "")
        .AddCellToRow(book.Buyer_Name)
        .AddCell(book.FullAddress);
```
Strike out discarded operation by calling SetStrikethrough method:
```csharp
if (book.Discarded)
{
    row
        .SetFont(exoItalic)
        .SetStrikethrough(_options.DiscardedStrikethroughMode == StrikethroughMode.FullRow, _options.DiscardedStrikethroughColor, _options.DiscardedStrikethroughStroke);
}
```
For discarded operation add new row and print discard reason there:   
```csharp
if (book.Discarded && !string.IsNullOrEmpty(book.DiscardReason))
{
    row = _table.AddRow();
    row
        .SetFont(exoItalic).SetFontSize(8).SetFontColor(Color.Gray)
        .SetBackColor(Color.FromRgba(1.0, 1.0, 0.9))
        .AddCellToRow("")
        .AddCellToRow(book.DiscardReason, 2)
        .AddCellToRow("")
        .AddCellToRow("")
        .AddCellToRow("")
        .AddCellToRow("")
        .AddCellToRow("")
        .AddCellToRow("")
        .AddCellToRow("");
}
```

5.4. **Create sections**.
First section is a cover of our book, second section includes a table with 10 columns, where later will be placed operations from input file, and the third section contains summary information.
Method CreateSections() will be called from LogBookCoordinator constructor.
```csharp
private IEnumerable<Section> CreateSections()
```
Create cover page section:
```csharp
// Section 1
var s1 = SectionBuilder.New()
    .SetStyleFont(exoRegular)
    .SetMargins(50)
    .SetSize(_options.PaperSize)
    .SetOrientation(_options.Orientation)
    .AddParagraph("Operations Log")
        .SetAlignment(HorizontalAlignment.Center)
        .SetFontSize(46)
        .SetMarginTop(50)
.ToSection()
    .AddParagraph("Book")
        .SetAlignment(HorizontalAlignment.Center)
        .SetFontSize(56)
        .SetMarginTop(10)
.ToSection()
    .AddParagraph("Powered by PDFFlow")
        .SetFont(exoBold)
        .SetAlignment(HorizontalAlignment.Right)
        .SetMarginTop(100)
.ToSection();
```
Create main section with a table containing operations log. Then set Section's properties (size, orientation and numeration style):
```csharp
// Section 2
var s2 = SectionBuilder.New()
    .SetStyleFont(exoRegular)
    .SetMargins(_options.SectionMargins)
    .SetSize(_options.PaperSize)
    .SetOrientation(_options.Orientation)
    .SetPageNumberStart(_options.StartingPage - 1);
```
Call AddRepeatingAreas() method (will be described later) which adds headers and footers above the table and below the table in the current section:
```csharp
AddRepeatingAreas(s2);
```
Call Section's method **AddTable()** to add table to the second section and save it to the _table variable. Set repeatHeaders to true, which means that you will see table header at every page, not only at the beginning of a table. Then set only top and bottom borders of the cells to be visible by setting left and right borders to value None. TextOverflowAction will behave as described in the library documentation:
```csharp
_table = s2.AddTable();
_table
    .SetRepeatHeaders(true)
    .SetBorderStroke(Stroke.None, Stroke.Solid, Stroke.None, Stroke.Solid)
    .SetContentRowStyleTextOverflowAction(_options.TextOverflowAction);
```
Call Table's method **AddTitleRow()**, configure row's style and add cells. You will see title row at every page. Add empty title above the first column calling AddCell(""). Next column we will call "Description of model" and we'll see it at every even page. Then call AddCell("", 5) where second parameter is colspan, which means that next 5 cells will be merged together.
```csharp
    table.AddTitleRow(r =>
    {
        var style = r.CellsStyle.Clone().SetHorizontalAlignment(HorizontalAlignment.Left)
            .SetVerticalAlignment(VerticalAlignment.Bottom).SetBackColor(Color.White)
            .SetMinHeight(25).SetFont(new Font { Name = FontNames.Exo, Size = 13 });
        style.Border = new Border();
        style.SetBorderStyle(Stroke.None);
        style.Border.Top.Style = Stroke.Solid;
        r.AddCell("").SetStyle(style);
        r.AddCell("Description of model").SetStyle(style);
        r.AddCell("", 5).SetStyle(style);
        r.AddCell("Reciept").SetStyle(style);
        r.AddCell("", 3).SetStyle(style);
    });
```
Make table to become two-page spread by calling method **SetMultipageSpread** and passing columns per page as params array. We need 6 columns at the first page and 4 columns at the second page, so we pass values 6 and 4:
```csharp
_table.SetMultipageSpread(6, 4);
```

Call method **AddColumnPercentToTable()** with header text as first parameter and value in percents as second parameter to add header to the table and automatically calculate column's width.
Total width of the first 6 columns is 100%, because we want them to fit the whole first page's width. Total width of the rest 4 columns is 100% either, because we want them to fit the whole second page's width.
```csharp
_table
    .AddColumnPercentToTable("#", 5)
    .AddColumnPercentToTable("Manufacturer", 24)
    .AddColumnPercentToTable("Model", 16)
    .AddColumnPercentToTable("VIN", 24)
    .AddColumnPercentToTable("Date Received", 16)
    .AddColumnPercentToTable("Source", 15)
    .AddColumnPercentToTable("#", 5)
    .AddColumnPercentToTable("Date Sent", 16)
    .AddColumnPercentToTable("Buyer Name", 30)
    .AddColumnPercentToTable("Buyer Address", 49);
});
```
Create second table's header under the first header:
```csharp
_table
    .SetHeaderRowStyleFont(exoBold)
    .SetHeaderRowStyleBackColor(Color.FromRgba(.8f, .8f, .8f))
    .SetHeaderRowStyleHorizontalAlignment(HorizontalAlignment.Left)
    .AddHeaderRow()
    .ApplyStyle(style)                        
    .SetMinHeight(25)
    .AddCellToRow("")
    .AddCellToRow("Description of model", 5)
    .AddCellToRow("")
    .AddCellToRow("Reciept", 3);
```
Create the last section with the summary information and return collection of Sections:
```csharp
// Section 3

var s3 = SectionBuilder.New().SetStyleFont(exoRegular.SetSize(11));

string dateFormat = DateFormat;

s3
    .SetMargins(_options.SectionMargins)
    .SetSize(_options.PaperSize)
    .SetOrientation(_options.Orientation)
    .AddParagraph("Book Name")
        .SetAlignment(HorizontalAlignment.Center)
        .SetMargins(0, 200, 0, 0)
.ToSection()    
    .AddParagraph(_options.BookName)
        .SetAlignment(HorizontalAlignment.Center)
        .SetFont(exoBold)
.ToSection()
    .AddParagraph("Date of Print")
        .SetAlignment(HorizontalAlignment.Center)
        .SetMargins(0, 10, 0, 0)
.ToSection()
    .AddParagraph(_options.DateOfPrint.Value.ToString(dateFormat))
        .SetAlignment(HorizontalAlignment.Center)
        .SetFont(exoBold)
.ToSection()
    .AddParagraph("Date range")
        .SetAlignment(HorizontalAlignment.Center)
        .SetMargins(0, 10, 0, 0)
.ToSection()
    .AddParagraph(string.Concat(_options.DateRangeStart.Value.ToString(dateFormat), " - ", _options.DateRangeEnd.Value.ToString(dateFormat)))
    .SetAlignment(HorizontalAlignment.Center)
    .SetFont(exoBold)
.ToSection()
    .AddParagraph("Number of Records")
    .SetAlignment(HorizontalAlignment.Center)
    .SetMargins(0, 10, 0, 0)
.ToSection()
    .AddParagraph(_options.NumberOfRecords.ToString())
    .SetAlignment(HorizontalAlignment.Center)
    .SetFont(exoBold);

return new List<SectionBuilder> {s1, s2, s3};
}
```
Create method BuildSections() to add sections to DocumentBuilder. It can be made by calling DocumentBuilder's method **AddSection()**:
```csharp
private void BuildSections()
{
    foreach (Section section in _sections)
    {
        _builder.AddSection(section);
    }
}
```

#### 6. Add repeating headers and footers
Implement AddRepeatingAreas() method. Call AddHeader(true) to add Odd header, AddHeader(false) to add Even header. Same with the footers:
```csharp
private void AddRepeatingAreas(Section section)
{
    AddHeader(true);  
    AddHeader(false); 
    AddFooter(true);  
    AddFooter(false); 
```
In order to **add header** to the section, create AddHeader() method. Call **section.AddHeaderToOddPage()** to create repeating header which will be printed only at Odd pages of the section. Call section.AddHeaderToEvenPage() to print repeating header only at Even pages. The same with footers. You can add as many repeating areas as you wish. Then add content to the repeating areas:
```csharp
private void AddRepeatingAreas(SectionBuilder section)
{
    AddHeader(true);
    AddHeader(false);
    AddFooter(true);
    AddFooter(false);

    void AddHeader(bool isOddPage)
    {
        string SentOrReceived = isOddPage ? "SENT" : "RECEIVED";
        string HeaderText = isOddPage ? _options.HeaderOdd : _options.HeaderEven;
        RepeatingAreaBuilder header; 
        if (isOddPage)
            header = section.AddHeaderToOddPage(50);
        else
            header = section.AddHeaderToEvenPage(50);
        header
            .AddTable()
                .SetBorderStroke(Stroke.None)
                .AddColumnToTable()
                .AddColumnToTable()
                .AddRow()
                .AddCell(SentOrReceived)
                        .SetFontSize(24)
                        .SetVerticalAlignment(VerticalAlignment.Bottom)
                .ToRow()
                    .AddCell(HeaderText)
                        .SetPadding(0, 0, 10, 0)
                        .SetHorizontalAlignment(HorizontalAlignment.Right)
                        .SetVerticalAlignment(VerticalAlignment.Bottom);
    }

    void AddFooter(bool isOddPage)
    {
        string dateRangeStr1 = "";
        string dateRangeStr2 = "";
        if (_options.DisplayDateRange && _options.DateRangeStart.HasValue && _options.DateRangeEnd.HasValue)
        {
            dateRangeStr1 = " | Date Range ";
            dateRangeStr2 = $"{_options.DateRangeStart.Value.ToString(DateFormat)} - {_options.DateRangeEnd.Value.ToString(DateFormat)}";
        }
        RepeatingAreaBuilder footer;
        if (isOddPage)
            footer = section.AddFooterToOddPage(30);
        else
            footer = section.AddFooterToEvenPage(30);
        footer
            .AddTable()
                .SetBorderStroke(Stroke.None)
                .SetContentRowStroke(Stroke.None)
                .AddColumnToTable()
                .AddColumnToTable()
                .AddRow()
                    .AddCell()
                            .AddParagraph()
                            .AddTextToParagraph("Page ")
                            .AddPageNumberToParagraph()
                            .AddTextToParagraph(" | ")
                            .AddText(_options.BookName)
                            .SetFont(exoBold)
                    .ToRow()
                        .AddCell()
                            .SetHorizontalAlignment(HorizontalAlignment.Right)
                            .AddParagraph()
                                .AddTextToParagraph("Date Of Print ")
                                    .AddText(_options.DateOfPrint.Value.ToString(DateFormat))
                                        .SetFont(exoBold)
                            .ToParagraph()
                                .AddText(dateRangeStr1)
                            .ToParagraph()
                                .AddText(dateRangeStr2)
                                    .SetFont(exoBold);
    }
}
```


#### 7. Dispose stream:
```csharp
public void Dispose()
{
    Dispose(true);
    GC.SuppressFinalize(this);
}
protected virtual void Dispose(bool disposing)
{
    if (_disposed)
        return;
    if (disposing)
    {
        if (_stream != null)
        {
            if (!_continuous)
                BuildSections();
                _stream.Dispose();
        }
    }
    _disposed = true;
}
```
#### 8. Generated **PDF file** should look as shown below:
The resulting LogBook.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/LogBook.pdf).