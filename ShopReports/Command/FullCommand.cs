﻿using ShopReports.Models;
using ShopReports.Models.ListOverrides;
using ShopReports.ReportsManagers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopReports.Command
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
            ValidateCommand();
            AscendingOrDescending();

            List<List<Transaction>> transactionsSortedByShop = TransactionsSortedByShop();
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

        private List<List<Transaction>> TransactionsSortedByShop()
        {
            IEnumerable<IEnumerable<Transaction>> sortedList =
                from tr in _transactions
                group tr by tr.ShopName into newGroup
                select newGroup;

            return sortEarnedPerDayDecimalList(sortedList); ;
        }

        private List<List<Transaction>> sortEarnedPerDayDecimalList(IEnumerable<IEnumerable<Transaction>> sortedList)
        {
            if (_ascending)
            {
                return sortedList.Select(x => x.OrderBy(c => c.Price).ToList()).ToList();
            }
            else
            {
                return sortedList.Select(x => x.OrderByDescending(c => c.Price).ToList()).ToList();
            }
        }

        private void WritePerShop(List<List<Transaction>> shoplist)
        {
            List<List<Transaction>> shoplist1 = shoplist.ToList();

            for (int i = 0; i < shoplist1.Count; i++)
            {
                ListTransactionDTO<TransactionDTO> transactionDTOs = ListTransactionDTO<TransactionDTO>.ToListTransactionDTO(createDTOListFromTransactionList(shoplist1[i]).ToList());
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

        private void ValidateCommand()
        {
            if (_command.Count > 1 && _command.Count < 3)
            {
                if(!(_command[1] == "-asc" || _command[1] == "-desc"))
                {
                    throw new InvalidCommandException($"\"{_command[1]}\" is not a valid parameter for {_command[0]}.");
                }
            }
            else if (_command.Count == 1)
            {

            }
            else
            {
                throw new InvalidCommandException($"\"{String.Join(Environment.NewLine, _command)}\" has the wrong amount of parameters.");
            }
        }
    }
}