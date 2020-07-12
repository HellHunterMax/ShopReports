using System.Globalization;

namespace ShopReports.Models
{
    public class TransactionDTO
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string Item { get; set; }
        public string DateTime { get; set; }
        public string Price { get; set; }

        public static string ConvertDecimalToStringCurrency(decimal price)
        {
            return price.ToString("C2", CultureInfo.GetCultureInfo("lt-LT"));
        }

        public override string ToString()
        {
            return $"{City},{Street},{Item},{DateTime},{Price}";
        }
    }
}