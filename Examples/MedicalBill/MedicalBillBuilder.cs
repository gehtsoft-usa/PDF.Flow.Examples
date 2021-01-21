using System;
using System.Collections.Generic;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Utils;
using Gehtsoft.PDFFlow.UserUtils;

namespace MedicalBill
{
    internal class MedicalBillBuilder
    {
        internal const PageOrientation Orientation = PageOrientation.Portrait;
        internal static readonly Box Margins = new Box(29, 20, 29, 20);
        internal static readonly XUnit PageWidth =
            (PredefinedSizeBuilder.ToSize(PaperSize.Letter).Width -
                (Margins.Left + Margins.Right));

        internal static readonly Color BLUE_COLOR = new Color(0x01, 0x61, 0xAB);
        internal static readonly string[] CARD_TEXTS1 = 
            { "Card number", "Exp.date", "Amount" };
        internal static readonly string[] CARD_TEXTS2 = { "Signature", "SVV" };
        internal static readonly string[] CARD_TEXTS3 = 
            { "Statement Date", "Guarantor number", "Pay the amount"};
        internal static readonly string[] CARD_TEXTS4 = 
            { "Visit #to apply payment", "Show amount paid here" };
        internal static readonly string[] CLIENT_NAME_TEXT = 
            { "Your name (Last, First, Middle initial)" };
        internal static readonly string[] CLIENT_ADDRESS_TEXT = { "Address" };
        internal static readonly string[] ADDRESS_FIELDS_TEXT = { "City", "State", "Zip" };
        internal static readonly string[] CLIENT_PHONE_TEXT = { "Telephone" };
        internal static readonly string MARITAL_STATUS_TEXT = "Marital status";
        internal static readonly string[] MARITAL_STATUS_VALUES_TEXT = 
            { "Single", "Married", "Separat.", "Divorced", "Widowed" };
        internal static readonly string[] EMPL_PHONE_TEXT = 
            { "Employer’s name", "Telephone" };
        internal static readonly string[] EMPL_ADDRESS_TEXT = { "Employer’s address" };
        internal static readonly string[] ENS_PR_NAME_TEXT = 
            { "Your primary insurance company’s name", "Effective date" };
        internal static readonly string[] ENS_PR_ADDRESS_TEXT = 
            { "Primary insurance company’s address", "Phone" };
        internal static readonly string[] ENS_POLICY_TEXT = 
            { "Policyholder’s ID number", "Group plan number" };
        internal static readonly string[] ENS_RELATION_TEXT = 
            { "Relationship to patient" };
        internal static readonly string[] ENS_SC_NAME_TEXT = 
            { "Your secondary insurance company’s name", "Effective date" };
        internal static readonly string[] ENS_SC_ADDRESS_TEXT = 
            { "Secondary insurance company’s address", "Phone" };

        internal static readonly FontBuilder FNT7 = Fonts.Helvetica(7f);
        internal static readonly FontBuilder FNT7B = Fonts.Helvetica(7f).SetBold();
        internal static readonly FontBuilder FNT8B = Fonts.Helvetica(8f).SetBold();
        internal static readonly FontBuilder FNT9 = Fonts.Helvetica(9f);
        internal static readonly FontBuilder FNT9B = Fonts.Helvetica(9f).SetBold();
        internal static readonly FontBuilder FNT9B_B = 
            Fonts.Helvetica(9f).SetBold().SetColor(BLUE_COLOR);
        internal static readonly FontBuilder FNT9B_W = 
            Fonts.Helvetica(9f).SetBold().SetColor(Color.White);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT10B = Fonts.Helvetica(10f).SetBold();
        internal static readonly FontBuilder FNT11 = Fonts.Helvetica(11f);
        internal static readonly FontBuilder FNT12 = Fonts.Helvetica(12f);
        internal static readonly FontBuilder FNT12B = Fonts.Helvetica(12f).SetBold();
        internal static readonly FontBuilder FNT20B = Fonts.Helvetica(20f).SetBold();
        internal static readonly FontBuilder FNTZ12 = 
            FontBuilder.New().SetName(FontNames.ZapfDingbats).SetSize(12);
        internal static readonly FontBuilder FNTZ16 = 
            FontBuilder.New().SetName(FontNames.ZapfDingbats).SetSize(16);

        private readonly string imageDir;
        private readonly List<RowData> rowsData;
        private readonly List<string> cardNames;
        private Params parameters = new Params();

        public string GuarantorName
        {
            get { return parameters.GuarantorName; }
            set { parameters.GuarantorName = value; }
        }

        public string GuarantorNumber { 
            get { return parameters.GuarantorNumber; }
            set { parameters.GuarantorNumber = value; }
        }

        public string StatementDate
        {
            get { return parameters.StatementDate; }
            set { parameters.StatementDate = value; }
        }

        public string CenterName
        {
            get { return parameters.CenterName; }
            set { parameters.CenterName = value; }
        }

        public string CenterAddress
        {
            get { return parameters.CenterAddress; }
            set { parameters.CenterAddress = value; }
        }

        public string CenterPhone
        {
            get { return parameters.CenterPhone; }
            set { parameters.CenterPhone = value; }
        }

        public string CenterPatent
        {
            get { return parameters.CenterPatent; }
            set { parameters.CenterPatent = value; }
        }

        public string FinServAddress
        {
            get { return parameters.FinServAddress; }
            set { parameters.FinServAddress = value; }
        }

        public string FinServUrl
        {
            get { return parameters.FinServUrl; }
            set { parameters.FinServUrl = value; }
        }

        public MedicalBillBuilder(string imageDir)
        {
            this.imageDir = imageDir;
            this.rowsData = new List<RowData>();
            this.cardNames = new List<string>();
            //Default values 
            this.GuarantorName = "Sample Guarantor";
            this.GuarantorNumber = "2nnnnn";
            this.StatementDate = "07/10/2020";
            this.CenterName = "Sample Medical Center";
            this.CenterAddress = "123 Main Street\nAnywhere, NY 12345 - 6789";
            this.CenterPhone = "123 - 456 - 7890";
            this.CenterPatent = "Sample Patent";
            this.FinServAddress = "Main Street 123";
            this.FinServUrl = "http://www.ourwebsite.com/PatientFinancialServices.aspx";
        }

        public MedicalBillBuilder AddRowData(String date, String description, 
            String  charges = "", String payment = "",  String balance = "")
        {
            RowData rowData = new RowData
            {
                Date = date,
                Description = description,
                Charges = charges,
                Payment = payment,
                Balance = balance
            };
            rowsData.Add(rowData);
            return this;
        }

        public MedicalBillBuilder AddCardName(String cardName)
        {
            cardNames.Add(cardName);
            return this;
        }

        public DocumentBuilder CreateDocumentBuilder()
        {
            //Create document builder:
            DocumentBuilder documentBuilder = DocumentBuilder.New();
            //Create MedicalBillFrontBuilder
            parameters.ImageDir = imageDir;
            parameters.CardNames = cardNames;
            parameters.RowsData = rowsData;
            parameters.Balance = GetBalance();

            //Build Front page
            new MedicalBillFrontBuilder(parameters)
                .Build(documentBuilder);
            //Build Back page
            new MedicalBillBackBuilder(parameters)
                .Build(documentBuilder);
            return documentBuilder;
        }

        private string GetBalance()
        {
            return rowsData.Count > 0 ? "$" + rowsData[rowsData.Count - 1].Balance : "";
        }

    }

    internal class Params
    {
        public List<RowData> RowsData { get; internal set; }
        public List<string> CardNames { get; internal set; }
        public string ImageDir { get; internal set; }
        public string GuarantorNumber { get; internal set; }
        public string StatementDate { get; internal set; }
        public string CenterName { get; internal set; }
        public string CenterAddress { get; internal set; }
        public string CenterPhone { get; internal set; }
        public string CenterPatent { get; internal set; }
        public string GuarantorName { get; internal set; }
        public string Balance { get; internal set; }
        public string FinServAddress { get; internal set; }
        public string FinServUrl { get; internal set; }
    }

    internal class RowData
    {
        public string Date { get; internal set; }
        public string Description { get; internal set; }
        public string Charges { get; internal set; }
        public string Payment { get; internal set; }
        public string Balance { get; internal set; }
    }
}