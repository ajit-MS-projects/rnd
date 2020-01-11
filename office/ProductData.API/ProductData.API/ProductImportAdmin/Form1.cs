using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Affilinet.Business.ProductImport.Download;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Data.Access;
using System.Transactions;

namespace ProductImportAdmin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DocumentAttributes objDocAtt = new DocumentAttributes();
            objDocAtt.FileSourceURI = "http://www.raddiscount.de/cgi-bin/partner/getdb.pl?database=affilinet_243.csv&partnerid=651";
            objDocAtt.FileDestination = "D:\\Ajit\\abc1.csv";
            objDocAtt.DocumentEncoding = Encoding.Default;
            DownloadManager objDnLoadMgr = new DownloadManager();
            objDnLoadMgr.DocAttribList.Add(objDocAtt);
            objDnLoadMgr.DownloadDocuments();
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            IAffiliDatabase db = new AffiliGenericDataBase(DatabaseConnectionsEnum.LocalMachine);
            db.SetupCommand("GetProductData");
            dataGridView1.DataSource = db.ExecuteReaderProcessed();//db.ExecuteDataset().Tables[0];
            //db.Command.Connection.Close();
            //db.Command.Connection.Dispose();
            //MessageBox.Show("data 1");
            db.SetupCommand("GetProductData1");
            dataGridView1.DataSource = db.ExecuteReaderProcessed();

            db.SetupCommand("inProduct");
            db.AddInParameter("val1",DbType.String,"330");
            //db.AddInParameter("val2", DbType.String, "Ajit");
            db.AddParameter("val2", DbType.String, ParameterDirection.Input, "Rahul");
            db.AddParameter("val3", DbType.Int16, ParameterDirection.InputOutput,234);
            db.AddParameter("val4", DbType.Int16, ParameterDirection.ReturnValue);
            //db.AddOutParameter("val3", DbType.Int16,5);
           // db.SetParameterValue("val2", "Sachin");
            db.ExecuteNonQuery();
            MessageBox.Show(db.GetParameterValue("val3").ToString());
            MessageBox.Show(db.GetParameterValue("val4").ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IAffiliDatabase db = new AffiliGenericDataBase(DatabaseConnectionsEnum.LocalMachine);
            db.SetupCommand("inProduct");
            db.AddInParameter("val1", DbType.String, "330");
            db.AddParameter("val2", DbType.String, ParameterDirection.Input, "Ajit");
            db.AddParameter("val3", DbType.Int16, ParameterDirection.InputOutput, 234);


            db.SetupMultipleCommands("inProduct");
            db.AddInParameter("val1", DbType.String, "330");
            db.AddParameter("val2", DbType.String, ParameterDirection.Input, "Rahulpp");
            db.AddParameter("val3", DbType.Int16, ParameterDirection.InputOutput, 555);
            //using (TransactionScope scope = new TransactionScope())
            //{
            //    db.ExecuteNonQuery();
            //    db.SetupCommand("inProduct");
            //    db.AddInParameter("val1", DbType.String, "330");
            //    db.AddParameter("val2", DbType.String, ParameterDirection.Input, DBNull.Value);
            //    db.AddParameter("val3", DbType.Int16, ParameterDirection.InputOutput, 555);
            //    //db.SetParameterValue("val1",  "331");
            //    //db.SetParameterValue("val2",   DBNull.Value);
            //    //db.SetParameterValue("val3", 777);
            //    db.ExecuteNonQuery();
            //    scope.Complete();
            //}
            db.SetupMultipleCommands("inProduct");
            db.AddInParameter("val1", DbType.String, "330");
            db.AddParameter("val2", DbType.String, ParameterDirection.Input, "Vishal");
            db.AddParameter("val3", DbType.Int16, ParameterDirection.InputOutput, 555);
            db.ExecuteNonQuery();
            db.ExecuteMultipleNonQuery(true);
        }
    }
}
