using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class LoadGame : MonoBehaviour
    {
        public string SceneName;

        private void Start()
        {
            Debug.Log("LoadGame Start");
            // 在这里添加加载游戏的逻辑
            SceneManager.LoadSceneAsync(SceneName);
        }
    }
}