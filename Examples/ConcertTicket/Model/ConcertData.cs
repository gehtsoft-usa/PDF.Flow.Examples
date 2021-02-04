using System.Collections.Generic;

namespace ConcertTicketData.Model
{
    public class ConcertData
    {
        public string TitleRulesOfPurchase { get; set; }
        public string TitleRulesOfAttendance { get; set; }
        public string RulesOfPurchase { get; set; }
        public List<string> RulesOfAttendance { get; set; }

        public string TitleBandsList { get; set; }
        public string BandsList { get; set; }

        public string TitleHowtoFind { get; set; }
        public string HowToFind { get; set; }
        public string TitleLearn { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string CounterFoil { get; set; }


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


