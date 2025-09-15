## ExcelConverter

Excel文件格式转换工具，支持将Excel文件转换为多种常用数据格式，方便在Unity项目中使用数据。

### 支持的目标文件格式

- JSON
- CSV
- XML

### 安装方式

在Unity Package Manager中选择`Add package from git URL`，并使用下面的路径：

```
https://github.com/YiTongHu-SC/ExcelConverter.git
```

### 使用说明

1. 在Unity编辑器中，选择`Tools/ExcelConverter`菜单项打开转换工具窗口
2. 选择目标格式：`JSON/CSV/XML`
3. 选择输出文件的编码类型：`GB2312/UTF-8`
3. 选择需要转换的Excel文件路径
4. 选择输出文件路径
4. 点击`转换文件`按钮生成对应格式的文件

### Excel文件格式要求

- 第一行为字段名（列标题）
- 从第二行开始为数据内容
- 支持的数据类型：字符串、数字、布尔值

数据示例：

| ID     | 角色名称 | 角色描述  |
|--------|------|-------|
| 100000 | 李二狗  | 初始角色1 |
| 100001 | 费老五  | 初始角色2 |

### 特性

- 支持多种输出格式
- 自动保存上次转换的文件路径，不需要重复选取路径

### 参考项目

本项目参考了 [Excel2Unity](https://github.com/qinyuanpei/Excel2Unity.git)