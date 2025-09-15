using System;

namespace HuYitong.GameBaseTool.FSM.Test
{
    /// <summary>
    /// 测试用状态枚举
    /// </summary>
    public enum TestState
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Invalid
    }

    /// <summary>
    /// 测试用转换枚举
    /// </summary>
    public enum TestTransition
    {
        StartWalk,
        StartRun,
        Jump,
        Stop,
        Land,
        Invalid
    }
}
