using System;
using System.Collections.Generic;
using System.Text;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;

namespace secondtry
{
    public class Commands
    {
        public static Command Tool()
        {
            var tool = new Command("tool");
            
            tool.AddCommand(execute());
            return tool;
           
            Command execute() =>
                new Command("execute", new Option(userpath()), 
                handler: CommandHandler.Create<string>(ToolAction.userpath));
            Option userpath() => new Option(alias: "--path", description: "set a userpath", argument: new Argument<string>());
        }
       
    }
    public static class ToolAction
    {
        public static void userpath(string path) =>
            Execute.run(path);
        
    }
}
