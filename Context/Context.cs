using BankamatikUygulamasi.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankamatikUygulamasi.Context
{
    public class Context : DbContext
    {
        public DbSet<BankamatikUygulamasi.Entity.Customer> Customers { get; set; }
        public DbSet<CustomerLogs> CustomerLogs { get; set; }
        public DbSet<CustomerTransactions> CustomerTransactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

    }
}
