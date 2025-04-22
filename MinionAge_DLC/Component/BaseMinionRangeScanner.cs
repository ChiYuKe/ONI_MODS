using HarmonyLib;
using Klei.AI;
using KModTool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExplosionHandler;
using static STRINGS.ELEMENTS;

namespace DebuffRoulette
{

    [AddComponentMenu("KMonoBehaviour/scripts/MinionRangeScanner")]
    public class BaseMinionRangeScanner : KMonoBehaviour
    {
        private Coroutine timerCoroutine;
        private RangeVisualizer rangeVisualizer;

        // 定时器间隔
        public float TimerInterval = 4f; // z执行频率
        public int radius = 10; // 检测半径，最好和 RangeVisualizer 组件的 Range范围对应



        // RangeVisualizer 范围可视化的参数公开
        public Vector2I OriginOffset = new Vector2I(0, 0);
        public bool BlockingTileVisible = true;




        // 当对象生成时启动定时器
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();


            // 订阅事件
            // KModEventManager.Instance.Subscribe("ObjectSelected", OnObjectSelected);

        }



        //private void OnObjectSelected(object data)
        //{
        //    Console.WriteLine("Event triggered with data: " + data);

        //    KModEventManager.Instance.Unsubscribe("ObjectSelected", OnObjectSelected);

        //}





        // 当对象生成时启动定时器
        protected override void OnSpawn()
        {
            base.OnSpawn();
            Console.WriteLine("[KDEBUG] OnSpawn 被调用");

            // 如果 RangeVisualizer 组件尚未添加，才添加它
            rangeVisualizer = gameObject.GetComponent<RangeVisualizer>();


            if (rangeVisualizer == null)
            {

                Vector2I newRangeMin = new Vector2I(OriginOffset.x - radius, OriginOffset.y - radius);
                Vector2I newRangeMax = new Vector2I(OriginOffset.x + radius, OriginOffset.y + radius);

                Console.WriteLine("[KDEBUG] 当前对象上没有 RangeVisualizer，正在添加...");
                rangeVisualizer = gameObject.AddComponent<RangeVisualizer>();

                // 初始化 RangeVisualizer 组件
                rangeVisualizer.OriginOffset = OriginOffset;
                rangeVisualizer.RangeMin = newRangeMin;
                rangeVisualizer.RangeMax = newRangeMax;
                rangeVisualizer.BlockingTileVisible = BlockingTileVisible;

                Console.WriteLine("[KDEBUG] 已为当前对象添加并初始化 RangeVisualizer 组件。");
            }
            else
            {
                Console.WriteLine("[KDEBUG] 当前对象已具有 RangeVisualizer 组件。");
            }

            // 启动定时器协程
            try
            {
                timerCoroutine = StartCoroutine(TimerCoroutine());
                Console.WriteLine("[KDEBUG] 定时器协程已启动");
            }
            catch (System.Exception e)
            {
                Console.WriteLine("[KDEBUG] 启动协程时发生错误: " + e.Message);
            }
        }


        // 定时器协程，每 `TimerInterval` 秒执行一次
        private IEnumerator TimerCoroutine()
        {
            Console.WriteLine("[KDEBUG] 定时器开始...");

            while (true)
            {
                yield return new WaitForSeconds(TimerInterval); // 等待指定的间隔时间
                HandleAccessiblePickupables(); // 执行处理可访问的拾取对象任务
            }
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

                // 收集可拾取层 对象
                GameScenePartitioner.Instance.GatherEntries(cellX, cellY, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pickupableItemsInRange);


            }


            // 处理收集到的可拾取对象
            foreach (var entry in pickupableItemsInRange)
            {
                Pickupable pickupable = entry.obj as Pickupable;
                if (pickupable != null)
                {

                    if (pickupable.gameObject == gameObject)
                    {
                        continue;
                    }

                    Console.WriteLine("[KDEBUG] 找到的对象: " + pickupable.name);




                    PrimaryElement primaryElement = pickupable.gameObject.GetComponent<PrimaryElement>();
                    if (pickupable.gameObject == gameObject) { continue; }

                    if (primaryElement == null) { continue; }

                    if (primaryElement.Temperature > 273.15f + 50f) { continue; }

                    primaryElement.Temperature++;

                    KModEventManager.Instance.TriggerEvent("K_Temperature", pickupable.gameObject);


                }
            }
        }







        // 判断两点之间的视线是否被阻挡
        public static bool IsCellIsVisibledByLineOfSight(int startCell, int targetCell, RangeVisualizer rangeVisualizer)
        {
            try
            {
                int startX;
                int startY;
                Grid.CellToXY(startCell, out startX, out startY);
                int targetX;
                int targetY;
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
                Console.WriteLine($"[KDEBUG] 检查视线时发生错误:  {ex.Message}");
                return false;
            }
        }

        // 如果需要在对象销毁时停止协程
        protected override void OnCleanUp()
        {
            base.OnCleanUp();

            // 停止定时器协程
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                Console.WriteLine("[KDEBUG] 定时器协程已停止");
            }
        }
    }
}
