using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PvScoutWeb.PvScoutService;

namespace PvScoutWeb
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

        }



        private void btnFillRoof_Click(object sender, RoutedEventArgs e)
        {
            rvRoof.FillRoof();
        }


        private void btnCreateRoof_Click(object sender, RoutedEventArgs e)
        {
            double[] roofSize = Helper.ConvertToRoofSize(txtRoofWidth.Text, txtRoofHeight.Text);

            rvRoof.ChangeRoofSize(roofSize[0], roofSize[1]);
        }

    }
}
