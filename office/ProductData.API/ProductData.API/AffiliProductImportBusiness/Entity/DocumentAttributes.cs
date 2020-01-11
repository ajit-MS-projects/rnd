using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affilinet.Business.ProductImport.Common;

namespace Affilinet.Business.ProductImport.Entity
{
    /// <summary>
    /// An entity class to store all file attributes to download and process it.
    /// </summary>
    public class DocumentAttributes
    {
        #region Private variables
        private string fileSourceURI;
        private string userId; 
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the prod prog id.
        /// </summary>
        /// <value>The prod prog id.</value>
        public int ProdProgId { get; set; }
        /// <summary>
        /// Gets or sets the name of the prog.
        /// </summary>
        /// <value>The name of the prog.</value>
        public string ProgName { get; set; }
        /// <summary>
        /// Gets or sets the file source URI.
        /// </summary>
        /// <value>The file source URI.</value>
        public string FileSourceURI
        {
            get { return fileSourceURI; }
            set
            {
                fileSourceURI = value;
                if (!string.IsNullOrEmpty(fileSourceURI))
                {
                    SourceURLProtocol = SourceURLProtocolEnum.HTTP;
                    if (fileSourceURI.ToLower().StartsWith("ftp://"))
                        SourceURLProtocol = SourceURLProtocolEnum.FTP;
                    else if (!fileSourceURI.ToLower().StartsWith("http://") && !fileSourceURI.ToLower().StartsWith("https://"))
                        fileSourceURI = "http://" + fileSourceURI;

                    string extension = fileSourceURI.Substring(fileSourceURI.Length - 3, 3).ToUpper().Replace(".", "");
                    if (Enum.IsDefined(typeof(FileExtensionsEnum), extension))
                        Extension = (FileExtensionsEnum)Enum.Parse(typeof(FileExtensionsEnum), extension);
                    else
                        Extension = FileExtensionsEnum.CSV;
                }
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether for this instance authentication is required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is authentication required; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticationRequired { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether for this instance file processing is complete.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is file processing complete; otherwise, <c>false</c>.
        /// </value>
        public bool IsFileProcessingComplete { get; set; }
        /// <summary>
        /// Gets or sets the file extension. It gets set automatically when FileSourceURI property is set
        /// </summary>
        /// <value>The extension.</value>
        public FileExtensionsEnum Extension { get; set; }
        /// <summary>
        /// Gets or sets the user id authentication source URL
        /// </summary>
        /// <value>The user id.</value>
        public string UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                if (!string.IsNullOrEmpty(userId)) IsAuthenticationRequired = true;
            }
        }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
        /// <summary>
        /// Full Directory path to store files
        /// </summary>
        public string FileDestination { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }
        /// <summary>
        /// Returns full path of file(uncompressed).
        /// </summary>
        /// <value>The source file full path.</value>
        public string FileFullPath
        {
            get
            {
                return FileDestination + FileName ;
            }
        }
        /// <summary>
        /// Gets or sets the type xml/csv etc. of the document.
        /// </summary>
        /// <value>The type of the document.</value>
        public DocumentTypesEnum DocumentType { get; set; }
        /// <summary>
        /// Gets or sets the document encoding.
        /// </summary>
        /// <value>The document encoding.</value>
        public Encoding DocumentEncoding { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether file download is a success.
        /// </summary>
        /// <value><c>true</c> if [file download success]; otherwise, <c>false</c>.</value>
        public bool FileDownloadSuccess { get; set; }
        /// <summary>
        /// Gets set protocol of source URL. It gets set automatically when FileSourceURI property is set
        /// </summary>
        public SourceURLProtocolEnum SourceURLProtocol;
        /// <summary>
        /// Gets the file extension string.
        /// </summary>
        /// <value>The extension string.</value>
        public string ExtensionString
        {
            get
            {
                return "." + Extension.ToString();
            }
        }
        /// <summary>
        /// Determines the type of the document xml/csv.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <returns></returns>
        public static DocumentTypesEnum GetDocumentType(object fileType)
        {
            DocumentTypesEnum retVal = DocumentTypesEnum.CSV;
            if (fileType != DBNull.Value)
            {
                string strFileType = fileType.ToString();
                //if (Enum.IsDefined(typeof(DocumentTypesEnum), strFileType))
                //    retVal = (DocumentTypesEnum)Enum.Parse(typeof(DocumentTypesEnum), strFileType);
                if(strFileType == "2")
                    retVal = DocumentTypesEnum.XML;
            }
            return retVal;
        }
        /// <summary>
        /// Gets or sets the aggregate programSettings object.
        /// </summary>
        /// <value>The prog settings.</value>
        public ProgramSettings ProgSettings { get; set; }
        /// <summary>
        /// Gets or sets list of program fields for in csv file.
        /// </summary>
        /// <value>The prog fields.</value>
        public List<ProgramFields> ProgFields { get; set; }
        /// <summary>
        /// Gets or sets the manual import.
        /// </summary>
        /// <value>The manual import.</value>
        public ManualImportEnum ManualImport { get; set; }

        /// <summary>
        /// Gets or sets the reporting counters and information object.
        /// </summary>
        /// <value>The report.</value>
        public ReportingInfo Report { get; set; }
        /// <summary>
        /// Gets or sets the download report.
        /// </summary>
        /// <value>The download report.</value>
        public DownloadReportingInfo DownloadReport { get; set; }
        /// <summary>
        /// Gets or sets the ssis report.
        /// </summary>
        /// <value>The ssis report.</value>
        public SsisImportReportingInfo SsisReport { get; set; }
        /// <summary>
        /// Gets or sets the account manager.
        /// </summary>
        /// <value>The account manager.</value>
        public String AccountManager { get; set; }
        /// <summary>
        /// Gets or sets the account manager email.
        /// </summary>
        /// <value>The account manager email.</value>
        public String AccountManagerEmail { get; set; }
        #endregion
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentAttributes"/> class.
        /// </summary>
        public DocumentAttributes()
        {
            ProgFields = new List<ProgramFields>();
            Report = new ReportingInfo();
            DownloadReport = new DownloadReportingInfo();
            SsisReport = new SsisImportReportingInfo();
        } 
        #endregion
    }
}