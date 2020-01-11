using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affilinet.Business.ProductExport.Entity
{
    public class ProgramSettings
    {
        /// </summary>
        /// <value>The image num to use.</value>
        public int ImageNumToUse { get; set; }

        /// </summary>
        /// <value>The plateform of the program.</value>
        public int PlatformID { get; set; }
        public bool DiURL { get; set; }
        public int TextLinkNb { get; set; }
        public int ProgramID { get; set; }
    }
}
