using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solar.Pvscout.Business.Entity
{
    public abstract class BaseLayer
    {
        public abstract double Width { get; set; }
    }
    public class PvModuleLayer:BaseLayer
    {
        public override double Width { get; set; }
    }
}


// ui to bl roof params
// poosiblity to calculate