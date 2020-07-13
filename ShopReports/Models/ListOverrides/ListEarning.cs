using System;
using System.Collections.Generic;
using System.Text;

namespace ShopReports.Models.ListOverrides
{
    public class ListEarning<Earning> : List<Earning>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Day, Earned" + Environment.NewLine);
            foreach (Earning transactionDTO in this)
            {
                sb.Append(transactionDTO.ToString() + Environment.NewLine);
            }
            return sb.ToString();
        }

        public static ListEarning<Earning> ToListTransactionDTO(List<Earning> transactionDTOs)
        {
            ListEarning<Earning> newTransactionDTOs = new ListEarning<Earning>();

            foreach (Earning transactionDTO in transactionDTOs)
            {
                newTransactionDTOs.Add(transactionDTO);
            }
            return newTransactionDTOs;
        }
    }
}
