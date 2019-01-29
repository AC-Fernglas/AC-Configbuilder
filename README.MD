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
ListId, beschreibt in diesem Kontext, den Wert, welcher nach ```network-dev``` , ```ìnterface network-if``` oder ```proxy-set``` kommen würde. <br>
Damit beschreibt ListId, welcher Abschnitt in der Konfiguration bearbeitet werden soll. <br>


## Options für die Commands
### Replace
Der Replace-Command hat eine zusätzliche Option. <br>
``` replace --path <path>```
Mit der Option ```--path <path>``` wird dauerhaft der Pfad, in welchem das Programm nach Konfigurationen zum überarbeiten sucht verädnert. <br>
Default ist der Wert auf den Samplesordner gelegt.

### Create
Der Create-Command hat mehrere zusätzliche Optionen. <br>
``` create --path <path>```
Mit der Option ```--path <path>``` wird der Pfad, welcher angibt wo die leere Konfiguration erstellt werden soll, für diese Ausführung verändert. <br>
<br>
``` create --networkdev <anzahl>```
Mit der Option ```--networkdev <anzahl>``` wird die Anzahl der network-dev Blöcken varriert, standartmäßig ist diese auf 1 gesetzt. <br>
<br>
``` create --interfacenetworkif <anzahl>```
Mit der Option ```--interfacenetworkif <anzahl>``` wird die Anzahl der interface network-if Blöcken varriert, standartmäßig ist diese auf 1 gesetzt. <br>
<br>
``` create --proxyset <anzahl>```
Mit der Option ```--proxyset <anzahl>``` wird die Anzahl der proxy-set Blöcken varriert, standartmäßig ist diese auf 1 gesetzt. <br>
<br>
``` create --proxyip <anzahl>```
Mit der Option ```--proxyip <anzahl>``` wird die Anzahl der proxy-ip Blöcken varriert, standartmäßig ist diese auf 1 gesetzt. <br>

### [Release Note](https://github.com/AC-Fernglas/AC-Configbuilder/blob/Ver.01/ReleaseNote.md)

Danke das du dir die Zeit genommen hast mich zu lesen. <br>
Habe einen schönen Tag :)
