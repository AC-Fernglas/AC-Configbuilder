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
                var Net = c.Option(@"--networkdev <anzahl>", "Setzt die Anzahl für Networkdevabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Int = c.Option(@"--interfacenetworkif <anzahl>", "Setzt die Anzahl für Interfacenetworkifabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Set = c.Option(@"--proxyset <anzahl>", "Setzt die Anzahl für Proxysetabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                var Ip = c.Option(@"--proxyip <anzahl>", "Setzt die Anzahl für Proxyipabschnitte. Normal ist dieser Wert auf 1", CommandOptionType.SingleValue);
                //c.OnExecute(() => { obj.RunCreate(path, Net,Int,Set,Ip); });
            });
            return app.Execute(commands);

        }
    }
    class Execute
    {
        private void setuserpath(string mypath) //dunno
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
                    exe.replaceitem(AC, myconfig); //replaces the wanted details
                    obj.getobject(AC, item); //output
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

            Networkdev newList = new Networkdev();
            networkDevs.Add(newList);

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
                    if (configureExit)
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
                            networkDevs.Add(newList);
                            AC = ObjectNetworkdev(AC, Name, subIdentValue, networkDevListTndex);
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
                            AC = ObjectNetworkdev(AC, Name, Value, networkDevListTndex);
                        }
                        else
                        {
                           var Value = ParserGrammar.ValueParser.Parse(line);
                            AC = ObjectNetworkdev(AC, Name, Value, networkDevListTndex);
                        }


                        continue;
                    }
                    else if (configureExit == false && subIdentExit == false && ident == "configure network" && subIdent == "interface network-if")
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
                            AC = ObjectNetworkdev(AC, Name, Value, networkDevListTndex);
                        }
                        else
                        {
                            var Value = ParserGrammar.ValueParser.Parse(line);
                            AC = ObjectNetworkdev(AC, Name, Value, networkDevListTndex);
                        }
                        continue;
                    }
                    if (ident == "configure voip" && subIdent == "proxy-set")
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
                            AC = ObjectNetworkdev(AC, Name, Value, networkDevListTndex);
                        }
                        else
                        {
                            var Value = ParserGrammar.ValueParser.Parse(line);
                            AC = ObjectNetworkdev(AC, Name, Value, networkDevListTndex);
                        }
                        continue;
                    }
                    else if (ident == "configure voip" && subIdent == "proxy-ip")
                    { var Name = ParserGrammar.NameParser.Parse(line);
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
                            AC = ObjectNetworkdev(AC, Name, Value, networkDevListTndex);
                        }
                        else
                        {
                           var Value = ParserGrammar.ValueParser.Parse(line);
                            AC = ObjectNetworkdev(AC, Name, Value, networkDevListTndex);
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
                    return "apptape";
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
        private ACConfig ObjectNetworkdev(ACConfig Config, string Name, dynamic Value, int Index)
        {
            if (Value == null)
            {
                return Config;
            }
            var ListNetworkdev = Config.configureNetwork.networkdev;
            var property = ListNetworkdev[Index].GetType().GetProperties().FirstOrDefault(d => d.Name == Name);
            var propertyType = property.PropertyType;

            if(propertyType == typeof(Int32)  || propertyType == typeof(Nullable<Int32>))
            {
                property.SetValue(ListNetworkdev[Index], Convert.ToInt32(Value));
            }
            else if (propertyType.IsEnum)
            {
                var enumMember = propertyType
                    .GetFields() // List all fields of an enum
                    .FirstOrDefault(m => 
                        m.Name == Convert.ToString(Value) ||
                        m.GetCustomAttributes(typeof(NameAttribute), false) // Search for the NameAttribute
                            .Any(a => (a as NameAttribute).Name == Convert.ToString(Value) // check if the name Attribut has the same value as the Value.
                        )
                    );
                if(enumMember == null)
                {
                    Console.WriteLine($"Warn: skip property {property.Name} because the value {Value} is not valid for this field.");
                    return Config;
                }
                var enumValue = Enum.Parse(propertyType, enumMember.Name);
                property.SetValue(ListNetworkdev[Index], enumValue);

                //Console.WriteLine($"Warn: skip property {property.Name} because enums currently not supported.");
            }
            else if (propertyType == typeof(Boolean))
            {
                property.SetValue(ListNetworkdev[Index], Convert.ToBoolean(Value));
            }
            else
            {
                property.SetValue(ListNetworkdev[Index], Value);
            }

            return Config;
        }
        private string validpath(CommandOption path)
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
        public void replaceitem(ACConfig AC, ACConfig myconfig)
        {
            if (myconfig.configureNetwork != null)
            {
                if (myconfig.configureNetwork.networkdev != null)
                {
                    foreach (var item in myconfig.configureNetwork.networkdev)
                    {
                        foreach (var i in AC.configureNetwork.networkdev)
                        {
                            if (item.listid == i.listid)
                            {

                                change(i, item);
                            }
                        }
                    }
                }
                if (myconfig.configureNetwork.interfacenetworkif != null)
                {
                    foreach (var item in myconfig.configureNetwork.interfacenetworkif)
                    {
                        foreach (var i in AC.configureNetwork.interfacenetworkif)
                        {
                            if (item.listid == i.listid)
                            {

                                change(i, item);
                            }
                        }
                    }
                }
            }
            if (myconfig.configureviop != null)
            {
                if (myconfig.configureviop.proxyset != null)
                {
                    foreach (var item in myconfig.configureviop.proxyset)
                    {
                        foreach (var i in AC.configureviop.proxyset)
                        {
                            if (item.listid == i.listid)
                            {

                                change(i, item);
                            }
                        }
                    }
                }
                if (myconfig.configureviop.proxyset != null)
                {
                    foreach (var item in myconfig.configureviop.proxyip)
                    {
                        foreach (var i in AC.configureviop.proxyip)
                        {
                            foreach (var propertyInfo in item.GetType().GetProperties())
                            {
                                if (item.ip == i.ip)
                                {

                                    change(i, item);
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
