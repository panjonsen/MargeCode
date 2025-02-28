# MargeCode Program Introduction / MargeCode 程序介绍
![image](https://github.com/user-attachments/assets/0aa2acad-cae8-4765-9536-8b6847be740b)

## Overview / 概述

### English
The `MargeCode` program is a C# utility designed to merge multiple source code files from a specified directory into a single output file. It supports customizable configurations via a `config.json` file, allowing users to specify which file extensions to include, which folders and extensions to exclude, and the name of the output file. The program is particularly useful for consolidating code for review, archiving, or sharing purposes.

### 中文
`MargeCode` 程序是一个用 C# 编写的实用工具，旨在将指定目录中的多个源代码文件合并到一个输出文件中。它通过 `config.json` 文件支持可自定义的配置，用户可以指定要包含的文件扩展名、要排除的文件夹和扩展名，以及输出文件的名称。该程序特别适用于整合代码以供审查、存档或分享。

---

## Features / 功能

### English
- **File Merging**: Combines all specified source files into a single text file.
- **Configurable Filters**: Uses `config.json` to define included extensions (e.g., `.cs`), excluded folders (e.g., `bin`, `obj`), and excluded extensions (e.g., `.exe`, `.dll`).
- **Dynamic Exclusion**: Automatically excludes the folder where the program is located to avoid self-referential issues.
- **Organized Output**: Groups files by extension in the output file with clear separators for readability.
- **Error Handling**: Provides feedback on errors, such as missing configuration files or processing issues.

### 中文
- **文件合并**：将所有指定的源文件合并到一个文本文件中。
- **可配置过滤器**：使用 `config.json` 定义要包含的扩展名（例如 `.cs`）、要排除的文件夹（例如 `bin`、`obj`）和要排除的扩展名（例如 `.exe`、`.dll`）。
- **动态排除**：自动排除程序所在文件夹，避免自引用问题。
- **有序输出**：按扩展名分组文件，在输出文件中使用清晰的分隔符以提高可读性。
- **错误处理**：提供关于错误的反馈，例如缺少配置文件或处理问题。

---

## How to Use / 使用方法

### English

1. **Prepare the Environment**:
   - Ensure you have the .NET runtime installed to run the compiled C# program.
   - Place the compiled `MargeCode.exe` in a directory above the files you want to merge (e.g., parent directory of your project).

2. **Configure the Program**:
   - Create a `config.json` file in the same directory as `MargeCode.exe` (optional, defaults will be used if not provided).
   - Example `config.json`:
     ```json
     {
       "includeExtensions": [".cs"],
       "excludeFolders": ["node_modules", "bin", "obj", ".idea", ".git", "UploadFile", ".config", ".vs"],
       "excludeExtensions": [".exe", ".png", ".dll", ".docx", ".docxf", ".pptx", ".xlsx", ".json", ".config", ".csproj", ".http", ".sln", ".user"],
       "outputFile": "all_code.txt"
     }
