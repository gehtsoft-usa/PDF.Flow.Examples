##### Example: The BankAccountStatement

The **BankAccountStatement** project is an example of creating a **Bank Account Statement** document. The example shows how to create a complex document that includes tables, nested tables and lists, as well as creating forms using tabulation. The example showcases a way  to create a two-columns page using "Left" and "Right" repeating area.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/BankAccountStatement).

**Table of Contents**

- [Prerequisites](#prerequisites)
- [Purpose](#purpose)
- [Description](#description)
- [Output file](#output-file)
- [Open the project in Visual Studio](#1-open-the-project-in-visual-studio)
- [Run the sample application](#2-run-the-sample-application)
- [Source code structure](#3-source-code-structure)
- [Class Program](#4-class-program)
- [The BankAccountStatementRunner class](#5-the-bankaccountstatementrunner-class)
- [The BankAccountStatementBuilder class](#6-the-bankaccountstatementbuilder-class)
- [The BankAccountStatementFirstPageBuilder class](#7-the-bankaccountstatementfirstpagebuilder-class)
- [The BankAccountStatementSecondPageBuilder class](#8-the-bankaccountstatementsecondpagebuilder-class)
- [The BankAccountStatementLastPageBuilder class](#9-the-bankaccountstatementlastpagebuilder-class)
- [The Helper class](#10-the-helper-class)
- [Summary](#summary)

# Prerequisites
1) **Visual Studio 2017** or above is installed.
To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core Framework SDK 2.1** or above is installed.
To install the framework use the following link: https://dotnet.microsoft.com/download

# Purpose
The example shows how to create a **Bank Account Statement** (Fig. 1), which is a complex multi-page document. 

The names of the logical areas of the document are marked in blue in Figure 1.

The first or front page consists of:
* Title with bank name;
* Company name;
* Questions;
* Options;
* Advertisement;
* Account summary;
* Account info.

The second and the following page(s) contain a table with transaction history and tables with the service fees. The pages consist of:

* Title;
* Overdraft protection;
* Transaction history;
* Service fee summary;
* Transaction fee summary.

A number of the following pages, including the last page, depends on the size of the transaction history. The pages are added automatically without any code change, since the data an external file is used, and **the library manages addition of  the pages.** resulting in much less code to write.

The last page consists of:

* Title;
* Polices;
* Instructions;
* Balance calculation;
* Footer.

  ![Fig. 1](../Articles%20Images/BankAccount_Ill.png "The first page")

Fig. 1

### Description
###### Output file
The example creates the **BankAccountStatement.pdf** file in the output **bin/(debug|release)/netcoreapp2.1** folder, unless otherwise specified in the command line.

#### 1. Open the project in Visual Studio
There are 2 ways to open the project:

1. After installing Visual Studio, you just need to double click on the project file of the BankAccountStatement.csproj.

2. From inside Visual Studio: 
* Run Visual Studio
* File -> Open -> Project/Solution -> Choose the BankAccountStatement.csproj 

#### 2. Run the sample application
Running from Visual Studio:
    - Press Ctrl-F5 key.

Running from the command line:

In the directory with the BankAccountStatement.csproj, run the command: 

```
dotnet run
```

You can get optional parameters of the command line using the command:


```
dotnet run -help
```

that shows specifications of the command line options:

```
Usage: dotnet run [fullPathToOutFile] [appToView]
Where: fullPathToOutFile - a path to the result file, 'BankAccountStatement.pdf' by default
appToView - the name of an application to view the file immediately after preparing, by default none app starts
```

#### 3. Source code structure

The source code consists of several files (classes).

There is a number of reasons to create several classes, in descending order of importance:

* Separation of responsibility. Class Program processes the command line options, any possible errors during the document creation, and displays a report of the errors. The rest of the classes do not process the command line options and errors. They are used only to build the document.
* Reduction of responsibility of each class. This can help limit the size of the code for each class, which improves understanding of the code example.
* The document consists of several pages; each class  builds its own page.

#### 4. Class Program
Responsibility:
* Parse command line options. It processes the command line options using the **PrepareParameters** that prepares the **Parameters** structure.
* Display prompts to user when one of the following options is presented in the command line: "?", "-h", "-help", "--h", "--help". It is processed by the  **Usage**.
* Build document. It delegates the document building to the **BankAccountStatementRunner** class with the name of the resulting document file.
* Run the application to demonstrate the resulting document if it is specified in the command line options.

#### 5. The BankAccountStatementRunner class
The responsibility of the  **BankAccountStatementRunner.Run**:
* Read the content of the file: *Content/sample-statement-data.json*;
* Convert it to a list of objects of the **Model.Statement** class;
* Read the content of the file: *Content/sample-statement-info-data.json*;
* Convert it to an object of the **Model.StatementInfo** class;
* Create an object of the **BankAccountStatementBuilder** class and initialize it using the converted data;
* Build the document.

It delegates the document building to the **BankAccountStatementBuilder** class using the **Build** method.

##### The data format

The Account Statement data is contained in the files *Content/sample-statement-data.json* and *Content/sample-statement-info-data.json*.
This data is converted into the objects of the **Model.Statement** class and the **Model.StatementInfo** class respectively.

In a real application this data can be received from an external source, such as a database.

##### The *sample-statement-data.json* file and Model.Statement class

This file contains a JSON array of all operations performed on the user's account for the given period. Items of the array are JSON objects, each containing the data for one operation:

*sample-statement-data.json*:

```json
[
  {
    "Date": "2020-05-26T00:00:00",
    "Check": null,
    "Details": "Mobile Deposit : Ref Number :677577755404",
    "Deposits": 300,
    "Withdrawals": 0,
    "EndingDailyBalance": 850
  },
  .....
  {
    "Date": "2020-06-24T00:00:00",
    "Check": "1258",
    "Details": "Check",
    "Deposits": 0,
    "Withdrawals": 42.93,
    "EndingDailyBalance": 2417.96
  }
]

```

The **BankAccountStatementRunner.Run** method converts this JSON array into a list of objects of the **Model.Statement** class:

###### The **Model.Statement** class:

```csharp
using System;
using System.Globalization;

namespace BankAccountStatement.Model
{
    public class Statement
    {
        public DateTime Date { get; set; }
        public string Check { get; set; }
        public string Details { get; set; }
        public double Deposits { get; set; }
        public double Withdrawals { get; set; }
        public double EndingDailyBalance { get; set; }
    }
}

```

##### The *sample-statement-info-data.json* and the Model.Statement class

This file contains the JSON object. The object contains the general data of the entire bank account statement.

*sample-statement-info-data.json*:

```json
{
  "BankName": "SampleBank",
  "BankNameState": "N.A.",
  "AccountNumber": "1234567890",
  "DateBegin": "2020-05-26T00:00:00",
  "DateEnd": "2020-06-24T00:00:00",
  "CompanyName": "CompanyName",
  "CompanyAddress": "123 Main Street\nAnywhere NY 12345-6789",
  "ReportAddress": "Overdraft Collections and Recovery, P.O. Box 1234, Anywhere, OR 12345-6789",
  "PhoneFree": "1-800-CALL-XXXXX",
  "Phone": "1-234-567-8900",
  "TTY": "1-234-567-8900",
  "Online": "samplebank.com",
  "White": "SampleBank, N.A.\nP.O. Box 1234\nNewYork, OR 12345-6789",
  "BusinessPlan": "samplebank.com/business-plan-center",
  "AccountOptions": "samplebank.com/biz",
  "Advt": "The SampleBank Mobile App is now available in Spanish!\n\nYou can securely manage your finances virtually anytime, anywhere in Spanish.\nOnce you have downloaded the latest version of the SampleBank MobileВ® App from Google Play or the Apple App Store, go to Mobile Settings and set your language preference to Spanish.",
  "BeginningBalance": 550,
  "Withdrawals": -1639.32,
  "Deposits": 3820,
  "EndingBalance": 2417.96,
  "AverageBalance": 1186.58,
  "DepositRTN": "123456789",
  "WireRTN": "987654321",
  "StandartServiceFee": 10,
  "MinimumRequired": 500,
  "ServiceFee": 0,
  "ServiceDiscount": 5,
  "TransactionUnits": 17,
  "TransactionUnitsIncluded": 50,
  "TransactionExcessUnits": 0,
  "ServiceCharge": 0.50,
  "TotalServiceCharge": 0,
  "FeedBackPhone": "1-234-XXX-CARE"
}

```

The **BankAccountStatementRunner.Build** method converts the JSON object into an object of the **Model.StatementInfo** class:

###### The **Model.StatementInfo** class:
```csharp
using System;
using System.Globalization;

namespace BankAccountStatement.Model
{
    public class StatementInfo
    {
        public string BankName { get; set; }
        public string BankNameState { get; set; }
        public string AccountNumber { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneFree { get; set; }
        public string Phone { get; set; }
        public string TTY { get; set; }
        public string Online { get; set; }
        public string White { get; set; }
        public string ReportAddress { get; set; }
        public string BusinessPlan { get; set; }
        public string AccountOptions { get; set; }
        public string Advt { get; set; }
        public double BeginningBalance { get; set; }
        public double Withdrawals { get; set; }
        public double Deposits { get; set; }
        public double EndingBalance { get; set; }
        public double AverageBalance { get; set; }
        public string DepositRTN { get; set; }
        public string WireRTN { get; set; }
        public double StandartServiceFee { get; set; }
        public double MinimumRequired { get; set; }
        public double ServiceFee { get; set; }
        public double ServiceDiscount { get; set; }
        public int TransactionUnits { get; set; }
        public int TransactionUnitsIncluded { get; set; }
        public int TransactionExcessUnits { get; set; }
        public double ServiceCharge { get; set; }
        public double TotalServiceCharge { get; set; }
        public string FeedBackPhone { get; set; }

        public static string ToString(double value)
        {
            return String
                .Format(CultureInfo.InvariantCulture, "{0:0,0.00}", value);
        }
    }
}
```
#### 6. The BankAccountStatementBuilder class
Responsibility:
* Save a list of the **Model.Statement** objects in the **statement** property; 
* Save an object of the **Model.StatementInfo** class in the **statementInfo** property.

The **Build** responsibility is to:

* Create an object of the **Gehtsoft.PDFFlow.Builder.DocumentBuilder** class;
* Create an object of the **BankAccountStatementFirstPageBuilder** class, initialize it with **statementInfo** property and build the first page using the **Build** method;
* Create an object of the **BankAccountStatementFirstPageBuilder** class, initialize it with the **statementInfo** property and the **statement** property  and build the other page(s) using the **Build** method;
* Create an object of the **BankAccountStatementLastPageBuilder** class, initialize it with **statementInfo** property  and build the last page using the **Build** method;
* Return the created object of the **Gehtsoft.PDFFlow.Builder.DocumentBuilder** class.


#### 7. The BankAccountStatementFirstPageBuilder class

The **BankAccountStatementFirstPageBuilder.Build method** responsibility is to:
* Create the page and set its parameters;

```csharp
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
```
* Create the header with the bank name;

```csharp
            AddTitle(sectionBuilder.AddHeaderToBothPages(136).AddTable(), 
                20f, 4, AddMainTitleWithBankName);
```
* Add 2 columns with the "Company Name" and the "Question" blocks;
```csharp
            AddTwoPanels(sectionBuilder, 30f, AddCompanyName, AddQuestionsToCell);
```
* Add the "Options" block;
```csharp
            AddOptions(sectionBuilder, 30f);
```
* Add the "Advertisement" block;
```csharp
            AddAdvt(sectionBuilder, 12f);
```
* Add 2 columns with the "Account summary" and the "Account info" blocks.
```csharp
            AddTwoPanels(sectionBuilder, 0f, AddActivitySummary, AddAccountInfo);
```

#### The Title display

The Title on the first page (See Fig. 3) is different from the Title on the other pages. It contains:

* The Bank Name;
* The text that is common to all pages: the account number, the start and end dates of the account statement, and the page numbers;
* The bank logo that is common to all pages.
  ![Fig. 2](../Articles%20Images/First_Title.png "The first page title")
Fig. 2

In order to avoid code duplication, creation of the Title on the first and other pages is performed using the methods of the **BankAccountStatement** class that is the base for the page building classes. 

###### The BankAccountStatement.AddTitle method

The method adds the table with one row and 2 columns consisting of:
* The Title text in the left column;
* The bank logo in the right column.

The method takes the following parameters:

* The **cellBuilder** - an object of the **Gehtsoft.PDFFlow.Builder.TableCellBuilder** class that will be used for the Title;
* The **bottomMargin** - the margin values below the table, float;
* The **pageCount** - the number of the document pages, int;
* The **AddTitleAction** - the action to fill the left column.

```charp
        protected void AddTitle(TableBuilder tableBuilder, float bottomMargin, 
            int pageCount, Action<TableCellBuilder, int> AddTitleAction)
        {
            tableBuilder
                .SetBorder(Stroke.None)
                .SetWidth(PageWidth)
                .AddColumnPercentToTable("", 90).AddColumnPercentToTable("", 10)
                .SetMarginBottom(bottomMargin);
            var rowBuilder = tableBuilder.AddRow();
            AddTitleAction(rowBuilder.AddCell(), pageCount);
            AddLogo(rowBuilder.AddCell());
        }

```

The **BankAccountStatementFirstPageBuilder.Build** method passes the **BankAccountStatement.AddTitleWithBankName** method as a **AddTitleAction** parameter in order to add the Bank Name to the common Title (See Fig.5)

###### The BankAccountStatement.AddTitleWithBankName method

The title must contain the account number, the start and end dates of the account statement, and the page numbers. The method adds a paragraph with the Bank Name and calls the **BankAccountStatement.AddTextTitle** method that adds the common text of the Title.


```csharp
        protected void AddMainTitleWithBankName(TableCellBuilder cellBuilder, 
            int pageCount)
        {
            AddParagraph(cellBuilder, 10f, statementInfo.BankName +
                            " Simple Business Checking", FNT18_3B);
            AddTextTitle(cellBuilder, pageCount, 0);
        }

```

The paragraph is added using the **Helper.AddParagraph** method that has a parameter of the **Gehtsoft.PDFFlow.Builder.TableCellBuilder** class.
The static class **Helper** was created to avoid code duplication. The methods of the class will be described at the end of the article. 

###### The BankAccountStatement.AddTextTitle method

In order to add the account number, the start and end dates of the account statement, and the page numbers according to the design, this method creates a table with five columns and one row. The data location is controlled by the width of the table and the width of the columns.

Adding data to the row is delegated to the **BankAccountStatement.AddStatementInfoToTitle** method.

```csharp
        protected void AddTextTitle(TableCellBuilder cellBuilder, int pageCount, float topMargin)
        {
            cellBuilder.AddTable(tableBuilder => 
            {
                tableBuilder
                    .SetMarginTop(topMargin)
                    .SetBorder(Stroke.None)
                    .SetWidth(XUnit.FromPercent(70))
                    .AddColumnPercentToTable("", 23)
                    .AddColumnPercentToTable("", 18)
                    .AddColumnPercentToTable("", 42)
                    .AddColumnPercent("", 17);
                AddStatementInfoToTitle(tableBuilder.AddRow(), pageCount);
            });
        }
```


###### The BankAccountStatement.AddStatementInfoToTitle method

The method adds the account number, the start and end dates of the account statement, and the page numbers to the table's row.

```csharp
        private void AddStatementInfoToTitle(TableRowBuilder tableRowBuilder, int pageCount)
        {
            tableRowBuilder.AddCell("Account number:").SetFont(FNT8_9);
            tableRowBuilder.AddCell(statementInfo.AccountNumber + " ").SetFont(FNT9B);
            tableRowBuilder.AddCell(statementInfo.DateBegin.ToString(
               "MMMM dd, yyyy", DocumentLocale) +
                " - " +
                statementInfo.DateEnd.ToString(
                "MMMM dd, yyyy", DocumentLocale)).SetFont(FNT8_9);
            var paragraphBuilder = tableRowBuilder.AddCell().SetFont(FNT8_9).AddParagraph();
            paragraphBuilder
                .AddTextToParagraph("Page ")
                .AddPageNumberToParagraph()
                .AddTextToParagraph(" of " + pageCount);
        }

```

###### The BankAccountStatement.AddLogo method

The method adds the bank logo to the table's cell.

```csharp
            cellBuilder
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .AddImageToCell(Path.Combine("images", "BS_Logo_2x.png"));

```


###### The AddTwoPanels method

The **BankAccountStatementFirstPageBuilder.Build** uses the **AddTwoPanels** method to create 2 blocks of the same type. See Fig. 3 and Fig. 4.

To add the content of the blocks, two parameters are passed to the method: the actions to fill the left and right parts of the each block. 

The method:

* Creates a table with the full width of the page. The table consists of two columns with widths 62% and 38% of the page width;
* Adds one row to the table;
* Creates two cells in the row and sets the right border for the left cell;
* Calls the actions to fill the left and right cells.

The method takes the parameters: 
* The **SectionBuilder** - an object of the **Gehtsoft.PDFFlow.Builder.SectionBuilder** class of the section where the two blocks are added; 
* The **bottomMargin** - a margin below this block;
* The **LeftPanelAction** - the action to fill the left part of the block;
* The **RightPanelAction** - the action to fill the right part of the block.


```csharp
        private void AddTwoPanels (SectionBuilder sectionBuilder, float bottomMargin, 
            Action<TableCellBuilder> LeftPanelAction, 
            Action<TableCellBuilder> RightPanelAction)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .SetWidth(PageWidth)
                .AddColumnPercentToTable("", 62).AddColumnPercentToTable("", 38)
                .SetMarginBottom(bottomMargin);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .ApplyStyle(StyleBuilder.New()
                    .SetBorderRight(2f, Stroke.Solid, Color.Black));
            LeftPanelAction(cellBuilder);
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                    .SetBorderStroke(Stroke.None)
                    .SetPadding(8, 0, 0, 0);
            RightPanelAction(cellBuilder);
        }

```

![Fig. 3](../Articles%20Images/BankAccountStatementFirst2Panel1.png "Two panels 1")

Fig. 3

To create the block in Fig. 3, the  **BankAccountStatementFirstPageBuilder.Build** passes the following actions to the **AddTwoPanels**:

* The **AddCompanyName** method;
* The **AddQuestions** method.

###### The AddCompanyName method

The method adds the text paragraph with the Bank Name to the table cell to fill the left part of the block shown in Fig. 3
```csharp
        private void AddCompanyName(TableCellBuilder cellBuilder)
        {
            AddParagraph(cellBuilder, 19.2f, statementInfo.CompanyName +
                    "\n" + statementInfo.CompanyAddress, FNT8_9);
        }

```


###### The AddQuestions method

The method to fill the right part of the block shown in Fig. 3:
* Adds the title of the "*Questions?*" block using the **Helper.AddParagraph** method;
* Adds the text paragraph of the "*Available by phone 24 hours...*" block using the **Helper.AddParagraph**;
* Adds the text paragraph with the text with different fonts for the "1-800-CALL-XXXXX(1-234-567-8900)" block;
* Adds the table with the contact information.

```csharp
        private void AddQuestions(TableCellBuilder cellBuilder)
        {
            AddParagraph(cellBuilder, "Questions?", FNT10_5B, 6f);
            AddParagraph(cellBuilder, "Available by phone 24 hours a day, 7 days a week:\n" +
                "Telecommunications Relay Services calls accepted", FNT7_9);
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder.SetMarginBottom(6);
            paragraphBuilder.AddText(statementInfo.PhoneFree).SetFont(FNT9_8B);
            paragraphBuilder.AddText("(" + statementInfo.Phone + ")").SetFont(FNT7_5);
            cellBuilder.AddTable(FillCallCentersTable);
        }

```



![Fig. 4](../Articles%20Images/BankAccountStatementFirst2Panel2.png "Two panels 2")

Fig. 4

To create the block shown in Fig. 4, the **BankAccountStatementFirstPageBuilder.Build** passes the following actions to the **AddTwoPanels** method:

* The **AddActivitySummary** method;
* The **AddAccountInfo** method.

###### The AddActivitySummary method

The method fills the left part of the block shown in Fig. 4: 
* adds the nested table to the cell of the external table;
* fills the table using the **FillActivitySummaryTable** method.

```csharp
        private void AddActivitySummary(TableCellBuilder outerCellBuilder)
        {
            outerCellBuilder.AddTable(FillActivitySummaryTable);
        }
```

###### The FillActivitySummaryTable method

The method fills the table with the account summary data:

* Sets the table;
```csharp
            tableBuilder
                .SetWidth(XUnit.FromPercent(80))
                .SetBorder(Stroke.None)
                .SetAltRowStyleBackColor(Color.White)
                .AddColumnPercentToTable("", 75).AddColumnPercent("", 25);
```
* Creates the first row with one cell in the table;
```csharp
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .ApplyStyle(
                    StyleBuilder.New()
                        .SetBorderLeftStroke(Stroke.None)
                        .SetBorderTop(
                            0.5f, Stroke.Solid, Color.Black)
                        .SetBorderRightStroke(Stroke.None)
                        .SetBorderBottomStroke(Stroke.None)
                        .SetFont(FNT10_5B)
                )
```
* Expands the cell to 2 columns;
```csharp
                .SetColSpan(2)
```
* Fills the cell with the title text;
```csharp
                .AddParagraphToCell("Activity summary");
```
* Fills the rows of the table using the **FillActivitySummaryTableRow** method by passing the **Field** array with the account summary data to the table.  
```csharp
            FillActivitySummaryTableRow(tableBuilder, new Field[] {
                        new Field("Beginning balance on " +
                            statementInfo.DateBegin.ToString(
                                "MMMM dd, yyyy", DocumentLocale),
                            "$" + StatementInfo.ToString(statementInfo.BeginningBalance)),
                        new Field("Deposits/Credits",
                            StatementInfo.ToString(statementInfo.Deposits)),
                        new Field("Withdrawals/Debits",
                            StatementInfo.ToString(statementInfo.Withdrawals)),
                        new Field("Ending balance on " +
                            statementInfo.DateEnd.ToString("MMMM dd, yyyy",
                            DocumentLocale),
                            "$" + StatementInfo.ToString(statementInfo.EndingBalance)),
                        new Field("Average ledger balance this period",
                            "$" + StatementInfo.ToString(statementInfo.AverageBalance))
                    });

```

###### The FillActivitySummaryTableRow method

The method adds the rows to the table and fills them using the **Field** array with the account summary data.

The rows' cells have four different styles (See Fig. 4):
* The text of the fourth row must have Bold font;
* The right cell must be Right-aligned.

The large margin must be above the last row.

Therefore, the method first defines four styles. Then, the method adds the rows applying the style and using the **TableCellBuilder.SetPadding** for the last line in the **for** cycle through the array. 

```csharp
        private void FillActivitySummaryTableRow(TableBuilder tableBuilder, Field[] fields)
        {
            StyleBuilder style1l = StyleBuilder.New().SetBorderStroke(Stroke.None);
            StyleBuilder style1r = StyleBuilder.New().SetBorderStroke(Stroke.None);
            StyleBuilder style2l = StyleBuilder.New()
                .SetBorderRightStroke(Stroke.None)
                .SetBorderTop(2f, Stroke.Solid, Color.Black)
                .SetBorderLeftStroke(Stroke.None)
                .SetBorderBottomStroke(Stroke.None);
            StyleBuilder style2r = StyleBuilder.New()
                .SetBorderRightStroke(Stroke.None)
                .SetBorderTop(2f, Stroke.Solid, Color.Black)
                .SetBorderLeftStroke(Stroke.None)
                .SetBorderBottomStroke(Stroke.None);
            for (int i = 0, l = fields.Length; i < l; i++)
            {
                StyleBuilder stylel = i != 3 ? style1l : style2l;
                StyleBuilder styler = i != 3 ? style1r : style2r;
                var font = i != 3 ? FNT7_2 : FNT7_2B;
                Field field = fields[i];
                var rowBuilder = tableBuilder.AddRow();
                var cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .ApplyStyle(stylel)
                    .SetFont(font);
                if (i == l - 1)
                {
                    cellBuilder.SetPadding(0, 15, 0, 0);
                }
                cellBuilder
                    .AddParagraph(field.name);
                cellBuilder = rowBuilder.AddCell();
                cellBuilder
                    .ApplyStyle(styler)
                    .SetFont(font)
                    .SetHorizontalAlignment(HorizontalAlignment.Right);
                if (i == l - 1)
                {
                    cellBuilder.SetPadding(0, 16, 0, 0);
                }
                cellBuilder
                    .AddParagraph(field.value);
            }
        }

```

###### The AddAccountInfo method

The right part of the block consists of 7 paragraphs with different styles (See Fig. 4). The method adds them using the **Helper.AddParagraph** method that takes a parameter of the **Gehtsoft.PDFFlow.Builder.TableCellBuilder** class. 

```csharp
        private void AddAccountInfo(TableCellBuilder outerCellBuilder)
        {
            AddParagraph(outerCellBuilder, "Account number:", FNT7_2, 6);
            AddParagraph(outerCellBuilder, statementInfo.CompanyName, FNT7_2B);
            AddParagraph(outerCellBuilder, "New York account terms and conditions apply", 
                FNT7, 6);
            AddParagraph(outerCellBuilder, "For Direct Deposit use", FNT7_2);
            AddParagraph(outerCellBuilder, "Routing Number (RTN): " +
                            statementInfo.DepositRTN, FNT7, 6);
            AddParagraph(outerCellBuilder, "For Wire Transfers use", FNT7_2);
            AddParagraph(outerCellBuilder, "Routing Number(RTN): " + 
                            statementInfo.WireRTN, FNT7_2);
        }

```

###### The AddOptions method

The method creates the **Options** block. See Fig. 5

![Fig. 5](../Articles%20Images/BankAccountStatementFirstOptions.png "Options")

Fig. 5

The column is a table with two columns with:
* The "*Your Business and SampleBank*" text in the left column;
* The account options in the right column.

The method:

* Creates a table with the full width of the page. The table consists of one row and 2 columns with widths 63% and 37% of the page width;
* Adds 2 cells to the row;
* Sets the top borders for the cells to display lines above them. In order to add a gap between the lines on the top, the method sets the left border for the right cell. The left border is set up with 10 pt. width and white color. The color is not visible and separates the borders between the cell.

The cells are filled using the **AddYourBusiness** and **AddOptionsToCell** methods.
```csharp
        private void AddOptions(SectionBuilder sectionBuilder, float bottomMargin)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .SetWidth(PageWidth)
                .AddColumnPercentToTable("", 63).AddColumnPercentToTable("", 37)
                .SetMarginBottom(bottomMargin);
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .ApplyStyle(StyleBuilder.New()
                    .SetBorderTop(2f, Stroke.Solid, Color.Black));
            AddYourBusiness(cellBuilder);
            cellBuilder = rowBuilder.AddCell();
            cellBuilder
                .ApplyStyle(StyleBuilder.New()
                    .SetBorderLeft(10f, Stroke.Solid, Color.White)
                    .SetBorderTop(2f, Stroke.Solid, Color.Black))
                .SetPadding(8, 0, 0, 0);
            AddOptionsToCell(cellBuilder);
        }

```

###### The AddYourBusiness method

The method fills the left part of the block shown in Fig. 5 with text. 
The method adds a paragraph with the "*Your Business and SampleBank*" text using the **Helper.AddParagraph** method. 
The method sets the next paragraph, since the paragraph has an URL added using the **ParagraphBuilder.SetUrlStyle** and **ParagraphBuilder.AddUrlToParagraph**. 

```csharp
        private void AddYourBusiness(TableCellBuilder cellBuilder)
        {
            AddParagraph(cellBuilder, "Your Business and "
                            + statementInfo.BankName, FNT13B);
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFontColor(Color.Blue)
                        .SetFontName("Helvetica")
                        .SetFontSize(7.5f)
                        .SetFontUnderline(Stroke.Solid, Color.Blue)
                        .SetBorder(0, Stroke.None, Color.White)
                    )
                .SetFont(FNT7_5)
                .AddTextToParagraph("The plans you establish today will shape your business far into the future. The heart of the planning process is your business plan. Take the time now to build a strong foundation.\nFind out more at ")
                .AddUrlToParagraph("https://" + statementInfo.BusinessPlan, 
                    statementInfo.BusinessPlan)
                .AddTextToParagraph(".");
        }

```

###### The AddOptionsToCell method

The method fills the right part of the block shown in Fig. 5 with text and checkboxes.
The method adds a paragraph with the "*Account options*" text using the **Helper.AddParagraph** method.
The method sets the next paragraph since the paragraph has an URL added using the **ParagraphBuilder.SetUrlStyle** and **ParagraphBuilder.AddUrlToParagraph**. 
The checkboxes are added using the **AddCheckBoxes** method with the **Field** array.  One **Field** is for one checkbox.

```csharp
        private void AddOptionsToCell(TableCellBuilder cellBuilder)
        {
            AddParagraph(cellBuilder, "Account options", FNT10_5B);
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFontColor(Color.Blue)
                        .SetFontName("Helvetica")
                        .SetFontSize(7f)
                        .SetFontUnderline(Stroke.Solid, Color.Blue)
                    )
                .SetFont(FNT7)
                .AddTextToParagraph(
                    "A check mark in the box indicates you have these convenient services with your account(s). Go to ")
                .AddUrlToParagraph("https://" + 
                statementInfo.AccountOptions, statementInfo.AccountOptions)
                .AddTextToParagraph(
                    " or call the number above if you have questions or if you would like to add new services");
            Field[] fields = new Field[]
            {
                new Field("Business Online Banking", "true"),
                new Field("Online Statements", "true"),
                new Field("Business Bill Pay", "true"),
                new Field("Business Spending Report", "true"),
                new Field("Overdraft Protection", "true"),
            };
            AddCheckBoxes(cellBuilder, fields);
        }

```

###### The AddCheckBoxes method

The method adds checkboxes to the table's cell.
To align the checkboxes to the right of the cell, the method adds a nested table to the cell.
The method creates columns 60% width for the label and 40% width for the checkbox in the table.
The method adds one row for each checkbox and adds the label rows and the checkbox symbols to the cells in the **foreach** cycle. 


```csharp
        private void AddCheckBoxes(TableCellBuilder outerCellBuilder, Field[] fields)
        {
            outerCellBuilder.AddTable(tableBuilder => 
            {
                tableBuilder
                    .SetBorder(Stroke.None)
                    .SetWidth(XUnit.FromPercent(100))
                    .SetMarginTop(10)
                    .AddColumnPercentToTable("", 60).AddColumnPercent("", 40);
                foreach(Field field in fields)
                {
                    var tableRowBuilder = tableBuilder.AddRow();
                    tableRowBuilder
                        .SetBorderStroke(Stroke.None);
                    var cellBuilder = tableRowBuilder.AddCell();
                    cellBuilder.SetFont(FNT7_2).AddParagraph(field.name);
                    cellBuilder = tableRowBuilder.AddCell();
                    cellBuilder.SetHorizontalAlignment(HorizontalAlignment.Right);
                    cellBuilder.SetFont(FNTZ8).AddParagraphToCell(CHECK_BOX);
                }
            });
        }

```

###### The AddAdvt method

The method creates the **Advertisement** block. See Fig. 6.

![Fig. 6](../Articles%20Images/BankAccountStatementFirstAdvt.png "Advertisement")

Fig. 6

The method adds the line separating this block from the previous block. The method adds a paragraph with the advertisement text using the **Helper.AddParagraph**.

```csharp
        private void AddAdvt(SectionBuilder sectionBuilder, float bottomMargin)
        {
            sectionBuilder
                .AddLine(PageWidth, 0.5f, Stroke.Solid).SetMarginBottom(12);
            AddParagraph(sectionBuilder, statementInfo.Advt, FNT7_5, bottomMargin);
        }

```

#### 8. The BankAccountStatementSecondPageBuilder class

The **BankAccountStatementSecondPageBuilder.Build** has responsibilities to :
* Create a page and set its parameters;

```csharp
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder.SetOrientation(Orientation).SetMargins(Margins);
```
* Create a header without the bank name;

```csharp
            var headerBuilder = sectionBuilder.AddHeaderToBothPages(80);
            AddTitle(headerBuilder.AddTable()
                , 20f, 4, AddCommonPageTitle);
```
* Add a line separating the header from the rest of the page content;
```csharp
            headerBuilder.AddLine(PageWidth, 2f, Stroke.Solid);
```
* Add the blocks "Overdraft protection", "Transaction history", "Service fee summary", "Transaction fee summary".
```csharp
            AddOverdraftProtection(sectionBuilder, 32f);
            AddTransactionHistory(sectionBuilder, 12f);
            AddServiceFeeSummary(sectionBuilder);
            AddTransactionFeeSummary(sectionBuilder);
```

###### The Title display

All *Titles* from the second page (See Fig. 7) contain:

* The *Title* text common to all pages: the account number, the start and end dates of the account statement, and the page numbers;
* The bank logo common to all pages.

![Fig. 7](../Articles%20Images/BankAccountStatementNextTitle.png "The next page title")

Fig. 7

The separate parts of the Title are created using the method of the **BankAccountStatement.AddTitle** base class. 

The **BankAccountStatementSecondPageBuilder.Build** method passes the **BankAccountStatement.AddCommonPageTitle** method as a **AddTitleAction** parameter in order to add the common *Title*.

###### The BankAccountStatement.AddCommonPageTitle method

Calls the **BankAccountStatement.AddTextTitle** method to add the common text to the *Title* with a 10 pt. top margin. 
```csharp
        protected void AddCommonPageTitle(TableCellBuilder cellBuilder, int pageCount)
        {
            AddTextTitle(cellBuilder, pageCount, 10);
        }

```

###### The AddOverdraftProtection method

The method adds the "**Overdraft Protection**" block shown in Fig. 8.

![Fig. 8](../Articles%20Images/BankAccountStatementNextOverdraftProtection.png "Overdraft Protection")

Fig. 8


The paragraph is added using the **Helper.AddParagraph** method that takes an object of the **Gehtsoft.PDFFlow.Builder.SectionBuilder** class as a parameter.

The method adds: 
* The paragraph with the "*Overdraft Protection*" text;
* The paragraph with the "*Your account is linked to...*" text.

###### The AddTransactionHistory method

The method adds the "**Transaction history**" block shown in Fig. 9.

![Fig. 9](../Articles%20Images/BankAccountStatementNextTransactionhistory.png "Transaction history")

Fig. 9

In order to add the block, the method:
* Adds the line and the "*Transaction history*" title using the **AddStatementsHeadLine** method;
* Adds the statement table using the **AddStatementsTable** method;
* Adds the "The Ending Daily Balance does ..." comment using the **AddEndingDayBalanceComment** method.

###### The AddStatementsHeadLine method

The method adds the line and the paragraph with the "*Transaction history*" title to the section.

###### The AddStatementsTable method

The method creates the table with 6 columns and: 
* Adds the headers of table's columns using the **AddStatementsHead** method;
* Creates the local variable of **total** of the **Model.Statement** class to count the totals of the transactions;
* Adds the rows with transactions using the **AddStatements** method in which the properties of the **total** variable are changed;
* Adds the row with totals of the transactions using the **AddStatementsTotal** method and passing it the **total** variable.

```csharp
        private void AddStatementsTable(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .ApplyStyle(
                    StyleBuilder.New()
                    .SetFont(FNT7_2)
                    )
                .SetMarginBottom(4)
                .AddColumnPercentToTable("", 4)
                .AddColumnPercentToTable("", 11)
                .AddColumnPercentToTable("", 48)
                .AddColumnPercentToTable("", 13)
                .AddColumnPercentToTable("", 13)
                .AddColumnPercent("", 11);
            AddStatementsHead(tableBuilder);
            Statement total = new Statement();
            AddStatements(tableBuilder, total);
            AddStatementsTotal(tableBuilder, total);
        }
```


###### The AddStatementsHead method

The method adds the *Statements* row to the table and sets it according to the design:
* sets the VerticalAlignment;
* sets the bottom border;
* adds the headers of the *Statements* columns.

```csharp
        private void AddStatementsHead(TableBuilder tableBuilder)
        {
            var tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder
                .SetFont(FNT7_2)
                .SetVerticalAlignment(VerticalAlignment.Bottom)
                .SetBorder(borderBuilder =>
                {
                    borderBuilder
                        .SetLeftBorder(0, Stroke.None, Color.None)
                        .SetTopBorder(0, Stroke.None, Color.None)
                        .SetRightBorder(0, Stroke.None, Color.None)
                        .SetBottomBorder(0.5f, Stroke.Solid, Color.Black);
                });
            tableRowBuilder.AddCell("Date").SetPadding(0, 0, 4, 4);
            tableRowBuilder.AddCell("Check\nNumber")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 4);
            tableRowBuilder.AddCell("Description").SetPadding(4, 0, 4, 4);
            tableRowBuilder.AddCell("Deposits/\nCredits")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 4);
            tableRowBuilder.AddCell("Withdrawals/\nDebits")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 4);
            tableRowBuilder.AddCell("Ending daily\nbalance")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 0, 4);
        }
```

###### The AddStatementsHead method

The method adds a row for each object of the **Model.Statement** class to the *Statements* table. The objects are in a list in the **statements** property.  The values of each **Model.Statement** object from the list are used to fill the cells of one row of the table.

The empty values of 0 and null must not be shown. To do this, the value of each property is converted depending on its type. The conversion is performed using the methods:

* The **Helper.ReplaceNullDate** for the *DateTime* type;
* The **Helper.ReplaceNullStr** for the *String* type;
* The **Helper.ReplaceZeroDouble** for the *Double* type.

When  rows are added, the properties of the **total** parameter are changed 
to sum the values of the **Deposits** and **Withdrawals** properties.

```csharp
        private void AddStatements(TableBuilder tableBuilder, Statement total)
        {
            double deposits = 0;
            double withdrawals = 0;
            foreach (Statement statement in statements)
            {
                deposits += statement.Deposits;
                withdrawals += statement.Withdrawals;
                var tableRowBuilder = tableBuilder.AddRow();
                tableRowBuilder
                    .SetFont(FNT7_2)
                    .SetVerticalAlignment(VerticalAlignment.Top)
                    .SetBorder(borderBuilder =>
                    {
                        borderBuilder
                            .SetLeftStroke(Stroke.None)
                            .SetTopStroke(Stroke.None)
                            .SetRightStroke(Stroke.None)
                            .SetBottomBorder(0.5f, Stroke.Solid, Color.Black);
                    });
                tableRowBuilder
                    .AddCell(ReplaceNullDate(statement.Date, "M/d", " "))
                    .SetPadding(0, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceNullStr(statement.Check, " "))
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(4, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceNullStr(statement.Details, " "))
                    .SetPadding(4, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceZeroDouble(statement.Deposits, " "))
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(4, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceZeroDouble(statement.Withdrawals, " "))
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(4, 0, 4, 0);
                tableRowBuilder
                    .AddCell(ReplaceZeroDouble(statement.EndingDailyBalance, " "))
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(4, 0, 0, 0);
            }
            total.Deposits = Math.Round(deposits, 2, MidpointRounding.ToEven);
            total.Withdrawals = Math.Round(withdrawals, 2, MidpointRounding.ToEven);
        }

```


###### The AddStatementsTotal method

The method adds the last two rows to the *Statements* table.
These rows have the following features:

* a Bold font;
* The first row contains only one value in the "Ending Balance" column that is filled from the **statementInfo.EndingBalance** property;
* The second row contains two values in the "Deposits/Credits" and "Withdrawals/Debits" columns. These values contain the **total** of the **Deposits** and **Withdrawals** properties. The **total** parameter is passed to the method.

```csharp
        private void AddStatementsTotal(TableBuilder tableBuilder, Statement total)
        {
            var tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder
                .SetFont(FNT7_2B)
                .SetVerticalAlignment(VerticalAlignment.Top)
                .SetBorder(borderBuilder =>
                {
                    borderBuilder
                        .SetLeftStroke(Stroke.None)
                        .SetTopStroke(Stroke.None)
                        .SetRightStroke(Stroke.None)
                        .SetBottomBorder(2f, Stroke.Solid, Color.Black);
                });
            tableRowBuilder
                .AddCell("Ending balance on " +
                            statementInfo.DateEnd.ToString("M/d", DocumentLocale))
                .SetPadding(0, 0, 4, 0)
                .SetColSpan(5);
            tableRowBuilder
                .AddCell(ReplaceZeroDouble(statementInfo.EndingBalance, " "))
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 0, 0);
            tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder
                .SetFont(FNT7_2B)
                .SetVerticalAlignment(VerticalAlignment.Top);
            tableRowBuilder.AddCell("Totals").SetPadding(0, 0, 4, 0).SetColSpan(3);
            tableRowBuilder
                .AddCell(ReplaceZeroDouble(total.Deposits, " ", "$"))
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 0);
            tableRowBuilder
                .AddCell(ReplaceZeroDouble(total.Withdrawals, " ", "$"))
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 4, 0);
            tableRowBuilder
                .AddCell(" ")
                .SetHorizontalAlignment(HorizontalAlignment.Right)
                .SetPadding(4, 0, 0, 0);
        }

```

###### The AddEndingDayBalanceComment method

The method adds the "*The Ending Daily Balance does not reflect...*" text below the table using the **Helper.AddParagraph** method that takes an object of the **Gehtsoft.PDFFlow.Builder.SectionBuilder** class as the parameter. 


###### The AddServiceFeeSummary method

The method adds the "**Service fee summary**" block shown in Fig. 10.

![Fig. 10](../Articles%20Images/BankAccountStatementNextFeesummary.png "Service fee summary")

Fig. 10

The block consists of: 
* A text paragraph with the "*Monthly service fee summary*" title;
* The fee table.

To create this block, the method uses:
* The AddServiceFeeSummaryParagrpaph method;
* The AddServiceFeeSummaryTable method.

The last row of the table is not included in the second page due to the number of items in the *Statements* list. **The library creates the additional page automatically.** It also adds the Title using the **Gehtsoft.PDFFlow.Builder.SectionBuilder.AddHeaderToBothPages** method.
Therefore, the sample program does not need to control the creation of additional pages, which saves you a lot of time not having to provide the logic for page control.

###### The AddServiceFeeSummaryParagrpaph method

The method adds the following elements to the page: 
* A paragraph with the "*Monthly service fee summary*" title;
* A paragraph with text.

The method sets the paragraph since an URL is added to it.

```csharp
        private void AddServiceFeeSummaryParagrpaph(SectionBuilder sectionBuilder)
        {
            AddParagraph(sectionBuilder, "Monthly service fee summary", FNT7_9B, 4.8f);
            var paragraphBuilder = sectionBuilder.AddParagraph(); 
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFont(FNT7_2URL)
                        .SetBorder(0, Stroke.None, Color.White)
                    )
                .SetFont(FNT7_2)
                .AddTextToParagraph(
                    "For a complete list of fees and detailed account information, please see the " + 
                    statementInfo.BankName + 
                    " Fee and Information Schedule and Account Agreement applicable to your account or talk to a banker. Go to "
                    )
                .AddUrlToParagraph("https://" + 
                    statementInfo.Online + 
                    "/feefaq", statementInfo.Online + "/feefaq")
                .AddTextToParagraph(
                    " to find answers to common questions about the monthly service fee on your account.");
        }

```

###### The AddServiceFeeSummaryTable method

The method creates the table with four columns. The last column is for the checkbox in the "Average ledger balance " row. 
The rows are added to the table using the **AddTableRows** method, to which the rows are passed as an array of objects of the **RowData** inner class. The rows are created using the **GetServiceFeeSummaryRowData** method. The row numbers with bottom border is the last parameter passed to the **AddTableRows** method. These are #0 and #2 rows. 


###### The internal RowData class

An object of this class contains:
* The **data** array of the strings;
* The **font** array of fonts with one font for each string.

Also, the class defines:
* The **virtual** **GetLength** method used to define a number of the rows;
* The **virtual** **AddTextToParagraph** method used to add the rows to the table;
* The **virtual** **BottomBorderWidth** method using to define the bottom border width of the row;
* The **virtual** **BottomBorderStroke**  method to define the bottom border style of the row.


###### The internal BottomBorderRowData class

This class extends the **RowData class** for the rows with the bottom borders.

This class overrides the methods:
* The **AddTextToParagraph**;
* The **BottomBorderWidth**;
* The **BottomBorderStroke**.

###### The internal ComplexRowData class

This class extends the **RowData class** for the rows with different fonts such as:
* "Have any **ONE** of the following account requirements";
* CHECK_BOX + "Online only statements ($5.00 discount)".

The class overrides the methods:
* The **GetLength**;
* The **AddTextToParagraph**.


###### The GetServiceFeeSummaryRowData method

The method prepares the data to fill the rows of *ServiceFeeSummary* table.
The method creates objects of the **RowData** array with the values for the *ServiceFeeSummary* table using the **statementInfo** property.

```csharp
        private RowData[] GetServiceFeeSummaryRowData()
        {
            return new RowData[]
                {
                    new RowData(
                        "Fee period " + 
                        statementInfo.DateBegin.ToString("MM/dd/yyyy", DocumentLocale) + 
                        " - " + 
                        statementInfo.DateEnd.ToString("MM/dd/yyyy", DocumentLocale),
                        "Standard monthly service fee " + 
                        ReplaceZeroDouble(statementInfo.StandartServiceFee, "$0.00", "$"),
                        "You paid " + 
                        ReplaceZeroDouble(statementInfo.ServiceFee, "$0.00", "$")
                    ),
                    new RowData(
                        new FontBuilder[] {
                            FNT7_2B,
                            FNT7_2
                        },
                        "How to reduce the monthly service fee by " + 
                        ReplaceZeroDouble(statementInfo.ServiceDiscount, "$0.00", "$"),
                        "Minimum required",
                        "This fee period"
                    ),
                    new ComplexRowData(
                        new RowData (
                                new FontBuilder[] {
                                    FNT7_2,
                                    FNT7_2B,
                                    FNT7_2
                                },
                                "Have any ",
                                "ONE",
                                " of the following account requirements\n" + 
                                "Average ledger balance"
                        ),
                        new FontBuilder[] {
                            FNT7_2,
                            FNT7_2,
                            FNTZ8
                        },
                        ReplaceZeroDouble(statementInfo.MinimumRequired, "$0.00", "$"),
                        ReplaceZeroDouble(Math.Round(statementInfo.AverageBalance, 0, 
                        MidpointRounding.ToEven), "$0.00", "$"),
                        CHECK_BOX
                    ),
                    new RowData(
                        new FontBuilder[] {
                            FNT7_2B
                        },
                        "Monthly service fee discount(s) (applied when box is checked)"
                    ),
                    new ComplexRowData(
                        new RowData (
                                new FontBuilder[] {
                                    FNTZ8,
                                    FNT7_2
                                },
                                CHECK_BOX,
                                " Online only statements (" +
                                ReplaceZeroDouble(statementInfo.ServiceDiscount, "$0.00", "$") + 
                                "  discount)"
                        ),
                        ReplaceZeroDouble(statementInfo.MinimumRequired, "$0.00", "$"),
                        ReplaceZeroDouble(Math.Round(statementInfo.AverageBalance, 0, 
                            MidpointRounding.ToEven), "$0.00", "$")
                    )
                };
        }

```

###### The AddTableRows method

The method adds rows to the *ServiceFeeSummary* table using the passed array of objects of the **RowData** class. 

```csharp
        private void AddTableRows(TableBuilder tableBuilder, RowData[] rows, 
            int columnCount, params int[] underlines)
        {
            for(int i = 0, l = rows.Length; i < l; i++) 
            {
                RowData row = rows[i];
                var tableRowBuilder = tableBuilder.AddRow();
                tableRowBuilder
                    .SetBorder(borderBuilder =>
                    {
                        borderBuilder
                            .SetLeftBorder(0, Stroke.None, Color.None)
                            .SetTopBorder(0, Stroke.None, Color.None)
                            .SetRightBorder(0, Stroke.None, Color.None);
                        if (underlines.Contains(i))
                        {
                            borderBuilder
                                .SetBottomBorder(0.5f, Stroke.Solid, Color.Black);
                        } else
                        {
                            borderBuilder
                                .SetBottomBorder(row.BottomBorderWidth(), 
                                    row.BottomBorderStroke(), Color.Black);
                        }
                    });
                for(int j = 0; j < columnCount; j++)
                {
                    var cellBuilder = tableRowBuilder.AddCell();
                    cellBuilder
                        .SetHorizontalAlignment(j > 0 ? 
                            HorizontalAlignment.Right : 
                            HorizontalAlignment.Left)
                        .SetVerticalAlignment(VerticalAlignment.Bottom);
                    cellBuilder
                        .SetPadding(0, 0, 0, 0);
                    if (j < row.GetLength())
                    {
                        row.AddTextToParagraph(cellBuilder.AddParagraph(), j);
                    } else
                    {
                        cellBuilder
                            .SetFont(row.GetLastFont())
                            .AddParagraphToCell(" ");
                    }
                }
            }
        }

```


###### The AddTransactionFeeSummary method

The method adds the "**Transaction fee summary**" block shown in Fig. 11.

![Fig. 11](../Articles%20Images/BankAccountStatementNextServicefeesummary1.png "Transaction fee summary") 

Fig. 11

The block consists of:
* a line;
* a paragraph with the "*Account transaction fees summary*" title;
* a fee table;
* a line;
* a paragraph with the "*Your feedback matters...*" checkbox.

To create the block, the method uses:
* The **AddTransactionFeeSummaryParagrpaph** method;
* The **AddTransactionFeeSummaryTable** method;
* The **Gehtsoft.PDFFlow.Builder.SectionBuilder.AddLine** method;
* The **AddYourFeedBack** method.

###### The AddTransactionFeeSummaryParagraph method

The method:
* Adds a line using the **Gehtsoft.PDFFlow.Builder.SectionBuilder.AddLine** method;
* Adds a paragraph with a title using the **Helper.AddParagraph** method that takes an object of the **Gehtsoft.PDFFlow.Builder.SectionBuilder** class as the parameter.


###### The AddTransactionFeeSummaryTable method

The method creates the table with 6 columns.
The rows are added to the table using the **AddTableRows** method that is passed the rows as an array of objects of the **RowData** inner class. The rows are created using the **GetTransactionFeeSummaryRowData** method.

The row numbers with bottom border is the last parameter passed to the **AddTableRows** method. This is a #0 row. 

###### The GetTransactionFeeSummaryRowData method

The method creates the data to fill the rows of the *TransactionFeeSummary* table.
The method creates objects of the **RowData** array with the values for the *TransactionFeeSummary* table using the **statementInfo** property.

```csharp
        private RowData[] GetTransactionFeeSummaryRowData()
        {
            return new RowData[]
                {
                    new RowData(
                        "Service charge description",
                        "Units used",
                        "Units included",
                        "Excess units",
                        "Service charge per\nexcess units ($)",
                        "Total service\ncharge ($)"
                    ),
                    new BottomBorderRowData(
                        2f,
                        Stroke.Solid,
                        "Transactions",
                        ReplaceZeroInt(statementInfo.TransactionUnits, "0"),
                        ReplaceZeroInt(statementInfo.TransactionUnitsIncluded, "0"),
                        ReplaceZeroInt(statementInfo.TransactionExcessUnits, "0"),
                        ReplaceZeroDouble(statementInfo.ServiceCharge, "0.00"),
                        ReplaceZeroDouble(statementInfo.TotalServiceCharge, "0.00")
                    ),
                    new RowData(
                        new FontBuilder[] {
                            FNT7_2B
                        },
                        "Total service charges",
                        " ",
                        " ",
                        " ",
                        " ",
                        ReplaceZeroDouble(statementInfo.TotalServiceCharge, "$0.00", "$")
                    )
                };
        }
```


###### The AddYourFeedBack method

The method adds a table with two rows to the page.
A checkbox is added to the left column.
The text beginning with "*Your feedback matters*" is added to the right column. 

```csharp
        private void AddYourFeedBack(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetBorder(Stroke.None)
                .SetContentRowBorderWidth(0, 0, 0, 0)
                .ApplyStyle(
                    StyleBuilder.New()
                    .SetHorizontalAlignment(HorizontalAlignment.Left)
                    )
                .SetMarginTop(10)
                .SetWidth(XUnit.FromPercent(54))
                .AddColumnPercentToTable("", 7)
                .AddColumnPercent("", 93);
            var tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder.AddCell(CHECK_BOX).SetFont(FNTZ8);
            var cellBuilder = tableRowBuilder.AddCell();
            var paragraphBuilder = cellBuilder.AddParagraph();
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New().SetFont(FNT7_5URL)
                 )
                .AddTextToParagraph(
                    "Your feedback matters\n",FNT7_9B
                )
                .AddTextToParagraph(
                    "Share your compliments and complaints so we can better serve you.\n" + 
                    "Call us at " + 
                    statementInfo.FeedBackPhone + 
                    "(" + statementInfo.Phone + 
                    ") or visit ",
                    FNT7_5
                )
                .AddUrlToParagraph(
                    "https://" + 
                    statementInfo.Online + 
                    "/feedback", 
                    statementInfo.Online + "/feedback"
                    )
                .AddTextToParagraph(
                    ".",
                    FNT7_5
                );
        }

```

#### 9. The BankAccountStatementLastPageBuilder class

The **BankAccountStatementLastPageBuilder.Build method** has the responsibility to:
* Create the page and set its parameters;

```csharp
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
```
* Create the header without the bank name;
```csharp
            var headerBuilder = sectionBuilder.AddHeaderToBothPages(80);
            AddTitle(headerBuilder.AddTable()
                , 20f, 4, AddCommonPageTitle);
```
* Add a line separating the header from the rest of the page content;
```csharp
            headerBuilder.AddLine(PageWidth, 2f, Stroke.Solid);
```
* Create the footer "В©2020 SampleBank , N.A. All rights reserved. Member FDIC.";
```csharp
            var footerBuilder = sectionBuilder.AddFooterToBothPages(8);
            var paragraphBuilder = footerBuilder.AddParagraph();
            paragraphBuilder
                .SetAlignment(HorizontalAlignment.Left)
                .SetFont(FNT5_6)
                .AddTextToParagraph("В©2020 " + statementInfo.BankName + 
                    " , " + statementInfo.BankNameState + 
                    " All rights reserved. Member FDIC.");
```
* Add the "Polices" block as the second header;
```csharp
            headerBuilder = sectionBuilder.AddHeaderToBothPages(90);
            AddGeneralStatementPolicies(headerBuilder);
```
* Add a line separating the "Polices" from the rest of the page content;
```csharp
            headerBuilder.AddLine(PageWidth, 0.5f, Stroke.Solid);
```
* Add the "Instructions" and "Balance calculation" blocks.
```csharp
            AddAccountBalanceCalculationWorksheet(sectionBuilder);
```

###### The AddGeneralStatementPolicies methods

The method adds the "Polices" block shown in Fig. 12.

![Fig. 12](../Articles%20Images/BankAccountStatementLastPolices.png "Polices") 

Fig. 12

The method adds a paragraph with the "*General statement policies for SampleBank*" title using the **Helper.AddParagraph** method that takes a parameter of the **Gehtsoft.PDFFlow.Builder.RepeatingAreaBuilder** class.

Then the method adds the table filled by the **FillGeneralStatementPoliciesTable** method.

```csharp
        private void AddGeneralStatementPolicies(RepeatingAreaBuilder builder)
        {
            AddParagraph(
                builder,
                "General statement policies for " + statementInfo.BankName,
                FNT11_B
            );
            builder
                .AddTableToRepeatingArea(FillGeneralStatementPoliciesTable);
        }

```
###### The FillGeneralStatementPoliciesTable method

The method adds two columns and one row to the table and adds the **Polices** text to the two cells of the row.

```csharp
        private void FillGeneralStatementPoliciesTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetBorder(Stroke.None)
                .SetMarginTop(10)
                .SetMarginBottom(6)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 50)
                .AddColumnPercent("", 50);
            var tableRowBuilder = tableBuilder.AddRow();
            var paragraphBuilder = tableRowBuilder.AddCell().AddParagraph();
            paragraphBuilder
                .AddTextToParagraph("Notice", FNT7_2B)
                .AddTextToParagraph(": " +
                statementInfo.BankName + ", " +
                statementInfo.BankNameState +
                " may furnish information about accounts belonging to individuals, including sole proprietorships, to consumer reporting agencies.  If this applies to you, you have the right to dispute the accuracy of information that we have reported by writing to us at: "
                + statementInfo.ReportAddress + ".",
                FNT7_2);
            tableRowBuilder
                .AddCell("You must describe the specific information that is inaccurate or in dispute and the basis for any dispute with supporting documentation.  In the case of information that relates to an identity theft, you will need to provide us with an identity theft report.")
                .SetFont(FNT7_2)
                .SetPadding(8, 0, 0, 0);
        }

```

###### The AddAccountBalanceCalculationWorksheet method

The method adds the "Instructions" and the "Balance calculation" blocks to the page. See Fig. 13.

![Fig. 13](../Articles%20Images/Instruction-calculation.png "Polices") 

Fig. 13

The block is the part of the page with two columns. Therefore, the method uses:  

* The **Gehtsoft.PDFFlow.Builder.SectionBuilder.AddRptAreaLeftToBothPages** method to add the left column with the "Instructions" block;
* The **Gehtsoft.PDFFlow.Builder.SectionBuilder.AddRptAreaRightToBothPages** to add the right column with the "Balance calculation" block. 

The columns are filled using the **AddInstructions** and **AddBalanceCalc** methods respectively.

```csharp
        private void AddAccountBalanceCalculationWorksheet(SectionBuilder sectionBuilder)
        {
            float repAreaWidth = PageWidth * 0.5f - 1;
            sectionBuilder.AddRptAreaLeftToBothPages(repAreaWidth, AddInstructions);
            sectionBuilder.AddRptAreaRightToBothPages(repAreaWidth, AddBalanceCalc);
        }

```


###### The AddInstructions method

The method adds:
* The common text with the "*Account Balance Calculation Worksheet*" title using the **AddAboutAccountBalanceCalculationWorksheet** method;
* The instruction with the "*ENTER*" title using the **AddInstuctionAboutENTER** method;
* The instruction with the "*ADD*" title using the **AddInstuctionAboutADD** method;
* The instruction with the "*SUBTRACT*" title using the **AddInstuctionAboutSUBTRACT** method.

###### The AddAboutAccountBalanceCalculationWorksheet method

The method adds a paragraph with the "*Account Balance Calculation Worksheet*" title using the **Helper.AddParagraph** method that takes the parameter of the **Gehtsoft.PDFFlow.Builder.RepeatingAreaBuilder** class.

The method adds the numbered list using the **Helper.AddNumberedListToParagraph** method. It passes an  array of items using the **GetAboutAccountBalanceCalculationItems** method.
```csharp
        private void AddAboutAccountBalanceCalculationWorksheet(
            RepeatingAreaBuilder builder)
        {
            AddParagraph(
                builder,
                "Account Balance Calculation Worksheet",
                FNT11_B
            );
            string[] items = GetAboutAccountBalanceCalculationItems();
            AddNumberedListToParagraph(
                builder,
                items,
                FNT7_2,
                14
            );
        }

```

###### The GetAboutAccountBalanceCalculationItems method 

The method creates array of rows.

```csharp
        private string[] GetAboutAccountBalanceCalculationItems()
        {
            return new string[]
            {
                "Use the following worksheet to calculate your overall account balance.",
                "Go through your register and mark each check, withdrawal, ATM transaction, payment, deposit or other credit listed on your statement. Be sure that your register shows any interest paid into your account and any service charges, automatic payments or ATM transactions withdrawn from your account during this statement period.",
                "Use the chart to the right to list any deposits, transfers to your account, outstanding checks, ATM withdrawals, ATM payments or any other withdrawals (including any from previous months) which are listed in your register but not shown on your statement."
            };
        }

```

###### The methods: AddInstuctionAboutENTER, AddInstuctionAboutADD, AddInstuctionAboutSUBTRACT
The methods add the "Instructions" block. See Fig. 14.

![Fig. 14](../Articles%20Images/Instruction2.png "Instructions") 

Fig. 14

The figure is divided in three parts that are added by each method.

These parts are of the same type, but with a different and complex content. The fact that the parts are the same type is reflected in the code methods that are almost the same. Each method:

* Adds the title using the **AddInstructionTitle** method and passing it different margins above the title and different title texts;
* Adds and sets the table with two columns, one column for the instructions and one column for the form with the input fields ($ ) using the **ConfigureDescriptionFormTable** method; 
* Fills the left column with the instructions using one of the following methods: the **FillEndingBalanceENTER**, the **FillInstructionAnyDeposits**, and the **FillInstructionAnyWithdrawals**;
* Adds the form with the input fields using the following methods: the **AddEndingBalanceForm**, the **AddAnyDepositsForm**, and the **AddAnyWithdrawalsForm**.


###### The AddInstructionTitle method

The method adds the paragraph with title using the **Helper.AddParagraph** method that takes the parameter of the **Gehtsoft.PDFFlow.Builder.RepeatingAreaBuilder** class.

###### The ConfigureDescriptionFormTable method

The method sets the table. The method adds two columns of a specified width and sets a margin of the table.

```csharp
        private void ConfigureDescriptionFormTable(TableBuilder tableBuilder, 
            float descriptionColumnPercent = 64, float formColumnPercent = 36)
        {
            tableBuilder
                .SetMarginTop(1f)
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetAltRowStyleBackColor(Color.White)
                .AddColumnPercentToTable("", descriptionColumnPercent)
                .AddColumnPercentToTable("", formColumnPercent)
                .SetAlignment(HorizontalAlignment.Left);
        }

```

###### The methods: **FillEndingBalanceENTER**, **FillInstructionAnyDeposits**, **FillInstructionAnyWithdrawals**

The methods add the columns with instructions below the title of the instructions (the parts in Fig. 14 highlighted with blue rectangles). Each method:

* Creates the table for the item name and the instructions;
* Sets the table equally using the **ConfigureDescription** method described above. The table consists of two columns. The left narrow column is for the item names: A, B, C, respectively. The right wide column is for the instructions. This is done so that the text of the instructions is indented from the left margin by the width of the item name;
* Adds items of the instructions using the **AddDescriptionRow** method for each item. The item name, or space in case on no item name, and the action to add the item of the text instructions are passed to the method.  This is done using:
The **AddDescriptionToCell** method;
The **AddCalculateDescriptionToCell** method; 
The **AddWithdrawalsDescriptionToCell** method;
The **AddCalculateEndingBalanceDescriptionToCell** method.

###### The AddDescriptionRow method

The method:
* Adds a row to the table;
* Sets the minimum height of the row using a value passed in the parameter;
* Fills the left cell of the row using the item name passed in a parameter;
* Calls the action passed in a parameter for the right cell.

```csharp
        private void AddDescriptionRow(TableBuilder tableBuilder, float minHeight, 
            string listIndex, string[] text, 
            Action<TableCellBuilder, string[]> addTextAction)
        {
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .ApplyStyle(
                    StyleBuilder.New()
                        .SetTableCellMinHeight(minHeight)
                );
            rowBuilder.AddCell(listIndex).SetFont(FNT7_2B);
            addTextAction(rowBuilder.AddCell(), text);
        }

```

###### The AddDescriptionToCell method
The method sets the cell specifying its font and dotted border. The method adds the paragraphs for each string from the array passed in the parameter in the **foreach** cycle. It is used by the methods: 

* The **FillEndingBalanceENTER** to add the "*A*" item;
* The **FillInstructionAnyDeposits**  to add the "*B*" item.

```csharp 
        private void AddDescriptionToCell(TableCellBuilder cellBuilder, string[] text)
        {
            cellBuilder
                .SetFont(FNT7_2)
                .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.Dotted)
                .SetBorderColor(Color.Black);
            foreach (string item in text)
            {
                cellBuilder.AddParagraphToCell(item);
            }
        }

```

###### The AddCalculateDescriptionToCell method
The method sets the cell specifying its dotted border.
The method adds the paragraphs for each string from the array passed in the parameter in the **for** cycle. The method sets Bold font for the first paragraph. It is used by the method:

* The **FillInstructionAnyDeposits** to add the "*CALCULATE THE SUBTOTAL*" item.

```csharp 
        private void AddCalculateDescriptionToCell(TableCellBuilder cellBuilder, 
            string[] text)
        {
            cellBuilder
                .SetPadding(0, 10, 0, 0)
                .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.Dotted)
                .SetBorderColor(Color.Black);
            for (int i = 0, l = text.Length; i < l; i++)
            {
                string item = text[i];
                var font = i == 0 ? FNT7_2B : FNT7_2;
                ParagraphBuilder paragraphBuilder = cellBuilder.AddParagraph();
                paragraphBuilder
                    .SetFont(font)
                    .AddText(item);
            }
        }

```

###### The AddWithdrawalsDescriptionToCell method
The method configures a cell to have no borders. The method adds a paragraphs for each string from an array passed in the parameter in the **foreach** cycle.
It is used by the method:

* The **FillInstructionAnyDeposits** to add the "*C*" item.

```csharp 
        private void AddWithdrawalsDescriptionToCell(TableCellBuilder cellBuilder, 
            string[] text)
        {
            cellBuilder
                .SetFont(FNT7_2)
                .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.None);
            foreach (string item in text)
            {
                ParagraphBuilder paragraphBuilder = cellBuilder.AddParagraph();
                paragraphBuilder.AddText(item);
            }
        }

```

###### The AddCalculateEndingBalanceDescriptionToCell method
The method sets a cell to have no borders. The method adds a paragraphs for each string from the array passed in the parameter in the **for** cycle. The method sets  Bold font for the first paragraph. It is used by the method:

* The **FillInstructionAnyWithdrawals** to add the "*CALCULATE THE ENDING BALANCE*" item.

```csharp 
        private void AddCalculateEndingBalanceDescriptionToCell(
            TableCellBuilder cellBuilder, string[] text)
        {
            cellBuilder
                .SetPadding(0, 10, 0, 0)
                .SetBorderStroke(Stroke.None, Stroke.None, Stroke.None, Stroke.None);
            for (int i = 0, l = text.Length; i < l; i++)
            {
                string item = text[i];
                var font = i == 0 ? FNT7_2B : FNT7_2;
                ParagraphBuilder paragraphBuilder = cellBuilder.AddParagraph();
                paragraphBuilder
                    .SetFont(font)
                    .AddText(item);
            }
        }
```

###### The methods: **AddEndingBalanceForm**, **AddAnyDepositsForm**, **AddAnyWithdrawalsForm**
The methods add the input forms in the "**Instructions**" block highlighted in Fig 18 with blue rectangles. 

![Fig. 15](../Articles%20Images/Instruction3.png "Forms") 

Fig. 15

The methods add different number of the form rows using the **Helper.AddFormRow**:
* **AddEndingBalanceForm** - 1
* **AddAnyDepositsForm** - 6
* **AddAnyWithdrawalsForm** - 1

The **AddAnyWithdrawalsForm** method adds the field in the rectangle to the second row form using the **Helper.AddFormRowBox** method.



###### The AddBalanceCalc method

The method adds the "**Balance calculation**" to the page. See Fig. 16.

![Fig. 16](../Articles%20Images/BankAccountStatementLastCalcTable.png "Calculation") 

Fig. 16

This is a table with 3 columns. The method:
* Sets the borders for all rows;
* Adds and sets the first row to make it without borders;
* Fills the first row with the header names; 
* Adds 26 empty row in the **for** cycle;
* Adds and sets the last row to not have borders;
* Adds and sets the last row to make the cells of the row, except the last one, without borders;
* Adds the "*Total amount $*" text to the summary cell of the last row.

```csharp
        private void AddBalanceCalc(RepeatingAreaBuilder builder)
        {
            var tableBuilder = builder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(90))
                .SetBorder(Stroke.Solid, Color.Black, 0.5f)
                .SetAltRowStyleBackColor(Color.White)
                .AddColumnPercentToTable("", 20)
                .AddColumnPercentToTable("", 60)
                .AddColumnPercentToTable("", 20)
                .SetAlignment(HorizontalAlignment.Right);
            var rowBuilder = tableBuilder.AddRow();
            foreach (string head in new string[] {
                                                    "Number",
                                                    "Items Outstanding",
                                                    "Amount"
                                                    })
            {
                rowBuilder
                    .AddCell(head).ApplyStyle(
                                StyleBuilder.New()
                                    .SetBorderStroke(Stroke.None)
                                    .SetFont(FNT7B)
                                    .SetHorizontalAlignment(
                                        HorizontalAlignment.Center)
                            );
            }
            for(int i = 0; i < 26; i++)
            {
                rowBuilder = tableBuilder.AddRow();
                rowBuilder.AddCellToRow(" ").AddCellToRow(" ").AddCellToRow(" ");
            }
            rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder
                .AddCell("Total amount  $");
            cellBuilder
                .ApplyStyle(
                    StyleBuilder.New()
                        .SetBorderStroke(Stroke.None)
                        .SetFont(FNT7B)
                        .SetHorizontalAlignment(
                            HorizontalAlignment.Right)
                        .SetVerticalAlignment(
                            VerticalAlignment.Bottom)
                )
                .SetColSpan(2)
                .SetPadding(0, 0, 4, 0);
            rowBuilder
                .AddCellToRow(" ");
        }

```



#### 10. The Helper class
The static class was created in order to avoid code duplication.

###### The AddParagraph methods

All methods add  text paragraphs to the containers passed to them in the same way.

The methods differ in the type of the first parameter.

The parameter can be:
* **cellBuilder** - an object of the**Gehtsoft.PDFFlow.Builder.TableCellBuilder** class.
*  **sectionBuilder** - an object of the **Gehtsoft.PDFFlow.Builder.SectionBuilder** class.
* **builder** - an object of the **Gehtsoft.PDFFlow.Builder.RepeatingAreaBuilder** class.

In addition to the described above, the methods can have the following parameters: 
* **topMagin** - the optional parameter, float.  The margin value above the paragraph.
* **text** - the text of the paragraph.
* **paragrpaphFont** - the font of the text.
* **bottomMagin** - the optional parameter, float.  The margin value below the paragraph.

The full list of the methods:

```csharp
        public static void AddParagraph(SectionBuilder sectionBuilder, string text, 
                    FontBuilder paragrpaphFont, float bottomMagin = 0.0f)

        public static void AddParagraph(SectionBuilder sectionBuilder, float topMagin, 
            string text, FontBuilder paragrpaphFont, float bottomMagin = 0.0f)

        public static void AddParagraph(TableCellBuilder cellBuilder, string text,
            FontBuilder paragrpaphFont, float bottomMagin = 0.0f)

        public static void AddParagraph(TableCellBuilder cellBuilder, float topMagin, 
            string text,
            FontBuilder paragrpaphFont, float bottomMagin = 0.0f)

        public static void AddParagraph(RepeatingAreaBuilder builder, string text,
            FontBuilder paragrpaphFont, float bottomMagin = 0.0f)

        public static void AddParagraph(RepeatingAreaBuilder builder, float topMagin, 
            string text,
            FontBuilder paragrpaphFont, float bottomMagin = 0.0f)
```

###### The ReplaceNullDate method

The method returns a date in text format in accordance with the passed format or a passed default value when a null is passed instead of a date. 

The parameters:
*  **date** - an object of the DateTime class or null;
* **format** - the date display format, string;
* **forNull** - the returned value if null is passed in the **date** parameter.

###### The ReplaceNullStr method

The method returns the passed string or a passed default value when null is passed instead of a date. The parameters:

* **value** - the value that must be returned if it is not null, string;
* **forNull** - the returned value if null is passed in the **value** parameter.

###### The ReplaceZeroDouble method

The method returns the text of the passed double value with the prefix or a passed default value without prefix if NaN or 0 is passed. 

The parameters:
* **value** - the value, double, to be converted to a string;
* **forZero** - the returned value if NaN or 0 Рїis passed in the **value** parameter;
* **currencyPrefix** - optional prefix, default is an empty string.

###### The ReplaceZeroInt method

The method returns the text of the passed int value or a passed default value if 0 is passed.

The parameters:
* **value** - the value, int, to be converted to string;
* **forZero** - the returned value if 0 is passed in the **value** parameter.

###### The AddFormRow method

The method adds a row in the text__________  form to a table's cell.

The parameters:
* **cellBuilder** - the object of the**Gehtsoft.PDFFlow.Builder.TableCellBuilder** class;
* **text** - the optional prefix, string, default is "$".

The method uses *Tabulation* to form the underline.

```csharp
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

```


###### The AddFormRowBox method

The method adds a row in the text__________ form to a table's cell placing the cell to the rectangle. 

The parameters:
* **cellBuilder** - An object of the**Gehtsoft.PDFFlow.Builder.TableCellBuilder** class;
* **text** - the optional prefix, string, default is "$".

The method uses *Tabulation* to form the underline and the table form the rectangle.

```csharp
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
```

###### The AddNumberedListToParagraph method
The method adds a numbered list using an array of strings passed to it in the **items** parameter. The method adds a paragraph for each item of the array and sets the **numbered** type of the list item using the **Gehtsoft.PDFFlow.Builder.ParagraphBuilder.SetListNumbered** method. 

```csharp
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

```
# Summary
The above example showed how to create a complex document that includes tables, nested tables, and lists, as well as how to create forms using tabulation. It demonstrated how to create a two-column page using "Left" and "Right" repeating areas.

The resulting BankAccountStatement.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/BankAccountStatement.pdf).

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/BankAccountStatement).

