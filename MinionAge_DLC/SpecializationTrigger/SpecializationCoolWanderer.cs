using Klei.AI;
using KModTool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MinionAge_DLC
{
    public static class SpecializationCoolWanderer
    {
        // 默认检测半径
        private const int DefaultEmissionRadius = 2;

        // 动画名称
        private static readonly HashedString[] PreAnims = new HashedString[] { "grid_pre", "grid_loop" };
        private static readonly HashedString PostAnim = "grid_pst";

        // 动画延迟和持续时间
        private static readonly float DistanceDelay = 0.25f;
        private static readonly float Duration = 3f;


        /// <summary>
        /// 手动触发扫描逻辑
        /// </summary>
        /// <param name="minion">要扫描的小人对象</param>
        /// <param name="emissionRadius">检测半径（可选，默认为 2）</param>
        public static void TriggerScan(GameObject minion, int emissionRadius = DefaultEmissionRadius)
        {
            if (minion == null)
            {
                Debug.LogError("Minion 对象不能为空！");
                return;
            }
            // 检查对象是否具有目标 Tag和buff
            var prefabID = minion.GetComponent<KPrefabID>();
            // 检查 CoolWanderer Buff 是否存在
            float bufftime = KModDeBuff.GetEffectRemainingTime(minion, "CoolWanderer");
            if (bufftime <= 0 || prefabID.HasTag("Corpse"))
            {
                KModDeBuff.RemoveEffect(minion, "CoolWanderer");
                return;
            }

            // 处理范围内的可拾取对象
            HandleAccessiblePickupables(minion, emissionRadius);
        }

        // 处理范围内的可拾取对象
        private static void HandleAccessiblePickupables(GameObject minion, int emissionRadius)
        {
            if (minion.transform == null) { return; }

            // 获取当前对象位置对应的网格单元
            int centerCell = Grid.PosToCell(minion.transform.GetPosition());

            // 获取声波范围内的所有单元格
            HashSet<int> cellsInRange = GameUtil.CollectCellsBreadthFirst(centerCell, (int cell) => !Grid.Solid[cell], emissionRadius);

            // 绘制声波效果
            DrawVisualEffect(centerCell, cellsInRange);

            // 处理范围内的可拾取对象
            List<ScenePartitionerEntry> pickupableItemsInRange = new List<ScenePartitionerEntry>();
            foreach (int cell in cellsInRange)
            {
                int cellX, cellY;
                Grid.CellToXY(cell, out cellX, out cellY);

                // 收集可拾取层对象
                GameScenePartitioner.Instance.GatherEntries(cellX, cellY, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pickupableItemsInRange);
            }

            // 处理收集到的可拾取对象
            PrimaryElement selfprimaryElement = minion.GetComponent<PrimaryElement>();
            foreach (var entry in pickupableItemsInRange)
            {
                Pickupable pickupable = entry.obj as Pickupable;
                if (pickupable != null && pickupable.gameObject != minion)
                {



                    PrimaryElement primaryElement = pickupable.gameObject.GetComponent<PrimaryElement>();
                    if (primaryElement != null && primaryElement.Temperature >= 273.15f + 0f)
                    {
                        primaryElement.Temperature--;
                    }
                    if (selfprimaryElement != null && selfprimaryElement.Temperature <= 273.15f + 55f)
                    {
                        selfprimaryElement.Temperature++;
                    }
                }
            }
        }

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
            animController.TintColour = new Color32(0, 0, 255, 200);
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
}