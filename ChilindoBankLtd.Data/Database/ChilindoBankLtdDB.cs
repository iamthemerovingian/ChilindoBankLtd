using ChilindoBankLtd.Data.Entities;
using System.Data.Entity;

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
