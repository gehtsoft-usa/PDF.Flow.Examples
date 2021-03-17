##### Example: Rental Agreement

**Rental Agreement** project is an example of creating a **“Rental Agreement”** document. The example shows how to create a complex document that includes tables, nested tables, and lists. Also it shows one of the ways to display forms for manual filling of a printed document.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/RentalAgreement).

**Table of Content**  

- [Prerequisites](#prerequisites)
- [Purpose](#purpose)
- [Description](#description)
- [Output file](#output-file)
- [Open the project in Visual Studio](#1-open-the-project-in-visual-studio)
- [Run the sample application](#2-run-the-sample-application)
- [Source code structure](#3-source-code-structure)
- [Adding the data](#4-adding-the-data)
- [The Program class](#5-the-program-class)
- [The RentalAgreementRunner class](#6-the-rentalagreementrunner-class)
- [The RentalAgreementBuilder class](#7-the-rentalagreementbuilder-class)
- [The RentalAgreementTextBuilder class](#8-the-rentalagreementtextbuilder-class)
- [The RentalAgreementHelper class](#9-the-rentalagreementhelper-class)
- [The RentalAgreementDepositBuilder class](#10-the-rentalagreementddepositbuilder-class)
- [The RentalAgreementAmountBuilder class](#11-the-rentalagreementdamountbuilder-class)
- [The RentalAgreementCheckListBuilder class](#12-the-rentalagreementchecklistbuilder-class)
- [Summary](#summary)

# Prerequisites

1. **Visual Studio 2017** or later is installed.
   To install a community version of Visual Studio, use the following link: https://visualstudio.microsoft.com/vs/community/.
   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2. **.NET Core Framework SDK 2.1** or later is installed.
   To install the framework, use the following link: https://dotnet.microsoft.com/download.

# Purpose

The example shows how to create a “Rental Agreement” that is a complex fifteen-page document.

The document consists of:

* Nine pages with the main text of the agreement and a signature page
* Security Deposit Receipt page
* AMOUNT ($) DUE AT SIGNING page
* Three Move-in Checklist pages

# Description

#### Output file

The example creates the **RentalAgreement.pdf** file in the output **bin/(debug|release)/netcoreapp2.1** folder, unless specified otherwise in the command line.

#### 1. Open the project in Visual Studio

There are two ways to open the project:

* After installing Visual Studio, double-click the project file **RentalAgreement.csproj**.

* In Visual Studio, select **File** > **Open** > **Project/Solution** > **RentalAgreement.csproj**. 

#### 2. Run the sample application

There are two ways to run the sample application:

* From Visual Studio by clicking **F5**.

* From a command line: in the directory with **RentalAgreement.csproj**, run the command: 

```
dotnet run
```

You can get optional parameters of the command line using the command:

```
dotnet run -help
```

that shows the specification of the command line options:

```
Usage: dotnet run [fullPathToOutFile] [appToView]
Where: fullPathToOutFile - a path to the result file, 'RentalAgreement.pdf' by default
appToView - the name of an application to view the file immediately after preparing, by default none app starts
```

## 3. Source code structure

The source code consists of several files (classes).

The reasons to write several classes are the following (in descending order of priority):

* Separation of responsibility. The class `Program` processes the command line options, any possible errors during the document creation, and displays the error report. The other classes do not process the command line options and errors. They are used to build the document itself.
* Reduction of responsibility of each class. This helps to limit the size of code for each class, which improves understanding of the code example.
* Independent blocks of content. The document consists of several content blocks which are independent, so for each class you can select its own block.

## 4. Adding the data

The data used in the document are stored in JSON files and processed in the corresponding model files. Create the following files:

* **Content/ra-agreement.json** file:

  ```json
  {
    "Date": "2020-12-09T00:00:00",
    "AptAddress": "1 Main Street, Apt 4, Small Town, Alabama, 20992",
    "AptFeaturesBath": "2.5 bathroom(s)",
    
      ...
      
    "PetsAllowed": "Two (2) pets",
    "PetsFee": 300,
    "ArmedForcesNotice": "thirty (30) days",
    "LawsState": "North Carolina"
  }
  ```

* **Model/AgreementData.cs** file:

  ```c#
  namespace RentalAgreement.Model
  {
      public class AgreementData
      {
  
          public DateTime Date { get; set; }
          public string AptAddress { get; set; }
          public string AptFeaturesBath { get; set; }
          public string AptFeatures { get; set; }
          public string AptFurnishing { get; set; }
          public string Appliances { get; set; }
        public DateTime DateBegin { get; set; }
          public DateTime DateEnd { get; set; }
        public string CancelNoticePeriodDays { get; set; }
          public string ContinueNoticePeriondDays { get; set; }
          public double MonthPayment { get; set; }
          public double NonsufficientFundsFee { get; set; }
          public double LateFee { get; set; }
          public double SecurityDeposit { get; set; }
          public double RightToBuyPrice { get; set; }
          public double RightToBuyDeposit { get; set; }
          public string AbandonmentPeriod { get; set; }
          public string ParkingSpace { get; set; }
          public string ParkingSpaceDescription { get; set; }
          public int TerminationPeriod { get; set; }
          public double TerminationFee { get; set; }
          public string PetsAllowed { get; set; }
          public double PetsFee { get; set; }
          public string ArmedForcesNotice { get; set; }
          public string LawsState { get; set; }
  
  
          public override string ToString() 
          {
              return  "AgreementData{" +  
                      "Date=" + Date +
                      ", AptAddress=" + AptAddress +
                      ", AptFeaturesBath=" + AptFeaturesBath +
                      ", AptFeatures=" + AptFeatures +
                      ", AptFurnishing=" + AptFurnishing +
                      ", Appliances=" + Appliances +
                      ", DateBegin=" + DateBegin +
                      ", DateEnd=" + DateEnd +
                      ", CancelNoticePeriodDays=" + CancelNoticePeriodDays +
                      ", ContinueNoticePeriondDays=" + ContinueNoticePeriondDays +
                      ", MonthPayment=" + MonthPayment +
                      ", NonsufficientFundsFee=" + NonsufficientFundsFee +
                      ", LateFee=" + LateFee +
                      ", SecurityDeposit=" + SecurityDeposit +
                      ", RightToBuyPrice=" + RightToBuyPrice +
                      ", RightToBuyDeposit=" + RightToBuyDeposit +
                      ", AbandonmentPeriod=" + AbandonmentPeriod +
                      ", ParkingSpace=" + ParkingSpace +
                      ", ParkingSpaceDescription=" + ParkingSpaceDescription +
                      ", TerminationPeriod=" + TerminationPeriod +
                      ", TerminationFee=" + TerminationFee +
                      ", PetsAllowed=" + PetsAllowed +
                      ", PetsFee=" + PetsFee +
                      ", ArmedForcesNotice=" + ArmedForcesNotice +
                      ", LawsState=" + LawsState +
                      "}";
          }
      }
  }
  ```

* **Content/ra-agreement-text.json** file:

  ```json
  [
    {
      "Header": "OCCUPANT(S):",
      "Text": [
        "The Premises is to be occupied strictly as a residential dwelling with the following Two (2) Occupants to reside on the Premises in addition to the Tenant(s) mentioned above: {tenantAdd.knownAs.asList}, hereinafter known as the “Occupant(s)”."
      ]
    },
      
  	...
      
    {
      "Header": "ENTIRE AGREEMENT:",
      "Text": [
      "This Agreement contains all the terms agreed to by the parties relating to its subject matter including any attachments or addendums. This Agreement replaces all previous discussions, understandings, and oral agreements. The Landlord and Tenant(s) agree to the terms and conditions and shall be bound until the end of the Lease Term.",
        "The parties have agreed and executed this agreement on {agreement.date}."
      ]
    } 
  ]
  ```

* **Model/AgreementText.cs** file:

  ```
  namespace RentalAgreement.Model
  {
      public class AgreementText
      {
  
          public string Header { get; set; }
          public string[] Text { get; set; }
  
          public override string ToString() 
          {
              return  "AgreementText{" +  
                      "Header=" + Header +
                      ", Text=" + Text +
                       "}";
          }
      }
  }
  ```

* **Content/ra-checklist.json** file:

  ```json
  [
    {
      "Name": "Living Room",
      "Items": [
          "Floors Condition",
          "Walls Condition",
          "Ceiling Condition",
          "Windows Condition",
          "Lighting Condition",
          "Electrical Outlets",
          "Other Condition",
          "Other Condition"
      ]
    },
      
   	...
    
    {
      "Name": "Other",
      "Items": [
          "Heating Condition",
          "AC Unit",
          "Hot Water",
          "Smoke Alarm",
          "Door Bell",
          "Other Condition",
          "Other Condition"
      ]
    }
  ]
  ```

* **Model/CheckList.cs** file:

  ```c#
  namespace RentalAgreement.Model
  {
      public class CheckList
      {
  
          public string Name { get; set; } = "";
          public string[] Items { get; set; } = { };
  
          public override string ToString() 
          {
              return "CheckList{" +  
                     "Name=" + Name +
                      ", Items=" + Items +
                     "}";
          }
      }
  }
  ```

* **Content/ra-parties.json** file:

  ```json
  [
    {
      "Party": "Landlord",
      "KnownAs": "Landlord",
      "Name": "Best Landlord Company",
      "NameExt": "ATTN. John Landlord",
      "MailAddress": "2 Maple Ln, Suite A, Best Town, Alabama, 29227",
      "Signer": "John Landlord as President of Best Landlord Company"
    },  
  
      ...
      
    {
      "Party": "Manager",
      "Name": "The management company",
      "KnownAs": "Best Management Company",
      "MailAddress": "5 Maple Ave, Suite 12A, Best City, Alabama, 29277",
      "Phone": "(888) 222-3333",
      "EmailAddress": "email@email.com"
    }
  ]
  ```

 * **Model/PartyData.cs** file:

    ```c#
    namespace RentalAgreement.Model
    {
        public class PartyData
        {
    
            public string Party { get; set; } = "";
            public string KnownAs { get; set; } = "";
            public string Name { get; set; } = "";
            public string NameExt { get; set; } = "";
            public string MailAddress { get; set; } = "";
            public string Phone { get; set; } = "";
            public string EmailAddress { get; set; } = "";
            public string Signer { get; set; } = "";
    
    
            public override string ToString() 
          {
                return "AgreementText{" +  
                       "Party=" + Party +
                        ", KnownAs=" + KnownAs +
                        ", Name=" + Name +
                        ", NameExt=" + NameExt +
                        ", MailAddress=" + MailAddress +
                        ", Phone=" + Phone +
                        ", EmailAddress=" + EmailAddress +
                        ", Signer=" + EmailAddress +
                       "}";
            }
        }
    }
    ```

## 5. The Program class

Responsibility:

* Parse command line options.
  You can process the command line options using the `PrepareParameters` that prepares the `Parameters` structure.
* Display prompts to the user when one of the following options is present in the command line: `“?”`, `“-h”`, `“-help”`, `“--h”`, `“--help”`. It is processed by `Usage`.
* Build the document. It delegates the document building to the `RentalAgreementRunner` class with the name of the resulting document file.
* Run the application to demonstrate the resulting document if specified in the command line options.

## 6. The RentalAgreementRunner class

Responsibility:

* Create an object of the `RentalAgreementBuilder` class.

* Initialize its properties with data of Rental Agreement. The properties of the `RentalAgreementBuilder` class have the default values located in several JSON files.

  ```c#
              string agreementTextJsonFile = CheckFile(
                  Path.Combine("Content", "ra-agreement-text.json"));
              string agreementJsonFile = CheckFile(
                  Path.Combine("Content", "ra-agreement.json"));
              string partiesJsonFile = CheckFile(
                  Path.Combine("Content", "ra-parties.json"));
              string checklistJsonFile = CheckFile(
                  Path.Combine("Content", "ra-checklist.json"));
  
              string agreementTextJsonContent = 
                  File.ReadAllText(agreementTextJsonFile);
              string agreementJsonContent = 
                  File.ReadAllText(agreementJsonFile);
              string partiesJsonContent =
                  File.ReadAllText(partiesJsonFile);
              string checklistJsonContent =
                  File.ReadAllText(checklistJsonFile);
  
              List<AgreementText> agreementText =
                  JsonConvert.DeserializeObject<List<AgreementText>>(
                      agreementTextJsonContent);
              AgreementData agreement =
                  JsonConvert.DeserializeObject<AgreementData>(
                      agreementJsonContent);
              List<PartyData> partyData = 
                  JsonConvert.DeserializeObject<List<PartyData>>(
                      partiesJsonContent);
              List<CheckList> checkList = 
                  JsonConvert.DeserializeObject<List<CheckList>>(
                      checklistJsonContent);
  ```

* Build the document. It delegates the document building to the `RentalAgreementBuilder` class using the `Build` method.

  ```c#
              var rentalAgreementBuilder =
                  new RentalAgreementBuilder();
  
              rentalAgreementBuilder.AgreementText = agreementText;
              rentalAgreementBuilder.Agreement = agreement;
              rentalAgreementBuilder.PartyData = partyData;
              rentalAgreementBuilder.CheckList = checkList;
  
              return rentalAgreementBuilder.Build();
  ```

  

## 7. The RentalAgreementBuilder class

Responsibility:

* Save the properties with data.

The method `DocumentBuilder` has the following responsibilities:

* Create an object of the `Gehtsoft.PDFFlow.Builder.DocumentBuilder` class.
* Create an object of the `rentalAgreementTextBuilder` class and build the pages with the main text of the agreement and the signature page using the `Build` method of the created object.
* Create an object of the `rentalAgreementDepositBuilder` class and build the Security Deposit Receipt page using the `Build` method of the created object.
* Create an object of the `rentalAgreementAmountBuilder` class and build the AMOUNT ($) DUE AT SIGNING page using the `Build` method of the created object.
* Create an object of the `rentalAgreementCheckListBuilder` class and build the Move-in Checklist pages using the `Build` method of the created object.
* Return the created object of the `Gehtsoft.PDFFlow.Builder.DocumentBuilder` class.

## 8. The RentalAgreementTextBuilder class

Responsibility:

* Create a page with a repeating area and set its parameters. See Fig. 1.

  ![](../Articles%20Images/RentalAgreement-01.png "The page")

  Fig. 1.
  
  This is done by the `Build` method.
  
  ```c#
  internal static readonly Box Margins = new Box(25, 16, 60, 16);
  internal static readonly XUnit PageWidth =
      (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
          (Margins.Left + Margins.Right));
  
  internal static readonly XUnit PageHeight =
      (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Height -
          (Margins.Top + Margins.Bottom));
  
  	internal void Build(DocumentBuilder documentBuilder)
          {
              var sectionBuilder = documentBuilder.AddSection();
              sectionBuilder
                  .SetOrientation(Orientation)
                  .SetMargins(Margins);
              sectionBuilder.SetRepeatingAreaPriority(
                  RepeatingAreaPriority.Vertical);
          }         
  ```
  
  The `SetRepeatingAreaPriority` method sets the priority of automatic repeating areas for the section (Horizontal or Vertical). If Horizontal - headers and footers stretch to the full width, if Vertical - left and right areas stretch to the full height.
  
* Set the data from the models: 

  ```c#
          private PartyData landlord = new PartyData();
          private PartyData manager = new PartyData();
          private List<PartyData> tenant = new List<PartyData>();
          private List<PartyData> tenantAdd = new List<PartyData>();
          private AgreementData agreement;
  
          internal Dictionary<string, string> dict;
          private List<AgreementText> agreementText;
  
          internal RentalAgreementTextBuilder SetLandord(PartyData landlord)
          {
              this.landlord = landlord;
              return this;
          }
  
          ...
  
          internal RentalAgreementTextBuilder SetAgreementText(
              List<AgreementText> agreementText)
          {
              this.agreementText = agreementText;
              return this;
          }
  ```

* Create the pages with the main text of the agreement and the signature page.
  This is done by the `Build` method. It creates objects one by one and runs the following methods:


* `BuildHeader`
* `BuildFooterBar`
* `BuildEqualHousingOpportunity`
* `BuildTextsPages`
* `BuildSignatures`

```csharp
        internal void Build(DocumentBuilder documentBuilder)
        {
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            sectionBuilder.SetRepeatingAreaPriority(
                RepeatingAreaPriority.Vertical);
            BuildHeader(
                sectionBuilder.AddHeaderToBothPages(80), PageWidth);
            BuildFooterBar(sectionBuilder.AddFooterToBothPages(90), 86);
            BuildEqualHousingOpportunity(
                sectionBuilder.AddRptAreaLeftToBothPages(65), PageHeight);
            sectionBuilder
                .AddParagraph(MAIN_TITLE)
                .SetAlignment(HorizontalAlignment.Center)
                .SetFont(MAIN_TITLE_FONT)
                .SetMarginBottom(MAIN_TITLE_BOTTOM_MARGIN);
            BuildTextsPages(sectionBuilder);
            BuildSignatures(sectionBuilder, GetSignData());
        }
```

Repeating headers and footers are created by the `BuildHeader` and `BuildFooterBar` methods. They are built in the `rentalAgreementHelper` class.

###### The BuildSignatures method

This method creates the signature page. It consists of the image from file **images/ra-logo-2x.png** and a line created using the `AddLine` method:

  ```c#
       private void BuildSignatures(SectionBuilder sectionBuilder, 
            Sign[] data)
        {
            for (int i = 0, l = data.Length; i < l; i++)
            {
                Sign sign = data[i];
                var paragraphBuilder = sectionBuilder.AddParagraph();
                if (i == 0)
                {
                    paragraphBuilder.SetMarginTop(SIGNATURES_TOP_MARGIN);
                }
                if (i < 2)
                {
                    paragraphBuilder
                        .SetFont(HEADER_FONT)
                        .AddTextToParagraph(
                            i == 0 ?
                            "LANDLORD(S) SIGNATURE" :
                            "TENANT(S) SIGNATURE"
                        )
                        .SetMarginBottom(SIGNATURE_BOTTOM_MARGIN);
                    paragraphBuilder = sectionBuilder.AddParagraph();
                }
                paragraphBuilder
                    .AddTextToParagraph(sign.name + " ", HEADER_FONT, true)
                    .AddTabulation(380, TabulationType.Left,
                        TabulationLeading.BottomLine);
                if (sign.value != null)
                {
                    paragraphBuilder = sectionBuilder.AddParagraph()
                        .AddTextToParagraph(sign.value);
                }
                paragraphBuilder.SetMarginBottom(SIGNATURE_BOTTOM_MARGIN);
            }
        }
  ```



## 9. The RentalAgreementHelper class

Responsibility:

* Create a header repeating on every page. See Fig. 2.

  ![](../Articles%20Images/RentalAgreementHeader.png "The header")

Fig. 2.

###### The BuildHeader method

This method refers to `RepeatingAreaBuilder` and builds the header. It consists of the image from the file **images/ra-logo-2x.png** and a line created using the `AddLine` method.

  ```c#
  internal static void BuildHeader(RepeatingAreaBuilder builder, 
      float pageWidth)
  {
      builder
          .AddImage(Path.Combine("images", "ra-logo-2x.png"),
              XSize.FromHeight(60));
      builder.AddLine(pageWidth, 1.5f, Stroke.Solid, Color.Gray)
          .SetMarginTop(5);
  }
  ```

* Create a footer with the signature form only for the pages with the main text of the agreement. See Fig. 3.

  ![](../Articles%20Images/RentalAgreementFooterBar.png "The footer bar")

  Fig. 3.

  ###### The BuildFooterBar method

  This method refers to `RepeatingAreaBuilder` and builds the left part of the footer. It consists of a table with a single row and three cells in it. The first cell contains the text "Initials", the second cell contains a link, and the third cell contains the image from the file **images/ra-barcode.png** and the line, created by `AddLine` method.

  ###### The BuildEqualHousingOpportunity method

  This method refers to `RepeatingAreaBuilder` and builds the right part of the footer. It only builds in the image from file **images/equal-housing-opportunity-logo-160w.png**.

  ```c#
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
  
          internal static void BuildEqualHousingOpportunity(RepeatingAreaBuilder builder,
              float pageHeight)
          {
              builder
                  .AddImage(Path.Combine("images",
                      "equal-housing-opportunity-logo-160w.png"),
                      XSize.FromHeight(64))
                      .SetMarginTop(pageHeight - 66f);
          }
      }
  ```

* Create a footer with a page number for the rest of the pages. See Fig. 4.

  ![](../Articles%20Images/RentalAgreementFooter.png "The footer")

  Fig. 4.

  ###### The BuildFooter method

  This method refers to `RepeatingAreaBuilder` and builds the footer with a page number for the other pages of the document. It consists of one paragraph created using the `AddTextToParagraph` and `AddPageNumber` methods.

  ```c#
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
  ```

* Configure the date format.

  ```c#
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
  ```



## 10. The RentalAgreementDepositBuilder class

Responsibility:

* Create a form of the Security Deposit Receipt page and set the page properties. See Fig. 5.

  ![](../Articles%20Images/RentalAgreementDeposit.png "The deposit")

  Fig. 5.

  ###### The Build method

  This method builds the Security Deposit Receipt page. The `AddTextToParagraph` method adds a text element to the paragraph. The `AddTabulation` method adds a tabulation to the paragraph, it can be configured using the `TabulationType` and `TabulationLeading` parameters.

  ```c#
          internal void Build(DocumentBuilder documentBuilder)
          {
              var sectionBuilder = documentBuilder
                  .AddSection()
                  .SetOrientation(Orientation)
                  .SetMargins(Margins);
              //sectionBuilder.SetRepeatingAreaPriority(RepeatingAreaPriority.Vertical);
              BuildHeader(sectionBuilder.AddHeaderToBothPages(140), PageWidth);
              BuildFooter(sectionBuilder.AddFooterToBothPages(40));
              //BuildFooterQqualHousingOpportunity(sectionBuilder.AddFooterToBothPages(85));
              //BuildQqualHousingOpportunity(sectionBuilder.AddRptAreaLeftToBothPages(85));
              var paragraphBuilder = sectionBuilder.AddParagraph();
              paragraphBuilder.SetFont(MAIN_TITLE_FONT)
                  .SetAlignment(HorizontalAlignment.Center)
                  .SetMarginBottom(MAIN_TITLE_BOTTOM_MARGIN)
                  .AddText("Security Deposit Receipt");
              paragraphBuilder = sectionBuilder.AddParagraph();
              paragraphBuilder
                  .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                  .AddTextToParagraph("Date:", TEXT_FONT, true)
                  .AddTabulation(150, TabulationType.Left,
                      TabulationLeading.BottomLine);
             <...>
              paragraphBuilder = sectionBuilder.AddParagraph();
              paragraphBuilder
                  .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                  .AddTextToParagraph("Landlord’s Signature ", HEADER_FONT, 
                      true)
                  .AddTabulation(250, TabulationType.Left,
                      TabulationLeading.BottomLine);
          }
  ```


## 11. The RentalAgreementAmountBuilder class

Responsibility:

* Create the AMOUNT ($) DUE AT SIGNING page and set the page properties. See Fig. 6.

  ![](../Articles%20Images/RentalAgreementAmount.png "The amount")

  Fig. 6.

  The page is created by the `Build` method.

  ###### The Build method

  This method builds several paragraphs in the page. These paragraphs consist of the text and data implemented by the `AddTextToParagraph` method.

  ```c#
          internal void Build(DocumentBuilder documentBuilder)
          {
              var sectionBuilder = documentBuilder
                  .AddSection()
                  .SetOrientation(Orientation)
                  .SetMargins(Margins);
              //sectionBuilder.SetRepeatingAreaPriority(RepeatingAreaPriority.Vertical);
              BuildHeader(sectionBuilder.AddHeaderToBothPages(140), PageWidth);
              BuildFooter(sectionBuilder.AddFooterToBothPages(40));
              //BuildFooterQqualHousingOpportunity(sectionBuilder.AddFooterToBothPages(85));
              //BuildQqualHousingOpportunity(sectionBuilder.AddRptAreaLeftToBothPages(85));
              sectionBuilder.AddParagraph()
                  .SetAlignment(HorizontalAlignment.Center)
                  .SetMarginBottom(MAIN_TITLE_BOTTOM_MARGIN)
                  .AddText("AMOUNT ($) DUE AT SIGNING");
              sectionBuilder.AddParagraph()
                  .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                  .AddTextToParagraph("Security Deposit: ", HEADER_FONT)
                  .AddTextToParagraph(FundToString(agreement.SecurityDeposit), 
                      TEXT_FONT);
              sectionBuilder.AddParagraph()
                  .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                  .AddTextToParagraph("First (1st) Month's Rent: ", HEADER_FONT)
                  .AddTextToParagraph(FundToString(agreement.MonthPayment), 
                      TEXT_FONT);
              sectionBuilder.AddParagraph()
                  .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                  .AddTextToParagraph("Pet Fee(s): ", HEADER_FONT)
                  .AddTextToParagraph(FundToString(agreement.PetsFee), 
                      TEXT_FONT)
                  .AddTextToParagraph(" for all the Pet(s)", TEXT_FONT);
          }
  ```

  

## 12. The RentalAgreementCheckList Builder class

Responsibility: 

* Create the Move-in Checklist pages. See Fig. 7.

  ![](../Articles%20Images/RentalAgreementCheckList.png "The checklist")

  Fig. 7.

* Configure the data:

  ```
      private AgreementData agreement;
      private List<CheckList> checkList;
  
      internal RentalAgreementCheckListBuilder SetAgreement(
          AgreementData agreement)
      {
          this.agreement = agreement;
          return this;
      }
  
      internal RentalAgreementCheckListBuilder SetCheckList(
          List<CheckList> checkList)
      {
          this.checkList = checkList;
          return this;
      }
  ```

  ###### The Build method

  This method builds three checklist pages. 

  ```c#
          internal void Build(DocumentBuilder documentBuilder)
          {
              var sectionBuilder = documentBuilder.AddSection();
              sectionBuilder
                  .SetOrientation(Orientation)
                  .SetMargins(Margins);
              sectionBuilder.SetRepeatingAreaPriority(
                  RepeatingAreaPriority.Vertical);
              BuildHeader(sectionBuilder.AddHeaderToBothPages(80), PageWidth);
              BuildFooter(sectionBuilder.AddFooterToBothPages(40));
              //BuildFooterQqualHousingOpportunity(sectionBuilder.AddFooterToBothPages(85));
              //BuildQqualHousingOpportunity(sectionBuilder.AddRptAreaLeftToBothPages(65), PageHeight);
              sectionBuilder.AddParagraph()
                  .SetFont(MAIN_TITLE_FONT)
                  .SetAlignment(HorizontalAlignment.Center)
                  .SetMarginBottom(MAIN_TITLE_BOTTOM_MARGIN)
                  .AddText("Move-in Checklist");
              
              	...
                  
              sectionBuilder.AddParagraph()
                  .SetFont(TEXT_FONT)
                  .SetMarginBottom(RAGRAPH_BOTTOM_MARGIN)
                  .AddTextToParagraph("Write the condition of the space along with any specific damage or repairs needed. Be sure to write" +
                      " any repair needed such as paint chipping, wall damage, or any lessened area that could be" +
                      " considered maintenance needed at the end of the lease, and therefore, be deducted at the end of the" +
                      " Lease Term.");
              foreach(CheckList room in checkList)
              {
                  sectionBuilder.AddParagraph()
                      .SetFont(MAIN_TITLE_FONT)
                      .SetMarginBottom(RAGRAPH_BOTTOM_MARGIN)
                      .AddTextToParagraph(room.Name);
                  for (int i = room.Items.Length - 1, j = 0; i >= 0; i--, j++)
                  {
                      string item = room.Items[j];
                      sectionBuilder.AddParagraph()
                          .SetMarginBottom(i > 0 ? 
                                      ROW_BOTTOM_MARGIN : 
                                      RAGRAPH_BOTTOM_MARGIN)
                          .AddTextToParagraph(item + " ", TEXT_FONT, true)
                          .AddTabulation(220, TabulationType.Left,
                              TabulationLeading.BottomLine)
                          .AddTabulation(460, TabulationType.Left,
                              TabulationLeading.BottomLine)
                          .AddTextToParagraph(" Specific Damage ", TEXT_FONT, 
                          true);
  
                  }
  
              }
             
              ...
                  
              sectionBuilder.AddParagraph()
                  .SetMarginBottom(PARAGRAPH_BOTTOM_MARGIN)
                  .AddTextToParagraph("Landlord's Signature ", HEADER_FONT, 
                      true)
                  .AddTabulation(250, TabulationType.Left,
                      TabulationLeading.BottomLine);
          }
  ```

# Summary

The above example showed how to create a complex document that includes tables, images, and paragraphs.

The resulting **RentalAgreement.pdf** document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/RentalAgreement.pdf).

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/RentalAgreement).

