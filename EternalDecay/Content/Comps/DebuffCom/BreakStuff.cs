using System;
using System.Collections.Generic;
using UnityEngine;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;
using Random = System.Random;

namespace EternalDecay.Content.Comps.DebuffCom
{
    public class BreakStuff
    {
        private CellVisibility cellVisibility;
        private Random random;
        private float eventTriggerProbability;

        public BreakStuff(float eventTriggerProbability = 0.5f)
        {
            this.cellVisibility = new CellVisibility();
            this.random = new Random();
            this.eventTriggerProbability = eventTriggerProbability;
        }

        public void TriggerScan(GameObject minion)
        {
            cellVisibility = new CellVisibility();
            int minionCell = Grid.PosToCell(minion);
            CheckAndTriggerMinionEvent(minionCell, minion);
        }

        private void CheckAndTriggerMinionEvent(int minionCell, GameObject minion)
        {
            if (!cellVisibility.IsVisible(minionCell))
            {
                if (random.NextDouble() < eventTriggerProbability)
                {
                    TriggerEvent(minionCell, minion);
                }
            }
        }

        private void TriggerEvent(int minionCell, GameObject minion)
        {

            var choreProvider = minion.GetComponent<ChoreProvider>();
            if (choreProvider != null)
            {
                // 创建并启动攻击行为
                var kaggressiveChore = new KAggressiveChore(choreProvider, null);
                kaggressiveChore.smi.StartSM(); 
            }

            // 触发特效
            // GameUtil.KInstantiate(Assets.GetPrefab("MyCustomEffect"), minion.transform.position, Grid.SceneLayer.FXFront, null, 0).SetActive(true);

            // 执行爆炸
            // ExplosionUtil.Explode(minionCell, power: 1f, explosionSpeedRange: new Vector2(3f, 6f), targetTiles: MakeCircleShape());

            // 直接让附近建筑受损
            // DamageNearbyBuildings(minionCell, radius: 5, damage: 10);
        }



















        // 对附近建筑造成伤害
        private void DamageNearbyBuildings(int centerCell, int radius, int damage)
        {
            var offsets = MakeCircleShape(radius);
            foreach (var offset in offsets)
            {
                int cell = Grid.OffsetCell(centerCell, offset);
                if (!Grid.IsValidCell(cell)) continue;

                // 检查该格子上是否有建筑
                GameObject building = Grid.Objects[cell, (int)ObjectLayer.Building];
                if (building == null) continue;

                var hp = building.GetComponent<BuildingHP>();
                if (hp != null && !hp.invincible)
                {
                    // 触发受损事件
                    building.Trigger(-794517298, new BuildingHP.DamageSourceInfo
                    {
                        damage = damage,
                        source = "愤怒的复制人",
                        popString = "发脾气伤害"
                    });
                }
            }
        }

        // 球形范围
        public static List<CellOffset> MakeCircleShape(int radius = 5)
        {
            var offsets = new List<CellOffset>();
            int r2 = radius * radius;
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    if (x * x + y * y <= r2)
                        offsets.Add(new CellOffset(x, y));
                }
            }
            return offsets;
        }
    }
}
