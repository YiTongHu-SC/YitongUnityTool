# YitongUnityTool

[Chinese Version](Readme.md)

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

1. In the Unity editor, select the `Tools/Excel Converter` menu item to open the conversion tool window
2. Select the Excel file you want to convert
3. Choose the target format (JSON/CSV/XML)
4. Click the convert button to generate the file in the corresponding format

### Excel File Format Requirements

- The first row is for field names (column headers)
- Data content starts from the second row
- Supported data types: String, Number, Boolean

### Features

- Supports multiple output formats
- Preserves original data types
- Simple and easy-to-use graphical interface
- Supports Unicode characters (including Chinese)
- Automatically generates files that conform to Unity project structure

### Reference

This project references [https://github.com/qinyuanpei/Excel2Unity.git](https://github.com/qinyuanpei/Excel2Unity.git)