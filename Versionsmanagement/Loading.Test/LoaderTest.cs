using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Loading;

namespace Loading.Test
{
    /// <summary>
    /// 3 Projektversionen:
    /// 1: noch ohne Versionselement
    /// 2: Versionselement eingeführt; Name in Nachname umbenannt;
    /// 3: Kommentar als neues optionales Feld;
    /// </summary>
    [TestClass]
    public class LoaderTest
    {
        /// <summary>
        /// Lade gültige Version 1
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"XSD\Version1.xsd", "XSD")]
        [DeploymentItem(@"XML\Projekt1.xml", "XML")]
        public void TestLoadVersion1()
        {
            Loader loader = new Loader();
            Projekt projekt = loader.Load(@"XML\Projekt1.xml");

            Assert.IsNotNull(projekt);
            Assert.IsTrue(loader.LoadErrors.Count == 0);
        }

        /// <summary>
        /// Lade gültige Version 2
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"XSD\Version2.xsd", "XSD")]
        [DeploymentItem(@"XML\Projekt2.xml", "XML")]
        public void TestLoadVersion2()
        {
            Loader loader = new Loader();
            Projekt projekt = loader.Load(@"XML\Projekt2.xml");

            Assert.IsNotNull(projekt);
            Assert.IsTrue(loader.LoadErrors.Count == 0);
        }

        /// <summary>
        /// Lade gültige Version 3
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"XSD\Version3.xsd", "XSD")]
        [DeploymentItem(@"XML\Projekt3.xml", "XML")]
        public void TestLoadVersion3()
        {
            Loader loader = new Loader();
            Projekt projekt = loader.Load(@"XML\Projekt3.xml");

            Assert.IsNotNull(projekt);
            Assert.IsTrue(loader.LoadErrors.Count == 0);
        }

        /// <summary>
        /// Lade Version 1 mit Versionsnummer 3
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"XSD\Version3.xsd", "XSD")]
        [DeploymentItem(@"XML\Projekt4.xml", "XML")]
        public void TestLoadVersion1alsVersion3()
        {
            Loader loader = new Loader();
            Projekt projekt = loader.Load(@"XML\Projekt4.xml");

            Assert.IsNull(projekt);
            Assert.IsTrue(loader.LoadErrors.Count > 0);
        }

        /// <summary>
        /// Lade Version 3 mit Versionsnummer 1
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"XSD\Version3.xsd", "XSD")]
        [DeploymentItem(@"XML\Projekt5.xml", "XML")]
        public void TestLoadVersion3alsVersion1()
        {
            Loader loader = new Loader();
            Projekt projekt = loader.Load(@"XML\Projekt5.xml");

            Assert.IsNull(projekt);
            Assert.IsTrue(loader.LoadErrors.Count > 0);
        }

        /// <summary>
        /// Lade Version 2 als Version 3 -> sollte gehen
        /// </summary>
        [TestMethod]
        [DeploymentItem(@"XSD\Version3.xsd", "XSD")]
        [DeploymentItem(@"XML\Projekt6.xml", "XML")]
        public void TestLoadVersion2alsVersion3()
        {
            Loader loader = new Loader();
            Projekt projekt = loader.Load(@"XML\Projekt6.xml");

            Assert.IsNotNull(projekt);
            Assert.IsTrue(loader.LoadErrors.Count == 0);
        }
    }
}
