using ACConfigBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Tests
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
            testreturnConfigList = new TestOutput().TestObjectToList(AC);
            Assert.NotEmpty(testreturnConfigList);
        }

        [Fact]
        public void TestWriteOutputFromList()
        {
            List<string> AC = new List<string>()
            {
                "Line1",
                "Line 2 with whitespaces   ",
                "This is Line 3 as sentence"
            };
          MemoryStream s = new MemoryStream();
          s =  new TestOutput().WriteTestOutput(AC, s);
          Assert.NotEmpty(s.ToArray());
          Assert.NotNull(s);
          byte[] bytes = s.ToArray();
            using (var stream = new MemoryStream(bytes))
            using (var reader = new StreamReader(stream))
            {
                var collection = new List<string>();
                string input;

                while ((input = reader.ReadLine()) != null)
                    collection.Add(input);

                Assert.Equal(3, collection.Count);
            }
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
        public string WriteTestOutput(List<string> AC)
         { 
            var output = "";
            using(var stream = new MemoryStream()){
               writeOutput(AC, stream);
               // reset stream position to 0. Because the writer may leave the stream at the end.
               stream.Position = 0;
               output = StreamReader(stream).ReadToEnd();
            }
            return output;
         }
        {
            return writeOutput(AC, s);
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
