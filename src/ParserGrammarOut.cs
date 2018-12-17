using System;
using System.Collections.Generic;
using System.Text;

namespace secondtry
{
    public class ParserGrammarOut
    {
        public void getobject(ACConfig AC)
        {
            foreach (var propertyInfo in AC.configureNetwork)
            {
                foreach (var prop in propertyInfo.GetType().GetValue())
                {
                    var value = propertyInfo.GetValue(AC);
                    Console.WriteLine(value);
                }
                
            }
        }
    }
}
