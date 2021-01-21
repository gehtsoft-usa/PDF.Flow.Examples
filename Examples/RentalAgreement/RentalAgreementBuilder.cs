using System.Collections.Generic;
using System.Globalization;
using RentalAgreement.Model;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;

namespace RentalAgreement
{
    internal class RentalAgreementBuilder
    {

        internal static readonly CultureInfo DocumentLocale
            = new CultureInfo("en-US");
        internal const PageOrientation Orientation
            = PageOrientation.Portrait;

        internal const float MAIN_TITLE_BOTTOM_MARGIN = 30;
        internal const float PARAGRAPH_TOP_MARGIN = 28;
        internal const float PARAGRAPH_ROW_DIST = 6;
        internal const float PARAGRAPH_BOTTOM_MARGIN = 16;
        internal const float SIGNATURES_TOP_MARGIN = 40;
        internal const float SIGNATURE_BOTTOM_MARGIN = 28;

        internal static readonly FontBuilder PAGE_NUMBER_FONT =
            Fonts.Times(9f);
        internal static readonly FontBuilder URL_FONT =
            Fonts.Helvetica(9f).SetBold().SetUnderline(
                Stroke.Solid, Color.Blue);
        internal static readonly FontBuilder FNT11 =
            Fonts.Times(11f);
        internal static readonly FontBuilder TEXT_FONT =
            Fonts.Times(12f);
        internal static readonly FontBuilder INITALS_FONT =
            Fonts.Helvetica(9f);
        internal static readonly FontBuilder HEADER_FONT =
            Fonts.Times(12f).SetBold();
        internal static readonly FontBuilder MAIN_TITLE_FONT =
            Fonts.Times(20f).SetBold();
        
        internal const string MAIN_TITLE = 
            "LEASE WITH OPTION TO PURCHASE";

        private List<PartyData> partyData;
        private PartyData landlord = new PartyData();
        private PartyData manager = new PartyData();
        private List<PartyData> tenant = new List<PartyData>();
        private List<PartyData> tenantAdd = new List<PartyData>();

        public List<AgreementText> AgreementText { get; internal set; }
        public AgreementData Agreement { get; internal set; }
        public List<CheckList> CheckList { get; internal set; }

        public List<PartyData> PartyData
        {
            get { return partyData; }
            internal set
            {
                partyData = value;
                SyncPartyData();
            }
        }

        internal DocumentBuilder Build()
        {            
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            var rentalAgreementTextBuilder = new RentalAgreementTextBuilder();
            rentalAgreementTextBuilder
                .SetLandord(landlord)
                .SetManager(manager)
                .SetTenant(tenant)
                .SetTenantAdd(tenantAdd)
                .SetAgreement(Agreement)
                .SetAgreementText(AgreementText)
                .Build(documentBuilder);
            var rentalAgreementDepositBuilder = 
                new RentalAgreementDepositBuilder();
            rentalAgreementDepositBuilder.Build(documentBuilder);
            var rentalAgreementAmountBuilder = 
                new RentalAgreementAmountBuilder();
            rentalAgreementAmountBuilder
                .SetAgreement(Agreement)
                .Build(documentBuilder);
            var rentalAgreementCheckListBuilder = 
                new RentalAgreementCheckListBuilder();
            rentalAgreementCheckListBuilder
                .SetAgreement(Agreement)
                .SetCheckList(CheckList)
                .Build(documentBuilder);
            return documentBuilder;
        }


        private void SyncPartyData()
        {
            tenant.Clear();
            tenantAdd.Clear();
            foreach (PartyData party in partyData)
            {
                switch (party.Party)
                {
                    case "Landlord":
                        landlord = party;
                        break;
                    case "Manager":
                        manager = party;
                        break;
                    case "Tenant":
                        tenant.Add(party);
                        break;
                    case "TenantAdd":
                        tenantAdd.Add(party);
                        break;
                }
            }
        }

    }
}