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
            app.ShowHelp();
            app.Command("replace", u => //should search and replace configs that allready exist
            {
                u.HelpOption(helptemplate);
                u.Description = "Dieser Befehl soll es ermöglichen die hinterlegte Konfiguration zu editieren.";
                var path = u.Option("--path <fullpath>", "Setzt einen dauerhaften benutzerdefinierten Pfad. Wenn dieser Befehl nicht benutzt wird, wird der Pfad , welcher in der Config.json als OutputDirectory angegeben ist verwendet.", CommandOptionType.SingleValue);
                var configPath = u.Option("--config <fullpath>", "Benutzt ein benutzerdefinierte Konfiguration. Wenn dieser Befehl nicht benutzt wird die Standardkonfiguration verwendet.", CommandOptionType.SingleValue);
                var templatePath = u.Option("--template <fullpath>", "Benutzt einen benutzerdefiniertes Templateverzeichnis. Wenn dieser Befehl nicht benutzt wird, werden die Standardtemplates verwendet.", CommandOptionType.SingleValue);
                var customer = u.Option("--customer <customer>", "Setzt das Kundenkürzel. Wenn nicht benutzt, wird das vorhandene Kürzel benutzt.", CommandOptionType.SingleValue);
                var country = u.Option("--country <contry>", "Setzt das länderkürzel. Wenn nicht benutzt, wird das vorhandene Kürzel benutzt.", CommandOptionType.SingleValue);
                u.OnExecute(() => { obj.runReplace(path, configPath, templatePath, customer, country); });
            });
            app.Command("create", c => //creats a new config 
            {
                c.HelpOption(helptemplate);
                c.Description = "Erstellt eine neue Configvorlage.";
                var path = c.Option("--path <fullpath>", "Benutzt einen benutzerdefinierten Pfad. Wenn dieser Befehl nicht benutzt wird, wird der Pfad , welcher in der Config.json als OutputDirectory angegeben ist verwendet.", CommandOptionType.SingleValue);
                var configPath = c.Option("--config <fullpath>", "Benutzt ein benutzerdefinierte Konfiguration. Wenn dieser Befehl nicht benutzt wird die Standardkonfiguration verwendet.", CommandOptionType.SingleValue);
                var templatePath = c.Option("--template <fullpath>", "Benutzt einen benutzerdefiniertes Templateverzeichnis. Wenn dieser Befehl nicht benutzt wird, werden die Standardtemplates verwendet.", CommandOptionType.SingleValue);
                var Net = c.Option("--networkdev <anzahl>", "Setzt die Anzahl für Networkdevabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Int = c.Option("--interfacenetworkif <anzahl>", "Setzt die Anzahl für Interfacenetworkifabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Set = c.Option("--proxyset <anzahl>", "Setzt die Anzahl für Proxysetabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Ip = c.Option("--proxyip <anzahl>", "Setzt die Anzahl für Proxyipabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                c.OnExecute(() => { obj.RunCreate(path, configPath, templatePath, Net, Int, Set, Ip); });
            });
            return app.Execute(commands);

        }
    }
    public class Execute
    {
        public void runReplace(
            CommandOption Path,
            CommandOption configPath,
            CommandOption templatePath,
            CommandOption customer,
            CommandOption country) //run for replace
        {
            ACConfig AC = new ACConfig();
            Output obj = new Output();
            Execute exe = new Execute();
            ACConfig ConfigWithChanges = new ACConfig();
            List<string> dirs = new List<string>();
            new Arrangement().prepareForStart(Path, configPath, templatePath, out ConfigWithChanges, out dirs);
            //search all files in Directory 
            foreach (var file in dirs)
            {
                AC = new InputToACObject().parseinobject(new StreamReader(file)); //parses current configuration into the AC object
                if (ConfigWithChanges.configureNetwork != null)
                {
                    AC = exe.replaceitem(AC, ConfigWithChanges.configureNetwork.networkdev, "networkdev", country, customer);
                    AC = exe.replaceitem(AC, ConfigWithChanges.configureNetwork.interfacenetworkif, "interfacenetworkif", country, customer);
                }
                if (ConfigWithChanges.configureviop != null)
                {
                    AC = exe.replaceitem(AC, ConfigWithChanges.configureviop.proxyset, "proxyset", country, customer);
                    AC = exe.replaceitem(AC, ConfigWithChanges.configureviop.proxyip, "proxyip", country, customer); //replaces the wanted details
                }
                var objList = obj.objectToList(AC);
                obj.writeOutput(objList, file); //output
            }
        }
        protected void change(dynamic configItem, dynamic item, CommandOption country, CommandOption customer) //replaces the Item
        {
            foreach (var propertyInfo in item.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(item);
                if (value == null)
                {
                    if (searchAcronym(propertyInfo.Name))
                    {
                        swapAcronym(configItem, item, country, customer, propertyInfo);
                    }
                    continue;
                }else
                { 
                if (searchAcronym(propertyInfo.Name))
                {
                    swapAcronym(configItem, item, country, customer, propertyInfo);
                }
                else
                {
                        configItem.GetType().GetProperty(propertyInfo.Name).SetValue(configItem, value);
                }
                }
            }
        }
        protected void swapAcronym(dynamic oldconfig, dynamic item, CommandOption country, CommandOption customer, PropertyInfo propertyInfo)
        {
            var value = propertyInfo.GetValue(item);
            var configValue = oldconfig.GetType().GetProperty(propertyInfo.Name).GetValue(oldconfig);
            if (configValue == null)
            {
                return;
            }
            else if (country.HasValue() && customer.HasValue())
            {
                if (value == null)
                {
                    value = configValue.Substring(0, configValue.IndexOf('_') + 1) + customer.Value() + "-" + country.Value();
                }
                value = value.Substring(0, configValue.IndexOf('_') + 1) + customer.Value() + "-" + country.Value();
                oldconfig.GetType().GetProperty(propertyInfo.Name).SetValue(oldconfig, value);
            }
            else if (country.HasValue() && !customer.HasValue())
            {
                if (value == null)
                {
                    value = configValue.Substring(0, configValue.IndexOf('-') + 1)  + country.Value();
                }
                value = value.Substring(0, value.IndexOf('_') + 1)+ configValue.Substring(configValue.IndexOf('_')+1, configValue.IndexOf('-') - configValue.IndexOf('_')) + country.Value();
                oldconfig.GetType().GetProperty(propertyInfo.Name).SetValue(oldconfig, value);
            }
            else if (!country.HasValue() && customer.HasValue())
            {
                if (value == null)
                {
                    value = configValue.Substring(0, configValue.IndexOf('_') + 1) + customer.Value() + configValue.Substring(configValue.IndexOf('-'));
                }
                value = value.Substring(0, configValue.IndexOf('_') + 1) + customer.Value() + configValue.Substring(configValue.IndexOf('-'));
                oldconfig.GetType().GetProperty(propertyInfo.Name).SetValue(oldconfig, value);
            }
            if (!country.HasValue() && !customer.HasValue())
            {

                if (value == null)
                {
                    value = configValue;
                }
                value = value.Substring(0, configValue.IndexOf('_')) + configValue.Substring(configValue.IndexOf('_'));
                oldconfig.GetType().GetProperty(propertyInfo.Name).SetValue(oldconfig, value);
            }
        }
        protected bool searchAcronym(string Name)
        {
            switch (Name)
            {
                case ParserVariables.name:
                    return true;
                case ParserVariables.sbcipv:
                    return true;
                case ParserVariables.udev:
                    return true;
                case ParserVariables.pname:
                    return true;
                case ParserVariables.sname:
                    return true;
                default:
                    return false;
            }
        }
        public ACConfig replaceitem(ACConfig AC, dynamic list, string whatlist, CommandOption country, CommandOption customer)
        {
            if (list == null)
            {
                return AC;
            }
            foreach (var config in list)
            {
                switch (whatlist)   // switches on which list is now given 
                {
                    case "networkdev":
                        foreach (var configItem in AC.configureNetwork.networkdev)
                        {
                            if (config.listid != configItem.listid)
                            {
                                continue;
                            }
                            change(configItem, config, country, customer);
                        }
                        break;
                    case "interfacenetworkif":
                        foreach (var configItem in AC.configureNetwork.interfacenetworkif)
                        {
                            if (config.listid != configItem.listid)
                            {
                                continue;
                            }
                            change(configItem, config, country, customer);
                        }
                        break;
                    case "proxyset":
                        foreach (var configItem in AC.configureviop.proxyset)
                        {
                            if (config.listid != configItem.listid)
                            {
                                continue;
                            }
                            change(configItem, config, country, customer);
                        }
                        break;
                    case "proxyip":
                        foreach (var configItem in AC.configureviop.proxyip)
                        {
                            if (config.ip != configItem.ip)
                            {
                                continue;
                            }
                            change(configItem, config, country, customer);
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
            CommandOption Int,
            CommandOption Set,
            CommandOption Ip) // second command -> creates an empty configuration with x list of the diffrent blocks
        {
            Arrangement arrangement = new Arrangement();
            var paths = arrangement.getDefaultPaths(Path, configPath, templatePath);
            var configuration = arrangement.LoadSystemConfig(System.IO.Path.GetFullPath(System.IO.Path.Combine(paths.configPath, "Config.json")));
            var outputPath = arrangement.fileproof(paths.path, configuration.outputDirectory);
            var time = DateTime.Now;
            var filepath = outputPath + @"\" + time.ToString("yyyy.mm.dd.hh.mm") + ".txt"; //creats a time
            new Output().Write(Net, Int, Set, Ip, filepath, paths.tempaltePath);
        }
    }
}

