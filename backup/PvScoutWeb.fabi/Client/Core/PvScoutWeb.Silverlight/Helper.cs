using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PvScoutWeb.Silverlight.Service;
using System.Collections.Generic;
using PvScoutWeb.Silverlight.Views;

namespace PvScoutWeb.Silverlight
{
    public static class Helper
    {
        public static double[] ConvertToRoofSize(string width, string height)
        {
            double _width;
            double _height;
            double[] roofSize = new double[2]{0,0};
            if(double.TryParse(width,out _width) &&  double.TryParse(height, out _height))
            {
                roofSize[0] = _width;
                roofSize[1] = _height;
            }
            return roofSize;
        }

        public static List<PvModulView> GetModulsFromService()
        {
            MockPvScoutService pvScoutService = new MockPvScoutService();
            // mapp the Modul entity to the Modul control
            List<Model.PvModul> listPvModuls = pvScoutService.GetPvModuls();

            List<PvModulView> listPvModulsView = new List<PvModulView>(listPvModuls.Count);
 
            foreach(Model.PvModul m in listPvModuls)
            {
                PvModulView mView = new PvModulView();
                mView.Name = "PvModul_" + m.Id;
                mView.Width = m.Width;
                mView.Height = m.Height;
                mView.SetValue(Canvas.TopProperty, m.Top);
                mView.SetValue(Canvas.LeftProperty, m.Left);

                listPvModulsView.Add(mView);
            }

            return listPvModulsView;
        }
    }
}
