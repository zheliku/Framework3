# v1.0.0
1. 重构UIKit，分为UI2DKit和UI3DKit
2. TimerKit新增PassIntervalFrame()方法
3. Framework重命名为Framework3
4. 更新全部Samples示例场景
5. 正式发布！！！

# v0.3.5
1. ResKit：
   - 添加LoadFromResourcesAsync()方法
   - 更新Samples示例场景至0.LoadFormResources

# v0.3.4
1. SingletonKit：
   - 修复Dispose()方法失效的问题
   - 重构PersistentMonoSingleton和ReplaceableMonoSingleton
2. 更新Samples示例场景至SingletonKit

# v0.3.3
1. 完善Samples示例场景至PoolKit
2. 更新PoolKit，添加GenericPool，待测试
3. PoolKit：
   - 修复SingletonPool.Get()不触发OnGet事件的问题
   - 修复ObjectPool.Clear()不触发OnClear事件的问题
   - 重构SingletonPool，更新TimerKit和AudioKit

# v0.3.2
1. 修复DestroyChildren删除transform而不是gameobject的问题
2. 完善Samples示例场景至EventKit
3. 添加EventsPro插件支持
4. 更新api：
   - BindableProperty的SetValueWithoutEvent改为SetValueWithoutNotify

# v0.3.1
1. 测试sample使用textmeshpro

# v0.3.0
1. 优化代码结构：BindableKit、DataKit、EventKit、FluentAPI、FSMKit、GridKit、PoolKit、SingletonKit、TimerKit、UIKit、UtilityKit
2. 修改部分api

# v0.2.0
1. 测试Unity包版本显示

# v0.1.0
1. 微调BindableKit、DataKit、EventKit、FluentAPI、FSMKit、GridKit、PoolKit、SingletonKit、TimerKit、UIKit、UtilityKit
2. 添加使用注释
3. 优化代码结构

# v0.0.4
1. EasyEvent优先级支持float类型
2. 调整Singleton为Core包中
3. 优化代码结构：ActionKit

# v0.0.3
1. 修复CodeGenKit代码生成中命名空间错误的问题

# v0.0.2
1. 添加 Architecture.cs 的文档注释，优化代码结构

# v0.0.1
1. 初始化 Framework3