using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loading
{
    internal class VersionElement
    {
        public string Name { get; set; }
        public Type Typ { get; set; }
        public double VonVersion { get; set; }
        public double? BisVersion { get; set; }
    }
}
