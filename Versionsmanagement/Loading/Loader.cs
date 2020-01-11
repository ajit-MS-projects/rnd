using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Loading
{
    /// <summary>
    /// XML Version Loader
    /// </summary>
    public class Loader
    {
		#region Fields 

        private XmlDocument _document;
        private bool _isValid;
        private List<string> _loadErrors = new List<string>();
        private XElement _projekt;
        private string _xmlPath;

		#endregion Fields 

		#region Properties 

        /// <summary>
        /// Beim Laden aufgetretene Fehler.
        /// </summary>
        public List<string> LoadErrors 
        {
            get { return _loadErrors; }
            set { _loadErrors = value; } 
        }

		#endregion Properties 

		#region Operations 

        /// <summary>
        /// Lade-Methode fuer alle Versionen
        /// </summary>
        /// <param name="filePath">Pfad zur Projektdatei.</param>
        /// <returns>Projekt-Objekt</returns>
        public Projekt Load(string xmlPath)
        {
            try
            {
                // Zurücksetzen der Fehlerliste
                _loadErrors.Clear();

                // Dokument-Pfad setzen
                _xmlPath = xmlPath;
                
                // Feststellen der Projektversion
                string versionsNummer = this.GetVersion();

                // Validieren und Laden des Dokuments
                if (this.ValidateVersion(versionsNummer))
                {
                    return this.LoadVersion(versionsNummer);
                }
                else // ungültiges Dokument -> Kann nicht geladen werden
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.LoadErrors.Add(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Feststellen, ob gegebenes Dokument gültig ist.
        /// </summary>
        /// <param name="xmlPath">Pfad zum zu prüfenden Dokument.</param>
        /// <returns>true = Dokument ist gültig.</returns>
        public bool Validate(string xmlPath)
        {
            try
            {
                _loadErrors.Clear();
                _xmlPath = xmlPath;

                // feststellen der projektversion
                string versionsNummer = this.GetVersion();

                // validate xml datei
                return this.ValidateVersion(versionsNummer);
            }
            catch (Exception ex)
            {
                _loadErrors.Add(ex.Message);
                return false;
            }
        }

		#endregion Operations 

		#region Implementation 

        /// <summary>
        /// Feststellen der Versionsnummer
        /// </summary>
        /// <returns>Versionsnummer</returns>
        private string GetVersion()
        {
            string version = this.GetElementValue(Projekt.ELEMENT_VERSION);
            if (string.IsNullOrEmpty(version)) version = "1";

            return version;
        }

        /// <summary>
        /// Wert eines gegebenen XML-Elements zurückgeben.
        /// </summary>
        /// <param name="elementName">Element, für welches der Wert festgestellt werden soll.</param>
        /// <returns>Wert des XML-Elements.</returns>
        private string GetElementValue(string elementName, string defaultValue)
        {
            if (_projekt == null) // lade Dokument
                _projekt = XElement.Load(_xmlPath);

            string retVal = string.Empty;
            try
            {
                var element = from p in _projekt.Elements(elementName)
                                     select p.Value;
                
                foreach (string value in element)
                {
                    retVal = value;
                }

                if (string.IsNullOrEmpty(retVal)) // setzen eines Standardwertes
                {
                    retVal = defaultValue;
                }

                return retVal;
            }
            catch
            {
                // setzen eines Standardwertes
                return defaultValue;
            }
        }

        private string GetElementValue(string elementName)
        {
            return this.GetElementValue(elementName, string.Empty);
        }

        /// <summary>
        /// Laden eines Projektdatei mit gegebener Versionsnummer.
        /// </summary>
        /// <param name="versionsNummer">Versionsnummer des Projekts.</param>
        /// <returns>Projektobjekt mit geladenen Projektdaten.</returns>
        private Projekt LoadVersion(string versionsNummer)
        {
            double versionNr = Convert.ToDouble(versionsNummer);
            Projekt projekt = new Projekt(versionNr);

            // allgemeingültige Elemente
            projekt.ID = Convert.ToInt32(versionsNummer);
            projekt.Vorname = _projekt.Element(Projekt.ELEMENT_VORNAME).Value;

            switch (versionsNummer)
            {
                case "1":  // nur version 1
                    {
                        projekt.Nachname = _projekt.Element(Projekt.ELEMENT_NAME).Value;
                        break;
                    }
                case "2":
                    {
                        projekt.Nachname = _projekt.Element(Projekt.ELEMENT_NACHNAME).Value;
                        break;
                    }
                case "3":
                    {
                        projekt.Nachname = _projekt.Element(Projekt.ELEMENT_NACHNAME).Value;
                        projekt.Kommentar = this.GetElementValue(Projekt.ELEMENT_KOMMENTAR, "");                        
                        break;
                    }
            }

            return projekt;
        }

        /// <summary>
        /// Prüfen, ob Dokument mit gegebener Versionsnummer gültig ist.
        /// </summary>
        /// <param name="versionsNummer">Versionsnummer des Dokuments.</param>
        /// <returns>true = Dokument ist gültig</returns>
        private bool ValidateVersion(string versionsNummer)
        {
            _isValid = true;
            XmlReader reader = null;

            try
            {
                // definiere die Reader-Settings
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, string.Format(@"XSD\Version{0}.xsd", versionsNummer));
                settings.ValidationType = ValidationType.Schema;
                
                // erstelle Reader
                reader = XmlReader.Create(_xmlPath, settings);
                
                // Dokument laden
                _document = new XmlDocument();                
                _document.Load(reader);
            }
            catch (Exception ex)
            {
                _isValid = false;
                _loadErrors.Add(ex.Message);
            }
            finally
            {
                reader.Close();
            }

            return _isValid;
        }

		#endregion Implementation 
    }
}
