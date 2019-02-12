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
        public OutputTests()
        {
            testConfigOutput = new TestOutput().GetConfigStringFromTestObject(new TestConfig()
            {
                TestPropWithAttribute = "jgs",
                TestPropWithOutAttribute = "fghjkl"
            });
        }
        [Fact]
        public void GetConfigStringFromObjectRespectPropertyName()
        {
            Assert.Single(testConfigOutput, d => d.Contains("2000"));
        }

        [Fact]
        public void GetConfigStringFromObjectUseObjectPropertyNameIfNocustomPropertyNameIsSet()
        {
            Assert.Single(testConfigOutput, d => d.Contains("TestPropWithOutAttribute"));
        }
    }

    public class TestOutput : Output
    {
        public IEnumerable<string> GetConfigStringFromTestObject(object input)
        {
            return GetConfigStringFromObject(input);
        }
    }

    public class TestConfig
    {
        [AcProperty(PropertyName = "2000")]
        public string TestPropWithAttribute { get; set; }
        public string TestPropWithOutAttribute { get; set; }

    }
}
