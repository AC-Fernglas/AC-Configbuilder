# AC-Configbuilder

Der AC-Configbuilder soll eine automatisierung für das Erstellen von Konfigurationsdatein für AudioCode werden. <br>
Dazu liest es durch einen Parser der mit [Sprache](https://github.com/sprache/Sprache) erstellt wurde eine zu veränderende Konfiguration ein und <br> 
ersetzt gewünschte Datein.

## Struktur

Der AC-Configbuilder ist eine Consolenanwendung, welche über Commands gesteuert werden soll.


### Was soll es können
**1.  .config Datei erstellen, die AC-Datein n-fach erstellen kann.**
   * Config Strucktur erstellen [GUID](https://www.audiocodes.com/media/13244/gateway-and-sbc-cli-reference-guide-ver-72.pdf)
   * .json file für Config
   * Konfigurationen als Template definieren

**2. ID's über API ermitteln und einfügen**
  * API ansprechen [GUID](https://www.audiocodes.com/media/13528/rest-api-for-mediant-devices-ver-72.pdf)
  * festgelegte ID's abrufen und übernehmen
 
**3. Konfigurationen automatisch einspielen**
  * n-viele Konfiguratioenn einspielen [GUID](https://www.audiocodes.com/media/13528/rest-api-for-mediant-devices-ver-72.pdf)
  * multible Devices zulassen

### [Release Note](https://github.com/AC-Fernglas/AC-Configbuilder/blob/Ver.01/ReleaseNote.md)

Danke das du dir die Zeit genommen hast mich zu lesen. 
Habe einen schönen Tag :)
