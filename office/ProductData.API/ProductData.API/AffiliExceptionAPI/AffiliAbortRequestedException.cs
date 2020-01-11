using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Exceptions
{
    public class AffiliAbortRequestedException : AffiliGenericException
    {
        /// <summary>
        /// constructor that sets a default message
        /// </summary>
        public AffiliAbortRequestedException() : base("Abort requested by admin", null) { }
        /// <summary>
        /// constructor that sets a cutstom message
        /// </summary>
        public AffiliAbortRequestedException(String message) : base(message, null) { }
    }
}
