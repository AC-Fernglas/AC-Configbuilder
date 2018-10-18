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
            string _path = @"..\config\qwertz.json";
            if (validpath(path) != " ")
            {
                _path = validpath(path);
            }
            if (add.HasValue() && add.Value() != " " && add.Value() != null)
            {
                //Hinzufügen einer Eigenschaft
            }

            if (del.HasValue() && del.Value() != " " && del.Value() != null) //Löschen einer Eigenschaft
            {
                StringBuilder newFile = new StringBuilder();
               

                string[] file = File.ReadAllLines($@"{_path}");
                List<string> list = new List<string>(file);
                foreach (string line in list)

                {

                    if (line.Contains(del.Value().ToString()))

                    {
                        continue;
                    }

                    newFile.Append(line);

                }

                File.WriteAllText($@"{_path}", newFile.ToString());
           
            }
        
        }
    

        public void useon(CommandOption path, CommandOption filenumber)
        {
            string _path = @"..\samples";
            int amm = 1;
            string configpath = @"..\config\qwertz.json";
            if (validpath(path) != " ")
            {
                _path = validpath(path);
            }
            if (filenumber.HasValue() && filenumber.Value() != " " && filenumber.Value() != null)
            {
                amm = int.Parse(filenumber.Value());
            }
            for (int i = 0; i < amm; i++)
            {
                DateTime date = new DateTime();
                File.Create(_path + "file" + date.DayOfYear.ToString() + date.Minute.ToString() +".txt");
                TextWriter tw = new StreamWriter(_path);
                string[] file = File.ReadAllLines($@"{configpath}");

                for (int a = 0; i < file.Length; i++)
                {
                    tw.WriteLine(file[a]);
                } 

            }
            
        }

        public string validpath(CommandOption path)
        {
            if (path.HasValue() && path.Value() != " " && path.Value() != null)
            {
                string _path = path.Value().ToString();
                if (!File.Exists(_path))
                {
                    Console.WriteLine("Bitte überprüfe deinen Path.");
                    return " ";
                }

                return _path;
            }
            return " ";
        }
    }
}
