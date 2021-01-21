using System.IO;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;

namespace Resume
{
    internal static class ResumeBuilder
    {
        private static readonly string BillGatesPath = Path.Combine(Directory.GetCurrentDirectory(), "Content", "Images", "Bill_Gates.jpg");
        
        public static DocumentBuilder AddResume(this DocumentBuilder builder)
        {
            var section = builder.AddSection().SetOrientation(PageOrientation.Portrait).SetMargins(20f, 30f);
            
            section
                .AddRightArea()
                .AddSeparationArea()
                .AddMainDocumentFlow();
            
            return builder;
        }

        private static SectionBuilder AddRightArea(this SectionBuilder s)
        {
            RepeatingAreaBuilder rightAreaBuilder = s.AddRptAreaRightToBothPages(200f);
            rightAreaBuilder
                .AddLine()
                .SetWidth(2f);
            rightAreaBuilder
                .AddParagraph()
                .SetMarginTop(10f)
                .AddText(" Contact")
                .SetFontSize(16f);
            rightAreaBuilder
                .AddParagraph()
                .SetMarginTop(7f)
                .SetFontSize(14f)
                .AddTextToParagraph("+1-202-555-0163 ")
                .AddText("(Mobile)")
                .SetFontColor(Color.Gray);
            rightAreaBuilder
                .AddParagraph()
                .SetFontSize(14f)
                .AddTextToParagraph("email@example.com");
            rightAreaBuilder
                .AddParagraph()
                .SetMarginTop(10f)
                .SetFontSize(14f)
                .SetTextOverflowAction(TextOverflowAction.Ellipsis)
                .AddUrlToParagraph("https://www.linkedin.com/in/williamhgates", "www.linkedin.com/in/williamhgates")
                .AddText(" (LinkedIn)")
                .SetFontColor(Color.Gray);
            rightAreaBuilder
                .AddParagraph()
                .SetFontSize(14f)
                .SetTextOverflowAction(TextOverflowAction.Ellipsis)
                .AddUrlToParagraph("https://www.gatesnotes.com/", "www.gatesnotes.com")
                .AddText(" (Site)")
                .SetFontColor(Color.Gray);
            return s;
        }
        
        internal static SectionBuilder AddSeparationArea(this SectionBuilder s)
        {
            s.AddRptAreaRightToBothPages(20f)
                .AddLine(length: 0.5f, width: s.PageSize.Height - s.Margins.Vertical)
                .SetMarginLeft(9f)
                .SetColor(Color.Gray);
            return s;
        }
        
        internal static SectionBuilder AddMainDocumentFlow(this SectionBuilder s)
        {
            s.AddLine()
                .SetWidth(2f);
            s.AddImage(BillGatesPath, ScalingMode.OriginalSize)
                .SetMarginLeft(1.5f)
                .SetMarginTop(7f);
            s.AddParagraph("Bill Gates").SetMarginTop(6.5f).SetFontSize(28f);
            s.AddParagraph("Co-chair, Bill & Melinda Gates Foundation").SetFontSize(14f).SetMarginTop(5f);
            s.AddParagraph("Seattle").SetFontSize(14f).SetFontColor(Color.Gray);

            s.AddParagraph("Summary").SetFontSize(18f).SetMarginTop(20f);
            s.AddParagraph(
                    "Co-chair of the Bill & Melinda Gates Foundation. Microsoft Co-founder. Voracious reader. Avid traveler. Active blogger.")
                .SetFontSize(14f)
                .SetMarginTop(10f);

            s.AddLine(40f).SetColor(Color.Gray).SetMarginTop(20f);

            s.AddParagraph("Experience").SetFontSize(18f).SetMarginTop(10f);
            s.AddParagraph("Bill & Melinda Gates Foundation").SetFontSize(14f).SetMarginTop(15f);
            s.AddParagraph("Co-chair").SetFontSize(12f);
            s.AddParagraph("2000 - Present (20 years)").SetFontSize(12f);
            s.AddParagraph("Microsoft").SetFontSize(14f).SetMarginTop(20f);
            s.AddParagraph("Co-founder").SetFontSize(12f);
            s.AddParagraph("1975 - Present (45 years)").SetFontSize(12f);

            s.AddLine(40f).SetColor(Color.Gray).SetMarginTop(20f);

            s.AddParagraph("Education").SetFontSize(18f).SetMarginTop(10f);
            s.AddParagraph("Harvard University").SetFontSize(14f).SetMarginTop(15f);
            s.AddParagraph(" · (1973 - 1975)").SetFontSize(12f);
            s.AddParagraph("Lakeside School, Seattle").SetFontSize(14f).SetMarginTop(20f);
            
            return s;
        }
    }
}