using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ShopReports.Models
{
    [XmlRoot("Times")]
    public class TimesModel
    {
        [XmlElement("Times")]
        public List<Time> Times { get; set; }

        public int RushHour { get; set; }

        public TimesModel()
        {
        }

        public TimesModel(List<Time> times, int rushHour)
        {
            Times = times;
            RushHour = rushHour;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Hour, Count, Earned" + Environment.NewLine);

            foreach (Time time in Times)
            {
                sb.Append(time.ToCsvString() + Environment.NewLine);
            }

            sb.Append($"Rush hour: {RushHour}" + Environment.NewLine);
            return sb.ToString();
        }
    }
}