using BootCamp.Chapter.Models;
using System;
using System.Collections.Generic;

namespace BootCamp.Chapter.ReportsManagers
{
    internal class ReportsManagerXML : ReportsManager
    {
        public override List<Transaction> ReadTransactionFile(string path)
        {
            List<Transaction> transactions = new List<Transaction>();
            Models.XML.TransactionModelxml.Transactions xmlTransactions = XmlConvert.DeserializeFile<Models.XML.TransactionModelxml.Transactions>(path);
            foreach (Models.XML.TransactionModelxml.TransactionsTransaction tr in xmlTransactions.Transaction)
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

        public override void WriteTimeTransaction(string path, TimesModel timesModel)
        {
            ValidateFilePathToWrite(path);
            string data = XmlConvert.SerializeObject(timesModel);

            WriteTransaction(path, data);
        }
    }
}