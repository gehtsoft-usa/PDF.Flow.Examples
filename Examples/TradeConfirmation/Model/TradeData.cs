using System.Collections.Generic;
namespace TradeConfirmationData.Model
{
    public class TradeData
    {
        public string CustomerName { get; set; }
        public List<string> CustomerAddress { get; set; }
        public string Account { get; set; }
        public string Bought { get; set; }
        public string Price { get; set; }
        public string PrincipalAmount { get; set; }
        public string AccruedInterest { get; set; }
        public string TransactionFee { get; set; }
        public string Total { get; set; }
        public string BankQualified { get; set; }
        public string State { get; set; }
        public string BankQualified2 { get; set; }
        public string DatedDate { get; set; }
        public string YieldtoMaturity { get; set; }
        public string YieldtoCall { get; set; }
        public string TaxExempt { get; set; }
        public string Capacity { get; set; }
        public string BondForm { get; set; }
        public string TradeDate { get; set; }
        public string TradeTime { get; set; }
        public string SettlementDate { get; set; }
        public string Order { get; set; }

        public override string ToString()
        {
            return "TradeData{" +
                    "CustomerName=" + CustomerName +
                    ", CustomerAddress: [" + CustomerAddress.ToString() + "]" +
                    ", Account=" + Account +
                    ", Bought=" + Bought +
                    ", Price=" + Price +
                    ", PrincipalAmount=" + PrincipalAmount +
                    ", AccruedInterest=" + AccruedInterest +
                    ", TransactionFee=" + TransactionFee +
                    ", Total=" + Total +
                    "BankQualified=" + BankQualified +
                    ", State=" + State +
                    ", BankQualified2=" + BankQualified2 +
                    ", DatedDate=" + DatedDate +
                    ", YieldtoMaturity=" + YieldtoMaturity +
                    ", YieldtoCall=" + YieldtoCall +
                    ", TaxExempt=" + TaxExempt +
                    "Capacity=" + Capacity +
                    ", BondForm=" + BondForm +
                    ", TradeDate=" + TradeDate +
                    ", TradeTime=" + TradeTime +
                    ", SettlementDate=" + SettlementDate +
                    ", Order=" + Order +
                     "}";
        }
    }
}


