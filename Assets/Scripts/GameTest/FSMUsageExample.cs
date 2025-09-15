using GameBaseTool.FSM;
using UnityEngine;

namespace HuYitong.GameBaseTool.FSM.Test
{
    /// <summary>
    /// 状态机使用示例
    /// 演示如何在实际游戏中使用状态机
    /// </summary>
    public class FSMUsageExample : MonoBehaviour
    {
        [Header("状态机示例")]
        [SerializeField] private KeyCode walkKey = KeyCode.W;
        [SerializeField] private KeyCode runKey = KeyCode.R;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode stopKey = KeyCode.S;
        
        private StateMachine<TestStateMachine, TestState, TestTransition> _playerStateMachine;
        private TestStateMachine _playerContext;

        private void Start()
        {
            SetupStateMachine();
        }

        private void SetupStateMachine()
        {
            // 创建玩家上下文
            _playerContext = gameObject.AddComponent<TestStateMachine>();
            
            // 创建状态机
            _playerStateMachine = new StateMachine<TestStateMachine, TestState, TestTransition>(_playerContext);
            
            // 添加所有状态
            _playerStateMachine.AddState(new IdleState());
            _playerStateMachine.AddState(new WalkingState());
            _playerStateMachine.AddState(new RunningState());
            _playerStateMachine.AddState(new JumpingState());
            
            // 设置初始状态
            _playerStateMachine.SetCurrent(TestState.Idle);
            
            Debug.Log("玩家状态机已初始化，当前状态: " + _playerStateMachine.CurrentStateID);
        }

        private void Update()
        {
            // 更新状态机
            _playerStateMachine?.Tick(Time.deltaTime);
            
            // 处理输入
            HandleInput();
            
            // 显示当前状态
            DisplayCurrentState();
        }

        private void HandleInput()
        {
            if (_playerStateMachine == null) return;

            if (Input.GetKeyDown(walkKey))
            {
                _playerStateMachine.PerformTransition(TestTransition.StartWalk);
            }
            else if (Input.GetKeyDown(runKey))
            {
                _playerStateMachine.PerformTransition(TestTransition.StartRun);
            }
            else if (Input.GetKeyDown(jumpKey))
            {
                _playerStateMachine.PerformTransition(TestTransition.Jump);
            }
            else if (Input.GetKeyDown(stopKey))
            {
                _playerStateMachine.PerformTransition(TestTransition.Stop);
            }
            else if (Input.GetKeyDown(KeyCode.L)) // L键用于着陆
            {
                _playerStateMachine.PerformTransition(TestTransition.Land);
            }
        }

        private void DisplayCurrentState()
        {
            if (_playerStateMachine?.HasCurrentState() == true)
            {
                // 可以在这里更新UI显示当前状态
                // 例如：statusText.text = "当前状态: " + _playerStateMachine.CurrentStateID;
            }
        }

        [ContextMenu("打印状态机信息")]
        private void PrintStateMachineInfo()
        {
            if (_playerStateMachine == null)
            {
                Debug.Log("状态机未初始化");
                return;
            }

            Debug.Log($"当前状态: {_playerStateMachine.CurrentStateID}");
            Debug.Log($"有当前状态: {_playerStateMachine.HasCurrentState()}");
            
            // 测试状态验证
            Debug.Log($"Idle状态有效: {_playerStateMachine.IsValidState(TestState.Idle)}");
            Debug.Log($"Invalid状态有效: {_playerStateMachine.IsValidState(TestState.Invalid)}");
        }

        private void OnGUI()
        {
            if (_playerStateMachine == null) return;

            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Label("状态机控制示例", GUI.skin.box);
            GUILayout.Label($"当前状态: {_playerStateMachine.CurrentStateID}");
            GUILayout.Space(10);
            
            GUILayout.Label("按键控制:");
            GUILayout.Label($"W - 开始行走");
            GUILayout.Label($"R - 开始跑步");
            GUILayout.Label($"Space - 跳跃");
            GUILayout.Label($"S - 停止");
            GUILayout.Label($"L - 着陆");
            
            GUILayout.EndArea();
        }
    }
}
