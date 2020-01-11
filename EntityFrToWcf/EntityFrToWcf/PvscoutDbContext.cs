using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;

namespace EntityFrToWcf
{
    public class PvscoutDbContext : DbContext
    {
        public DbSet<Anwender> Anwenders { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<BankInfo> BankInfos { get; set; }
        public PvscoutDbContext()
        {
            Database.SetInitializer<PvscoutDbContext>(new PvscoutDbContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder p_ModelBuilder)
        {

            p_ModelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Deklaration der Tabellen für BasisModul
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            EntityTypeConfiguration<Anwender> anwenderConfigurator = p_ModelBuilder.Entity<Anwender>();
            EntityTypeConfiguration<BankInfo> bankverbindungConfigurator = p_ModelBuilder.Entity<BankInfo>();
            EntityTypeConfiguration<Person> personConfigurator = p_ModelBuilder.Entity<Person>();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Erstellen der Tabellen für BasisModul
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            anwenderConfigurator.ToTable("Anwender");
            bankverbindungConfigurator.ToTable("Bankverbindungen");
            personConfigurator.ToTable("Personen");
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

           base.OnModelCreating(p_ModelBuilder);
        }
    }


    public class PvscoutDbContextInitializer : DropCreateDatabaseIfModelChanges<PvscoutDbContext>
    {
        protected override void Seed(PvscoutDbContext context)
        {
            Anwender anwender = new Anwender()
                                    {
                                        AName = "Fred Peters",
                                        Person = new Person() {Name = "person for Fred"},
                                        BankInfos = new List<BankInfo>()
                                    };
            anwender.BankInfos.Add(new BankInfo(){BankName = "Sparkasse"});
            anwender.BankInfos.Add(new BankInfo() { BankName = "RBS" });
            context.Anwenders.Add(anwender);
            context.SaveChanges();
        }
    }
}
