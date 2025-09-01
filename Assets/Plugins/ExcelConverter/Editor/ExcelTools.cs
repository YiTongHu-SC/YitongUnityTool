#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace HuYitong.ExcelConverter
{
    public class ExcelTools : EditorWindow
    {
        /// <summary>
        /// 当前编辑器窗口实例
        /// </summary>
        private static ExcelTools instance;

        /// <summary>
        /// Excel文件列表
        /// </summary>
        private static List<string> excelList;

        /// <summary>
        /// 项目根路径	
        /// </summary>
        private static string pathRoot;

        /// <summary>
        /// 滚动窗口初始位置
        /// </summary>
        private static Vector2 scrollPos;

        /// <summary>
        /// 输出格式索引
        /// </summary>
        private static int indexOfFormat = 0;

        /// <summary>
        /// 输出格式
        /// </summary>
        private static string[] formatOption = new string[] { "JSON", "CSV", "XML" };

        /// <summary>
        /// 编码索引
        /// </summary>
        private static int indexOfEncoding = 0;

        /// <summary>
        /// 编码选项
        /// </summary>
        private static string[] encodingOption = new string[] { "UTF-8", "GB2312" };

        /// <summary>
        /// Excel源文件路径
        /// </summary>
        private static string sourcePath = "";

        /// <summary>
        /// 输出文件路径
        /// </summary>
        private static string outputPath = "";

        // 添加用于保存路径的键名常量
        private const string SOURCE_PATH_PREF_KEY = "ExcelTools_SourcePath";
        private const string OUTPUT_PATH_PREF_KEY = "ExcelTools_OutputPath";

        /// <summary>
        /// 显示当前窗口	
        /// </summary>
        [MenuItem("Plugins/ExcelTools")]
        static void ShowExcelTools()
        {
            Init();
            //加载Excel文件
            LoadExcel();
            instance.Show();
        }

        void OnGUI()
        {
            DrawOptions();
            DrawPaths();
            DrawExport();
        }

        /// <summary>
        /// 绘制插件界面配置项
        /// </summary>
        private void DrawOptions()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("请选择格式类型:", GUILayout.Width(85));
            indexOfFormat = EditorGUILayout.Popup(indexOfFormat, formatOption, GUILayout.Width(125));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("请选择编码类型:", GUILayout.Width(85));
            indexOfEncoding = EditorGUILayout.Popup(indexOfEncoding, encodingOption, GUILayout.Width(125));
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制路径选择项
        /// </summary>
        private void DrawPaths()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Excel源文件路径:", GUILayout.Width(120));
            EditorGUILayout.TextField(sourcePath, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("选择路径", GUILayout.Width(80)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("选择Excel源文件路径", pathRoot, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    sourcePath = selectedPath;
                    // 保存路径
                    SavePaths();
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("输出文件路径:", GUILayout.Width(120));
            EditorGUILayout.TextField(outputPath, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("选择路径", GUILayout.Width(80)))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("选择输出文件路径", pathRoot, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    outputPath = selectedPath;
                    // 保存路径
                    SavePaths();
                }
            }

            Repaint();

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制插件界面输出项
        /// </summary>
        private void DrawExport()
        {
            //输出
            if (GUILayout.Button("转换文件"))
            {
                ConvertFiles();
            }
        }

        /// <summary>
        /// 转换Excel文件
        /// </summary>
        private static void ConvertFiles()
        {
            // 检查是否有选中的Excel文件
            if (excelList == null || excelList.Count == 0)
            {
                if (string.IsNullOrEmpty(sourcePath))
                {
                    Debug.LogWarning("未指定源文件路径，请指定源文件路径！");
                    return;
                }

                // 从指定路径加载所有Excel文件
                LoadExcelFromPath();
            }

            if (excelList.Count == 0)
            {
                Debug.LogWarning("没有找到可转换的Excel文件，请检查源文件路径是否正确！");
                return;
            }

            foreach (string assetsPath in excelList)
            {
                //获取Excel文件的绝对路径
                string excelPath = assetsPath;

                // 如果指定了输出路径，则使用指定的输出路径
                string outputDirectory =
                    string.IsNullOrEmpty(outputPath) ? Path.GetDirectoryName(excelPath) : outputPath;
                string fileName = Path.GetFileNameWithoutExtension(excelPath);

                //构造Excel工具类
                ExcelUtility excel = new ExcelUtility(excelPath);

                //判断编码类型
                Encoding encoding = null;
                if (indexOfEncoding == 0)
                {
                    encoding = Encoding.GetEncoding("utf-8");
                }
                else if (indexOfEncoding == 1)
                {
                    encoding = Encoding.GetEncoding("gb2312");
                }

                //判断输出类型
                string output = "";
                if (indexOfFormat == 0)
                {
                    output = Path.Combine(outputDirectory, fileName + ".json");
                    excel.ConvertToJson(output, encoding);
                }
                else if (indexOfFormat == 1)
                {
                    output = Path.Combine(outputDirectory, fileName + ".csv");
                    excel.ConvertToCSV(output, encoding);
                }
                else if (indexOfFormat == 2)
                {
                    output = Path.Combine(outputDirectory, fileName + ".xml");
                    excel.ConvertToXml(output);
                }

                Debug.Log("已转换: " + output);
            }

            //刷新本地资源
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 从指定路径加载Excel文件
        /// </summary>
        private static void LoadExcelFromPath()
        {
            if (excelList == null) excelList = new List<string>();
            excelList.Clear();

            if (!string.IsNullOrEmpty(sourcePath) && Directory.Exists(sourcePath))
            {
                string[] files = Directory.GetFiles(sourcePath, "*.xlsx");
                foreach (string file in files)
                {
                    // string relativePath = file.Replace(pathRoot + "\\", "").Replace("\\", "/");
                    excelList.Add(file);
                }
            }
        }

        /// <summary>
        /// 加载Excel
        /// </summary>
        private static void LoadExcel()
        {
            if (excelList == null) excelList = new List<string>();
            excelList.Clear();
        }

        private static void Init()
        {
            instance = EditorWindow.GetWindow<ExcelTools>();
            pathRoot = Application.dataPath;
            scrollPos = new Vector2(instance.position.x, instance.position.y + 75);

            // 加载保存的路径
            LoadPaths();
        }

        /// <summary>
        /// 加载保存的路径
        /// </summary>
        private static void LoadPaths()
        {
            sourcePath = EditorPrefs.GetString(SOURCE_PATH_PREF_KEY, "");
            outputPath = EditorPrefs.GetString(OUTPUT_PATH_PREF_KEY, "");
        }

        /// <summary>
        /// 保存路径
        /// </summary>
        private static void SavePaths()
        {
            EditorPrefs.SetString(SOURCE_PATH_PREF_KEY, sourcePath);
            EditorPrefs.SetString(OUTPUT_PATH_PREF_KEY, outputPath);
        }

        void OnSelectionChange()
        {
            //当选择发生变化时重绘窗体
            Show();
            LoadExcel();
            Repaint();
        }
    }
}

#endif