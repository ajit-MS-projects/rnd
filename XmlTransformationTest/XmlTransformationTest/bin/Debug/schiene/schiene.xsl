<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/Schiene">
    <ArrayOfSchiene  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
      <Schiene>
        <ID>
          <xsl:value-of select="./PRIMKEY/WERT" />
        </ID>
        <VERSION Versionsnummer="{./VERSION/@Versionsnummer}" AnlegeDatum="{./VERSION/@AnlegeDatum}" Abgekuendigt="{./VERSION/@Abgekuendigt}">
          <Aenderung />
          <AngelegtDurch Titel="{./VERSION/ANGELEGT_DURCH/@Titel}" Anrede="{./VERSION/ANGELEGT_DURCH/@Anrede}" NachName="{./VERSION/ANGELEGT_DURCH/@NachName}" VorName="{./VERSION/ANGELEGT_DURCH/@VorName}" Kuerzel="{./VERSION/ANGELEGT_DURCH/@Kuerzel}" />
        </VERSION>
      </Schiene>
      <ARTIKELNUMMER>
        <ARTIKELNUMMER_SOLS>
          <xsl:value-of select="./ARTIKELNUMMER/SOLS" />
        </ARTIKELNUMMER_SOLS>
        <ARTIKELNUMMER_KUNDE>
          <xsl:value-of select="./ARTIKELNUMMER/KUNDE" />
        </ARTIKELNUMMER_KUNDE>
        <BESTELL_NUMMER>
          <xsl:value-of select="./ARTIKELNUMMER/BESTELLNUMMER" />
        </BESTELL_NUMMER>
        <REVISIONS_NUMMER>
          <xsl:value-of select="./ARTIKELNUMMER/REVISIONSNUMMER" />
        </REVISIONS_NUMMER>
      </ARTIKELNUMMER>

      <xsl:copy-of select = "./HERSTELLER" />

      <xsl:apply-templates select="VERPACKUNGS_EINHEIT"/>

      <xsl:copy-of select = "./ZUSATZ_ARTIKEL" />
      <xsl:copy-of select = "./ZUSATZ_ARTIKEL_OPTIONAL" />
      <xsl:apply-templates select="BESCHREIBUNG"/>
      <xsl:apply-templates select="LAENDER_FREIGABE"/>
      <PROGRAMM_FREIGABE>
        <DICTONARY_EINTRAG typekey="Solarschmiede.Artikel.Data.Enum.Programmart, ArtikelAdvanced, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" typevalue="Solarschmiede.Artikel.Data.Freigabe, ArtikelAdvanced, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null">
          <Programmart>CreoTool</Programmart>
          <Freigabe>
            <FREIGEGEBEN>true</FREIGEGEBEN>
          </Freigabe>
        </DICTONARY_EINTRAG>
      </PROGRAMM_FREIGABE>
      <xsl:apply-templates select="ARTIKEL_GRUPPE"/>
      <xsl:apply-templates select="SORTIERUNG"/>
      <xsl:apply-templates select="GEWICHT"/>
      <ARTIKELVERWENDBARKEIT>
        <!--<xsl:value-of select="./VERWENDBARKEIT/@Key" />-->
        nix
      </ARTIKELVERWENDBARKEIT>
    </ArrayOfSchiene>
  </xsl:template>

  <!--VERPACKUNGS_EINHEIT start-->
  <xsl:template match="VERPACKUNGS_EINHEIT">
    <VERPACKUNGS_EINHEIT>
      <PREIS Betrag="{./Verpackungseinheit/PREIS/@Wert}">
        <Waehrung TextKeyLang="" TextKeyKurz="{./Verpackungseinheit/PREIS/EINHEIT/@Key}" TextKeySymbol="" Decimals="{./Verpackungseinheit/PREIS/EINHEIT/@Decimals}" />
      </PREIS>
      <ANZAHL>
        <xsl:value-of select="./Verpackungseinheit/ANZAHL/@Wert" />
      </ANZAHL>
      <EINHEIT TextKeyLang="{./Verpackungseinheit/ANZAHL/EINHEIT/@TextKeyLang}" TextKeyKurz="{./Verpackungseinheit/ANZAHL/EINHEIT/@Key}" Decimals="{./Verpackungseinheit/ANZAHL/EINHEIT/@Decimals}" />
    </VERPACKUNGS_EINHEIT>
  </xsl:template>

  <!--Note: Comment above and uncomment following if you need all tags of Verpackungseinheit from source xml, above 1 gives u only 1 in output i.e. 1st-->
  <!--<xsl:template match="VERPACKUNGS_EINHEIT">
    <VERPACKUNGS_EINHEIT>
      <xsl:apply-templates select="Verpackungseinheit"/>
    </VERPACKUNGS_EINHEIT>
  </xsl:template>
  
   <xsl:template match="Verpackungseinheit">
      <xsl:apply-templates select="PREIS"/>
      <xsl:apply-templates select="ANZAHL"/>
      
  </xsl:template>
  
  <xsl:template match="PREIS">
     <PREIS Betrag="{@Wert}">
       <xsl:apply-templates select="./EINHEIT"/>
     </PREIS>
  </xsl:template>
  
  <xsl:template match="PREIS/EINHEIT">
    <Waehrung TextKeyLang="" TextKeyKurz="{@Key}" TextKeySymbol="" Decimals="{@Decimals}" />
  </xsl:template>

  <xsl:template match="ANZAHL">
    <ANZAHL>
      <xsl:value-of select="@Wert" />
    </ANZAHL>
    <xsl:apply-templates select="EINHEIT"/>
  </xsl:template>
  
  <xsl:template match="EINHEIT">
    <EINHEIT TextKeyLang="{@TextKeyLang}" TextKeyKurz="{@Key}" Decimals="{@Decimals}" />
  </xsl:template>-->

  <!--VERPACKUNGS_EINHEIT end-->

  <!--BESCHREIBUNG start-->
  <xsl:template match="BESCHREIBUNG">
    <BESCHREIBUNG>
      <xsl:apply-templates select="./DICTONARY_EINTRAG"/>
    </BESCHREIBUNG>
  </xsl:template>

  <xsl:template match="BESCHREIBUNG/DICTONARY_EINTRAG">
    <DICTONARY_EINTRAG typekey="{@typekey}" typevalue="{@typevalue}">
      <xsl:apply-templates select="BESCHREIBUNGSART"/>
      <xsl:apply-templates select="MEHRSPRACHIGERTEXT"/>
    </DICTONARY_EINTRAG>
  </xsl:template>

  <xsl:template match="BESCHREIBUNGSART">
    <BESCHREIBUNGSART Key="{@Key}" />
  </xsl:template>

  <xsl:template match="MEHRSPRACHIGERTEXT">
    <MehrsprachlicherText>
      <xsl:apply-templates select="./DICTONARY_EINTRAG"/>
    </MehrsprachlicherText>
  </xsl:template>

  <xsl:template match="MEHRSPRACHIGERTEXT/DICTONARY_EINTRAG">
    <DICTIONARY>
      <string>
        <xsl:value-of select="./CULTURE_INFO/CULTURE_KUERZEL"/>
      </string>
      <string>
        <xsl:value-of select="string"/>
      </string>
    </DICTIONARY>
  </xsl:template>

  <!--BESCHREIBUNG end-->

  <!--LAENDER_FREIGABE start-->
  <xsl:template match="LAENDER_FREIGABE">
    <LAENDER_FREIGABE>
      <xsl:apply-templates select="./DICTONARY_EINTRAG"/>
    </LAENDER_FREIGABE>
  </xsl:template>

  <xsl:template match="LAENDER_FREIGABE/DICTONARY_EINTRAG">
    <DICTIONARY>
      <string>
        <xsl:value-of select="./CULTURE_INFO/CULTURE_KUERZEL"/>
      </string>
      <Freigabe>
        <FREIGEGEBEN>
          <xsl:value-of select="./FREIGABE/@Wert"/>
        </FREIGEGEBEN>
      </Freigabe>
    </DICTIONARY>
  </xsl:template>

  <!--LAENDER_FREIGABE end-->

  <!--ARTIKEL_GRUPPE start-->
  <xsl:template match="ARTIKEL_GRUPPE">
    <ARTIKEL_GRUPPE>
      <GRUPPEN_NAME />
      <GRUPPE KEY="{./GRUPPENART/@Key}" />
    </ARTIKEL_GRUPPE>
  </xsl:template>

  <!--ARTIKEL_GRUPPE end-->

  <!--SORTIERUNG start-->
  <xsl:template match="SORTIERUNG">
    <SORTIERUNG>
    <DICTONARY_EINTRAG typekey="{./DICTONARY_EINTRAG/@typekey}"  typevalue="{./DICTONARY_EINTRAG/@typevalue}">
      <Sortierart KEY="{./DICTONARY_EINTRAG/SORTIERART/@Key}" />
      <Position>
        <POSITION>
          <xsl:value-of select="./DICTONARY_EINTRAG/POSITION/POSITION"/>
        </POSITION>
      </Position>
    </DICTONARY_EINTRAG>
    </SORTIERUNG>
  </xsl:template>
  <!--SORTIERUNG end-->

  <!--GEWICHT start-->
  <xsl:template match="GEWICHT">
    <GEWICHT_KG>
      <WERT Wert="{./SPEZIFISCH/@WERT}">
        <Einheit TextKeyLang="{./SPEZIFISCH/EINHEIT/@TextKeyLang}" TextKeyKurz="{./SPEZIFISCH/EINHEIT/@Key}" Decimals="{./SPEZIFISCH/EINHEIT/@Decimals}" />
      </WERT>
    </GEWICHT_KG>
  </xsl:template>
  <!--GEWICHT end-->
</xsl:stylesheet>