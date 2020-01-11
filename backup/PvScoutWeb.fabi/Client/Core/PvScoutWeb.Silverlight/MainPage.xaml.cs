using System.Windows;
using System.Windows.Controls;

namespace PvScoutWeb.Silverlight
{
    public partial class MainPage : UserControl
    {
        
        public MainPage()
        {
            InitializeComponent();
            
        }



        private void btnFillRoof_Click(object sender, RoutedEventArgs e)
        {
            roof.FillRoof();
        }


        private void btnCreateRoof_Click(object sender, RoutedEventArgs e)
        {
            double[] roofSize= Helper.ConvertToRoofSize(txtRoofWidth.Text, txtRoofHeight.Text);

            roof.ChangeRoofSize(roofSize[0], roofSize[1]);
        }





        
    }
}
