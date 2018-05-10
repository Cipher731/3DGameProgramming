## 一、序

之前的作业都是采用课程介绍的架构来实现游戏，逻辑确实很清晰，但是在写的时候有一种不怎么Unity的感觉，碰巧前几天看了一些关于Unity编程实践的文章又看了一些Unity的官方教程，这次作业就尝试着用Unity way的实现方法，并且和课程中的架构进行比较。
## 二、主要差异
在课程架构中游戏的入口是Director，Director负责当前SceneController的管理，SceneController负责当前场景下的游戏渲染以及用户交互等，此外还有UserGUI等组件由Director进行管理。

这次尝试的结构比较突出的一点是基于MonoBehaviour实现各个脚本，类似Director角色的是GameManager，采用了Unity式的单例模式，但是有点不同的是另外存在GuiManager负责GUI的渲染，同样也采用了Unity式的单例模式，两者都有挂载于空对象的预制，由挂载于静态场景组件上的Loader脚本进行实例化。GameManager通过子Manager管理游戏。
## 三、值得一提的部分
### 1、Unity式的单例模式
```csharp
public static GameManager Instance;

private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
    }
    else if (Instance != this)
    {
        Destroy(Instance);
    }

    DontDestroyOnLoad(gameObject);

	// other initialization
}
```
### 2、Quaternion插值实现平滑旋转
```csharp
var targetRotation = Quaternion.LookRotation(_movement);
var deltaRotation = Quaternion.Lerp(_rigidbody.rotation, targetRotation, 5f * Time.deltaTime);
_rigidbody.MoveRotation(deltaRotation);
```
### 3、NavMesh和NavMeshAgent实现自动寻路追敌
通过Navigation标签页生成NavMesh，通过NavMeshAgent的SetDestination方法进行自动寻路
### 4、在NavMesh上随机选择可用的位置
```csharp
private Vector3 RandomPoint(Vector3 center, float range)
{
    for (var i = 0; i < 30; i++)
    {
        var randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas) &&
            Vector3.Distance(_birthPlace, randomPoint) >= 10f)
        {
            return randomPoint;
        }
    }

    return Vector3.zero;
}
```
## 四、总结
基于MonoBehaviour实现的脚本和游戏中各个物体的关系更加密切，很符合面向对象的逻辑直觉，相比之下课程架构中存在在各个组件中传递游戏对象引用的行为，直觉上割裂了面向对象的思维，更像是面向过程的编程，例如ActionManager的实现，Action对象持有作用对象的引用来从外部对其进行修改，动作的生成、执行和停止等都需要通过ActionManger进行操作，个人认为如果要统一管理动作可以采用ActionManager，但动作本身应当由游戏对象本身以MonoBehaviour脚本的Component形式进行持有，ActionManager只对其进行状态的管理，启动或者停止，就像与Animator的交互一样。

总而言之，通过这次实践，我认为课程架构的指导思想是基于Unity这个平台来实现类似Cocos的编程美学并介绍面向对象编程的优秀实践，而不是追求最佳的Unity编程实践，由于要在Unity的基础上实现一些设计模式，所以有些实现给人捉襟见肘的感觉，但是课程架构中介绍的一些编程思维是不论在哪里都可以得以活用的，在这次实践中也可以看到相关的部件只是换了一种呈现方式，这种思维才是最有用的知识。