using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Affilinet.Data.Access;
using Constants = Affilinet.Business.ProductExport.Common.Constants;

namespace Affilinet.Business.ProductExport.DAO
{
    public class ExportDao
    {
        /// <summary>
        /// Refers to Live product DB
        /// </summary>
        private IAffiliDatabase _productDBdatabase;
        private IAffiliDatabase productDBdatabase
        {
            get
            {
                if (_productDBdatabase == null)
                    _productDBdatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductData);
                return _productDBdatabase;
            }
        }

        #region Product DB
        public DataSet ReadPublisherAndFileLocationSettings(String ProdProgId, String PublisherId)
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.ReadPublisherAndFileLocationSettings);

            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, ProdProgId);
            productDBdatabase.AddInParameter("PublisherId", DbType.Int32, PublisherId);

            return productDBdatabase.ExecuteDataset();

        }

        public DataSet ReadExportconfiguration()
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.ReadExportconfiguration);

            return productDBdatabase.ExecuteDataset();
        }
        #endregion
    }
}
