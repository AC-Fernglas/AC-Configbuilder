using System;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
namespace AC_Configurator
{ 
    class jsonconstructor  // funktioniert
    {
      
     
        public int network_dev { get; set; }
        public int vlan_id { get; set; }
        public int underlying_if { get; set; }
        public string name { get; set; }
        public string tagging { get; set; }

        private void construct(int NDEV, int vlan, int ulif, string NAme, string tag)
        {
                 network_dev = NDEV;
                 vlan_id = vlan;
                 underlying_if = ulif;
                 name = NAme;
                 tagging = tag;

        }

        public static void constructjson(int NDEV, int vlan, int ulif, string NAme, string tag)
        {
            jsonconstructor Config = new jsonconstructor
            {
                network_dev = NDEV,
                vlan_id = vlan,
                underlying_if = ulif,
                name = NAme,
                tagging = tag,

        };

            string myjson = JsonConvert.SerializeObject(Config, Formatting.Indented);
            Console.WriteLine(myjson);
            gethelp(Console.ReadLine());
        }

        public static void getjson()
        {
           jsonconstructor loadedjson = new jsonconstructor();
            using (StreamReader file = File.OpenText(@"..\..\..\..\config\Konfigurationen.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                
                loadedjson = (jsonconstructor)serializer.Deserialize(file, typeof(jsonconstructor));
            }

            Console.WriteLine("Welches Attribut möchtest du ändern ?");
            Console.WriteLine("Gib --help ein wenn du Hilfe brauchst.");
            gethelp(Console.ReadLine());


           
        }
        public static void gethelp(string command)
        {
            switch (command)
            {
                case "--help":
                    Console.Clear();
                    Console.WriteLine("Attributsübersicht");
                    Console.WriteLine();
                    Console.WriteLine("net_dev          Bearbeitet network_dev, gib eine Zahl/ ID für dieses Attribut an.");
                    Console.WriteLine("vlid             Bearbeitet vlan_id, gib eine Zahl/ ID für dieses Attribut an.");
                    Console.WriteLine("ulif             Bearbeitet underlying_if, gib eine Zahl/ ID für dieses Attribut an.");
                    Console.WriteLine("name             Bearbeitet name, gib eine Wort/Name für dieses Attribut an.");
                    Console.WriteLine("tag              Bearbeitet tag, gib eine tagged/untagged für dieses Attribut an.");
                    Console.WriteLine();
                    Console.WriteLine("--all            Lässt dich alle Attribute bearbeiten.");
                    Console.WriteLine("--save           Speichert deine Änderungen in die Konfigurationsdatei im config-Ordner.");
                    Console.WriteLine("--show           Zeigt die aktuelle Konfiguration.");
                    Console.WriteLine("--back           Verlassen des Konfigmenüs.");
                    gethelp(Console.ReadLine());
                    break;
                case "netdev":
                    setdata(1);
                    break;
                case "vlid":
                    setdata(2);
                    break;
                case "ulif":
                    setdata(3);
                    break;
                case "name":
                    setdata(4);
                    break;
                case "tag":
                    setdata(5);
                    break;
                case "--all": //worked
                    setdata(6);
                    break;
                case "--save":
                    
                    break;
                case "--show":
                    catchconfig();
                    break;
                case "--back": //worked
                    Console.Clear();
                    program.Main(null);
                    break;
                default:
                    Console.WriteLine("Dein Befehl existiert nicht.");
                    Console.WriteLine("Bitte gebe den Befehl ein mit dem du arbeiten möchtest.");
                    gethelp(Console.ReadLine());
                    break;
            }
        }

        public  static void setdata(int index)
        {
              int NDEV = 0;
              int vlan = 0;
              int ulif = 0;
              string NAme = "name";
              string tag = "tagged";

            switch (index)
            {
                case 1:
            Console.Write("Gib deine network-dev ein : ");
                NDEV = Convert.ToInt16(Console.ReadLine());
                    return;
                case 2:
            Console.Write("Gib deine vlan-id ein : ");
                vlan = Convert.ToInt16(Console.ReadLine());
                    return;
                case 3:
            Console.Write("Gib deine underlying_if ein : ");
                ulif = Convert.ToInt16(Console.ReadLine());
                    return;
                case 4:
                    Console.Write("Gib den name ein : ");
                    string name = Console.ReadLine();
                    return;
                case 5:
                    Console.Write("Soll das Attribut tagged sein ? Y/N : ");
                    string qwe = Console.ReadLine();
                    if (qwe == "Y" || qwe == "y")
                    {
                        tag = "tagged";
                    }
                    else if (qwe == "N" || qwe == "n")
                    {
                        tag = "untagged";
                    }
                    else
                    {
                        Console.WriteLine("Flasche eingabe."); setdata(5);
                    }
                        return;
                case 6:
                    for (int i = 1; i < 6; i++)
                    {
                        setdata(i);
                    }
                    constructjson(NDEV, vlan, ulif, NAme, tag);
                    return;
            default:

                    break;
            }

           
        } 

        public static void catchconfig() //aktuelle Konfiguration zusammsammeln und anzeigen
        {

        }

        public static void savejson(jsonconstructor myjson) //speichern der config    //benutzerdefinierte speicherpfadangabe ? -> speichern im subfolder?
        {
            using (StreamWriter file = File.CreateText(@"..\..\..\..\config\Konfigurationen.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, myjson);
            }
        }
    }


    class jsonconverter
    {
        public static void getdata(string data)
        {
            //daten aus den Sampels auslesen zum benutzen
        }
    }

}
