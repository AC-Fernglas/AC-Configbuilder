using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACConfigBuilder
{
    public class Output
    {
        public void getobject(ACConfig AC, string path)
        {
            List<string> configback = new List<string>();
            if (AC.configureNetwork != null)
            {
                if (AC.configureNetwork.networkdev != null)
                {
                    configback.Add("configure network");
                    foreach (var item in AC.configureNetwork.networkdev)
                    {
                        foreach (var propertyInfo in item.GetType().GetProperties())
                        {
                            var value = propertyInfo.GetValue(item);
                            if (value != null)
                            {
                                if (value.ToString() == "True")
                                {
                                    configback.Add("  activate");
                                }
                                else
                                {
                                    configback.Add("  " + propertyInfo.Name + " " + value);
                                }
                            }
                        }
                        configback.Add(" exit");
                    }
                }
                if (AC.configureNetwork.interfacenetworkif != null)
                {
                    foreach (var item in AC.configureNetwork.interfacenetworkif)
                    {
                        foreach (var propertyInfo in item.GetType().GetProperties())
                        {
                            var value = propertyInfo.GetValue(item);
                            if (value != null)
                            {
                                if (value.ToString() == "True")
                                {
                                    configback.Add("  activate");
                                }
                                else
                                {
                                    configback.Add("  " + propertyInfo.Name + " " + value);
                                }
                            }
                        }
                        configback.Add(" exit");
                    }
                    configback.Add("exit");
                }
            }
            if (AC.configureNetwork != null)
            {

                if (AC.configureviop != null)
                {
                    configback.Add("configure voip");
                    if (AC.configureviop.proxyset != null)
                    {
                        foreach (var item in AC.configureviop.proxyset)
                        {
                            foreach (var propertyInfo in item.GetType().GetProperties())
                            {
                                var value = propertyInfo.GetValue(item);
                                if (value != null)
                                {
                                    if (value.ToString() == "True")
                                    {
                                        configback.Add("  activate");
                                    }
                                    else
                                    {
                                        configback.Add("  " + propertyInfo.Name + " " + value);
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
                            var value = propertyInfo.GetValue(item);
                            if (value != null)
                            {
                                if (value.ToString() == "True")
                                {
                                    configback.Add("  activate");
                                }
                                else
                                {
                                    configback.Add("  " + propertyInfo.Name + " " + value);
                                }
                            }
                        }
                        configback.Add(" exit");
                    }
                    configback.Add("exit");
                }
            }
            giveitback(configback, path);
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
