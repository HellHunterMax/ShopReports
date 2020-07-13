using System;
using System.Collections.Generic;
using System.Text;

namespace ShopReports.Models.ListOverrides
{
    public class ListTransactionDTO<TransactionDTO> : List<TransactionDTO>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("City, Street, Item, DateTime, Price" + Environment.NewLine);
            foreach (TransactionDTO transactionDTO in this)
            {
                sb.Append(transactionDTO.ToString() + Environment.NewLine);
            }
            return sb.ToString();
        }

        public static ListTransactionDTO<TransactionDTO> ToListTransactionDTO(List<TransactionDTO> transactionDTOs)
        {
            ListTransactionDTO<TransactionDTO> newTransactionDTOs = new ListTransactionDTO<TransactionDTO>();

            foreach (TransactionDTO transactionDTO in transactionDTOs)
            {
                newTransactionDTOs.Add(transactionDTO);
            }
            return newTransactionDTOs;
        }
    }
}
