using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Sprache;
using Newtonsoft.Json;
using System.Linq;

namespace secondtry
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
                var path = u.Option(@"--path <fullpath>", "Setzt einen benutzerdefinierten Pfad, wenn dieser Befehl nicht benutzt wird, wird der Sampelsordner benutzt.", CommandOptionType.SingleValue);
                u.OnExecute(() => { obj.run(path); });
            });
            app.Command("create", c => //creats a new config 
            {
                c.HelpOption(helptemplate);
                c.Description = "Erstellt eine neue Configvorlage.";
                var path = c.Option(@"--path <fullpath>", "Setzt einen benutzerdefinierten Pfad, wenn dieser Befehl nicht benutzt wird, wird der Sampelsordner benutzt.", CommandOptionType.SingleValue);

                var Net =  c.Option(@"--networkdev <anzahl>", "Setzt die Anzahl für Networkdevabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Int = c.Option(@"--interfacenetworkif <anzahl>", "Setzt die Anzahl für Interfacenetworkifabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Set = c.Option(@"--proxyset <anzahl>", "Setzt die Anzahl für Proxysetabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Ip = c.Option(@"--proxyip <anzahl>", "Setzt die Anzahl für Proxyipabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                c.OnExecute(() => { obj.RunCreate(path, Net,Int,Set,Ip); });
            });
            return app.Execute(commands);

        }
    }

    class Execute {
        private void setuserpath(string mypath)
        {
            string[] userconfig = File.ReadAllLines(@"..\\..\\..\\..\\..\\config\\qwertz.json");
            StringBuilder newFile = new StringBuilder();
            string[] file = File.ReadAllLines($@"..\\..\\..\\..\\..\\config\\qwertz.json");
            List<string> list = new List<string>(file);
            list[2] = "\"userpath\": " + "\"" + mypath + "\"";
            foreach (string line in list)
            {
                newFile.Append(line + "\n");
            }
            File.WriteAllText($@"..\\..\\..\\..\\config\\qwertz.json", newFile.ToString());
        }
        public void run(CommandOption path) //run for replace
        {
            Execute exe = new Execute();
            ACConfig AC = new ACConfig();
            Output obj = new Output();
            string mypath = $@"..\\..\\..\\..\\samples";
            if (exe.validpath(path) != null & path.Value() != " ") //valify path if is on given
            {
                // mypath = exe.validpath(path); //sets user path for usage
                var newFile = new StringBuilder();
                var config = File.ReadAllText(@"..\..\..\..\config\qwertz.json"); //get json
                var host = JsonConvert.DeserializeObject<ACConfig>(config); //get path to json
                var myconfig = JsonConvert.DeserializeObject<ACConfig>(File.ReadAllText(host.userpath));//open json to use
                List<string> dirs = new List<string>();
                dirs.Add(exe.findDirectorys(mypath)); //search all files in Directory 
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
            }
        }
        private string findDirectorys(string mypath) // opens the .txt files in the directorypath
        {
            string[] dirs = Directory.GetFiles(mypath, "*.txt", SearchOption.TopDirectoryOnly);//only the top not sup directorys
            foreach (var item in dirs)
            {
                return item;
            }
            return null;
        }
        private void getIdentNameAndValue(string line, out bool configureExit, out bool subIdentExit, out string subIdent, out string subIdentValue) // parses the Indentifyer for the current block of the configuration
        {
            subIdent = ParserGrammar.getsubident.Parse(line);
            configureExit = false;
            subIdentExit = false;
            subIdentValue = String.Empty;
            if (subIdent == "network-dev" ||
                subIdent == "interface network-if" ||
                subIdent == "proxy-set" ||
                subIdent == "proxy-ip" ||
                subIdent == "exit")
            {
                if (subIdent == "exit")
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
            // here I create the whole instance of my object in which i wanna parse
            //
            //
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
                while ((line = Reader.ReadLine()) != null)
                {
                    if (line == string.Empty || line == "")
                    {
                        continue;
                    }
                    if (configureexit)
                    {
                        getConfigureIdent(line, out configureExit, out ident);
                        continue;
                    }
                    if (subIdentExit)
                    {
                        getIdentNameAndValue(line, out configureExit, out subIdentExit, out subIdent, out subIdentValue);
                        var Name = zurück(subIdent);
                        if (Name != null && subIdent == "network-dev")
                        {
                            networkDevs.Add(newListNetworkDev);
                            AC = ListParsing(AC, Name, subIdentValue, networkDevListTndex, AC.configureNetwork.networkdev, subIdent);
                        }
                        else if (Name != null && subIdent == "interface network-if")
                        {
                            interfaceNetworkIfs.Add(newListInterfaceNetworkIf);
                            AC = ListParsing(AC, Name, subIdentValue, interfaceNetwokIfListTndex, AC.configureNetwork.interfacenetworkif, subIdent);
                        }
                        else if (Name != null && subIdent == "proxy-set")
                        {
                            proxySets.Add(newListProxySet);
                            AC = ListParsing(AC, Name, subIdentValue, proxySetListTndex, AC.configureviop.proxyset, subIdent);
                        }
                        else if (Name != null && subIdent == "proxy-ip")
                        {
                            proxyIps.Add(newListProxyIp);
                            AC = ListParsing(AC, Name, subIdentValue, proxyIpListTndex, AC.configureviop.proxyip, subIdent);
                        }
                        continue;

                    }

                    if (configureExit == false && subIdentExit == false && ident == "configure network" && subIdent == "network-dev")
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        if (Name == ParserVariables.exit)
                        {
                            subIdentExit = true;
                            networkDevListTndex++;
                            continue;
                        }
                        Name = zurück(Name);
                        if (Name == null)
                        {
                            continue;
                        }
                        if (Name == "activate")
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
                    else if (configureExit == false && subIdentExit == false && ident == "configure network" && subIdent == "interface network-if")
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        if (Name == ParserVariables.exit)
                        {
                            subIdentExit = true;
                            interfaceNetwokIfListTndex++;
                            continue;
                        }
                        Name = zurück(Name);
                        if (Name == null)
                        {
                            continue;
                        }
                        if (Name == "activate")
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
                    if (configureexit == false && subidentexit == false && ident == "configure voip" && subident == "proxy-set")
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        if (Name == ParserVariables.exit)
                        {
                            subIdentExit = true;
                            proxySetListTndex++;
                            continue;
                        }
                        Name = zurück(Name);
                        if (Name == null)
                        {
                            continue;
                        }
                        if (Name == "activate")
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
                    else if (configureexit == false && subidentexit == false && ident == "configure voip" && subident == "proxy-ip")
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        if (Name == ParserVariables.exit)
                        {
                            subIdentExit = true;
                            proxyIpListTndex++;
                            continue;
                        }
                        Name = zurück(Name);
                        if (Name == null)
                        {
                            continue;
                        }
                        if (Name == "activate")
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
        private string zurück(string Name)
        {
            switch (Name)
            {
                case ParserVariables.prosetlistident:
                case ParserVariables.devlistident:
                case ParserVariables.interlistident:
                    return "listid";
                case ParserVariables.proiplistident:
                    return "ip";
                case ParserVariables.activate:
                    return "activate";
                case ParserVariables.vlan:
                    return "vlan";
                case ParserVariables.underlyingdev:
                    return "underlyingdev";
                case ParserVariables.Name:
                    return "Name";
                case ParserVariables.tag:
                    return "tag";
                case ParserVariables.apptype:
                    return "apptype";
                case ParserVariables.ipaddress:
                    return "ipaddress";
                case ParserVariables.prefixlength:
                    return "prefixlength";
                case ParserVariables.gateway:
                    return "gateway";
                case ParserVariables.underlyingif:
                    return "underlyingif";
                case ParserVariables.proxyname:
                    return "proxyname";
                case ParserVariables.proxyenablekeepalive:
                    return "proxyenablekeepalive";
                case ParserVariables.srdname:
                    return "srdname";
                case ParserVariables.sbcipv4sipintname:
                    return "sbcipv4sipintname";
                case ParserVariables.keepalivefailresp:
                    return "keepalivefailresp";
                case ParserVariables.successdetectretries:
                    return "successdetectretries";
                case ParserVariables.successdetectint:
                    return "successdetectint";
                case ParserVariables.proxyredundancymode:
                    return "proxyredundancymode";
                case ParserVariables.isproxyhotswap:
                    return "isproxyhotswap";
                case ParserVariables.proxyloadbalancingmethod:
                    return "proxyloadbalancingmethod";
                case ParserVariables.minactiveservlb:
                    return "minactiveservlb";
                case ParserVariables.proxyaddress:
                    return "proxyaddress";
                case ParserVariables.transporttype:
                    return "transporttype";
                default:
                    return null;
            }
        }
        private ACConfig ListParsing(ACConfig Config, string Name, dynamic Value, int Index, dynamic myList, string subIdent)
        {
            ;
            if (Value == null)
            {
                return Config;
            }
            foreach (var item in myList[Index].GetType().GetProperties())
            {
                var property = item;
                if (property.Name != Name)
                {
                    continue;
                }
                var propertyType = property.PropertyType;

                if (propertyType == typeof(Int32) || propertyType == typeof(Nullable<Int32>))
                {
                    property.SetValue(myList[Index], Convert.ToInt32(Value));
                }
                else if (propertyType.IsEnum)
                {
                    var enumMember = propertyType
                        .GetFields(); // List all fields of an enum
                    foreach (var member in enumMember)
                    {
                        if (member.Name == Convert.ToString(Value) ||
                             Convert.ToString(Value) == Convert.ToString(member.GetCustomAttributes(typeof(NameAttribute), false)))
                        {
                            if (member == null)
                            {
                                Console.WriteLine($"Warn: skip property {property.Name} because the value {Value} is not valid for this field.");
                                return Config;
                            }
                            var enumValue = Enum.Parse(propertyType, member.Name);
                            property.SetValue(myList[Index], enumValue);

                        }  //Console.WriteLine($"Warn: skip property {property.Name} because enums currently not supported.");
                    }
                }
                else if (propertyType == typeof(Boolean))
                {
                    property.SetValue(myList[Index], Convert.ToBoolean(Value));
                }
                else
                {
                    property.SetValue(myList[Index], Value);
                }
                switch (subIdent)
                {
                    case "network-dev":
                        Config.configureNetwork.networkdev = myList;
                        return Config;
                    case "interfacenetworkif":
                        Config.configureNetwork.interfacenetworkif.Add(myList);
                        return Config;
                    case "proxyset":
                        Config.configureviop.proxyset.Add(myList);
                        return Config;
                    case "proxyip":
                        Config.configureviop.proxyip.Add(myList);
                        return Config;
                    default:
                        break;
                }
                return Config;
            }
            return Config;
        }
        private string validpath(CommandOption filepath)
        {

            if (filepath.HasValue() && filepath.Value() != " " && filepath.Value() != null)
            {
                var path = filepath.Value().ToString();
                path = path.Replace(@"\\", ":"); // to cancel out c:\\\\test.text
                string temp = Path.GetPathRoot(path); //For cases like: \text.txt
                if (temp.StartsWith(@"\")) return null;
                string pt = Path.GetFullPath(path);
                return pt;
            }
            return null;
        }
        private void change(dynamic i, dynamic item)
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
                switch (whatlist)
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

        public void RunCreate (CommandOption path, CommandOption Net, CommandOption Dev, CommandOption Set, CommandOption Ip)
        {
            Execute exe = new Execute();
            ACConfig AC = new ACConfig();
            string mypath = $@"..\\..\\..\\..\\samples";
            if (exe.validpath(path) != " ")
            {
                mypath = exe.validpath(path);
            }
            DateTime time = new DateTime();
            time = DateTime.Now;
            var filepath = mypath + @"\\" + time.Year.ToString() + "." + time.Month.ToString() + "." + time.Day.ToString() + "-" + time.Hour.ToString() + "." + time.Minute.ToString() + ".txt";
            
            Write(Net, Dev, Set, Ip, filepath);
        }
        private void Write(CommandOption Net, CommandOption Dev, CommandOption Set, CommandOption Ip, string mypath)
        {
            var netcounter = 1;
            var devcounter = 1;
            var setcounter = 1;
            var ipcounter = 1;
            if (Net.HasValue() == true && Net.Value() != null )
            {
                int.TryParse(Net.Value(), out  netcounter);
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
            var Networkdevvorlage = File.ReadAllText($@"..\\..\\..\\..\\config\\Networkdevvorlage.txt");
            var Interfacenetworkifvorlage = File.ReadAllText($@"..\\..\\..\\..\\config\\Interfacenetworkifvorlage.txt");
            var Proxysetvorlage = File.ReadAllText($@"..\\..\\..\\..\\config\\Proxysetvorlage.txt");
            var Proxyipvorlage = File.ReadAllText($@"..\\..\\..\\..\\config\\Proxyipvorlage.txt");
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
