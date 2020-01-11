using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Data.Access;
using System.Data;
using Const=AffiliSolrBusiness.Common.Constants;


namespace AffiliSolrBusiness.DAO
{
    public class ProductDao
    {
        /// <summary>
        /// Refers to Live product DB
        /// </summary>
        private IAffiliDatabase _productDBdatabase;
        /// <summary>
        /// Refers to a read only connection to live DB, used to read program fields and settings
        /// </summary>
        private IAffiliDatabase _liveDatabase;
        private IAffiliDatabase productDBdatabase
        {
            get
            {
                if(_productDBdatabase==null)
                    _productDBdatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductData);
                return _productDBdatabase;
            }
        }
         private IAffiliDatabase   liveDatabase
         {
             get
             {
                 if (_liveDatabase == null)
                     _liveDatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductDataLiveReadOnly);
                 return _liveDatabase;
             }
         }

        
        #region Product DB
         public DataTable GetProductSchema()
         {

             productDBdatabase.SetupCommand(Const.ReadOnlyStoredProcs.GetProductSchema);

             //productDBdatabase.AddInParameter("OrderByDesc", DbType.Int16, order);

             return productDBdatabase.ExecuteReaderProcessed();
         }
         public DataTable GetProducts(String productIds)
         {

             productDBdatabase.SetupCommand(Const.ReadOnlyStoredProcs.GetProducts);

             productDBdatabase.AddInParameter("productIds", DbType.String, productIds);

             return productDBdatabase.ExecuteReaderProcessed();
         }
        #endregion
    }
}
