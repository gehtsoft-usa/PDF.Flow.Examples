using System.Collections.Generic;

namespace ConcertTicketData.Model
{
    public class ConcertData
    {
        public string TitleRulesOfPurchase { get; set; } = null!;
        public string TitleRulesOfAttendance { get; set; } = null!;
        public string RulesOfPurchase { get; set; } = null!;
        public List<string> RulesOfAttendance { get; set; } = null!;

        public string TitleBandsList { get; set; } = null!;
        public string BandsList { get; set; } = null!;

        public string TitleHowtoFind { get; set; } = null!;
        public string HowToFind { get; set; } = null!;
        public string TitleLearn { get; set; } = null!;
        public string Facebook { get; set; } = null!;
        public string Twitter { get; set; } = null!;
        public string Instagram { get; set; } = null!;
        public string CounterFoil { get; set; } = null!;


        public override string ToString()
        {
            return "{" + ", titlerulesofpurchase=" + TitleRulesOfPurchase +
                    ", titlerulesofattendance=" + TitleRulesOfAttendance +
                    ", rulesofpurchase=" + RulesOfPurchase +
                    ", rulesofattendance: [" + RulesOfAttendance.ToString() +  "]" +
                     "}";
        }

    }


}


