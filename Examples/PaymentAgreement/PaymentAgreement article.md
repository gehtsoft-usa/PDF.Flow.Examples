##### Example: PaymentAgreement

# Purpose
The PaymentAgreement project is an example of payment agreement generation. The example demonstrates how to create a multi-page document that includes a series of paragraphs. This example also demonstrates  usage of images, lines, and tabulation.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/PaymentAgreement).

**Table of Contents**  

- [Prerequisites](#prerequisites)
- [Description](#description)
    + [Agreement data](#agreement-data)
    + [Barcode](#barcode)
    + [Company logo](#company-logo)
    + [Output file](#output-file)
- [Writing the source code](#writing-the-source-code)
    + [1. Create new console application.](#1-create-new-console-application)
    + [2. Modify class Program.](#2-modify-class-program)
    + [3. Create classes to contain the agreement data](#3-create-classes-to-contain-the-agreement-data)
    + [4. Modify class for running document generation](#4-modify-class-for-running-document-generation)
    + [5. Build the document in the PaymentAgreementBuilder class](#5-build-the-document-in-the-paymentagreementbuilder-class)
    + [6. Build the headers for the agreement pages](#6-build-the-headers-for-the-agreement-pages)
    + [7. Build the footers for the agreement pages](#7-build-the-footers-for-the-agreement-pages)
    + [8. Build the text content of the agreement pages](#8-build-the-text-content-of-the-agreement-pages)
    + [9. Build the signatures block at the last of page](#9-build-the-signatures-block-at-the-last-of-page)
- [Summary](#summary)

# Prerequisites
1) **Visual Studio 2017** or above is installed.
   To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
   To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### The Agreement data
The data for the agreement parties is located in the **Content/pa-parties.json** file:
```json
[
  {
    "Party": "Debtor",
    "Name": "John A. Patient",
    "MailAddress": "1 Main Street, Cortland, New York, 10388"
  },  
  {
    "Party": "Co-Signer",
    "Name": "Jill B. Patient"
  },
  {
    "Party": "Creditor",
    "Name": "Best Hospital Group",
    "MailAddress": "1 Health Ln, Nyack, New York, 10977"
  }
]
```

The data for the agreement's fields is located in the **Content/pa-agreement.json** file:
```json
{
  "Date": "2020-12-16T00:00:00",
  "Balance": 37530,
  "DiscountedBalance": 32000,
  "DownPayment": 1500,
  "InterestRate": 3.25,
  "RepaymentPeriodBegins": "2020-01-01T00:00:00",
  "RepaymentPeriodEnds": "2025-12-01T00:00:00",
  "RepaymentFrequency": "1st of each month",
  "ExtensionPeriodOne": 5,
  "ExtensionPeriod": 10,
  "BrokenPayPeriod": "five (5) business days",
  "GoverningLawState": "New York",
  "GoverningLaw": "Governing Law"
}
```

The data for the agreement paragraphs is located in the **Content/pa-agreement-text.json** file:
```json
[
  {
    "Header": "I. THE PARTIES.",
    "Text": [
      "This Payment (Installment) Agreement (“Agreement”) dated {agreement.date}, is by and between:",
      "Creditor: {parties.creditor.name} , with a mailing address of {parties.creditor.mailaddress} (“Creditor”), and",
      "Debtor: {parties.debtor.name}, with a mailing address of {parties.debtor.mailAddress} (“Debtor”).",
      "HEREINAFTER, the Debtor and Creditor (“Parties”) agree to the following:"
    ]
  },
  {
    "Header": "II. BALANCE.",
    "Text": [
      "At the time of this Agreement, the Debtor owes the Creditor the amount of {agreement.balance} (“Current Balance”) for an Outstanding Balance (debt).",
      "a.) Discounted Balance. In consideration of the Debtor’s faith to repay the Current Balance in this Agreement, the Creditor agrees the Current Balance shall be reduced to the discounted balance of {agreement.discountedBalance}. If the Debtor shall default under any of the terms of this Agreement, the Debtor shall owe the Creditor the Current Balance in addition to other penalties, fees, and any accumulated interest."
    ]
  },
  {
    "Header": "III. REPAYMENT PLAN.",
    "Text": [
      "To satisfy the Amount Owed, the Debtor agrees to repay the Creditor under the following terms:",
      "Down-Payment. The Debtor shall pay a downpayment in the amount of {agreement.downPayment}",
      "Interest Rate. The Amount Owed shall bear interest at a rate of {agreement.interestRate} compounded annually. The rate must be equal to or less than the usury rate in the State of the Borrower.",
      "Repayment Period. The Debtor shall re-pay the Creditor on the {agreement.repaymentFrequency} beginning {agreement.repaymentPeriodBegins} and ending on {agreement.repaymentPeriodEnds} in equal installments",
      "Payment Instructions. The payment is required to be made via ACH or physical check"
    ]
  },
  {
    "Header": "IV. LATE PAYMENT.",
    "Text": [
      "Any partial or late payment under this Agreement shall be allowed in accordance with the following:",
      "Allow the Debtor to make payment within {agreement.extensionPeriodOne} days provided the Debtor pays a late fee of: {agreement.extensionPeriod} (“Extension Period”). If payment is not made within the Extension Period, this Agreement shall be in default."
    ]
  },
  {
    "Header": "V. PREPAYMENT:",
    "Text": [
      "The Debtor may pre-pay the Amount Owed without penalty."
    ]
  },
  {
    "Header": "VI. CO-SIGNER.",
    "Text": [
      "This Agreement shall have one (1) Co-Signer known as {parties.co-signer.name} hereinafter known as the “Co-Signer” agrees to the liabilities and obligations on behalf of the Debtor under the terms of this Agreement. If the Debtor does not make a payment, the Co-Signer shall be personally responsible and therefore is guaranteeing the payment of the principal, late fees, and all accrued interest under the terms of this Agreement."
    ]
  },
  {
    "Header": "VII. DEFAULT.",
    "Text": [
      "If for any reason the Debtor should not oblige to any section or portion of this Agreement, the Debtor shall be considered in default. Under such an event, the remaining balance of the Amount Owed shall be due within {agreement.brokenPayPeriod} with the Debtor liable to pay all reasonable attorney's fees and costs of collection of the Creditor. In addition, the Creditor may reclaim any property or goods in connection with the Amount Owed, hold and dispose of the same, and collect expenses, together with any deficiency due from the Debtor, subject to the Debtor’s right to redeem said items pursuant to law." 
    ]
  },
  {
    "Header": "VIII. GOVERNING LAW.",
    "Text": [
      "This Agreement shall be governed by, and construed in accordance with, the laws of the State of {agreement.governingLawState} (“{agreement.governingLaw}”)." 
    ]
  },
  {
    "Header": "IX. SEVERABILITY:",
    "Text": [
      "The unenforceability or invalidity of any clause in this Agreement shall not have an impact on the enforceability or validity of any other clause. Any unenforceable or invalid clause shall be regarded as removed from this Agreement to the extent of its unenforceability and invalidity. Therefore, this Agreement shall be interpreted and enforced as if it did not contain the said clause to the extent of its unenforceability and invalidity." 
    ]
  },
  {
    "Header": "X. ENTIRE AGREEMENT.",
    "Text": [
      "This Agreement contains all the terms agreed to by the Debtor and Creditor relating to its subject matter, including any attachments or addendums. This Agreement replaces all previous discussions, understandings, and oral agreements.",
      "IN WITNESS WHEREOF, the Parties have executed this Agreement as of the undersigned dates written below." 
    ]
  }  
]
```

### Barcode
A picture of the agreement barcode is in the `images/pa-barcode.png` file.

### Company logo
The company's logo is in the `images/pa-logo.png` file.

### Output file
The example creates the **PaymentAgreement.pdf** file in the output **bin/(Debug|Release)/netcoreapp2.1** folder.


# Writing the source code

#### 1. Create a new console application.
1.1.    Run Visual Studio  
1.2.    File -> Create -> Console Application (.Net Core)

#### 2. Modify the class Program.
2.1. Set the path to the output PDF-file in the `Main` method:

```c#
    string file = "PaymentAgreement.pdf";
```
2.2. Call the `Run` method of a "runner" class for the agreement generation and the **Build()** for building a document into an output PDF-file:

```c#
    PaymentAgreementRunner.Run().Build(file);
```
2.3. After the generation is completed, notify the user about the successful generation:

At first add at the top of the file:

```c#
using System.IO;
```
Then write before the end of the method `Main`:
```c#
    Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                        + "\" document has been successfully built");
```
We have an error that  `PaymentAgreementRunner` is not defined. To resolve it, add a new class `PaymentAgreementRunner.cs` and define the `Run` method in it:

```c#
using Gehtsoft.PDFFlow.Builder;

namespace PaymentAgreement
{
    internal class PaymentAgreementRunner
    {
        internal static DocumentBuilder Run()
        {
            return DocumentBuilder.New().AddSectionToDocument();
        }
    }
}
```

2.4 After running the project we get **PaymentAgreement.pdf** file in the output **bin/(Debug|Release)/netcoreapp2.1** folder. Open it in **Adobe Acrobat**. It is empty and it's normal, because we haven't added anything to it yet. 

Now, run the project again. If you didn't close **Adobe Acrobat** before running you would get an error: **FILE_OPEN_ERROR**.

This occurs because **Adobe Acrobat** has locked the file.  This is not convenient for development, and it is better to use a viewer that shows a document without locking it.

 Also it isn't too convenient to have the document in in the output **bin/(Debug|Release)/netcoreapp2.1** folder.

Lets make little changes in `Program.cs` to allow us to specify the options of the command line:

* A full path to the result file of the document.
* the name of a desired application to view the resulting document.

~~~c#
using System;
using System.Diagnostics;
using System.IO;

namespace PaymentAgreement
{
    class Program
    {
        static int Main(string[] args)
        {
            Parameters parameters = new Parameters(null, "PaymentAgreement.pdf");

            if (!PrepareParameters(parameters, args))
            {
                return 1;
            }

            try {
                PaymentAgreementRunner.Run().Build(parameters.file);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                return 1;
            }
            Console.WriteLine("\"" + Path.GetFullPath(parameters.file) 
                + "\" document has been successfully built");
            if (parameters.appToView != null)
            {
                Start(parameters.file, parameters.appToView);
            }
            return 0;
        }

        private static void Start(string file, string appToView)
        {
            ProcessStartInfo psi;
            psi = new ProcessStartInfo("cmd", @"/c start " + appToView + " " + file);
            Process.Start(psi);
        }

        private static bool PrepareParameters(Parameters parameters, string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].Equals("?")
                    || args[0].Equals("-h")
                    || args[0].Equals("-help")
                    || args[0].Equals("--h")
                    || args[0].Equals("--help")
                    )
                {
                    Usage();
                    return false;
                }
                parameters.file = args[0];
                if (args.Length > 1)
                {
                    parameters.appToView = args[1];
                }
            }

            if (File.Exists(parameters.file))
            {
                try
                {
                    File.Delete(parameters.file);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Can't delete file: " + 
                        Path.GetFullPath(parameters.file));
                    Console.Error.WriteLine(e.Message);
                    return false;
                }
            }
            return true;
        }
        private static void Usage()
        {
            Console.WriteLine("Usage: dotnet run [fullPathToOutFile] [appToView]");
            Console.WriteLine(
                "fullPathToOutFile - a path to the result file, 'PaymentAgreement.pdf' by default");
            Console.WriteLine(
                "appToView - the name of an application to view the file immediately after preparing, by default none app starts");
        }

        internal class Parameters
        {
            public string appToView;
            public string file;
            public Parameters(string appToView, string file)
            {
                this.appToView = appToView;
                this.file = file;
            }
        }
    }
}

~~~



The changes we made:

* Added `internal class Parameters`. We need it to parse the command line options to its properties values.
* Implemented  method `PrepareParameters` that:
  * Analyzes the command line options.
  * If an option is set for the popular **help** options (?, -h, --h, -help, --help) then the list of the command  line options is shown.
  * Fills `Parameters` properties: `appToView` and `file`
* Added `try-catch` for error processing to make the error message clearer than just **FILE_OPEN_ERROR**
* Added an ability to run a desired PDF viewer, if it is specified in the command line options.

To specify the options in Visual Studio open Menu->Project->PaymentAgreement properties->Debug and specify **Application arguments:** ../../../PaymentAgreement.pdf chrome.

Now, run the project again. Now the empty document **PaymentAgreement.pdf** is shown in the Chrome, assuming you have it installed.

#### 3. Create classes that contain the agreement data

3.1. Create a new folder: `Model`

3.2. Add a new class `PaymentAgreement.cs` in the folder `Model`:

```c#
namespace PaymentAgreement.Model
{
    public class PartyData
    {

        public string Party { get; set; } = "";
        public string Name { get; set; } = "";
        public string MailAddress { get; set; } = "";

        public override string ToString() 
        {
            return "AgreementText{" +  
                   "Party=" + Party +
                   ", Name=" + Name +
                   ", MailAddress=" + MailAddress +
                   "}";
        }
    }
}
```

3.3. Add a new class `AgreementData.cs` in the folder `Model`:

```c#
using System;

namespace PaymentAgreement.Model
{
    public class AgreementData
    {

        public DateTime Date { get; set; }
        public double Balance { get; set; }
        public double DiscountedBalance { get; set; }
        public double DownPayment { get; set; }
        public double InterestRate { get; set; }
        public DateTime RepaymentPeriodBegins { get; set; }
        public DateTime RepaymentPeriodEnds { get; set; }
        public string RepaymentFrequency { get; set; }
        public int ExtensionPeriodOne { get; set; }
        public int ExtensionPeriod { get; set; }
        public string BrokenPayPeriod { get; set; }
        public string GoverningLawState { get; set; }
        public string GoverningLaw { get; set; }

        public override string ToString() 
        {
            return  "AgreementData{" +  
                    "Date=" + Date +
                    ", Balance=" + Balance +
                    ", DiscountedBalance=" + DiscountedBalance +
                    ", DownPayment=" + DownPayment +
                    ", InterestRate=" + InterestRate +
                    ", RepaymentPeriodBegins=" + RepaymentPeriodBegins +
                    ", RepaymentPeriodEnds=" + RepaymentPeriodEnds +
                    ", RepaymentFrequency=" + RepaymentFrequency +
                    ", ExtensionPeriodOne=" + ExtensionPeriodOne +
                    ", ExtensionPeriod=" + ExtensionPeriod +
                    ", BrokenPayPeriod=" + BrokenPayPeriod +
                    ", GoverningLawState=" + GoverningLawState +
                    ", GoverningLaw=" + GoverningLaw +
                    "}";
        }
    }
}
```

3.4. Add a new class `AgreementText.cs` in the folder `Model`:

```c#
namespace PaymentAgreement.Model
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

#### 4. Modify class for running document generation

4.1. Modify the class `PaymentAgreementRunner.cs`:

```c#
using System.Collections.Generic;
using System.IO;
using PaymentAgreement.Model;
using Gehtsoft.PDFFlow.Builder;
using Newtonsoft.Json;

namespace PaymentAgreement
{
    internal class PaymentAgreementRunner
    {
        internal static DocumentBuilder Run()
        {
            return DocumentBuilder.New().AddSectionToDocument();
        }
    }
}
```

4.2. Add the `CheckFile` method to the `PaymentAgreementRunner` class:

```c#
    private static string CheckFile(string file)
    {
        if (!File.Exists(file))
        {
            throw new IOException("File not found: " + Path.GetFullPath(file));
        }
        return file;
    }
```

This method is responsible for checking if the file exists. If it doesn't exist, then an instance of `IOException` will be thrown.

4.3 Rewrite completely the `Run` method of the `PaymentAgreementRunner` class. 

4.3.1. Firstly, read the JSON data from the files:

```c#
        public static DocumentBuilder Run()
        {
            string agreementTextJsonFile = CheckFile(
                Path.Combine("Content", "pa-agreement-text.json"));
            string agreementJsonFile = CheckFile(
                Path.Combine("Content", "pa-agreement.json"));
            string partiesJsonFile = CheckFile(
                Path.Combine("Content", "pa-parties.json"));

            string agreementTextJsonContent = 
                File.ReadAllText(agreementTextJsonFile);
            string agreementJsonContent = 
                File.ReadAllText(agreementJsonFile);
            string partiesJsonContent =
                File.ReadAllText(partiesJsonFile);
    
            List<AgreementText> agreementText =
                JsonConvert.DeserializeObject<List<AgreementText>>(agreementTextJsonContent);
            AgreementData agreement =
                JsonConvert.DeserializeObject<AgreementData>(agreementJsonContent);
            List<PartyData> partyData = 
                JsonConvert.DeserializeObject<List<PartyData>>(partiesJsonContent);
```
4.3.2. Secondly, we create an instance of a "builder" class and pass the data to it:

```c#
            var paymentAgreementBuilder =
                new PaymentAgreementBuilder();

            paymentAgreementBuilder.AgreementText = agreementText;
            paymentAgreementBuilder.Agreement = agreement;
            paymentAgreementBuilder.PartyData = partyData;
```

4.3.3. In the end, we return an instance of the `DocumentBuilder` class so we can save the document later:

```c#
            return paymentAgreementBuilder.Build();
        }
```
We get an error that `PaymentAgreementBuilder` is not defined. To solve it add a new class `PaymentAgreementBuilder.cs`, define the `Build` method, and the properties referred by `PaymentAgreementRunner`  in it:

```c#
using Gehtsoft.PDFFlow.Builder;
using PaymentAgreement.Model;
using System.Collections.Generic;

namespace PaymentAgreement
{
    internal class PaymentAgreementBuilder
    {
        public List<AgreementText> AgreementText { get; internal set; }
        public AgreementData Agreement { get; internal set; }
        public List<PartyData> PartyData { get; internal set; }

        internal DocumentBuilder Build()
        {
            return DocumentBuilder.New().AddSectionToDocument();
        }
    }
}
```

4.3.4. After running the project we get an empty document, as expected.

#### 5. Build the document in the PaymentAgreementBuilder class

5.1. Modify the file `PaymentAgreementBuilder.cs`. 

Insert at the top of the file:

```c#
using System.IO;
using System;
using System.Globalization;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;
using System.Text.RegularExpressions;
using System.Text;
```

5.2. Create constants to use them for reducing the size of the code.

```c#
    internal class PaymentAgreementBuilder
    {

        internal static readonly CultureInfo DocumentLocale
            = new CultureInfo("en-US");
        internal const PageOrientation Orientation
            = PageOrientation.Portrait;
        internal static readonly Box Margins = new Box(29, 20, 29, 20);
        internal static readonly XUnit PageWidth =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));

        internal static readonly FontBuilder FNT9 =
            Fonts.Times(9f);
        internal static readonly FontBuilder FNT11 =
            Fonts.Times(11f);
        internal static readonly FontBuilder FNT11B =
            Fonts.Times(11f).SetBold();
        internal static readonly FontBuilder FNT20B =
            Fonts.Times(20f).SetBold();
        
        internal const string MAIN_TITLE = "PAYMENT AGREEMENT";
        
```

5.3. Create a dictionary that will contain the values for resolving  placeholders in paragraphs like {agreement.extensionPeriodOne}

```c#
        internal Dictionary<string, string> dict;
```

5.4. Create properties to store the data of agreement

Some of them defined in the class. Rewrite all of them.

```c#
        private List<PartyData> partyData;
        private PartyData debtor = new PartyData();
        private PartyData cosigner = new PartyData();
        private PartyData creditor = new PartyData();

        public List<AgreementText> AgreementText { get; internal set; }
        public AgreementData Agreement { get; internal set; }
        public List<PartyData> PartyData
        {
            get { return partyData; }
            internal set
            {
                partyData = value;
                SyncPartyData();
            }
        }
```

5.5. Create a method to synchronize the values of the properties `debtor`, `cosigner`, `creditor` with the value that was set to the `partyData` property

```c#
        private void SyncPartyData()
        {
            foreach (PartyData party in partyData)
            {
                switch (party.Party)
                {
                    case "Debtor":
                        debtor = party;
                        break;
                    case "Co-Signer":
                        cosigner = party;
                        break;
                    case "Creditor":
                        creditor = party;
                        break;
                }
            }
        }
```

5.6. Rewrite completely the `Build` method:

```c#
        internal DocumentBuilder Build()
        {            
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            BuildHeader(sectionBuilder.AddHeaderToBothPages(80));
            BuildFooter(sectionBuilder.AddFooterToBothPages(70));
            sectionBuilder
                .AddParagraph(MAIN_TITLE)
                .SetAlignment(HorizontalAlignment.Center)
                .SetFont(FNT20B)
                .SetMarginBottom(30);
            BuildTextsPages(sectionBuilder);
            BuildSignatures(sectionBuilder, GetSignData());
            return documentBuilder;
        }

```

What we have done in this method:
* Created a document builder.

* Added a section to the document.

* Set the section 'Portrait' orientation and margins.

* Built the headers for all pages.

* Built the footers for all pages.

* Added the main title to the first page.

* Set center alignment for the title. 

* Set a font for the title.

* Set the bottom margin for the title.

* Built all texts of the agreement.

* Built the signatures at the end of the agreement.


We have received a lot of errors about the method not being defined. Add empty implementation for them, as it is proposed by  Visual Studio. Remove the throwing `NotImplementedException` from them.

~~~c#
        private void BuildHeader(RepeatingAreaBuilder repeatingAreaBuilder)
        {
        }

        private void BuildFooter(RepeatingAreaBuilder repeatingAreaBuilder)
        {
        }

        private void BuildTextsPages(SectionBuilder sectionBuilder)
        {
        }

        private void BuildSignatures(SectionBuilder sectionBuilder, object p)
        {
        }
        private object GetSignData()
        {
            return null;
        }

~~~



#### 6. Build the headers for the agreement pages

Implement the method to build the header:


```c#
        private void BuildHeader(RepeatingAreaBuilder builder)
        {
            builder
                .AddImage(Path.Combine("images", "pa-logo.png"), 
                    XSize.FromHeight(75));
            builder.AddLine(PageWidth, 1.5f, Stroke.Solid, Color.Gray)
                .SetMarginTop(5);

        }

```

What we have done in this method:

* Added  "images/pa-logo.png" image to the header and set its scaling to 75 points from height

* Added a line to the header after the image and set its depth to 1.5 points. The line is solid and gray color.

After running the project we get

* Page with a 'Portrait' orientation.

* Page with the title and the logo. See Fig. 1.

![Fig. 1](../Articles%20Images/PaymentAgreement-header.png "The  header")

Fig. 1

#### 7. Build the footers for the agreement pages

Implement the method to build the footer:


```c#
        private void BuildFooter(RepeatingAreaBuilder builder)
        {
            builder
                .AddImage(Path.Combine("images", "pa-barcode.png"),
                    XSize.FromHeight(50))
                .SetAlignment(HorizontalAlignment.Left);
            ParagraphBuilder paragraphBuilder = builder
                .AddParagraph();
            FontBuilder font = Fonts.Helvetica(7f).SetBold().SetUnderline(Stroke.Solid, Color.Blue);
            paragraphBuilder
                .AddTabSymbol()
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFont(font))
                .AddUrlToParagraph("http://www.besthospital.com/payments")
                .AddTabulation(2, TabulationType.Left,
                    TabulationLeading.None)
                .AddTabulation(550, TabulationType.Right,
                    TabulationLeading.None)
                .AddTextToParagraph(" ", FNT11, true)
                .AddTextToParagraph("Page ", FNT9)
                .AddPageNumber().SetFont(FNT9);
        }

```

What we have done in this method:

* Added "images/pa-barcode.png" image to the footer and set its scaling to 50 points from height,  set its alignment to left.

* Added a paragraph to the footer with the URL and the page number and set the URL style. We used used tabulation to place the page number on the right side of the page.

After running the project we get a page with the footer. See Fig. 2.

![Fig. 2](../Articles%20Images/PaymentAgreement-footer.png "The  footer")

Fig. 2

#### 8. Build the text content of the agreement pages

8.1 Implement the method to build text pages:

~~~c#
        private void BuildTextsPages(SectionBuilder sectionBuilder)
        {
            foreach (AgreementText article in AgreementText)
            {
                var paragraphBuilder = sectionBuilder.AddParagraph();
                paragraphBuilder.AddText(article.Header).SetFont(FNT11B);
                paragraphBuilder.SetMarginBottom(16);
                int last = article.Text.Length - 1;
                if (last < 0)
                {
                    continue;
                }
                paragraphBuilder.SetFont(FNT11).AddText(" ");
                for (int i = 0; ; i++)
                {
                    paragraphBuilder.AddText(article.Text[i]);
                    if (i == last)
                    {
                        break;
                    }
                    paragraphBuilder = sectionBuilder.AddParagraph().SetFont(FNT11);
                    paragraphBuilder.SetMarginBottom(16);
                }
            }
        }

~~~

What we have done in this method:

* For each block of text from the collection stored in AgreementText properties we:

  * Created the first paragraph for the block of text. Specified bottom margin for the paragraph. Added a title to the paragraph.

  * For each text of the block of text:

    * Added the text to the paragraph

    * Created a paragraph for next text except the last one, specified the bottom margin for the paragraph.

After running the project we get the pages with text. See a fragment in Fig. 3.



![Fig. 2](../Articles%20Images/PaymentAgreement-text.png "The text")

Fig. 3

As we see there are defects like `{agreement.date}` in the text. They occur because the agreement data in the **Content/pa-agreement-text.json** file contain placeholders which must be replaced with a value from other json files. We will do it next.

8.2. Create a method to initialize the dictionary with the values for resolving  placeholders in paragraphs:


```c#
        private void InitDictionary()
        {
            this.dict = new Dictionary<string, string>
            {
                ["agreement.date"] = DateToString(Agreement.Date),
                ["parties.creditor.name"] = creditor.Name,
                ["parties.creditor.mailaddress"] = creditor.MailAddress,
                ["parties.debtor.name"] = debtor.Name,
                ["parties.debtor.mailAddress"] = debtor.MailAddress,
                ["parties.co-signer.name"] = cosigner.Name,
                ["agreement.balance"] = FundToString(Agreement.Balance),
                ["agreement.discountedBalance"] = FundToString(Agreement.DiscountedBalance),
                ["agreement.downPayment"] = FundToString(Agreement.DownPayment),
                ["agreement.interestRate"] = PercentsToString(Agreement.InterestRate),
                ["agreement.repaymentFrequency"] = Agreement.RepaymentFrequency,
                ["agreement.repaymentPeriodBegins"] = DateToString(Agreement.RepaymentPeriodBegins),
                ["agreement.repaymentPeriodEnds"] = DateToString(Agreement.RepaymentPeriodEnds),
                ["agreement.extensionPeriodOne"] = Agreement.ExtensionPeriodOne.ToString(),
                ["agreement.extensionPeriod"] = Agreement.ExtensionPeriod.ToString(),
                ["agreement.brokenPayPeriod"] = Agreement.BrokenPayPeriod,
                ["agreement.governingLawState"] = Agreement.GoverningLawState,
                ["agreement.governingLaw"] = Agreement.GoverningLaw
            };
        }

```

8.3. Create methods to convert values of different types to string type:

```c#
        private string DateToString(DateTime date)
        {
            return date.ToString(
                        "MMMM dd yyyy", DocumentLocale);
        }

        private string FundToString(double fund)
        {
            return "$" + String.Format(
                DocumentLocale, "{0:0,0.00}", fund);
        }

        private string PercentsToString(double procents)
        {
            if (procents < 10)
            {
                return 
                    String.Format(DocumentLocale, "{0:,0.00}", procents) + "%";
            }
            return String.Format(DocumentLocale, "{0:0,0.00}", procents) + "%";
        }

```

8.4. Create methods to resolve placeholders:

```c#
        private string SubsPlaceholders(string text)
        {
            Regex regex = new Regex(@"\{\s*([^\s\}]+)\s*\}");
            MatchCollection matches = regex.Matches(text);
            if (matches.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                int end = 0;
                foreach(Match match in matches)
                {
                    int start = match.Index;
                    sb.Append(text.Substring(end, start - end));
                    sb.Append(SubsPlaceholder(match));
                    end = start + match.Length;

                }
                sb.Append(text.Substring(end));
                text = sb.ToString();
            }
            return text;
        }

        private string SubsPlaceholder(Match match)
        {
            if (dict.TryGetValue(match.Groups[1].ToString(), out string result))
                return result;
            return match.Groups[0].ToString();
        }
```

8.5. Change  the `BuildTextsPages` method to resolve placeholders:

```c#
        private void BuildTextsPages(SectionBuilder sectionBuilder)
        {
            InitDictionary();
            foreach (AgreementText article in AgreementText) {
                var paragraphBuilder = sectionBuilder.AddParagraph();
                paragraphBuilder.AddText(article.Header).SetFont(FNT11B);
                paragraphBuilder.SetMarginBottom(16);
                int last = article.Text.Length - 1;
                if (last < 0)
                {
                    continue;
                }
                paragraphBuilder.SetFont(FNT11).AddText(" ");
                for (int i = 0; ; i++)
                {
                    paragraphBuilder.AddText(SubsPlaceholders(article.Text[i]));
                    if (i == last)
                    {
                        break;
                    }
                    paragraphBuilder = sectionBuilder.AddParagraph().SetFont(FNT11);
                    paragraphBuilder.SetMarginBottom(16);
                }
            }
        }

```

After running the project we get the pages with text where the data values are used instead of the placeholders. See a fragment in Fig. 4.



![Fig. 2](../Articles%20Images/PaymentAgreement-text2.png "The text")

Fig. 4

As we see now we have 2 pages instead 1. But we didn't have to control the boundary of a page nor the additional of a new page. This is one of the major benefits of using the PDFFlow library, which takes care of the creation of new pages as necessary. 

#### 9. Build the signatures block on the last page

8.1 Create a structure to contain the signature data. We need it to reduce the code size by using a cycle for signatures:

~~~c#
        internal struct Sign
        {
            public string name;
            public string value;
            public Sign(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
        }

~~~

8.2 Implement the `GetSignData` method  which prepares signatures from the data values of the `PartyData` property:

~~~c#
        private Sign[] GetSignData()
        {
            Sign[] result = new Sign[PartyData.Count];
            for (int i = 0, l = PartyData.Count; i < l; i++)
            {
                PartyData party = PartyData[i];
                result[i] = new Sign(party.Party + "’s Signature:", party.Name);
            }
            return result;
        }

~~~

8.3. Implement the `BuildSignatures` method which adds signatures to the end of the last page:

~~~c#
        private void BuildSignatures(SectionBuilder sectionBuilder, Sign[] data)
        {

            foreach(Sign sign in data)
            {
                sectionBuilder.AddParagraph()
                   .SetMarginBottom(20)
                   .AddTextToParagraph(sign.name, FNT11B, true)
                   .AddTextToParagraph(" Date: ", FNT11, true)
                   .AddTabulation(250, TabulationType.Left,
                        TabulationLeading.BottomLine)
                   .AddTabulation(500, TabulationType.Right,
                        TabulationLeading.BottomLine);
                sectionBuilder.AddParagraph().SetMarginBottom(20)
                    .AddText(sign.value);
            }
        }

~~~

What we have done in this method:

* For each signature we:

  * Created the first paragraph for the signature. Added the signature name to it. Added a line under the expected signature using tabulation.

  * Created a second paragraph for the signature. Specified bottom margin to separate signatures. Added the signature value to it.


After running the project we get a complete document. See a fragment in Fig. 5.



![Fig. 2](../Articles%20Images/PaymentAgreement-sign.png "The text")

Fig. 5

# Summary
The above example shows how to create a multi-page document that includes a series of paragraphs, images, and signatures. 

The resulting PaymentAgreement.pdf document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/PaymentAgreement.pdf).

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/PaymentAgreement).
