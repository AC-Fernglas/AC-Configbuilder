using System;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
namespace AC_Configurator
{
    class Program
    {
        static void Main(string[] args)
        {
            explanations.Introduction();
        }
    }

     class explanations
    {
        public void Introduction()
        {
            Console.WriteLine("Hallo, Willkommen im AC-Konfigurator");
            Console.Write("Möchtest du die Konfiguration bearbeiten? Y/N :");
            while (Console.ReadLine() != "y" && Console.ReadLine() != "Y" && Console.ReadLine() != "N" && Console.ReadLine() != "n")
            {
                Console.WriteLine("Flasche Eingabewerte.");
                Console.Write("Möchtest du die Konfiguration bearbeiten? Y/N :");

            }
            if (Console.ReadLine() == "y" || Console.ReadLine() == "Y")
            {
               Konfigurationen.RdWrJsonFile();
                Console.ReadLine();
                
            }
      
        } 

    }

         class Konfigurationen
    {
        public int network_dev { get; set; }
        public int vLan_id { get; set; }
        public string underlying_if { get; set; }
        public string name { get; set; }
        public string tagged { get; set; }
     
           
        
        public static void RdWrJsonFile()
        {
            string filepath = "../config/Konfigurationen.json";
            string readResult = string.Empty;
            string writeResult = string.Empty;
            using (StreamReader r = new StreamReader(filepath))
            {
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);
                readResult = jobj.ToString();
              
                writeResult = jobj.ToString();
                Console.WriteLine(writeResult);
            }
            Console.WriteLine(readResult);
            File.WriteAllText(filepath, writeResult);
        }
    }
}
}
