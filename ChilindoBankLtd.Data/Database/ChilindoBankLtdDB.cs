using ChilindoBankLtd.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilindoBankLtd.Data.Database
{
    public class ChilindoBankLtdDB :DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }

        public ChilindoBankLtdDB() : base("ChilindoBankLtd_SQL")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
