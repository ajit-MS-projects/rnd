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

        private void btnGetPosition_Click(object sender, RoutedEventArgs e)
        {
            PvScoutServiceClient objClient = new PvScoutServiceClient();
            objClient.GetPvModulePositionCompleted+=new EventHandler<GetPvModulePositionCompletedEventArgs>(objClient_GetPvModulePositionCompleted);
            objClient.GetPvModulePositionAsync(new PvModuleActual() { Width = 5, Height = 10 });
        }
        private void objClient_GetPvModulePositionCompleted(object sender, GetPvModulePositionCompletedEventArgs args)
        {
            PvModule objPvModule = args.Result;
            txtResult.Text = String.Format("Virtual:: width={0} height={1}", objPvModule.ObjPvModuleVirtual.Width.ToString(), objPvModule.ObjPvModuleVirtual.Height.ToString());
        }
    }
}
