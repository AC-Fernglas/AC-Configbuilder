using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;

namespace secondtry
{
    class Program
    {
        static void Main(string[] args)
        {
           commands obj = new commands();
           
           obj.Idel(args);
        }
    }

    class commands
    {
        public int Idel(string [] commands)
        {
            var app = new CommandLineApplication();
            var helptemplate = "-h|--help";
           
            app.HelpOption(helptemplate);
            app.Command("edit", e => {
            e.HelpOption(helptemplate);
            e.Description = "Dieser Befehl soll es ermöglichen die hinterlegte Konfiguration zu editieren.";
            var add = e.Option("--add <Property>", "Fügt eine Eigenschaft in die bestehende Konfiguration hinzu.", CommandOptionType.SingleValue);
            var del = e.Option("--del <Property>", "Löscht eine bestehende Eigenschaft aus der Konfiguration.", CommandOptionType.SingleValue);
            var path = e.Option(@"--path <fullpath>", "Setzt einen benutzerdefinierten Pfad, wenn dieser Befehl nicht benutzt wird, wird die Standardconfig benutzt.", CommandOptionType.SingleValue);
            e.OnExecute(() => { editmethod(add,del,path); }
                    );
            });
            app.Command("use", u => {
                u.HelpOption(helptemplate);
                u.Description = "Dieser Befehl soll es ermöglichen die hinterlegte Konfiguration zu editieren.";
                var path = u.Option(@"--path <fullpath>", "Setzt einen benutzerdefinierten Pfad, wenn dieser Befehl nicht benutzt wird, wird der Sampelsordner benutzt.", CommandOptionType.SingleValue);
                u.OnExecute(() => { useon(path); });
            });



          return  app.Execute(commands);
        }
        public void editmethod(CommandOption add, CommandOption del, CommandOption path) //benutzerdefinierten path in hauptconfig schreiben und dort verwalten
        {
            string mypath = @".\config\qwertz.json";
            if (validpath(path) != " ")
            {
               mypath = validpath(path);
                setuserpath(mypath);
            }
            if (add.HasValue() && add.Value() != " " && add.Value() != null)
            {
                //Hinzufügen einer Eigenschaft
            }
            if (del.HasValue() && del.Value() != " " && del.Value() != null) //Löschen einer Eigenschaft
            {
                StringBuilder newFile = new StringBuilder();
                string[] file = File.ReadAllLines($@"{mypath}");
                List<string> list = new List<string>(file);
                foreach (string line in list)
                {
                    if (line.Contains(del.Value().ToString()))
                    {
                        continue;
                    }
                    newFile.Append(line);
                }
                File.WriteAllText($@"{mypath}", newFile.ToString());
            }        
        }
        public void setuserpath(string mypath)
        {
            string[] userconfig = File.ReadAllLines(@".\config\qwertz.json");
            StringBuilder newFile = new StringBuilder();
                string[] file = File.ReadAllLines($@".\config\qwertz.json");
                List<string> list = new List<string>(file);
                list[2] =  "\"userpath\": " + "\""+ mypath +"\"";
                foreach (string line in list)
                {
                    newFile.Append(line + "\n");
                }
                File.WriteAllText($@".\config\qwertz.json", newFile.ToString());
            
        }
        public void useon(CommandOption path) // neue funktion -> suchen und ersetzten 
        {
            string mypath = $@".\samples";
            if (validpath(path) != " ")
            {
                mypath = validpath(path);
            }
            openfiles(mypath);
        }
        public void openfiles(string mypath)
        {
            var newFile = new StringBuilder();
            var config = File.ReadAllText(@".\config\qwertz.json"); //get json
            var host = JsonConvert.DeserializeObject<JsonObjects>(config); //get path to json
            var myconfig = JsonConvert.DeserializeObject<JsonObjects>(File.ReadAllText(host.userpath));//open json to use
            string[] dirs = Directory.GetFiles(mypath, "*.txt",SearchOption.TopDirectoryOnly);
            foreach (var item in dirs)
            {
                string[] file = File.ReadAllLines(item);
               
                foreach (var lines in file)
                {
                    Console.WriteLine(lines);
                    Console.ReadLine();
                    

                            if (lines.Contains(myconfig.configureNetwork.name))
                            {
                                Console.WriteLine();
                                Console.ReadLine();
                                newFile.Append(" ");
                                continue;
                            }
                        
                    
                    newFile.Append(lines);
                }
                File.WriteAllText(item, newFile.ToString());
            }

               
        File.WriteAllText(mypath +".txt", newFile.ToString());    
        }
        public string validpath(CommandOption path)
        {
            if (path.HasValue() && path.Value() != " " && path.Value() != null)
            {
                string mypath = path.Value().ToString();
                if (!File.Exists(mypath))
                {
                    Console.WriteLine("Bitte überprüfe deinen Path.");
                    return " ";
                }
                return mypath;
            }
            return " ";
        }
    }
}
