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
using System.Windows.Navigation;
using PvScoutWeb.PvScoutService;

namespace PvScoutWeb
{
    public partial class PvScoutBasePage : Page
    {
        public PvScoutBasePage()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            PvScoutServiceClient objClient = new PvScoutServiceClient();
            objClient.GetPvModulePositionCompleted += new EventHandler<GetPvModulePositionCompletedEventArgs>(objClient_GetPvModulePositionCompleted);
            objClient.GetPvModulePositionAsync(new PvModuleActual() { Width = 5, Height = 10 });

            //objClient.HelloCompleted += new EventHandler<HelloCompletedEventArgs>(objClient_HelloCompleted);
            //objClient.HelloAsync();

        }
        private void objClient_GetPvModulePositionCompleted(object sender, GetPvModulePositionCompletedEventArgs args)
        {
            PvModule objPvModule = args.Result;
            txtResult.Text = String.Format("Virtual:: width={0} height={1}", objPvModule.ObjPvModuleVirtual.Width.ToString(), objPvModule.ObjPvModuleVirtual.Height.ToString());
        }

        
        private void objClient_HelloCompleted(object sender, HelloCompletedEventArgs args)
        {
            String result = args.Result;
            txtResult.Text = String.Format("Virtual:: width={0} ", result);
        }
    }
}
