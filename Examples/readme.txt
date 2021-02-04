This sample set consists of prebuild release version of
Gehtsoft.PDFFlow library.

-------------
Prerequisites
-------------

1) Visual Studio 2017 or above is installed.

To install a community version of Visual Studio use
the following link:

https://visualstudio.microsoft.com/vs/community/

Please make sure that the way you are going to use Visual
Studio is allowed by the community license. You may need
to buy Standard or Professional Edition.

2) .NET Core Framework SDK 2.1 or above is installed.
To install the framework use the following link:

https://dotnet.microsoft.com/download

-------------------------------------
What is Gehtsoft.PDFFlow library for?
-------------------------------------

Gehtsoft.PDFFlow is made to simplify generation of the PDF
documents from .NET applications.

It is build on the base of Haru library (http://libharu.org/) and provides
the set of classes that allows the user to define a true document
(as a sequence of sections with their own layouts, paragraphs, images,
tables) and takes care on layouting the document content into PDF document.

The major benefit of this library is that the developer does not need
any special document publishing knowledge except the knowledge that any
qualified office user already has and the developer concentrates on
solving the business task (which exact information needs to be printed)
rather than fighting with complex repetitive drawing and layouting
"magic" operation that are involved into of hand-making of a PDF document.

The documentation for the library is available by the link below
http://docs.gehtsoftusa.com/pdfflow/web-content.html

-------
License
-------

The source code of the examples is
provided under Apache license
(https://en.wikipedia.org/wiki/Apache_License).

The Haru.Net package located in .nuget folder
is provided under zLib license
(https://github.com/libharu/libharu/blob/master/LICENCE)

Gehtsoft.PDFFlow package located in .nuget folder is provided
to build and evaluate examples ONLY. Using this package
in any application, even if such application is derived under
Apache license from the provided examples, is
strictly prohibited and violates conditions of use.

You must obtain Gehtsoft.PDFFlow package either under
free community license for open source projects or for
development purposes, or you must buy a commercial license in
order to use in a real business process, even if the product is
used for internal business processes or for marketing
purposes (e.g. for demonstration to customers).

Should you have any question about the licensing or prices please
feel free to contact Gehtsoft USA LLC via contact@gehtsoftusa.com

------------------------------------
 EXAMPLES
------------------------------------
PDFFlow Library comes with a number of examples. Each example is accompanied by a how-to article, 
which makes it a breeze to create your own documents fast. 

The resulting PDF documents are all located in the /results folder.

If you would like to submit some new ideas for documents you are working on or would find useful, 
please post a question on our support forum at http://www.fxcodebase.com/code/viewforum.php?f=56.

------------------------------------
The following examples are included:
------------------------------------

1) Afterschool Agreement

This example shows how to create a real agreement for an afterschool program with forms to fill out, 
images, and formatting options.

2) Airplane Ticket

Create a real-world airplane ticket in just a short time with this example.

3) Bank Account Statement

The real-world example shows how to create a complex document that includes tables, nested tables, and lists, 
as well as creating forms using tabulation. The example showcases a way to create a two-column page using "Left" 
and "Right" repeating areas. The project includes a how-to article as well.

4) Boarding Pass

This example accompanies the Airplane Ticket example. The example demonstrates a simple one-page document 
that includes a lot of small tables, images, and paragraphs. This example also demonstrates the usage of margins and lines. 
See the accompanying article for some tips and tricks on layouting and formatting.

5) Concert Ticket

Another useful example showing how to create a real concert or event ticket. This one showcases the rather 
complex layouting with tables and formatting options, so it's a good one to review for your own project 
or copy and use as-is right away. A how-to article is included in the project as well.

6) House Rental Contract

The example demonstrates the creation of a simple multi-page contractual text document. 
The document includes a repeating footer with automatic page numeration. 
The footer is made of two distinct sections with different content for the initials area on all pages of the contract,
except the page with full signatures. Additionally, this example demonstrates how to use Word-like tabulation and 
how to work with JSON as a data source. See the accompanying article for step-by-step instructions on creating this document.

7) Lab Test Results

The example demonstrates how to create a real medical laboratory test results report, which is a multi-page document 
that contains repeating headers, footers and images. This example also shows how to divide page into tables and paragraphs, 
parse document contents from json file and add tabulation to separate text parts on the same line.

8) LogBook with Complex Tables

The example demonstrates a multi-page document that consists of a complex table that spreads into a two-pages layout 
(i.e. half of the table columns is printed on the left page and half of the table columns is printed on the right page). 
Additionally, the example shows how to populate the table with data from a JSON source. 
The PDFFlow Library allows you to easily develop documents with a variety of complex tables without wasting time providing for complex layouts,
measuring, multi-page spread-tables, etc. Make sure to review the how-to article to understand better how the library allows you to work with tables.

9) Medical Bill

MedicalBill project is an example of creating a “Medical Bill” document. The example shows how to create a complex document that includes tables, 
nested tables, and lists. Also, it shows one of the ways to display forms for manually filling a printed document. 
The project includes a how-to article as well.

10) Payment Agreement

This example demonstrates how to create a multi-page contract document that includes a series of paragraphs. 
This example also demonstrates the usage of images, lines, and tabulation. There is also a how-to article accompanying the example.

11) Subscription Receipt

The example demonstrates the generation of a simple one-page subscription receipt document that includes a footer and a document flow area above it. 
This example also demonstrates the usage of an image, horizontal lines, a table with transparent borders, and clickable URLs/emails. 
The accompanying article goes into details on how to create this particular document, as well as similar documents.

12) Rental Agreement

The example shows how to code a typical legal rental agreement document with different sections. 
Each section has its own headers and footers, with pages having initials and clickable URL areas, 
as well as tracking scannable QR-code. Automatic pagination is also generated. 
Finally, the document has a complex move-in checklist with areas to fill in free text, as well as signature areas. 
The accompanying article has some good tips and tricks on layouting, formatting and general project and code organizing.

13) Resume

In this example, we generate a sample resume/CV. The example demonstrates a simple multi-page document that includes 
the right repeating area on both pages (even and odd) with the same content and document flow area on the left side of the page. 
This example also demonstrates the usage of the vertical line (between the document flow and the repeating area) and clickable URLs. 
See the article that comes with the example for best practices and some formatting tips.

14) Travel Insurance Claim Form

The example shows how to generate a typical Travel Insurance Claim Form document. 
The example demonstrates how to create a two-page document that contains many fill-in forms and images. 
This example also shows how to divide a page into tables, configure its titles and borders, add multiple paragraphs and checkboxes into cells. 
The accompanying article is a good place to start learning from this example.

15) Trade Confirmation

This example shows how to create a real brokerage trade confirmation. The example demonstrates a simple one-page document 
that with a table, image, and paragraphs with various alignments. This example also demonstrates the usage of margins and lines. 


16) Tutorial (Book)

In this example we will generate an actual programming tutorial book. The example demonstrates the creation of 
a multi-page text document with mixed content. The document includes several sections and a repeating footer with automatic page numeration. 
Additionally, this example shows how to use images and how to work with tables. Read the article that describes this example in detail.
