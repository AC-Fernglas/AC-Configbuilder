using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Sprache;
using Newtonsoft.Json;


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
        public int Idel(string[] commands)
        {
            var app = new CommandLineApplication();
            Execute obj = new Execute();
            var helptemplate = "-h|--help";
            app.HelpOption(helptemplate);
            app.Command("use", u =>
            {
                u.HelpOption(helptemplate);
                u.Description = "Dieser Befehl soll es ermöglichen die hinterlegte Konfiguration zu editieren.";
                var path = u.Option(@"--path <fullpath>", "Setzt einen benutzerdefinierten Pfad, wenn dieser Befehl nicht benutzt wird, wird der Sampelsordner benutzt.", CommandOptionType.SingleValue);
                u.OnExecute(() => { obj.run(path); });
            });
            return app.Execute(commands);

        }
    }
    class Execute
    {
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
        public void run(CommandOption path)
        {
            Execute exe = new Execute();
            ACConfig AC = new ACConfig();
            Output obj = new Output();
            string mypath = $@"..\\..\\..\\..\\samples";
            if (exe.validpath(path) != " ")
            {
                mypath = exe.validpath(path);
            }
            var newFile = new StringBuilder();
            var config = File.ReadAllText(@"..\\..\\..\\..\\config\\qwertz.json"); //get json
            var host = JsonConvert.DeserializeObject<ACConfig>(config); //get path to json
            var myconfig = JsonConvert.DeserializeObject<ACConfig>(File.ReadAllText(host.userpath));//open json to use
            List<string> dirs = new List<string>();
            dirs.Add(exe.findDirectorys(mypath));
            foreach (var item in dirs)
            {
                AC = exe.parseinobject(item);
                exe.replaceitem(AC, myconfig);
                obj.getobject(AC, item);
            }
        }
        private string findDirectorys(string mypath)
        {
            string[] dirs = Directory.GetFiles(mypath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (var item in dirs)
            {
                return item;
            }
            return null;
        }
        private void getIdentNameAndValue(string line, out bool configureExit, out bool subidentexit, out string subident, out string subidentvalue)
        {
            subident = ParserGrammar.getsubident.Parse(line);
            configureExit = false;
            subidentexit = false;
            subidentvalue = String.Empty;
            if (subident == "network-dev" ||
                subident == "interface network-if" ||
                subident == "proxy-set" ||
                subident == "proxy-ip" ||
                subident == "exit")
            {
                if (subident == "exit")
                {
                    configureExit = true;
                    subidentexit = true;
                    return;
                }
                subidentexit = false;
                subidentvalue = ParserGrammar.subidentvalue.Parse(line);
            }
        }
        private void getConfigureIdent(string line, out bool configureexit, out string ident)
        {
            ident = String.Empty;
            configureexit = true;

            if (ParserGrammar.getidentifier.Parse(line) == "configure network" || ParserGrammar.getidentifier.Parse(line) == "configure voip" || ParserGrammar.getidentifier.Parse(line) == Environment.NewLine || ParserGrammar.getidentifier.Parse(line) == "\n")
            {
                ident = ParserGrammar.getidentifier.Parse(line);
                configureexit = false;
            }
        }
        private ACConfig parseinobject(string path)
        {


            Configureviop vo = new Configureviop();
            ConfigureNetwork co = new ConfigureNetwork();
            List<Networkdev> networkdev = new List<Networkdev>();
            List < Interfacenetworkif> interfacenetworkif = new List<Interfacenetworkif>();
            List<Proxyip> proxyip = new List<Proxyip>();
            List<Proxyset> proxyset = new List<Proxyset>();
            ACConfig AC = new ACConfig();

            Networkdev newlist = new Networkdev();
            networkdev.Add(newlist);

            AC.configureNetwork = co;
            AC.configureviop = vo;

            AC.configureNetwork.networkdev = networkdev;
            AC.configureNetwork.interfacenetworkif = interfacenetworkif;

            AC.configureviop.proxyip = proxyip;
            AC.configureviop.proxyset = proxyset;
            
            string ident = " ";
            bool configureexit = true;
            string subident = " ";
            bool subidentexit = true;
            string subidentvalue = "";

            using (StreamReader Reader = new StreamReader(path))
            {
                string line = " ";
                while ((line = Reader.ReadLine()) != null)
                {
                    if (configureexit)
                    {
                        getConfigureIdent(line, out configureexit, out ident);
                        continue;
                    }
                    if (subidentexit)
                    {
                        getIdentNameAndValue(line, out configureexit, out subidentexit, out subident, out subidentvalue);
                        var Name = zurück(subident);
                        if (Name != null && subident == "network-dev")
                        {
                            AC = ObjectNetworkdev(AC, Name, subidentvalue);
                        }
                        continue;
                       
                    }

                    if (configureexit == false && subidentexit == false && ident == "configure network" && subident == "network-dev")
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        var Value = ParserGrammar.ValueParser.Parse(line);
                        Name = zurück(Name);
                        if (Name != null )
                        {
                            AC = ObjectNetworkdev(AC, Name, Value);
                        }
                        continue;
                    }
                    else if (configureexit == false && subidentexit == false && ident == "configure network" && subident == "interface network-if")
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        var Value = ParserGrammar.ValueParser.Parse(line);
                        Name = zurück(Name);
                        if (Name != null)
                        {
                            
                        }
                        continue;
                    }
                    if (ident == "configure voip" && subident == "proxy-set")
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        var Value = ParserGrammar.ValueParser.Parse(line);
                        Name = zurück(Name);
                        if (Name != null)
                        {
                        }
                        continue;
                    }
                    else if (ident == "configure voip" && subident == "proxy-ip")
                    {
                        var Name = ParserGrammar.NameParser.Parse(line);
                        var Value = ParserGrammar.ValueParser.Parse(line);
                        Name = zurück(Name);
                        if (Name != null)
                        {
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
                case ParserVariables.proiplistident :
                    return "ip";
                case ParserVariables.activate:
                    return "activate";
                case ParserVariables.vlan:
                    return "vlan";
                case ParserVariables.underlyingdev :
                    return "underlyingdev";
                case ParserVariables.Name:
                    return "Name";
                case ParserVariables.tag :
                    return "tag";
                case ParserVariables.apptype :
                    return "apptape";
                case ParserVariables.ipaddress :
                    return "ipaddress";
                case ParserVariables.prefixlength :
                    return "prefixlength";
                case ParserVariables.gateway:
                    return "gateway";
                case ParserVariables.underlyingif :
                    return "underlyingif";
                case ParserVariables.proxyname :
                    return "proxyname";
                case ParserVariables.proxyenablekeepalive:
                    return "proxyenablekeepalive";
                case ParserVariables.srdname:
                    return "srdname";
                case ParserVariables.sbcipv4sipintname :
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
        private ACConfig ObjectNetworkdev(ACConfig Config, string Name, dynamic Value)
        {
           var ListNetworkdev = Config.configureNetwork.networkdev;
            var i = 0;
                foreach (var item in ListNetworkdev[i].GetType().GetProperties())
                {
                    if (item.GetValue(ListNetworkdev[i]) == null && (Value != null && item.Name == Name))
                    {
                    item.GetType().GetProperty(item.Name).SetValue(item.GetValue(item), Value);
                     }
                    else
                    {
                    i++;
                    ObjectNetworkdev(Config, Name, Value);
                    }
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
