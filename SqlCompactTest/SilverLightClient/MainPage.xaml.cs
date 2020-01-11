using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverLightClient.ProductServiceRef;

namespace SilverLightClient
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            //BasicHttpBinding binding = new BasicHttpBinding();
            //EndpointAddress ed = new EndpointAddress("http://localhost:9010/ProductService");
            //ProductServiceRef.ManufacturerServiceClient proxy = new ProductServiceRef.ManufacturerServiceClient(binding, ed);
            //proxy.GetAllManufacturersCompleted += new EventHandler<ProductServiceRef.GetAllManufacturersCompletedEventArgs>(client_GetAllManufacturersCompleted);
            //proxy.GetAllManufacturersAsync();    

            ManufacturerServiceClient client = new ManufacturerServiceClient();
            client.GetAllManufacturersCompleted += new EventHandler<GetAllManufacturersCompletedEventArgs>(client_GetAllManufacturersCompleted);
            client.GetAllManufacturersAsync();
        }

        void client_GetAllManufacturersCompleted(object sender, GetAllManufacturersCompletedEventArgs e)
        {

            grdData.ItemsSource = e.Result;

        }
    }
}
