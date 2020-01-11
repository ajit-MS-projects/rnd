using System;
using Affilinet.Data.Access;
using System.Data;
using Constants = Affilinet.ProductExport.Authentication.Common.Constants;

namespace Affilinet.ProductExport.Authentication.DAO
{
    /// <summary>
    /// Represents data access object for authentication tasks.
    /// </summary>
    public class AuthenticationDao : IDisposable
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
                    _productDBdatabase = _productDBdatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductData);

                return _productDBdatabase;
            }
        }

        /// <summary>
        /// Refers to Live Affilinet DB
        /// </summary>
        private IAffiliDatabase _affilinetDBdatabase;

        private IAffiliDatabase affilinetDBdatabase
        {
            get
            {
                if (_affilinetDBdatabase == null)
                    _affilinetDBdatabase = _affilinetDBdatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.Affilinet);

                return _affilinetDBdatabase;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationDao"/> class.
        /// </summary>
        public AuthenticationDao()
        { }

        #region Affilinet DB
        /// <summary>
        /// Returns the publisher ID if the password is correct
        /// </summary>
        /// <param name="login">The publisher id</param>
        /// <param name="password">The publisher password</param>
        /// <param name="admin"></param>
        /// <returns></returns>
        public int VerifyCredentials(int login, string password, bool admin)
        {
            int result = 0;
            
            affilinetDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.VerifyPartnerPassword);
            affilinetDBdatabase.AddInParameter("Login", DbType.Int32, login);
            affilinetDBdatabase.AddInParameter("Password", DbType.String, password);
            affilinetDBdatabase.AddInParameter("Admin", DbType.Boolean, admin);
            object objTmp = affilinetDBdatabase.ExecuteScalar();
            if (objTmp != null && objTmp != DBNull.Value)
                result = Convert.ToInt32(objTmp);

            return result;

        }
        #endregion

        #region Product DB
        /// <summary>
        /// Check if is allowed for the given publisher to download the list from this shop
        /// </summary>
        /// <param name="publisherID">The publisher id.</param>
        /// <param name="shopID">The shop id.</param>
        /// <param name="csvPassword">The csv password.</param>
        /// <returns></returns>
        public bool IsAuthorizedCSVPassword(int publisherID, int shopID, string  csvPassword)
        {
            bool result = false;

            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetListPermission);
            productDBdatabase.AddInParameter("PartnerID", DbType.Int32, publisherID);
            productDBdatabase.AddInParameter("CsvPw", DbType.String, csvPassword);
            productDBdatabase.AddInParameter("ListID", DbType.Int32, shopID);
            object objTmp = productDBdatabase.ExecuteScalar();
            if (objTmp != null && objTmp != DBNull.Value)
                result = Convert.ToBoolean(objTmp);            

            return result;
        }
        /// <summary>
        /// Check if is allowed for the given publisher to download the list from this shop
        /// </summary>
        /// <param name="publisherID">The publisher id.</param>
        /// <param name="shopID">The shop id.</param>
        /// <returns></returns>
        public bool isAuthorizedForShopID(int publisherID, int shopID)
        {
            bool result = false;

            productDBdatabase.SetupSqlCommand(Constants.ReadOnlyStoredProcs.fn_IsAuthorizedForShop);
            productDBdatabase.AddInParameter("PartnerID", DbType.Int32, publisherID);
            productDBdatabase.AddInParameter("ShopID", DbType.Int32, shopID);
            object objTmp = productDBdatabase.ExecuteScalar();
            if (objTmp != null && objTmp != DBNull.Value)
                result = Convert.ToBoolean(objTmp);

            return result;
        }
        #endregion

        #region Dispose

        private bool disposed;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public void Dispose(bool disposing)
        {
            if (!(this.disposed))
            {
                if (disposing)
                {
                    _productDBdatabase = null;
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// It also suppresses finalization
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);//This call is to makes sure that disposed object does not get put on the GC's finalize queue.
        }
        /// <summary>
        /// Finalizes this instance, if dispose is not called explicitly
        /// </summary>
        protected void Finalize()
        {
            Dispose(false);
        }
        #endregion
    }
}
