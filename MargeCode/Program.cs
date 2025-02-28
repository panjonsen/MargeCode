namespace MargeCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

class Program
{
    class Config
    {
        public string[] IncludeExtensions { get; set; } = Array.Empty<string>();
        public string[] ExcludeFolders { get; set; } = Array.Empty<string>();
        public string[] ExcludeExtensions { get; set; } = Array.Empty<string>();
        public string OutputFile { get; set; } = "all_code.txt";
    }

    private const string ConfigFileName = "margeConfig.json";

    static void Main(string[] args)
    {
        try
        {
            // 读取配置
            Config config = LoadConfig();

            // 获取程序自身路径和目录信息
            string currentExePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string currentDir = Path.GetDirectoryName(currentExePath) ?? Directory.GetCurrentDirectory();
            string currentExeFullName = Path.GetFullPath(currentExePath);
            
            // 使用当前目录作为合并根目录
            Console.WriteLine($"Merging files from: {currentDir}");
            Console.WriteLine($"Excluding program file: {currentExeFullName}");

            // 获取并合并文件
            MergeFiles(currentDir, config, currentExeFullName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    private static Config LoadConfig()
    {
        if (!File.Exists(ConfigFileName))
        {
            Console.WriteLine($"Config file '{ConfigFileName}' not found. Using default settings.");
            return new Config();
        }

        string jsonContent = File.ReadAllText(ConfigFileName);
        return JsonConvert.DeserializeObject<Config>(jsonContent) ?? new Config();
    }

    private static void MergeFiles(string directory, Config config, string currentExeFullName)
    {
        var files = GetFiles(directory, config)
            .Where(file => file.FullName != currentExeFullName);

        if (!files.Any())
        {
            Console.WriteLine("No files found matching the criteria.");
            return;
        }

        var filesByExtension = files
            .GroupBy(file => file.Extension.ToLowerInvariant())
            .OrderBy(g => g.Key);

        int totalFiles = 0;
        using (var writer = new StreamWriter(config.OutputFile, false, Encoding.UTF8))
        {
            foreach (var group in filesByExtension)
            {
                string ext = group.Key;
                string separator = $"============={ext}===============";

                Console.WriteLine(separator);
                writer.WriteLine(separator);

                foreach (var file in group)
                {
                    Console.WriteLine($"Merging: {file.FullName}");
                    writer.WriteLine($"===== {file.FullName} =====");
                    writer.WriteLine(File.ReadAllText(file.FullName));
                    writer.WriteLine();
                    totalFiles++;
                }

                Console.WriteLine();
                writer.WriteLine();
            }
        }

        Console.WriteLine($"Done! Merged {totalFiles} files into {config.OutputFile}");
    }

    private static IEnumerable<FileInfo> GetFiles(string directory, Config config)
    {
        return new DirectoryInfo(directory).EnumerateFiles("*", SearchOption.AllDirectories)
            .Where(file =>
                config.IncludeExtensions.Any(ext => ext.Equals(file.Extension, StringComparison.OrdinalIgnoreCase)) &&
                !config.ExcludeExtensions.Any(ext => ext.Equals(file.Extension, StringComparison.OrdinalIgnoreCase)) &&
                !config.ExcludeFolders.Any(excluded => file.FullName.IndexOf($@"\{excluded}\", StringComparison.OrdinalIgnoreCase) >= 0)
            );
    }
}