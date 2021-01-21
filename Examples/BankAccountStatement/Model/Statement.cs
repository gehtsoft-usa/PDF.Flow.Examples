using System;

namespace BankAccountStatement.Model
{
    public class Statement
    {
        public DateTime Date { get; set; }
        public string Check { get; set; }
        public string Details { get; set; }
        public double Deposits { get; set; }
        public double Withdrawals { get; set; }
        public double EndingDailyBalance { get; set; }

        public override string ToString()
        {
            return "Statement{Date=" + Date + 
                   ", Check=" + Check + 
                   ", Details=" + Details + 
                   ", Deposits=" + Deposits + 
                   ", Withdrawals=" + Withdrawals + 
                   ", EndingDailyBalance=" + EndingDailyBalance +
                   "}";
        }
    }
}
