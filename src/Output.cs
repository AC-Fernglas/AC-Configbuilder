using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace ACConfigBuilder
{
    public class Output
    {
        public void writeOutput(List<string> AC, Stream s)
        {
            StreamWriter writer = new StreamWriter(s);

            foreach (var item in AC)
            {
                writer.WriteLine(item);
                writer.Flush();
            }

        }
        public void writeOutput(List<string> AC, string path)
        {
            using (var stream = new StreamWriter(path))
            {
                writeOutput(AC, stream.BaseStream);
            }
        }
        public List<string> objectToList(ACConfig AC)
        {
            List<string> configback = new List<string>();
            if (AC.configureNetwork != null)
            {
                configback.Add("configure network");
                AddNetworkDevices(AC, configback);
                AddInterfaces(AC, configback);
                configback.Add("exit");
            }
            if (AC.configureviop != null)
            {
                configback.Add("configure voip");
                AddProxy(AC, configback);
                AddProxyIp(AC, configback);
                configback.Add("exit");
            }
            return configback;
        }

        private static void AddProxyIp(ACConfig AC, List<string> configback)
        {
            if (AC.configureviop.proxyip == null)
            {
                return;
            }
            foreach (var item in AC.configureviop.proxyip)
            {
                configback.AddRange(GetConfigStringFromObject(item));

            }
        }

        private static void AddProxy(ACConfig AC, List<string> configback)
        {
            if (AC.configureviop.proxyset == null)
            {
                return;
            }
            foreach (var item in AC.configureviop.proxyset)
            {
                configback.AddRange(GetConfigStringFromObject(item));
            }
        }

        private static void AddInterfaces(ACConfig AC, List<string> configback)
        {
            if (AC.configureNetwork.interfacenetworkif == null)
            {
                return;
            }
            foreach (var item in AC.configureNetwork.interfacenetworkif)
            {
                configback.AddRange(GetConfigStringFromObject(item));
            }
        }

        private static void AddNetworkDevices(ACConfig AC, List<string> configback)
        {
            if (AC.configureNetwork.networkdev == null)
            {
                return;
            }
            foreach (var item in AC.configureNetwork.networkdev)
            {
                configback.AddRange(GetConfigStringFromObject(item));
            }
        }

        protected static IEnumerable<string> GetConfigStringFromObject(object item)
        {
            var isSetValue = false;
            foreach (var propertyInfo in item.GetType().GetProperties())
            {
                var acProperty = propertyInfo.GetCustomAttributes(typeof(AcPropertyAttribute), false).FirstOrDefault() as AcPropertyAttribute;
                var name = acProperty == null ? propertyInfo.Name : acProperty.PropertyName;
                var value = propertyInfo.GetValue(item);
                if (((value is Boolean || value is bool) && (bool)value == false))
                {
                    continue;
                }
                if (value != null)
                {
                    isSetValue = true;
                    if ((value is Boolean || value is bool) && (bool)value == true)
                    {
                        yield return "  activate";
                    }
                    else
                    {
                        yield return "  " + name + " " + value;
                    }
                }
            }
            if (isSetValue)
            {
                yield return " exit";
            }
            else
            {
                yield break;
            }
        }
        /// <summary>
        /// Creates the new Configuration for AC
        /// </summary>
        /// <param name="Net">Set Count of the Network-Dev Subsection</param>
        /// <param name="Int">Set Count of the Interface Network-if Subsection</param>
        /// <param name="Set">Set Count of the Proxy-Set Subsection</param>
        /// <param name="Ip">Set Count of the Proxy-IP Subsection</param>
         public void Write(CommandOption Net, CommandOption Int, CommandOption Set, CommandOption Ip, string OutputPath, string tempaltePath)
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
            if (Int.HasValue() == true && Int.Value() != null)
            {
                int.TryParse(Int.Value(), out devcounter);
            }

            var Networkdevvorlage = File.ReadAllText(Path.Combine(tempaltePath, @"NetworkDev.template"));
            var Interfacenetworkifvorlage = File.ReadAllText(Path.Combine(tempaltePath, @"InterfaceNetwokIf.template"));
            var Proxysetvorlage = File.ReadAllText(Path.Combine(tempaltePath, @"ProxySet.template"));
            var Proxyipvorlage = File.ReadAllText(Path.Combine(tempaltePath, @"ProxyIp.template"));
            using (StreamWriter writer = new StreamWriter(OutputPath))
            {
                writer.WriteLine("configure network");
                for (int i = 0; i < netcounter; i++)
                {
                    writer.WriteLine(Networkdevvorlage);
                    if (i == netcounter)
                    {
                        writer.WriteLine(@" exit");
                    }
                }
                for (int i = 0; i < devcounter; i++)
                {
                    writer.WriteLine(Interfacenetworkifvorlage);
                    if (i == devcounter)
                    {
                        writer.WriteLine(@" exit");
                    }
                }
                writer.WriteLine("exit");
                writer.WriteLine("configure voip");
                for (int i = 0; i < setcounter; i++)
                {
                    writer.WriteLine(Proxysetvorlage);
                    if (i == setcounter)
                    {
                        writer.WriteLine(@" exit");
                    }
                }
                for (int i = 0; i < ipcounter; i++)
                {
                    writer.WriteLine(Proxyipvorlage);
                    if (i == ipcounter)
                    {
                        writer.WriteLine(@" exit");
                    }
                }
                writer.WriteLine("exit");
            }

        }
    }
}
