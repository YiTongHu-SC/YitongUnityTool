using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace HuYitong.GameTestTools
{
    [InitializeOnLoad]
    public class RunGameEditor : Editor
    {
        private const string PrevSceneKey = "TestRunGame_PrevScenePath";

        static RunGameEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        ///     保证退出场景时，加载之前的场景，而不是回到Loading场景
        /// </summary>
        /// <param name="state"></param>
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
                // 当退出播放模式时，加载之前保存的场景
                if (EditorPrefs.HasKey(PrevSceneKey))
                {
                    var lastScenePath = EditorPrefs.GetString(PrevSceneKey);
                    if (!string.IsNullOrEmpty(lastScenePath) && File.Exists(lastScenePath))
                        EditorSceneManager.OpenScene(lastScenePath);

                    EditorPrefs.DeleteKey(PrevSceneKey);
                }
        }

        [MenuItem("Test/RunGame", priority = 10)]
        public static void RunGame()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog("RunGame", "当前已在或即将进入 Play 模式。", "确定");
                return;
            }

            // 选择初始场景：取 Build Settings 中第一个启用的场景
            var startScenePath = GetFirstEnabledBuildScenePath();
            if (string.IsNullOrEmpty(startScenePath) || !File.Exists(startScenePath))
            {
                EditorUtility.DisplayDialog("RunGame",
                    "未在 Build Settings 中找到启用的初始场景。请在 File > Build Settings 中添加并启用至少一个场景。", "确定");
                return;
            }

            // 记录并提示保存当前修改
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return; // 用户取消保存

            var prevScenePath = SceneManager.GetActiveScene().path; // 可能为空（未保存的新场景）
            EditorPrefs.SetString(PrevSceneKey, prevScenePath);

            // 打开初始场景（如与当前相同则跳过）
            if (!string.Equals(prevScenePath, startScenePath))
                EditorSceneManager.OpenScene(startScenePath, OpenSceneMode.Single);

            // 进入 Play
            EditorApplication.isPlaying = true;
        }

        private static string GetFirstEnabledBuildScenePath()
        {
            var scenes = EditorBuildSettings.scenes;
            if (scenes == null || scenes.Length == 0) return null;
            foreach (var s in scenes)
                if (s.enabled && !string.IsNullOrEmpty(s.path))
                    return s.path;

            return null;
        }
    }
}