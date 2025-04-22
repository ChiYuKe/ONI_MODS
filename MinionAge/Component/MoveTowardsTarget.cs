using System.Collections;
using UnityEngine;

public class MoveTowardsTarget : MonoBehaviour
{
    public GameObject targetObject; // 飞行的目标对象
    public float moveSpeed = 5f;     // 飞行速度
    public float arcHeight = 3f;     // 控制飞行的最大高度
    public float minDistance = 3f;   // 最小飞行距离

    private bool isMoving = false;
    private Vector3 targetStartPos; // 目标的初始位置
    private Navigator targetNavigator; // 目标对象的 Navigator 组件
    private KBatchedAnimController animController; // 动画控制器

    private void Start()
    {
        if (targetObject != null)
        {
            // 获取目标对象的 Navigator 组件
            targetNavigator = targetObject.GetComponent<Navigator>();
            // 获取动画控制器
            animController = GetComponent<KBatchedAnimController>();
            StartMoving();
        }
    }

    // 启动飞行
    private void StartMoving()
    {
        if (isMoving)
        {
            return; // 如果正在飞行，则不启动新的飞行
        }

        // 判断目标与当前对象的距离，如果小于最小距离，则不执行飞行
        if (targetObject != null && Vector3.Distance(transform.position, targetObject.transform.position) > minDistance)
        {
            Debug.Log($"开始飞行，目标距离: {Vector3.Distance(transform.position, targetObject.transform.position)}");
            targetStartPos = targetObject.transform.position; // 记录目标的初始位置

            // 暂停目标对象的 Navigator 组件
            if (targetNavigator != null)
            {
                targetNavigator.Pause("Flying");
            }

            StartCoroutine(MoveToTarget());
        }
        else
        {
            Debug.Log("目标距离过近，无法飞行");
        }
    }

    // 飞行协程
    private IEnumerator MoveToTarget()
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 targetPos = targetStartPos; // 飞向目标的初始位置

        // 计算飞行的时间
        float journeyLength = Vector3.Distance(startPos, targetPos);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            // 如果目标对象移动，停止当前飞行
            if (targetObject.transform.position != targetStartPos)
            {
                Debug.Log("目标移动，停止飞行");
                isMoving = false;
                yield break; // 退出协程
            }

            // 计算已经飞行的时间比例
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            // 使用抛物线公式计算高度变化
            float height = Mathf.Sin(fractionOfJourney * Mathf.PI) * arcHeight;

            // 在接近目标点时逐渐减少高度
            if (fractionOfJourney > 0.8f)
            {
                height *= Mathf.Lerp(1f, 0f, (fractionOfJourney - 0.8f) / 0.2f);
            }

            // 计算当前飞行位置
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, fractionOfJourney);
            currentPos.y += height;

            transform.position = currentPos;

            yield return null;
        }

        // 确保物体最终完全到达目标位置
        transform.position = targetPos;
        isMoving = false;

        // 切换到空闲动画
        if (animController != null)
        {
            animController.Play("idle", KAnim.PlayMode.Loop); // 播放空闲动画
        }

        // 恢复目标对象的 Navigator 组件
        if (targetNavigator != null)
        {
            targetNavigator.Unpause("Flying");
        }
    }

    // 重置飞行状态
    public void ResetMove()
    {
        isMoving = false;
        StopAllCoroutines();

        // 切换到空闲动画
        if (animController != null)
        {
            animController.Play("idle", KAnim.PlayMode.Loop); // 播放空闲动画
        }

        // 恢复目标对象的 Navigator 组件
        if (targetNavigator != null)
        {
            targetNavigator.Unpause("Flying");
        }
    }

    // 销毁时清理
    private void OnDestroy()
    {
        ResetMove();
    }
}