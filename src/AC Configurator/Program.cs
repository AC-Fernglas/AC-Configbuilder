using System;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
namespace AC_Configurator
{
    class programm
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Willkommen bei AC-Configurator");
            Console.Write("Möchtest du deine Configeinstellungen bearbeiten? Y/N: ");
            string qs = Console.ReadLine();
            if (qs == "Y" || qs == "y")
            {
                jsonconstructor.getjson();
            }else if (qs == "N" || qs == "n") { 

                    Console.WriteLine("In Welchem Verzeichnis möchtest du arbeiten?");
                    string subdictionary = Console.ReadLine();
                    Console.WriteLine("Wie heißt die Datei?");
                    string filename = Console.ReadLine();
                    
                    Console.Write("Sollen diese Daten als Vorlage für dieses Verzeiniss verwendet werden? Y/N :");
                    qs = Console.ReadLine();
                if (qs == "Y" || qs == "y")
                {
                    jsonconverter.getdata(qwert.loadfilesetting(subdictionary, filename));


                }
            }
        }
    }

    class qwert
    { // ordentliche benennung überlegen
        public static string loadfilesetting(string verzeichniss , string filename )
        {
          return File.ReadAllText($@"..\..\..\..\samples\{verzeichniss}\{filename}.txt");
      
            
        }



    }
}
