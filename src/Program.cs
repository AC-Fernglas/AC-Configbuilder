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
            app.Command("create", c =>
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
        private void getIdentNameAndValue(string line,out bool configureExit,out bool subidentexit, out string subident, out string subidentvalue)
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
        private void getConfigureIdent(string line,out bool configureexit, out string ident)
        {
            ident = String.Empty;
            configureexit = true;

            if (ParserGrammar.getidentifier.Parse(line) == "configure network" || ParserGrammar.getidentifier.Parse(line) == "configure voip" || ParserGrammar.getidentifier.Parse(line) == Environment.NewLine|| ParserGrammar.getidentifier.Parse(line) == "\n")
            {
                ident = ParserGrammar.getidentifier.Parse(line);
                configureexit = false;
            }
        }
        private ACConfig parseinobject(string path)
        {


            Configureviop vo = new Configureviop();
            ConfigureNetwork co = new ConfigureNetwork();
            ACConfig AC = new ACConfig();
            AC.configureNetwork = co;
            AC.configureviop = vo;

            List<Networkdev> netlist = new List<Networkdev>();
            List<Interfacenetworkif> inif = new List<Interfacenetworkif>();
            List<Proxyip> prip = new List<Proxyip>();
            List<Proxyset> prese = new List<Proxyset>();
            

            string ident = " ";
            bool configureexit = true;
            string subident = " ";
            bool subidentexit = true;
            string subidentvalue = "";

            bool activate = false;

            string underlying = "";
            string name = "";
            string tagging = "";

            string apptype = "";
            string ipaddr = "";
            int prel = 0;
            string gateway = "";
            string name2 = "";
            string udev = "";
            int vlan = 0;

            string prname = "";
            string peka = "";
            string srdname = "";
            string ssin = "";
            string kfr = "";
            int sdr = 0;
            int sdi = 0;
            string prm = "";
            int iphs = 0;
            int plbm = 0;
            int masl = 0;

            string prad = "";
            string taty = "";


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
                        getIdentNameAndValue(line,out configureexit,out subidentexit, out subident, out subidentvalue);
                        continue;
                    }

                    if (configureexit == false && subidentexit == false && ident == "configure network" && subident == "network-dev")
                    {
                        switch (ParserGrammar.networkDiviceNameParser.Parse(line))
                        {
                            case "vlan-id":
                                vlan = int.Parse(ParserGrammar.networkDiviceValueParser.Parse(line));
                                continue;
                            case "underlying-if":
                                underlying = ParserGrammar.networkDiviceValueParser.Parse(line);
                                continue;
                            case "name":
                                name = ParserGrammar.networkDiviceValueParser.Parse(line);
                                continue;
                            case "tagging":
                                tagging = ParserGrammar.networkDiviceValueParser.Parse(line);
                                continue;
                            case "activate":
                                activate = true;
                                continue;
                            default:
                                netlist.Add(createlist(int.Parse(subidentvalue), vlan, underlying, name, tagging, activate));
                                activate = false;
                                subidentexit = true;
                                continue;
                        }
                    }
                    else if (configureexit == false && subidentexit == false && ident == "configure network" && subident == "interface network-if")
                    {
                        switch (ParserGrammar.interfaceNetworkIfNameParser.Parse(line))
                        {
                            case "application-type":
                                apptype = ParserGrammar.interfaceNetworkIfValueParser.Parse(line);
                                continue;
                            case "ip-address":
                                ipaddr = ParserGrammar.interfaceNetworkIfValueParser.Parse(line);
                                continue;
                            case "prefix-length":
                                prel = int.Parse(ParserGrammar.interfaceNetworkIfValueParser.Parse(line));
                                continue;
                            case "gateway":
                                gateway = ParserGrammar.interfaceNetworkIfValueParser.Parse(line);
                                continue;
                            case "name":
                                name2 = ParserGrammar.interfaceNetworkIfValueParser.Parse(line);
                                continue;
                            case "underlying-dev":
                                udev = ParserGrammar.interfaceNetworkIfValueParser.Parse(line);
                                continue;
                            case "activate":
                                activate = true;
                                continue;
                            default:
                                inif.Add(createlistinif(apptype, ipaddr, prel, gateway, name2, udev, int.Parse(subidentvalue), activate));
                                activate = false;
                                subidentexit = true;
                                continue;
                        }
                    }
                    if (ident == "configure voip" && subident == "proxy-set")
                    {
                        switch (ParserGrammar.proxySetNameParser.Parse(line))
                        {
                            case "proxy-name":
                                prname = ParserGrammar.proxySetValueParser.Parse(line);
                                continue;
                            case "proxy-enable-keep-alive":
                                peka = ParserGrammar.proxySetValueParser.Parse(line);
                                continue;
                            case "srd-name":
                                srdname = ParserGrammar.proxySetValueParser.Parse(line);
                                continue;
                            case "sbcipv4-sip-int-name":
                                ssin = ParserGrammar.proxySetValueParser.Parse(line);
                                continue;
                            case "keepalive-fail-resp":
                                kfr = ParserGrammar.proxySetValueParser.Parse(line);
                                continue;
                            case "success-detect-retries":
                                sdr = int.Parse(ParserGrammar.proxySetValueParser.Parse(line));
                                continue;
                            case "success-detect-int":
                                sdi = int.Parse(ParserGrammar.proxySetValueParser.Parse(line));
                                continue;
                            case "proxy-redundancy-mode":
                                prm = ParserGrammar.proxySetValueParser.Parse(line);
                                continue;
                            case "is-proxy-hot-swap":
                                iphs = int.Parse(ParserGrammar.proxySetValueParser.Parse(line));
                                continue;
                            case "proxy-load-balancing-method":
                                plbm = int.Parse(ParserGrammar.proxySetValueParser.Parse(line));
                                continue;
                            case "min-active-serv-lb":
                                masl = int.Parse(ParserGrammar.proxySetValueParser.Parse(line));
                                continue;
                            case "activate":
                                activate = true;
                                continue;
                            default:
                                prese.Add(createlistprese(prname, peka, srdname, ssin, kfr, sdr, sdi, prm, iphs, plbm, masl, int.Parse(subidentvalue), activate));
                                activate = false;
                                subidentexit = true;
                                continue;
                        }
                    }
                    else if (ident == "configure voip" && subident == "proxy-ip")
                    {
                        switch (ParserGrammar.proxyIpNameParser.Parse(line))
                        {
                            case "proxy-address":
                                prad = ParserGrammar.proxyIpValueParser.Parse(line);
                                continue;
                            case "transport-type":
                                taty = ParserGrammar.proxyIpValueParser.Parse(line);
                                continue;
                            case "activate":
                                activate = true;
                                continue;
                            default:
                                prip.Add(createlistprip(prad, taty, subidentvalue, activate));
                                activate = false;
                                subidentexit = true;
                                continue;
                        }
                    }
                }
            }
                co.networkdev = netlist;
                co.interfacenetworkif = inif;
                vo.proxyip = prip;
                vo.proxyset = prese;
            return AC;
        }

        private Networkdev createlist(int listid, int vlan, string underlying, string name, string tagging, bool activate)
        {
            Enum.TryParse(tagging, out tag Tagging);
            Networkdev net = new Networkdev
            {
                listid = listid,
                vlanip = vlan,
                underlyingif = underlying,
                name = name,
                tagging = Tagging,
                activate = activate
            };
            return net;
        }
        private Proxyip createlistprip(string prad,string taty, string subidentvalue, bool activate)
        {
            Enum.TryParse(taty, out transporttype Transporttype);
            Proxyip prip = new Proxyip
            {
                ip = subidentvalue,
                proxyadress = prad,
                Transporttype = Transporttype,
                activate = activate
            };
            return prip;
        }
        private Proxyset createlistprese(string prname,string peka,string srdname,string ssin,string kfr,int sdr,int sdi,string prm,int iphs,int plbm,int masl, int subidentvalue, bool activate)
        {
            if (peka == "using-option")
            {
                peka = "uoption";
            }
            Enum.TryParse(peka, out proxyenablekeepalive blub);
            Enum.TryParse(prm, out proxyredundancymode blab);
            Proxyset prse = new Proxyset
            {
                listid = subidentvalue,
                proxyname = prname,
                Proxyenablekeepalive = blub,
                srdname = srdname,
                sbcipv4sipintname = ssin,
                keepalivefailresp = kfr,
                successdetectretries = sdr,
                successdetectint = sdi,
                Proxyredundancymode = blab,
                isproxyhotswap = iphs,
                proxyloadbalancingmethod =plbm,
                minactiveservlb= masl,
                activate = activate
            };
            return prse;
        }
        private Interfacenetworkif createlistinif(string apptype, string ipadr, int prel, string gateway, string name2, string udev, int listid, bool activate)
        {
            Enum.TryParse(apptype, out applicationtype Apptype);
            Interfacenetworkif inif = new Interfacenetworkif
            {
                Applicationtype = Apptype,
                ipadress =ipadr,
                prefixlength =prel,
                gateway = gateway,
                name = name2,
                underlyingdev =udev,
                listid = listid,
                activate = activate
            };
            return inif;
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
