using System.Collections.Generic;
using System.IO;
using PaymentAgreement.Model;
using Gehtsoft.PDFFlow.Builder;
using Newtonsoft.Json;

namespace PaymentAgreement
{
    public static class PaymentAgreementRunner
    {
        public static DocumentBuilder Run()
        {
            string agreementTextJsonFile = CheckFile(
                Path.Combine("Content", "pa-agreement-text.json"));
            string agreementJsonFile = CheckFile(
                Path.Combine("Content", "pa-agreement.json"));
            string partiesJsonFile = CheckFile(
                Path.Combine("Content", "pa-parties.json"));

            string agreementTextJsonContent = 
                File.ReadAllText(agreementTextJsonFile);
            string agreementJsonContent = 
                File.ReadAllText(agreementJsonFile);
            string partiesJsonContent =
                File.ReadAllText(partiesJsonFile);

            List<AgreementText> agreementText =
                JsonConvert.DeserializeObject<List<AgreementText>>(agreementTextJsonContent);
            AgreementData agreement =
                JsonConvert.DeserializeObject<AgreementData>(agreementJsonContent);
            List<PartyData> partyData = 
                JsonConvert.DeserializeObject<List<PartyData>>(partiesJsonContent);

            var paymentAgreementBuilder =
                new PaymentAgreementBuilder();

            paymentAgreementBuilder.AgreementText = agreementText;
            paymentAgreementBuilder.Agreement = agreement;
            paymentAgreementBuilder.PartyData = partyData;

            return paymentAgreementBuilder.Build();
        }

        private static string CheckFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new IOException("File not found: " + 
                    Path.GetFullPath(file));
            }
            return file;
        }
    }
}