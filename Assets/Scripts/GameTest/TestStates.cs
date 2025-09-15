using GameBaseTool.FSM;
using UnityEngine;

namespace HuYitong.GameBaseTool.FSM.Test
{
    /// <summary>
    /// 空闲状态
    /// </summary>
    public class IdleState : FsmState<TestStateMachine, TestState, TestTransition>
    {
        public bool HasEntered { get; private set; }
        public bool HasExited { get; private set; }
        public int ReasonCallCount { get; private set; }
        public int ActCallCount { get; private set; }

        public IdleState() : base(TestState.Idle)
        {
            AddTransition(TestTransition.StartWalk, TestState.Walking);
            AddTransition(TestTransition.StartRun, TestState.Running);
            AddTransition(TestTransition.Jump, TestState.Jumping);
        }

        public override void Enter()
        {
            HasEntered = true;
            Debug.Log("进入空闲状态");
        }

        public override void Exit()
        {
            HasExited = true;
            Debug.Log("退出空闲状态");
        }

        public override void Reason(float deltaTime = 0)
        {
            ReasonCallCount++;
        }

        public override void Act(float deltaTime = 0)
        {
            ActCallCount++;
        }

        public void Reset()
        {
            HasEntered = false;
            HasExited = false;
            ReasonCallCount = 0;
            ActCallCount = 0;
        }
    }

    /// <summary>
    /// 行走状态
    /// </summary>
    public class WalkingState : FsmState<TestStateMachine, TestState, TestTransition>
    {
        public bool HasEntered { get; private set; }
        public bool HasExited { get; private set; }

        public WalkingState() : base(TestState.Walking)
        {
            AddTransition(TestTransition.Stop, TestState.Idle);
            AddTransition(TestTransition.StartRun, TestState.Running);
            AddTransition(TestTransition.Jump, TestState.Jumping);
        }

        public override void Enter()
        {
            HasEntered = true;
            Debug.Log("进入行走状态");
        }

        public override void Exit()
        {
            HasExited = true;
            Debug.Log("退出行走状态");
        }

        public override void Reason(float deltaTime = 0)
        {
            // 行走逻辑
        }

        public override void Act(float deltaTime = 0)
        {
            // 行走动作
        }

        public void Reset()
        {
            HasEntered = false;
            HasExited = false;
        }
    }

    /// <summary>
    /// 跑步状态
    /// </summary>
    public class RunningState : FsmState<TestStateMachine, TestState, TestTransition>
    {
        public bool HasEntered { get; private set; }
        public bool HasExited { get; private set; }

        public RunningState() : base(TestState.Running)
        {
            AddTransition(TestTransition.Stop, TestState.Idle);
            AddTransition(TestTransition.Jump, TestState.Jumping);
        }

        public override void Enter()
        {
            HasEntered = true;
            Debug.Log("进入跑步状态");
        }

        public override void Exit()
        {
            HasExited = true;
            Debug.Log("退出跑步状态");
        }

        public override void Reason(float deltaTime = 0)
        {
            // 跑步逻辑
        }

        public override void Act(float deltaTime = 0)
        {
            // 跑步动作
        }

        public void Reset()
        {
            HasEntered = false;
            HasExited = false;
        }
    }

    /// <summary>
    /// 跳跃状态
    /// </summary>
    public class JumpingState : FsmState<TestStateMachine, TestState, TestTransition>
    {
        public bool HasEntered { get; private set; }
        public bool HasExited { get; private set; }

        public JumpingState() : base(TestState.Jumping)
        {
            AddTransition(TestTransition.Land, TestState.Idle);
        }

        public override void Enter()
        {
            HasEntered = true;
            Debug.Log("进入跳跃状态");
        }

        public override void Exit()
        {
            HasExited = true;
            Debug.Log("退出跳跃状态");
        }

        public override void Reason(float deltaTime = 0)
        {
            // 跳跃逻辑
        }

        public override void Act(float deltaTime = 0)
        {
            // 跳跃动作
        }

        public void Reset()
        {
            HasEntered = false;
            HasExited = false;
        }
    }
}
