using System;
using Affilinet.Business.ProductImport.Download;
using Affilinet.Business.ProductImport.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Affilinet.Data.Access;
using System.Data;
using System.Text;

namespace TestProductImport
{


    /// <summary>
    ///This is a test class for DownloadManagerTest and is intended
    ///to contain all DownloadManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DownloadManagerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion



        //public void DownloadHTTPTest()
        //public void GetWebRequestTest()
        //public void WriteToFileTest()
        //public void ReadDocumentTest()

        //public void DownloadFtpTest()
        //public void GetFtpWebRequestTest()
        //public void SanitizeFtpUrITest()
        //public void UnzipZipFileTest()
        //public void UnzipGZipFileTest()
        //public void UnzipFileTest()

        //public void DownloadHTTPZipTest()
        //public void GetWebClientTest()
        //public void UnzipZipFileTest()
        //public void UnzipGZipFileTest()
        //public void UnzipFileTest()

        public enum ProgIdsEnum
        {
            HttpNa = 250,
            HttpAu = 21,
            HttpAuZip = 141, //https
            HttpAuGz = 143,
            FtpAuZip = 239,
            FtpAuGz = 330,
            HttpXml = 0,
            FtpXml = 0,
            HttpInvSrc = 17,
            HttpInvDirectory = 2,
            HttpInvCred = 23,
            HttpInvZip = 0,
            FtpInvSrc = 402,
            FtpInvCred = 490
        }
        /// <summary>
        ///A test for DownloadDocumentsAsynch
        ///</summary>
        //[TestMethod()]
        public void DownloadDocumentsAsynchTestHttpNa()
        {
            DownloadManager target = new DownloadManager();
            target.DocAttribList.Add(GetDocAttObject((int)ProgIdsEnum.HttpNa, 0));
            target.DownloadDocumentsAsynch();
            //Wating for child thread to finish job
            //only needed in test method
            System.Threading.Thread.Sleep(1000 * 10);
        }

        /// <summary>
        ///A test for DownloadDocuments
        ///</summary>
        [TestMethod()]
        public void DownloadDocumentsTestHttpNa()
        {
            DownloadDocumentsTest((int)ProgIdsEnum.HttpNa, 0);
        }
        [TestMethod()]
        public void DownloadDocumentsTestHttpAu()
        {
            DownloadDocumentsTest((int)ProgIdsEnum.HttpAu, (int)ProgIdsEnum.HttpAuZip);
        }
        [TestMethod()]
        public void DownloadDocumentsTestHttpAuZip()
        {
            DownloadDocumentsTest((int)ProgIdsEnum.HttpAuZip, (int)ProgIdsEnum.HttpAuZip);
        }
        [TestMethod()]
        public void DownloadDocumentsTestHttpAuGz()
        {
            DownloadDocumentsTest((int)ProgIdsEnum.HttpAuGz, (int)ProgIdsEnum.HttpAuZip);
        }

        [TestMethod()]
        public void DownloadDocumentsTestFtpAuZip()
        {
            
            DownloadDocumentsTest((int)ProgIdsEnum.FtpAuZip, (int)ProgIdsEnum.HttpAuZip);
        }
        [TestMethod()]
        public void DownloadDocumentsTestFtpAuGz()
        {
            DownloadDocumentsTest((int)ProgIdsEnum.FtpAuGz, (int)ProgIdsEnum.HttpAuZip);
        }
        [TestMethod()]
        public void DownloadDocumentsTestHttpXml()
        {
            throw new NotImplementedException();
        }
        [TestMethod()]
        public void DownloadDocumentsTestFtpXml()
        {
            throw new NotImplementedException();
        }
        [TestMethod()]
        public void DownloadDocumentsTestHttpInvSrc()
        {
            DownloadManager target = new DownloadManager();
            DocumentAttributes da = GetDocAttObject((int)ProgIdsEnum.HttpInvSrc, (int)ProgIdsEnum.HttpNa);
            da.FileSourceURI = "http://xyz.com/abc.zip";
            target.DocAttribList.Add(da);
            target.DownloadDocuments();
        }
        [TestMethod()]
        public void DownloadDocumentsTestHttpInvDirectory()
        {
            DownloadManager target = new DownloadManager();
            DocumentAttributes da = GetDocAttObject((int)ProgIdsEnum.HttpInvDirectory, (int)ProgIdsEnum.HttpNa);
            da.FileDestination = "d:/test1234";
            target.DocAttribList.Add(da);
            target.DownloadDocuments();
        }
        [TestMethod()]
        public void DownloadDocumentsTestHttpInvCred()
        {
            DownloadManager target = new DownloadManager();
            DocumentAttributes da = GetDocAttObject((int)ProgIdsEnum.HttpInvCred, (int)ProgIdsEnum.HttpNa);
            da.UserId = "1234";
            target.DocAttribList.Add(da);
            target.DownloadDocuments();
        }
        [TestMethod()]
        public void DownloadDocumentsTestHttpInvZip()
        {
            throw new NotImplementedException();
        }
        [TestMethod()]
        public void DownloadDocumentsTestFtpInvSrc()
        {
            DownloadManager target = new DownloadManager();
            DocumentAttributes da = GetDocAttObject((int)ProgIdsEnum.FtpInvSrc, (int)ProgIdsEnum.HttpNa);
            da.FileSourceURI = "ftp://xyz.com/abc.zip";
            target.DocAttribList.Add(da);
            target.DownloadDocuments();
        }
        [TestMethod()]
        public void DownloadDocumentsTestFtpInvCred()
        {
            DownloadManager target = new DownloadManager();
            DocumentAttributes da = GetDocAttObject((int)ProgIdsEnum.FtpInvCred, (int)ProgIdsEnum.HttpNa);
            da.UserId = "1234";
            target.DocAttribList.Add(da);
            target.DownloadDocuments();
        }
       
        public void DownloadDocumentsTest(int ProdProgId, int ProdProgId2)
        {
            DownloadManager target = new DownloadManager();
            target.DocAttribList.Add(GetDocAttObject(ProdProgId, ProdProgId2));
            target.DownloadDocuments();
        }

        private DocumentAttributes GetDocAttObject(int ProdProgId, int ProdProgId2)
        {
            IAffiliDatabase database = new AffiliGenericDataBase(DatabaseConnectionsEnum.ProductData);
            DataTable dtTmp = null;

            database.SetupCommand("TestGetScheduledPrograms");

            database.AddInParameter("ProdProgId", DbType.Int16, ProdProgId);
            database.AddInParameter("ProdProgId2", DbType.Int16, ProdProgId2);

            dtTmp = database.ExecuteReaderProcessed();
            if (dtTmp.Rows.Count == 0) throw new Exception("No record fould for prodprogram id:" + ProdProgId);
            DataRow dr = dtTmp.Rows[0];
            DocumentAttributes objDocAtt = new DocumentAttributes
            {
                ProdProgId = ProdProgId,
                FileSourceURI = dr["SourceURL"].ToString(),
                FileDestination = "d:/ProdImportCsv/files/",
                FileName = dr["URL"].ToString().Replace("files/", ""),
                DocumentEncoding = dr["EncodingString"] == DBNull.Value || dr["EncodingString"].ToString().Equals("") ? Encoding.Default : Encoding.GetEncoding(dr["EncodingString"].ToString()),
                UserId = dr["LoginName"] == DBNull.Value ? "" : dr["LoginName"].ToString(),
                Password = dr["Password"] == DBNull.Value ? "" : dr["LoginName"].ToString(),
                ProgName = dr["Name"].ToString(),
                DocumentType = DocumentAttributes.GetDocumentType(dr["FileType"])
            };
            return objDocAtt;
        }

        /// <summary>
        ///A test for DownloadManager Constructor
        ///</summary>
        [TestMethod()]
        public void DownloadManagerConstructorTest1()
        {
            DownloadManager target = new DownloadManager();
        }

        /// <summary>
        ///A test for DownloadManager Constructor
        ///</summary>
        [TestMethod()]
        public void DownloadManagerConstructorTest()
        {
            List<DocumentAttributes> objDocAtts = new List<DocumentAttributes>();
            DownloadManager target = new DownloadManager(objDocAtts);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
