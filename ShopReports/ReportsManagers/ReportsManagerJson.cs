using BootCamp.Chapter.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BootCamp.Chapter.ReportsManagers
{
    public class ReportsManagerJson : ReportsManager
    {
        public override List<Transaction> ReadTransactionFile(string path)
        {
            ValidateFilePathToRead(path);
            List<TransactionModeljson> dtoTransactions = JsonConvert.DeserializeObject<List<TransactionModeljson>>(File.ReadAllText(path));
            List<Transaction> transactions = new List<Transaction>();

            foreach (TransactionModeljson transaction in dtoTransactions)
            {
                if (Transaction.TryParse(transaction.ToString(), out Transaction tr))
                {
                    transactions.Add(tr);
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
            ValidateFilePathToWrite(path);
            string data = JsonConvert.SerializeObject(model, Formatting.Indented);

            WriteTransaction(path, data);
        }
    }
}