using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIMaintenanceTasks
{
    public class MaintenanceManager
    {
        private MaintenanceDAO objMainDao = null; 
        public MaintenanceManager()
        {
             objMainDao = new MaintenanceDAO();
        }
        public void StartMaintenanceTasks(int progId)
        {
            ImageMaintenance objImgMain = new ImageMaintenance(objMainDao);
            objImgMain.StartImageMaintenance(progId);
        }
    }
}
