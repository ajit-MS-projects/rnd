using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solar.Exceptions
{
    public class SolarInvalidInputException : SolarGenericException
    {
        /// <summary>
        /// constructor that sets a default message
        /// </summary>
        public SolarInvalidInputException() : base("Input parameters are invalid.", null) { }
        /// <summary>
        /// constructor that sets a cutstom message
        /// </summary>
        public SolarInvalidInputException(String message) : base(message, null) { }
    }
}
