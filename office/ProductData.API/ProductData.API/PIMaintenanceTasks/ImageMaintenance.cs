using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PIMaintenanceTasks
{
    public class ImageMaintenance
    {
        public static int RowCount = 0;
        private MaintenanceDAO objMainDao = null;
        public ImageMaintenance()
        {
            this.objMainDao = new MaintenanceDAO();
        }
        public ImageMaintenance(MaintenanceDAO objMainDao)
        {
             this.objMainDao = objMainDao;
        }
        public int GetCountOfImagesForHashing(int prodProgId)
        {
            return objMainDao.GetCountOfImagesForHashing(prodProgId);
        }
        public void StartImageMaintenance(int prodProgId)
        {
            DataTable dtImages = null;
            do
            {
                dtImages = objMainDao.GetImagesForHashing(prodProgId);
                dtImages.TableName = MConstants.Generic.ProductImageTable + "Tmp";
                if (dtImages.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtImages.Rows)
                    {
                        if (dr[MConstants.ProductImage.ImageURL] != DBNull.Value)
                        {
                            string imgUrl =
                                dr[MConstants.ProductImage.ImageURL].ToString();
                            dr[MConstants.ProductImage.HashCode] =
                                MUtilities.GetMd5Hash(imgUrl);
                        }
                    }
                    RowCount += dtImages.Rows.Count;
                    objMainDao.BulkInsertTable(dtImages);
                    objMainDao.UpdateImagesHashing();
                }
            } while (dtImages != null && dtImages.Rows.Count > 0);

        }
    }
}
