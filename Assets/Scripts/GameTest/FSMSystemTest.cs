using UnityEngine;
using System.Collections;
using StellarCore.FSM;

namespace HuYitong.GameBaseTool.FSM.Test
{
    /// <summary>
    /// 状态机自动化测试类
    /// </summary>
    public class FSMSystemTest : MonoBehaviour
    {
        [Header("测试配置")] [SerializeField] private bool runTestsOnStart = true;
        [SerializeField] private bool enableDebugLogs = true;

        private StateMachine<TestStateMachine, TestState, TestTransition> _stateMachine;
        private TestStateMachine _testContext;
        private IdleState _idleState;
        private WalkingState _walkingState;
        private RunningState _runningState;
        private JumpingState _jumpingState;

        private int _passedTests = 0;
        private int _failedTests = 0;
        private int _totalTests = 0;

        private void Start()
        {
            if (runTestsOnStart)
            {
                StartCoroutine(RunAllTests());
            }
        }

        [ContextMenu("运行所有测试")]
        public void StartTests()
        {
            StartCoroutine(RunAllTests());
        }

        private IEnumerator RunAllTests()
        {
            LogTest("=== 开始状态机自动化测试 ===");
            _passedTests = 0;
            _failedTests = 0;
            _totalTests = 0;

            SetupTestEnvironment();

            // 基础功能测试
            yield return StartCoroutine(TestStateMachineCreation());
            yield return StartCoroutine(TestStateAddition());
            yield return StartCoroutine(TestStateDeletion());
            yield return StartCoroutine(TestStateValidation());

            // 状态转换测试
            yield return StartCoroutine(TestSetCurrentState());
            yield return StartCoroutine(TestStateTransitions());
            yield return StartCoroutine(TestInvalidTransitions());
            yield return StartCoroutine(TestStateLifecycle());

            // Tick系统测试
            yield return StartCoroutine(TestTickSystem());

            // 边界情况测试
            yield return StartCoroutine(TestEdgeCases());

            // 输出测试结果
            LogTest($"=== 测试完成 ===");
            LogTest($"总测试数: {_totalTests}");
            LogTest($"通过: {_passedTests}");
            LogTest($"失败: {_failedTests}");
            LogTest($"成功率: {(float)_passedTests / _totalTests * 100:F1}%");

            if (_failedTests == 0)
            {
                LogTest("🎉 所有测试通过！状态机工作正常。");
            }
            else
            {
                LogError($"❌ 有 {_failedTests} 个测试失败，需要修复。");
            }
        }

        private void SetupTestEnvironment()
        {
            // 创建测试上下文
            _testContext = gameObject.AddComponent<TestStateMachine>();

            // 创建状态机
            _stateMachine = new StateMachine<TestStateMachine, TestState, TestTransition>(_testContext);

            // 创建状态实例
            _idleState = new IdleState();
            _walkingState = new WalkingState();
            _runningState = new RunningState();
            _jumpingState = new JumpingState();
        }

        private IEnumerator TestStateMachineCreation()
        {
            LogTest("测试: 状态机创建");

            AssertNotNull(_stateMachine, "状态机应该成功创建");
            AssertNotNull(_testContext, "测试上下文应该存在");
            AssertFalse(_stateMachine.HasCurrentState(), "新创建的状态机不应该有当前状态");

            yield return null;
        }

        private IEnumerator TestStateAddition()
        {
            LogTest("测试: 状态添加");

            _stateMachine.AddState(_idleState);
            _stateMachine.AddState(_walkingState);
            _stateMachine.AddState(_runningState);
            _stateMachine.AddState(_jumpingState);

            AssertTrue(_stateMachine.IsValidState(TestState.Idle), "空闲状态应该被添加");
            AssertTrue(_stateMachine.IsValidState(TestState.Walking), "行走状态应该被添加");
            AssertTrue(_stateMachine.IsValidState(TestState.Running), "跑步状态应该被添加");
            AssertTrue(_stateMachine.IsValidState(TestState.Jumping), "跳跃状态应该被添加");
            AssertFalse(_stateMachine.IsValidState(TestState.Invalid), "无效状态不应该存在");

            yield return null;
        }

        private IEnumerator TestStateDeletion()
        {
            LogTest("测试: 状态删除");

            // 添加一个临时状态进行删除测试
            var tempState = new IdleState();
            _stateMachine.AddState(tempState);
            AssertTrue(_stateMachine.IsValidState(TestState.Idle), "临时状态应该存在");

            _stateMachine.DeleteState(TestState.Idle);
            // 重新添加以便后续测试
            _stateMachine.AddState(_idleState);

            yield return null;
        }

        private IEnumerator TestStateValidation()
        {
            LogTest("测试: 状态验证");

            AssertTrue(_stateMachine.IsValidState(TestState.Idle), "有效状态应该返回true");
            AssertFalse(_stateMachine.IsValidState(TestState.Invalid), "无效状态应该返回false");

            yield return null;
        }

        private IEnumerator TestSetCurrentState()
        {
            LogTest("测试: 设置当前状态");

            // 重置状态
            _idleState.Reset();

            _stateMachine.SetCurrent(TestState.Idle);

            AssertTrue(_stateMachine.HasCurrentState(), "应该有当前状态");
            AssertEqual(_stateMachine.CurrentStateID, TestState.Idle, "当前状态ID应该是Idle");
            AssertNotNull(_stateMachine.CurrentState, "当前状态对象不应该为空");
            AssertTrue(_idleState.HasEntered, "进入Idle状态时应该调用Enter方法");

            yield return null;
        }

        private IEnumerator TestStateTransitions()
        {
            LogTest("测试: 状态转换");

            // 重置所有状态
            _idleState.Reset();
            _walkingState.Reset();
            _runningState.Reset();

            // 设置初始状态
            _stateMachine.SetCurrent(TestState.Idle);

            // 测试 Idle -> Walking 转换
            _stateMachine.PerformTransition(TestTransition.StartWalk);
            AssertEqual(_stateMachine.CurrentStateID, TestState.Walking, "应该转换到Walking状态");
            AssertTrue(_idleState.HasExited, "应该退出Idle状态");
            AssertTrue(_walkingState.HasEntered, "应该进入Walking状态");

            // 测试 Walking -> Running 转换
            _stateMachine.PerformTransition(TestTransition.StartRun);
            AssertEqual(_stateMachine.CurrentStateID, TestState.Running, "应该转换到Running状态");
            AssertTrue(_walkingState.HasExited, "应该退出Walking状态");
            AssertTrue(_runningState.HasEntered, "应该进入Running状态");

            yield return null;
        }

        private IEnumerator TestInvalidTransitions()
        {
            LogTest("测试: 无效转换");

            _stateMachine.SetCurrent(TestState.Idle);
            var originalState = _stateMachine.CurrentStateID;

            // 尝试无效转换
            _stateMachine.PerformTransition(TestTransition.Invalid);
            AssertEqual(_stateMachine.CurrentStateID, originalState, "无效转换不应该改变状态");

            // 尝试当前状态不支持的转换（Running状态不支持StartWalk）
            _stateMachine.SetCurrent(TestState.Running);
            _stateMachine.PerformTransition(TestTransition.StartWalk);
            AssertEqual(_stateMachine.CurrentStateID, TestState.Running, "不支持的转换不应该改变状态");

            yield return null;
        }

        private IEnumerator TestStateLifecycle()
        {
            LogTest("测试: 状态生命周期");

            _idleState.Reset();
            _jumpingState.Reset();

            _stateMachine.SetCurrent(TestState.Idle);
            _stateMachine.PerformTransition(TestTransition.Jump);

            AssertTrue(_idleState.HasEntered, "Idle状态应该被进入");
            AssertTrue(_idleState.HasExited, "Idle状态应该被退出");
            AssertTrue(_jumpingState.HasEntered, "Jumping状态应该被进入");
            AssertEqual(_stateMachine.CurrentStateID, TestState.Jumping, "当前状态应该是Jumping");

            yield return null;
        }

        private IEnumerator TestTickSystem()
        {
            LogTest("测试: Tick系统");

            _idleState.Reset();
            _stateMachine.SetCurrent(TestState.Idle);

            // 调用几次Tick
            for (int i = 0; i < 5; i++)
            {
                _stateMachine.Tick(Time.deltaTime);
                yield return null;
            }

            AssertTrue(_idleState.ReasonCallCount >= 5, $"Reason应该被调用至少5次，实际: {_idleState.ReasonCallCount}");
            AssertTrue(_idleState.ActCallCount >= 5, $"Act应该被调用至少5次，实际: {_idleState.ActCallCount}");
        }

        private IEnumerator TestEdgeCases()
        {
            LogTest("测试: 边界情况");

            // 测试在没有当前状态时调用Tick
            var tempStateMachine = new StateMachine<TestStateMachine, TestState, TestTransition>(_testContext);
            tempStateMachine.Tick(); // 应该不会崩溃

            // 测试在没有当前状态时执行转换
            tempStateMachine.PerformTransition(TestTransition.StartWalk); // 应该不会崩溃

            // 测试设置不存在的状态
            tempStateMachine.SetCurrent(TestState.Invalid); // 应该记录错误但不崩溃
            AssertFalse(tempStateMachine.HasCurrentState(), "设置无效状态后不应该有当前状态");

            yield return null;
        }

        // 断言辅助方法
        private void AssertTrue(bool condition, string message)
        {
            _totalTests++;
            if (condition)
            {
                _passedTests++;
                LogTest($"✓ {message}");
            }
            else
            {
                _failedTests++;
                LogError($"✗ {message}");
            }
        }

        private void AssertFalse(bool condition, string message)
        {
            AssertTrue(!condition, message);
        }

        private void AssertNotNull(object obj, string message)
        {
            AssertTrue(obj != null, message);
        }

        private void AssertEqual<T>(T expected, T actual, string message)
        {
            AssertTrue(expected.Equals(actual), $"{message} - 期望: {expected}, 实际: {actual}");
        }

        private void LogTest(string message)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[FSM测试] {message}");
            }
        }

        private void LogError(string message)
        {
            Debug.LogError($"[FSM测试] {message}");
        }
    }
}