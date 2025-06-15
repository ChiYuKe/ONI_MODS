using UnityEngine;
using System.Collections;
using System;

public class ParabolicMover
{
    /// <summary>
    /// 通过抛物线运动让物体从当前坐标移动到目标坐标。
    /// </summary>
    /// <param name="movingObject">要移动的物体（GameObject）。</param>
    /// <param name="targetPos">目标位置（Vector3）。物体将飞到这个位置。</param>
    /// <param name="timeToReachTarget">飞行持续时间（float），默认值为 1 秒。</param>
    /// <param name="height">抛物线的最大高度（float），默认值为 2 米。</param>
    /// <returns>一个 IEnumerator，可以用于协程。</returns>
    public static IEnumerator ParabolicMoveCoroutine(GameObject movingObject, Vector3 targetPos, float timeToReachTarget = 1f, float height = 2f)
    {
        // 检查传入的物体是否为 null
        if (movingObject == null)
        {
            throw new ArgumentNullException(nameof(movingObject), "移动的物体为 null，无法执行飞行动画！");
        }

        Transform objectTransform = movingObject.transform;
        if (objectTransform == null)
        {
            throw new ArgumentNullException(nameof(objectTransform), "物体的 Transform 组件为 null！");
        }

        // 获取物体的初始位置
        Vector3 startPos = objectTransform.position;
        float elapsedTime = 0f; // 已经过的时间

        // 开始抛物线运动
        while (elapsedTime < timeToReachTarget)
        {
            // 检查物体是否还存在
            if (movingObject == null || objectTransform == null)
            {
                throw new ArgumentNullException(nameof(movingObject), " 飞行过程中物体对象消失了，停止飞行！");
            }

            // 累加经过的时间
            elapsedTime += Time.deltaTime;
            float fraction = elapsedTime / timeToReachTarget; // 计算飞行的进度

            // 使用正弦函数来模拟抛物线的高度变化
            float heightOffset = Mathf.Sin(fraction * Mathf.PI) * height;

            // 计算物体水平的移动（从起始位置到目标位置）
            Vector3 horizontalMovement = Vector3.Lerp(startPos, targetPos, fraction);

            // 将水平移动与垂直高度组合，更新物体的位置
            objectTransform.position = new Vector3(horizontalMovement.x, startPos.y + heightOffset, horizontalMovement.z);

            yield return null; // 等待下一帧
        }

        // 确保物体最终到达目标位置
        movingObject.transform.position = targetPos;
    }
}
