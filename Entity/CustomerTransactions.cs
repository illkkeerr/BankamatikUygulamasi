using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankamatikUygulamasi.Entity
{
    public class CustomerTransactions
    {
        [Key]
        public int TransactionsId { get; set; }
        public decimal AmountOfMoney { get; set; }
        public int TransactionTypeId { get; set; }
        public int CustomerId { get; set; }
        public string Explanation { get; set; } = "";
        public DateTime DateTime { get; set; } = DateTime.Now;
        public TransactionType TransactionType { get; set; }

        public Customer Customer { get; set; }

    }
}