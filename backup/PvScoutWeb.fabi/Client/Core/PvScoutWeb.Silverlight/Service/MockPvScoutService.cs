using System.Collections.Generic;

namespace PvScoutWeb.Silverlight.Service
{
    public class MockPvScoutService : IPvScoutService
    {
        // fake a modul call
        public Model.PvModul GetPvModul()
        {
            return new Model.PvModul
            {
                Id = 1,
                Height = 180,
                Width = 60
            };

        }

        public List<Model.PvModul> GetPvModuls()
        {
            List<Model.PvModul> modulList = new List<Model.PvModul>();

            modulList.Add(new Model.PvModul{ Id = 1, Height = 180, Width = 60, Top = 20, Left = 20 });
            modulList.Add(new Model.PvModul{ Id = 2, Height = 180, Width = 60, Top = 20, Left = 100 });
            modulList.Add(new Model.PvModul{ Id = 3, Height = 180, Width = 60, Top = 20, Left = 180 });

            return modulList;

        }
    }
}
