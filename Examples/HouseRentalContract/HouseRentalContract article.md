##### Example: HouseRentalContract
 
# Purpose
HouseRentalContract project is an example of the home rent contract generation. The example demonstrates creation of a simple multi-page text document. The document includes a repeating footer with automatic page numeration. The footer is made of two distinctive sections with different content for the initials area on all pages of the contract, except the page with full signatures. Additionally, this example demonstrates how to use Word-like tabulation and how to work with JSON as a data source.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/HouseRentalContract).

# Prerequisites
1) **Visual Studio 2017** or above is installed.
To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### Contract data
The contract content is located in the file **Content/contract.txt**.
This is a template with the contract text and the fields, which will be substituted with the values from the dictionary.
Such fields should be taken in braces: 
```txt
A deposit in the amount of {deposit} will be held.
```
Breaks between the paragraphs have to be indicated with {paragraphbreak}.
### Dictionary data
The values for the contract fields are located in the file **Content/contract_dictionary.json**:
```json
[
  {
    "Key": "renter",
    "Value": "John Smith"
  },
  {
    "Key": "guests",
    "Value": "4"
  }
]
```

### Output file
The example creates the file **HouseRentalContract.pdf** in the output **bin/(debug|release)/netcoreapp2.1** folder.


# Writing the source code

#### 1. Create new console application.
1.1.	Run Visual Studio
1.2.	File -> Create -> Console Application (.Net Core)

#### 2. Modify class Program.
2.1. In the function **Main()** set path to output PDF-file:
```c#
    Parameters parameters = new Parameters(null, "HouseRentalContract.pdf");
```
2.2. Call method Run() for house rental contract generation and Build() for building a document into an output PDF-file:
```c#
    HouseRentalContractRunner.Run().Build(parameters.file);
```
2.3. After the file is generated, notify the user about it:
```c#
    Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                        + "\" document has been successfully built");
```


#### 3. Create class for running document generation

Create new document by calling method New() of the DocumentBuilder. Then call AddHouseRentalContract() method for adding content to the document:
```c#
public static class HouseRentalContractRunner
{
    public static DocumentBuilder Run()
    {
        return DocumentBuilder.New()
            .ApplyStyle(StyleBuilder.New().SetLineSpacing(1.2f))
            .AddHouseRentalContract();
    }
}
```


#### 4. Build document structure

4.1. Create class **HouseRentalContractBuilder** which will contain method for building dicument structure
```c#
public static class HouseRentalContractBuilder
```

4.2. Under the Model subfolder create **class ContractDictionary**:
```c#
using System.Runtime.Serialization;

namespace HouseRentalContract.Model
{
    [DataContract]
    public class ContractDictionary
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
```

4.3. **Define fields** of the HouseRentalContractBuilder class.

Path to the output file:
```c#
private static readonly string PdfFile;
```

Path to the project directory:
```c#
private static readonly string ProjectDir;
```

Path to the input template text of the contract:
```c#
private static readonly string ContractTextFile
```

Path to the JSON file, containing values to be replaced in the template:
```c#
private static readonly string DictionaryJsonFile;
```

JSON content:
```c#
private static readonly string DictionaryJsonContent;
```

Different fonts:
```c#
private static readonly Font DocumentFont;
private static readonly Font TitleFont;
```

Contract text:
```c#
public static List<string> ContractData { get; set; }
```

Dictionary data:
```c#
public static List<ContractDictionary> DictionaryData { get; }
```


4.4. **Create constructor** static HouseRentalContractBuilder() and **initialize fields**:

Initialize path variables:
```c#
PdfFile = Path.Combine(Environment.CurrentDirectory, "Contract.pdf");
ProjectDir = Directory.GetCurrentDirectory();
ContractTextFile = Path.Combine(ProjectDir, "Content", "contract.txt");
DictionaryJsonFile = Path.Combine(ProjectDir, "Content", "contract_dictionary.json");
```

Read contract data:
```c#
ContractData = File.ReadAllLines(ContractTextFile).ToList();
```


Read dictionary data:
```c#
DictionaryJsonContent = File.ReadAllText(DictionaryJsonFile);
DictionaryData = JsonConvert.DeserializeObject<List<ContractDictionary>>(DictionaryJsonContent);
```

Initialize fonts:
```c#
DocumentFont = Fonts.Helvetica(16f);
TitleFont = Fonts.Times(16f).SetName("Times-Bold").SetSize(28f);
 ```       

4.5. Create method **AddHouseRentalContract()**.

This method will create two sections by calling AddSection() method on DocumentBuilder, set needed paper size, orientation and margins and call the methods for adding content to the sections. All these methods must be the extension methods of SectionBuilder and return SectionBuilder, so we can call them in an uninterrupted chain:
```c#
        public static DocumentBuilder AddHouseRentalContract(this DocumentBuilder builder)
        {
            builder.AddSection()
                .SetSize(PaperSize.Letter)
                .SetOrientation(PageOrientation.Portrait)
                .SetMargins(new Box(40, 80, 40, 40))
                .AddContractTitle()
                .ReplaceKeyWordsFromDictionary()
                .AddContractText()
                .AddFooter();

            builder.AddSection()
                .SetSize(PaperSize.Letter)
                .SetOrientation(PageOrientation.Portrait)
                .SetMargins(new Box(40, 80, 40, 40))
                .AddSignatures()
                .AddLastFooter();
            return builder;
        }
```


4.6. **Add contract title** as a Paragraph. 

Just pass paragraph text as a string parameter of AddParagraph(). The method returns Paragraph, so you can then set paragraph’s alignment, margins and font.  
```c#
        internal static SectionBuilder AddContractTitle(this SectionBuilder s)
        {
            s.AddParagraph("House Rental Contract")
                .SetAlignment(HorizontalAlignment.Center)
                .SetMargins(0, 0, 0, 20)
                .SetFont(TitleFont);
            return s;
        }
```

4.7. **Replace the keywords in the template** text with the values from dictionary:
```c#
        internal static SectionBuilder ReplaceKeyWordsFromDictionary(this SectionBuilder s)
        {
            ContractContent = ContractContent.ConvertAll(c => c.Replace("{paragraphbreak}", " "));
            foreach (ContractDictionary item in DictionaryData)
            {
                ContractContent = ContractContent.ConvertAll(c => c.Replace("{" + item.Key + "}", item.Value));
            }
            return s;
        }
```

4.8. **Add contract text** to the current section. 

Add each paragraph to the current section, set its font and alignment. 
```c#
        internal static SectionBuilder AddContractText(this SectionBuilder s)
        {
            foreach (var paragraph in ContractContent)
                s.AddParagraph(paragraph).SetFont(DocumentFont).SetJustifyAlignment(true);
            return s;
        }
```

4.9. **Add footer with page number and tabulation**.

Method AddFooterToBothPages() of the SectionBuilder adds a footer to every page. You could also call AddFooterToOddPage() to add this footer only to odd pages, or AddFooterToEvenPage() for even pages only. You can add as many footers as you need, and they will be placed above each other. We need one here.

Method AddPageNumberToParagraph() will render automatically calculated number of the page.
As we need a line for the signature and the text “(Initials)” under it center-aligned regarding to the line, we use **word-like tabulation** for this purpose. There are two methods to indicate tabulation positions in Paragraph: AddTabulation (sets the absolute position in pixels) and AddTabulationInPercent (sets the position in percents). In the first row of the paragraph we need an underlined place for the signature, so we jump to the position of 60 % with an empty leading and then jump to the position of 100 % using BottomLine leading. As we need the text “(Initials)” to be center-aligned relative to this line, we jump to the center-type tabulation positioned at 80 %:
```c#
        internal static SectionBuilder AddFooter(this SectionBuilder s)
        {
            s.AddFooterToBothPages(45)
                .AddParagraph()
                    .SetMargins(20, 0, 0, 0)
                    .AddPageNumberToParagraph()
                    .AddTabSymbol()
                    .AddTabSymbol()
                    .AddTabulationInPercent(60, TabulationType.Left)
                    .AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine)
            .ToArea()
                .AddParagraph()
                    .AddTabSymbol()
                    .AddTextToParagraph("(Initials)")
                    .AddTabulationInPercent(80, TabulationType.Center);
            return s;
        }
```

4.10. **Add signatures** in the second section. 

Use tabulation in percents as described above. 

In the first row of paragraph we will have a line from the beginning to 20% and another line from 60% to 100%. In the second row of paragraph there will be a text "(Date)" left-aligned around position of 10% and a text "(Renter)" center-aligned around 80 %:
```c#
        internal static SectionBuilder AddSignatures(this SectionBuilder s)
        {
            foreach (var paragraph in LastPageContent)
                s.AddParagraph(paragraph).SetFont(DocumentFont).SetJustifyAlignment(true);

            s.AddParagraph()
                .SetMargins(30, 0, 0, 0)
                .AddTabSymbol().AddTabulationInPercent(20, TabulationType.Right, TabulationLeading.BottomLine)
                .AddTabSymbol().AddTabSymbol().AddTabulationInPercent(60, TabulationType.Left).AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
            s.AddParagraph()
                .AddTabSymbol().AddTextToParagraph("(Date)").AddTabulationInPercent(10, TabulationType.Center)
                .AddTabSymbol().AddTextToParagraph("(Renter)").AddTabulationInPercent(80, TabulationType.Center);
            s.AddParagraph(" ");
            s.AddParagraph()
                .AddTabSymbol().AddTabulationInPercent(20, TabulationType.Right, TabulationLeading.BottomLine)
                .AddTabSymbol().AddTabSymbol().AddTabulationInPercent(60, TabulationType.Left).AddTabulationInPercent(100, TabulationType.Right, TabulationLeading.BottomLine);
            s.AddParagraph()
                .AddTabSymbol().AddTextToParagraph("(Date)").AddTabulationInPercent(10, TabulationType.Center)
                .AddTabSymbol().AddTextToParagraph("(Landlord)").AddTabulationInPercent(80, TabulationType.Center);

            return s;
        }
```

4.11. **Add footer with page number** in the second section:
```c#
        internal static SectionBuilder AddLastFooter(this SectionBuilder s)
        {
            s.AddFooterToBothPages(45)
                .AddParagraph()
                .SetMargins(20, 0, 0, 0)
                .AddPageNumber();
            return s;
        }	
```


#### 5. Generated **PDF file** should look as shown below:
The resulting HouseRentalContract.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/HouseRentalContract.pdf).
