using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solar.Data.Access;

namespace DatabaseTests
{
    public partial class Form1 : Form
    {
        private ISolarDatabase _pvscoutDatabase;
        private ISolarDatabase PvscoutDatabase
        {
            get
            {
                if (_pvscoutDatabase == null)
                    _pvscoutDatabase = new SolarGenericDataBase(DatabaseConnectionsEnum.PvScout);
                return _pvscoutDatabase;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSqlMdfFile_Click(object sender, EventArgs e)
        {
            //System.Data.sql
            DataTable dtTable = GetRoofType("0");
            foreach (DataRow dr in dtTable.Rows)
            {
                int RoofTypeId = int.Parse(dr["RoofTypeId"].ToString());
                string DisplayName = dr["DisplayName"].ToString();
                txtOutput.Text = RoofTypeId.ToString() + Environment.NewLine;
                txtOutput.Text = DisplayName;
            }
        }
        public DataTable GetRoofType(String roofTypeId)
        {
            DataTable dtRetVal = null;

            PvscoutDatabase.SetupCommand("GetRoofTypes");
            PvscoutDatabase.AddInParameter("RoofTypeId", DbType.String, "0");

            dtRetVal = PvscoutDatabase.ExecuteReaderProcessed();

            return dtRetVal;
        }
    }
}
