# AC-Configbuilder

Der AC-Configbuilder soll eine automatisierung für das Erstellen von Konfigurationsdatein für AudioCode werden. <br>
Er erstellt lerre Vorlagen der gewünschten Konfiguration oder ersetzt bestimme Teile einer bereits bestehenden.

## Struktur

Der AC-Configbuilder ist eine Consolenanwendung, welche über Commands gesteuert werden soll. <br>

Mit dem Befehl ```--replace``` ist es möglich bestehende Konfigurationen zu bearbeiten. <br>
Durch die Option ```--path <pathtothedirectory>```kann der Benutzer das Verzeichniss in welchem Konfigurationsdatein geändert werden soll ändern. <br>

Der Befehl ```--create```erstellt eine neue konfigurationsdatei. <br>
Durch die Option ```--path <pathtothedirectory>```kann der Benutzer das Verzeichniss in welchem Konfigurationsdatein erstellt werden soll ändern. <br>
Die Anzahl der einzelnen, von der Datei zu konfigurienden Abschnitte, können mit ```--networkdev <Anzahl>``` , ```--interfacenetworkif <Anzahl>``` , ```--proxyset <Anzahl>``` und ```--proxyip <Anzahl>``` variiertwerden.

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
