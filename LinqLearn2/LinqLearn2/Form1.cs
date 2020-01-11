using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace LinqLearn2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            Func<int, int, int> function1 = (a, b) => a + b;
            Expression<Func<int, int, int>> expression = (a, b) => a + b;
            Expression<Func<int, int>> expression1 = b => b;

            MessageBox.Show(expression.Compile()(5,7).ToString());
            MessageBox.Show(function1(2,3).ToString());

            BinaryExpression body = (BinaryExpression)expression.Body;
            ParameterExpression left = (ParameterExpression)body.Left;
            ParameterExpression right = (ParameterExpression)body.Right;

            textBox1.Text = expression.Body.ToString() + Environment.NewLine;
            textBox1.Text  += string.Format(" The left part of the expression: " +
              "{0}{4} The NodeType: {1}{4} The right part: {2}{4} The Type: {3}{4}",
              left.Name, body.NodeType, right.Name, body.Type, Environment.NewLine);
        }
    }
}
