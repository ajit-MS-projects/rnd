using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Affilinet.Utility.Logging
{
    /// <summary>
    /// Class is an entity and represents log entry instance.
    /// </summary>
    public class LoggingInfo
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message {get;set;}
        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string MethodName {get;set;}
        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public string ClassName {get;set;}
        /// <summary>
        /// Gets or sets the name of the sp.
        /// </summary>
        /// <value>The name of the sp.</value>
        public string SpName {get;set;}
        /// <summary>
        /// Gets or sets the event id.
        /// </summary>
        /// <value>The event id.</value>
        public int EventId {get;set;}
        /// <summary>
        /// Gets or sets the exception object.
        /// </summary>
        /// <value>The exception object.</value>
        public Exception ExceptionObject { get; set; }
        internal TraceEventType Severity{get;set;}
        internal ICollection<String> Category {get;set;}
        internal int Priority {get;set;}
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingInfo"/> class.
        /// </summary>
        public LoggingInfo()
        {
            Category = new List<String>();
        }
    }

    
}
