using System;
using System.Data;
using Affilinet.Data.Access;
using Constants = AffiliErrorIndexing.Common.Constants;

namespace AffiliErrorIndexing.DAO
{
    class ErrorIndexingDAO
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

        
        /// <summary>
        /// Updates the Pa_ProductProgramErrorIndex table and set the issueOccurred flag
        /// </summary>
        /// <param name="prodProgId"></param>
        /// <param name="errorIndexName"></param>
        /// <param name="issueOccurred"></param>
        public void UpdateErrorIndex(int prodProgId, String errorIndexName, Boolean issueOccurred)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateErrorIndex);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("errorIndexName", DbType.String, errorIndexName);
            productDBdatabase.AddInParameter("issueOccurred", DbType.Boolean, issueOccurred);

            productDBdatabase.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the Pa_ProductProgramErrorIndex table and set the issueOccurred flag
        /// </summary>
        /// <param name="prodProgId"></param>
        /// <param name="weightConfigurationId"></param>
        /// <param name="title"></param>
        /// <param name="issueOccurred"></param>
        /// <param name="avg"></param>
        /// <param name="deviation"></param>
        internal void UpdatePa_PPErrorIndex(string prodProgId, string weightConfigurationId, string title, bool issueOccurred, double avg, double deviation, double valueLastCycle)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdatePa_PPErrorIndex);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("WeightConfigurationId", DbType.String, weightConfigurationId);
            productDBdatabase.AddInParameter("Title", DbType.String, title);
            productDBdatabase.AddInParameter("IssueOccurred", DbType.Boolean, issueOccurred);
            productDBdatabase.AddInParameter("Average", DbType.Int32, avg);
            productDBdatabase.AddInParameter("Deviation", DbType.Int32, deviation);
            productDBdatabase.AddInParameter("ValueLastCycle", DbType.String, valueLastCycle);

            productDBdatabase.ExecuteNonQuery();
        }
        
        

        /// <summary>
        /// Return all requiered values for the ErrorIndex calculation for one indicator
        /// </summary>
        /// <param name="prodProgId">The programId</param>
        /// <param name="weightageConfigurationTitle">Title in Pa_WeightageConfiguration table</param>
        /// <param name="masterListName">Name in MasterList table</param>
        /// <returns></returns>
        public DataSet GetValuesForErrorIndexIndicator(string prodProgId, string weightageConfigurationTitle, string masterListName)
        {
            DataSet dsRetVal = null;

            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetValuesForErrorIndexIndicator);
            productDBdatabase.AddInParameter("ProdProgId", DbType.String, prodProgId);
            productDBdatabase.AddInParameter("WeightageConfigurationTitle", DbType.String, weightageConfigurationTitle);
            productDBdatabase.AddInParameter("MasterListName", DbType.String, masterListName);

            dsRetVal = productDBdatabase.ExecuteDataset();
            return dsRetVal;
        }

        /// <summary>
        /// Return all requiered values for the ErrorIndex calculation (formular)
        /// </summary>
        /// <param name="prodProgId">The programId</param>
        /// <returns></returns>
        public DataTable GetValuesForErrorIndexCalculation(string prodProgId)
        {
            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetValuesForErrorIndexCalculation);
            productDBdatabase.AddInParameter("ProdProgId", DbType.String, prodProgId);
            return productDBdatabase.ExecuteReaderProcessed();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodProgId"></param>
        /// <param name="priority"></param>
        /// <param name="totalIndicatorWeight"></param>
        internal void UpdateProductProgram(string prodProgId, decimal priority, int totalIndicatorWeight)
        {
            productDBdatabase.SetupCommand(Constants.DmlStoredProcs.UpdateProductProgramPriority);
            productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
            productDBdatabase.AddInParameter("Priority", DbType.Decimal, priority);
            productDBdatabase.AddInParameter("ErrorIndicator", DbType.Int32, totalIndicatorWeight);
            
            productDBdatabase.ExecuteNonQuery();
        }

        internal int GetIndicatorWeight(string prodProgId, string errIndicator)
        {
            int result = 0;

            productDBdatabase.SetupCommand(Constants.ReadOnlyStoredProcs.GetErrorIndicatorWeight);
            productDBdatabase.AddInParameter("ProdProgId", DbType.String, prodProgId);
            productDBdatabase.AddInParameter("ErrorIndexName", DbType.String, errIndicator);
            object retVal = productDBdatabase.ExecuteScalar();
            if (retVal != null && retVal != DBNull.Value && !string.IsNullOrEmpty(retVal.ToString()))
                int.TryParse(retVal.ToString(), out result);
            
            return result;
        }
    }
}
