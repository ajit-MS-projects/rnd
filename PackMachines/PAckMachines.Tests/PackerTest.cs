using PackMachines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PackMachines.Tests
{
    [TestClass()]
    public class PackerTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod()]
        public void PackVirtualMachinesTest_Bandwidth()
        {
            var inputVirtualMachine = new VirtualMachine() { BandWidth = 2, Cores = 4, Memory = 16 };
            int noOfVirtualMachinesToBeAllocated = 11;
            var inputPhysicalMachine = new PhysicalMachine() { BandWidth = 10, Cores = 64, Memory = 256 };
            int expectedMaxVmsPerPhysicalMachine = inputPhysicalMachine.BandWidth / inputVirtualMachine.BandWidth;

            var outputPhysicalMachines = PackVirtualMachinesTest(expectedMaxVmsPerPhysicalMachine, inputPhysicalMachine, noOfVirtualMachinesToBeAllocated, inputVirtualMachine);
        }

        [TestMethod()]
        public void PackVirtualMachinesTest_Cores()
        {
            var inputVirtualMachine = new VirtualMachine() { BandWidth = 1, Cores = 8, Memory = 16 };
            int noOfVirtualMachinesToBeAllocated = 11;
            var inputPhysicalMachine = new PhysicalMachine() { BandWidth = 10, Cores = 64, Memory = 256 };
            int expectedMaxVmsPerPhysicalMachine = inputPhysicalMachine.Cores / inputVirtualMachine.Cores;

            var outputPhysicalMachines = PackVirtualMachinesTest(expectedMaxVmsPerPhysicalMachine, inputPhysicalMachine, noOfVirtualMachinesToBeAllocated, inputVirtualMachine);
        }

        [TestMethod()]
        public void PackVirtualMachinesTest_Memory()
        {
            var inputVirtualMachine = new VirtualMachine() { BandWidth = 1, Cores = 1, Memory = 128 };
            int noOfVirtualMachinesToBeAllocated = 11;
            var inputPhysicalMachine = new PhysicalMachine() { BandWidth = 10, Cores = 64, Memory = 256 };
            int expectedMaxVmsPerPhysicalMachine = inputPhysicalMachine.Memory / inputVirtualMachine.Memory;

            var outputPhysicalMachines = PackVirtualMachinesTest(expectedMaxVmsPerPhysicalMachine, inputPhysicalMachine, noOfVirtualMachinesToBeAllocated, inputVirtualMachine);
        }





        #region Private Methods

        private List<PhysicalMachine> PackVirtualMachinesTest(int expectedMaxVmsPerPhysicalMachine, 
                                             PhysicalMachine inputPhysicalMachine, int noOfVirtualMachinesToBeAllocated,
                                             VirtualMachine inputVirtualMachine)
        {
            var target = new Packer();
            var virtualMachines = InitializeVirtualMachines(inputVirtualMachine, noOfVirtualMachinesToBeAllocated);
            List<PhysicalMachine> outputPhysicalMachines = target.PackVirtualMachines(virtualMachines, inputPhysicalMachine);
            Assert.IsNotNull(outputPhysicalMachines);
            Assert.IsTrue(outputPhysicalMachines.Count ==
                          Math.Ceiling((decimal) virtualMachines.Count/((decimal) expectedMaxVmsPerPhysicalMachine)));
            AssertPhysicalMachines(expectedMaxVmsPerPhysicalMachine, inputVirtualMachine, virtualMachines,
                                   outputPhysicalMachines);

            AssertMaxBoundaries(outputPhysicalMachines, inputPhysicalMachine);

            return outputPhysicalMachines;
        }

        private List<VirtualMachine> InitializeVirtualMachines(VirtualMachine inputVirtualMachine, int countOfMachines)
        {
            var virtualMachines = new List<VirtualMachine>();
            int i = 0;
            while (i < countOfMachines)
            {
                virtualMachines.Add(new VirtualMachine() { BandWidth = inputVirtualMachine.BandWidth, Cores = inputVirtualMachine.Cores, Memory = inputVirtualMachine.Memory });
                i++;
            }
            return virtualMachines;
        }

        private static void AssertPhysicalMachines(int expectedMaxVmsPerPhysicalMachine,VirtualMachine inputVirtualMachine, List<VirtualMachine> virtualMachines, List<PhysicalMachine> outputPhysicalMachines)
        {
            int allocatedVms = 0;
            for (int i = 0; i < outputPhysicalMachines.Count; i++)
            {
                int expectedVmsIncurrentPhyMachine = allocatedVms < (virtualMachines.Count - expectedMaxVmsPerPhysicalMachine)
                                                         ? expectedMaxVmsPerPhysicalMachine
                                                         : virtualMachines.Count - allocatedVms;
                AssertPhysicalMachineAllocations(outputPhysicalMachines[i], inputVirtualMachine, expectedVmsIncurrentPhyMachine);
                allocatedVms += expectedMaxVmsPerPhysicalMachine;
            }
        }

        private void AssertMaxBoundaries(List<PhysicalMachine> outputPhysicalMachines, PhysicalMachine inputPhysicalMachine)
        {
            foreach (var outputPhysicalMachine in outputPhysicalMachines)
            {
                Assert.IsTrue(outputPhysicalMachine.Cores<=inputPhysicalMachine.Cores, "Overutilization of cores");
                Assert.IsTrue(outputPhysicalMachine.Memory <= inputPhysicalMachine.Memory, "Overutilization of memory");
                Assert.IsTrue(outputPhysicalMachine.BandWidth <= inputPhysicalMachine.BandWidth, "Overutilization of bandwidth");
            }
        }

        private static void AssertPhysicalMachineAllocations(PhysicalMachine physicalMachine, VirtualMachine inputVirtualMachine, int expectedVms)
        {
            Assert.IsTrue(physicalMachine.CountOfVirtualMachines == expectedVms,"Invalid Count of virtual machines.");
            Assert.IsTrue(physicalMachine.Cores == expectedVms * inputVirtualMachine.Cores, "Invalid core utilization for physical machine");
            Assert.IsTrue(physicalMachine.BandWidth == expectedVms * inputVirtualMachine.BandWidth, "Invalid core utilization for physical machine");
            Assert.IsTrue(physicalMachine.Memory == expectedVms * inputVirtualMachine.Memory, "Invalid core utilization for physical machine");
        }
    #endregion
    }
}
