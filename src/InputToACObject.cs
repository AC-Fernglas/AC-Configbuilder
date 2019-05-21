using Sprache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACConfigBuilder
{
    public class InputToACObject
    {
        public void getIdentNameAndValue(string line, out bool configureExit, out bool subIdentExit, out string subIdent, out string subIdentValue) // parses the Indentifyer for the current block of the configuration
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
        public void getConfigureIdent(string line, out bool configureExit, out string ident) //parses the head of the currend configurationblock
        {
            ident = String.Empty;
            configureExit = true;
            var parsedLine = ParserGrammar.getidentifier.Parse(line);
            if (parsedLine == "configure network" || parsedLine == "configure voip" || parsedLine == Environment.NewLine || parsedLine == "\n")
            {
                ident = parsedLine;
                configureExit = false;
            }
        }
        public ACConfig parseinobject(StreamReader Reader)
        {
            Configureviop vo = new Configureviop();
            ConfigureNetwork co = new ConfigureNetwork();
            List<Networkdev> networkDevs = new List<Networkdev>();
            List<Interfacenetworkif> interfaceNetworkIfs = new List<Interfacenetworkif>();
            List<Proxyip> proxyIps = new List<Proxyip>();
            List<Proxyset> proxySets = new List<Proxyset>();
            ACConfig AC = new ACConfig();


            AC.configureNetwork = co;
            AC.configureviop = vo;

            AC.configureNetwork.networkdev = networkDevs;
            AC.configureNetwork.interfacenetworkif = interfaceNetworkIfs;

            AC.configureviop.proxyip = proxyIps;
            AC.configureviop.proxyset = proxySets;

            string ident = String.Empty;
            bool configureExit = true;
            string subIdent = String.Empty;
            bool subIdentExit = true;
            string subIdentValue = String.Empty;
            int networkDevListTndex = 0;
            int interfaceNetwokIfListTndex = 0;
            int proxySetListTndex = 0;
            int proxyIpListTndex = 0;

            using (Reader)
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
                        getIdentNameAndValue(line, out configureExit, out subIdentExit, out subIdent, out subIdentValue);                   // wenn true -> keien Bereich(networkDev / interfaceNetworkIf) in dem Konfig konfiguriert wird
                        var Name = returnRealName(subIdent);
                        if (Name == null)
                        {
                            continue;
                        }
                        if (subIdent == ParserVariables.networkDev)
                        {
                            Networkdev newListNetworkDev = new Networkdev();
                            networkDevs.Add(newListNetworkDev);                                                                           //neue Liste wird erstellt für jede Bereich von networkDev
                            AC = ListParsing(AC, Name, subIdentValue, networkDevListTndex, AC.configureNetwork.networkdev, subIdent);
                        }
                        else if (subIdent == ParserVariables.interfaceNetwokIf)
                        {
                            Interfacenetworkif newListInterfaceNetworkIf = new Interfacenetworkif();
                            interfaceNetworkIfs.Add(newListInterfaceNetworkIf);
                            AC = ListParsing(AC, Name, subIdentValue, interfaceNetwokIfListTndex, AC.configureNetwork.interfacenetworkif, subIdent);
                        }
                        else if (subIdent == ParserVariables.proxySet)
                        {
                            Proxyset newListProxySet = new Proxyset();
                            proxySets.Add(newListProxySet);
                            AC = ListParsing(AC, Name, subIdentValue, proxySetListTndex, AC.configureviop.proxyset, subIdent);
                        }
                        else if (subIdent == ParserVariables.proxyIp)
                        {
                            Proxyip newListProxyIp = new Proxyip();
                            proxyIps.Add(newListProxyIp);
                            AC = ListParsing(AC, Name, subIdentValue, proxyIpListTndex, AC.configureviop.proxyip, subIdent);
                        }
                        continue;
                    }

                    if (ident == "configure network" && subIdent == ParserVariables.networkDev)
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
                        AC = AddToList(AC, Name, AC.configureNetwork.networkdev, subIdent, networkDevListTndex, ParserGrammar.ValueParser.Parse(line));
                        continue;
                    }
                    else if ( ident == "configure network" && subIdent == ParserVariables.interfaceNetwokIf)
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
                        AC = AddToList(AC, Name, AC.configureNetwork.interfacenetworkif, subIdent, interfaceNetwokIfListTndex, ParserGrammar.ValueParser.Parse(line));
                        continue;
                    }
                    if (ident == "configure voip" && subIdent == ParserVariables.proxySet)
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
                        AC = AddToList(AC, Name, AC.configureviop.proxyset, subIdent, proxySetListTndex, ParserGrammar.ValueParser.Parse(line));
                        continue;
                    }
                    else if (ident == "configure voip" && subIdent == ParserVariables.proxyIp)
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
                        AC = AddToList(AC,Name, AC.configureviop.proxyip,subIdent,proxyIpListTndex, ParserGrammar.ValueParser.Parse(line));
                        continue;
                    }
                }
            }
            return AC;
        }
        public ACConfig AddToList(ACConfig AC, string Name, dynamic List, string subIdent,int Index, string value )
        {
            if (Name == ParserVariables.activate)
            {
                AC = ListParsing(AC, Name, true, Index, List, subIdent);
            }
            else
            {
                AC = ListParsing(AC, Name, value, Index, List, subIdent);
            }
            return AC;
        }
        public string returnRealName(string Name) // wandelt die in der Konfiguration enthaltenen bezeichner in die Variablen Namen der Listen um da diese nicht zu 100% übereinstimmen 
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
                    return ParserVariables.successdetint;
                case ParserVariables.proxyredundancymode:
                    return ParserVariables.proxymode;
                case ParserVariables.isproxyhotswap:
                    return ParserVariables.proxyswap;
                case ParserVariables.proxyloadbalancingmethod:
                    return ParserVariables.proxymethod;
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
        protected ACConfig ListParsing(ACConfig Config, string Name, dynamic Value, int Index, dynamic myList, string subIdent) //setzt die Value in aus dfem Bereich in die Passende liste an der Richtigen stelle
        {
            if (String.IsNullOrWhiteSpace(Value.ToString())) // falls es keine Value gibt brauch es nix machen
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
                        Config.configureNetwork.interfacenetworkif = myList;
                        return Config;
                    case ParserVariables.proxySet:
                        Config.configureviop.proxyset = myList;
                        return Config;
                    case ParserVariables.proxyIp:
                        Config.configureviop.proxyip = myList;
                        return Config;
                    default:
                        break;
                }
                return Config;
            }
            return Config;
        }
    }
}
