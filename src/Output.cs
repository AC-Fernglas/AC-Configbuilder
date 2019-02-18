using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace ACConfigBuilder
{
    public class Output
    {
        public dynamic writeOutput(List<string> AC, dynamic s)
        {
            StreamWriter writer = new StreamWriter(s);
            using (writer)
            {
                foreach (var item in AC)
                {
                    writer.WriteLine(item);
                    writer.Flush();
                }
            }
            return s;
        }
        public void  writeOutput(List<string> AC, string path)
        {
            writeOutput(AC, path);
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

    }
}
