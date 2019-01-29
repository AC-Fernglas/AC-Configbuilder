using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Sprache;
using Newtonsoft.Json;
using System.Linq;

namespace ACConfigBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Commands obj = new Commands();
            obj.Idel(args);
        }
    }
    class Commands
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
                var path = u.Option("--path <fullpath>", "Setzt einen benutzerdefinierten Pfad, wenn dieser Befehl nicht benutzt wird, wird der Sampelsordner benutzt.", CommandOptionType.SingleValue);
                u.OnExecute(() => { obj.run(path); });
            });
            app.Command("create", c => //creats a new config 
            {
                c.HelpOption(helptemplate);
                c.Description = "Erstellt eine neue Configvorlage.";
                var path = c.Option("--path <fullpath>", "Setzt einen benutzerdefinierten Pfad, wenn dieser Befehl nicht benutzt wird, wird der Sampelsordner benutzt.", CommandOptionType.SingleValue);

                var Net = c.Option("--networkdev <anzahl>", "Setzt die Anzahl für Networkdevabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Int = c.Option("--interfacenetworkif <anzahl>", "Setzt die Anzahl für Interfacenetworkifabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Set = c.Option("--proxyset <anzahl>", "Setzt die Anzahl für Proxysetabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Ip = c.Option("--proxyip <anzahl>", "Setzt die Anzahl für Proxyipabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                c.OnExecute(() => { obj.RunCreate(path, Net, Int, Set, Ip); });
            });
            app.Command(null, c =>
            {
                app.ShowHelp();
            });
            return app.Execute(commands);

        }
    }

    class Execute
    {
        private void setuserpath(string configPath, string changePath)
        {
            StringBuilder newFile = new StringBuilder();
            string[] file = File.ReadAllLines(configPath + @"\Config.json");
            List<string> list = new List<string>(file);
            list[3] = "\"changeDirectory\": " +  changePath;
            foreach (string line in list)
            {
                newFile.Append(line + "\n");
            }
            File.WriteAllText(configPath + @"\Config.json", newFile.ToString());
}
        public void run(CommandOption path) //run for replace
        {
            Execute exe = new Execute();
            ACConfig AC = new ACConfig();
            Output obj = new Output();
            var currentDirectory = Directory.GetCurrentDirectory();
            if (currentDirectory == validpath(path,@"..\netcoreapp2.2"))
            {
                Directory.SetCurrentDirectory(@"..\..\..\");
            }
            var configPath = validpath(path, EnviromentVariable.configDirectory);
            var config = File.ReadAllText(configPath + @"\Config.json"); //get json
            var host = JsonConvert.DeserializeObject<ACConfig>(config); //get path to json
            var myconfig = JsonConvert.DeserializeObject<ACConfig>(File.ReadAllText(host.userpath));//open json to use
            var changePath = host.changeDirectory;
            var mypath = String.Empty;
            if (path.HasValue() && path.Value() != " " && path.Value() != null)
            {
               mypath = validpath(path, null);
               setuserpath(configPath,mypath);
            }
            else
            {
                mypath = validpath(path, changePath.ToString());
            }
            List<string> dirs = new List<string>();
            dirs = exe.findDirectorys(mypath); //search all files in Directory 
            foreach (var item in dirs)
            {
                AC = exe.parseinobject(item); //parses current configuration into the AC object
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
                obj.getobject(AC, item); //output
            }

        }
        private List<string> findDirectorys(string mypath) // opens the .txt files in the directorypath
        {
            string[] dirs = Directory.GetFiles(mypath, "*.txt", SearchOption.TopDirectoryOnly);//only the top not sup directorys
            return dirs.ToList<string>();
        }
        private void getIdentNameAndValue(string line, out bool configureExit, out bool subIdentExit, out string subIdent, out string subIdentValue) // parses the Indentifyer for the current block of the configuration
        {
            subIdent = ParserGrammar.getsubident.Parse(line);
            configureExit = false;
            subIdentExit = false;
            subIdentValue = String.Empty;
            if (subIdent == ParserVariables.networkDev ||
                subIdent == ParserVariables.interfaceNetwokIf ||
                subIdent == ParserVariables.proxySet ||
                subIdent == ParserVariables.proxyIp ||
                subIdent == ParserVariables.exit)
            {
                if (subIdent == ParserVariables.exit)
                {
                    configureExit = true;
                    subIdentExit = true;
                    return;
                }
                subIdentExit = false;
                subIdentValue = ParserGrammar.subidentvalue.Parse(line);
            }
        }
        private void getConfigureIdent(string line, out bool configureExit, out string ident) //parses the head of the currend configurationblock
        {
            ident = String.Empty;
            configureExit = true;
            if (ParserGrammar.getidentifier.Parse(line) == "configure network" || ParserGrammar.getidentifier.Parse(line) == "configure voip" || ParserGrammar.getidentifier.Parse(line) == Environment.NewLine || ParserGrammar.getidentifier.Parse(line) == "\n")
            {
                ident = ParserGrammar.getidentifier.Parse(line);
                configureExit = false;
            }
        }
        private ACConfig parseinobject(string path)
        {
            Configureviop vo = new Configureviop();
            ConfigureNetwork co = new ConfigureNetwork();
            List<Networkdev> networkDevs = new List<Networkdev>();
            List<Interfacenetworkif> interfaceNetworkIfs = new List<Interfacenetworkif>();
            List<Proxyip> proxyIps = new List<Proxyip>();
            List<Proxyset> proxySets = new List<Proxyset>();
            ACConfig AC = new ACConfig();

            Networkdev newListNetworkDev = new Networkdev();
            Interfacenetworkif newListInterfaceNetworkIf = new Interfacenetworkif();
            Proxyip newListProxyIp = new Proxyip();
            Proxyset newListProxySet = new Proxyset();

            AC.configureNetwork = co;
            AC.configureviop = vo;

            AC.configureNetwork.networkdev = networkDevs;
            AC.configureNetwork.interfacenetworkif = interfaceNetworkIfs;

            AC.configureviop.proxyip = proxyIps;
            AC.configureviop.proxyset = proxySets;

            string ident = " ";
            bool configureExit = true;
            string subIdent = " ";
            bool subIdentExit = true;
            string subIdentValue = "";
            int networkDevListTndex = 0;
            int interfaceNetwokIfListTndex = 0;
            int proxySetListTndex = 0;
            int proxyIpListTndex = 0;

            using (StreamReader Reader = new StreamReader(path))
            {
                string line = " ";
                while ((line = Reader.ReadLine()) != null)                                                  // zu editierende File einlesen (Zeile für Zeile)
                {
                    if (line == string.Empty || line == "")                                                 // wenn leere Zeile überspringe
                    {
                        continue;
                    }
                    if (configureExit)                                                                      // wenn True -> kein Überbereich(configure voip // configure network) in dem die Konfig definiert wird
                    {
                        getConfigureIdent(line, out configureExit, out ident);
                        continue;
                    }
                    if (subIdentExit)
                    {
                        getIdentNameAndValue(line, out configureExit, out subIdentExit, out subIdent, out subIdentValue);  // wenn true -> keien Bereich(networkDev / interfaceNetworkIf) in dem Konfig konfiguriert wird
                        var Name = returnRealName(subIdent);
                        if (Name != null && subIdent == ParserVariables.networkDev)
                        {
                            networkDevs.Add(newListNetworkDev);                                            //neue Liste wird erstellt für jede Bereich von networkDev
                            AC = ListParsing(AC, Name, subIdentValue, networkDevListTndex, AC.configureNetwork.networkdev, subIdent);
                        }
                        else if (Name != null && subIdent == ParserVariables.interfaceNetwokIf)
                        {
                            interfaceNetworkIfs.Add(newListInterfaceNetworkIf);
                            AC = ListParsing(AC, Name, subIdentValue, interfaceNetwokIfListTndex, AC.configureNetwork.interfacenetworkif, subIdent);
                        }
                        else if (Name != null && subIdent == ParserVariables.proxySet)
                        {
                            proxySets.Add(newListProxySet);
                            AC = ListParsing(AC, Name, subIdentValue, proxySetListTndex, AC.configureviop.proxyset, subIdent);
                        }
                        else if (Name != null && subIdent == ParserVariables.proxyIp)
                        {
                            proxyIps.Add(newListProxyIp);
                            AC = ListParsing(AC, Name, subIdentValue, proxyIpListTndex, AC.configureviop.proxyip, subIdent);
                        }
                        continue;
                    }

                    if (configureExit == false && subIdentExit == false && ident == "configure network" && subIdent == ParserVariables.networkDev)
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        if (Name == ParserVariables.exit) // Bereich wird durch exit beendet -> Listenindex wird erhöht und es gibt keinen Bereich in dem definiert die Konfig defoiniert wird
                        {
                            subIdentExit = true;
                            networkDevListTndex++;
                            continue;
                        }
                        Name = returnRealName(Name);
                        if (Name == null)
                        {
                            continue;
                        }
                        if (Name == ParserVariables.activate) //boolscher wert der in der KOnfiguration keinen weiteren Wert zugewieden bekommt -> entweder da oder nicht
                        {
                            var Value = true;
                            AC = ListParsing(AC, Name, Value, networkDevListTndex, AC.configureNetwork.networkdev, subIdent);
                        }
                        else
                        {
                            var Value = ParserGrammar.ValueParser.Parse(line);
                            AC = ListParsing(AC, Name, Value, networkDevListTndex, AC.configureNetwork.networkdev, subIdent);
                        }


                        continue;
                    }
                    else if (configureExit == false && subIdentExit == false && ident == "configure network" && subIdent == ParserVariables.interfaceNetwokIf)
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        if (Name == ParserVariables.exit)
                        {
                            subIdentExit = true;
                            interfaceNetwokIfListTndex++;
                            continue;
                        }
                        Name = returnRealName(Name);
                        if (Name == null)
                        {
                            continue;
                        }
                        if (Name == ParserVariables.activate)
                        {
                            var Value = true;
                            AC = ListParsing(AC, Name, Value, interfaceNetwokIfListTndex, AC.configureNetwork.interfacenetworkif, subIdent);
                        }
                        else
                        {
                            var Value = ParserGrammar.ValueParser.Parse(line);
                            AC = ListParsing(AC, Name, Value, interfaceNetwokIfListTndex, AC.configureNetwork.interfacenetworkif, subIdent);
                        }
                        continue;
                    }
                    if (configureExit == false && subIdentExit == false && ident == "configure voip" && subIdent == ParserVariables.proxySet)
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        if (Name == ParserVariables.exit)
                        {
                            subIdentExit = true;
                            proxySetListTndex++;
                            continue;
                        }
                        Name = returnRealName(Name);
                        if (Name == null)
                        {
                            continue;
                        }
                        if (Name == ParserVariables.activate)
                        {
                            var Value = true;
                            AC = ListParsing(AC, Name, Value, proxySetListTndex, AC.configureviop.proxyset, subIdent);
                        }
                        else
                        {
                            var Value = ParserGrammar.ValueParser.Parse(line);
                            AC = ListParsing(AC, Name, Value, proxySetListTndex, AC.configureviop.proxyset, subIdent);
                        }
                        continue;
                    }
                    else if (configureExit == false && subIdentExit == false && ident == "configure voip" && subIdent == ParserVariables.proxySet)
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        if (Name == ParserVariables.exit)
                        {
                            subIdentExit = true;
                            proxyIpListTndex++;
                            continue;
                        }
                        Name = returnRealName(Name);
                        if (Name == null)
                        {
                            continue;
                        }
                        if (Name == ParserVariables.activate)
                        {
                            var Value = true;
                            AC = ListParsing(AC, Name, Value, proxyIpListTndex, AC.configureviop.proxyip, subIdent);
                        }
                        else
                        {
                            var Value = ParserGrammar.ValueParser.Parse(line);
                            AC = ListParsing(AC, Name, Value, proxyIpListTndex, AC.configureviop.proxyip, subIdent);
                        }
                        continue;
                    }
                }
            }
            return AC;
        }
        private string returnRealName(string Name) // wandelt die in der Konfiguration enthaltenen bezeichner in die Variablen Namen der Listen um da diese nicht zu 100% übereinstimmen 
        {
            switch (Name)
            {
                case ParserVariables.proxySet:
                case ParserVariables.networkDev:
                case ParserVariables.interfaceNetwokIf:
                    return ParserVariables.listid;
                case ParserVariables.proxyIp:
                    return ParserVariables.ip;
                case ParserVariables.activate:
                    return ParserVariables.activate;
                case ParserVariables.vlanid:
                    return ParserVariables.vlan;
                case ParserVariables.underlyingdev:
                    return ParserVariables.udev;
                case ParserVariables.Name:
                    return ParserVariables.name;
                case ParserVariables.tagging:
                    return ParserVariables.Tag;
                case ParserVariables.apptype:
                    return ParserVariables.appt;
                case ParserVariables.ipaddress:
                    return ParserVariables.ipa;
                case ParserVariables.prefixlength:
                    return ParserVariables.prefix;
                case ParserVariables.gateway:
                    return ParserVariables.gate;
                case ParserVariables.underlyingif:
                    return ParserVariables.uif;
                case ParserVariables.proxyname:
                    return ParserVariables.pname;
                case ParserVariables.proxyenablekeepalive:
                    return ParserVariables.proxyalive;
                case ParserVariables.srdname:
                    return ParserVariables.sname;
                case ParserVariables.sbcipv4sipintname:
                    return ParserVariables.sbcipv;
                case ParserVariables.keepalivefailresp:
                    return ParserVariables.keepresp;
                case ParserVariables.successdetectretries:
                    return ParserVariables.successdet;
                case ParserVariables.successdetectint:
                    return ParserVariables.successdetectint;
                case ParserVariables.proxyredundancymode:
                    return ParserVariables.proxymode;
                case ParserVariables.isproxyhotswap:
                    return ParserVariables.proxyswap;
                case ParserVariables.proxyloadbalancingmethod:
                    return ParserVariables.proxymode;
                case ParserVariables.minactiveservlb:
                    return ParserVariables.minlb;
                case ParserVariables.proxyaddress:
                    return ParserVariables.padress;
                case ParserVariables.transporttype:
                    return ParserVariables.ttype;
                default:
                    return null;
            }
        }
        private ACConfig ListParsing(ACConfig Config, string Name, dynamic Value, int Index, dynamic myList, string subIdent) //setzt die Value in aus dfem Bereich in die Passende liste an der Richtigen stelle
        {
            ;
            if (Value == null) // falls es keine Value gibt brauch es nix machen
            {
                return Config;
            }
            foreach (var item in myList[Index].GetType().GetProperties()) //property aus index X   bekommeb
            {
                var property = item;
                if (property.Name != Name) // wenn die property nicht den selben bezeichner hat wie der gegebene Name
                {
                    continue;
                }
                var propertyType = property.PropertyType; // Gets the type of the property

                if (propertyType == typeof(Int32) || propertyType == typeof(Nullable<Int32>)) //if type = int then parse Value into an integer
                {
                    property.SetValue(myList[Index], Convert.ToInt32(Value));
                }
                else if (propertyType.IsEnum) // if type = enum then parse Value into a enum
                {
                    var enumMember = propertyType
                        .GetFields(); // List all fields of an enum
                    foreach (var member in enumMember)
                    {
                        if (member.Name == Convert.ToString(Value) ||
                             Convert.ToString(Value) == Convert.ToString(member.GetCustomAttributes(typeof(NameAttribute), false))) // if the name of the field = Name or the Attribute = Name 
                        {
                            if (member == null)
                            {
                                Console.WriteLine($"Warn: skip property {property.Name} because the value {Value} is not valid for this field.");
                                return Config;
                            }
                            var enumValue = Enum.Parse(propertyType, member.Name);  // parses Value into an enumValue
                            property.SetValue(myList[Index], enumValue);

                        }  //Console.WriteLine($"Warn: skip property {property.Name} because enums currently not supported.");
                    }
                }
                else if (propertyType == typeof(Boolean)) // if type = boolean then parse Value into a boolean
                {
                    property.SetValue(myList[Index], Convert.ToBoolean(Value));
                }
                else                            //if type is something else (hopefully a string) then replace the old value with the new
                {
                    property.SetValue(myList[Index], Value);
                }
                switch (subIdent) // returns the right Config 
                {
                    case ParserVariables.networkDev:
                        Config.configureNetwork.networkdev = myList;
                        return Config;
                    case ParserVariables.interfaceNetwokIf:
                        Config.configureNetwork.interfacenetworkif.Add(myList);
                        return Config;
                    case ParserVariables.proxySet:
                        Config.configureviop.proxyset.Add(myList);
                        return Config;
                    case ParserVariables.proxyIp:
                        Config.configureviop.proxyip.Add(myList);
                        return Config;
                    default:
                        break;
                }
                return Config;
            }
            return Config;
        }
        private string validpath(CommandOption filepath, string otherPath) //valify the userpath
        {
            var path = String.Empty;
            if (otherPath != null)
            {
                path = otherPath;
            }
            else
            {
                path = filepath.Value().ToString();
            }
            path = path.Replace(@"\\", ":"); // to cancel out c:\\\\test.text
            string temp = Path.GetPathRoot(path); //For cases like: \text.txt
            if (temp.StartsWith(@"\")) return null;
            string pt = Path.GetFullPath(path);
            return pt;
            
        }
        private void change(dynamic i, dynamic item) //replaces the Item
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
            foreach (var item in list)
            {
                switch (whatlist)   // switches on which list is now given 
                {
                    case "networkdev":
                        foreach (var i in AC.configureNetwork.networkdev)
                        {
                            if (item.listid == i.listid)
                            {
                                change(i, item);
                            }
                        }
                        break;
                    case "networkinterfaceif":
                        foreach (var i in AC.configureNetwork.interfacenetworkif)
                        {
                            if (item.listid == i.listid)
                            {
                                change(i, item);
                            }
                        }
                        break;
                    case "proxyset":
                        foreach (var i in AC.configureviop.proxyset)
                        {
                            if (item.listid == i.listid)
                            {
                                change(i, item);
                            }
                        }
                        break;
                    case "proxyip":
                        foreach (var i in AC.configureviop.proxyip)
                        {
                            if (item.ip == i.ip)
                            {
                                change(i, item);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return AC;
        }

        public void RunCreate(CommandOption path, CommandOption Net, CommandOption Dev, CommandOption Set, CommandOption Ip) // second command -> creates an empty configuration with x list of the diffrent blocks
        {
            var mypath = validpath(path, null);
            DateTime time = new DateTime();
            time = DateTime.Now;
            var filepath = mypath + @"\" + time.Year.ToString() + "." + time.Month.ToString() + "." + time.Day.ToString() + "-" + time.Hour.ToString() + "." + time.Minute.ToString() + ".txt"; //creats a time
            var configPath = validpath(path, EnviromentVariable.configDirectory);
            Write(Net, Dev, Set, Ip, filepath, configPath);
        }
        private void Write(CommandOption Net, CommandOption Dev, CommandOption Set, CommandOption Ip, string mypath,string configPath)
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
            
            var Networkdevvorlage = File.ReadAllText(configPath+@"\Networkdevvorlage.txt");
            var Interfacenetworkifvorlage = File.ReadAllText(configPath + @"\Interfacenetworkifvorlage.txt");
            var Proxysetvorlage = File.ReadAllText(configPath + @"\Proxysetvorlage.txt");
            var Proxyipvorlage = File.ReadAllText(configPath + @"\Proxyipvorlage.txt");
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
    }
}
