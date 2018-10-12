using System;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
namespace AC_Configurator
{
    class programm
    {
        public static void  Main(string[] args)
        {
            jsonconstructor.getjson();
            jsonconstructor.constructjson();
        }
    }
    class jsoncoverter
    {
       

      
    }

    class jsonconstructor  // funktioniert
    {
        public int  ALTER { get; set; }
        public int HÖHE { get; set; }
        public int GEWICHT { get; set; }
        public string ART { get; set; }

        private void construct(int age, int height, int weight, string species)
        {
            ALTER = age;
            HÖHE = height;
            GEWICHT = weight;
            ART = species;

        }

        public static void constructjson()
        {
            jsonconstructor Baum = new jsonconstructor
            {
                ALTER = 12,
                HÖHE = 1234,
                GEWICHT = 23476,
                ART = "Eiche"
            };

            string myjson = JsonConvert.SerializeObject(Baum, Formatting.Indented);
            Console.WriteLine(myjson);
            setjson(Baum);
            Console.ReadLine();
        }

        public static void getjson()
        {
            jsonconstructor Baum1 = new jsonconstructor();
            using (StreamReader file = File.OpenText(@"..\..\..\..\config\Konfigurationen.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                
                Baum1 = (jsonconstructor)serializer.Deserialize(file, typeof(jsonconstructor));
            }
            Console.WriteLine(Baum1.ALTER);
            setjson(Baum1);
            Console.ReadLine();
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
}
