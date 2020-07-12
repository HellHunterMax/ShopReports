using System;
using System.Collections.Generic;
using System.IO;

namespace ShopReports.ReportsManagers
{
    public class ReportsManagerCsv : ReportsManager
    {
        public override List<Transaction> ReadTransactionFile(string path)
        {
            ValidateFilePathToRead(path);
            List<Transaction> transactions = new List<Transaction>();

            string[] transactionsLines = File.ReadAllText(path).Split(Environment.NewLine);

            foreach (string line in transactionsLines)
            {
                if (Transaction.TryParse(line, out Transaction transaction))
                {
                    transactions.Add(transaction);
                }
            }
            if (transactions.Count == 0)
            {
                throw new NoTransactionsFoundException($"{path} contained no vaid transactions.");
            }

            return transactions;
        }

        public override void WriteModel<T>(string path, T model)
        {
            throw new NotImplementedException();
        }
    }
}
