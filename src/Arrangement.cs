using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ACConfigBuilder
{
    public class Arrangement
    {
        protected void setuserpath(string configPath, string changePath)
        {
            string[] file = File.ReadAllLines(Path.Combine(configPath, "Config.json"));
            List<string> list = new List<string>(file);
            list[1] = "\"outputDirectory\": \"" + @Path.GetFullPath(changePath) + "\",";
            using (StreamWriter writer = new StreamWriter(Path.Combine(configPath, "Config.json")))
            {
                foreach (var item in list)
                {
                    writer.Write(item + Environment.NewLine);
                    writer.Flush();
                }
            }
        }
        /// <summary>
        ///  Checks if the Directory exists, if not it creates the Directory and change.json 
        /// </summary>
        public string fileproof(string CurrentPath ,string outputDirectory)
        {
            var exist = Directory.Exists(outputDirectory);
            if (!exist)
            {
                exist = Directory.Exists(Path.Combine(CurrentPath, outputDirectory));
            }
            if (exist == false)
            {
                var path = Path.Combine(CurrentPath, outputDirectory);
                Directory.CreateDirectory(path);
                File.Create(path + "\\change.json").Dispose();
                return path;
            }
            if (Directory.Exists(outputDirectory))
            {
                if (!Directory.Exists(Path.Combine(CurrentPath, outputDirectory, "\\change.json")))
                {
                    File.Create(outputDirectory + "\\change.json").Dispose();
                }
                return outputDirectory;
            }
            else
            {
                return Path.Combine(CurrentPath, outputDirectory);
            }
        }
        /// <summary>
        /// opens the .txt files in the directorypath,
        /// only the top not sup directorys
        /// </summary>
        protected List<string> findFilesInDirectory(string mypath)
        {
            string[] dirs = Directory.GetFiles(mypath, "*.txt", SearchOption.TopDirectoryOnly);
            return dirs.ToList<string>();
        }
        public string GetToolPath()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path).ToString();
            return path;
        }
        public (string path, string configPath, string tempaltePath) getDefaultPaths(
            CommandOption Path,
            CommandOption configpath,
            CommandOption templatePath)
        {
            var toolPath = GetToolPath();
            var configPath = configpath.HasValue() ? configpath.Value() : System.IO.Path.Combine(toolPath, "config");
            if (Path.HasValue())
            {
                setuserpath(configPath, Path.Value().Replace(@"\", @"\\"));
            }
            var path = Path.HasValue() ? Path.Value().Replace(@"\", @"\\") : Directory.GetCurrentDirectory();
            var tempaltePath = templatePath.HasValue() ? templatePath.Value() : System.IO.Path.Combine(toolPath, "config", "Template");
            return (
               path,
               configPath,
               tempaltePath
                );
        }
        /// <summary>
        /// Loads the Config, which is the base for the ACConfig
        /// </summary>
        /// <param name="ConfigLodingPath">Path to the config that forms the basis of the program </param>
        public ACConfig LoadSystemConfig(string ConfigLodingPath)
        {
            var config = File.ReadAllText(ConfigLodingPath);
            config = config.Replace(@"\", "/");//get json
            return JsonConvert.DeserializeObject<ACConfig>(config);
        }
        public void prepareForStart(CommandOption Path, CommandOption configPath, CommandOption templatePath, out ACConfig ConfigWithChanges, out List<string> dirs)
        {
            var paths = getDefaultPaths(Path, configPath, templatePath);
            var configuration = LoadSystemConfig(System.IO.Path.GetFullPath(System.IO.Path.Combine(paths.configPath, "Config.json")));
            var outputPath = fileproof(paths.path ,configuration.outputDirectory);
            var changePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(fileproof(paths.path, configuration.outputDirectory), "change.json"));
            ConfigWithChanges = JsonConvert.DeserializeObject<ACConfig>(File.ReadAllText(changePath));//open json to use
            dirs = findFilesInDirectory(outputPath);
        }
    }
}
