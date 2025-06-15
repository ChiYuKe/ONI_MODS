using System;
using System.Collections;
using UnityEngine;

public class LineOfSightUtils
{
    /// <summary>
    /// 检查两点之间的视线是否被阻挡，考虑了可见性和阻挡条件。
    /// </summary>
    /// <param name="startCell">起始单元格（表示为网格索引）。</param>
    /// <param name="targetCell">目标单元格（表示为网格索引）。</param>
    /// <param name="rangeVisualizer">用于获取阻挡条件的 RangeVisualizer 实例。</param>
    /// <returns>如果视线被阻挡，则返回 true；否则返回 false。</returns>
    public static bool IsLineOfSightBlocked(int startCell, int targetCell, RangeVisualizer rangeVisualizer)
    {
        try
        {
            // 将网格单元格索引转换为 x 和 y 坐标
            int startX, startY, targetX, targetY;
            Grid.CellToXY(startCell, out startX, out startY);
            Grid.CellToXY(targetCell, out targetX, out targetY);

            // 检查 rangeVisualizer 是否为 null
            if (rangeVisualizer == null)
            {
                throw new ArgumentNullException(nameof(rangeVisualizer), "RangeVisualizer 为 null，无法进行视线检查。");
            }

            // 获取常规阻挡和可见阻挡的回调函数
            Func<int, bool> blockingCb = rangeVisualizer.BlockingCb;
            Func<int, bool> visibleBlockingCb = rangeVisualizer.BlockingVisibleCb ?? ((int i) => rangeVisualizer.BlockingTileVisible);

            // 执行视线阻挡检查
            bool lineOfSightBlocked = !Grid.TestLineOfSight(startX, startY, targetX, targetY, blockingCb, visibleBlockingCb, true);
            return lineOfSightBlocked;
        }
        catch (Exception ex)
        {
            // 捕获异常并记录错误信息
            Debug.LogError($"检查视线时发生错误: {ex.Message}");
            return false;
        }
    }
}
