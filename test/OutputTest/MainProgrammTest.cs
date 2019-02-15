using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using ACConfigBuilder;
using Sprache;

namespace Tests
{
  public class MainProgrammtest
    {
        [Fact]
        public void ReturnTestRealNameListID()
        {
            string TestName = "network-dev";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("listid", TestName);
        }
        [Fact]
        public void ReturnTestRealNameIP()
        {
            string TestName = "proxy-ip";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("ip", TestName);
        }
        [Fact]
        public void ReturnTestRealNameActivateVlanID()
        {
            string TestName = "activate";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("activate", TestName);
        }
        [Fact]
        public void ReturnTestRealNameVlanID()
        {
            string TestName = "vlan-id";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("vlan", TestName);
        }
        [Fact]
        public void ReturnTestRealNameUnderlyingIf()
        {
            string TestName = "underlying-if";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("underlyingif", TestName);
        }
        [Fact]
        public void ReturnTestRealNameUnderlyingDev()
        {
            string TestName = "underlying-dev";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("underlyingdev", TestName);
        }
        [Fact]
        public void ReturnTestRealNameName()
        {
            string TestName = "name";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("Name", TestName);
        }
        [Fact]
        public void ReturnTestRealNameTagging()
        {
            string TestName = "tagging";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("tag", TestName);
        }
        [Fact]
        public void ReturnTestRealNameApptype()
        {
            string TestName = "application-type";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("apptype", TestName);
        }
        [Fact]
        public void ReturnTestRealNameIPAdress()
        {
            string TestName = "ip-address";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("ipaddress", TestName);
        }
        [Fact]
        public void ReturnTestRealNameGateway()
        {
            string TestName = "gateway";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("gateway", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyName()
        {
            string TestName = "proxy-name";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("proxyname", TestName);
        }
        [Fact]
        public void ReturnTestRealNameSrdName()
        {
            string TestName = "srd-name";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("srdname", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyEnablekeepAlive()
        {
            string TestName = "proxy-enable-keep-alive";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("proxyenablekeepalive", TestName);
        }
        [Fact]
        public void ReturnTestRealNameSPCIV4()
        {
            string TestName = "sbcipv4-sip-int-name";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("sbcipv4sipintname", TestName);
        }
        [Fact]
        public void ReturnTestRealNameKeepAliveFailResp()
        {
            string TestName = "keepalive-fail-resp";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("keepalivefailresp", TestName);
        }
        [Fact]
        public void ReturnTestRealNameSuccessRetries()
        {
            string TestName = "success-detect-retries";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("successdetectretries", TestName);
        }
        [Fact]
        public void ReturnTestRealNameSuccessint()
        {
            string TestName = "success-detect-int";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("successdetectint", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyRedunadacyMode()
        {
            string TestName = "proxy-redundancy-mode";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("proxyredundancymode", TestName);
        }
        [Fact]
        public void ReturnTestRealNameHotSwap()
        {
            string TestName = "is-proxy-hot-swap";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("isproxyhotswap", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyBalancingMethode()
        {
            string TestName = "proxy-load-balancing-method";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("proxyloadbalancingmethod", TestName);
        }
        [Fact]
        public void ReturnTestRealNameMinActive()
        {
            string TestName = "min-active-serv-lb";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("minactiveservlb", TestName);
        }
        [Fact]
        public void ReturnTestRealNameProxyAddress()
        {
            string TestName = "proxy-address";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("proxyaddress", TestName);
        }
        [Fact]
        public void ReturnTestRealNameTransportType()
        {
            string TestName = "transport-type";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Contains("transporttype", TestName);
        }
        [Fact]
        public void ReturnTestRealNameDefault()
        {
            string TestName = "blub";
            TestName = new TestMainProgramm().ReturnTestName(TestName);
            Assert.Null(TestName);
        }
        [Fact]
        public void TestValidPathConfigPath()
        {
            string Testpath = @"C:\";
            string OtherPath = null;
            Testpath = new TestMainProgramm().ValidTestPath(OtherPath, Testpath);
            Assert.Contains(@"C:\",Testpath);
        }
        [Fact]
        public void TestValidPathUserPath()
        {
            string Testpath = @"C:\";
            string OtherPath = @"C:\";
            Testpath = new TestMainProgramm().ValidTestPath(OtherPath, Testpath);
            Assert.Contains(@"C:\", Testpath);
        }
    }
    public class TestMainProgramm : Execute
    {
        public string ReturnTestName(string TestName)
        {
            return returnRealName(TestName);
        }
        public string ValidTestPath(string OtherPath,string Path)
        {
            return validpath(Path,OtherPath);
        }
    }
}
