using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Exceptions
{
    /// <summary>
    /// Base class of all Affili exceptions
    /// Change History: Version;Date;Changed by;Change description
    /// 1.0;15-Feb-2009;Ajit Chahal;-
    /// </summary>
    public abstract class AffiliBaseException : System.Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AffiliBaseException() : base() { }
        /// <summary>
        /// Constructor that accepts a message describing exception.
        /// </summary>
        /// <param name="message">Message describing exception</param>
        public AffiliBaseException(string message) : base(message) { }
        /// <summary>
        /// Constructor that accepts a message describing exception and inner exception object.
        /// </summary>
        /// <param name="message">Message describing exception</param>
        /// <param name="innerException">Exception object in current scope.</param>
        public AffiliBaseException(string message, Exception innerException): base(message,innerException) { }

        /// <summary>
        /// Create an exception log, message must be set in constructor or default message is logged.
        /// </summary>
        public abstract void CreateLog();
        /// <summary>
        /// Create an exception log
        /// </summary>
        /// <param name="message">Exception message to be logged.</param>
        public abstract void CreateLog(string message);
        /// <summary>
        /// Create an exception log
        /// </summary>
        /// <param name="message">Exception message to be logged.</param>
        /// <param name="eventId">An id designating a specific event of the application.</param>
        public abstract void CreateLog(string message, int eventId);
    }
}
