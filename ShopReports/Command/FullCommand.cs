using BootCamp.Chapter.Models;
using BootCamp.Chapter.ReportsManagers;
using System.Collections.Generic;
using System.Linq;

namespace BootCamp.Chapter.Command
{
    internal class FullCommand : ICommand
    {
        private List<string> _command;
        private List<Transaction> _transactions;
        private ReportsManager _reportsManager;
        private string _fileExtention;
        private bool _ascending = true;

        public FullCommand(string fileExtention, List<string> command, List<Transaction> transactions, ReportsManager reportsManager)
        {
            _fileExtention = fileExtention;
            _command = command;
            _transactions = transactions;
            _reportsManager = reportsManager;
        }

        public void Execute()
        {
            AscendingOrDescending();
            IEnumerable<IEnumerable<Transaction>> transactionsSortedByShop = TransactionsSortedByShop();
            WritePerShop(transactionsSortedByShop);
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

        private IEnumerable<IEnumerable<Transaction>> TransactionsSortedByShop()
        {
            IEnumerable<IEnumerable<Transaction>> sortedList =
                from tr in _transactions
                group tr by tr.ShopName into newGroup
                select newGroup;

            sortEarnedPerDayDecimalList(ref sortedList);

            return sortedList;
        }

        private void sortEarnedPerDayDecimalList(ref IEnumerable<IEnumerable<Transaction>> sortedList)
        {
            if (_ascending)
            {
                sortedList = sortedList.Select(x => x.OrderBy(c => c.Price));
            }
            else
            {
                sortedList = sortedList.Select(x => x.OrderByDescending(c => c.Price));
            }
        }

        private void WritePerShop(IEnumerable<IEnumerable<Transaction>> shoplist)
        {
            List<IEnumerable<Transaction>> shoplist1 = shoplist.ToList();

            for (int i = 0; i < shoplist1.Count; i++)
            {
                IEnumerable<TransactionDTO> transactionDTOs = createDTOListFromTransactionList(shoplist1[i]);
                _reportsManager.WriteModel(shoplist1[i].First().ShopName + _fileExtention, transactionDTOs);
            }
        }

        private IEnumerable<TransactionDTO> createDTOListFromTransactionList(IEnumerable<Transaction> transactions)
        {
            return transactions.Select(a => new TransactionDTO
            {
                City = a.City,
                DateTime = a.DateTime.ToString(),
                Item = a.Item,
                Price = TransactionDTO.ConvertDecimalToStringCurrency(a.Price),
                Street = a.Street
            });
        }
    }
}