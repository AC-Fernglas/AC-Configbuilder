﻿using Sprache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;

namespace ACConfigBuilder
{
    public class InputToACObject
    {
        /// <summary>
        /// parses the Indentifyer for the current block of the configuration
        /// </summary>
        public void getIdentNameAndValue(string line, out bool configureExit, out bool subIdentExit, out string subIdent, out string subIdentValue)
        {
            subIdent = ParserGrammar.getsubident.Parse(line);
            configureExit = false;
            subIdentExit = false;
            subIdentValue = String.Empty;
            switch (subIdent)
            {
                case ParserVariables.networkDev:
                case ParserVariables.interfaceNetwokIf:
                case ParserVariables.proxySet:
                case ParserVariables.proxyIp:
                    subIdentExit = false;
                    subIdentValue = ParserGrammar.subidentvalue.Parse(line);
                    return;
                case ParserVariables.exit:
                    configureExit = true;
                    subIdentExit = true;
                    return;
                default:
                    return;
            }
        }
        /// <summary>
        /// parses the head of the currend configurationblock
        /// </summary>
        public void getConfigureIdent(string line, out bool configureExit, out string ident)
        {
            ident = String.Empty;
            configureExit = true;
            var parsedLine = ParserGrammar.getidentifier.Parse(line);
            switch (parsedLine)
            {
                case ParserVariables.configure:
                case ParserVariables.voip:
                case "\n":
                    ident = parsedLine;
                    configureExit = false;
                    break;
                default:
                    break;
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
            int networkDevListTndex = -1;
            int interfaceNetwokIfListTndex = -1;
            int proxySetListTndex = -1;
            int proxyIpListTndex = -1;
            string Name = string.Empty;

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
                        Name = returnRealName(subIdent);
                        if (Name == null)
                        {
                            continue;
                        }
                        switch (subIdent)
                        {
                            case ParserVariables.networkDev:
                                Networkdev newListNetworkDev = new Networkdev();
                                networkDevs.Add(newListNetworkDev);
                                networkDevListTndex++;
                                AC = ListParsing(AC, Name, subIdentValue, networkDevListTndex, AC.configureNetwork.networkdev, subIdent);
                                continue;
                            case ParserVariables.interfaceNetwokIf:
                                Interfacenetworkif newListInterfaceNetworkIf = new Interfacenetworkif();
                                interfaceNetworkIfs.Add(newListInterfaceNetworkIf);
                                interfaceNetwokIfListTndex++;
                                AC = ListParsing(AC, Name, subIdentValue, interfaceNetwokIfListTndex, AC.configureNetwork.interfacenetworkif, subIdent);
                                continue;
                            case ParserVariables.proxyIp:
                                Proxyip newListProxyIp = new Proxyip();
                                proxyIps.Add(newListProxyIp);
                                proxyIpListTndex++;
                                AC = ListParsing(AC, Name, subIdentValue, proxyIpListTndex, AC.configureviop.proxyip, subIdent);
                                continue;
                            case ParserVariables.proxySet:
                                Proxyset newListProxySet = new Proxyset();
                                proxySets.Add(newListProxySet);
                                proxySetListTndex++;
                                AC = ListParsing(AC, Name, subIdentValue, proxySetListTndex, AC.configureviop.proxyset, subIdent);
                                continue;
                            default:
                                continue; ;
                        }
                    }
                    Name = ParserGrammar.NameParser.Parse(line);
                    if (Name == ParserVariables.exit)
                    {
                        subIdentExit = true;

                        continue;
                    }
                    Name = returnRealName(Name);
                    if (Name == null)
                    {
                        continue;
                    }
                    switch (subIdent)
                    {
                        case ParserVariables.networkDev:
                            AC = AddToList(AC, Name, AC.configureNetwork.networkdev, subIdent, networkDevListTndex, getVar(Name, line));
                            continue;
                        case ParserVariables.interfaceNetwokIf:
                            AC = AddToList(AC, Name, AC.configureNetwork.interfacenetworkif, subIdent, interfaceNetwokIfListTndex, getVar(Name, line));
                            continue;
                        case ParserVariables.proxyIp:
                            AC = AddToList(AC, Name, AC.configureviop.proxyip, subIdent, proxyIpListTndex, getVar(Name, line));
                            continue;
                        case ParserVariables.proxySet:
                            AC = AddToList(AC, Name, AC.configureviop.proxyset, subIdent, proxySetListTndex, getVar(Name, line));
                            continue;
                        default:
                            break;
                    }
                }
            }
            return AC;
        }
        public dynamic getVar(string Name, string line)
        {
            if (Name == ParserVariables.activate)
            {
                return true;
            }
            else
            {
                return ParserGrammar.ValueParser.Parse(line);
            }
        }
        public ACConfig AddToList(ACConfig AC, string Name, dynamic List, string subIdent, int Index, dynamic value)
        {
            AC = ListParsing(AC, Name, value, Index, List, subIdent);
            return AC;
        }
        /// <summary>
        ///  wandelt die in der Konfiguration enthaltenen bezeichner in die Variablen Namen der Listen um da diese nicht zu 100% übereinstimmen.
        ///  Default return null !
        /// </summary>
        public string returnRealName(string Name)
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
        /// <summary>
        /// setzt die Value in aus dfem Bereich in die Passende liste an der Richtigen stelle
        /// </summary>
        /// <param name="Config">Loaded Config</param>
        /// <param name="Name"> Name of item to change</param>
        /// <param name="Value"> some Value of the setting</param>
        /// <param name="Index"></param>
        /// <param name="myList">SubList like Network-dev</param>
        /// <param name="subIdent">Name of the Subident like network-dev</param>
        /// <returns>ACConfig</returns>
        protected ACConfig ListParsing<T>(ACConfig Config, string Name, dynamic Value, int Index, IList<T> myList, string subIdent) where T : class
        {
            if (String.IsNullOrWhiteSpace(Value.ToString())) // falls es keine Value gibt brauch es nix machen
            {
                return Config;
            }
            var property = myList[Index].GetType().GetProperties().Where(a => a.Name == Name).FirstOrDefault(); // Gets the type of the property
            var propertyType = property.PropertyType;
            if (propertyType == typeof(Int32) || propertyType == typeof(Nullable<Int32>)) //if type = int then parse Value into an integer
            {
                property.SetValue(myList[Index], Convert.ToInt32(Value));
            }
            else if (propertyType.IsEnum) // if type = enum then parse Value into a enum
            {
                var newValue = selectRightEnummember(Value, property);
                if (newValue == null)
                {
                    return Config;
                }
                else
                {
                    property.SetValue(myList[Index], newValue); // List all fields of an enum
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

            return SetInTheRightList(Config, myList, subIdent);

        }
        private ACConfig SetInTheRightList(ACConfig Config, dynamic myList, string subIdent)
        {
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
                    return Config;
            }
        }
        private object selectRightEnummember(dynamic Value, PropertyInfo property)
        {
            var member = property.PropertyType.GetFields()
            .Where(p => 
             p.Name == Convert.ToString(Value) ||
             Convert.ToString(Value) == Convert.ToString(p.GetCustomAttributes(typeof(NameAttribute), false)))
            .FirstOrDefault();
            if (member == null)
            {
                return member;
            }
            return Enum.Parse(property.PropertyType, member.Name);  // parses Value into an enumValue/
        }
    }
}
