using System;

namespace ShopReports
{
    internal class FileExtensionUnsupportedException : Exception
    {
        public FileExtensionUnsupportedException() : base()
        {
        }

        public FileExtensionUnsupportedException(string message) : base(message)
        {
        }

        public FileExtensionUnsupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}