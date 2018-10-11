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
            jsoncoverter.getjson();
        }
    }
    class jsoncoverter
    {
        public static void getjson()
        {
            string json = JsonConvert.SerializeObject(File.ReadAllText(@"C:\Develop\AC-Configbuilder\config\Konfigurationen.json"), Formatting.Indented);
            Console.WriteLine(json);

            jsonconstructor Baum = new jsonconstructor
            {
                ALTER = 12,
                HÖHE = 1234,
                GEWICHT = 23476,
                ART = "Eiche"

            };

          string myjson = JsonConvert.SerializeObject(Baum, Formatting.Indented);
            Console.ReadLine();
        }

    }

    class jsonconstructor
    {
        public int  ALTER { get; set; }
        public int HÖHE { get; set; }
        public int GEWICHT { get; set; }
        public string ART { get; set; }

        public void construct(int age, int height, int weight, string species)
        {
            ALTER = age;
            HÖHE = height;
            GEWICHT = weight;
            ART = species;

        } 
    }
}
