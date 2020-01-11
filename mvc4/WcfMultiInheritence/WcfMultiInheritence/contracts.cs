using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;
using WcfMultiInheritence;

namespace WcfMultiInheritence
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MetaInformation : Attribute
    {
        /// <summary>
        /// Aktiviere Kopieren
        /// </summary>
        public bool EnableCopy { get; set; }

        /// <summary>
        /// Ist eine Collection
        /// </summary>
        public bool IsCollection { get; set; }
    }

    [DataContract]
    public abstract class BasisObjekt : IBasisObjekt
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:pvscout.contracts.datacontracts.BasisObjekt"/>-Klasse.
        /// </summary>
        protected BasisObjekt() : this(DateTime.Now)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:pvscout.contracts.datacontracts.BasisObjekt"/>-Klasse.
        /// Erstellungszeitpunkt und Aktiv=true werden gesetzt
        /// </summary>
        /// <param name="p_ErstellungsDatum">Der Erstellungszeitpunkt</param>
        protected BasisObjekt(DateTime? p_ErstellungsDatum)
        {
            Aktiv = true;
            ErstellungsDatum = p_ErstellungsDatum;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="T:pvscout.contracts.datacontracts.BasisObjekt"/>-Klasse.
        /// Erstellungszeitpunkt und Aktiv=true werden gesetzt
        /// </summary>
        /// <param name="p_ErstellungsDatum">Der Erstellungszeitpunkt</param>
        /// <param name="p_ErstellungsAnwenderID">Die AnwenderID des Erstellers</param>
        /// <param name="p_BearbeitungGleichErstellung">Sollen die Erstellungsdaten auch für die Bearbeitung gelten</param>
        protected BasisObjekt(DateTime? p_ErstellungsDatum, int p_ErstellungsAnwenderID,
                              bool p_BearbeitungGleichErstellung)
            : this(p_ErstellungsDatum)
        {
            ErstellungsAnwenderID = p_ErstellungsAnwenderID;

            if (p_BearbeitungGleichErstellung)
            {
                BearbeitungsAnwenderID = p_ErstellungsAnwenderID;
                BearbeitungsDatum = p_ErstellungsDatum;
            }
        }

        #region IBasisObjekt Members

        /// <summary>
        /// Die Datenbank-ID (oftmals der Primärschlüssell)
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// wann wurde das Objekt zu letzt bearbeitet
        /// </summary>
        [DataMember]
        public DateTime? BearbeitungsDatum { get; set; }

        /// <summary>
        /// Welcher Anwender hat da Objekt zu letzt bearbeitet
        /// </summary>
        [DataMember]
        public int? BearbeitungsAnwenderID { get; set; }

        /// <summary>
        /// Wann wurde das Objekt erstellt
        /// </summary>
        [DataMember]
        public DateTime? ErstellungsDatum { get; set; }

        /// <summary>
        /// Welcher Anwender hat das Objekt erstellt
        /// </summary>
        [DataMember]
        public int? ErstellungsAnwenderID { get; set; }

        /// <summary>
        /// Aktiv = false --> zum Löschen markiert
        /// </summary>
        [DataMember]
        public bool Aktiv { get; set; }

        /// <summary>
        /// Implentierung der CopyObjekt-Methode
        /// </summary>
        public void CopyObject(object p_ZielObjekt)
        {
            if (p_ZielObjekt == null)
                throw new Exception("to object in BasisObjekt.CopyObject() can not be null.");
            var fromPropertyInfos = GetType().GetProperties();
            foreach (var frPropInfo in fromPropertyInfos)
            {
                object fromValue = frPropInfo.GetValue(this, null); //get the value of property from from FROM object
                var toPropInfo = p_ZielObjekt.GetType().GetProperty(frPropInfo.Name);
                    //get the property with same name from TO object
                if (toPropInfo != null)
                {
                    bool copyValue = true; //by default copy value of all properties 
                    var attrs = Attribute.GetCustomAttributes(toPropInfo);
                    foreach (var attr in attrs)
                    {
                        if (attr != null && attr is MetaInformation)
                        {
                            var metaInfo = (MetaInformation) attr;
                            copyValue = metaInfo.EnableCopy;
                            break;
                        }
                    }
                    if (copyValue)
                        toPropInfo.SetValue(p_ZielObjekt, fromValue, null);
                            //if TO object has this property set the value
                }
            }
        }

        #endregion

        /// <summary>
        /// Diese Methode füllt ein Objekt mit den übergebenen Daten
        /// TODO: BITTE AUSLAGERN IN BASISMODUL
        /// </summary>
        /// <param name="p_BasisObjekt">das Objekt das gefüllt werden soll</param>
        /// <param name="p_BearbeitungsAnwenderID">die AnwenderID</param>
        /// <param name="p_Aktiv">Datensatz benutzbar?</param>
        /// <param name="p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten">Daten des Erstelldatums auch setzen</param>
        /// <param name="p_BearbeitungsDatum">das Datum das im Objekt stehen soll</param>
        public static void SetzeBasisdaten(IBasisObjekt p_BasisObjekt, int p_BearbeitungsAnwenderID, bool p_Aktiv = true,
                                           bool p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten = false,
                                           DateTime? p_BearbeitungsDatum = null)
        {
            // Bearbeitungsdatum setzen: Damit untere Ebenen das gleiche Datum erhalten
            if (p_BearbeitungsDatum == null) p_BearbeitungsDatum = DateTime.Now;

            // Bearbeitung
            p_BasisObjekt.BearbeitungsAnwenderID = p_BearbeitungsAnwenderID;
            p_BasisObjekt.BearbeitungsDatum = p_BearbeitungsDatum;
            p_BasisObjekt.Aktiv = p_Aktiv;

            // Erstellung
            if (p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten)
            {
                p_BasisObjekt.ErstellungsAnwenderID = p_BasisObjekt.BearbeitungsAnwenderID;
                p_BasisObjekt.ErstellungsDatum = p_BasisObjekt.BearbeitungsDatum;
            }

            var type = p_BasisObjekt.GetType();

            // Rekursin über Reflektion (nur Fields)
            foreach (
                var val in
                    type.GetFields(BindingFlags.Instance | BindingFlags.Public).Select(
                        p_FieldInfo => p_FieldInfo.GetValue(p_BasisObjekt)).OfType<IBasisObjekt>())
            {
                SetzeBasisdaten(val, p_BearbeitungsAnwenderID, p_Aktiv,
                                p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten, p_BearbeitungsDatum);
            }

            // Rekursin über Reflektion (nur Properties)
            foreach (
                var val in
                    type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(
                        p_PropertyInfo => p_PropertyInfo.GetValue(p_BasisObjekt, null)).OfType<IBasisObjekt>())
            {
                SetzeBasisdaten(val, p_BearbeitungsAnwenderID, p_Aktiv,
                                p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten, p_BearbeitungsDatum);
            }

            // Rekursin über Reflektion (nur Properties, Listen)
            foreach (
                var val in
                    type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(
                        p_PropertyInfo => p_PropertyInfo.GetValue(p_BasisObjekt, null)).OfType
                        <IEnumerable<IBasisObjekt>>().SelectMany(p_Enumerable => p_Enumerable))
            {
                SetzeBasisdaten(val, p_BearbeitungsAnwenderID, p_Aktiv,
                                p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten, p_BearbeitungsDatum);
            }
        }

        /// <summary>
        /// Diese Methode füllt ein Objekt mit den übergebenen Daten
        /// TODO: BITTE AUSLAGERN IN BASISMODUL
        /// </summary>
        /// <param name="p_BasisObjektList">eine Liste der Objekt die gefüllt werden soll</param>
        /// <param name="p_BearbeitungsAnwenderID">die AnwenderID</param>
        /// <param name="p_Aktiv">Datensatz benutzbar?</param>
        /// <param name="p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten">Daten des Erstelldatums auch setzen</param>
        /// <param name="p_BearbeitungsDatum">das Datumdas im Objekt stehen soll</param>
        public static void SetzeBasisdaten(List<IBasisObjekt> p_BasisObjektList, int p_BearbeitungsAnwenderID,
                                           bool p_Aktiv = true,
                                           bool p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten = false,
                                           DateTime? p_BearbeitungsDatum = null)
        {

            foreach (var basisObjekt in p_BasisObjektList)
            {


                // Bearbeitungsdatum setzen: Damit untere Ebenen das gleiche Datum erhalten
                if (p_BearbeitungsDatum == null) p_BearbeitungsDatum = DateTime.Now;

                // Bearbeitung
                basisObjekt.BearbeitungsAnwenderID = p_BearbeitungsAnwenderID;
                basisObjekt.BearbeitungsDatum = p_BearbeitungsDatum;
                basisObjekt.Aktiv = p_Aktiv;

                // Erstellung
                if (p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten)
                {
                    basisObjekt.ErstellungsAnwenderID = basisObjekt.BearbeitungsAnwenderID;
                    basisObjekt.ErstellungsDatum = basisObjekt.BearbeitungsDatum;
                }

                var type = basisObjekt.GetType();

                // Rekursin über Reflektion (nur Fields)
                foreach (
                    var val in
                        type.GetFields(BindingFlags.Instance | BindingFlags.Public).Select(
                            p_FieldInfo => p_FieldInfo.GetValue(basisObjekt)).OfType<IBasisObjekt>())
                {
                    SetzeBasisdaten(val, p_BearbeitungsAnwenderID, p_Aktiv,
                                    p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten, p_BearbeitungsDatum);
                }

                // Rekursin über Reflektion (nur Properties)
                foreach (
                    var val in
                        type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(
                            p_PropertyInfo => p_PropertyInfo.GetValue(basisObjekt, null)).OfType<IBasisObjekt>())
                {
                    SetzeBasisdaten(val, p_BearbeitungsAnwenderID, p_Aktiv,
                                    p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten, p_BearbeitungsDatum);
                }

                // Rekursin über Reflektion (nur Properties, Listen)
                foreach (
                    var val in
                        type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(
                            p_PropertyInfo => p_PropertyInfo.GetValue(basisObjekt, null)).OfType
                            <IEnumerable<IBasisObjekt>>().SelectMany(p_Enumerable => p_Enumerable))
                {
                    SetzeBasisdaten(val, p_BearbeitungsAnwenderID, p_Aktiv,
                                    p_SetzeErstellungsdatenZusammenMitBearbeitungsdaten, p_BearbeitungsDatum);
                }
            }
        }


    }
    [DataContract]
    public class Flaeche : IFlaeche
    {
        /// <summary>
        /// Standardkonstruktor
        /// </summary>
        public Flaeche()
        {
            Punkte = new List<Punkt3D>();
        }

        /// <summary>
        /// Die einzelnen Randpunkte der Fläche
        /// </summary>
        public List<Punkt3D> Punkte { get; set; }

        /// <summary>
        /// Die Fläche in einzelne Dreiecke unterteilt
        /// </summary>
        [DataMember]
        public List<Dreieck> Dreiecke { get; set; }

        /// <summary>
        /// Die X-Koordinate die am weitesten Links liegt
        /// </summary>
        public double Left
        {
            get
            {
                var ret = double.MaxValue;

                foreach (var punkt in Punkte)
                {
                    ret = Math.Min(ret, punkt.X);
                }

                return ret;
            }
        }

        /// <summary>
        /// Die X-Koordinate die am weitesten Rechts liegt
        /// </summary>
        public double Right
        {
            get
            {
                var ret = double.MinValue;

                foreach (var punkt in Punkte)
                {
                    ret = Math.Max(ret, punkt.X);
                }

                return ret;
            }
        }

        /// <summary>
        /// Die Y-Koordinate die am weitesten Oben liegt
        /// </summary>
        public double Top
        {
            get
            {
                var ret = double.MinValue;

                foreach (var punkt in Punkte)
                {
                    ret = Math.Max(ret, punkt.Y);
                }

                return ret;
            }
        }

        /// <summary>
        /// Die Y-Koordinate die am weitesten Unten liegt
        /// </summary>
        public double Bottom
        {
            get
            {
                var ret = double.MaxValue;

                foreach (var punkt in Punkte)
                {
                    ret = Math.Min(ret, punkt.Y);
                }

                return ret;
            }
        }
    }


    [DataContract]
    [KnownType(typeof (SparrenkonstruktionParameter))]
    public class SatteldachParameter : DachParameter, ISatteldachParameter
    {
        /// <summary>
        /// Die Laenge der oberen und unteren Kante des Satteldaches
        /// </summary>
        [DataMember]
        public double FirstUndTraufeLaenge { get; set; }

        /// <summary>
        /// Die Laenge der seitlichen Kanten des Satteldaches
        /// </summary>
        [DataMember]
        public double OrtgangLaenge { get; set; }

        /// <summary>
        /// Der Ueberstand ueber die unter Kante des Satteldaches
        /// </summary>
        [DataMember]
        public double UeberstandUeberTraufe { get; set; }

        /// <summary>
        /// Der Ueberstand ueber die seitliche Kanten des Satteldaches
        /// </summary>
        [DataMember]
        public double UeberstandUeberOrtgang { get; set; }
    }

    [DataContract]
    [KnownType(typeof (SatteldachParameter))]
    public class GebaeudeParameter : BasisParameter, IGebaeudeParameter
    {
        /// <summary>
        /// Gebäudelänge in Meter
        /// </summary>
        [DataMember]
        public double Laenge { get; set; }

        /// <summary>
        /// Gebäudebreite in Meter
        /// </summary>
        [DataMember]
        public double Breite { get; set; }

        /// <summary>
        /// Gebäudehöhe in Meter
        /// </summary>
        [DataMember]
        public double Hoehe { get; set; }

        /// <summary>
        /// Details zum Gebäudedach
        /// </summary>
        [DataMember]
        public DachParameter DachParameter { get; set; }
    }

    [DataContract]
    public class DachkonstruktionParameter : BasisParameter, IDachkonstruktionParameter
    {
        /// <summary>
        /// ID der Dachkonstruktionsart (bspw. Sparren, Pfetten)
        /// </summary>
        [DataMember]
        public int DachkonstruktionsartID { get; set; }
    }

    public class MaterialArt : BasisObjekt, IMaterialArt
    {
        /// <summary>
        /// Der Name des Materials
        /// </summary>
        public string Name { get; set; }
    }

    public class QuerlattungParameter : BasisParameter, IQuerlattungParameter
    {
        /// <summary>
        /// Ob eine Querlattung vorhanden ist
        /// </summary>
        public bool QuerlattungVorhanden { get; set; }

        /// <summary>
        /// Welches Material die Sparren haben. Holz oder Stahl?
        /// </summary>
        public MaterialArt MaterialArt { get; set; }

        /// <summary>
        /// Der Abstand von Links zur ersten Lattung in Meter
        /// </summary>
        public double AbstandVonOben { get; set; }

        /// <summary>
        /// Der Abstand zwischen den einzelnen Lattungen in Meter
        /// </summary>
        public double AbstandZwischenLattung { get; set; }

        /// <summary>
        /// Die Breite der einzelnen Lattungen in Meter
        /// </summary>
        public double Lattungbreite { get; set; }

        /// <summary>
        /// Die Dicke der einzelnen Lattungen in Meter
        /// </summary>
        public double Lattungdicke { get; set; }
    }

    [DataContract]
    public class SparrenkonstruktionParameter : DachkonstruktionParameter, ISparrenKonstruktionParameter
    {
        /// <summary>
        /// Welches Material die Sparren haben. Holz oder Stahl?
        /// </summary>
        public MaterialArt MaterialArt { get; set; }

        /// <summary>
        /// Der Abstand von Links zum ersten Sparren in Meter
        /// </summary>
        [DataMember]
        public double AbstandVonLinks { get; set; }

        /// <summary>
        /// Der Abstand zwischen den einzelnen Sparren in Meter
        /// </summary>
        [DataMember]
        public double AbstandZwischenSparren { get; set; }

        /// <summary>
        /// Die Breite der einzelnen Sparren in Meter
        /// </summary>
        [DataMember]
        public double Sparrenbreite { get; set; }

        /// <summary>
        /// Die Dicke der einzelnen Sparren in Meter
        /// </summary>
        [DataMember]
        public double Sparrendicke { get; set; }

        /// <summary>
        /// Ob eine Querlattung montiert wird
        /// </summary>
        public QuerlattungParameter QuerlattungParameter { get; set; }
    }

    [DataContract]
    [KnownType(typeof (SparrenkonstruktionParameter))]
    public abstract class DachParameter : BasisParameter, IDachParameter
    {
        /// <summary>
        /// Die Oberste Kante des Daches
        /// </summary>
        [DataMember]
        public double FirstHoehe { get; set; }

        /// <summary>
        /// Dachneigung in Grad
        /// </summary>
        [DataMember]
        public double Neigung { get; set; }

        /// <summary>
        /// Dachausrichtung in Grad
        /// </summary>
        [DataMember]
        public double Ausrichtung { get; set; }

        /// <summary>
        /// ID der Dachart (Satteldach, Walmdach etc.)
        /// </summary>
        [DataMember]
        public int DachartID { get; set; }

        /// <summary>
        /// ID der Dacheindeckungsart (bspw. Ziegel)
        /// </summary>
        [DataMember]
        public int DacheindeckungsartID { get; set; }

        /// <summary>
        /// Details zur Dachkonstruktion (Sparren, etc.)
        /// </summary>
        [DataMember]
        public DachkonstruktionParameter DachkonstruktionParameter { get; set; }
    }
    public enum LayerKey
    {
        /// <summary>
        /// Nicht zugeordnete Objekt
        /// </summary>
        UNBEKANNT,
        /// <summary>
        /// Alle Objekte des Gebäudes. Wände, Boden usw.
        /// </summary>
        GEBAEUDE,
        /// <summary>
        /// Objekte eines Daches. Dachsegmente, Eindeckung, usw.
        /// </summary>
        DACH,
        /// <summary>
        /// Objekte der Dachkonstruktion. Sparren, Pfetten, usw.
        /// </summary>
        DACHKONSTRUKTION,
        /// <summary>
        /// Alle Stoerobjekte
        /// </summary>
        STOEROBJEKT
    }
    [DataContract]
    public class Wand : PvObjekt, IWand
    {
        /// <summary>
        /// Der Key des Layernamens
        /// </summary>
        public override LayerKey Key
        {
            get { return LayerKey.GEBAEUDE; }
        }
    }
    public class Stoerobjekt : PvObjekt, IStoerobjekt
    {
        /// <summary>
        /// Der Key des Layernamens
        /// </summary>
        public override LayerKey Key
        {
            get { return LayerKey.STOEROBJEKT; }
        }
    }
    [DataContract]
    public class Punkt3D : IPunkt3D
    {
        /// <summary>
        /// Standardkonstruktor
        /// </summary>
        public Punkt3D()
            : this(0, 0, 0)
        { }

        /// <summary>
        /// Initialisierungskonstruktor
        /// </summary>
        /// <param name="p_X"></param>
        /// <param name="p_Y"></param>
        /// <param name="p_Z"></param>
        public Punkt3D(double p_X, double p_Y, double p_Z)
        {
            X = p_X;
            Y = p_Y;
            Z = p_Z;
        }

        /// <summary>
        /// Kopierkonstruktor
        /// </summary>
        /// <param name="p_Punkt"></param>
        public Punkt3D(Punkt3D p_Punkt) : this(p_Punkt.X, p_Punkt.Y, p_Punkt.Z) { }

        /// <summary>
        /// Die X-Koordinate
        /// </summary>
        [DataMember]
        public double X { get; set; }

        /// <summary>
        /// Die Y-Koordinate
        /// </summary>
        [DataMember]
        public double Y { get; set; }

        /// <summary>
        /// Die Z-Koordinate
        /// </summary>
        [DataMember]
        public double Z { get; set; }

        /// <summary>
        /// Ueberschreibung der ToString-Funktion
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "X: " + X + ", Y: " + Y + ", Z: " + Z;
        }
    }
    [DataContract]
    public abstract class Dachkonstruktion : IDachkonstruktion
    {
        private List<PvObjekt> m_Children;

        /// <summary>
        /// Die einzelnen Objekte der Dachkonstruktion
        /// </summary>
        public virtual List<PvObjekt> Children
        {
            get
            {
                if (m_Children == null) m_Children = new List<PvObjekt>();
                return m_Children;
            }
        }
    }
    [DataContract]
    public class PvObjektMetadaten : IPvObjektMetadaten
    {
        /// <summary>
        /// Name des PvObjekts
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
    [DataContract]
    public class DachSegment : PvObjekt, IDachSegment
    {
        /// <summary>
        /// Standardkonstruktor
        /// </summary>
        public DachSegment()
        {
            Stoerobjekte = new List<Stoerobjekt>();
        }

        /// <summary>
        /// Der Key des Layernamens
        /// </summary>
        public override LayerKey Key
        {
            get { return LayerKey.DACH; }
        }

        /// <summary>
        /// Die Dachkonstruktion des Dachsegments
        /// </summary>
        [DataMember]
        public Dachkonstruktion Dachkonstruktion { get; set; }

        /// <summary>
        /// Liste der Stoerobjekte am Dach
        /// </summary>
        public List<Stoerobjekt> Stoerobjekte { get; set; }

        /// <summary>
        /// Die einzelnen Objekte auf einem Dach
        /// </summary>
        public override List<PvObjekt> Children
        {
            get
            {
                var ret = new List<PvObjekt>();
                if (Dachkonstruktion != null) ret.AddRange(Dachkonstruktion.Children);
                if (Stoerobjekte != null) ret.AddRange(Stoerobjekte);
                return ret;
            }
        }
    }
    [DataContract]
    [KnownType(typeof(Gebaeude))]
    [KnownType(typeof(Wand))]
    [KnownType(typeof(DachSegment))]
    public class PvObjekt : BasisObjekt, IPvObjekt
    {
        /// <summary>
        /// Metadaten eines PvObjekts
        /// </summary>
        [DataMember]
        public PvObjektMetadaten Metadaten { get; set; }

        /// <summary>
        /// Der Key des Layernamens
        /// </summary>
        public virtual LayerKey Key { get { return LayerKey.UNBEKANNT; } }

        private List<PvObjekt> m_Children;
        private List<Flaeche> m_Flaechen;

        /// <summary>
        /// Alle Anlagenobjekte die direkt unterhalb dieses Objektes liegen
        /// </summary>
        public virtual List<PvObjekt> Children
        {
            get
            {
                if (m_Children == null) m_Children = new List<PvObjekt>();
                return m_Children;
            }
        }

        /// <summary>
        /// Die IDs aller Anlagenobjekte die unterhalb dieses Objektes und der darunter liegen
        /// </summary>
        public List<int> GetChildrenID()
        {
            var ret = new List<int>();

            foreach (var kind in Children)
            {
                ret.Add(kind.ID);
                ret.AddRange(kind.GetChildrenID());
            }

            return ret;
        }

        /// <summary>
        /// Die IDs aller Anlagenobjekte die unterhalb dieses Objektes und der darunter liegen
        /// </summary>
        public PvObjekt GetChildren(int p_ID)
        {
            foreach (var pvObjekt in Children)
            {
                if (pvObjekt.ID == p_ID) return pvObjekt;
                var pvObjektKind = pvObjekt.GetChildren(p_ID);
                if (pvObjektKind != null) return pvObjektKind;
            }
            return null;
        }

        /// <summary>
        /// Parent-Anlagenobjekts
        /// Wenn es leer ist liegt es direkt in der Gesamtanlage
        /// </summary>
        public PvObjekt Parent { get; set; }

        /// <summary>
        /// Die Einstellungen zu einem Objekt
        /// </summary>
        public BasisParameter Parameter { get; set; }

        /// <summary>
        /// Die Farbe des Objektes
        /// </summary>
        [DataMember]
        public Color Farbe { get; set; }

        /// <summary>
        /// Stellt die einzelnen zu zeichnenden Flaechen mit deren Dreiecke dar
        /// </summary>
        [DataMember]
        public List<Flaeche> Flaechen
        {
            get
            {
                if (m_Flaechen == null) m_Flaechen = new List<Flaeche>();
                return m_Flaechen;
            }
            set { m_Flaechen = value; }
        }

        /// <summary>
        /// Gibt die Dreiecke der Grundflaeche des Objekts zurueck
        /// </summary>
        /// <returns>Liste Dreiecke der Grundflaeche</returns>
        public virtual List<Flaeche> GetGrundflaechen()
        {
            var minZ = (from flaeche in Flaechen from punkt in flaeche.Punkte select punkt.Z).Concat(new[] { Double.MaxValue }).Min();

            return (from flaeche in Flaechen let istGrundflaeche = flaeche.Punkte.All(punkt => punkt.Z.Equals(minZ)) where istGrundflaeche select flaeche).ToList();
        }

        /// <summary>
        /// Der Verschiebevektor vom Startpunkt des Parentobjektes gesehen
        /// </summary>
        public Punkt3D Startvektor { get; set; }

        /// <summary>
        /// Die Neigung vom Startpunkt des Parentobjektes gesehen
        /// </summary>
        public double Neigung { get; set; }

        /// <summary>
        /// Die Ausrichtung vom Startpunkt des Parentobjektes gesehen
        /// </summary>
        public double Ausrichtung { get; set; }
    }
    [DataContract]
    public class Gebaeude : PvObjekt, IGebaeude
    {
        /// <summary>
        /// Der Key des Layernamens
        /// </summary>
        public override LayerKey Key
        {
            get { return LayerKey.GEBAEUDE; }
        }

        /// <summary>
        /// Das Dach vom Gebaeude
        /// </summary>
        [DataMember]
        public List<DachSegment> Dach { get; set; }

        /// <summary>
        /// Die Waende vom Gebaeude
        /// </summary>
        [DataMember]
        public List<Wand> Waende { get; set; }

        /// <summary>
        /// Die einzelnen Objekte des Gebaeudes
        /// </summary>
        public override List<PvObjekt> Children
        {
            get
            {
                var ret = new List<PvObjekt>();
                if (Waende != null) ret.AddRange(Waende);
                if (Dach != null) ret.AddRange(Dach);
                return ret;
            }
        }

        /// <summary>
        /// Einstellmöglichkeiten eines Gebäudes
        /// </summary>
        [DataMember]
        public GebaeudeParameter GebaeudeParameter { get; set; }
    }

   
}