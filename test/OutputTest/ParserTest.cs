using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using ACConfigBuilder;
using Sprache;

namespace Tests
{
  public class ParserTest
    {
        [Fact]
        public void TestParsergettingNameandValueRight()
        {
            string TestString = " network-dev 123";
            string subidentTest = String.Empty;
            string subValue = String.Empty;
            bool cExit, subExit = false;
            new TestParserGrammar().getTestIdentNameAndValue(TestString, out cExit, out subExit, out subidentTest, out subValue);
            Assert.Contains("network-dev", subidentTest);
            Assert.Contains("123",subValue);
            
        }
        [Fact]
        public void TestParsercatchExit()
        {
            string TestString = " exit";
            string subidentTest = String.Empty;
            string subValue = String.Empty;
            bool cExit, subExit = false;
            new TestParserGrammar().getTestIdentNameAndValue(TestString, out cExit, out subExit, out subidentTest, out subValue);
            Assert.Contains("exit", subidentTest);
            Assert.True(cExit);
            Assert.True(subExit);
        }
        [Fact]
        public void TestParsergettingConfigureIdent()
        {
            string line = "configure network";
            bool configureExit = false;
            string ident = String.Empty;
            new TestParserGrammar().getTestConfigureIdent(line, out configureExit, out ident);
            Assert.Contains("configure network", ident);
            Assert.False(configureExit);
        }

    }
    public class TestParserGrammar : Input2Object
    {
        public void getTestIdentNameAndValue(string Test, out bool cExit,out  bool subexit,out string subidentTest, out string subValue)
        {
           getIdentNameAndValue(Test,out cExit,out subexit,out subidentTest,out subValue );
        }
        public void getTestConfigureIdent(string line, out bool configureExit, out string ident)
        {
            getConfigureIdent(line, out configureExit, out ident);
        }
    }
}
