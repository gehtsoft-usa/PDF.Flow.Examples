using TradeConfirmationData.Model;
using Gehtsoft.PDFFlow.Builder;
using Newtonsoft.Json;
using System.IO;

namespace TradeConfirmation
{
    public static class TradeConfirmationRunner
    {
        public static DocumentBuilder Run()
        {
            string firmJsonFile = CheckFile(Path.Combine("Content", "firm-data.json"));
            string firmJsonContent = File.ReadAllText(firmJsonFile);
            FirmData firmData =
               JsonConvert.DeserializeObject<FirmData>(firmJsonContent);

            string tradeJsonFile = CheckFile(Path.Combine("Content", "trade-data.json"));
            string tradeJsonContent = File.ReadAllText(tradeJsonFile);

            TradeData tradeData =
               JsonConvert.DeserializeObject<TradeData>(tradeJsonContent);

            TradeConfirmationBuilder TradeConfirmationBuilder =
                new TradeConfirmationBuilder();

            TradeConfirmationBuilder.FirmData = firmData;
            TradeConfirmationBuilder.TradeData = tradeData;

            return TradeConfirmationBuilder.Build();
        }
        private static string CheckFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new IOException("File not found: " + Path.GetFullPath(file));
            }
            return file;
        }
    }
}