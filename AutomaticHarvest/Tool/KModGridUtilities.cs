using System;
using UnityEngine;

namespace KModTool
{
    internal static class KModGridUtilities
    {
        /// <summary>
        /// 检查从起始单元到目标单元的视线是否被阻挡。
        /// </summary>
        /// <param name="startCell">起始网格单元。</param>
        /// <param name="targetCell">目标网格单元。</param>
        /// <param name="rangeVisualizer">范围可视化器，用于提供阻挡检测回调函数。</param>
        /// <returns>如果视线未被阻挡，则返回 true；否则返回 false。</returns>
        public static bool IsLineOfSightBlocked(int startCell, int targetCell, RangeVisualizer rangeVisualizer)
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
                    throw new ArgumentNullException(nameof(rangeVisualizer), "【KMod】 RangeVisualizer is null. Cannot perform line of sight check.");
                }

                Func<int, bool> blockingCb = rangeVisualizer.BlockingCb;
                Func<int, bool> visibleBlockingCb = rangeVisualizer.BlockingVisibleCb ?? ((int i) => rangeVisualizer.BlockingTileVisible);

                bool lineOfSightBlocked = Grid.TestLineOfSight(startX, startY, targetX, targetY, blockingCb, visibleBlockingCb, true);
                return lineOfSightBlocked;
            }
            catch (Exception ex)
            {
                Debug.LogError($"【KMod】An error occurred while checking line of sight:  {ex.Message}");
                return false;
            }
        }
    }
    public static class KGameObjectUtil
    {
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
    }

}