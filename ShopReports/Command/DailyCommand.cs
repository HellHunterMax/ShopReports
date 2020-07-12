using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopReports.Command
{
    public class DailyCommand : ICommand
    {
        private string _path;
        private List<string> _command;
        private List<Transaction> _transactions;
        private ReportsManager _reportsManager;
        private string _shop;
        private bool _ascending;

        public DailyCommand(string path, List<string> command, List<Transaction> transactions, ReportsManager reportsManager)
        {
            _path = path;
            _command = command;
            _transactions = transactions;
            _reportsManager = reportsManager;
        }

        public void Execute()
        {
            AscendingOrDescending();
            ExtractShopName();
            ValidateShopName();

            IEnumerable<EarnedDayDecimal> earnedPerDayDecimal = GetEarnedPerDayDecimalForShop();
            sortEarnedPerDayDecimalList(ref earnedPerDayDecimal);

            List<Earning> EarnedPerDay = GetEarningListFromEarnedDayDecimalList(earnedPerDayDecimal);

            _reportsManager.WriteModel(_path, EarnedPerDay);
        }

        private void AscendingOrDescending()
        {
            switch (_command[_command.Count - 1])
            {
                case "-desc":
                    _command.RemoveAt(_command.Count - 1);
                    _ascending = false;
                    break;

                case "-asc":
                    _command.RemoveAt(_command.Count - 1);
                    _ascending = true;
                    break;

                default:
                    _ascending = true;
                    break;
            }
        }

        private void ExtractShopName()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 1; i < _command.Count; i++)
            {
                sb.Append(_command[i] + " ");
            }
            if (String.IsNullOrEmpty(sb.ToString()))
            {
                throw new InvalidCommandException("Must enter a shop name.");
            }
            _shop = sb.ToString().Trim();
        }

        private void ValidateShopName()
        {
            IEnumerable<String> shopnames = _transactions.Select(z => z.ShopName).Distinct();

            if (shopnames.Contains(_shop))
            {
                return;
            }
            else
            {
                throw new InvalidCommandException($"Shop name {_shop} does not exsist.");
            }
        }

        private IEnumerable<EarnedDayDecimal> GetEarnedPerDayDecimalForShop()
        {
            IEnumerable<EarnedDayDecimal> sortedTransactionsByDayOfWeek = _transactions.Where(x => x.ShopName == _shop)
                                                            .Select(x => new { x.DateTime.DayOfWeek, x.Price, x.DateTime })
                                                            .GroupBy(x => x.DayOfWeek)
                                                            .Select(x => new EarnedDayDecimal
                                                            {
                                                                Day = x.First().DayOfWeek.ToString(),
                                                                //Sum of all day's devided by count of day's.
                                                                Earned = x.Sum(z => z.Price) / (x.Select(z => z.DateTime.Date).Distinct().Count())
                                                            });
            return sortedTransactionsByDayOfWeek;
        }

        private void sortEarnedPerDayDecimalList(ref IEnumerable<EarnedDayDecimal> earnedPerDayDecimal)
        {
            if (_ascending)
            {
                earnedPerDayDecimal = earnedPerDayDecimal.OrderBy(x => x.Earned);
            }
            else
            {
                earnedPerDayDecimal = earnedPerDayDecimal.OrderByDescending(x => x.Earned);
            }
        }

        private static List<Earning> GetEarningListFromEarnedDayDecimalList(IEnumerable<EarnedDayDecimal> sortedTransactionsByDayOfWeek)
        {
            List<Earning> sortedEarnedDayModel = new List<Earning>();

            foreach (EarnedDayDecimal earnedDayDecimal in sortedTransactionsByDayOfWeek)
            {
                sortedEarnedDayModel.Add(Earning.ConvertFromDecimal(earnedDayDecimal));
            }
            return sortedEarnedDayModel;
        }
    }
}