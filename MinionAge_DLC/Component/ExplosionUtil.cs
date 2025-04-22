using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MinionAge_DLC
{
    public class ExplosionUtil
    {
        
        /// <summary>
        /// 在指定单元格触发爆炸，并对目标单元格造成伤害。
        /// </summary>
        /// <param name="cell">爆炸的中心单元格。</param>
        /// <param name="power">爆炸的威力。</param>
        /// <param name="explosionSpeedRange">碎片飞散的速度范围。</param>
        /// <param name="targetTiles">目标单元格的偏移列表。</param>
        public static void Explode(int cell, float power, Vector2 explosionSpeedRange, List<CellOffset> targetTiles)
        {
            ExplosionUtil.SendDebrisFlying(cell, explosionSpeedRange); // 让碎片飞散
            ExplosionUtil.DamageTiles(cell, targetTiles, power);       // 对目标单元格造成伤害
        }

        /// <summary>
        /// 重新映射值到新的范围。
        /// </summary>
        /// <param name="value">需要重新映射的值。</param>
        /// <param name="inMin">输入范围的最小值。</param>
        /// <param name="inMax">输入范围的最大值。</param>
        /// <param name="outMin">输出范围的最小值。</param>
        /// <param name="outMax">输出范围的最大值。</param>
        /// <returns>重新映射后的值。</returns>
        private static float Remap(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
        }

        
        /// <summary>
        /// 在指定单元格触发爆炸，并对爆炸半径内的单元格造成伤害。
        /// </summary>
        /// <param name="cell">爆炸的中心单元格。</param>
        /// <param name="power">爆炸的威力。</param>
        /// <param name="explosionSpeedRange">碎片飞散的速度范围。</param>
        /// <param name="radius">爆炸的半径。</param>
        /// <returns>被完全破坏的单元格列表。</returns>
        public static List<int> ExplodeInRadius(int cell, float power, Vector2 explosionSpeedRange, int radius)
        {
            // 获取爆炸范围内的单元格
            List<Vector2I> filledCircle = global::ProcGen.Util.GetFilledCircle(Grid.CellToPosCCC(cell, Grid.SceneLayer.Building), (float)radius);
            Vector3 vector = Grid.CellToPos(cell);
            List<int> list = new List<int>();

            // 对爆炸范围内的单元格造成伤害
            foreach (Vector2I vector2I in filledCircle)
            {
                int num = Grid.PosToCell(vector2I);
                if (Grid.IsValidCell(num))
                {
                    float num2 = Vector2.Distance(vector, vector2I); // 计算距离
                    float num3 = 1f - num2 / (float)radius;         // 计算伤害衰减
                    float num4 = power * ExplosionUtil.Remap(num3, 0f, 0.7f, 0f, 1f); // 重新映射伤害
                    num4 = Mathf.Clamp01(num4);                     // 限制伤害范围
                    WorldDamage.Instance.ApplyDamage(num, num4, -1, null, null); // 应用伤害
                    if (num4 >= 1f)
                    {
                        list.Add(num); // 记录被完全破坏的单元格
                    }
                }
            }

            // 对爆炸中心单元格造成伤害
            WorldDamage.Instance.ApplyDamage(cell, 1f, -1, null, null);
            ExplosionUtil.SendDebrisFlying(cell, explosionSpeedRange); // 让碎片飞散
            return list;
        }

       
        /// <summary>
        /// 对指定单元格列表造成伤害。
        /// </summary>
        /// <param name="impactCell">爆炸的中心单元格。</param>
        /// <param name="targetTiles">目标单元格的位置列表。</param>
        /// <param name="power">伤害的威力。</param>
        private static void DamageTiles(int impactCell, List<Vector2> targetTiles, float power)
        {
            foreach (Vector2 vector in targetTiles)
            {
                int num = Grid.PosToCell(vector);
                if (Grid.IsValidCell(num))
                {
                    WorldDamage.Instance.ApplyDamage(num, power, -1, null, null); // 对单元格造成伤害
                }
            }
            WorldDamage.Instance.ApplyDamage(impactCell, 1f, -1, null, null); // 对爆炸中心单元格造成伤害
        }

       
        /// <summary>
        /// 对指定单元格列表造成伤害。
        /// </summary>
        /// <param name="impactCell">爆炸的中心单元格。</param>
        /// <param name="targetTiles">目标单元格的偏移列表。</param>
        /// <param name="power">伤害的威力。</param>
        private static void DamageTiles(int impactCell, List<CellOffset> targetTiles, float power)
        {
            foreach (CellOffset cellOffset in targetTiles)
            {
                int num = Grid.OffsetCell(impactCell, cellOffset);
                if (Grid.IsValidCell(num))
                {
                    WorldDamage.Instance.ApplyDamage(num, power, -1, null, null); // 对单元格造成伤害
                }
            }
            WorldDamage.Instance.ApplyDamage(impactCell, 1f, -1, null, null); // 对爆炸中心单元格造成伤害
        }

       
        /// <summary>
        /// 让爆炸范围内的碎片飞散。
        /// </summary>
        /// <param name="cell">爆炸的中心单元格。</param>
        /// <param name="explosionSpeedRange">碎片飞散的速度范围。</param>
        private static void SendDebrisFlying(int cell, Vector2 explosionSpeedRange)
        {
            Vector3 vector = Grid.CellToPos(cell);
            ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
            GameScenePartitioner.Instance.GatherEntries((int)vector.x - 2, (int)vector.y - 2, 4, 4, GameScenePartitioner.Instance.pickupablesLayer, pooledList);

            // 让爆炸范围内的碎片飞散
            foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
            {
                GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
                if (gameObject.GetComponent<Navigator>() == null)
                {
                    Vector2 normalized = (gameObject.transform.GetPosition() - vector).normalized;
                    Vector2 vector2 = (normalized + new Vector2(0f, 0.55f)) * (0.5f * UnityEngine.Random.Range(explosionSpeedRange.x, explosionSpeedRange.y));
                    if (GameComps.Fallers.Has(gameObject))
                    {
                        GameComps.Fallers.Remove(gameObject);
                    }
                    if (GameComps.Gravities.Has(gameObject))
                    {
                        GameComps.Gravities.Remove(gameObject);
                    }
                    GameComps.Fallers.Add(gameObject, vector2); // 添加碎片飞散效果
                }
            }
            pooledList.Recycle();
        }
    }
}