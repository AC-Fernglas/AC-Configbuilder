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

            Console.WriteLine("Möchtest du sie speichern ? Y/N :");
            string qe = Console.ReadLine();
            if (qe == "Y" || qe == "y")
            {
                setjson(Config);
            }
        }

        public static void getjson()
        {
            jsonconstructor Baum1 = new jsonconstructor();
            using (StreamReader file = File.OpenText(@"..\..\..\..\config\Konfigurationen.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                
                Baum1 = (jsonconstructor)serializer.Deserialize(file, typeof(jsonconstructor));
            }
            
   


            Console.Write("network-dev: ");
            int NDEV = Convert.ToInt16(Console.ReadLine());
            Console.Write("vlan-id: ");
            int vlan = Convert.ToInt16(Console.ReadLine());
            Console.Write("underlying-if: ");
            int ulif = Convert.ToInt16(Console.ReadLine());
            Console.Write("name: ");
            string NAme = Console.ReadLine();
            Console.Write("tagging: ");
            string tag = Console.ReadLine();

            constructjson(NDEV,vlan, ulif, NAme,tag);
        }

        public static void setjson(jsonconstructor myjson)
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

        }
    }

}
