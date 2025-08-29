# YitongUnityTool

[![Language: Chinese](https://img.shields.io/badge/Language-Chinese-green)](Readme.md)
[![Language: English](https://img.shields.io/badge/Language-English-blue)](Readme.en.md)

A collection of Unity tools designed to improve development efficiency.

## ExcelConverter

An Excel file format conversion tool that supports converting Excel files to multiple common data formats for easy use in Unity projects.

### Supported Target File Formats

- JSON
- CSV
- XML

### Installation

In Unity Package Manager, select `Add package from git URL` and use the following path:

```
https://github.com/YiTongHu-SC/YitongUnityTool.git?path=Assets/Plugins/ExcelConverter
```

### Usage

1. In the Unity editor, select the `Tools/ExcelConverter` menu item to open the conversion tool window
2. Select target format: `JSON/CSV/XML`
3. Select output file encoding: `GB2312/UTF-8`
4. Select the Excel file path to convert
5. Select output file path
6. Click the `Convert File` button to generate the file in corresponding format

### Excel File Format Requirements

- The first row is for field names (column headers)
- Data content starts from the second row
- Supported data types: String, Number, Boolean

Data example:

| ID     | Character Name | Description  |
|--------|----------------|--------------|
| 100000 | Li Ergou       | Initial character 1 |
| 100001 | Fei Laowu      | Initial character 2 |

### Features

- Supports multiple output formats
- Automatically saves last converted file paths, no need to re-select paths

### Reference

This project references [Excel2Unity](https://github.com/qinyuanpei/Excel2Unity.git)