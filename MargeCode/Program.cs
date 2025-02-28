namespace MargeCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

class Program
{
    // 配置类，用于映射 JSON
    class Config
    {
        public string[] IncludeExtensions { get; set; } = Array.Empty<string>();
        public string[] ExcludeFolders { get; set; } = Array.Empty<string>();
        public string[] ExcludeExtensions { get; set; } = Array.Empty<string>();
        public string OutputFile { get; set; } = "all_code.txt";
    }

    static void Main(string[] args)
    {
        try
        {
            // 读取配置
            Config config;
            string configPath = "config.json";
            if (!File.Exists(configPath))
            {
                Console.WriteLine($"Config file '{configPath}' not found. Using default settings.");
                config = new Config();
            }
            else
            {
                string jsonContent = File.ReadAllText(configPath);
                config = JsonConvert.DeserializeObject<Config>(jsonContent) ?? new Config();
            }

            // 获取程序所在目录名，加入排除列表
            string currentDirName = Path.GetFileName(Directory.GetCurrentDirectory());
            var excludeFolders = config.ExcludeFolders.ToList();
            excludeFolders.Add(currentDirName); // 动态排除程序所在文件夹

            // 获取父目录作为合并根目录
            string rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName ?? Directory.GetCurrentDirectory();
            Console.WriteLine($"Merging files from: {rootDirectory}");

            // 更新 config 的 ExcludeFolders
            config.ExcludeFolders = excludeFolders.ToArray();

            // 获取所有符合条件的文件
            var files = GetFiles(rootDirectory, config);

            // 如果没有文件，直接退出
            if (!files.Any())
            {
                Console.WriteLine("No files found matching the criteria.");
                return;
            }

            // 按后缀名分组
            var filesByExtension = files
                .GroupBy(file => file.Extension.ToLowerInvariant())
                .OrderBy(g => g.Key);

            // 合并文件并优化打印
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
                        string content = File.ReadAllText(file.FullName);
                        writer.WriteLine(content);
                        writer.WriteLine();

                        totalFiles++;
                    }

                    Console.WriteLine();
                    writer.WriteLine();
                }
            }

            Console.WriteLine($"Done! Merged {totalFiles} files into {config.OutputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static IEnumerable<FileInfo> GetFiles(string directory, Config config)
    {
        var dirInfo = new DirectoryInfo(directory);

        return dirInfo.EnumerateFiles("*", SearchOption.AllDirectories)
            .Where(file =>
                config.IncludeExtensions.Any(ext => ext.Equals(file.Extension, StringComparison.OrdinalIgnoreCase)) &&
                !config.ExcludeExtensions.Any(ext => ext.Equals(file.Extension, StringComparison.OrdinalIgnoreCase)) &&
                !config.ExcludeFolders.Any(excluded => file.FullName.IndexOf($@"\{excluded}\", StringComparison.OrdinalIgnoreCase) >= 0)
            );
    }
}