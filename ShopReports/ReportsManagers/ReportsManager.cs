using System;
using System.Collections.Generic;
using System.IO;

namespace ShopReports.ReportsManagers
{
    public abstract class ReportsManager
    {
        public abstract List<Transaction> ReadTransactionFile(string path);

        public void WriteTransaction(string path, string toBeWritten)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new NoTransactionsFoundException($"{nameof(path)} cannot be empty.");
            }

            File.AppendAllText(path, toBeWritten);
        }

        //public abstract void WriteTimeTransaction(string path, TimesModel timesModel);

       // public abstract void WriteCityTransaction(string path, string toBeWritten);

        public abstract void WriteModel<T>(string path, T model);

        public void ValidateFilePathToRead(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new NoTransactionsFoundException($"{nameof(path)} cannot be empty.");
            }
            if (!File.Exists(path))
            {
                throw new NoTransactionsFoundException($"{path} does not exist.");
            }
            if (new FileInfo(path).Length == 0)
            {
                throw new NoTransactionsFoundException($"{path} is empty");
            }
        }
        public void ValidateFilePathToWrite(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new NoTransactionsFoundException($"{nameof(path)} cannot be empty.");
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}