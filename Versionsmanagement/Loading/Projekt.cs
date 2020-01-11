using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loading
{
    /// <summary>
    /// Transfer-Objekt, Version 3
    /// </summary>
    public class Projekt
    {
        //private List<VersionElement> _versionElements;

        //internal List<VersionElement> VersionElements 
        //{
        //    get { return _versionElements; }
        //    set { _versionElements = value; }
        //}

        # region Elementnamen

        /// <summary>
        /// Gueltige Elemente
        /// </summary>
        public const string ELEMENT_ID = "ID";
        public const string ELEMENT_VORNAME = "Vorname";

        /// <summary>
        /// Version 1
        /// </summary>
        public const string ELEMENT_NAME = "Name";

        /// <summary>
        /// Version 2
        /// </summary>
        public const string ELEMENT_VERSION = "Version";
        public const string ELEMENT_NACHNAME = "Nachname";

        /// <summary>
        /// Version 3
        /// </summary>
        public const string ELEMENT_KOMMENTAR = "Kommentar";

        # endregion Elementnamen

        public Projekt(double versionsnummer)
        {
            this.Version = versionsnummer;
            // lade versionshistorie
            //_versionElements = new List<VersionElement>();
            
            //// alle
            //_versionElements.Add(new VersionElement() { Name = ELEMENT_ID, Typ = typeof(Int32), VonVersion = 1, BisVersion = null });
            //_versionElements.Add(new VersionElement() { Name = ELEMENT_VORNAME, Typ = typeof(string), VonVersion = 1, BisVersion = null });

            //// nur version 1
            //_versionElements.Add(new VersionElement() { Name = ELEMENT_NAME, Typ = typeof(string), VonVersion = 1, BisVersion = 1 });

            //// ab version 2
            //_versionElements.Add(new VersionElement() { Name = ELEMENT_NACHNAME, Typ = typeof(string), VonVersion = 2, BisVersion = null });
            //_versionElements.Add(new VersionElement() { Name = ELEMENT_VERSION, Typ = typeof(double), VonVersion = 2, BisVersion = null });

            //// ab version 3
            //_versionElements.Add(new VersionElement() { Name = ELEMENT_KOMMENTAR, Typ = typeof(string), VonVersion = 3, BisVersion = null });

            //_versionElements = _versionElements.Where(v => v.VonVersion <= versionsnummer && v.BisVersion >= versionsnummer).ToList();
        }

        # region Properties

        public double Version { get; set; }
        public int ID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Kommentar { get; set; }

        # endregion Properties
    }
}
