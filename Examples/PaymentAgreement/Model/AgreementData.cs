using System;

namespace PaymentAgreement.Model
{
    public class AgreementData
    {

        public DateTime Date { get; set; }
        public double Balance { get; set; }
        public double DiscountedBalance { get; set; }
        public double DownPayment { get; set; }
        public double InterestRate { get; set; }
        public DateTime RepaymentPeriodBegins { get; set; }
        public DateTime RepaymentPeriodEnds { get; set; }
        public string RepaymentFrequency { get; set; }
        public int ExtensionPeriodOne { get; set; }
        public int ExtensionPeriod { get; set; }
        public string BrokenPayPeriod { get; set; }
        public string GoverningLawState { get; set; }
        public string GoverningLaw { get; set; }

        public override string ToString() 
        {
            return  "AgreementData{" +  
                    "Date=" + Date +
                    ", Balance=" + Balance +
                    ", DiscountedBalance=" + DiscountedBalance +
                    ", DownPayment=" + DownPayment +
                    ", InterestRate=" + InterestRate +
                    ", RepaymentPeriodBegins=" + RepaymentPeriodBegins +
                    ", RepaymentPeriodEnds=" + RepaymentPeriodEnds +
                    ", RepaymentFrequency=" + RepaymentFrequency +
                    ", ExtensionPeriodOne=" + ExtensionPeriodOne +
                    ", ExtensionPeriod=" + ExtensionPeriod +
                    ", BrokenPayPeriod=" + BrokenPayPeriod +
                    ", GoverningLawState=" + GoverningLawState +
                    ", GoverningLaw=" + GoverningLaw +
                    "}";
        }
    }
}