using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Sprache;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace ACConfigBuilder
{
    class Program
    {
        static void Main(string[] args)

        {
            try
            {
                Commands obj = new Commands();
                obj.Idel(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
    public class Commands
    {
        public int Idel(string[] commands)  // start 
        {
            var app = new CommandLineApplication();
            Execute obj = new Execute();
            var helptemplate = "-h|--help"; //definition of the helptemplate
            app.HelpOption(helptemplate);
                app.Command("replace", u => //should search and replace configs that allready exist
            {
                u.HelpOption(helptemplate);
                u.Description = "Dieser Befehl soll es ermöglichen die hinterlegte Konfiguration zu editieren.";
                var path = u.Option("--path <fullpath>", "Setzt einen dauerhaften benutzerdefinierten Pfad. Wenn dieser Befehl nicht benutzt wird, wird der Pfad , welcher in der Config.json als userpath angegeben ist verwendet.", CommandOptionType.SingleValue);
                var configPath = u.Option("--config <fullpath>", "Benutzt ein benutzerdefinierte Konfiguration. Wenn dieser Befehl nicht benutzt wird die Standardkonfiguration verwendet.", CommandOptionType.SingleValue);
                var templatePath = u.Option("--template <fullpath>", "Benutzt einen benutzerdefiniertes Templateverzeichnis. Wenn dieser Befehl nicht benutzt wird, werden die Standardtemplates verwendet.", CommandOptionType.SingleValue);
                u.OnExecute(() => { obj.runReplace(path, configPath, templatePath); });
            });
            app.Command("create", c => //creats a new config 
            {
                c.HelpOption(helptemplate);
                c.Description = "Erstellt eine neue Configvorlage.";
                var path = c.Option("--path <fullpath>", "Benutzt einen benutzerdefinierten Pfad. Wenn dieser Befehl nicht benutzt wird, wird der Pfad , welcher in der Config.json als changeDirectory angegeben ist verwendet.", CommandOptionType.SingleValue);
                var configPath = c.Option("--config <fullpath>", "Benutzt ein benutzerdefinierte Konfiguration. Wenn dieser Befehl nicht benutzt wird die Standardkonfiguration verwendet.", CommandOptionType.SingleValue);
                var templatePath = c.Option("--template <fullpath>", "Benutzt einen benutzerdefiniertes Templateverzeichnis. Wenn dieser Befehl nicht benutzt wird, werden die Standardtemplates verwendet.", CommandOptionType.SingleValue);
                var Net = c.Option("--networkdev <anzahl>", "Setzt die Anzahl für Networkdevabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Int = c.Option("--interfacenetworkif <anzahl>", "Setzt die Anzahl für Interfacenetworkifabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Set = c.Option("--proxyset <anzahl>", "Setzt die Anzahl für Proxysetabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Ip = c.Option("--proxyip <anzahl>", "Setzt die Anzahl für Proxyipabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                c.OnExecute(() => { obj.RunCreate(path, configPath, templatePath, Net, Int, Set, Ip); });
            });
            app.Command(null, c =>
            {
                app.ShowHelp();
            });
            return app.Execute(commands);

        }
    }

    public class Execute
    {
        protected void setuserpath(string configPath, string changePath)
        {
            StreamWriter writer = new StreamWriter(configPath);
            string[] file = File.ReadAllLines(configPath + @"\Config.json");
            List<string> list = new List<string>(file);
            list[3] = "\"changeDirectory\": " + changePath;
            foreach (string line in list)
            {
                writer.WriteLine(line);
            }
        }
        public void runReplace(
            CommandOption Path,
            CommandOption configPath,
            CommandOption templatePath) //run for replace
        {
            Execute exe = new Execute();
            ACConfig AC = new ACConfig();
            Output obj = new Output();

            var paths = getDefaultPaths(Path, configPath, templatePath);
            var configpath = System.IO.Path.GetFullPath(System.IO.Path.Combine(paths.configPath, "Config.json"));
            var config = File.ReadAllText(configpath); //get json
            var configuration = JsonConvert.DeserializeObject<ACConfig>(config); //get path to json
            var outputPath = fileproof(configuration.outputDirectory);
            var changePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(fileproof(outputPath), "change.json"));
            var myconfig = JsonConvert.DeserializeObject<ACConfig>(File.ReadAllText(changePath));//open json to use
            var dirs = exe.findFilesInDirectory(outputPath); //search all files in Directory 
            foreach (var file in dirs)
            {
                AC = new InputToACObject().parseinobject(new StreamReader(file)); //parses current configuration into the AC object
                if (myconfig.configureNetwork != null)
                {
                    AC = exe.replaceitem(AC, myconfig.configureNetwork.networkdev, "networkdev");
                    AC = exe.replaceitem(AC, myconfig.configureNetwork.interfacenetworkif, "interfacenetworkif");
                }
                if (myconfig.configureviop != null)
                {
                    AC = exe.replaceitem(AC, myconfig.configureviop.proxyset, "proxyset");
                    AC = exe.replaceitem(AC, myconfig.configureviop.proxyip, "proxyip"); //replaces the wanted details
                }
                var objList = obj.objectToList(AC);
                obj.writeOutput(objList, file); //output
            }
        }
        public string fileproof(string outputDirectory)
        {
            var exit = Directory.Exists(outputDirectory);
            if (!exit)
            {
                exit = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), outputDirectory));
            }
            if (exit == false) 
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), outputDirectory);
                Directory.CreateDirectory(path);
                File.Create(path + "\\change.json");
                return path;
            }
            if (Directory.Exists(outputDirectory))
            {
                return outputDirectory;
            }
            else
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), outputDirectory);
            }
        }
        protected List<string> findFilesInDirectory(string mypath) // opens the .txt files in the directorypath
        {
            string[] dirs = Directory.GetFiles(mypath, "*.txt", SearchOption.TopDirectoryOnly);//only the top not sup directorys
            return dirs.ToList<string>();
        }
        protected void change(dynamic i, dynamic item) //replaces the Item
        {
            foreach (var propertyInfo in item.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(item);
                if (value != null)
                {
                    i.GetType().GetProperty(propertyInfo.Name).SetValue(i, value);
                }
            }
        }
        public ACConfig replaceitem(ACConfig AC, dynamic list, string whatlist)
        {
            if (list == null)
            {
                return AC;
            }
            foreach ( var config in list)
            {
                switch (whatlist)   // switches on which list is now given 
                {
                    case "networkdev":
                        foreach (var i in AC.configureNetwork.networkdev)
                        {
                            if (config.listid == i.listid)
                            {
                                change(i, config);
                            }
                        }
                        break;
                    case "interfacenetworkif":
                        foreach (var i in AC.configureNetwork.interfacenetworkif)
                        {
                            if (config.listid == i.listid)
                            {
                                change(i, config);
                            }
                        }
                        break;
                    case "proxyset":
                        foreach (var i in AC.configureviop.proxyset)
                        {
                            if (config.listid == i.listid)
                            {
                                change(i, config);
                            }
                        }
                        break;
                    case "proxyip":
                        foreach (var i in AC.configureviop.proxyip)
                        {
                            if (config.ip == i.ip)
                            {
                                change(i, config);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return AC;
        }

        public void RunCreate(
            CommandOption Path,
            CommandOption configPath,
            CommandOption templatePath,
            CommandOption Net,
            CommandOption Dev,
            CommandOption Set,
            CommandOption Ip) // second command -> creates an empty configuration with x list of the diffrent blocks
        {
            var paths = getDefaultPaths(Path, configPath, templatePath);
            fileproof(paths.configPath);
            var time = DateTime.Now;
            var filepath = paths.path + @"\" + time.Year.ToString() + "." + time.Month.ToString() + "." + time.Day.ToString() + "-" + time.Hour.ToString() + "." + time.Minute.ToString() + ".txt"; //creats a time
            Write(Net, Dev, Set, Ip, filepath, paths.configPath, paths.tempaltePath);
        }
        protected void Write(CommandOption Net, CommandOption Dev, CommandOption Set, CommandOption Ip, string mypath, string configPath, string tempaltePath)
        {
            var netcounter = 1;
            var devcounter = 1;
            var setcounter = 1;
            var ipcounter = 1;
            if (Net.HasValue() == true && Net.Value() != null)
            {
                int.TryParse(Net.Value(), out netcounter);
            }
            if (Ip.HasValue() == true && Ip.Value() != null)
            {
                int.TryParse(Ip.Value(), out ipcounter);
            }
            if (Set.HasValue() == true && Set.Value() != null)
            {
                int.TryParse(Set.Value(), out setcounter);
            }
            if (Dev.HasValue() == true && Dev.Value() != null)
            {
                int.TryParse(Dev.Value(), out devcounter);
            }

            var Networkdevvorlage = File.ReadAllText(Path.Combine(tempaltePath, @"NetworkDev.template"));
            var Interfacenetworkifvorlage = File.ReadAllText(Path.Combine(tempaltePath, @"InterfaceNetwokIf.template"));
            var Proxysetvorlage = File.ReadAllText(Path.Combine(tempaltePath, @"ProxySet.template"));
            var Proxyipvorlage = File.ReadAllText(Path.Combine(tempaltePath, @"ProxyIp.template"));
            using (StreamWriter writer = new StreamWriter(mypath))
            {
                writer.WriteLine("configure network");
                for (int i = 0; i < netcounter; i++)
                {
                    writer.WriteLine(Networkdevvorlage);
                }
                writer.WriteLine(@" exit");
                for (int i = 0; i < devcounter; i++)
                {
                    writer.WriteLine(Interfacenetworkifvorlage);
                }
                writer.WriteLine(@" exit");
                writer.WriteLine("exit");
                writer.WriteLine("configure voip");
                for (int i = 0; i < setcounter; i++)
                {
                    writer.WriteLine(Proxysetvorlage);
                }
                writer.WriteLine(@" exit");
                for (int i = 0; i < ipcounter; i++)
                {
                    writer.WriteLine(Proxyipvorlage);
                }
                writer.WriteLine(@" exit");
                writer.WriteLine("exit");
            }

        }
        private string GetToolPath()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path).ToString();
            return path;
        }
        private (string path, string configPath, string tempaltePath) getDefaultPaths(
            CommandOption Path,
            CommandOption configpath,
            CommandOption templatePath)
        {

            var toolPath = GetToolPath();
            var path = Path.HasValue() ? Path.Value() : Directory.GetCurrentDirectory();
            var configPath = configpath.HasValue() ? configpath.Value() : System.IO.Path.Combine(toolPath, "config");
            var tempaltePath = templatePath.HasValue() ? templatePath.Value() : System.IO.Path.Combine(toolPath, "onfig", "Template");
            return (
               path,
               configPath,
               tempaltePath
                );
        }
    }
}

