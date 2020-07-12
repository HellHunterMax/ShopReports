using System.Globalization;

namespace ShopReports.Models
{
    public class Time
    {
        public int Hour { get; set; }
        public int Count { get; set; }
        public string Earned { get; set; }

        public static Time ConvertFromDecimal(HourCountEarnedDecimal input)
        {
            return new Time() { Hour = input.Hour, Count = input.Count, Earned = ConvertDecimalToStringCurrency(input.Earned) };
        }

        private static string ConvertDecimalToStringCurrency(decimal earned)
        {
            return earned.ToString("C2", CultureInfo.GetCultureInfo("lt-LT"));
        }
    }
}