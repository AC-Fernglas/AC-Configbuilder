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
        public void  startOutput(ACConfig AC, string path)
        {
            giveitback(objectToList(AC), path);
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
                if (value != null)
                {
                    isSetValue = true;
                    if (value.ToString() == "True")
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

        private void giveitback(List<string> back, string path)
        {
            using (StreamWriter writer = new StreamWriter(@path))
            {
                foreach (var item in back)
                {
                    writer.WriteLine(item);
                }
            }
        }
    }
}
