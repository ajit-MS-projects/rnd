using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using winClient.HospitalRef;

namespace winClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            HospitalContext context = new HospitalContext(new Uri("http://localhost:1129/HospitalService.svc/"));

            Patient patient = context.Patients.Where(p => p.Id == 1).SingleOrDefault();
            MessageBox.Show(patient.Name);

            //Patient newPatient = Patient.CreatePatient(3);
            //context.AddToPatients(newPatient);

            context.
        }
    }
}
