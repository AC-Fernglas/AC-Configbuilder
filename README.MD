# AC-Configbuilder

## Überblick
Der AC-Configbuilder soll eine automatisierung für das Erstellen von Konfigurationsdatein für AudioCode werden. <br>
Er erstellt lerre Vorlagen der gewünschten Konfiguration oder ersetzt bestimme Teile einer bereits bestehenden.

## Installation

Für die Installation wir [.Net Core 2.2 SDK](https://dotnet.microsoft.com/download) oder neuer gebraucht. <br>
Ist .Net Core 2.2 SDK installiert, benutze folgenden Code <br>
<br>
```dotnet tool install --global ACConfigBuilder``` <br>
<br>
Hast du schon eine ältere Version istalliert und möchtest diese auf eine neuere updaten, benutze folgenden Code <br>
<br>
```dotnet tool update --global ACConfigBuilder``` <br>
<br>

## Benutzung
```
Usage: [options] [command]

Options:
  -h|--help  Show help information

Commands:

  create     Erstellt eine neue Configvorlage.
  replace    Dieser Befehl soll es ermöglichen die hinterlegte Konfiguration zu editieren. 
```
## Replace-Command
Der Replace-Command soll eine bereits bestehende Konfiguration editieren. <br> 
Dazu muss man in der Change.json im Configordner angeben, wo etwas geändert werden muss.
<details close>
<summary>Beispiel für Change.json</summary>

```
{
    "configurenetwork": {
      "networkdev": [
        {
          "listid": <\value>,
          "vlan": "<\value>",
          "underlyingif": "<\value>",
          "Name" : "<\value>",
          "tag": "<\value>"
        }
      ],
      "interfacenetworkif":[
        {
          "listid": <\value>,
          "apptype": "<\value>",
          "ipaddress":"<\value>",
          "prefixlength": <\value>,
          "gateway": "<\value>",
          "Name": "<\value>",
          "underlyingdev": "<\value>"
        }
      ]
    },
    "configureviop":{
      "proxyset":[
        {
          "listid": <\value>,
          "proxyname": "<\value>",
          "proxyenablekeepalive": "<\value>",
          "srdname":"<\value>",
          "sbcipv4sipintname": "<\value>",
          "keepalivefailresp":"<\value>",
          "successdetectretries": <\vaule>,
          "successdetectint" : <\value>,
          "proxyredundancymode" : "<\value>",
          "isproxyhotswap": <\value>,
          "proxyloadbalancingmethod": <\value>,
          "minactiveservlb": <\value>
        }
      ],
      "proxyip":[
        {
          "ip": "<\value>",
          "proxyadress": "<\value>",
          "transporttype": "<\value>"
        }
      ]
    }
}
```  

</details>

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
