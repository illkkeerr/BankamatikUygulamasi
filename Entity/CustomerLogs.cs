using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankamatikUygulamasi.Entity
{
    public class CustomerLogs
    {
        [Key]
        public int CustomerLogsId { get; set; }
        public DateTime LogTime { get; set; } = DateTime.Now;
        public bool IsLoginOrLogout { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
