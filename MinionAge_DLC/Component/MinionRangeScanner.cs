//using Klei.AI;
//using KModTool;
//using MinionAge.Tool;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace DebuffRoulette
//{
//    [AddComponentMenu("KMonoBehaviour/scripts/MinionRangeScanner")]
//    public class MinionRangeScanner : KMonoBehaviour
//    {
//        // 定时器间隔
//        public float TimerInterval = 4f; // 执行频率
//        public int EmissionRadius = 2; // 检测半径

//        // RangeVisualizer 范围可视化的参数公开
//        public Vector2I OriginOffset = new Vector2I(0, 0);
//        private bool BlockingTileVisible = true;

//        // 目标 Tag
//        private static readonly Tag TargetTag = TUNINGS.HeatWanderer;

//        private Coroutine timerCoroutine;
//        private RangeVisualizer rangeVisualizer;

//        // 当对象生成时初始化
//        protected override void OnPrefabInit()
//        {
//            base.OnPrefabInit();
           
//        }

//        // 当对象生成时启动定时器
//        protected override void OnSpawn()
//        {
//            base.OnSpawn();
//            try
//            {
//                timerCoroutine = StartCoroutine(TimerCoroutine());
              
//            }
//            catch (System.Exception e)
//            {
//                Debug.Log("启动协程时发生错误: " + e.Message);
//            }
//        }

//        // 检查对象是否具有目标 Tag
//        private bool HasTargetTag()
//        {
//            var prefabID = gameObject.GetComponent<KPrefabID>().HasTag(TargetTag);
//            return prefabID;
//        }



//        // 定时器协程，每 `TimerInterval` 秒执行一次
//        private IEnumerator TimerCoroutine()
//        {
//            while (true)
//            {
//                yield return new WaitForSeconds(TimerInterval); // 等待指定的间隔时间
//                float bufftime = KModDeBuff.GetEffectRemainingTime(gameObject, "HeatWanderer");
//                var prefabID = gameObject.GetComponent<KPrefabID>();
//                // 检查目标
//                if (bufftime > 0 && !prefabID.HasTag("Corpse"))
//                {
//                    HandleAccessiblePickupables();
//                }
//                else { KModDeBuff.RemoveEffect(gameObject, "HeatWanderer");  }
//            }
//        }

//        // 执行定时任务的逻辑，处理所有可见且可访问的可拾取对象
//        private void HandleAccessiblePickupables()
//        {
//            if (transform == null) { return; }

//            // 获取当前对象位置对应的网格单元
//            int centerCell = Grid.PosToCell(transform.GetPosition());

//            // 获取声波范围内的所有单元格
//            HashSet<int> cellsInRange = GameUtil.CollectCellsBreadthFirst(centerCell, (int cell) => !Grid.Solid[cell], EmissionRadius);

//            // 绘制声波效果
//            DrawVisualEffect(centerCell, cellsInRange);

//            // 处理范围内的可拾取对象
//            List<ScenePartitionerEntry> pickupableItemsInRange = new List<ScenePartitionerEntry>();
//            foreach (int cell in cellsInRange)
//            {
//                int cellX, cellY;
//                Grid.CellToXY(cell, out cellX, out cellY);

//                // 收集可拾取层对象
//                GameScenePartitioner.Instance.GatherEntries(cellX, cellY, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pickupableItemsInRange);
//            }

//            // 处理收集到的可拾取对象
//            foreach (var entry in pickupableItemsInRange)
//            {
//                Pickupable pickupable = entry.obj as Pickupable;
//                if (pickupable != null && pickupable.gameObject != gameObject)
//                {
//                    PrimaryElement primaryElement = pickupable.gameObject.GetComponent<PrimaryElement>();
//                    PrimaryElement selfprimaryElement = gameObject.GetComponent<PrimaryElement>();


//                    //枪斗术
//                    Emote radiationItchEmote = Db.Get().Emotes.Minion.Radiation_Glare;

//                    KModMiscellaneous.PerformEmoteActionOnSingleGameObject(gameObject, 10, radiationItchEmote, false);



//                    if (primaryElement != null && primaryElement.Temperature <= 273.15f + 50f)
//                    {
//                        primaryElement.Temperature++;
//                    }
//                    if (selfprimaryElement != null && selfprimaryElement.Temperature >= 273.15f + 15f)
//                    {
//                        selfprimaryElement.Temperature--;
//                    }
//                }
//            }
//        }

//        // 绘制声波效果
//        private void DrawVisualEffect(int centerCell, HashSet<int> cells)
//        {
//            // 播放声波声音
//            // SoundEvent.PlayOneShot(GlobalResources.Instance().AcousticDisturbanceSound, Grid.CellToPos(centerCell), 1f);

//            // 播放声波动画
//            foreach (int cell in cells)
//            {
//                int gridDistance = GetGridDistance(cell, centerCell);
//                GameScheduler.Instance.Schedule("radialgrid_pre", distanceDelay * (float)gridDistance, new Action<object>(SpawnEffect), cell, null);
//            }
//        }

//        // 生成声波动画
//        private void SpawnEffect(object data)
//        {
//            int cell = (int)data;
//            KBatchedAnimController animController = FXHelpers.CreateEffect("radialgrid_kanim", Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront), null, false, Grid.SceneLayer.FXFront, false);
//            // 设置动画颜色为红色 (RGBA: 255, 0, 0, 255)
//            animController.TintColour = new Color32(255, 0, 0, 200);
//            animController.destroyOnAnimComplete = false;
//            animController.Play(PreAnims, KAnim.PlayMode.Loop);
//            GameScheduler.Instance.Schedule("radialgrid_loop", duration, new Action<object>(DestroyEffect), animController, null);
//        }

//        // 销毁声波动画
//        private void DestroyEffect(object data)
//        {
//            KBatchedAnimController animController = (KBatchedAnimController)data;
//            animController.destroyOnAnimComplete = true;
//            animController.Play(PostAnim, KAnim.PlayMode.Once, 1f, 0f);
//        }

//        // 计算两个单元格之间的网格距离
//        private int GetGridDistance(int cell, int centerCell)
//        {
//            Vector2I cellPos = Grid.CellToXY(cell);
//            Vector2I centerPos = Grid.CellToXY(centerCell);
//            return Math.Abs(cellPos.x - centerPos.x) + Math.Abs(cellPos.y - centerPos.y);
//        }

//        // 当对象销毁时清理资源
//        protected override void OnCleanUp()
//        {
//            base.OnCleanUp();

//            // 停止定时器协程
//            var prefabID = gameObject.GetComponent<KPrefabID>();
//            if (timerCoroutine != null && prefabID.HasTag("Corpse"))
//            {
//                StopCoroutine(timerCoroutine);
//                Debug.Log("[KDEBUG] 定时器协程已停止");
//            }

            
//        }

//        // 动画名称
//        private static readonly HashedString[] PreAnims = new HashedString[] { "grid_pre", "grid_loop" };
//        private static readonly HashedString PostAnim = "grid_pst";

//        // 动画延迟和持续时间
//        private static float distanceDelay = 0.25f;
//        private static float duration = 3f;
//    }
//}