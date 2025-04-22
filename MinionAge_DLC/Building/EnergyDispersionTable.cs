using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using System;
using UnityEngine;
using static STRINGS.MISC.STATUSITEMS;
using MinionAge;
using TemplateClasses;
using KModTool;
using static UnityEngine.GraphicsBuffer;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

namespace MinionAge_DLC
{
    [AddComponentMenu("KMonoBehaviour/scripts/ItemPedestal")]
    public class EnergyDispersionTable : KMonoBehaviour
    {
        [MyCmpReq]
        protected SingleEntityReceptacle receptacle;

        private RangeVisualizer rangeVisualizer;
        private int radius = 10; // 检测半径，最好和 RangeVisualizer 组件的 Range范围对应
        private Vector2I OriginOffset = new Vector2I(0, 0);
        private bool BlockingTileVisible = true;

        private static readonly EventSystem.IntraObjectHandler<EnergyDispersionTable> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<EnergyDispersionTable>(delegate (EnergyDispersionTable component, object data)
        {
            component.OnOccupantChanged(data);
        });


       
        private ProgressBar progressBar; // 进度条组件
        private float maxMass;            // 最大质量

        private float massReductionInterval = 10f; // 质量减少的时间间隔（秒）
        private float massReductionAmount = 1f; // 每次减少的质量
        private GameObject currentOccupant; // 当前占用的对象
        private bool isMassReductionActive = false; // 是否正在减少质量

        private const float CheckInterval = 10f; // 每 2 秒执行一次逻辑
        private bool isCheckActive = false; // 是否正在执行检查逻辑

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe(-731304873, OnOccupantChangedDelegate);

            rangeVisualizer = gameObject.GetComponent<RangeVisualizer>();

            if (rangeVisualizer != null)
            {
                Vector2I newRangeMin = new Vector2I(OriginOffset.x - radius, OriginOffset.y - radius);
                Vector2I newRangeMax = new Vector2I(OriginOffset.x + radius, OriginOffset.y + radius);

                rangeVisualizer = gameObject.GetComponent<RangeVisualizer>();

                // 初始化 RangeVisualizer 组件
                rangeVisualizer.OriginOffset = OriginOffset;
                rangeVisualizer.RangeMin = newRangeMin;
                rangeVisualizer.RangeMax = newRangeMax;
                rangeVisualizer.BlockingTileVisible = BlockingTileVisible;
            }

            if (receptacle.Occupant != null)
            {
                KBatchedAnimController component = receptacle.Occupant.GetComponent<KBatchedAnimController>();
                if (component != null)
                {
                    component.enabled = true;
                    component.sceneLayer = Grid.SceneLayer.Move;
                }

                OnOccupantChanged(receptacle.Occupant);
            }
        }

        private void OnOccupantChanged(object data)
        {
            if (data == null)
            {
                Debug.LogWarning("没有物品在基座上");
                StopMassReduction(); // 停止质量减少
                StopCheckLogic();
                DestroyProgressBar(); // 销毁进度条
                return;
            }

            // 确保 data 是 GameObject 类型
            GameObject occupyingObject = data as GameObject;
            if (occupyingObject != null)
            {
                Debug.Log("当前物品的名字：" + occupyingObject.name);
                currentOccupant = occupyingObject;

                // 初始化进度条
                InitializeProgressBar();

                StartMassReduction(); // 开始质量减少
                StartCheckLogic(); // 开始检测对象
            }
            else
            {
                Debug.LogWarning("data 不是 GameObject 类型，无法获取物品名称");
                StopMassReduction(); // 停止质量减少
                StopCheckLogic(); // 停止检测对象
                DestroyProgressBar(); // 销毁进度条
            }
        }



        private void InitializeProgressBar()
        {
            if (progressBar != null)
            {
                DestroyProgressBar(); // 如果已有进度条，先销毁
            }

            // 检查基座上方的物体是否附加了 PrimaryElement 组件
            GameObject occupant = receptacle.Occupant;
            if (occupant == null)
            {
                Debug.LogWarning("基座上没有物体，无法创建进度条");
                return;
            }

            PrimaryElement primaryElement = occupant.GetComponent<PrimaryElement>();
            if (primaryElement == null)
            {
                Debug.LogWarning("基座上方的物体没有 PrimaryElement 组件，无法创建进度条");
                return;
            }

            // 设置最大质量
            maxMass = primaryElement.Mass;

            // 检查进度条预制体是否加载
            if (ProgressBarsConfig.Instance == null || ProgressBarsConfig.Instance.progressBarPrefab == null)
            {
                Debug.LogError("进度条预制体未加载！");
                return;
            }

            // 创建进度条，绑定到基座本身
            progressBar = ProgressBar.CreateProgressBar(base.gameObject, () =>
            {
                // 获取基座上方的物体
                GameObject currentOccupant = receptacle.Occupant;
                if (currentOccupant != null)
                {
                    // 获取物体的 PrimaryElement 组件
                    PrimaryElement currentPrimaryElement = currentOccupant.GetComponent<PrimaryElement>();
                    if (currentPrimaryElement != null)
                    {
                        return currentPrimaryElement.Mass / maxMass; // 返回当前质量比例
                    }
                }
                return 0f; // 如果没有物体或物体没有 PrimaryElement 组件，返回 0
            });

            if (progressBar == null)
            {
                Debug.LogError("进度条创建失败！");
                return;
            }

            // 设置进度条可见性
            progressBar.SetVisibility(true);

            // 设置进度条颜色
            progressBar.barColor = Color.green;

            Debug.Log("进度条创建成功！");
        }




        private void DestroyProgressBar()
        {
            if (progressBar != null)
            {
                Util.KDestroyGameObject(progressBar.gameObject); // 销毁进度条
                progressBar = null;
            }
        }

        private void StartMassReduction()
        {
            if (isMassReductionActive) return; // 如果已经在减少质量，则直接返回

            isMassReductionActive = true;
            InvokeRepeating(nameof(ReduceMass), massReductionInterval, massReductionInterval); // 定时调用 ReduceMass 方法
        }

        private void StopMassReduction()
        {
            if (!isMassReductionActive) return; // 如果已经停止减少质量，则直接返回

            isMassReductionActive = false;
            CancelInvoke(nameof(ReduceMass)); // 停止定时调用
        }

        private void StartCheckLogic()
        {
            if (isCheckActive) return; // 如果已经在执行检查逻辑，则直接返回

            isCheckActive = true;
            InvokeRepeating(nameof(CheckOccupant), CheckInterval, CheckInterval); // 定时调用 CheckOccupant 方法
        }

        private void StopCheckLogic()
        {
            if (!isCheckActive) return; // 如果已经停止执行检查逻辑，则直接返回

            isCheckActive = false;
            CancelInvoke(nameof(CheckOccupant)); // 停止定时调用
        }

        private void CheckOccupant()
        {
            if (currentOccupant == null) { return; }

            // 根据对象的标签或类型执行不同的逻辑
            KPrefabID prefabID = currentOccupant.GetComponent<KPrefabID>();
            if (prefabID != null)
            {
                if (prefabID.HasTag("KmodMiniBrainCore2")) //人造大脑
                    HandleEdibleItem();
            }
            else if (prefabID.HasTag(GameTags.Metal)) // 如果对象是金属
            {
                Debug.Log("对象是金属，执行特定逻辑");
                HandleMetalItem();
            }
            else
            {
                Debug.Log("对象是其他类型，执行默认逻辑");
                HandleDefaultItem();
            }
        }

            
        

        private void HandleEdibleItem()
        {
            HandleAccessiblePickupables();
        }

        private void HandleMetalItem()
        {
            // 处理金属对象的逻辑
            Debug.Log("处理金属对象的逻辑");
        }

        private void HandleDefaultItem()
        {
            // 处理默认对象的逻辑
            Debug.Log("处理默认对象的逻辑");
        }

        // 执行定时任务的逻辑，处理所有可见且可访问的可拾取对象
        private void HandleAccessiblePickupables()
        {
            if (transform == null)
            {
                Console.WriteLine("[KDEBUG] transform 为空，无法执行操作");
                return;
            }

            // 获取当前对象位置对应的网格单元
            int currentCell = Grid.PosToCell(transform.GetPosition());

            // 获取所有非固体网格单元
            List<int> potentiallyAccessibleCells = new List<int>();
            GameUtil.GetNonSolidCells(currentCell, this.radius, potentiallyAccessibleCells);

            List<int> visibleAccessibleCells = new List<int>();

            // 遍历所有非固体网格单元，提前排除视线阻挡的格子
            foreach (int cell in potentiallyAccessibleCells)
            {
                int cellX, cellY;
                Grid.CellToXY(cell, out cellX, out cellY);

                // 如果视线不被阻挡，将该格子添加到 visibleAccessibleCells 列表
                bool isVisible = IsCellIsVisibledByLineOfSight(currentCell, cell, rangeVisualizer);
                if (isVisible)
                {
                    visibleAccessibleCells.Add(cell);
                }
            }

            // 使用 visibleAccessibleCells 中的格子来收集可拾取对象
            List<ScenePartitionerEntry> pickupableItemsInRange = new List<ScenePartitionerEntry>();
            foreach (int cell in visibleAccessibleCells)
            {
                int cellX, cellY;
                Grid.CellToXY(cell, out cellX, out cellY);

                // 收集可拾取层对象
                GameScenePartitioner.Instance.GatherEntries(cellX, cellY, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pickupableItemsInRange);
            }

            // 处理收集到的可拾取对象
            foreach (var entry in pickupableItemsInRange)
            {
                Pickupable pickupable = entry.obj as Pickupable;
                if (pickupable != null)
                {
                    if (pickupable.gameObject == gameObject) continue;
                    if (pickupable.gameObject == currentOccupant) continue;
                    if (pickupable.gameObject.GetComponent<MinionBrain>() == null) continue;
                    if (pickupable.GetComponent<KPrefabID>().HasTag("Corpse")) continue;
                    if (pickupable.GetComponent<KPrefabID>().HasTag(GameTags.Minions.Models.Bionic)) continue;

                    var fx = pickupable.gameObject.GetComponent<FlyingEffectForMinion>();
                    if (fx == null)
                        fx = pickupable.gameObject.AddComponent<FlyingEffectForMinion>();

                    fx.FlyTo(transform.position, () => {
                        Debug.Log("飞行完成，可爆炸或播放动画");
                    });








                    // KFMOD.PlayOneShot(GlobalAssets.GetSound("Hatch_voice_die", false), CameraController.Instance.GetVerticallyScaledPosition(pickupable.gameObject.transform.GetPosition(), false), 1f);
                    // GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), pickupable.gameObject.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);



                    // AudioUtil.PlaySound(ModAssets.Sounds.SPLAT, ModAssets.GetSFXVolume() * 0.5f, 1f);
                    // AudioUtil.PlaySound(ModAssets.Sounds.XWYSZMB, CameraController.Instance.GetVerticallyScaledPosition(pickupable.gameObject.transform.GetPosition()), 1f);

                    Console.WriteLine("[KDEBUG] 找到的对象: " + pickupable.name);
                }


            }
        }





         





        // 判断两点之间的视线是否被阻挡
        public static bool IsCellIsVisibledByLineOfSight(int startCell, int targetCell, RangeVisualizer rangeVisualizer)
        {
            try
            {
                int startX, startY;
                Grid.CellToXY(startCell, out startX, out startY);
                int targetX, targetY;
                Grid.CellToXY(targetCell, out targetX, out targetY);

                if (rangeVisualizer == null)
                {
                    throw new ArgumentNullException(nameof(rangeVisualizer), "[KDEBUG] RangeVisualizer 为空。无法执行视线检查");
                }

                Func<int, bool> blockingCb = rangeVisualizer.BlockingCb;
                Func<int, bool> visibleBlockingCb = rangeVisualizer.BlockingVisibleCb ?? ((int i) => rangeVisualizer.BlockingTileVisible);

                bool lineOfSightBlocked = Grid.TestLineOfSight(startX, startY, targetX, targetY, blockingCb, visibleBlockingCb, true);
                return lineOfSightBlocked;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[KDEBUG] 检查视线时发生错误: {ex.Message}");
                return false;
            }
        }

        private void ReduceMass()
        {
            // 获取基座上方的物体
            GameObject occupant = receptacle.Occupant;
            if (occupant == null)
            {
                Debug.LogWarning("基座上没有物体，无法减少质量");
                StopMassReduction(); // 停止质量减少
                StopCheckLogic();
                DestroyProgressBar(); // 销毁进度条
                return;
            }

            // 获取物体的 PrimaryElement 组件
            PrimaryElement primaryElement = occupant.GetComponent<PrimaryElement>();
            if (primaryElement == null)
            {
                Debug.LogWarning("基座上方的物体没有 PrimaryElement 组件，无法减少质量");
                StopMassReduction(); // 停止质量减少
                StopCheckLogic();
                DestroyProgressBar(); // 销毁进度条
                return;
            }

            // 减少质量
            float currentMass = primaryElement.Mass;
            float newMass = currentMass - massReductionAmount;

            if (newMass <= 0)
            {
                Debug.Log("物体质量小于等于 0，销毁物体");
                Util.KDestroyGameObject(occupant); // 销毁物体

                StopMassReduction(); // 停止质量减少
                StopCheckLogic();
                DestroyProgressBar(); // 销毁进度条



                //ExplosionHandler explosionHandler = gameObject.AddComponent<ExplosionHandler>();

                //// 配置 ExplosionHandler 的参数
                //explosionHandler.emitInterval = 0.5f;
                //explosionHandler.emitMassPerInterval = 15f;
                //explosionHandler.totalMassToEmit = 300f;
                //explosionHandler.emitVelocity = 40f;
                //explosionHandler.emitTemperature = 6000f;
                //explosionHandler.emitSubstance = SimHashes.NuclearWaste;
                //explosionHandler.cometPrefabID = "NuclearWasteComet";
                //explosionHandler.directionOption = ExplosionHandler.DirectionOption.Random;

                //// 启动爆炸
                //explosionHandler.StartExplosion();





            }
            else
            {
                primaryElement.Mass = newMass; // 更新质量
                Debug.Log("物体质量减少至：" + newMass);
            }
        }

        protected override void OnCleanUp()
        {
            StopMassReduction(); // 清理时停止减少质量
            StopCheckLogic(); // 清理时停止检查逻辑
            DestroyProgressBar(); // 清理时销毁进度条
            base.OnCleanUp();
        }
    }
}