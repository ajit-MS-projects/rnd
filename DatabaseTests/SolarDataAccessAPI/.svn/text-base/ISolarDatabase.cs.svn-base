using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Solar.Data.Access
{
    public interface ISolarDatabase
    {

        #region Properties
        /// <summary>
        /// Dbcommand object to store stored procedure command.
        /// </summary>
        DbCommand Command { get; set; }
        /// <summary>
        /// Holds instance of MS enterprise library Database class
        /// </summary>
        Database LibraryDataBase { get; }
        /// <summary>
        /// This property hold multiple commands for transactional or non transactional DMLs
        /// </summary>
        List<DbCommand> Commands { get; set; }
        /// <summary>
        /// This property is Automatically set to true once SetupMultipleCommands is called, 
        /// it's significance is that afterwards all parameters are added to the latest of multiple commands.
        /// </summary>
        bool MultipleCommands { get; set; }
        #endregion

        #region Parameters
        /// <summary>
        /// This method adds and input parameter to SP command
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dbType">Database data type</param>
        /// <param name="value">Parameter value</param>
        void AddInParameter(string name, DbType dbType, object value);
        /// <summary>
        /// This method adds and output parameter to SP command
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dbType">Database data type</param>
        /// <param name="size">Width in chars or Size of parameter</param>
        void AddOutParameter(string name, DbType dbType, int size);
        /// <summary>
        /// This method passes a parameter (input, output, InputOutput, return) & a value to a stored procedure.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dbType">Database data type</param>
        /// <param name="direction">Input/Output/InputOutput/return direction of SP parameter</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, DbType dbType, ParameterDirection direction, object value);
        /// <summary>
        /// This method passes a parameter (output, InputOutput, return) to a stored procedure.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dbType">Database data type</param>
        /// <param name="direction">Output/InOut/return direction of SP parameter</param>
        void AddParameter(string name, DbType dbType, ParameterDirection direction);
        /// <summary>
        ///  This method sets the value of the specified parameter when one wants to execute multiple inserts using the same connection and command but with different parameter values.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void SetParameterValue(string name, object value);
        /// <summary>
        /// This method finds the value of the specified parameter including(return).
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Value of the out parameter</returns>
        object GetParameterValue(string name);
        #endregion

        #region Execute Command
        /// <summary>
        /// Initializes sql string DBcommand object.
        /// </summary>
        /// <param name="sql">Stored procedure name</param>
        void SetupSqlCommand(string sql);
        /// <summary>
        /// Sets up a SP command object for use in subsequent requests.
        /// </summary>
        /// <param name="spName">Name of stored procedure to execute</param>
        void SetupCommand(string spName);
        /// <summary>
        /// Sets up a SP command object and adds to list for use in subsequent transactional requests.
        /// </summary>
        /// <param name="spName">Name of stored procedure to execute</param>
        void SetupMultipleCommands(string spName);
        /// <summary>
        /// Executes command for a dataset.
        /// </summary>
        /// <returns>Returns a dataset.</returns>
        DataSet ExecuteDataset();
        /// <summary>
        /// Executes command to return DataReader, Note: one must explicitly close associated connection after using DataReader.
        /// </summary>
        /// <returns>IDataReader object with live connection</returns>
        IDataReader ExecuteReader();
        /// <summary>
        /// Executes a command for datareader.
        /// </summary>
        /// <returns>Datatable object (with a closed conection.)</returns>
        DataTable ExecuteReaderProcessed();
        /// <summary>
        /// Executes a DML/DDL
        /// </summary>
        /// <returns>No.s of rows affected</returns>
        int ExecuteNonQuery();
        /// <summary>
        /// Executes multiple DMLs/DDLs in transactional/ non-transactional mode.
        /// </summary>
        /// <returns>No.s of rows affected</returns>
        int[] ExecuteMultipleNonQuery(bool transactional);

        /// <summary>
        /// Executes command for a scalar query.
        /// </summary>
        /// <returns>Returns a dataset.</returns>
        object ExecuteScalar();
        #endregion

       
    }
}
