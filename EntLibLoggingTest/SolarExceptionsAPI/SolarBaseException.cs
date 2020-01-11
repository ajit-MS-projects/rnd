using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solar.Exceptions
{
    /// <summary>
    /// Base class of all Solar exceptions
    /// Change History: Version;Date;Changed by;Change description
    /// </summary>
    public abstract class SolarBaseException : System.Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SolarBaseException() : base() { }
        /// <summary>
        /// Constructor that accepts a message describing exception.
        /// </summary>
        /// <param name="message">Message describing exception</param>
        public SolarBaseException(string message) : base(message) { }
        /// <summary>
        /// Constructor that accepts a message describing exception and inner exception object.
        /// </summary>
        /// <param name="message">Message describing exception</param>
        /// <param name="innerException">Exception object in current scope.</param>
        public SolarBaseException(string message, Exception innerException): base(message,innerException) { }

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
