# 状态机自动化测试说明

## 概述
本测试套件为 HuYitong.GameBaseTool.FSM 状态机系统提供全面的自动化测试。

## 测试文件说明

### 1. TestEnums.cs
定义测试用的枚举：
- `TestState`: 测试状态（Idle, Walking, Running, Jumping）
- `TestTransition`: 测试转换（StartWalk, StartRun, Jump, Stop, Land）

### 2. TestStates.cs
实现具体的测试状态类：
- `IdleState`: 空闲状态，支持跟踪进入/退出状态
- `WalkingState`: 行走状态
- `RunningState`: 跑步状态  
- `JumpingState`: 跳跃状态

### 3. TestStateMachine.cs
测试用的MonoBehaviour上下文类。

### 4. FSMSystemTest.cs - 主测试类
包含以下测试用例：

#### 基础功能测试
- ✅ 状态机创建测试
- ✅ 状态添加测试
- ✅ 状态删除测试
- ✅ 状态验证测试

#### 状态转换测试
- ✅ 设置当前状态测试
- ✅ 正常状态转换测试
- ✅ 无效转换处理测试
- ✅ 状态生命周期测试

#### 系统功能测试
- ✅ Tick系统测试
- ✅ 边界情况测试

### 5. FSMUsageExample.cs - 使用示例
演示如何在实际游戏中使用状态机，包括：
- 键盘输入处理
- 状态转换触发
- 实时状态显示

## 使用方法

### 自动运行测试
1. 在Unity中创建一个空的GameObject
2. 添加 `FSMSystemTest` 组件
3. 确保 "Run Tests On Start" 选项已勾选
4. 运行场景，测试将自动执行

### 手动运行测试
1. 在Inspector中点击 "运行所有测试" 按钮
2. 或者在组件的右键菜单中选择 "运行所有测试"

### 查看测试结果
测试结果将在Unity Console中显示，包括：
- 每个测试用例的通过/失败状态
- 总体测试统计信息
- 详细的错误信息（如果有）

## 测试覆盖范围

### 核心功能测试
- [x] 状态机创建和初始化
- [x] 状态的添加和删除
- [x] 状态有效性验证
- [x] 当前状态设置和获取

### 状态转换测试
- [x] 有效转换执行
- [x] 无效转换处理
- [x] 状态进入/退出生命周期
- [x] 转换目标状态验证

### 系统行为测试
- [x] Tick系统正常运行
- [x] 空状态机处理
- [x] 错误状态处理
- [x] 边界条件测试

## 扩展测试

如需添加新的测试用例，请遵循以下模式：

```csharp
private IEnumerator TestNewFeature()
{
    LogTest("测试: 新功能");
    
    // 测试逻辑
    
    AssertTrue(condition, "测试描述");
    
    yield return null;
}
```

## 性能测试建议

建议定期运行以下性能测试：
1. 大量状态添加/删除的性能
2. 频繁状态转换的性能
3. 高频率Tick调用的性能

## 注意事项

1. 测试运行时会输出大量日志，可通过 `enableDebugLogs` 开关控制
2. 某些测试会故意触发错误条件来验证错误处理
3. 测试使用协程运行，避免阻塞主线程
4. 建议在发布前运行完整测试套件确保状态机稳定性
