﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using ACConfigBuilder;
using Sprache;
using System.IO;
using Moq;
using McMaster.Extensions.CommandLineUtils;

namespace Tests
{
    public class MainProgrammtest
    {
        [Fact]
        public void ReturnTestRealNameListID()
        {
            string TestName = "network-dev";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("listid", TestName);
        }
        [Fact]
        public void ReturnTestRealNameIP()
        {
            string TestName = "proxy-ip";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("ip", TestName);
        }
        [Fact]
        public void ReturnTestRealNameActivateVlanID()
        {
            string TestName = "activate";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("activate", TestName);
        }
        [Fact]
        public void ReturnTestRealNameVlanID()
        {
            string TestName = "vlan-id";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("vlan", TestName);
        }
        [Fact]
        public void ReturnTestRealNameUnderlyingIf()
        {
            string TestName = "underlying-if";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("underlyingif", TestName);
        }
        [Fact]
        public void ReturnTestRealNameUnderlyingDev()
        {
            string TestName = "underlying-dev";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("underlyingdev", TestName);
        }
        [Fact]
        public void ReturnTestRealNameName()
        {
            string TestName = "name";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("Name", TestName);
        }
        [Fact]
        public void ReturnTestRealNameTagging()
        {
            string TestName = "tagging";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("tag", TestName);
        }
        [Fact]
        public void ReturnTestRealNameApptype()
        {
            string TestName = "application-type";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("apptype", TestName);
        }
        [Fact]
        public void ReturnTestRealNameIPAdress()
        {
            string TestName = "ip-address";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("ipaddress", TestName);
        }
        [Fact]
        public void ReturnTestRealNameGateway()
        {
            string TestName = "gateway";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("gateway", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyName()
        {
            string TestName = "proxy-name";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("proxyname", TestName);
        }
        [Fact]
        public void ReturnTestRealNameSrdName()
        {
            string TestName = "srd-name";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("srdname", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyEnablekeepAlive()
        {
            string TestName = "proxy-enable-keep-alive";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("proxyenablekeepalive", TestName);
        }
        [Fact]
        public void ReturnTestRealNameSPCIV4()
        {
            string TestName = "sbcipv4-sip-int-name";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("sbcipv4sipintname", TestName);
        }
        [Fact]
        public void ReturnTestRealNameKeepAliveFailResp()
        {
            string TestName = "keepalive-fail-resp";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("keepalivefailresp", TestName);
        }
        [Fact]
        public void ReturnTestRealNameSuccessRetries()
        {
            string TestName = "success-detect-retries";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("successdetectretries", TestName);
        }
        [Fact]
        public void ReturnTestRealNameSuccessint()
        {
            string TestName = "success-detect-int";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("successdetectint", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyRedunadacyMode()
        {
            string TestName = "proxy-redundancy-mode";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("proxyredundancymode", TestName);
        }
        [Fact]
        public void ReturnTestRealNameHotSwap()
        {
            string TestName = "is-proxy-hot-swap";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("isproxyhotswap", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyBalancingMethode()
        {
            string TestName = "proxy-load-balancing-method";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("proxyloadbalancingmethod", TestName);
        }
        [Fact]
        public void ReturnTestRealNameMinActive()
        {
            string TestName = "min-active-serv-lb";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("minactiveservlb", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyAddress()
        {
            string TestName = "proxy-address";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("proxyaddress", TestName);
        }
        [Fact]
        public void ReturnTestRealNameTransportType()
        {
            string TestName = "transport-type";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Contains("transporttype", TestName);
        }
        [Fact]
        public void ReturnTestRealNameDefault()
        {
            string TestName = "blub";
            TestName = new Input2Object().ReturnTestName(TestName);
            Assert.Null(TestName);
        }
        [Fact]
        public void findTestDirectory()
        {
            var TestDirectory = new TestArrangement().GetTestDefaultPath(new CommandOption("--blub",CommandOptionType.SingleValue) ,
                new CommandOption("--blib", CommandOptionType.SingleValue), 
                new CommandOption("--bleb", CommandOptionType.SingleValue));
            List<string> Testlist = new TestArrangement().FindTestDirectory(new TestArrangement().ProofTestDirectory(TestDirectory.Path));
            Assert.NotNull(Testlist);
            Assert.Empty(Testlist);
        }
        [Fact]
        public void TestParseInObjectGetOpening()
        {
            List<string> FakeConfig = new List<string>()
            {
                "configure network",
                " network-dev 312",
                "  name blub",
                "  tagging untagged",
                "  activate",
                " exit",
                " interface network-if 21",
                "  name blab",
                "  activate",
                " exit",
                "exit",
                "configure voip",
                " proxy-set 21",
                "  proxy-name bleb",
                "  activate",
                " exit",
                " proxy-ip 21/32",
                "  proxy-address 1.2.4.1:blib",
                "  activate",
                " exit",
                "exit"
            };

            ACConfig TestAC = new Input2Object().ParseInTestObject(new StreamReader(GenerateStreamFromList(FakeConfig)));
            Assert.True(TestAC.configureNetwork.networkdev[0].tag.ToString() == "untagged");
            Assert.True(TestAC.configureNetwork.networkdev[0].activate == true);
            Assert.True(TestAC.configureNetwork.interfacenetworkif[0].Name == "blab");
            Assert.True(TestAC.configureviop.proxyset[0].activate == true);
            Assert.True(TestAC.configureviop.proxyip[0].proxyaddress == "1.2.4.1:blib");
        }
        public static Stream GenerateStreamFromList(List<string> FakeConfig)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            foreach (var item in FakeConfig)
            {
                writer.WriteLine(item);
                writer.Flush();
            }
            stream.Position = 0;
            return stream;
        }

        [Fact]
        public void TestReplaceItemNetworkdev()
        {
            ACConfig TestAc = new ACConfig()
            {
                configureNetwork = new ConfigureNetwork()
                {
                    networkdev = new List<Networkdev>()
                }
            };
            Networkdev Nd = new Networkdev()
            {
                listid = 21,
                Name = "blub_-"
            };
            TestAc.configureNetwork.networkdev.Add(Nd);
            List<Networkdev> list = new List<Networkdev>();
            Networkdev OtherNd = new Networkdev()
            {
                listid = 21,
                Name = "blup_-"
            };
            list.Add(OtherNd);
            var what = "networkdev";
            CommandOption customer = new CommandOption("--blub", CommandOptionType.SingleValue);
            customer.Values.Add("Klaus");
            TestAc = new TestMainProgramm().replaceitem(TestAc, list, what, new CommandOption("--blub", CommandOptionType.SingleValue), customer);
            Assert.True(TestAc.configureNetwork.networkdev[0].Name == "blup_Klaus-");
        }
        [Fact]
        public void TestReplaceItemInterfaceNetworkIf()
        {
            ACConfig TestAc = new ACConfig()
            {
                configureNetwork = new ConfigureNetwork()
                {
                   interfacenetworkif = new List<Interfacenetworkif>()

                }
            };
            Interfacenetworkif INI = new Interfacenetworkif()
            {
                listid = 21,
                Name = "blub_-"
            };
            TestAc.configureNetwork.interfacenetworkif.Add(INI);
            List<Interfacenetworkif> list = new List<Interfacenetworkif>();
            Interfacenetworkif OtherINI = new Interfacenetworkif()
            {
                listid = 21,
                Name = "blup_-"
            };
            list.Add(OtherINI);
            var what = "interfacenetworkif";
            CommandOption country = new CommandOption("--blub", CommandOptionType.SingleValue);
            country.Values.Add("de");
            TestAc = new TestMainProgramm().replaceitem(TestAc, list, what, country, new CommandOption("--blub", CommandOptionType.SingleValue));
            Assert.True(TestAc.configureNetwork.interfacenetworkif[0].Name == "blup_-de");
        }
        [Fact]
        public void TestReplaceItemProxySet()
        {
            ACConfig TestAc = new ACConfig()
            {
                configureviop = new Configureviop()
                {
                    proxyset = new List<Proxyset>()
                }
            };
            Proxyset PS = new Proxyset()
            {
                listid = 21,
                proxyname = "blub_-"
            };
            TestAc.configureviop.proxyset.Add(PS);
            List<Proxyset> list = new List<Proxyset>();
            Proxyset OtherPS = new Proxyset()
            {
                listid = 21,
                proxyname = "blup_-"
            };
            list.Add(OtherPS);
            var what = "proxyset";
            TestAc = new TestMainProgramm().replaceitem(TestAc, list, what, new CommandOption("--blub", CommandOptionType.SingleValue), new CommandOption("--blub", CommandOptionType.SingleValue));
            Assert.True(TestAc.configureviop.proxyset[0].proxyname == "blup_-");
        }
        [Fact]
        public void TestReplaceItemProxyIp()
        {
            ACConfig TestAc = new ACConfig()
            {
                configureviop = new Configureviop()
                {
                    proxyip = new List<Proxyip>()
                }
            };
            Proxyip PIP = new Proxyip()
            {
                ip = "21/32",
                proxyaddress = "blub_-"
            };
            TestAc.configureviop.proxyip.Add(PIP);
            List<Proxyip> list = new List<Proxyip>();
            Proxyip OtherPIP = new Proxyip()
            {
                ip = "21/32",
                proxyaddress = "blup_-"
            };
            list.Add(OtherPIP);
            var what = "proxyip";
            TestAc = new TestMainProgramm().replaceitem(TestAc, list, what, new CommandOption("--blub", CommandOptionType.SingleValue),new CommandOption("--blub", CommandOptionType.SingleValue));
            Assert.True(TestAc.configureviop.proxyip[0].proxyaddress == "blup_-");
        }
        [Fact]
        public void TestReplaceItemNullList()
        {
            ACConfig Ac = new ACConfig()
            {
                configureNetwork = new ConfigureNetwork()
                {
                    networkdev = new List<Networkdev>()
                }
            };
            Networkdev Nd = new Networkdev()
            {
                listid = 21,
                Name = "blub"
            };
            Ac.configureNetwork.networkdev.Add(Nd);
            List<string> list = new List<string>(); 
            var what = "networkdev";
            var TestAc = new TestMainProgramm().ReplaceTestValues(Ac, list, what, new CommandOption("--blub", CommandOptionType.SingleValue), new CommandOption("--blub", CommandOptionType.SingleValue));
            Assert.Same(Ac,TestAc);
        }
        [Fact]
        public void IdelTest()
        {
           int? Empty =  new TestCommands().TestIdel(null);
            Assert.Equal(0,Empty);
        }
        [Fact]
        public void TestLoadSystemConfig()
        {
            var Config = new TestArrangement().TestLoadConfig(Path.Combine(new TestArrangement().GetTestDefaultPath(new CommandOption("--blub", CommandOptionType.SingleValue),
                new CommandOption("--blib", CommandOptionType.SingleValue),
                new CommandOption("--bleb", CommandOptionType.SingleValue)).configPath, @"Config.json"));
            Assert.NotNull(Config);
        }
    }
    public class Input2Object : InputToACObject
    {
        public string ReturnTestName(string TestName)
        {
            return returnRealName(TestName);
        }
        public ACConfig ParseInTestObject(StreamReader Reader)
        {
            return parseinobject(Reader);
        }
    }
    public class TestMainProgramm : Execute
        {
            public ACConfig ReplaceTestValues(ACConfig AC, dynamic list, string whatlist, CommandOption country, CommandOption customer)
            {
                return replaceitem(AC, list, whatlist, country, customer);
            }
        }
    public class TestArrangement : Arrangement
    {
        public List<string> FindTestDirectory(string path)
        {
            return findFilesInDirectory(path);
        }
        public ACConfig TestLoadConfig(string path)
        {
            return LoadSystemConfig(path);
        }
        public string ProofTestDirectory(string path)
        {
            return fileproof( Directory.GetCurrentDirectory() ,path);
        }
        public (string Path, string configPath, string templatePath) GetTestDefaultPath(CommandOption Path, CommandOption configPath, CommandOption templatePath)
        {
            return getDefaultPaths(Path,configPath,templatePath);
        }
    }
    public class TestCommands : Commands
    {
        public int TestIdel(string[] commands)
        {
            return Idel(commands);
        }
    }

}
