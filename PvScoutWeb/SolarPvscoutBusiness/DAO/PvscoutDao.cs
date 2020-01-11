using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solar.Data.Access;
using System.Data;
using Constants=Solar.Pvscout.Business.Common.Constants;

namespace Solar.Pvscout.Business.DAO
{
    public class PvscoutDao
    {
        private ISolarDatabase _pvscoutDatabase;
        private ISolarDatabase PvscoutDatabase
        {
            get
            {
                if (_pvscoutDatabase == null)
                    _pvscoutDatabase = new SolarGenericDataBase(DatabaseConnectionsEnum.PvScout);
                return _pvscoutDatabase;
            }
        }

        #region pvscount db
        public DataTable GetRoofType(String roofTypeId)
        {
            DataTable dtRetVal = null;

            PvscoutDatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetRoofTypes);
            PvscoutDatabase.AddInParameter("RoofTypeId", DbType.String, roofTypeId);

            dtRetVal = PvscoutDatabase.ExecuteReaderProcessed();

            return dtRetVal;
        }
        public DataTable GetAllRoofTypes()
        {
            return GetRoofType("0");
        }
        public DataTable GetManufacturers()
        {
            DataTable dtRetVal = null;

            PvscoutDatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetManufacturers);

            dtRetVal = PvscoutDatabase.ExecuteReaderProcessed();

            return dtRetVal;
        }

        private DataTable GetPvModules(String manufId, String pvscoutArticleNumber)
        {
            DataTable dtRetVal = null;

            PvscoutDatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetPvModules);
            if (!String.IsNullOrEmpty(pvscoutArticleNumber)) PvscoutDatabase.AddInParameter("pvscoutArticleNumber", DbType.String, pvscoutArticleNumber);
            if (!String.IsNullOrEmpty(manufId)) PvscoutDatabase.AddInParameter("ManufId", DbType.String, manufId);

            dtRetVal = PvscoutDatabase.ExecuteReaderProcessed();

            return dtRetVal;
        }
        public DataTable GetPvModule(String pvscoutArticleNumber)
        {
            return GetPvModules(null, pvscoutArticleNumber);
        }
        public DataTable GetAllPvModules(String manufId)
        {
            return GetPvModules(manufId, null);
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
                    _pvscoutDatabase = null;
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
