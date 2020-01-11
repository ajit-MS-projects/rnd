using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Solr.Utility.Logging.Common;
using Solr.Utility.Logging.Entity;

namespace Solr.Utility.Logging
{
    /// <summary>
    /// Common implementation for creating log entries. It is an interface over MS Looging block, and allows various log destinations using config file.
    /// </summary>
  public class GenericLogger : BaseLogger
    {
        #region Public Methods
        /// <summary>
        /// Methods uses MS Enterprise libray's logger object to generate Log.
        /// </summary>
        /// <param name="objLoggingInfo">Object of Logging info to store various log parameters.</param>
        /// <param name="loggingLevel">One of the log categories defined in configuration file.</param>
        public override void CreateLog(LoggingInfo objLoggingInfo, LoggingCategoriesEnum loggingLevel)
        {
            switch (loggingLevel)
            {
                case LoggingCategoriesEnum.Debug:
                    SetupLoggingInfoObject(objLoggingInfo, 1, TraceEventType.Verbose);
                    objLoggingInfo.Category.Add(Constants.LogCategories.Debug);
                    break;
                case LoggingCategoriesEnum.Information:
                    SetupLoggingInfoObject(objLoggingInfo, 2, TraceEventType.Information);
                    objLoggingInfo.Category.Add(Constants.LogCategories.Information);
                    break;
                case LoggingCategoriesEnum.Warning:
                    SetupLoggingInfoObject(objLoggingInfo, 3, TraceEventType.Warning);
                    objLoggingInfo.Category.Add(Constants.LogCategories.Warning);
                    break;
                case LoggingCategoriesEnum.CustomException:
                    SetupLoggingInfoObject(objLoggingInfo, 4, TraceEventType.Error);
                    objLoggingInfo.Category.Add(Constants.LogCategories.CustomException);
                    break;
                case LoggingCategoriesEnum.SystemException:
                    SetupLoggingInfoObject(objLoggingInfo, 5, TraceEventType.Critical);
                    objLoggingInfo.Category.Add(Constants.LogCategories.SystemException);
                    break;
                case LoggingCategoriesEnum.Email:
                    SetupLoggingInfoObject(objLoggingInfo, 1, TraceEventType.Information);
                    objLoggingInfo.Category.Add(Constants.LogCategories.Email);
                    break;
            }
            WriteToLog(objLoggingInfo);
        }
        public override void CreateDatabaseLog(List<DatabaseLog> databaseLogs)
        {
            if (databaseLogs != null && databaseLogs.Count > 0)
            {
                LogEntry objLogEntry = new LogEntry();
                objLogEntry.ExtendedProperties.Add(Constants.LogCategories.Database, databaseLogs);
                Logger.Write(objLogEntry, Constants.LogCategories.Database, objLogEntry.ExtendedProperties);
            }
        }

        #endregion
      #region Private Methods
      /// <summary>
      /// Sets up common properties of LoggingInfo object.
      /// </summary>
      /// <param name="objLoggingInfo"></param>
      /// <param name="priority"></param>
      /// <param name="severity"></param>
      private void SetupLoggingInfoObject(LoggingInfo objLoggingInfo, int priority, TraceEventType severity)
      {
          objLoggingInfo.Priority = priority;
          objLoggingInfo.Severity = severity;
      }

      /// <summary>
      /// Sets up properties of MS enterprise library Logger object & write to log.
      /// </summary>
      /// <param name="objLoggingInfo"></param>
      private void WriteToLog(LoggingInfo objLoggingInfo)
      {
          LogEntry objLogEntry = new LogEntry();
          objLogEntry.Message = objLoggingInfo.Message;
          objLogEntry.Priority = objLoggingInfo.Priority;
          objLogEntry.Severity = objLoggingInfo.Severity;
          objLogEntry.Categories = objLoggingInfo.Category;
          objLogEntry.EventId = objLoggingInfo.EventId;
          objLogEntry.ExtendedProperties = GetExceptionStackTrace(objLoggingInfo);
          Logger.Write(objLogEntry);

      }

      /// <summary>
      /// Extracts Exception generating Method, Namespace & stack trace of exception to be added to log.
      /// </summary>
      /// <param name="objLoggingInfo"></param>
      /// <returns></returns>
      private IDictionary<string, object> GetExceptionStackTrace(LoggingInfo objLoggingInfo)
      {
          IDictionary<string, object> objdiction = new Dictionary<string, object>();
          if (objLoggingInfo.ExceptionObject != null)
          {
              string strTmp = string.Empty;
              if (objLoggingInfo.ExceptionObject.TargetSite != null)
              {
                  strTmp = objLoggingInfo.ExceptionObject.TargetSite.Name.ToString();
                  objdiction.Add("[Method]", strTmp + " ");
                  if (objLoggingInfo.ExceptionObject.TargetSite.DeclaringType != null && objLoggingInfo.ExceptionObject.TargetSite.DeclaringType.FullName != null)
                  {
                      strTmp = objLoggingInfo.ExceptionObject.TargetSite.DeclaringType.FullName.ToString();
                      objdiction.Add("[Namespace]", strTmp + " ");
                  }
              }

              strTmp = objLoggingInfo.ExceptionObject.Message.ToString();
              objdiction.Add("[ErrorDescr]", strTmp + " ");

              strTmp = objLoggingInfo.ExceptionObject.ToString();
              strTmp = strTmp.Replace("<", "");
              strTmp = strTmp.Replace(">", "");
              strTmp = strTmp.Replace("&", "");
              strTmp = strTmp.Replace(";", "");
              objdiction.Add("[Stack]", strTmp + " ");
          }
          return objdiction;
      } 
      #endregion
    }
}
