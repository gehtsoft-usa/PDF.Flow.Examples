using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BankAccountStatement.Model
{
    class DayStatementCorrector
    {
        static int CorrectStatements(string statementsFile, string infoFile, string newFileExt)
        {
            string statementJsonFile = CheckFile(statementsFile);
            string infoJsonFile = CheckFile(infoFile);
            string statementJsonContent = File.ReadAllText(statementJsonFile);
            string infoJsonContent = File.ReadAllText(infoJsonFile);
            List<Statement> statements = JsonConvert.DeserializeObject<List<Statement>>(statementJsonContent);
            StatementInfo statementInfo = JsonConvert.DeserializeObject<StatementInfo>(infoJsonContent);
            if (statements.Count == 0)
            {
                Console.Error.WriteLine("Statements data is empty in file " + statementJsonFile);
                return 1;
            }

            if (!Correct(statementInfo, statements))
            {
                string newInfoJsonContent = JsonConvert.SerializeObject(statementInfo);
                string newInfoJsonFile = infoJsonFile + newFileExt;
                File.WriteAllText(newInfoJsonFile, newInfoJsonContent);

                string newStatementJsonContent = JsonConvert.SerializeObject(statements);
                string newStatementJsonFile = statementJsonFile + newFileExt;
                File.WriteAllText(newStatementJsonFile, newStatementJsonContent);
                Console.WriteLine("Files " + Path.GetFullPath(statementJsonFile) + ", " + Path.GetFullPath(infoJsonFile)  + " are incorrect");
                Console.WriteLine("Changed files: " + Path.GetFullPath(newStatementJsonFile) + ", " + Path.GetFullPath(newInfoJsonFile));
            }
            else
            {
                Console.WriteLine("Files " + Path.GetFullPath(statementJsonFile) + ", " + Path.GetFullPath(infoJsonFile) + " are correct");
            }

            return 0;
        }

        private static bool Correct(StatementInfo statementInfo, List<Statement> statements)
        {
            bool valid = !Sort(statements);
            double currentBalance = statementInfo.BeginningBalance;
            double minNegaticeBalance = 0.0;
            int i = 0;
            DateTime beginDate = DateTime.MinValue;
            DateTime currentDate = DateTime.MinValue;
            double sumDayBalance = 0;
            foreach (Statement statement in statements)
            {
                if (i++ == 0)
                {
                    beginDate = statement.Date;
                }
                else
                {
                    TimeSpan sub1 = statement.Date - currentDate;
                    sumDayBalance += currentBalance * sub1.Days;
                }
                currentDate = statement.Date;
                currentBalance = Math.Round(currentBalance + statement.Deposits - statement.Withdrawals, 2, MidpointRounding.ToEven);
                if (statement.EndingDailyBalance != currentBalance)
                {
                    statement.EndingDailyBalance = currentBalance;
                    valid = false;
                }
                if (currentBalance < minNegaticeBalance)
                {
                    Console.Error.WriteLine("Balance at " + statement.Date + " is negative: " + currentBalance);
                    minNegaticeBalance = currentBalance;
                }
            }
            sumDayBalance += currentBalance;
            TimeSpan sub = currentDate - beginDate;
            double averageBalace = Math.Round(sumDayBalance /(sub.Days + 1), 2, MidpointRounding.ToEven);
            if (statementInfo.EndingBalance != currentBalance)
            {
                statementInfo.EndingBalance = currentBalance;
                valid = false;
            }
            if (statementInfo.AverageBalance != averageBalace)
            {
                statementInfo.AverageBalance = averageBalace;
                valid = false;
            }
            if (statementInfo.DateBegin > beginDate)
            {
                statementInfo.DateBegin = beginDate;
                valid = false;
            }
            if (statementInfo.DateEnd < currentDate)
            {
                statementInfo.DateEnd = currentDate;
                valid = false;
            }
            return valid;
        }

        private static bool Sort(List<Statement> statements)
        {
            if (NeedSort(statements))
            {
                DoSort(statements);
                return true;
            }
            return false;
        }

        private static void DoSort(List<Statement> statements)
        {
            statements.Sort((statement1, statement2) => 
            { 
                if (statement1.Date == null)
                {
                    return statement2.Date == null ? 0 : -1;
                }
                if (statement2.Date == null)
                {
                    return 1;
                }
                if (statement1.Date > statement1.Date)
                {
                    return 1;
                }
                if (statement2.Date > statement1.Date)
                {
                    return -1;
                }
                return 0;
            });
        }

        private static bool NeedSort(List<Statement> statements)
        { 
            DateTime current = DateTime.MinValue;
            foreach(Statement statement in statements)
            {
                if (statement.Date < current)
                {
                    return true;
                }
                current = statement.Date;

            }
            return false;
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
