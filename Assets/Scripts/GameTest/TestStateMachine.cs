using UnityEngine;
using HuYitong.GameBaseTool.FSM;

namespace HuYitong.GameBaseTool.FSM.Test
{
    /// <summary>
    /// 测试用的MonoBehaviour类
    /// </summary>
    public class TestStateMachine : MonoBehaviour
    {
        public string TestData { get; set; } = "TestContext";
        
        private void Start()
        {
            Debug.Log("TestStateMachine 已初始化");
        }
    }
}
