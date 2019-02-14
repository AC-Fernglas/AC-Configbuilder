using ACConfigBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace XUnitTestProject1
{
    public class OutputTests
    {
        private IEnumerable<string> testConfigOutput;
        private List<string> testreturnConfigList;
        private ACConfig AC = new ACConfig();
        [Fact]
        public void GetConfigStringFromObjectRespectPropertyName()
        {

            testConfigOutput = new TestOutput().GetConfigStringFromTestObject(new TestConfig()
            {
                TestPropWithAttribute = "jgs"
            }).ToList();
            Assert.Single(testConfigOutput, d => d.Contains("2000"));
        }
        [Fact]
        public void GetConfigStringFromObjectWhereValueTrue()
        {

            testConfigOutput = new TestOutput().GetConfigStringFromTestObject(new TestConfig()
            {
                TestBoolValue = true
            }).ToList();
            Assert.Single(testConfigOutput, d => d.Contains("  activate"));
        }
        [Fact]
        public void GetConfigStringFromObjectWhereValueNull()
        {
            testConfigOutput = new TestOutput().GetConfigStringFromTestObject(new TestConfig()
            {
                TestnullValue = null
            }).ToList();
            Assert.Empty(testConfigOutput);
        }

        [Fact]
        public void GetConfigStringFromObjectUseObjectPropertyNameIfNocustomPropertyNameIsSet()
        {

            testConfigOutput = new TestOutput().GetConfigStringFromTestObject(new TestConfig()
            {
                TestPropWithOutAttribute = "fghjkl"
            }).ToList();
            Assert.Single(testConfigOutput, d => d.Contains("TestPropWithOutAttribute"));
        }

        [Fact]
        public void ObjectToListAllNull()
        {
            testreturnConfigList = new TestOutput().objectToList(new ACConfig());
            Assert.Empty(testreturnConfigList);
        }
        [Fact]
        public void ObjectToListNetworkDevNotNull()
        {
            AC = new ACConfig()
            {
                configureNetwork = new ConfigureNetwork()
                {
                    networkdev = new List<Networkdev>()
                }
            };
            Networkdev ND = new Networkdev()
            {
                Name = "TestName"
            };
            AC.configureNetwork.networkdev.Add(ND);
            testreturnConfigList = new TestOutput().objectToList(AC);
            Assert.NotEmpty(testreturnConfigList);
        }
        [Fact]
        public void ObjectToListInterfaceNetworkIfNotNull()
        {
            AC = new ACConfig()
            {
                configureNetwork = new ConfigureNetwork()
                {
                    interfacenetworkif = new List<Interfacenetworkif>()
                }
            };
            Interfacenetworkif INI = new Interfacenetworkif()
            {
                Name = "TestName"
            };
            AC.configureNetwork.interfacenetworkif.Add(INI);
            testreturnConfigList = new TestOutput().objectToList(AC);
            Assert.NotEmpty(testreturnConfigList);
        }
        [Fact]
        public void ObjectToListProxySetNotNull()
        {
            AC = new ACConfig()
            {
                configureviop = new Configureviop()
                {
                    proxyset = new List<Proxyset>()
                }
            };
            Proxyset PS = new Proxyset()
            {
                proxyname = "testProxyname"
            };
            AC.configureviop.proxyset.Add(PS);
            testreturnConfigList = new TestOutput().objectToList(AC);
            Assert.NotEmpty(testreturnConfigList);
        }
        [Fact]
        public void ObjectToListProxyIpNotNull()
        {
            AC = new ACConfig()
            {
                configureviop = new Configureviop()
                {
                    proxyip = new List<Proxyip>()
                }
            };

            Proxyip PI = new Proxyip()
            {
                ip = "213"
            };
            AC.configureviop.proxyip.Add(PI);
            testreturnConfigList = new TestOutput().objectToList(AC);
            Assert.NotEmpty(testreturnConfigList);
        }
        [Fact]
        public void TestOutut()
        {
            ACConfig AC = new ACConfig();

        }

    }

    public class TestOutput : Output
    {
        public IEnumerable<string> GetConfigStringFromTestObject(object input)
        {
            return GetConfigStringFromObject(input);
        }

        public List<string> TestObjectToList(ACConfig AC)
        {
            return objectToList(AC);
        }
    }

    public class TestConfig
    {
        [AcProperty(PropertyName = "2000")]
        public string TestPropWithAttribute { get; set; }
        public string TestPropWithOutAttribute { get; set; }
        public bool TestBoolValue { get; set; }
        public int? TestnullValue { get; set; }

    }
}
