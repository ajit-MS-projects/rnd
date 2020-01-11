using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EntityFrToWcf
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class AnwenderService : IAnwenderService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public Anwender LoadAnwender(int id)
        {
            Anwender anwender = null;
            using (PvscoutDbContext context = new PvscoutDbContext())
            {
                anwender =
                    context.Anwenders.Where(a => a.Id == id).Include(a => a.Person).Include(a => a.BankInfos).
                        FirstOrDefault();
            }
            return anwender;
        }

        public void SaveAnwender(Anwender anwender)
        {
            PvscoutDbContext context = new PvscoutDbContext();
            context.Anwenders.Add(anwender);
            context.SaveChanges();
        }
    }
}
