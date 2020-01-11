using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solr.Exceptions
{
    public class SolrInvalidInputException : SolrGenericException
    {
        /// <summary>
        /// constructor that sets a default message
        /// </summary>
        public SolrInvalidInputException() : base("Input parameters are invalid.", null) { }
        /// <summary>
        /// constructor that sets a cutstom message
        /// </summary>
        public SolrInvalidInputException(String message) : base(message, null) { }
    }
}
