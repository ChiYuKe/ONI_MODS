using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Klei.AI;
using UnityEngine;
using CykUtils;
using static STRINGS.CREATURES.STATS;
using System.Linq;
using static STRINGS.UI.USERMENUACTIONS.MANUAL_PUMP_DELIVERY;
using static STRINGS.UI.STARMAP;
using System;
using Satsuma.Drawing;
using KSerialization;


public class AutoPlantHarvester : KMonoBehaviour
{
    public int radius = 8; // 扫描半径（单位：格）
    public float growthSpeedMultiplier = 0.5f; // 生长加速倍率
    public List<Tag> allowedTags = new List<Tag>();
    public Vector3 storageFXOffset = Vector3.zero;



    [MyCmpGet]
    private RangeVisualizer visualizer; // 可视化组件，用于判断遮挡
    [MyCmpGet]
    private Storage storage;
    [MyCmpGet]
    private LogicOperationalController operational;

    [Serialize]
    public bool isEnabled = true; // 默认启用



    private HashSet<GameObject> uniquePlants = new HashSet<GameObject>(); // 去重植物对象集合
    private float scanInterval = 5f; // 扫描间隔时间

    // 是否启用调试日志（默认为关闭）
    public bool enableDebugLogs = false;




    protected override void OnPrefabInit()
    {
        base.OnPrefabInit();

        allowedTags.Add(GameTags.Edible);
        allowedTags.Add(GameTags.Seed);
        allowedTags.Add(GameTags.Organics);


    }










    // 当建筑生成（spawn）时触发
    protected override void OnSpawn()
    {
        base.OnSpawn();
        base.Subscribe<AutoPlantHarvester>(493375141, AutoPlantHarvester.OnRefreshUserMenuDelegate);



        // 每 5 秒自动调用一次扫描与收获逻辑
        InvokeRepeating(nameof(ScanAndHarvestPlants), 1f, scanInterval);
    }




    private static readonly EventSystem.IntraObjectHandler<AutoPlantHarvester> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<AutoPlantHarvester>(delegate (AutoPlantHarvester component, object data)
    {
        component.OnRefreshUserMenu(data);
    });
    private void OnRefreshUserMenu(object data)
    {
        // 清空按钮
        KIconButtonMenu.ButtonInfo emptyButton = new KIconButtonMenu.ButtonInfo("action_empty_contents", "清空内容", delegate
        {
            this.storage.DropAll(false, false, default(Vector3), true, null);
        }, global::Action.NumActions, null, null, null, "清空建筑内部的容物", true);


        KIconButtonMenu.ButtonInfo toggleButton;

        if (this.isEnabled) // 如果当前是启用状态，显示“禁用”按钮
        {
            toggleButton = new KIconButtonMenu.ButtonInfo(
                "action_building_disabled", // 使用一个代表关闭的图标
                "禁用自动收割",
                new System.Action(this.DisableHarvester), // 点击后禁用
                global::Action.NumActions,
                null, null, null,
                "点击禁用，停止自动扫描和收割。",
                true
            );
        }
        else // 如果当前是禁用状态，显示“启用”按钮
        {
            toggleButton = new KIconButtonMenu.ButtonInfo(
                "action_harvest", // 使用一个代表开启的图标
                "启用自动收割",
                new System.Action(this.EnableHarvester), // 点击后启用
                global::Action.NumActions,
                null, null, null,
                "点击启用，自动扫描和收割植物。",
                true
            );
        }

        // 注入按钮。给开关按钮一个较高的优先级（例如 0.5f）
        Game.Instance.userMenu.AddButton(base.gameObject, toggleButton, 0.5f);

        // 注入清空按钮（保持低优先级 1f）
        Game.Instance.userMenu.AddButton(base.gameObject, emptyButton, 1f);
    }

    // 4. 实现点击方法
    private void EnableHarvester()
    {
        this.isEnabled = true;
        // 刷新 UI，让按钮文本立即更新
        Game.Instance.userMenu.Refresh(base.gameObject);
    }

    private void DisableHarvester()
    {
        this.isEnabled = false;
        // 刷新 UI，让按钮文本立即更新
        Game.Instance.userMenu.Refresh(base.gameObject);
    }


    /// <summary>
    /// 扫描范围内的植物，并执行收获逻辑
    /// </summary>
    private void ScanAndHarvestPlants()
    {

        // 检查自定义的开关状态
        if (!isEnabled)
        {
            return;
        }
        // if (!operational.operational.IsOperational) {return;}


        // 获取当前建筑所在的格子索引
        int currentCell = Grid.PosToCell(transform.GetPosition());
        if (!Grid.IsValidCell(currentCell))
            return;

        // 分配临时列表资源（使用对象池以减少 GC）
        var cells = ListPool<int, AutoPlantHarvester>.Allocate();
        var plantEntries = ListPool<ScenePartitionerEntry, AutoPlantHarvester>.Allocate();

        try
        {
            // 获取当前格子为中心，半径范围内的非固体格子
            GameUtil.GetNonSolidCells(currentCell, radius, cells);

            // 清空去重集合
            uniquePlants.Clear();

            // 遍历所有非固体格子，收集植物对象
            for (int i = 0; i < cells.Count; i++)
            {
                int targetCell = cells[i];

                if (LineOfSightUtils.IsLineOfSightBlocked(currentCell, targetCell, visualizer))
                    continue;

                Grid.CellToXY(targetCell, out int cellX, out int cellY);
                GameScenePartitioner.Instance.GatherEntries(cellX, cellY, 1, 1, GameScenePartitioner.Instance.plants, plantEntries);
            }

            // 遍历所有找到的植物条目，去重
            foreach (var entry in plantEntries)
            {
                GameObject plantGO = SafeGetGameObject(entry.obj);
                if (plantGO != null)
                    uniquePlants.Add(plantGO);
            }

            // 遍历所有去重后的植物，进行收获 
            foreach (GameObject plantGO in uniquePlants)
            {
                if (plantGO == null)
                    continue;

                Harvestable harvestable = plantGO.GetComponent<Harvestable>();

                if (harvestable != null)
                {
                    Addvalue(plantGO, growthSpeedMultiplier);
                    PreventHarvestTask(plantGO);

                    if (harvestable.CanBeHarvested)
                    {
                        //记录植物所在的格子
                        int harvestCell = Grid.PosToCell(plantGO.transform.GetPosition());
                        // 必须在 try/catch 块外捕获，以用于延迟调度
                        int cellToStoreFrom = harvestCell;

                        try
                        {
                            harvestable.Harvest(); // 执行收获逻辑，掉落物生成

                            // 使用 GameScheduler 延迟执行 AutoStoreItems，将 harvestCell 作为 data 参数传入 ⭐️
                            if (storage != null)
                            {
                                GameScheduler.Instance.Schedule(
                                    "DelayedStore" + plantGO.name,
                                    0.1f,
                                    (System.Action<object>)AutoStoreItems, // 传入 Action<object> 类型的委托
                                    harvestCell,  //将格子索引作为 data 参数传入
                                    null
                                );
                            }


                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError($"❌ 收获失败: {plantGO.name}\n{ex}");
                        }
                    }
                }
            }
        }
        finally
        {
            cells.Recycle();
            plantEntries.Recycle();
        }
    }






    /// <summary>
    /// 自动扫描指定格子并存储物品 (用于延时调用)
    /// </summary>
    private void AutoStoreItems(object data) 
    {
        // 将传入的 data 参数转换回 int 类型的格子索引
        if (!(data is int harvestCell) || !Grid.IsValidCell(harvestCell))
            return;

        if (storage == null)
            return;

        Grid.CellToXY(harvestCell, out int cellX, out int cellY);


        // 计算影响范围，可以选择某个区域（比如周围3x3的格子）
        HashSet<int> affectedCells = new HashSet<int>();

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                int currentX = cellX + dx;
                int currentY = cellY + dy;

                if (Grid.IsValidCell(Grid.XYToCell(currentX, currentY)))
                {
                    affectedCells.Add(Grid.XYToCell(currentX, currentY));
                }
            }
        }
        // 调用绘制声波效果
        // DrawVisualEffect(harvestCell, affectedCells);

       






        var pickupEntries = ListPool<ScenePartitionerEntry, AutoPlantHarvester>.Allocate();

        // 搜索 3x3 区域
        GameScenePartitioner.Instance.GatherEntries(
            cellX - 1, cellY - 1,
            3, 3,
            GameScenePartitioner.Instance.pickupablesLayer,
            pickupEntries
        );

        // 你可以从某个地方获取建筑位置 (pointA)
        Vector3 buildingPosition = gameObject.transform.position; // 假设建筑位置是 (10, 0, 10)
        // 将植物位置（harvestCell）转换为世界坐标 (pointB)
        Vector3 plantPosition = Grid.CellToPos(harvestCell);

        Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, plantPosition, 0f);


        foreach (var entry in pickupEntries)
        {
            GameObject itemGO = SafeGetGameObject(entry.obj);
            Pickupable pickupable = itemGO != null ? itemGO.GetComponent<Pickupable>() : null;
            KPrefabID kPrefabID = itemGO != null ? itemGO.GetComponent<KPrefabID>() : null;

            if (pickupable != null && pickupable.gameObject != null)
            {
                //检查 KPrefabID 是否包含 allowedTags 列表中的任意一个 Tag
                bool hasTargetTag = allowedTags.Any(tag => kPrefabID.HasTag(tag));

                // 只有当物品具有目标 Tag 之一时，才尝试存储
                if (!hasTargetTag) continue;

                // 尝试存储
                if (storage.Store(pickupable.gameObject, true))
                {
                    PopFXManager.Instance.SpawnFX(
                        Def.GetUISprite(pickupable.gameObject, "ui", false).first, 
                        PopFXManager.Instance.sprite_Plus,
                        pickupable.GetProperName() + " " +  GameUtil.GetFormattedMass(
                            pickupable.TotalAmount,
                            GameUtil.TimeSlice.None, 
                            GameUtil.MetricMassFormat.UseThreshold, 
                            true, "{0:0.#}"),
                        pickupable.transform, 
                        this.storageFXOffset, 
                        1.5f, true, false, false);


                }
            }
        }

        pickupEntries.Recycle();
    }





    /// <summary>
    /// 阻止植物向复制人发布收获任务
    /// </summary>
    private void PreventHarvestTask(GameObject plantGO)
    {
        // 获取植物的 Harvestable 组件
        Harvestable harvestable = plantGO.GetComponent<Harvestable>();

        if (harvestable != null)
        {
            // 如果植物已有收获任务，强制取消
            if (harvestable.HasChore())
            {
                try
                {
                    harvestable.ForceCancelHarvest(); // 强制取消收获任务
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"❌ 取消收获任务失败: {plantGO.name}\n{ex}");
                }
            }

            // 关闭自动收获标记，防止复制人自动收获
            if (harvestable.harvestDesignatable != null)
            {
                harvestable.harvestDesignatable.SetHarvestWhenReady(false);  // 禁用自动收获标记
                harvestable.harvestDesignatable.MarkedForHarvest = false;     // 清除标记
            }
        }
    }

    /// <summary>
    /// 获取 GameObject（包括 Pickupable 和 KPrefabID 对象）
    /// </summary>
    public static GameObject SafeGetGameObject(object obj)
    {
        if (obj == null)
            return null;

        // 直接是 GameObject
        GameObject go = obj as GameObject;
        if (go != null)
            return go;

        // 是 Pickupable
        Pickupable pickupable = obj as Pickupable;
        if (pickupable != null)
            return pickupable.gameObject;

        // 是 KPrefabID
        KPrefabID kprefabID = obj as KPrefabID;
        if (kprefabID != null)
            return kprefabID.gameObject;

        // 其他情况
        return null;
    }

    /// <summary>
    /// 加速植物生长
    /// </summary>
    public void Addvalue(GameObject targetObject, float V)
    {
        Growing growingComponent = targetObject.GetComponent<Growing>();
        if (growingComponent == null)
        {
            Debug.Log("目标对象没有 Growing 组件！");
            return;
        }

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
            }
        }
    }









    private static readonly HashedString[] PreAnims = new HashedString[] { "grid_pre", "grid_loop" };
    private static readonly HashedString PostAnim = "grid_pst";
    // 动画延迟和持续时间
    private static readonly float DistanceDelay = 0.25f;
    private static readonly float Duration = 3f;

    // 绘制声波效果
    private static void DrawVisualEffect(int centerCell, HashSet<int> cells)
    {
        // 播放声波声音
        // SoundEvent.PlayOneShot(GlobalResources.Instance().AcousticDisturbanceSound, Grid.CellToPos(centerCell), 1f);

        // 播放声波动画
        foreach (int cell in cells)
        {
            int gridDistance = GetGridDistance(cell, centerCell);
            GameScheduler.Instance.Schedule("radialgrid_pre", DistanceDelay * (float)gridDistance, new Action<object>(SpawnEffect), cell, null);
        }
    }

    // 生成声波动画
    private static void SpawnEffect(object data)
    {
        int cell = (int)data;
        KBatchedAnimController animController = FXHelpers.CreateEffect("radialgrid_kanim", Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront), null, false, Grid.SceneLayer.FXFront, false);
        // 设置动画颜色为红色 (RGBA: 255, 0, 0, 255)
        animController.TintColour = new Color32(255, 0, 0, 200);
        animController.destroyOnAnimComplete = false;
        animController.Play(PreAnims, KAnim.PlayMode.Loop);
        GameScheduler.Instance.Schedule("radialgrid_loop", Duration, new Action<object>(DestroyEffect), animController, null);
    }

    // 销毁声波动画
    private static void DestroyEffect(object data)
    {
        KBatchedAnimController animController = (KBatchedAnimController)data;
        animController.destroyOnAnimComplete = true;
        animController.Play(PostAnim, KAnim.PlayMode.Once, 1f, 0f);
    }

    // 计算两个单元格之间的网格距离
    private static int GetGridDistance(int cell, int centerCell)
    {
        Vector2I cellPos = Grid.CellToXY(cell);
        Vector2I centerPos = Grid.CellToXY(centerCell);
        return Math.Abs(cellPos.x - centerPos.x) + Math.Abs(cellPos.y - centerPos.y);
    }


}
