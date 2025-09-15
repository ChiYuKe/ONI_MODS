using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// 位置工具类，用于获取与鼠标相关的世界坐标、格子位置，
/// 以及相机视口的边界等功能。
/// 提供了坐标限制（Clamp）、随机范围偏移、插值曲线等实用方法。
/// </summary>
public static class PosUtil
{
    /// <summary>
    /// 获取鼠标在世界中的位置，并限制在当前世界边界范围内。
    /// </summary>
    /// <returns>限制在世界边界内的鼠标世界坐标</returns>
    public static Vector3 ClampedMouseWorldPos()
    {
        if (Camera.main != null)
        {
            Vector3 vector = Camera.main.ScreenToWorldPoint(PosUtil.ClampedMousePos());
            Vector2 minimumBounds = ClusterManager.Instance.activeWorld.minimumBounds;
            Vector2 maximumBounds = ClusterManager.Instance.activeWorld.maximumBounds;
            return new Vector3(Mathf.Clamp(vector.x, minimumBounds.x, maximumBounds.x), Mathf.Clamp(vector.y, minimumBounds.y, maximumBounds.y), vector.z);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 获取鼠标所在的格子编号，并限制在世界范围内。
    /// </summary>
    /// <returns>鼠标所在格子的 Cell 编号</returns>
    public static int ClampedMouseCell()
    {
        return Grid.PosToCell(PosUtil.ClampedMouseWorldPos());
    }

    /// <summary>
    /// 在指定范围内获取随机偏移后的鼠标位置，并限制在世界范围内。
    /// 如果设置了 <see cref="MouseRangeOverride"/>，则强制使用该范围。
    /// </summary>
    /// <param name="range">随机偏移的半径范围</param>
    /// <returns>带随机偏移并限制后的鼠标世界坐标</returns>
    public static Vector3 ClampedMousePosWithRange(int range)
    {
        if (Camera.main != null)
        {
            if (PosUtil.MouseRangeOverride != null)
            {
                range = PosUtil.MouseRangeOverride.Value;
            }
            Vector3 vector = Camera.main.ScreenToWorldPoint(PosUtil.ClampedMousePos());
            float num = UnityEngine.Random.value * 3.1415927f * 2f;
            Vector3 vector2 = (float)range * Mathf.Sqrt(UnityEngine.Random.value) * new Vector3(Mathf.Cos(num), Mathf.Sin(num), 0f);
            Vector3 vector3 = vector + vector2;
            Vector2 minimumBounds = ClusterManager.Instance.activeWorld.minimumBounds;
            Vector2 maximumBounds = ClusterManager.Instance.activeWorld.maximumBounds;
            return new Vector3(Mathf.Clamp(vector3.x, minimumBounds.x, maximumBounds.x), Mathf.Clamp(vector3.y, minimumBounds.y, maximumBounds.y), vector3.z);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 在指定范围内获取随机偏移后的鼠标所在格子编号。
    /// </summary>
    /// <param name="range">随机偏移的半径范围</param>
    /// <returns>随机偏移后的位置所在的 Cell 编号</returns>
    public static int ClampedMouseCellWithRange(int range)
    {
        return Grid.PosToCell(PosUtil.ClampedMousePosWithRange(range));
    }

    /// <summary>
    /// 获取鼠标附近的一个随机格子编号，范围固定为 5。
    /// </summary>
    /// <returns>随机的鼠标附近格子编号</returns>
    public static int RandomCellNearMouse()
    {
        return PosUtil.ClampedMouseCellWithRange(5);
    }

    /// <summary>
    /// 获取鼠标所在格子的世界坐标。
    /// </summary>
    /// <returns>格子中心的世界坐标</returns>
    public static Vector3 ClampedMouseCellWorldPos()
    {
        return Grid.CellToPos(PosUtil.ClampedMouseCell());
    }

    /// <summary>
    /// 获取相机左下角的世界坐标，并限制在世界边界内。
    /// </summary>
    /// <returns>相机左下角对应的世界坐标</returns>
    public static Vector3 CameraMinWorldPos()
    {
        Camera main = Camera.main;
        if (main != null)
        {
            Ray ray = main.ViewportPointToRay(Vector3.zero);
            Vector2 minimumBounds = ClusterManager.Instance.activeWorld.minimumBounds;
            Vector2 maximumBounds = ClusterManager.Instance.activeWorld.maximumBounds;
            Vector3 point = ray.GetPoint(Mathf.Abs(ray.origin.z / ray.direction.z));
            return new Vector3(Mathf.Clamp(point.x, minimumBounds.x, maximumBounds.x), Mathf.Clamp(point.y, minimumBounds.y, maximumBounds.y), point.z);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 获取相机右上角的世界坐标，并限制在世界边界内。
    /// </summary>
    /// <returns>相机右上角对应的世界坐标</returns>
    public static Vector3 CameraMaxWorldPos()
    {
        Camera main = Camera.main;
        if (main != null)
        {
            Ray ray = main.ViewportPointToRay(Vector3.one);
            Vector2 minimumBounds = ClusterManager.Instance.activeWorld.minimumBounds;
            Vector2 maximumBounds = ClusterManager.Instance.activeWorld.maximumBounds;
            Vector3 point = ray.GetPoint(Mathf.Abs(ray.origin.z / ray.direction.z));
            return new Vector3(Mathf.Clamp(point.x, minimumBounds.x, maximumBounds.x), Mathf.Clamp(point.y, minimumBounds.y, maximumBounds.y), point.z);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 在向量 a 和 b 之间进行平滑的插值（EaseOutBack 曲线）。
    /// </summary>
    /// <param name="a">起始向量</param>
    /// <param name="b">目标向量</param>
    /// <param name="x">插值因子（0~1）</param>
    /// <returns>平滑插值后的向量</returns>
    public static Vector3 EaseOutLerp(Vector3 a, Vector3 b, float x)
    {
        return Vector3.LerpUnclamped(a, b, PosUtil.EaseOutBack(x));
    }

    /// <summary>
    /// 缓出型插值函数（Back 曲线），用于平滑过渡。
    /// </summary>
    /// <param name="x">插值因子（0~1）</param>
    /// <returns>计算后的曲线值</returns>
    public static float EaseOutBack(float x)
    {
        return 1f + 1.8f * Mathf.Pow(x - 1f, 3f) + 0.8f * Mathf.Pow(x - 1f, 2f);
    }

    /// <summary>
    /// 获取限制在屏幕范围内的鼠标屏幕坐标。
    /// </summary>
    /// <returns>限制在屏幕内的鼠标坐标</returns>
    private static Vector3 ClampedMousePos()
    {
        Vector3 mousePos = KInputManager.GetMousePos();
        return new Vector3(Mathf.Clamp(mousePos.x, 0f, (float)Screen.width), Mathf.Clamp(mousePos.y, 0f, (float)Screen.height), mousePos.z);
    }

    /// <summary>
    /// 鼠标随机偏移范围的重写值。
    /// 如果设置了该值，将覆盖传入的范围参数。
    /// </summary>
    public static int? MouseRangeOverride;
}
