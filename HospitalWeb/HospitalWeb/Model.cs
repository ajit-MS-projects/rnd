using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HospitalWeb
{
    public class Patient
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        public virtual ICollection<LabResult> LabResults { get; set; }
    }

    public class LabResult
    {
        public int Id { get; set; }
        public string Result { get; set; }
    }

    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<LabResult> LabResults { get; set; }

        static HospitalContext()
        {
            Database.SetInitializer(new HostpitalContextInitializer());
        }
    }

    public class HostpitalContextInitializer :
                 DropCreateDatabaseIfModelChanges<HospitalContext>
    {
        protected override void Seed(HospitalContext context)
        {
            context.Patients.Add(new Patient { Name = "Fred Peters" });
            context.Patients.Add(new Patient { Name = "John Smith" });
            context.Patients.Add(new Patient { Name = "Karen Fredricks" });
            context.SaveChanges();
        }
    }
}