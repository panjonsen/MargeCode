# MargeCode 程序介绍

## 概述

`MargeCode` 是一个用 C# 编写的文件合并工具，旨在将指定目录及其子目录中的特定类型的代码文件合并到一个输出文件中。它支持通过 JSON 配置文件自定义文件类型、排除文件夹和输出文件名，非常适合整理项目代码、生成代码快照或进行代码审查。

## 功能

1. **文件筛选**：
   - 根据文件扩展名（如 `.cs`）筛选需要合并的文件。
   - 支持排除特定文件夹（如 `node_modules`、`bin`）和文件类型（如 `.exe`、`.dll`）。

2. **动态排除**：
   - 自动排除程序所在目录，避免将自身或其他无关文件纳入合并。

3. **输出格式**：
   - 将文件按扩展名分组，每组文件以分隔符标记。
   - 每个文件内容前添加完整路径作为标题，便于区分。

4. **配置文件**：
   - 通过 `config.json` 文件自定义设置，支持灵活调整合并规则。

5. **错误处理**：
   - 提供异常捕获和错误提示，确保程序运行稳定。

## 使用方法

### 1. 准备工作

- **环境要求**：需要安装 .NET 运行时（如 .NET Core 或 .NET Framework）。
- **源代码**：将 `Program.cs` 编译为可执行文件（例如 `MargeCode.exe`）。
- **配置文件**：在程序目录下创建 `config.json`（若无则使用默认设置）。

### 2. 配置 `config.json`

以下是一个示例配置文件：

```json
{
  "includeExtensions": [".cs"],
  "excludeFolders": ["node_modules", "bin", "obj", ".idea", ".git", "UploadFile", ".config", ".vs"],
  "excludeExtensions": [".exe", ".png", ".dll", ".docx", ".docxf", ".pptx", ".xlsx", ".json", ".config", ".csproj", ".http", ".sln", ".user"],
  "outputFile": "all_code.txt"
}
