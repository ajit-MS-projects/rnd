using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PackMachines
{
    public abstract class Machine
    {
        public int Cores { get; set; }
        public int BandWidth { get; set; }
        public int Memory { get; set; }
    }
    public class VirtualMachine: Machine
    {
        
    }
    public class PhysicalMachine:Machine
    {
        public List<VirtualMachine> VirtualMachines { get; set; }
        public int CountOfVirtualMachines { get { return VirtualMachines.Count; } }
        public PhysicalMachine()
        {
            VirtualMachines=new List<VirtualMachine>();
        }
    }
    public class Packer
    {
        public List<PhysicalMachine> PackVirtualMachines(List<VirtualMachine> virtualMachines, PhysicalMachine inputPhysicalMachine)
        {
              var retVal = new List<PhysicalMachine>();
              var tmpPhyMachine = new PhysicalMachine();
              retVal.Add(tmpPhyMachine);
              foreach (var virtualMachine in virtualMachines)
              {
                  if (!(tmpPhyMachine.Cores < inputPhysicalMachine.Cores && tmpPhyMachine.BandWidth < inputPhysicalMachine.BandWidth && tmpPhyMachine.Memory < inputPhysicalMachine.Memory))
                  {
                      tmpPhyMachine = new PhysicalMachine();
                      retVal.Add(tmpPhyMachine);
                  }
                  tmpPhyMachine.Cores += virtualMachine.Cores;
                  tmpPhyMachine.BandWidth += virtualMachine.BandWidth;
                  tmpPhyMachine.Memory += virtualMachine.Memory;
                  tmpPhyMachine.VirtualMachines.Add(virtualMachine);
              }

            return retVal;
        }
    }
}
