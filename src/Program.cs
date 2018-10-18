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
                var filenumber = u.Option("--fn <number>", "Anzahl, wie viele Datein entstehen sollen. ", CommandOptionType.SingleValue);
                var path = u.Option(@"--path <fullpath>", "Setzt einen benutzerdefinierten Pfad, wenn dieser Befehl nicht benutzt wird, wird der Sampelsordner benutzt.", CommandOptionType.SingleValue);
                u.OnExecute(() => { useon(path, filenumber); });
            });



          return  app.Execute(commands);
        }
        public void editmethod(CommandOption add, CommandOption del, CommandOption path) //benutzerdefinierten path in hauptconfig schreiben und dort verwalten
        {
            string mypath = @"..\config\qwertz.json";
            if (validpath(path) != " ")
            {
                   mypath = validpath(path);
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
    

        public void useon(CommandOption path, CommandOption filenumber)
        {
            string mypath = @"..\samples\";
            int amm = 1;
            string configpath = @"..\config\qwertz.json";
            if (validpath(path) != " ")
            {
                mypath = validpath(path);
            }
            if (filenumber.HasValue() && filenumber.Value() != " " && filenumber.Value() != null)
            {
                amm = int.Parse(filenumber.Value());
            }
            for (int i = 0; i < amm; i++)
            {
               
                string filename = String.Format("{0}__{1}" + ".txt", "file", DateTime.Now.ToString("hh.mm.dd.MM.yy"));
                File.Create(mypath + filename);
                
                string [] file = File.ReadAllLines($@"{configpath}");
                StringBuilder newFile = new StringBuilder();
                foreach (string item in file)
                {
                    newFile.Append(item);
                    
                }

                File.WriteAllText($@"{mypath + filename}", newFile.ToString());


            }
            
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
