using System.Collections.Generic;
using System.IO;
using RentalAgreement.Model;
using Gehtsoft.PDFFlow.Builder;
using Newtonsoft.Json;

namespace RentalAgreement
{
    public static class RentalAgreementRunner
    {
        public static DocumentBuilder Run()
        {
            string agreementTextJsonFile = CheckFile(
                Path.Combine("Content", "ra-agreement-text.json"));
            string agreementJsonFile = CheckFile(
                Path.Combine("Content", "ra-agreement.json"));
            string partiesJsonFile = CheckFile(
                Path.Combine("Content", "ra-parties.json"));
            string checklistJsonFile = CheckFile(
                Path.Combine("Content", "ra-checklist.json"));

            string agreementTextJsonContent = 
                File.ReadAllText(agreementTextJsonFile);
            string agreementJsonContent = 
                File.ReadAllText(agreementJsonFile);
            string partiesJsonContent =
                File.ReadAllText(partiesJsonFile);
            string checklistJsonContent =
                File.ReadAllText(checklistJsonFile);

            List<AgreementText> agreementText =
                JsonConvert.DeserializeObject<List<AgreementText>>(
                    agreementTextJsonContent);
            AgreementData agreement =
                JsonConvert.DeserializeObject<AgreementData>(
                    agreementJsonContent);
            List<PartyData> partyData = 
                JsonConvert.DeserializeObject<List<PartyData>>(
                    partiesJsonContent);
            List<CheckList> checkList = 
                JsonConvert.DeserializeObject<List<CheckList>>(
                    checklistJsonContent);

            var rentalAgreementBuilder =
                new RentalAgreementBuilder();

            rentalAgreementBuilder.AgreementText = agreementText;
            rentalAgreementBuilder.Agreement = agreement;
            rentalAgreementBuilder.PartyData = partyData;
            rentalAgreementBuilder.CheckList = checkList;

            return rentalAgreementBuilder.Build();
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