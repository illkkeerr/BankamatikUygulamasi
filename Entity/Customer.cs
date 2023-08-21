using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankamatikUygulamasi.Entity
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }    

        public virtual List<CustomerLogs> CustomerLogs { get; set; }        
        public virtual List<CustomerTransactions> CustomerTransactions { get; set; }
    }
}
