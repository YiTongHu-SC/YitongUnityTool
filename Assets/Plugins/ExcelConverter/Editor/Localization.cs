#if UNITY_EDITOR
using UnityEditor;
using System.Collections.Generic;

namespace HuYitong.ExcelConverter
{
    /// <summary>
    /// 本地化系统，用于根据Unity编辑器语言设置显示相应语言的界面
    /// </summary>
    public static class Localization
    {
        public enum Language
        {
            Chinese,
            English
        }

        private static Language currentLanguage = Language.Chinese;

        // 中文文本
        private static Dictionary<string, string> chineseTexts = new Dictionary<string, string>()
        {
            {"SelectFormatType", "请选择格式类型:"},
            {"SelectEncodingType", "请选择编码类型:"},
            {"ExcelSourcePath", "Excel源文件路径:"},
            {"OutputPath", "输出文件路径:"},
            {"SelectPath", "选择路径"},
            {"ConvertFiles", "转换文件"},
            {"NoSourcePath", "未指定源文件路径，请指定源文件路径！"},
            {"NoExcelFiles", "没有找到可转换的Excel文件，请检查源文件路径是否正确！"},
            {"Converted", "已转换: "},
            {"PluginsMenu", "Plugins/ExcelTools"}
        };

        // 英文文本
        private static Dictionary<string, string> englishTexts = new Dictionary<string, string>()
        {
            {"SelectFormatType", "Please select format type:"},
            {"SelectEncodingType", "Please select encoding type:"},
            {"ExcelSourcePath", "Excel source path:"},
            {"OutputPath", "Output path:"},
            {"SelectPath", "Select path"},
            {"ConvertFiles", "Convert files"},
            {"NoSourcePath", "No source path specified, please specify the source path!"},
            {"NoExcelFiles", "No Excel files found, please check if the source path is correct!"},
            {"Converted", "Converted: "},
            {"PluginsMenu", "Plugins/ExcelTools"}
        };

        static Localization()
        {
            // 根据Unity编辑器语言设置当前语言
            UpdateLanguage();
        }

        /// <summary>
        /// 更新语言设置
        /// </summary>
        public static void UpdateLanguage()
        {
            // 根据Unity编辑器语言判断
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.StartsWith("zh"))
            {
                currentLanguage = Language.Chinese;
            }
            else
            {
                currentLanguage = Language.English;
            }
        }

        /// <summary>
        /// 获取指定键的本地化文本
        /// </summary>
        /// <param name="key">文本键</param>
        /// <returns>本地化文本</returns>
        public static string GetText(string key)
        {
            switch (currentLanguage)
            {
                case Language.Chinese:
                    return chineseTexts.ContainsKey(key) ? chineseTexts[key] : key;
                case Language.English:
                    return englishTexts.ContainsKey(key) ? englishTexts[key] : key;
                default:
                    return key;
            }
        }
    }
}
#endif