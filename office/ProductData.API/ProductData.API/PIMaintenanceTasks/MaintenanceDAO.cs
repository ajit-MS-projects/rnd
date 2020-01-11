using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Data.Access;
using System.Data;
using System.Data.SqlClient;

namespace PIMaintenanceTasks
{
    /// <summary>
    /// Data access object, contains all method to access DB
    /// </summary>
    public class MaintenanceDAO
    {
        private IAffiliDatabase productDBdatabase;
        private IAffiliDatabase localDatabase;
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceDAO"/> class.
        /// </summary>
        public MaintenanceDAO()
        {
            productDBdatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductData);
            localDatabase = new AffiliGenericDataBase(DatabaseConnectionsEnum.LocalCache); 
        }

        /// <summary>
        /// Gets the images for hashing for all programs.
        /// </summary>
        /// <returns></returns>
        public DataTable GetImagesForHashing()
        {
            return GetImagesForHashing(0);
        }

        /// <summary>
        /// Gets the images for hashing.
        /// </summary>
        /// <param name="prodProgId">The prod prog id for which images will be returned.</param>
        /// <returns></returns>
        public DataTable GetImagesForHashing(int prodProgId)
        {
            try
            {
                productDBdatabase.SetupCommand(MConstants.ReadOnlyStoredProcs.MnGetImagesForHashing);
                productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);
                productDBdatabase.Command.CommandTimeout = 9999;

                DataTable dtTmp = productDBdatabase.ExecuteDataset().Tables[0];

                return dtTmp;
            }
            catch (Exception ex)
            {
                throw;//TODO
            }
        }

        /// <summary>
        /// Gets the count of images for hashing.
        /// </summary>
        /// <param name="prodProgId">The prod prog id.</param>
        /// <returns></returns>
        public int GetCountOfImagesForHashing(int prodProgId)
        {
            try
            {
                productDBdatabase.SetupCommand(MConstants.ReadOnlyStoredProcs.MnGetCountOfImagesForHashing);
                productDBdatabase.AddInParameter("ProdProgId", DbType.Int32, prodProgId);

                object count = productDBdatabase.ExecuteScalar();

                int retVal = 0;
                if (count != DBNull.Value)
                    int.TryParse(count.ToString(),out retVal);
                
                return retVal;
            }
            catch (Exception ex)
            {
                throw;//TODO
            }
        }
        /// <summary>
        /// Creates the image temp table.
        /// </summary>
        /// <returns></returns>
        private void CreateImageTempTable()
        {
            try
            {
                productDBdatabase.SetupCommand(MConstants.ReadOnlyStoredProcs.CreateProductImageTemp);
                
                productDBdatabase.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;//TODO
            }

        }
        /// <summary>
        /// Updates the images hashing.
        /// </summary>
        public void UpdateImagesHashing()
        {
            try
            {
                productDBdatabase.SetupCommand(MConstants.ReadOnlyStoredProcs.UpdateImagesHashing);
                productDBdatabase.Command.CommandTimeout = 9999;

                productDBdatabase.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw;//TODO
            }

        }
        /// <summary>
        /// Bulks the insert table.
        /// </summary>
        /// <param name="TheSource">The source.</param>
        /// <returns></returns>
        public bool BulkInsertTable(DataTable TheSource)
        {
            bool result = false;
            DataTable DT = null;
            SqlBulkCopy BulkCopy = null;
            try
            {
                CreateImageTempTable();
                SqlConnection cnn =
                    new SqlConnection(
                        MUtilities.GetConnectionString(Affilinet.Data.Access.Constants.DBConnections.ProductData));
                
                cnn.Open();
                BulkCopy = new SqlBulkCopy(cnn);
                BulkCopy.DestinationTableName = TheSource.TableName;
//                BulkCopy.NotifyAfter = 50000;
                BulkCopy.BulkCopyTimeout = 60*60;
                

                BulkCopy.WriteToServer(TheSource);
                result = true;
                BulkCopy.Close();
                cnn.Close();
                cnn.Dispose();
            }
            catch (Exception Ex)
            {
                throw;
            }
            return result;
        }
    }
}
