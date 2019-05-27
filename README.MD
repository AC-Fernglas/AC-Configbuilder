# AC-Configbuilder

[//]: # (TOC Begin)
* [Überblick](#überblick)
* [Installation](#installation)
* [Benutzung](#benutzung)
* [Replace-Command](#replace-command)
* [Options für die Commands](#options-für-die-commands)
	* [Replace](#replace)
	* [Create](#create)
	* [Release Note](#release-notehttpsgithub.comac-fernglasac-configbuilderblobver.01releasenote.md)

[//]: # (TOC End)


## Überblick

Der AC-Configbuilder soll eine Automatisierung für das Erstellen von Konfigurationsdatein für AudioCode werden.
Er erstellt lerre Vorlagen der gewünschten Konfiguration oder ersetzt bestimme Teile einer bereits bestehenden.



| Build           | Master-Status                            | NuGet           | Packagestatus                            |
|-----------------|------------------------------------------|-----------------|------------------------------------------|
| ACConfigBuilder | [![Build Status](https://dev.azure.com/dominikmangatter/ACConfigBuilder/_apis/build/status/AC-Fernglas.AC-Configbuilder%20(1)?branchName=master)](https://dev.azure.com/dominikmangatter/ACConfigBuilder/_build/latest?definitionId=7&branchName=master) | ACConfigbuilder | [![ACConfigbuilder package in artifact_ACB feed in Azure Artifacts](https://feeds.dev.azure.com/dominikmangatter/_apis/public/Packaging/Feeds/029c8be4-1017-41ac-9482-75d3c6d96884/Packages/62646a40-7810-4d42-9c39-b8b78d3f5ba8/Badge)](https://dev.azure.com/dominikmangatter/ACConfigBuilder/_packaging?_a=package&feed=029c8be4-1017-41ac-9482-75d3c6d96884&package=62646a40-7810-4d42-9c39-b8b78d3f5ba8&preferRelease=true) |

## Installation

Für die Installation wird [.Net Core 2.2 SDK](https://dotnet.microsoft.com/download) oder neuer gebraucht. <br>
Ist .Net Core 2.2 SDK installiert, benutze folgenden Code.

```bash
dotnet tool install --global ACConfigBuilder
```

Hast du schon eine ältere Version istalliert und möchtest diese auf eine neuere updaten, benutze folgenden Code.

```bash
dotnet tool update --global ACConfigBuilder
```


## Benutzung

```bash
Usage: acb [options] [command]

Options:
  -h|--help  Show help information

Commands:

  create     Erstellt eine neue Configvorlage.
  replace    Dieser Befehl soll es ermöglichen die hinterlegte Konfiguration zu editieren. 
```

## Replace-Command

Der Replace-Command soll eine bereits bestehende Konfiguration editieren. 
Dazu muss man in der Change.json im Configordner angeben, wo etwas geändert werden muss.

<details close>

<summary>**Beispiel für Change.json**</summary>

```
{
    "configurenetwork": {
      "networkdev": [
        {
          "network-dev" :  <\value>,
          "vlan-id " : <\value>,
          "underlying-if" : <\value>,
          "name" : <\value>,
          "tagging" : <\value>,
          "activate"
        }
      ],
      "interfacenetworkif":[
        {
           "interface network-if" : <\value>,
           "application-type" : <\value>,
           "ip-address" : <\value>,
           "prefix-length" : <\value>,
           "gateway" : <\value>,
           "name" : <\value>,
           "underlying-dev" : <\value>, 
           "activate"
        }
      ]
    },
    "configureviop":{
      "proxyset":[
        {
            "proxy-set" : <\value>, 
            "proxy-name" : <\value>, 
            "proxy-enable-keep-alive" : <\value>,  
            "srd-name" : <\value>, 
            "sbcipv4-sip-int-name" : <\value>,  
            "keepalive-fail-resp" : <\value>, 
            "success-detect-retries" : <\value>, 
            "success-detect-int" :  <\value>, 
            "proxy-redundancy-mode" : <\value>, 
            "is-proxy-hot-swap" : <\value>, 
            "proxy-load-balancing-method" : <\value>, 
            "min-active-serv-lb" : <\value>, 
            "activate"
        }
      ],
      "proxyip":[
        {
          "proxy-ip" : <\value>,
          "proxy-address" : <\value>,
          "transport-type" : <\value>,
          "activate"
        }
      ]
    }
}
```  
</details>

## Options für die Commands

### Replace

Der Replace-Command hat eine zusätzliche Option.

```bash 
acb replace --path <path>
```

Mit der Option `--path <path>` wird dauerhaft der Pfad, in welchem das Programm nach Konfigurationen zum überarbeiten sucht verädnert.
Default ist der Wert auf den Samplesordner gelegt.

### Create

Der Create-Command erzuegt eine leere AudioCodes Konfiguration mit bestimmten Eigenschaften. Diese dient als Standardtemplate für mehrere Konfigurationen die dann über ein `replace` angepasst werden können. Hat man bereits eine AudioCodes Konfiguration braucht man den Create Befehl nicht.


#### Parameter

| Parameter              | Beschreibung                             | Standardwert                             | Erforderlich | Typ    |
|------------------------|------------------------------------------|------------------------------------------|--------------|--------|
| `--path`               | Setzt den Pfad in dem eine neue AudioCodes Konfiguration abgelegt wird. | aktueller Pfad in der Konsole            | nein         | string |
| `--config`             | Don't do anything                        | Pfad zur Standard ACB Konfiguration die mit dem Tool mitgeliefert wird | nein         | string |
| `--template`           | Setzt den Pfad zum Template Verzeichnis. Dort liegen AudioCodes Konfiguration Blöcke die das Tool Standardmäßig durch AC-Configuration Builder ausgeliefert werden. | Pfad zum Standard ACB Template Verzeichnis. Dort liegen AudioCodes Konfiguration Blöcke die das Tool Standardm | nein         | string |
| `--networkdev`         | Eine Anzahl von Netzwerkgeräten die in der generierten AudioCodes Konfiguration vorhanden sein sollen. Diese werden leer generiert und sind abhänig von dem Template Blöcken welche verwendet werden. | 1                                        | nein         | number |
| `--interfacenetworkif` | Eine Anzahl von "to be defined" die in der generierten AudioCodes Konfiguration vorhanden sein sollen. Diese werden leer generiert und sind abhänig von dem Template Blöcken welche verwendet werden. | 1                                        | nein         | number |
| `--proxyset`           | Eine Anzahl von Proxys die in der AudioCodes Konfiguration erzeugt werden sollen. Diese werden leer generiert und sind abhänig von dem Template Blöcken welche verwendet werden. | 1                                        | nein         | number |
| `--proxyip`            | Eine Anzahl von ProxyIps die in der AudioCodes Konfiguration erzeugt werden sollen. Diese werden leer generiert und sind abhänig von dem Template Blöcken welche verwendet werden. | 1                                        | nein         | number |



#### Examples

```bash
acb create --path <path>
```

Mit der Option `--path <path>` wird der Pfad, welcher angibt wo die leere Konfiguration erstellt werden soll, für diese Ausführung verändert.

```bash
acb create --networkdev <anzahl>
```

Mit der Option `--networkdev <anzahl>` wird die Anzahl der network-dev Blöcken varriert, standartmäßig ist diese auf 1 gesetzt.

```bash
acb create --interfacenetworkif <anzahl>
```

Mit der Option ```--interfacenetworkif <anzahl>``` wird die Anzahl der interface network-if Blöcken varriert, standartmäßig ist diese auf 1 gesetzt.

```bash
acb create --proxyset <anzahl>
```

Mit der Option ```--proxyset <anzahl>``` wird die Anzahl der proxy-set Blöcken varriert, standartmäßig ist diese auf 1 gesetzt.

```bash
acb create --proxyip <anzahl>
```

Mit der Option `--proxyip <anzahl>` wird die Anzahl der proxy-ip Blöcken varriert, standartmäßig ist diese auf 1 gesetzt.

### [Release Note](https://github.com/AC-Fernglas/AC-Configbuilder/blob/master/ReleaseNote.md)

Danke das du dir die Zeit genommen hast mich zu lesen.

Habe einen schönen Tag :)
