﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solr.Utility.Logging;
using Solr.Utility.Logging.Common;

namespace Solr.Exceptions
{
    /// <summary>
    /// Exception class to wrap actual exception, prived methods to create exception logs.
    /// Change History: Version;Date;Changed by;Change description
    /// </summary>
    public class SolrGenericException : SolrBaseException
    {
        private int EventId { get; set; }
        private string _message = "SolrGenericException";

        /// <summary>
        /// constructor that accepts an application event id as parameter.
        /// </summary>
        /// <param name="eventId">Application event id</param>
        public SolrGenericException(int eventId) : this("SolrGenericException", null, eventId) { }
        /// <summary>
        /// constructor that accepts an exception message and an application event id as parameter.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="eventId">Application event id</param>
        public SolrGenericException(string message, int eventId):this(message, null, eventId){ }
        /// <summary>
        /// constructor that accepts an exception message, inner exception and an application event id as parameter.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Exception object of current scope</param>
        /// <param name="eventId">Application event id</param>
        public SolrGenericException(string message, Exception innerException, int eventId) : base(message, innerException)
        {
            this.EventId = eventId;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SolrGenericException() : this(0) { }
        /// <summary>
        /// Construction that accepts an exception message.
        /// </summary>
        /// <param name="message">Exception message</param>
        public SolrGenericException(string message) : this(message, null, 0) { }
        /// <summary>
        /// Construction that accepts an exception message and inner exception.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Exception object of current scope</param>
        public SolrGenericException(string message, Exception innerException) : this(message, innerException, 0) { }
        /// <summary>
        /// Create an exception log, message must be set in constructor or default message is logged.
        /// </summary>
        public override void CreateLog()
        {
            CreateLog(string.IsNullOrEmpty(base.Message) ? _message : base.Message);
        }
        /// <summary>
        /// Create an exception log
        /// </summary>
        /// <param name="message">Exception message to be logged.</param>
        public override void CreateLog(string message)
        {
            CreateLog(message, EventId);
        }
        /// <summary>
        /// Create an exception log
        /// </summary>
        /// <param name="message">Exception message to be logged.</param>
        /// <param name="eventId">An id designating a specific event of the application.</param>
        public override void CreateLog(string message, int eventId)
        {
            BaseLogger objLogger = new GenericLogger();
            LoggingInfo objLogInfo = new LoggingInfo();
            objLogInfo.Message = base.Message;
            objLogInfo.ExceptionObject = this;
            objLogInfo.EventId = eventId;
            if (base.InnerException == null || base.InnerException.GetType() == typeof(SolrGenericException))
                objLogger.CreateLog(objLogInfo, LoggingCategoriesEnum.CustomException);
            else
                objLogger.CreateLog(objLogInfo, LoggingCategoriesEnum.SystemException);
        }
    }
}
