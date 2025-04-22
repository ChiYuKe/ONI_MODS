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

    public float growthSpeedMultiplier = 5f; // 生长加速倍率

    private RangeVisualizer visualizer; // 可视化组件，用于判断遮挡

    // 当建筑生成（spawn）时触发
    protected override void OnSpawn()
    {
        base.OnSpawn();
        visualizer = GetComponent<RangeVisualizer>();

        // 每 5 秒自动调用一次收获逻辑
        InvokeRepeating(nameof(ScanAndHarvestPlants), 1f, 5f);
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







}
