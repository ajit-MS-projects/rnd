using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Data.SqlCe;

namespace Solar.Data.Access
{
    /// <summary>
    /// This class provides generic implementation to access database, prvides interface to execute SPs, scalar queries, in transactional and non transactional way.
    /// Change History: Version;Date;Changed by;Change description
    /// </summary>
    public class SolarSqlCompactDataBase //: ISolarDatabase
    {
        private SqlCeDatabase _LibraryDataBase;

        #region Public Properties
        /// <summary>
        /// Represents current Dbcommand instance, is set automatically one setup command is called.
        /// </summary>
        public DbCommand Command { get; set; }
        /// <summary>
        /// Represents multiple commands, useful in case of transactions in code are required.
        /// </summary>
        public List<DbCommand> Commands { get; set; }
        /// <summary>
        /// Represents MS Enterprise Library data access block
        /// </summary>
        public SqlCeDatabase LibraryDataBase { get { return _LibraryDataBase; } }
        /// <summary>
        /// Is set to true automatically if multiple comaands are set up, one needs to set to false to use single command on same instance of SolarGenericDataBase.
        /// </summary>
        public bool MultipleCommands { get; set; }
        #endregion

        #region Initialize
        /// <summary>
        /// Constructor initializes MS DAL instances.
        /// </summary>
        /// <param name="dbConnectionStringId">Enum representing one of Solar Databases.</param>
        public SolarSqlCompactDataBase(DatabaseConnectionsEnum dbConnectionStringId)
        {
            _LibraryDataBase = new SqlCeDatabase(ConfigurationManager.ConnectionStrings[GetDB_Connection(dbConnectionStringId)].ConnectionString); //DatabaseFactory.CreateDatabase(GetDB_Connection(dbConnectionStringId));
            Commands = new List<DbCommand>();
        }

        /// <summary>
        /// Initializes sql string DBcommand object.
        /// </summary>
        /// <param name="sql">Stored procedure name</param>
        public void SetupSqlCommand(string sql)
        {
            Command = LibraryDataBase.GetSqlStringCommand(sql);
        }

        /// <summary>
        /// Initializes stored procedure DBcommand object.
        /// </summary>
        /// <param name="sql">Stored procedure name</param>
        public void SetupCommand(string sql)
        {
            Command = LibraryDataBase.GetSqlStringCommand(sql);
           // Command.CommandTimeout = 120;
        }
        /// <summary>
        /// This method adds storedprocedure command to list of commands to be executed later in transaction.
        /// </summary>
        /// <param name="sql">Stored procedure name</param>
        public void SetupMultipleCommands(string sql)
        {
            MultipleCommands = true;
            DbCommand cmd = LibraryDataBase.GetSqlStringCommand(sql);
            //cmd.CommandTimeout = 120;
            Commands.Add(cmd);
        }
        #endregion

        #region Parameters
        /// <summary>
        /// This method adds and input parameter to SP command
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dbType">Database data type</param>
        /// <param name="value">Parameter value</param>
        public void AddInParameter(string name, DbType dbType, object value)
        {
            LibraryDataBase.AddInParameter(GetCommand(), name, dbType, value);
        }
        /// <summary>
        /// This method adds and output parameter to SP command
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dbType">Database data type</param>
        /// <param name="size">Width in chars or Size of parameter</param>
        public void AddOutParameter(string name, DbType dbType, int size)
        {
            LibraryDataBase.AddOutParameter(GetCommand(), name, dbType, size);
        }
        /// <summary>
        /// This method passes a parameter (input, output, InputOutput, return) & a value to a stored procedure.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dbType">Database data type</param>
        /// <param name="direction">Input/Output/InOut/return direction of SP parameter</param>
        /// <param name="value">Parameter value</param>
        public void AddParameter(string name, DbType dbType, ParameterDirection direction, object value)
        {
            LibraryDataBase.AddParameter(GetCommand(), name, dbType, direction, "", DataRowVersion.Default, value);
        }
        /// <summary>
        /// This method passes a parameter (output, InputOutput, return) to a stored procedure.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dbType">Database data type</param>
        /// <param name="direction">Output/InOut/return direction of SP parameter</param>
        public void AddParameter(string name, DbType dbType, ParameterDirection direction)
        {
            AddParameter(name, dbType, direction, DBNull.Value);
        }
        /// <summary>
        /// This method finds the value of the specified parameter including(return).
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Value of the out parameter</returns>
        public object GetParameterValue(string name)
        {
            return LibraryDataBase.GetParameterValue(GetCommand(), name);
        }
        /// <summary>
        /// This method sets the value of the specified parameter when one wants to execute multiple inserts using the same connection and command but with different parameter values.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        public void SetParameterValue(string name, object value)
        {
            LibraryDataBase.SetParameterValue(GetCommand(), name, value);
        }
        #endregion

        #region Execute Command
        /// <summary>
        /// Sets up a SP command object for use in subsequent requests.
        /// </summary>
        /// <returns>IDataReader object with live connection</returns>
        public IDataReader ExecuteReader()
        {
            IDataReader objIDataReader;
            objIDataReader = LibraryDataBase.ExecuteReader(Command);
            Command.Dispose();
            Command = null;
            return objIDataReader;
        }
        /// <summary>
        /// Executes a command for datareader.
        /// </summary>
        /// <returns>Datatable object (with a closed conection.)</returns>
        public DataTable ExecuteReaderProcessed()
        {
            DataTable dtReturnData = new DataTable();
            using (IDataReader objIDataReader = LibraryDataBase.ExecuteReader(Command))
            {
                dtReturnData.Load(objIDataReader);
            }
            Command.Dispose();
            Command = null;
            return dtReturnData;
        }
        /// <summary>
        /// Executes command for a dataset.
        /// </summary>
        /// <returns>Returns a dataset.</returns>
        public DataSet ExecuteDataset()
        {
            DataSet dsRet = LibraryDataBase.ExecuteDataSet(Command);
            Command.Dispose();
            Command = null;
            return dsRet;
        }

        /// <summary>
        /// Executes command for a scalar query.
        /// </summary>
        /// <returns>Returns a dataset.</returns>
        public object ExecuteScalar()
        {
            object objRet = LibraryDataBase.ExecuteScalar(Command);
            Command.Dispose();
            Command = null;
            return objRet;
        }
        /// <summary>
        /// Executes a DML/DDL
        /// </summary>
        /// <returns>No.s of rows affected</returns>
        public int ExecuteNonQuery()
        {
            int retVal = LibraryDataBase.ExecuteNonQuery(Command);
            Command.Dispose();
            Command = null;
            return retVal;
        }
        /// <summary>
        /// Executes multiple DMLs/DDLs in transactional/ non-transactional mode.
        /// </summary>
        /// <returns>No.s of rows affected</returns>
        public int[] ExecuteMultipleNonQuery(bool transactional)
        {
            int[] cnt = new int[Commands.Count];
            TransactionScopeOption ObjtranScopeOpt = transactional ? TransactionScopeOption.Required : TransactionScopeOption.Suppress;
            using (TransactionScope scope = new TransactionScope(ObjtranScopeOpt))
            {
                int i = 0;
                foreach (DbCommand cmd in Commands)
                {
                    cnt[i++] = LibraryDataBase.ExecuteNonQuery(cmd);
                    cmd.Dispose();
                }
                scope.Complete();
            }
            Commands.Clear();
            Commands = null;
            MultipleCommands = false;
            return cnt;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns command for add parameters methods
        /// </summary>
        /// <returns></returns>
        private DbCommand GetCommand()
        {
            return MultipleCommands ? Commands[Commands.Count - 1] : Command;
        }
        /// <summary>
        /// Maps enum connection to string constant.
        /// </summary>
        /// <param name="dbConnect">Connection string reresentation defined in config file</param>
        /// <returns>String equivalent of enum.</returns>
        private string GetDB_Connection(DatabaseConnectionsEnum dbConnect)
        {
            switch (dbConnect)
            {
                case DatabaseConnectionsEnum.Logging:
                    return Constants.DBConnections.Logging;
                case DatabaseConnectionsEnum.PvScout:
                default:
                    return Constants.DBConnections.Solar;
            }
        }

        #endregion
    }

}
