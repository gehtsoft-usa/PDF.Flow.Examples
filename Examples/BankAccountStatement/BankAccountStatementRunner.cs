using BankAccountStatement.Model;
using Gehtsoft.PDFFlow.Builder;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace BankAccountStatement
{
    public static class BankAccountStatementRunner
    {
        public static DocumentBuilder Run()
        {
            string statementJsonFile = CheckFile(
                Path.Combine("Content", "sample-statement-data.json"));
            string infoJsonFile = CheckFile(
                Path.Combine("Content", "sample-statement-info-data.json"));
            string statementJsonContent = File.ReadAllText(statementJsonFile);
            string infoJsonContent = File.ReadAllText(infoJsonFile);
            StatementInfo statementInfo = JsonConvert.DeserializeObject<StatementInfo>(
                infoJsonContent);
            List<Statement> statements = JsonConvert.DeserializeObject<List<Statement>>(
                statementJsonContent);
            BankAccountStatementBuilder bankAccountStatementBuilder = 
                new BankAccountStatementBuilder(statementInfo, statements);
            return bankAccountStatementBuilder.Build();
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