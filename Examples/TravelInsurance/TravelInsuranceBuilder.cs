using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Utils;
using System.IO;

namespace TravelInsurance
{
    internal class TravelInsuranceBuilder
    {
        internal const PageOrientation Orientation = PageOrientation.Portrait;
        internal static readonly Box Margins = new Box(29, 21, 29, 0);

        internal static readonly string CheckboxPath = Path.Combine(
            Directory.GetCurrentDirectory(), "images", "TravelInsurance_Checkbox.png");
        internal static readonly string LogoPath = Path.Combine(Directory.GetCurrentDirectory(),
                                        "images", "TravelInsurance_Logo.png");

        internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7f);
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8f);
        internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11f);
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12f);
        internal static readonly FontBuilder FNT20B = Fonts.Helvetica(20f).SetBold();

        internal const string HEADER_BOX_TEXT =
            "Required documents – For all travel claims please submit air tickets and boarding " +
            "pass. For annual plans, please provide a copy of the passport showing duration of " +
            "trip. We reserve the right to request for additional information. To enable us to " +
            "process your claim expeditiously, please return the duly completed Claim Form with" +
            " supporting documents.\nPlease direct the claim form and all correspondence to:\n" +
            "Sample Company Travel Claims Unit \nc/o Sample Company Ltd, No. 5 Streetname #33-" +
            "01, Sample city 12345\n\nThe acceptance of this Form is NOT an admission of " +
            "liability on the part of Sample Company. Any documentary proof or report required " +
            "by the Company shall be furnished at the expense of the Policyholder or Claimant.\n";

        internal const string SECTIONF_LONG_TEXT =
            "I declare that to the best of my knowledge and belief that the above particulars " +
            "are true and accurate. If I made or shall make any false or fraudulent statements," +
            " or withhold material facts whatsoever in respect of this claim, the Policy shall " +
            "be void and I shall forfeit all rights to recover therein.\n\nI authorise any " +
            "hospital doctor, other person who has attended or examined me, to furnish to the " +
            "Company, and/ or its authorised representatives, any and all information relating " +
            "to any illness or injury, medical history, consultation, prescription or treatment," +
            " and copies of all hospital or medical records.  A photocopy of this authorisation " +
            "shall be considered as effective and valid as the original.";

        internal DocumentBuilder CreateDocumentBuilder()
        {
            //Create document builder:
            DocumentBuilder documentBuilder = DocumentBuilder.New();

            //Build Front page
            new TravelInsuranceFrontBuilder().Build(documentBuilder);

            //Build Back page
            new TravelInsuranceBackBuilder().Build(documentBuilder);

            return documentBuilder;
        }
    }
}