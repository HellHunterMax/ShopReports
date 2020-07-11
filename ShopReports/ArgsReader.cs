using BootCamp.Chapter.Command;
using BootCamp.Chapter.Exceptions;
using BootCamp.Chapter.ReportsManagers;
using System.Collections.Generic;
using System.Linq;

namespace BootCamp.Chapter
{
    public class ArgsReader
    {
        private const string timeCommand = "time";
        private const string cityCommand = "city";
        private const string dailyCommand = "daily";
        private const string fullCommand = "full";
        private const int fileToReadInt = 0;
        private const int commandInt = 1;
        private const int fileToWriteInt = 2;
        private string fileExtention = string.Empty;

        public void Read(string[] args)
        {
            ValidateArgs(args);
            ValidateCommand(args[commandInt]);

            ReportsManager reportsManager = GetCorrectReportsManagerFromArgs(args);

            List<Transaction> transactions = reportsManager.ReadTransactionFile(args[fileToReadInt]);

            ICommand command = GetCommand(args, reportsManager, transactions);

            command.Execute();
        }

        private ICommand GetCommand(string[] args, ReportsManager reportsManager, List<Transaction> transactions)
        {
            List<string> commands = args[commandInt].Split(' ').ToList();
            switch (commands[0].ToLower())
            {
                case timeCommand:
                    return new TimeCommand(args[fileToWriteInt], commands, transactions, reportsManager);

                case cityCommand:
                    return new CityCommand(args[fileToWriteInt], commands, transactions, reportsManager);

                case dailyCommand:
                    return new DailyCommand(args[fileToWriteInt], commands, transactions, reportsManager);

                case fullCommand:
                    return new FullCommand(fileExtention, commands, transactions, reportsManager);
            }

            return default;
        }

        private ReportsManager GetCorrectReportsManagerFromArgs(string[] args)
        {
            ReportsManager reportsManager;

            string fileType = args[fileToReadInt].Split('.')[1];

            switch (fileType)
            {
                case "json":
                    fileExtention = ".json";
                    reportsManager = new ReportsManagerJson();
                    break;

                case "xml":
                    fileExtention = ".xml";
                    reportsManager = new ReportsManagerXML();
                    break;

                default:
                    throw new FileExtensionUnsupportedException();
            }

            return reportsManager;
        }

        private void ValidateCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new InvalidCommandException($"Please give a valid command.");
            }
            string[] splitCommand = command.Split(' ');

            for (int i = 0; i < splitCommand.Length; i++)
            {
                splitCommand[i] = splitCommand[i].Trim();
            }

            switch (splitCommand[0])
            {
                case timeCommand:
                    break;

                case cityCommand:
                    break;

                case dailyCommand:
                    break;

                case fullCommand:
                    break;

                default:
                    throw new InvalidCommandException($"{splitCommand[0]} is not a valid command.");
            }
        }

        private void ValidateArgs(string[] args)
        {
            if (args == null || !(args.Length == 3 || args.Length == 2))
            {
                throw new InvalidCommandException($"The {nameof(args)} must contain 1. The file path to read. 2. The command. 3. The file path to write. except for Full command.");
            }
        }
    }
}