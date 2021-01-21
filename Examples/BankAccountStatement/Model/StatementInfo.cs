using System;
using System.Globalization;

namespace BankAccountStatement.Model
{
    public class StatementInfo
    {
        public string BankName { get; set; }
        public string BankNameState { get; set; }
        public string AccountNumber { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneFree { get; set; }
        public string Phone { get; set; }
        public string TTY { get; set; }
        public string Online { get; set; }
        public string White { get; set; }
        public string ReportAddress { get; set; }
        public string BusinessPlan { get; set; }
        public string AccountOptions { get; set; }
        public string Advt { get; set; }
        public double BeginningBalance { get; set; }
        public double Withdrawals { get; set; }
        public double Deposits { get; set; }
        public double EndingBalance { get; set; }
        public double AverageBalance { get; set; }
        public string DepositRTN { get; set; }
        public string WireRTN { get; set; }
        public double StandartServiceFee { get; set; }
        public double MinimumRequired { get; set; }
        public double ServiceFee { get; set; }
        public double ServiceDiscount { get; set; }
        public int TransactionUnits { get; set; }
        public int TransactionUnitsIncluded { get; set; }
        public int TransactionExcessUnits { get; set; }
        public double ServiceCharge { get; set; }
        public double TotalServiceCharge { get; set; }
        public string FeedBackPhone { get; set; }

        public static string ToString(double value)
        {
            return String
                .Format(CultureInfo.InvariantCulture, "{0:0,0.00}", value);
        }

        public override string ToString() 
        {
            return "StatementInfo{BankName=" + BankName +
               ", BankNameState=" + BankNameState +
               ", AccountNumber=" + AccountNumber +
               ", DateBegin=" + DateBegin +
               ", DateEnd=" + DateEnd +
               ", CompanyName=" + CompanyName +
               ", CompanyAddress=" + CompanyAddress +
               ", PhoneFree=" + PhoneFree +
               ", Phone=" + Phone +
               ", TTY=" + TTY +
               ", Online=" + Online +
               ", White=" + White +
               ", ReportAddress=" + ReportAddress +
               ", BusinessPlan=" + BusinessPlan +
               ", AccountOptions=" + AccountOptions +
               ", Advt=" + Advt +
               ", BeginningBalance=" + BeginningBalance +
               ", Credits=" + Withdrawals +
               ", Debits=" + Deposits +
               ", EndingBalance=" + EndingBalance +
               ", AverageBalance=" + AverageBalance +
               ", DepositRTN=" + DepositRTN +
               ", WireRTN=" + WireRTN +
               ", StandartServiceFee=" + StandartServiceFee +
               ", MinimumRequired=" + MinimumRequired +
               ", ServiceFee=" + ServiceFee +
               ", ServiceDiscount=" + ServiceDiscount +
               ", TransactionUnits=" + TransactionUnits +
               ", TransactionUnitsIncluded=" + TransactionUnitsIncluded +
               ", TransactionExcessUnits=" + TransactionExcessUnits +
               ", ServiceCharge=" + ServiceCharge +
               ", TotalServiceCharge=" + TotalServiceCharge +
               ", FeedBackPhone=" + FeedBackPhone +
                "}";
        }
    }
}