using System;
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

          
          return  app.Execute(commands);
        }
        public void editmethod(CommandOption add, CommandOption del, CommandOption path)
        {
            string _path = @"..\config\qwertz.txt";      
            if (path.HasValue() && path.Value() != " " && path.Value() != null)
            {
                _path = path.Value().ToString(); 
                if (!File.Exists(_path))
                {
                    Console.WriteLine("Bitte überprüfe deinen Path.");
                    return;
                }
            
           
            }
            if (add.HasValue() && add.Value() != " " && add.Value() != null)
            {
                //Hinzufügen einer Eigenschaft
            }

            if (del.HasValue() && del.Value() != " " && del.Value() != null) //Löschen einer Eigenschaft
            {
                StringBuilder newFile = new StringBuilder();
                string temp = "";

                string[] file = File.ReadAllLines($@"{_path}");

                foreach (string line in file)

                {

                    if (line.Contains(del.Value().ToString()))

                    {
                       
                        temp = line.Replace(del.Value().ToString(), " ");

                        temp = temp.Trim(new Char[] { '"', ':', ' ' ,'\n'});
                        Console.WriteLine(temp);

                        newFile.Append(temp + "\r\n");

                        continue;

                    }

                    newFile.Append(line);

                }

                File.WriteAllText($@"{_path}", newFile.ToString());
            }
        }
    }
}
