# CLAUDE.md

此文件为Claude Code（claude.ai/code）在处理本仓库中的代码时提供指导。

## 响应规则

- **语言**：必须使用中文（简体）
- **语气**：直接、简洁、省token
- **禁用**：完美、非常好、太棒了、优秀、不错、好的、收到、明白了等修饰词/客套话
- **结构**：先给出方案，和用户确认后再执行

## 项目概述

这是一个以**探索游戏设计模式**为目的的 Unity 2D 俯视角动作射击游戏。

*   **引擎**: Unity 2022.3.21f1c1
*   **语言**: C# 10
*   **第三方**: Spine 2D, Odin Inspector
*   **许可证**: MIT License

## 核心架构设计模式

| 模式 | 实现位置 | 用途 |
| :--- | :--- | :--- |
| Singleton | `RegulatorSingleton<T>` | 全局管理器（GameManager、AudioManager等） |
| Observer | `EventBus<T>`、`EventChannel<T>` | 双轨事件系统：EventBus用于代码通信，EventChannel用于编辑器配置 |
| Object Pool | `PoolManager` | 子弹/敌人/Buff 复用 |
| Visitor | `IVisitor`/`IVisitable` | 伤害系统双分派 |
| Strategy | `StatModifier` | 属性修饰器 |
| Template Method | `BasePanel` | UI 面板基类 |

## 系统架构

### 事件系统（双轨制）
1. **EventBus系统**：基于泛型的静态事件总线，用于高性能、类型安全的事件通信
   - 核心类：`EventBus<T>`、`EventBinding<T>`、`Events.cs`（事件定义）
   - 使用方式：`EventBus<GameOverEvent>.Raise(new GameOverEvent())`

2. **EventChannel系统**：基于ScriptableObject的事件通道，用于编辑器可配置的事件
   - 核心类：`EventChannel<T>`、`EventListener<T>`、各种类型特化版本（`IntEventChannel`、`FloatEventChannel`等）
   - 使用方式：通过Resources加载EventChannel资源，调用`Invoke()`方法

### 单例管理系统
- `RegulatorSingleton<T>`：增强型单例基类，自动处理场景切换时的实例管理
- 派生类：`GameManager`、`AudioManager`、`PoolManager`、`EnemySpawner`、`BuffSpawner`等
- 特点：支持跨场景持久化，自动清理旧实例

### 对象池系统
- `PoolManager`：管理三种对象池（Bullet、Enemy、Buff）
- `IPoolable`：可池化对象接口
- `PoolableObject`：可池化对象基类
- 支持自定义回收回调函数

### 状态/属性系统
- `Stats`：基础属性容器
- `StatModifier`：策略模式实现的属性修饰器，支持临时/永久效果
- `BuffBase`/`BuffData`：Buff系统基类
- `Health`/`HealthData`：生命值管理系统

### UI系统
- `BasePanel`：模板方法模式的UI面板基类
- `UIManager`：UI面板管理器
- 面板类：`BeginPanel`、`GamePanel`、`OptionPanel`、`PausePanel`、`GameOverPanel`等

### 玩家与战斗系统
- `PlayerController`：玩家移动控制
- `PlayerPartyManager`：玩家队伍管理
- `FireController`：射击控制
- `WeaponBase`/`Weapon191`：武器系统
- `EnemyBase`/`ChaseAI`：敌人AI系统
- `Bullet`/`BulletManager`：子弹系统

## 文件夹结构

```
Assets/Scripts/
├── Camera/              # 相机控制（CameraController）
├── DataPersistence/     # 数据持久化（JSManager、GameDataManager）
├── Enemy/              # 敌人 AI（EnemyBase、ChaseAI、Chaser）
├── EventBusSystem/     # EventBus事件系统
├── EventChannelSystem/ # EventChannel事件系统
├── ObjectPool/         # 对象池系统
├── Player/             # 玩家控制
├── Singleton/          # 单例管理器
├── Stats/              # 属性系统
├── UI/                 # UI面板
├── Utils/              # 工具类（Timer、Visitor、Tool等）
├── Weapons/            # 武器系统
├── BattleMain.cs       # 战斗场景入口
└── BeginMain.cs        # 开始场景入口
```

## 常用命令

### 开发工作流
1. **启动开发**：使用Unity Hub打开项目
2. **代码编译**：Unity编辑器自动编译C#脚本
3. **场景切换**：
   - 开始场景：`Scenes/BeginScene` → `BeginMain`
   - 战斗场景：`Scenes/BattleScene` → `BattleMain`
4. **资源管理**：ScriptableObject资源存放在`Assets/Resources/Datas/`目录

### 第三方工具
- **Spine动画**：2D骨骼动画系统，动画资源通过`SkeletonAnimation`组件使用
- **Odin Inspector**：增强编辑器Inspector，用于序列化字典等复杂类型

## 开发规范

### 命名约定
- **类名**: PascalCase（如 `PlayerController`）
- **方法名**: PascalCase（如 `TakeDamage()`）
- **私有字段**: camelCase + `[SerializeField]`（如 `[SerializeField] private float speed;`）
- **属性**: PascalCase with expression body（如 `public Stats Stats => stats;`）
- **接口**: `I` 前缀（如 `IPoolable`, `IVisitor`）
- **抽象类**: `Base` 后缀（如 `EnemyBase`, `BuffBase`）
- **常量**: ALL_CAPS 或 PascalCase（如 `MAX_HEALTH` 或 `MaxHealth`）

### 代码组织
1. **事件通信**：优先使用EventBus系统进行代码间通信，EventChannel用于编辑器可配置的事件
2. **单例使用**：通过`RegulatorSingleton<T>.Instance`访问单例实例
3. **对象池**：所有可复用的游戏对象（子弹、敌人、Buff）必须通过`PoolManager`创建和回收
4. **UI开发**：所有UI面板继承`BasePanel`，通过`UIManager.Instance.ShowPanel<T>()`显示

### 资源管理
- ScriptableObject资源存放在`Assets/Resources/`目录下
- 预制体按功能分类存放在`Assets/Prefabs/`相应子目录
- 音频资源通过`AudioManager.Instance.PlayBackGroundMusic()`和`AudioSpawner.Instance.PlaySound()`播放