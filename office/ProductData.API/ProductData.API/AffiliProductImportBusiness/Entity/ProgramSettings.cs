using System.Text;

namespace Affilinet.Business.ProductImport.Entity
{
    public class ProgramSettings
    {
        public string Title { get; set;}
        public string ImportSchemaID { get; set;}   
        public string ProgramID { get; set;}        
        public string Description { get; set;}        
        public string FieldSeperator { get; set;}        
        public char FldSep { get; set;}
        public char TextQual
        {
            get
            {
                if (!string.IsNullOrEmpty(TextQualifier))
                    return TextQualifier[0];
                else
                    return '\0';
            }
        }   
        public string LineSeperator { get; set;}        
        public string TextQualifier { get; set;}        
        public string DateFormat { get; set;}        
        public string DecimalSymbol { get; set;}        
        public string CSVHeadline { get; set;}        
        public string CategorySeperator { get; set;}        
        public bool Active { get; set;}
        /// <summary>
        /// Gets or sets the no. of header lines in CSV.
        /// </summary>
        /// <value>The headlines.</value>
        public int Headlines { get; set;}        
        public string ImportType { get; set;}
        /// <summary>
        /// Gets or sets the directory where processed files will be stored.
        /// </summary>
        /// <value>The processed file path.</value>
        public string ProcessedFilePath { get; set; }//Path = Server.MapPath("files/" + ProgramID + "/");        
        public string DigitGroupSymbol { get; set; }
        public string DefaultAffiliCat { get; set; }
        /// <summary>
        /// A product can be asociated with 3 images this field in ProductProgram defines,
        /// Gets or sets the image number to use.
        /// </summary>
        /// <value>The image num to use.</value>
        public int ImageNumToUse { get; set; }
        /// <summary>
        /// Gets or sets the encoding.
        /// </summary>
        /// <value>The encoding.</value>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// Gets or sets the XML product tag.
        /// </summary>
        /// <value>The XML product tag.</value>
        public string XmlProductTag { get; set; }

        public int PlatformID { get; set; }
        /// <summary>
        /// Gets or sets the EncodeDeeplink.
        /// </summary>
        /// <value>The EncodeDeeplink.</value>
        public int EncodeDeeplink { get; set; }

    }
}