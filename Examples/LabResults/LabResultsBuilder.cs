using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.UserUtils;
using Gehtsoft.PDFFlow.Utils;
using LabResults.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LabResults
{
    internal class LabResultsBuilder
    {
        internal const PageOrientation Orientation
            = PageOrientation.Portrait;
        internal static readonly Box Margins = new Box(29, 49, 29, 35);
        internal static readonly XUnit PageWidth = (PredefinedSizeBuilder.ToSize
            (PaperSize.Letter).Width - (Margins.Left + Margins.Right));

        internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7);
        internal static readonly FontBuilder FNT7Half = Fonts.Helvetica(7.5f);
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8);
        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9);
        internal static readonly FontBuilder FNT9B = Fonts.Helvetica(9).SetBold();
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10);
        internal static readonly FontBuilder FNT10B = Fonts.Helvetica(10).SetBold();
        internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11);

        internal static readonly Color blueGreen = Color.FromHtml("#1B7A98");

        List<TestDetailsData> testDetailsData =
            JsonConvert.DeserializeObject<List<TestDetailsData>>
            (File.ReadAllText(Path.Combine("Content", "test-details.json")));

        internal DocumentBuilder Build()
        {
            var documentBuilder = DocumentBuilder.New();

            foreach (var testData in testDetailsData)
            {
                var sectionBuilder = documentBuilder.AddSection();
                sectionBuilder
                    .SetOrientation(Orientation)
                    .SetMargins(Margins);

                new LabResultsHeaderBuilder().Build(sectionBuilder);
                new LabResultsTestDetailsBuilder().Build(sectionBuilder, testData);
                new LabResultsFooterBuilder().Build(sectionBuilder, testDetailsData.Count);
            }

            return documentBuilder;
        }
    }
}