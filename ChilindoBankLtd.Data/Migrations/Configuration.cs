namespace ChilindoBankLtd.Data.Migrations
{
    using ChilindoBankLtd.Data.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ChilindoBankLtd.Data.Database.ChilindoBankLtdDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ChilindoBankLtd.Data.Database.ChilindoBankLtdDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            DbModelBuilder modelBuilder = new DbModelBuilder();
            modelBuilder.Entity<BankAccount>().Property(p => p.RowVersion).IsConcurrencyToken();

            context.BankAccounts.AddOrUpdate((a) => new {a.AccountNumber, a.Balance },
                new BankAccount { AccountNumber = 11111111, Balance = 10, Currency = "US", IsLocked = false },
                new BankAccount { AccountNumber = 22222222, Balance = 100, Currency = "US", IsLocked = false },
                new BankAccount { AccountNumber = 33333333, Balance = 1000, Currency = "US", IsLocked = false },
                new BankAccount { AccountNumber = 44444444, Balance = 10000, Currency = "US", IsLocked = false }
                );
        }
    }
}
