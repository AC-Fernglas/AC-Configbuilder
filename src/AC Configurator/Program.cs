using System;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using System.IO;
namespace AC_Configurator
{
    class program
    {
      
        public static void Main(string[] args)
        {
          Console.WriteLine("Willkommen bei AC-Configurator");
            Console.WriteLine("Bitte gib den Befehle ein, mit welchem du arbeiten möchtest.");
            Console.WriteLine("Gibt --help ein um eine übersicht über alle Befehle zu bekommen.");
            command(Console.ReadLine());

          
        }


        public static void command(string commandinput)
        {
            switch (commandinput)
            {
                case "--help":
                    Console.Clear();
                    Console.WriteLine("Befehlsübersicht");
                    Console.WriteLine();
                    Console.WriteLine("--help       Dieser Befehl zeigt eine Übersicht über alle verfügbaren Befehle.");
                    Console.WriteLine("--config     Dieser Befehl ruft die derzeitige Kofiguration auf und lässt sie dich bearbeiten.");
                    Console.WriteLine("--safe       Dieser Befehl speichert deine Konfigurationsänderungen");
                    command(Console.ReadLine());
                    break;
                case "--config":
                    Console.Clear();
                    Console.WriteLine("Konfigurationseditor");
                    Console.WriteLine();
                    jsonconstructor.getjson();
                    break;
                case "--loadconfig": //config aus bestehendem sampel bauen
                    break;
                default:
                    Console.WriteLine("Dein Befehl existiert nicht.");
                    Console.WriteLine("Bitte gebe den Befehl ein mit dem du arbeiten möchtest.");
                    command(Console.ReadLine());
                    break;
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
