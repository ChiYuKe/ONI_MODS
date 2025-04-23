using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Klei.AI;
using KModTool;
using UnityEngine;
using static STRINGS.CREATURES.STATS;

[AddComponentMenu("KMonoBehaviour/scripts/AutoPlantHarvester")]
public class AutoPlantHarvester : KMonoBehaviour
{
    public int radius = 8; // 扫描半径（单位：格）

    public float growthSpeedMultiplier = 10f; // 生长加速倍率
    private HandleVector<int>.Handle listenerEntry;

    private RangeVisualizer visualizer; // 可视化组件，用于判断遮挡

    // 当建筑生成（spawn）时触发
    protected override void OnSpawn()
    {
        base.OnSpawn();
        visualizer = GetComponent<RangeVisualizer>();

        // 每 5 秒自动调用一次收获逻辑
        InvokeRepeating(nameof(ScanAndHarvestPlants), 1f, 5f);

        // 注册监听 Pickupable 掉落物体
        // listenerEntry = GameScenePartitioner.Instance.Add("AutoPlantHarvester", gameObject, Grid.PosToCell(transform.GetPosition()), GameScenePartitioner.Instance.pickupablesLayer, OnItemSpawned);
    }

    // 扫描范围内的植物，并执行收获
    private void ScanAndHarvestPlants()
    {
        // 获取当前建筑所在的格子索引
        int currentCell = Grid.PosToCell(transform.GetPosition());
        if (!Grid.IsValidCell(currentCell))
            return;

        // 分配临时列表资源（使用对象池以减少 GC）
        var cells = ListPool<int, AutoPlantHarvester>.Allocate();
        var plantEntries = ListPool<ScenePartitionerEntry, AutoPlantHarvester>.Allocate();

        HashSet<GameObject> uniquePlants = new HashSet<GameObject>(); // 用于去重植物对象

        try
        {
            // 获取当前格子为中心，半径范围内的非固体格子
            GameUtil.GetNonSolidCells(currentCell, radius, cells);

            // 遍历所有非固体格子
            for (int i = 0; i < cells.Count; i++)
            {
                int targetCell = cells[i];

              
                if (!KModGridUtilities.IsLineOfSightBlocked(currentCell, targetCell, visualizer))
                    continue;

                // 获取格子的 X,Y 坐标
                Grid.CellToXY(targetCell, out int cellX, out int cellY);

                // 获取该区域内注册在 plant 层的物体（即植物）
                GameScenePartitioner.Instance.GatherEntries(cellX, cellY, 1, 1, GameScenePartitioner.Instance.plants, plantEntries);
            }

            // 遍历找到的植物条目，转为 GameObject 并添加到去重集合
            foreach (var entry in plantEntries)
            {
                GameObject plantGO = KGameObjectUtil.SafeGetGameObject(entry.obj);
                if (plantGO != null)
                    uniquePlants.Add(plantGO);
            }

            Debug.Log($"[AutoPlantHarvester] 去重后植物数量: {uniquePlants.Count}");

            // 遍历所有可收获的植物
            foreach (GameObject plantGO in uniquePlants)
            {
                if (plantGO == null)
                    continue;

                // 获取植物的 Harvestable 组件
                Harvestable harvestable = plantGO.GetComponent<Harvestable>();

                if (harvestable != null)
                {
                    DebugInfo(plantGO);
                    Addvalue(plantGO, 0.1f);

                    // 如果植物可以收获，且希望禁止复制人干预
                    if (harvestable.CanBeHarvested)
                    {
                        Debug.Log($"✅ 可收获: {plantGO.name}");

                        

                        // 1. 取消收获任务
                        if (harvestable.HasChore())
                        {
                            try
                            {
                                harvestable.ForceCancelHarvest(); // 取消收获任务
                                Debug.Log($"🛑 取消任务: {plantGO.name}");
                            }
                            catch (System.Exception ex)
                            {
                                Debug.LogError($"❌ 取消任务失败: {plantGO.name}\n{ex}");
                            }
                        }

                        // 2. 关闭 HarvestWhenReady，防止自动标记
                        if (harvestable.harvestDesignatable != null)
                        {
                            harvestable.harvestDesignatable.SetHarvestWhenReady(false);  // 关闭自动收获标记
                            harvestable.harvestDesignatable.MarkedForHarvest = false;     // 清除标记
                            Debug.Log($"🚫 禁止复制人收获: {plantGO.name}");
                        }

                        // 3. 执行自动收获
                        try
                        {
                            harvestable.Harvest(); // 执行收获逻辑
                            Debug.Log($"✅ 成功收获: {plantGO.name}");

                            // 收获成功后等待一帧，然后处理新生成的 pickupables
                            StartCoroutine(HandleNewFruitsAfterHarvest());



                            // 获取掉落的果实并让它飞向建筑
                            Pickupable pickupable = plantGO.GetComponent<Pickupable>();
                            if (pickupable != null && IsFruit(pickupable))
                            {
                                StartCoroutine(ParabolicMoveCoroutine(plantGO));
                            }






                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError($"❌ 收获失败: {plantGO.name}\n{ex}");
                        }
                    }
                    else
                    {
                       

                        Debug.Log($"❌ 未成熟: {plantGO.name}");
                    }
                }
                else
                {
                    Debug.Log($"⚠️ 无 Harvestable 组件: {plantGO.name}");
                }
            }

        }
        finally
        {
            // 确保资源被正确释放（对象池回收）
            cells.Recycle();
            plantEntries.Recycle();
        }
    }


    private IEnumerator HandleNewFruitsAfterHarvest()
    {
        // 分配用于存储场景中注册的 Pickupable 条目（使用对象池）
        var nearbyPickupables = ListPool<ScenePartitionerEntry, AutoPlantHarvester>.Allocate();
        var PickupableCells = ListPool<int, AutoPlantHarvester>.Allocate();

        // 等待一帧或一段时间，确保 Harvest() 生成的果实已经出生
        yield return new WaitForSeconds(0.1f); // 也可以是 yield return null;

        // 获取当前建筑所在的格子
        int currentCell = Grid.PosToCell(transform.GetPosition());
        if (!Grid.IsValidCell(currentCell))
        {
            yield break;
        }

        // 获取当前格子为中心，半径范围内的非固体格子
        GameUtil.GetNonSolidCells(currentCell, radius, PickupableCells);



        Debug.Log($"[AutoPlantHarvester] cells列表长度: {PickupableCells.Count}");

        // 遍历所有目标格子
        for (int i = 0; i < PickupableCells.Count; i++)
        {
            int targetCell = PickupableCells[i];

            // 判断视线是否被遮挡（若遮挡则跳过）
            if (!KModGridUtilities.IsLineOfSightBlocked(currentCell, targetCell, visualizer))
                continue;

            // 获取格子的 XY 坐标（ScenePartitioner 使用的是 XY）
            Grid.CellToXY(targetCell, out int cellX, out int cellY);

            // 在该格子区域（1x1）内收集所有 Pickupable 条目
            GameScenePartitioner.Instance.GatherEntries(
                cellX, cellY, 1, 1,
                GameScenePartitioner.Instance.pickupablesLayer,
                nearbyPickupables
            );

            Debug.Log($"[AutoPlantHarvester] 列表长度: {nearbyPickupables.Count}");
        }

        try
        {
            // 遍历所有找到的物体
            foreach (var entry in nearbyPickupables)
            {
                // 安全地获取 GameObject 实例
                GameObject obj = KGameObjectUtil.SafeGetGameObject(entry.obj);
                if (obj == null) continue;

                Debug.Log($"[AutoPlantHarvester] 发现新果实: {obj.name}");

                // 尝试获取 Pickupable 组件
                Pickupable pickup = obj.GetComponent<Pickupable>();
                if (pickup != null && IsFruit(pickup)) // 判断是否是果实
                {
                    // 启动飞行动画（抛物线运动）
                    StartCoroutine(ParabolicMoveCoroutine(pickup.gameObject));
                }
            }
        }
        finally
        {
            // 回收对象池资源，避免 GC 压力
            nearbyPickupables.Recycle();
            PickupableCells.Recycle();


        }
    }

















    public void DebugInfo(GameObject targetObject)
    {
        // 获取目标对象的 Growing 组件
        Growing growingComponent = targetObject.GetComponent<Growing>();

        // 如果没有找到 Growing 组件，则打印错误信息
        if (growingComponent == null)
        {
            Debug.Log("目标对象没有 Growing 组件！");
            return;
        }
        // 使用反射获取 maturity 字段
        FieldInfo maturityField = typeof(Growing).GetField("maturity", BindingFlags.NonPublic | BindingFlags.Instance);
        if (maturityField != null)
        {
            var maturityInstance = maturityField.GetValue(growingComponent) as AmountInstance;
            if (maturityInstance != null)
            {
                // 获取并打印成熟度信息
                Debug.Log($"当前成熟度: {maturityInstance.value}, 最大成熟度: {maturityInstance.GetMax()}, 成长百分比: {maturityInstance.value / maturityInstance.GetMax() * 100}%");
            }
            else
            {
                Debug.Log("未能获取到 maturity 实例！");
            }
        }


        // 
      
        Debug.Log($"是否已完全成熟: {growingComponent.IsGrown()}");
        Debug.Log($"是否可以继续成长: {growingComponent.CanGrow()}");
        Debug.Log($"是否正在成长: {growingComponent.IsGrowing()}");
        Debug.Log($"家养植物成长时间: {growingComponent.DomesticGrowthTime()}");
        Debug.Log($"野生植物成长时间: {growingComponent.WildGrowthTime()}");
       

        Debug.Log($"是否已经达到下一个收获期: {growingComponent.ReachedNextHarvest()}");
      
    }



    public void Addvalue(GameObject targetObject, float V)
    {
        // 获取目标对象的 Growing 组件
        Growing growingComponent = targetObject.GetComponent<Growing>();

        // 如果没有找到 Growing 组件，则打印错误信息
        if (growingComponent == null)
        {
            Debug.Log("目标对象没有 Growing 组件！");
            return;
        }

        // 使用反射获取 maturity 字段
        FieldInfo maturityField = typeof(Growing).GetField("maturity", BindingFlags.NonPublic | BindingFlags.Instance);
        if (maturityField != null)
        {
            var maturityInstance = maturityField.GetValue(growingComponent) as AmountInstance;
            if (maturityInstance != null)
            {
                // 修改成熟度值来加速生长
                maturityInstance.value += V;

                // 确保成熟度不会超过最大值
                if (maturityInstance.value > maturityInstance.GetMax())
                {
                    maturityInstance.value = maturityInstance.GetMax();
                }

                Debug.Log($"加速后的成熟度: {maturityInstance.value}, 成长百分比: {maturityInstance.value / maturityInstance.GetMax() * 100}%");
            }
            else
            {
                Debug.Log("未能获取到 maturity 实例！");
            }
        }

    }


    private bool IsFruit(Pickupable pickup)
    {
        // pickup.HasTag(GameTags.Edible)

        return true;
    }

    private IEnumerator ParabolicMoveCoroutine(GameObject fruit)
    {
        // 获取 Pickupable 组件
        Pickupable pickupable = fruit.GetComponent<Pickupable>();
        if (pickupable == null)
        {
            Debug.LogWarning("无法找到 Pickupable 组件，无法让物品飞行！");
            yield break; // 终止协程
        }

        Vector3 startPos = fruit.transform.position;
        Vector3 targetPos = transform.position; // 目标建筑的位置
        float timeToReachTarget = 1f; // 飞行持续时间
        float elapsedTime = 0f;

        float height = 2f; // 最大高度（可以调整）

        while (elapsedTime < timeToReachTarget)
        {
            elapsedTime += Time.deltaTime;
            float fraction = elapsedTime / timeToReachTarget;

            // 计算当前的抛物线高度（模拟重力）
            float heightOffset = Mathf.Sin(fraction * Mathf.PI) * height;

            // 使用 Lerp 计算水平移动
            float horizontalFraction = fraction;
            Vector3 horizontalMovement = Vector3.Lerp(startPos, targetPos, horizontalFraction);

            // 组合水平移动和垂直高度
            fruit.transform.position = new Vector3(horizontalMovement.x, startPos.y + heightOffset, horizontalMovement.z);

            yield return null; // 等待下一帧
        }

        // 确保物品准确到达目标位置
        fruit.transform.position = targetPos;

        // 动画结束后储存物品
        StoreFruit(fruit);
    }





    private void StoreFruit(GameObject fruit)
    {
        Storage storage = GetComponent<Storage>();
        if (storage != null)
        {
            storage.Store(fruit.gameObject);
            Debug.Log($"✅ 果实已成功储存: {fruit.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ 未找到 Storage 组件，无法储存果实！");
        }
    }







}
