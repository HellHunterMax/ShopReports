using System;

namespace BootCamp.Chapter
{
    public class NoTransactionsFoundException : Exception
    {
        public NoTransactionsFoundException() : base()
        {
        }

        public NoTransactionsFoundException(string message) : base(message)
        {
        }

        public NoTransactionsFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}