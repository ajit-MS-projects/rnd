using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solar.Pvscout.Business.Entity
{
    public class PvModuleActual
    {
        public String PvScoutArticleNumber { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public String ArticleNumber { get; set; }
        public String ManufacturerId { get; set; }
        public String ImagePath { get; set; }
        public int Thickness { get; set; }
        public float Weight { get; set; }
        public String CellTechnology { get; set; }
        public String CellMaterial { get; set; }
    }
}
