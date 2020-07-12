using ShopReports.Models;
using System;
using System.Collections.Generic;

namespace ShopReports.ReportsManagers
{
    internal class ReportsManagerXML : ReportsManager
    {
        public override List<Transaction> ReadTransactionFile(string path)
        {
            List<Transaction> transactions = new List<Transaction>();
            TransactionModelxml.Transactions xmlTransactions = XmlConvert.DeserializeFile<TransactionModelxml.Transactions>(path);
            foreach (TransactionModelxml.TransactionsTransaction tr in xmlTransactions.Transaction)
            {
                if (Transaction.TryParse(tr.ToString(), out Transaction trans))
                {
                    transactions.Add(trans);
                }
            }
            return transactions;
        }

        public override void WriteModel<T>(string path, T model)
        {
            ValidateFilePathToWrite(path);
            string data = XmlConvert.SerializeObject(model);

            WriteTransaction(path, data);
        }
    }
}