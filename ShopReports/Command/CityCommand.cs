using ShopReports.Models;
using ShopReports.ReportsManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopReports.Command
{
    internal class CityCommand : ICommand
    {
        private string _path;
        private List<string> _command;
        private List<Transaction> _transactions;
        private ReportsManager _reportsManager;

        public CityCommand(string path, List<string> command, List<Transaction> transactions, ReportsManager reportsManager)
        {
            _path = path;
            _command = command;
            _transactions = transactions;
            _reportsManager = reportsManager;
        }

        public void Execute()
        {
            var report = CreateReport();
            WriteToFile(report);
        }

        private IEnumerable<string> CreateReport()
        {
            const string money = "-money";
            const string items = "-items";
            const string min = "-min";
            const string max = "-max";
            
            if (_command.Count != 3)
            {
                throw new InvalidCommandException($"\"{String.Join(Environment.NewLine, _command)}\" has the wrong amount of parameters.");
            }

            string command = _command[0];
            string commandParameter1 = _command[1];
            string commandParameter2 = _command[2];

            if (commandParameter1 == items)
            {
                if (commandParameter2 == min || commandParameter2 == max)
                {
                    return FindItemsMaxOrMin(_transactions, commandParameter2);
                }
                else
                {
                    throw new InvalidCommandException($"\"{commandParameter2}\" is not a valid parameter.");
                }
            }
            else if (commandParameter1 == money)
            {
                if (commandParameter2 == min || commandParameter2 == max)
                {
                    return FindMoneyMaxOrMin(_transactions, commandParameter2);
                }
                else
                {
                    throw new InvalidCommandException($"\"{commandParameter2}\" is not a valid parameter.");
                }
            }
            else
            {
                throw new InvalidCommandException($"\"{commandParameter1}\" is not a valid parameter.");
            }
        }

        private static IEnumerable<String> FindMoneyMaxOrMin(List<Transaction> transactions, string maxOrMin)
        {
            var groupedcity = transactions.GroupBy(i => i.City)
                                          .Select(g => new { g.Key, Total = g.Sum(i => i.Price) });

            decimal itemsPerCityMaxOrMin = maxOrMin == "-max" ? groupedcity.Max(g => g.Total) : groupedcity.Min(g => g.Total);

            var toReturn = from i in groupedcity
                           where i.Total == itemsPerCityMaxOrMin
                           select i.Key.Trim();
            return toReturn;
        }

        private static IEnumerable<String> FindItemsMaxOrMin(List<Transaction> transactions, string maxOrMin)
        {
            var groupedcity = from i in transactions
                              group i.City by i.City into g
                              select new { g.Key, Count = g.Count() };

            int itemsPerCityMaxOrMin = maxOrMin == "-max" ? groupedcity.Max(g => g.Count) : groupedcity.Min(g => g.Count);

            var cityNames = from i in groupedcity
                            where i.Count == itemsPerCityMaxOrMin
                            select i.Key.Trim();
            return cityNames;
        }

        private void WriteToFile(IEnumerable<string> toBeWritten)
        {
            StringBuilder sb = new StringBuilder();

            foreach (String line in toBeWritten)
            {
                sb.AppendLine(line);
            }

            _reportsManager.WriteTransaction(_path, sb.ToString().TrimEnd('\n').TrimEnd('\r'));
        }
    }
}