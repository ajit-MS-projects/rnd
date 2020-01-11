using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Solar.Utility.Logging.DAO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Reflection;
using Solar.Utility.Logging.Entity;

namespace Solar.Utility.Logging
{
    /// <summary>
    /// Custome trace listner, it creates log entries into reportLog table
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class DBTraceListener : CustomTraceListener
    {
        /// <summary>
        /// Writes trace information, a data object and event information to report log table.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The trace data to emit.</param>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/>
        /// </PermissionSet>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data != null && data is LogEntry)
            {
                LogEntry objLogEntry = (LogEntry)data;
                if (objLogEntry.ExtendedProperties != null && objLogEntry.ExtendedProperties.Count > 0)
                {//Report log are as KeyValuePair collection object in objLogEntry.ExtendedProperties, Extracting
                    using (LogDAO objdao = new LogDAO())
                    {
                        foreach (object logObject in objLogEntry.ExtendedProperties)
                        {
                            if (logObject is KeyValuePair<string, object> &&
                                ((KeyValuePair<string, object>) logObject).Value is List<DatabaseLog>)
                            {
                                List<DatabaseLog> logs =
                                    (List<DatabaseLog>) ((KeyValuePair<string, object>) logObject).Value;
                                for (int i = 0; i < logs.Count; i++)
                                {//Loop through all log parameters and log in a transaction.
                                    DatabaseLog log = logs[i];
                                    objdao.CreateDatabaseLog(i == logs.Count - 1, log.LogText, log.LogCategory);
                                    log.Dispose();
                                }
                                logs.Clear();
                                logs = null;
                            }
                        }
                    }
                }
            }
        }
        public override void Write(string message){}

        public override void WriteLine(string message){}
    }
   
}
