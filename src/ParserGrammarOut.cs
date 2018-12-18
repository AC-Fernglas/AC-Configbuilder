using System;
using System.Collections.Generic;
using System.Text;

namespace secondtry
{
    public class ParserGrammarOut
    {
        public void getobject(ACConfig AC)
        {
            List<string> configback = new List<string>();
            if (AC.configureNetwork != null)
            {
                if (AC.configureNetwork.networkdev != null)
                {
                    foreach (var item in AC.configureNetwork.networkdev)
                    {
                        foreach (var i in AC.configureNetwork.networkdev)
                        {
                            if (item.listid == i.listid)
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
                        }
                    }
                }
                if (AC.configureNetwork.interfacenetworkif != null)
                {
                    foreach (var item in AC.configureNetwork.interfacenetworkif)
                    {
                        foreach (var i in AC.configureNetwork.interfacenetworkif)
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
                    }
                }
            }
            if (AC.configureviop != null)
            {
                configback.Add("configure voip");
                if (AC.configureviop.proxyset != null)
                {
                    foreach (var item in AC.configureviop.proxyset)
                    {
                        foreach (var i in AC.configureviop.proxyset)
                        {
                            foreach (var propertyInfo in item.GetType().GetProperties())
                            {
                                var value = propertyInfo.GetValue(item);
                                if (value != null)
                                {
                                    switch (propertyInfo.Name)
                                    {
                                        case "listid":
                                            configback.Add(" " + "proxy-set" + " " + value);
                                            break;
                                        case "proxyname":
                                            configback.Add("  " + "proxy-name" + " " + value);
                                            break;
                                        case "Proxyenablekeepalive":
                                            configback.Add("  " + "proxy-enable-keep-alive" + " " + value);
                                            break;
                                        case "srdname":
                                            configback.Add("  " + "srd-name" + " " + value);
                                            break;
                                        case "sbcipv4sipintname":
                                            configback.Add("  " + "sbcipv4-sip-int-name" + " " + value);
                                            break;
                                        case "keepalivefailresp":
                                            configback.Add("  " + "keepalive-fail-resp" + " " + value);
                                            break;
                                        case "successdetectretries":
                                            configback.Add("  " + "success-detect-retries" + " " + value);
                                            break;
                                        case "successdetectint":
                                            configback.Add("  " + "success-detect-int" + " " + value);
                                            break;
                                        case "Proxyredundancymode":
                                            configback.Add("  " + "proxy-redundancy-mode" + " " + value);
                                            break;
                                        case "isproxyhotswap":
                                            configback.Add("  " + "is-proxy-hot-swap" + " " + value);
                                            break;
                                        case "proxyloadbalancingmethod":
                                            configback.Add("  " + "proxy-load-balancing-method" + " " + value);
                                            break;
                                        case "minactiveservlb":
                                            configback.Add("  " + "min-active-serv-lb" + " " + value);
                                            break;
                                        case "activate":
                                            if (value.ToString() == "True")
                                            {
                                                configback.Add("  activate");
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            configback.Add(" exit");
                        }
                    }
                    foreach (var item in AC.configureviop.proxyip)
                    {
                            foreach (var propertyInfo in item.GetType().GetProperties())
                            {
                                var value = propertyInfo.GetValue(item); // erweitere das auf die Ausgabe und dann sollte es funktionieren
                                if (value != null)
                                {
                                    switch (propertyInfo.Name)
                                    {
                                        case "ip":
                                            configback.Add(" " + "proxy - ip" + " " + value);
                                            break;
                                        case "proxyadress":
                                            configback.Add("  " + "proxy-address" + " " + value);
                                            break;
                                        case "Transporttype":
                                            configback.Add("  " + "transport-type" + " " + value);
                                            break;
                                        case "activate":
                                            if (value.ToString() == "True")
                                            {
                                                configback.Add("  activate");
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            configback.Add(" exit");
                    }
                    configback.Add("exit");
                }
            }
            giveitback(configback);
        }

        private void giveitback(List<string> back)
        {
            foreach (var item in back)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
