using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WcfMultiInheritence
{
    public interface IBasisObjekt
    {
        /// <summary>
        /// Aktiv = false --> zum Löschen markiert
        /// </summary>
        bool Aktiv { get; set; }

        /// <summary>
        /// Die Datenbank-ID (oftmals der Primärschlüssell)
        /// </summary>
        int ID { get; set; }

        /// <summary>
        /// Wann wurde das Objekt erstellt
        /// </summary>
        DateTime? ErstellungsDatum { get; set; }

        /// <summary>
        /// wann wurde das Objekt zu letzt bearbeitet
        /// </summary>
        DateTime? BearbeitungsDatum { get; set; }

        /// <summary>
        /// Welcher Anwender hat das Objekt erstellt
        /// </summary>
        int? ErstellungsAnwenderID { get; set; }

        /// <summary>
        /// Welcher Anwender hat da Objekt zu letzt bearbeitet
        /// </summary>
        int? BearbeitungsAnwenderID { get; set; }

        /// <summary>
        /// Implentierung der CopyObjekt-Methode
        /// </summary>
        /// <param name="p_ZielObjekt"></param>
        void CopyObject(object p_ZielObjekt);
    }
    public interface IWand : IPvObjekt
    {
    }
    public interface IStoerobjekt
    {
    }
    public interface IDachkonstruktion
    {
        /// <summary>
        /// Die einzelnen Objekte der Dachkonstruktion
        /// </summary>
        List<PvObjekt> Children { get; }
    }
    public interface IPvObjektMetadaten
    {
        /// <summary>
        /// Name des PvObjekts
        /// </summary>
        string Name { get; set; }
    }
    public interface IDachSegment : IPvObjekt
    {
        /// <summary>
        /// Die Dachkonstruktion des Dachsegments
        /// </summary>
        Dachkonstruktion Dachkonstruktion { get; set; }

        /// <summary>
        /// Liste der Stoerobjekte am Dach
        /// </summary>
        List<Stoerobjekt> Stoerobjekte { get; set; }
    }
    public interface IPvObjekt : IBasisObjekt
    {
        /// <summary>
        /// Metadaten eines PvObjekts
        /// </summary>
        PvObjektMetadaten Metadaten { get; set; }

        /// <summary>
        /// Alle Anlagenobjekte die unterhalb dieses Objektes liegen
        /// </summary>
        List<PvObjekt> Children { get; }

        /// <summary>
        /// Parent-Anlagenobjekts
        /// Wenn es leer ist liegt es direkt in der Gesamtanlage
        /// </summary>
        PvObjekt Parent { get; set; }

        /// <summary>
        /// Die Einstellungen zu einem Objekt
        /// </summary>
        BasisParameter Parameter { get; set; }

        /// <summary>
        /// Die Farbe der Flaeche
        /// </summary>
        Color Farbe { get; set; }

        /// <summary>
        /// Stellt die einzelnen zu zeichnenden Flaechen mit deren Dreiecke dar
        /// </summary>
        List<Flaeche> Flaechen { get; set; }

        /// <summary>
        /// Der Verschiebevektor vom Startpunkt des Parentobjektes gesehen
        /// </summary>
        Punkt3D Startvektor { get; set; }

        /// <summary>
        /// Die Neigung vom Startpunkt des Parentobjektes gesehen
        /// </summary>
        double Neigung { get; set; }

        /// <summary>
        /// Die Ausrichtung vom Startpunkt des Parentobjektes gesehen
        /// </summary>
        double Ausrichtung { get; set; }
    }
    public interface IDreieck
    {
        /// <summary>
        /// Der 1. Eckpunkt des Dreiecks
        /// </summary>
        Punkt3D Punkt1 { get; set; }

        /// <summary>
        /// Der 2. Eckpunkt des Dreiecks
        /// </summary>
        Punkt3D Punkt2 { get; set; }

        /// <summary>
        /// Der 3. Eckpunkt des Dreiecks
        /// </summary>
        Punkt3D Punkt3 { get; set; }

        /// <summary>
        /// Der 1. Startpunkt des Dreiecks
        /// </summary>
        Punkt3D Startpunkt1 { get; set; }

        /// <summary>
        /// Der 2. Startpunkt des Dreiecks
        /// </summary>
        Punkt3D Startpunkt2 { get; set; }

        /// <summary>
        /// Der 3. Startpunkt des Dreiecks
        /// </summary>
        Punkt3D Startpunkt3 { get; set; }

        /// <summary>
        /// Das ubergeordnete Objekt
        /// </summary>
        PvObjekt Parent { get; set; }
    }
    [DataContract]
    public class Dreieck : IDreieck
    {
        /// <summary>
        /// Standardkonstruktor
        /// </summary>
        public Dreieck()
            : this(new Punkt3D(), new Punkt3D(), new Punkt3D())
        { }

        /// <summary>
        /// Initialisierungskonstruktor
        /// </summary>
        /// <param name="p_Punkt1"></param>
        /// <param name="p_Punkt2"></param>
        /// <param name="p_Punkt3"></param>
        public Dreieck(Punkt3D p_Punkt1, Punkt3D p_Punkt2, Punkt3D p_Punkt3)
        {
            Startpunkt1 = new Punkt3D(p_Punkt1);
            Startpunkt2 = new Punkt3D(p_Punkt2);
            Startpunkt3 = new Punkt3D(p_Punkt3);
            Punkt1 = new Punkt3D(Startpunkt1);
            Punkt2 = new Punkt3D(Startpunkt2);
            Punkt3 = new Punkt3D(Startpunkt3);
        }

        /// <summary>
        /// Der 1. Eckpunkt des Dreiecks
        /// </summary>
        public List<Punkt3D> Punkte
        {
            get
            {
                return new List<Punkt3D> {
                    Punkt1,
                    Punkt2,
                    Punkt3
                };
            }
        }

        /// <summary>
        /// Der 1. Eckpunkt des Dreiecks
        /// </summary>
        [DataMember]
        public Punkt3D Punkt1 { get; set; }

        /// <summary>
        /// Der 2. Eckpunkt des Dreiecks
        /// </summary>
        [DataMember]
        public Punkt3D Punkt2 { get; set; }

        /// <summary>
        /// Der 3. Eckpunkt des Dreiecks
        /// </summary>
        [DataMember]
        public Punkt3D Punkt3 { get; set; }

        /// <summary>
        /// Der 1. Eckpunkt des Dreiecks
        /// </summary>
        public Punkt3D Startpunkt1 { get; set; }

        /// <summary>
        /// Der 2. Eckpunkt des Dreiecks
        /// </summary>
        public Punkt3D Startpunkt2 { get; set; }

        /// <summary>
        /// Der 3. Eckpunkt des Dreiecks
        /// </summary>
        public Punkt3D Startpunkt3 { get; set; }

        /// <summary>
        /// Das ubergeordnete Objekt
        /// </summary>
        public PvObjekt Parent { get; set; }

    }
    public interface IFlaeche
    {
        /// <summary>
        /// Die einzelnen Randpunkte der Fläche
        /// </summary>
        List<Punkt3D> Punkte { get; set; }

        /// <summary>
        /// Die X-Koordinate die am weitesten Links liegt
        /// </summary>
        double Left { get; }

        /// <summary>
        /// Die X-Koordinate die am weitesten Rechts liegt
        /// </summary>
        double Right { get; }

        /// <summary>
        /// Die Y-Koordinate die am weitesten Oben liegt
        /// </summary>
        double Top { get; }

        /// <summary>
        /// Die Y-Koordinate die am weitesten Unten liegt
        /// </summary>
        double Bottom { get; }

        /// <summary>
        /// Die Fläche in einzelne Dreiecke unterteilt
        /// </summary>
        List<Dreieck> Dreiecke { get; set; }
    }
    public interface IPunkt3D
    {
        /// <summary>
        /// Die X-Koordinate
        /// </summary>
        double X { get; set; }

        /// <summary>
        /// Die Y-Koordinate
        /// </summary>
        double Y { get; set; }

        /// <summary>
        /// Die Z-Koordinate
        /// </summary>
        double Z { get; set; }
    }
    public interface IGebaeude : IPvObjekt
    {
        /// <summary>
        /// Das Dach vom Gebaeude
        /// </summary>
        List<DachSegment> Dach { get; set; }

        /// <summary>
        /// Die Waende vom Gebaeude
        /// </summary>
        List<Wand> Waende { get; set; }

        /// <summary>
        /// Definition eines Gebäudes
        /// </summary>
        GebaeudeParameter GebaeudeParameter { get; set; }
    }
    public interface ISatteldachParameter : IDachParameter
    {
        /// <summary>
        /// Die Laenge der oberen und unteren Kante des Satteldaches
        /// </summary>
        double FirstUndTraufeLaenge { get; set; }

        /// <summary>
        /// Die Laenge der seitlichen Kanten des Satteldaches
        /// </summary>
        double OrtgangLaenge { get; set; }

        /// <summary>
        /// Der Ueberstand ueber die unter Kante des Satteldaches
        /// </summary>
        double UeberstandUeberTraufe { get; set; }

        /// <summary>
        /// Der Ueberstand ueber die seitliche Kanten des Satteldaches
        /// </summary>
        double UeberstandUeberOrtgang { get; set; }
    }
    public interface IQuerlattungParameter : IBasisParameter
    {
        /// <summary>
        /// Ob eine Querlattung vorhanden ist
        /// </summary>
        bool QuerlattungVorhanden { get; set; }

        /// <summary>
        /// Welches Material die Sparren haben. Holz oder Stahl?
        /// </summary>
        MaterialArt MaterialArt { get; set; }

        /// <summary>
        /// Der Abstand von Links zur ersten Lattung in Meter
        /// </summary>
        double AbstandVonOben { get; set; }

        /// <summary>
        /// Der Abstand zwischen den einzelnen Lattungen in Meter
        /// </summary>
        double AbstandZwischenLattung { get; set; }

        /// <summary>
        /// Die Breite der einzelnen Lattungen in Meter
        /// </summary>
        double Lattungbreite { get; set; }

        /// <summary>
        /// Die Dicke der einzelnen Lattungen in Meter
        /// </summary>
        double Lattungdicke { get; set; }
    }
    public interface IBasisParameter
    {
    }

    [DataContract]
    public abstract class BasisParameter : IBasisParameter
    {
    }
    public interface IGebaeudeParameter : IBasisParameter
    {
        /// <summary>
        /// Gebäudelänge in Meter
        /// 
        /// </summary>
        double Laenge { get; set; }

        /// <summary>
        /// Gebäudebreite in Meter
        /// </summary>
        double Breite { get; set; }

        /// <summary>
        /// Gebäudehöhe in Meter
        /// </summary>
        double Hoehe { get; set; }

        /// <summary>
        /// Details zum Gebäudedach
        /// </summary>
        DachParameter DachParameter { get; set; }
    }
    public interface IDachkonstruktionParameter : IBasisParameter
    {
        /// <summary>
        /// ID der Dachkonstruktionsart (bspw. Sparren, Pfetten)
        /// </summary>
        int DachkonstruktionsartID { get; set; }
    }
    public interface IMaterialArt : IBasisObjekt
    {
        /// <summary>
        /// Der Name des Materials
        /// </summary>
        string Name { get; set; }
    }
    public interface ISparrenKonstruktionParameter : IDachkonstruktionParameter
    {
        /// <summary>
        /// Welches Material die Sparren haben. Holz oder Stahl?
        /// </summary>
        MaterialArt MaterialArt { get; set; }

        /// <summary>
        /// Der Abstand von Links zum ersten Sparren in Meter
        /// </summary>
        double AbstandVonLinks { get; set; }

        /// <summary>
        /// Der Abstand zwischen den einzelnen Sparren in Meter
        /// </summary>
        double AbstandZwischenSparren { get; set; }

        /// <summary>
        /// Die Breite der einzelnen Sparren in Meter
        /// </summary>
        double Sparrenbreite { get; set; }

        /// <summary>
        /// Die Dicke der einzelnen Sparren in Meter
        /// </summary>
        double Sparrendicke { get; set; }

        /// <summary>
        /// Die Parameter für eine Querlattung
        /// </summary>
        QuerlattungParameter QuerlattungParameter { get; set; }
    }
    public interface IDachParameter : IBasisParameter
    {
        /// <summary>
        /// Die Oberste Kante des Daches
        /// </summary>
        double FirstHoehe { get; set; }

        /// <summary>
        /// Dachneigung in Grad
        /// </summary>
        double Neigung { get; set; }

        /// <summary>
        /// Dachausrichtung in Grad
        /// </summary>
        double Ausrichtung { get; set; }

        /// <summary>
        /// ID der Dachart (Satteldach, Walmdach etc.)
        /// </summary>
        int DachartID { get; set; }

        /// <summary>
        /// ID der Dacheindeckungsart (bspw. Ziegel)
        /// </summary>
        int DacheindeckungsartID { get; set; }

        /// <summary>
        /// Details zur Dachkonstruktion (Sparren, etc.)
        /// </summary>
        DachkonstruktionParameter DachkonstruktionParameter { get; set; }
    }
}