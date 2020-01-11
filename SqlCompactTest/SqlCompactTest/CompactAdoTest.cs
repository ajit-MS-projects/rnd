using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Solar.Data.Access;

namespace SqlCompactTest
{
    public partial class CompactAdoTest : Form
    {
        public CompactAdoTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SolarSqlCompactDataBase db = new SolarSqlCompactDataBase(DatabaseConnectionsEnum.PvScout);
            db.SetupCommand("select * from Manufacturers");
            DataTable dt = db.ExecuteReaderProcessed();
            dataGridView1.DataSource = dt;
        }
    }
}
