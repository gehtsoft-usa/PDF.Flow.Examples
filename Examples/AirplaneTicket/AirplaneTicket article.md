##### Example: AirplaneTicket

The **AirplaneTicket** project is an example of creation of an electronic airplane ticket using PDFFlow library. This example demonstrates how to create a single page document that contains different sections, how to arrange and format these sections. Also the example explains the process of filling the tables with data from JSON files.

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/AirplaneTicket). 

**Table of Content**  

- [Prerequisites](#prerequisites)
- [Purpose](#purpose)
- [Description](#description)
- [Writing the source code](#writing-the-source-code)
  - [Create a blank single page document](#1-create-a-blank-single-page-document)
  - [Create models to process JSON files](#2-create-models-to-process-json-files)
  - [Build a header](#3-build-a-header)
  - [Build the "Ticket Information" section](#4-build-ticket-information-section)
  - [Build the "Route" section](#5-build-"route"-section)
  - [Build the "About your trip" section](#6-build-"About-your-trip"-section)
  - [Build the "Fare breakdown" section](#7-build-"fare-breakdown"-section)
  - [Build the "Need help" section](#8-build-"need-help"-section)
- [Summary](#summary)

# Prerequisites
1. **Visual Studio 2017** or later is installed.
To install a community version of Visual Studio, use the following link: https://visualstudio.microsoft.com/vs/community/.
Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.
2. **.NET Core Framework SDK 2.1** or later is installed.
To install the framework, use the following link: https://dotnet.microsoft.com/download.

# Purpose
The example shows how to create an “Airplane Ticket”, which is a complex single page document.

The page consists of the following parts (see figures below):
* A header
* The "Ticket Information" section
* The "Route" section
* The "About your trip" section
* The "Fare breakdown" section
* The "Need help" section

![Fig. 1](../Articles%20Images/AirplaneTicket_1.png)

Step-by-step instructions on how to build each part are provided in this article.

# Description

####  Ticket data 

General information about the passenger and the ticket is located in the **Content/ticket-data.json** file:

```json
{
  "Status": "Confirmed",
  "Company": "Sample",
  "Passenger": "Pavel Rempel",
  "Document": "123456789",
  "TicketNo": "123-1234567890",
  "Order": "XXX1234X",
  "Issued": "2020-05-12T00:00:00"
}
```

Information about the flight route is located in the **Content/route-data.json** file:

```json
[
  {
    "Flight": "Sample 168",
    "FlightCompany": "Sample Airlines",
    "FlightPlaner": "Airbus A321",
    "Departure": "2020-06-16T07:05:00",
    "DepartureAirport": "Istanbul, SAW",
    "Arrival": "2020-06-16T07:30:00",
    "ArrivalAirport": "New York, JFK",
    "Class": "Econom",
    "ClassAdd": "Flexible",
    "Baggage": "1 place 23 kg",
    "BaggageAdd": "Cabin 10 kg",
    "CheckIn": "2020-06-16T06:25:00",
    "CheckInAirport": "Istanbul, SAW"
  },
  {
    "Flight": "Sample 169",
    "FlightCompany": "Sample Airlines",
    "FlightPlaner": "Airbus A321-100/200",
    "Departure": "2020-07-04T10:20:00",
    "DepartureAirport": "New York, JFK",
    "Arrival": "2020-07-05T05:30:00",
    "ArrivalAirport": "Istanbul, SAW",
    "Class": "Econom",
    "ClassAdd": "Flexible",
    "Baggage": "1 place 23 kg",
    "BaggageAdd": "Cabin 10 kg",
    "CheckIn": "2020-07-04T09:40:00",
    "CheckInAirport": "New York, JFK"
  }
]
```

Additional information about the flight is located in the **Content/about-trip.json** file:

```json
[
  "Use your Trip ID for all communication with {company.name} Company about this booking",
  "Check-in counters at all Airports close 45 minutes before departure",
  "For {company.name} Airways flights the free check-in baggage allowance is 15 kgs in Economy class    for domestic travel within US",
  "Your carry-on baggage shouldn't weigh more than 7kgs",
  "Carry photo identification, you will need it as proof of identity while checking-in",
  "If Cancellation is done through the customer support executives assistance, we will levy 500.00 	    USD per passenger per flight, however, if you do it online using your {company.name} Company          account, we will only levy 250 USD per passenger per flight as {company.name} Company Processing      charges. This is over and above the airline cancellation charges.",
  "For hassle free refund processing, cancel/amend your tickets with {company.name} Company Customer    Care instead of doing so directly with Airline."
]
```

Information about the ticket fare is located in the **Content/fare-breakdown.json** file:

```json
[
  {
    "Name": "Base fare",
    "Fare": 1840
  },
  {
    "Name": "Airline fuel charge",
    "Fare": 200
  },
  {
    "Name": "Passenger service fee",
    "Fare": 239
  },
  {
    "Name": "User development fee",
    "Fare": 389
  },
  {
    "Name": "Govt. Service Tax",
    "Fare": 117
  },
  {
    "Name": "Congestion fee",
    "Fare": 50
  },
  {
    "Name": "Other Charges and Taxes",
    "Fare": 50
  }
]
```

Information about contacts is located in the **Content/help-list.json** file:

```json
[
  {
    "Name": "{company.name} company support",
    "Value": "+1 124 5677 8765"
  },
  {
    "Name": "{company.name} airline helpline",
    "Value": "+1 124 5677 8766"
  },
  {
    "Name": "Need a hotel",
    "Value": "+1 124 5677 8767"
  }
]
```

#### Image of logo

Image of the logo of the airline company is located in the **images/AT_Logo_2x.png**

#### Output file
The example creates the **AirplaneTicket.pdf** file in the **bin/(debug|release)/netcoreapp2.1** output folder, unless specified otherwise in the command line.

# Writing the source code

## 1. Create a blank single page document.

1.1. Run Visual Studio, select **File** > **Create** > **Console Application (.Net Core)**, specify the project name as **AirplaneTicket**.

1.2. Create internal class `Parameters` inside class `Program` of the new project:

```c#
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
```

1.3. Add private method `Start` to class `Program`. This method opens the output file in the specified application:

```c#
        private static void Start(string file, string appToView)
        {
            ProcessStartInfo psi;
            psi = new ProcessStartInfo("cmd", @"/c start " + appToView + " " + file);
            Process.Start(psi);
        }
```

Remove errors by adding namespace to the top of class `Program`:

```c#
using System.Diagnostics
using System.IO;
```

1.4. Add private method `PrepareParameters` to class `Program`. This method checks and processes optional parameters that may be passed when the project is executed:

```c#
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
            return true;
        }
```

1.5. Add check logic of the output file to method `PrepareParameters` :

```c#
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
```

1.6. Add method `Usage` to class `Program` that displays information about optional parameters in the console:

```c#
        private static void Usage()
        {
            Console.WriteLine("Usage: dotnet run [fullPathToOutFile] [appToView]");
            Console.WriteLine("fullPathToOutFile - a path to the result file, ");
            Console.Write("'AirplaneTicket.pdf' by default");
            Console.WriteLine("appToView - the name of an application to view the file immediately"); 
            Console.Write(" after preparing by default none app starts");
        }
```

1.7. Modify method `Main` in class `Program`:

```c#
        static int Main(string[] args)
        {
            Parameters parameters = new Parameters(null, "AirplaneTicket.pdf");

            if (!PrepareParameters(parameters, args))
            {
                return 1;
            }

            try {
                AirplaneTicketRunner.Run().Build(parameters.file);
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
```

1.8. Add new class `AirplaneTicketRunner` to the project:

```c#
using Gehtsoft.PDFFlow.Builder;
using System.Collections.Generic;
using System.IO;

namespace AirplaneTicket
{
    public static class AirplaneTicketRunner
    {
        public static DocumentBuilder Run()
        {
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            documentBuilder.AddSection();

            return documentBuilder;
        }
    }
}
```

1.9. Run the project. There are two ways to do this:

* From Visual Studio by clicking **F5**.

* From a command line: in the directory with **AirplaneTicket.csproj**, run the command: 

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
Where: fullPathToOutFile - a path to the result file, 'MedicalBill.pdf' by default
appToView - the name of an application to view the file immediately after preparing, by default none app starts
```

You can set these optional parameters. For example, if you want to place an output file in the root directory of the project and view the file in the **Microsoft Edge** browser, write:

```
dotnet run ../../../AirplaneTicket.pdf msedge
```

After execution of the project, the **AirplaneTicket.pdf** file is generated. It contains a blank page.



## 2. Create models to process JSON files.

2.1. Create three new folders inside the project directory. Right-click on the `AirplaneTicket` project name in the Solution Explorer window and select **Add** > **New Folder**.

Rename first folder as **Content** and copy all JSON files used in the project there.

Rename second folder as **images** and copy the logo image there.

Rename third folder as **Model**.

2.2. Create a **Model/TicketData.json** file to read all items from the **Content/ticket-data.json** file:

```c#
using System;

namespace AirplaneTicket.Model
{
    public class TicketData
    {
        public string Company { get; set; }
        public string Passenger { get; set; }
        public string Document { get; set; }
        public string TicketNo { get; set; }
        public string Order { get; set; }
        public DateTime Issued { get; set; }
        public string Status { get; set; }


        public override string ToString() 
        {
            return  "TicketData{" + 
                    "Company=" + Company +
                    ", Passenger=" + Passenger +
                    ", Document=" + Document +
                    ", TicketNo=" + TicketNo +
                    ", Order=" + Order +
                    ", Issued=" + Issued +
                    ", Status=" + Status +
                     "}";

        }
    }
}
```

2.3. Create a **Model/RouteData.json** file to read all data from the **Content/route-data.json** file:

```c#
using System;

namespace AirplaneTicket.Model
{
    public class RouteData
    {

        public string Flight { get; set; }
        public string FlightCompany { get; set; }
        public string FlightPlaner { get; set; }
        public DateTime Departure { get; set; }
        public string DepartureAirport { get; set; }
        public DateTime Arrival { get; set; }
        public string ArrivalAirport { get; set; }
        public string Class { get; set; }
        public string ClassAdd { get; set; }
        public string Baggage { get; set; }
        public string BaggageAdd { get; set; }
        public DateTime CheckIn { get; set; }
        public string CheckInAirport { get; set; }


        public override string ToString()
        {
            return  "RouteData{"+
                    "Flight=" + Flight +
                    ", FlightCompany=" + FlightCompany +
                    ", FlightPlaner=" + FlightPlaner +
                    ", Departure=" + Departure +
                    ", DepartureAirport=" + DepartureAirport +
                    ", Arrival=" + Arrival +
                    ", ArrivalAirport=" + ArrivalAirport +
                    ", Class=" + Class +
                    ", ClassAdd=" + ClassAdd +
                    ", Baggage=" + Baggage +
                    ", BaggageAdd=" + BaggageAdd +
                    ", CheckIn=" + CheckIn +
                    ", CheckInAirport=" + CheckInAirport +
                    "}";
        }
    }
}

```

2.4. Create a **Model/FareData.json** file:

```c#
namespace AirplaneTicket.Model
{
    public class FareData
    {
        public string Name { get; set; }
        public double Fare { get; set; }

        public override string ToString() 
        {
            return  "FareData{" +  
                    ", Name=" + Name +
                    ", Fare=" + Fare +
                     "}";
        }
    }
}
```

2.5. Create a **Model/HelpData.json** file:

```c#
namespace AirplaneTicket.Model
{
    public class HelpData
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString() 
        {
            return  "HelpData{" +  
                    ", Name=" + Name +
                    ", Value=" + Value +
                     "}";
        }
    }
}
```



## 3. Build a header.

3.1. Add private method `CheckFile` to class `AirplaneTicketRunner`. This method checks if JSON files are available every time the project is run:

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

3.2. Modify method `Run` to use data from JSON files in C# code:

```c#
        public static DocumentBuilder Run()
        {
            string ticketJsonFile = CheckFile(
                Path.Combine("Content", "ticket-data.json"));
            string routeJsonFile = CheckFile(
                Path.Combine("Content", "route-data.json"));
            string tripJsonFile = CheckFile(
                Path.Combine("Content", "about-trip.json"));
            string fareJsonFile = CheckFile(
                Path.Combine("Content", "fare-breakdown.json"));
            string helpJsonFile = CheckFile(
                Path.Combine("Content", "help-list.json"));
            string ticketJsonContent = File.ReadAllText(ticketJsonFile);
            string routeJsonContent = File.ReadAllText(routeJsonFile);
            string tripJsonContent = File.ReadAllText(tripJsonFile);
            string fareJsonContent = File.ReadAllText(fareJsonFile);
            string helpJsonContent = File.ReadAllText(helpJsonFile);
            TicketData ticketData = 
                JsonConvert.DeserializeObject<TicketData>(ticketJsonContent);
            List<RouteData> routeData = 
                JsonConvert.DeserializeObject<List<RouteData>>(routeJsonContent);
            List<string> tripData = 
                JsonConvert.DeserializeObject<List<string>>(tripJsonContent);
            List<FareData> fareData =
                JsonConvert.DeserializeObject<List<FareData>>(fareJsonContent);
            List<HelpData> helpData =
                JsonConvert.DeserializeObject<List<HelpData>>(helpJsonContent);
            AirplaneTicketBuilder airplaneTicketBuilder = 
                new AirplaneTicketBuilder();
            airplaneTicketBuilder.TicketData = ticketData;
            airplaneTicketBuilder.RouteData = routeData;
            airplaneTicketBuilder.TripData = tripData;
            airplaneTicketBuilder.FareData = fareData;
            airplaneTicketBuilder.HelpData = helpData;
            return airplaneTicketBuilder.Build();
        }
```

3.3. Insert additional namespaces to the top of the `AirplaneTicketRunner` class to resolve errors:

```c#
using AirplaneTicket.Model;
using Newtonsoft.Json;
```

3.4. Right-click on the `AirplaneTicket` project name in the Solution Explorer window and select **Add** > **Class**. Name new class as `AirplaneTicketBuilder`, add necessary namespaces to the class:

```c#
using AirplaneTicket.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System;
using Gehtsoft.PDFFlow.UserUtils;

namespace AirplaneTicket
{
    internal class AirplaneTicketBuilder
    {
    }
}
```

3.5. Add fields and constants that define properties of the document page and locale of the document content:

 ```c#
        internal static readonly CultureInfo DocumentLocale  
            = new CultureInfo("en-US");
        internal const PageOrientation Orientation 
            = PageOrientation.Portrait;
        internal static readonly Box Margins  = new Box(29, 20, 29, 20);
        internal static readonly XUnit PageWidth = 
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));
 ```

3.6. Create text font variables:

```c#
        internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7f);
        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9f);
        internal static readonly FontBuilder FNT9B_G = 
            Fonts.Helvetica(9f).SetBold().SetColor(Color.Gray);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11f);
        internal static readonly FontBuilder FNT11_B = 
            Fonts.Helvetica(11f).SetBold();
        internal static readonly FontBuilder FNT20 = Fonts.Helvetica(20f);
        internal static readonly FontBuilder FNT20B = 
            Fonts.Helvetica(20f).SetBold();
```

3.7. Add a field that contains names of headers of the "Route" section of the document:

```c#
        private static readonly string[] ROUTE_HEADS = {
                "FLIGHT", "DEPARTURE", "ARRIVAL", "CLASS", "BAGGAGE", "CHECK-IN"
            };
```

3.8. Define properties of class `AirplaneTicketBuilder`:

```c#
        public TicketData TicketData { get; internal set; }
        public List<RouteData> RouteData { get; internal set; }
        public List<string> TripData { get; internal set; }
        public List<FareData> FareData { get; internal set; }
        public List<HelpData> HelpData { get; internal set; }
```

3.9. Create internal method `Build` in this class:

```c#
        internal DocumentBuilder Build()
        {
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            var sectionBuilder = documentBuilder.AddSection();
            sectionBuilder
                .SetOrientation(Orientation)
                .SetMargins(Margins);
            
            sectionBuilder.AddHeaderToBothPages(80, BuildHeader);
            return documentBuilder;
        }
```

3.10. Create private method `BuildHeader`:

```c#
        private void BuildHeader(RepeatingAreaBuilder builder)
        {
            var tableBuilder = builder.AddTable();
            tableBuilder
                .SetWidth(XUnit.FromPercent(50))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 25).AddColumnPercent("", 75);
        }
```

3.11. Add the logo to the header and paste the following code snippet inside method `BuildHeader`:

```c#
            var rowBuilder = tableBuilder.AddRow();
            var cellBuilder = rowBuilder.AddCell();
            cellBuilder.AddImage(Path.Combine("images", "AT_Logo_2x.png"),
                XSize.FromHeight(80));
```

3.11. Add the flight company name:

```c#
            cellBuilder = rowBuilder.AddCell()
                .SetVerticalAlignment(VerticalAlignment.Center)
                .SetPadding(17, 0, 0, 0);
            cellBuilder
                .AddParagraph(TicketData.Company + " company").SetFont(FNT20B);
            cellBuilder
                .AddParagraph("E-ticket").SetFont(FNT20);
```

3.12. Run the project. The **AirplaneTicket.pdf** document has a header now:

![Fig. 2](../Articles%20Images/AirplaneTicket_2.png)



## 4. Build the "Ticket Information" section.

4.1. Create private method `GetTicketData` inside class `AirplaneTicketBuilder`:

```c#
        private string[,] GetTicketData()
        {
            return new string[,]
            {
                {"Passenger:", TicketData.Passenger},
                {"Document:", TicketData.Document},
                {"Ticket No:", TicketData.TicketNo},
                {"Order:", TicketData.Order},
                {"Issued:", TicketData.Issued.ToString(
                                "dd MMMM yyyy", DocumentLocale)},
                {"Status:", TicketData.Status}
            };
        }
```

4.2. Create private method `FillTicketInfoTable`, set up a table with four columns:

```c#
        private void FillTicketInfoTable(TableBuilder tableBuilder, string[,] ticketData)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .SetContentRowStyleFont(FNT10)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercent("", 25);
        }
```

4.3. Each row of the table has two fields of the ticket data, the formatting of each row is the same. So, in order to avoid repetition of the code, add `for` loop to method `FillTicketInfoTable`:

```c#
            for(int i = 0, len = (ticketData.Length >> 2), j = len + i; i < len; i++, j++)
            {
                var rowBuilder = tableBuilder.AddRow();
                var cellBuilder = rowBuilder.AddCell(ticketData[i, 0]);
                cellBuilder
                    .SetPadding(0, 3.5f, 0, 8.5f)
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(
                        Stroke.None, Stroke.None, Stroke.None, Stroke.Solid);
                cellBuilder = rowBuilder.AddCell(ticketData[i, 1]);
                cellBuilder
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(0, 3.5f, 0, 8.5f)
                    .SetBorderWidth(0, 0, 10, 0.5f)
                    .SetBorderColor(
                        Color.Black, Color.Black, Color.White, Color.Black)
                    .SetBorderStroke(
                        Stroke.None, Stroke.None, Stroke.Solid, Stroke.Solid);
                cellBuilder = rowBuilder.AddCell(ticketData[j, 0]);
                cellBuilder
                    .SetPadding(0, 3.5f, 0, 8.5f)
                    .SetBorderWidth(10, 0, 0, 0.5f)
                    .SetBorderColor(
                        Color.White, Color.Black, Color.Black, Color.Black)
                    .SetBorderStroke(
                        Stroke.Solid, Stroke.None, Stroke.None, Stroke.Solid);
                cellBuilder = rowBuilder.AddCell(ticketData[j, 1]);
                cellBuilder
                    .SetHorizontalAlignment(HorizontalAlignment.Right)
                    .SetPadding(0, 3.5f, 0, 8.5f)
                    .SetBorderWidth(0, 0, 0, 0.5f)
                    .SetBorderStroke(
                        Stroke.None, Stroke.None, Stroke.None, Stroke.Solid);
            }
```

4.4. Add the "Ticket Information" section to the document, paste the following code snippet before the `return documentBuilder;` line in method `Build`:

```c#
            FillTicketInfoTable(sectionBuilder.AddTable(), GetTicketData());
```

4.5. Run the project, the document should look like this:

![Fig. 3](../Articles%20Images/AirplaneTicket_3.png)



## 5. Build "Route" section.

5.1. Create private method `FirstRouteRow` inside class `AirplaneTicketBuilder`:

```c#
        private string[] FirstRouteRow(RouteData rd)
        {
            return new string[]
            {
                rd.Flight,
                rd.Departure.ToString("dd MMMM", DocumentLocale),
                rd.Arrival.ToString("dd MMMM", DocumentLocale),
                rd.Class,
                rd.Baggage,
                rd.CheckIn.ToString("HH:mm", DocumentLocale),
            };

        }
```

5.2. Create private method `SecondRouteRow` :

```c#
        private string[] SecondRouteRow(RouteData rd)
        {
            return new string[]
            {
                rd.FlightCompany + "\n" + rd.FlightPlaner,
                rd.Departure.ToString("HH:mm", DocumentLocale) + 
                    "\n" + rd.DepartureAirport,
                rd.Arrival.ToString("HH:mm", DocumentLocale) + 
                    "\n" + rd.ArrivalAirport,
                rd.ClassAdd,
                rd.BaggageAdd,
                rd.CheckInAirport,
            };
        }
```

5.3. Add method `BuildRouteInfo` to the class. This method inserts name of the "Route" section and adds a table with the route information to the section:

```c#
        private void BuildRouteInfo(SectionBuilder sectionBuilder)
        {
            sectionBuilder.AddParagraph("Route")
                .SetFont(FNT11_B).SetMarginTop(22);
            sectionBuilder.AddLine(PageWidth, 2f, Stroke.Solid);
            FillRouteInfoTable(sectionBuilder.AddTable());
        }
```

5.4. Define private method `FillRouteInfoTable` and set up the table with multiple columns:

```c#
        private void FillRouteInfoTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercentToTable("", 16)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercentToTable("", 17)
                .AddColumnPercent("", 16);
            }
        }
```

5.5. Define the style of rows of the table:

```c#
            var rowBuilder = tableBuilder.AddRow();
            rowBuilder
                .ApplyStyle(
                    StyleBuilder.New()
                        .SetFont(FNT9B_G)
                        .SetPaddingTop(4.8f)
                        .SetPaddingBottom(8.2f)
                );
```

5.6. Add header names of each column of the table:

```c#
            foreach (string headName in ROUTE_HEADS)  
            {
                rowBuilder.AddCell(headName);
            }
```

5.7. Fill the table with the route data:

```c#
            foreach(RouteData rd in RouteData)
            {
                rowBuilder = tableBuilder.AddRow();
                rowBuilder
                    .ApplyStyle(
                        StyleBuilder.New()
                            .SetFont(FNT10)
                            .SetPaddingTop(3.5f)
                            .SetPaddingBottom(7.5f)
                    );
                foreach (string cellValue in FirstRouteRow(rd))
                {
                    rowBuilder.AddCell(cellValue);
                }
                rowBuilder = tableBuilder.AddRow();
                rowBuilder
                    .ApplyStyle(
                        StyleBuilder.New()
                            .SetFont(FNT7)
                            .SetPaddingBottom(5.5f)
                            .SetBorderBottom(0.5f, Stroke.Solid, Color.Black)
                    );
                foreach (string cellValue in SecondRouteRow(rd))
                {
                    rowBuilder.AddCell(cellValue);
                }
            }
```

5.8. Add the "Route" section to the document, paste this line of the code before the `return documentBuilder;` line in method `Build`:

```c#
            BuildRouteInfo(sectionBuilder);
```

5.9. Run the project to see the result:

![Fig. 4](../Articles%20Images/AirplaneTicket_4.png)



## 6. Build "About your trip" section.

6.1. Create private method `BuildAboutTrip` in class `AirplaneTicketBuilder`. It sets the name of the "About your trip" section, adds the line under the name, and calls method `BuildAboutList`:

```c#
        private void BuildAboutTrip(SectionBuilder sectionBuilder)
        {
            sectionBuilder.AddParagraph("About your trip")
               .SetFont(FNT11_B).SetMarginTop(14);
            var lineBuilder = sectionBuilder.AddLine(PageWidth, 2f, Stroke.Solid);
            lineBuilder.SetMarginBottom(10);
            BuildAboutList(sectionBuilder);
        }
```

6.2. Create private method `BuildAboutList`. This method fills the section with information about the trip:

```c#
        private void BuildAboutList(SectionBuilder sectionBuilder)
        {
            foreach (String text in TripData)
            {
                var paragraphBuilder = sectionBuilder.AddParagraph();
                paragraphBuilder
                    .SetMarginLeft(8)
                    .SetFont(FNT9)
                    .SetListBulleted()
                    .AddTextToParagraph(
                        text.Replace("{company.name}", TicketData.Company));
            }
        }
```

6.3. Add the "About the trip" section to the document, paste the following line of the code before the `return documentBuilder;` line in method `Build`:

```c#
            BuildAboutTrip(sectionBuilder);
```

6.4. Run the project again. Now the output file contains new section:

![Fig. 5](../Articles%20Images/AirplaneTicket_5.png)



## 7. Build the "Fare breakdown" section.

7.1. Create private method `BuildFare` inside class`AirplaneTicket`:

```c#
        private void BuildFare(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph("Fare breakdown")
               .SetFont(FNT11_B).SetMarginBottom(2);
            cellBuilder.AddTable(FillFareTable);
        }
```

7.2. Define method `FillFareTable` and create a table with two columns:

```c#
        private void FillFareTable(TableBuilder tableBuilder)
        {
            tableBuilder
                .SetWidth(XUnit.FromPercent(95))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 60)
                .AddColumnPercent("", 40);
            var tableRowStyle = StyleBuilder.New()
                    .SetBorderTop(2, Stroke.Solid, Color.Black);
            TableRowBuilder tableRowBuilder;
            TableCellBuilder cellBuilder;
            double sum = 0;
        }
```

7.3. Fill the table with information about fares:

```c#
            foreach(FareData fd in FareData)
            {
                tableRowStyle
                    .SetBorderBottom(0.5f, Stroke.Solid, Color.Black)
                    .SetFont(FNT10)
                    .SetPaddingTop(3.5f)
                    .SetPaddingBottom(8.5f);
                tableRowBuilder = 
                    tableBuilder.AddRow().ApplyStyle(tableRowStyle);
                tableRowBuilder.AddCell(fd.Name);
                cellBuilder = tableRowBuilder.AddCell(String
                    .Format(CultureInfo.InvariantCulture, "{0:0,0.00}", fd.Fare) +
                    " USD");
                cellBuilder.SetHorizontalAlignment(HorizontalAlignment.Right);
                sum += fd.Fare;
                tableRowStyle = StyleBuilder.New();
            }
```

7.4. Add a footer row to the table with the total fare for the ticket:

```c#
            tableRowStyle
                .SetBorderBottom(0.5f, Stroke.Solid, Color.Black)
                .SetFont(FNT10)
                .SetPaddingTop(3.5f)
                .SetPaddingBottom(8.5f);
            tableRowBuilder =
                tableBuilder.AddRow().ApplyStyle(tableRowStyle);
            tableRowBuilder.AddCell("Total fare:");
            cellBuilder = tableRowBuilder.AddCell(String
                .Format(CultureInfo.InvariantCulture, "{0:0,0.00}", sum) +
                " USD");
            cellBuilder.SetHorizontalAlignment(HorizontalAlignment.Right);
```

7.5. Create private method `BuildFareAndHelp` inside the class. This method inserts a table with two cells. The first cell contains information about the ticket fare and the other cell contains the list of contacts:

```c#
        private void BuildFareAndHelp(SectionBuilder sectionBuilder)
        {
            var tableBuilder = sectionBuilder.AddTable();
            tableBuilder
                .SetMarginTop(30)
                .SetWidth(XUnit.FromPercent(100))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 51)
                .AddColumnPercent("", 49);
            var tableRowBuilder = tableBuilder.AddRow();
            tableRowBuilder.AddCellToRow(BuildFare);
            tableRowBuilder.AddCellToRow();            
        }
```

For now, the second cell is empty.

7.6. Add the''Fare breakdown" and "Need help" sections to the document, paste the following line of the code before the `return documentBuilder;` line in method `Build`:

```c#
            BuildFareAndHelp(sectionBuilder);
```

7.7. Run the project again. Information about the ticket fare is visible:

![Fig. 6](../Articles%20Images/AirplaneTicket_6.png)



## 8. Build the "Need help" section.

8.1. Add private method `BuildHelp` to class`AirplaneTicketBuilder`:

```c#
        private void BuildHelp(TableCellBuilder cellBuilder)
        {
            cellBuilder.AddParagraph("Need help?")
               .SetFont(FNT11_B).SetMarginBottom(2);
            cellBuilder.AddTable(FillHelpTable);
        }
```

8.2. Create private method `FillHelpTable` inside the class. It inserts the table with two columns and defines its styling:

```c#
        private void FillHelpTable(TableBuilder tableBuilder)
        {
            var endIndex = HelpData.Count - 1;
            if (endIndex < 0)
            {
                return;
            }
            tableBuilder
                .SetWidth(XUnit.FromPercent(95))
                .SetBorder(Stroke.None)
                .AddColumnPercentToTable("", 60)
                .AddColumnPercent("", 40);
            var tableRowStyle = StyleBuilder.New()
                    .SetBorderTop(2, Stroke.Solid, Color.Black);
        }
```

8.3. Fill this table with information about contacts:

```c#
            for (int i = 0; ; i++)
            {
                var hd = HelpData[i];
                tableRowStyle
                    .SetBorderBottom(0.5f, Stroke.Solid, Color.Black)
                    .SetFont(FNT10)
                    .SetPaddingTop(3.5f)
                    .SetPaddingBottom(8.5f);
                var tableRowBuilder =
                    tableBuilder.AddRow().ApplyStyle(tableRowStyle);
                tableRowBuilder
                    .AddCell(hd.Name.Replace("{company.name}", 
                        TicketData.Company));
                var cellBuilder = tableRowBuilder.AddCell(hd.Value);
                cellBuilder.SetHorizontalAlignment(HorizontalAlignment.Right);
                if (i == endIndex)
                {
                    break;
                }
                tableRowStyle = StyleBuilder.New();
           }
```

8.4. Remove the last line of the code in method `BuildFareAndHelp` and add a cell with the help data:

```c#
            tableRowBuilder.AddCellToRow(BuildHelp);
```

8.5. Run the project. The last section is added to the document:

![Fig. 7](../Articles%20Images/AirplaneTicket_7.png)



# Summary
The above example shows how to create a complex single page document that includes tables, nested tables, images, and lists.

The resulting **AirplaneTicket.pdf** document can be accessed [here](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/results/AirplaneTicket.pdf).

The example source is available in [repo](https://github.com/gehtsoft-usa/PDF.Flow.Examples/tree/master/Examples/AirplaneTicket).
