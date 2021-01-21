using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using System;
using static MedicalBill.MedicalBillBuilder;

namespace MedicalBill
{
    internal class MedicalBillBackTextBuilder
    {
        private Params ps;

        public MedicalBillBackTextBuilder(Params ps)
        {
            this.ps = ps;
        }

        internal void Build(SectionBuilder sectionBuilder)
        {
            AddTexts(sectionBuilder);
        }

        private void AddTexts(SectionBuilder sectionBuilder)
        {
            AddFirstBlock(
                sectionBuilder, 
                String.Format("The {0} financial assistance policy\nplain language summary",
                ps.CenterName), 
                new StringWithUrl(
                    String.Format("{0} offers financial assistance to eligible patients who are uninsured, underinsured, and ineligible for a government health care program, or who are otherwise unable to pay for medically necessary care based on their individual financial situation.\nPatients seeking financial assistance must apply for the program, which is summarized below.",
                    ps.CenterName))
            );
            AddNextBlock(
                sectionBuilder, "Eligible Services", 
                new StringWithUrl("Eligible services include emergent or medically necessary services provided by the Hospital. Eligible patients include all patients who submit a financial assistance application (including requested documentation) and are determined to be eligible for financial assistance by the Patient Financial Services Department.")
            );
            AddListBlock(
                sectionBuilder, "How to Apply", 
                new StringWithUrl("Financial Assistance applications may be obtained/completed/submitted as follows:"), 
                new StringWithUrl[]
                {
                    new StringWithUrl(
                        String.Format("Obtain an application at The {0}'s Patient Financial Services Department located at {1}", 
                        ps.CenterName, ps.FinServAddress)), 
                        new StringWithUrl(String.Format("Request to have an application by mail at: {0}.", 
                        ps.CenterAddress.Replace("\n", ", ") )), 
                    new StringWithUrl(String.Format("Request to have an application mailed to you by calling {0}. Our hours of operation are: Monday-Friday, 8:30a.m.-4:30p.m.", 
                        ps.CenterPhone )), 
                    new StringWithUrl(
                        new Text[] { 
                            new Text("Download an application through the " + 
                            ps.CenterName + "'s website:\n "), 
                            new Link(ps.FinServUrl) })
                }, new StringWithUrl(
                    new Text[] {
                            new Text("Patient Financial Service Counselors are available Monday through Friday, 8:30 a.m. to 4:30 pm via telephone (123) 456-7890 to address questions related to the Financial Assistance Program.\nPlease feel free to email us at:")
                            , new Link("businessoffice@ourwebsite.com.", 
                            "mailto:businessoffice@ourwebsite.com.") })
            );
            AddNextBlock(
                sectionBuilder, 
                "Section 1557 — Notice of Nondiscrimination", 
                new StringWithUrl(
                    String.Format("The {0} complies with applicable Federal civil right laws and does not discriminate on the basis of race, color, national origin, age, disability, or sex.", 
                    ps.CenterName))
            );
        }

        private void AddFirstBlock(SectionBuilder sectionBuilder, 
            string title, StringWithUrl stringWithUrl)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder.SetMarginBottom(17).SetFont(FNT20B).AddText(title);
            paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder.SetFont(FNT11);
            foreach (Text text in stringWithUrl.Texts)
            {
                paragraphBuilder.AddText(text.Content);
            }
        }

        private void AddNextBlock(SectionBuilder sectionBuilder, 
            string title, StringWithUrl stringWithUrl)
        {
            AddBlockTitle(sectionBuilder, title);
            AddBlockText(sectionBuilder, stringWithUrl, FNT11);
        }

        private void AddBlockTitle(SectionBuilder sectionBuilder, string title)
        {
            var paragraphBuilder = sectionBuilder.AddParagraph();
            paragraphBuilder
                .SetMarginTop(12).SetMarginBottom(4).SetFont(FNT12B).AddText(title);
        }

        private void AddBlockText(SectionBuilder sectionBuilder, 
            StringWithUrl stringWithUrl, FontBuilder font, float topMargin = 0f)
        {
            var paragraphBuilder =
                sectionBuilder.AddParagraph().SetFont(font).SetMarginTop(topMargin);
            foreach (Text text in stringWithUrl.Texts)
            {
                paragraphBuilder
                    .AddText(text.Content);
            }
        }

        private void AddListBlock(SectionBuilder sectionBuilder, 
            string title, StringWithUrl stringWithUrlBegin, 
            StringWithUrl[] listWithUrls, StringWithUrl stringWithUrlEnd)
        {
            AddBlockTitle(sectionBuilder, title);
            AddBlockText(sectionBuilder, stringWithUrlBegin, FNT11);
            AddBlockList(sectionBuilder, listWithUrls);
            AddBlockText(sectionBuilder, stringWithUrlEnd, FNT11, 4f);
        }

        private void AddBlockList(SectionBuilder sectionBuilder, StringWithUrl[] listWithUrls)
        {
            foreach (StringWithUrl item in listWithUrls)
            {
                var paragraphBuilder = sectionBuilder.AddParagraph();
                paragraphBuilder
                    .SetMarginLeft(12).SetFont(FNT11).SetListBulleted(ListBullet.Bullet);
                foreach (Text text in item.Texts)
                {
                    AddTextOrUrl(paragraphBuilder, text);
                }
            }
        }

        private void AddTextOrUrl(ParagraphBuilder paragraphBuilder, Text text)
        {
            if (text.IsUrl())
            {
                AddLink(paragraphBuilder, (Link)text);
            }
            else
            {
                AddText(paragraphBuilder, text);
            }
        }

        private void AddLink(ParagraphBuilder paragraphBuilder, Link link)
        {
            paragraphBuilder
                .SetUrlStyle(
                    StyleBuilder.New()
                        .SetFontColor(Color.Blue)
                        .SetFontName("Helvetica")
                        .SetFontSize(11)
                        .SetFontUnderline(Stroke.Solid, Color.Blue))
                .AddUrlToParagraph(link.Href, link.Title);
        }

        private void AddText(ParagraphBuilder paragraphBuilder, Text text)
        {
            paragraphBuilder.AddTextToParagraph(text.Content);
        }
    }

    internal class StringWithUrl
    {
        private Text[] texts;

        public StringWithUrl(string v) : this(new Text[] { new Text(v) }) { }

        public StringWithUrl(Text[] vs)
        {
            this.texts = vs;
        }

        public Text[] Texts
        {
            get { return texts; }
        }
    }

    internal class Text
    {
        private string content;
        public String Content
        {
            get { return content; }
        }
        public Text(string content)
        {
            this.content = content;
        }
        public virtual bool IsUrl()
        {
            return false;
        }

        public override string ToString()
        {
            return content;
        }
    }

    internal class Link : Text
    {
        private string href;
        public String Title
        {
            get { return Content; }
        }
        public String Href
        {
            get { return href; }
        }
        public Link(string href) : base(href)
        {
            this.href = href;
        }
        public Link(string title, string href) : base(title)
        {
            this.href = href;
        }
        public override bool IsUrl()
        {
            return true;
        }
    }
}