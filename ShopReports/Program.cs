using System;

namespace BootCamp.Chapter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.ASCII;
            ArgsReader argsReader = new ArgsReader();
            argsReader.Read(args);
        }
    }
}