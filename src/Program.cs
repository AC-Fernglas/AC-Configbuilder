using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Sprache;
using System.Linq;

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
        public int Idel(string[] commands)
        {
            var app = new CommandLineApplication();
            var helptemplate = "-h|--help";
            app.HelpOption(helptemplate);
            app.Command("use", u =>
            {
                u.HelpOption(helptemplate);
                u.Description = "Dieser Befehl soll es ermöglichen die hinterlegte Konfiguration zu editieren.";
                var path = u.Option(@"--path <fullpath>", "Setzt einen benutzerdefinierten Pfad, wenn dieser Befehl nicht benutzt wird, wird der Sampelsordner benutzt.", CommandOptionType.SingleValue);
                u.OnExecute(() => { useon(path); });
            });
            return app.Execute(commands);
        }

        public void setuserpath(string mypath)
        {
            string[] userconfig = File.ReadAllLines(@".\config\qwertz.json");
            StringBuilder newFile = new StringBuilder();
            string[] file = File.ReadAllLines($@".\config\qwertz.json");
            List<string> list = new List<string>(file);
            list[2] = "\"userpath\": " + "\"" + mypath + "\"";
            foreach (string line in list)
            {
                newFile.Append(line + "\n");
            }
            File.WriteAllText($@".\config\qwertz.json", newFile.ToString());
        }
        public void useon(CommandOption path) // neue funktion -> suchen und ersetzten 
        {
            string mypath = $@".\samples";
            if (validpath(path) != " ")
            {
                mypath = validpath(path);
            }
            openfiles(mypath);

        }
        public void openfiles(string mypath)
        {
            var newFile = new StringBuilder();
            var config = File.ReadAllText(@".\config\qwertz.json"); //get json
            var host = JsonConvert.DeserializeObject<ACConfig>(config); //get path to json
            var myconfig = JsonConvert.DeserializeObject<ACConfig>(File.ReadAllText(host.userpath));//open json to use
            Console.WriteLine(JsonConvert.SerializeObject(myconfig));
            Console.ReadLine();
            string[] dirs = Directory.GetFiles(mypath, "*.txt", SearchOption.TopDirectoryOnly);
            foreach (var item in dirs)
            {
                parseinobject(item, myconfig);
            }
        }

        public void parseinobject(string path, ACConfig myconfig)
        {


            Configureviop vo = new Configureviop();
            ConfigureNetwork co = new ConfigureNetwork();
            ACConfig AC = new ACConfig();
            AC.configureNetwork = co;
            AC.configureviop = vo;

            List<networkdev> netlist = new List<networkdev>();
            List<interfacenetworkif> inif = new List<interfacenetworkif>();
            List<proxyip> prip = new List<proxyip>();
            List<proxyset> prese = new List<proxyset>();
            

            string ident = " ";
            bool configureexit = true;
            string subident = " ";
            bool subidentexit = true;
            string subidentvalue = "";

            string underlying = "";
            string name = "";
            string tagging = "";

            string apptype = "";
            string ipadr = "";
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
                        if (line == "" || ParserGrammarIn.getidentifier.Parse(line) == "configure network" || ParserGrammarIn.getidentifier.Parse(line) == "configure voip")
                        {
                            if (line == "")
                            {
                                continue;
                            }
                            ident = ParserGrammarIn.getidentifier.Parse(line);
                            configureexit = false;
                            continue;
                        }
                    }
                    else
                    {
                        if (subidentexit)
                        {
                            if (ParserGrammarIn.getsubident.Parse(line) == "network-dev" || ParserGrammarIn.getsubident.Parse(line) == "interface network-if" ||
                             ParserGrammarIn.getsubident.Parse(line) == "proxy-set" || ParserGrammarIn.getsubident.Parse(line) == "proxy-ip" || ParserGrammarIn.getsubident.Parse(line) == "exit")
                            {
                                if (ParserGrammarIn.getsubident.Parse(line) == "exit")
                                {
                                    configureexit = true;
                                    switch (subident)
                                    {
                                        case "network-dev":
                                            netlist.Add(createlist(int.Parse(subidentvalue), vlan, underlying, name, tagging));
                                            continue;
                                        case "interface network-if":
                                            inif.Add(createlistinif(apptype, ipadr, prel, gateway, name2, udev, int.Parse(subidentvalue)));
                                            continue;
                                        case "proxy-set":
                                            prese.Add(createlistprese(prname,peka,srdname,ssin,kfr,sdr,sdi,prm,iphs,plbm,masl, int.Parse(subidentvalue)));
                                            continue;
                                        case "proxy-ip":
                                           prip.Add(createlistprip(prad,taty, subidentvalue));
                                            continue;
                                        default:
                                            continue;

                                    };

                                }
                                subidentexit = false;
                                subident = ParserGrammarIn.getsubident.Parse(line);
                                subidentvalue = ParserGrammarIn.subidentvalue.Parse(line);
                                continue;
                            }
                        }
                        else
                        {
                            if (ident == "configure network")
                            {
                                if (subident == "network-dev")
                                {
                                    switch (ParserGrammarIn.dev.Parse(line))
                                    {
                                        case "vlan-id":
                                            vlan = int.Parse(ParserGrammarIn.devvalue.Parse(line));
                                            continue;
                                        case "underlying-if":
                                            underlying = ParserGrammarIn.devvalue.Parse(line);
                                            continue;
                                        case "name":
                                            name = ParserGrammarIn.devvalue.Parse(line);
                                            continue;
                                        case "tagging":
                                            tagging = ParserGrammarIn.devvalue.Parse(line);
                                            continue;
                                        case "activate":
                                            continue;
                                        default:
                                            subidentexit = true;
                                            continue;
                                    }
                                }
                                else if (subident == "interface network-if")
                                {
                                    switch (ParserGrammarIn.inif.Parse(line))
                                    {
                                        case "application-type":
                                            apptype = ParserGrammarIn.inifvalue.Parse(line);
                                            continue;
                                        case "ip-address":
                                            ipadr = ParserGrammarIn.inifvalue.Parse(line);
                                            continue;
                                        case "prefix-length":
                                            prel = int.Parse(ParserGrammarIn.inifvalue.Parse(line));
                                            continue;
                                        case "gateway":
                                            gateway = ParserGrammarIn.inifvalue.Parse(line);
                                            continue;
                                        case "name":
                                            name2 = ParserGrammarIn.inifvalue.Parse(line);
                                            continue;
                                        case "underlying-dev":
                                            udev = ParserGrammarIn.inifvalue.Parse(line);
                                            continue;
                                        case "activate":
                                            continue;
                                        default:
                                            subidentexit = true;
                                            continue;
                                    }
                                }
                            }
                            else if (ident == "configure voip")
                            {
                                if (subident == "proxy-set")
                                {
                                    switch (ParserGrammarIn.prse.Parse(line))
                                    {
                                        case "proxy-name":
                                            prname = ParserGrammarIn.prsevalue.Parse(line);
                                            continue;
                                        case "proxy-enable-keep-alive":
                                            peka = ParserGrammarIn.prsevalue.Parse(line);
                                            continue;
                                        case "srd-name":
                                            srdname = ParserGrammarIn.prsevalue.Parse(line);
                                            continue;
                                        case "sbcipv4-sip-int-name":
                                            ssin = ParserGrammarIn.prsevalue.Parse(line);
                                            continue;
                                        case "keepalive-fail-resp":
                                            kfr = ParserGrammarIn.prsevalue.Parse(line);
                                            continue;
                                        case "success-detect-retries":
                                            sdr = int.Parse(ParserGrammarIn.prsevalue.Parse(line));
                                            continue;
                                        case "success-detect-int":
                                            sdi = int.Parse(ParserGrammarIn.prsevalue.Parse(line));
                                            continue;
                                        case "proxy-redundancy-mode":
                                            prm = ParserGrammarIn.prsevalue.Parse(line);
                                            continue;
                                        case "is-proxy-hot-swap":
                                            iphs = int.Parse(ParserGrammarIn.prsevalue.Parse(line));
                                            continue;
                                        case "proxy-load-balancing-method":
                                            plbm = int.Parse(ParserGrammarIn.prsevalue.Parse(line));
                                            continue;
                                        case "min-active-serv-lb":
                                            masl = int.Parse(ParserGrammarIn.prsevalue.Parse(line));
                                            continue;
                                        case "activate":
                                            continue;
                                        default:
                                            subidentexit = true;
                                            continue;
                                    }
                                }
                                else if (subident == "proxy-ip")
                                {
                                    switch (ParserGrammarIn.prip.Parse(line))
                                    {
                                        case "proxy-address":
                                            prad = ParserGrammarIn.pripvalue.Parse(line);
                                            continue;
                                        case "transport-type":
                                            taty = ParserGrammarIn.pripvalue.Parse(line);
                                            continue;
                                        case "activate":
                                            continue;
                                        default:
                                            subidentexit = true;
                                            continue;
                                    }
                                }
                            }
                        }
                    }
                }
                co.networkdev = netlist;
                co.interfacenetworkif = inif;
                vo.proxyip = prip;
                vo.proxyset = prese;
            }
            replaceitem(AC,myconfig);
        }
        public networkdev createlist(int listid, int vlan, string underlying, string name, string tagging)
        {
            Enum.TryParse(tagging, out tag Tagging);
            networkdev net = new networkdev
            {
                listid = listid,
                vlanip = vlan,
                underlyingif = underlying,
                name = name,
                tagging = Tagging
            };
            return net;
        }
        public proxyip createlistprip(string prad,string taty, string subidentvalue)
        {
            Enum.TryParse(taty, out transporttype Transporttype);
            proxyip prip = new proxyip
            {
                ip = subidentvalue,
                proxyadress = prad,
                Transporttype = Transporttype
            };
            return prip;
        }
        public proxyset createlistprese(string prname,string peka,string srdname,string ssin,string kfr,int sdr,int sdi,string prm,int iphs,int plbm,int masl, int subidentvalue)
        {
            if (peka == "using-option")
            {
                peka = "uoption";
            }
            Enum.TryParse(peka, out proxyenablekeepalive blub);
            Enum.TryParse(prm, out proxyredundancymode blab);
            proxyset prse = new proxyset
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
                minactiveservlb= masl                
            };
            return prse;
        }
        public interfacenetworkif createlistinif(string apptype, string ipadr, int prel, string gateway, string name2, string udev, int listid)
        {
            Enum.TryParse(apptype, out applicationtype Apptype);
            interfacenetworkif inif = new interfacenetworkif
            {
                Applicationtype = Apptype,
                ipadress =ipadr,
                prefixlength =prel,
                gateway = gateway,
                name = name2,
                underlyingdev =udev,
                listid = listid
            };
            return inif;
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
        public void replaceitem(ACConfig AC, ACConfig myconfig)
        {
            if (myconfig.configureNetwork.networkdev != null)
            {
                foreach (var item in myconfig.configureNetwork.networkdev)
                {
                    foreach (var i in AC.configureNetwork.networkdev)
                    {
                        if (item.listid == i.listid)
                        {
                            if (item.name != null)
                            {
                                i.name = item.name;
                            }
                            if (item.vlanip.ToString() != null)
                            {
                                i.vlanip = item.vlanip;
                            }
                            if (item.underlyingif != null)
                            {
                                i.underlyingif = item.underlyingif;
                            }
                            if (item.tagging.ToString() != null)
                            {
                                i.tagging = item.tagging;
                            }
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
                            if (item.name != null)
                            {
                                i.name = item.name;
                            }
                            if (item.Applicationtype.ToString() != null)
                            {
                                i.Applicationtype = item.Applicationtype;
                            }
                            if (item.gateway != null)
                            {
                                i.gateway = item.gateway;
                            }
                            if (item.ipadress != null)
                            {
                                i.ipadress = item.ipadress;
                            }
                            if (item.prefixlength.ToString() != null)
                            {
                                i.prefixlength = item.prefixlength;
                            }
                            if (item.underlyingdev != null)
                            {
                                i.underlyingdev = item.underlyingdev;
                            }
                            
                        }
                    }
                }
            }
            if (myconfig.configureviop.proxyset != null)
            {
                foreach (var item in myconfig.configureviop.proxyset)
                {
                    foreach (var i in AC.configureviop.proxyset)
                    {
                        if (item.listid == i.listid)
                        {
                            if (item.isproxyhotswap.ToString() != null)
                            {
                                i.isproxyhotswap = item.isproxyhotswap;
                            }
                            if (item.keepalivefailresp != null)
                            {
                                i.keepalivefailresp = item.keepalivefailresp;
                            }
                            if (item.minactiveservlb.ToString() != null)
                            {
                                i.minactiveservlb = item.minactiveservlb;
                            }
                            if (item.Proxyenablekeepalive.ToString() != null)
                            {
                                i.Proxyenablekeepalive = item.Proxyenablekeepalive;
                            }
                            if (item.proxyloadbalancingmethod.ToString() != null)
                            {
                                i.proxyloadbalancingmethod = item.proxyloadbalancingmethod;
                            }
                            if (item.proxyname != null)
                            {
                                i.proxyname = item.proxyname;
                            }
                            if (item.Proxyredundancymode.ToString() != null)
                            {
                                i.Proxyredundancymode = item.Proxyredundancymode;
                            }
                            if (item.sbcipv4sipintname != null)
                            {
                                i.sbcipv4sipintname = item.sbcipv4sipintname;
                            }
                            if (item.srdname != null)
                            {
                                i.srdname = item.srdname;
                            }
                            if (item.successdetectint.ToString() != null)
                            {
                                i.successdetectint = item.successdetectint;
                            }
                            if (item.successdetectretries.ToString() != null)
                            {
                                i.successdetectretries = item.successdetectretries;
                            }
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
                        if (item.ip == i.ip)
                        {
                            if (item.proxyadress != null)
                            {
                                i.proxyadress = item.proxyadress;
                            }
                            if (item.Transporttype.ToString() != null)
                            {
                                i.Transporttype = item.Transporttype;
                            }
                        }
                    }
                }
            }
        }
    }
}
