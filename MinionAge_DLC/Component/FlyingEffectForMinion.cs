using System.Collections;
using UnityEngine;

/// <summary>
/// 专用于复制人的安全飞行组件，带抛物线弧度，自动恢复导航系统状态。
/// </summary>
public class FlyingEffectForMinion : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float arcHeight = 3f;
    public float minYBuffer = 0.5f;

    private bool isFlying = false;
    private Navigator navigator;
    private PathProber pathProber;

    public void FlyTo(Vector3 targetPos, System.Action onArrive = null)
    {
        if (isFlying) return;

        int currentCell = Grid.PosToCell(transform.position);
        int targetCell = Grid.PosToCell(targetPos);
        int gridDistance = Grid.GetCellDistance(currentCell, targetCell);

        if (gridDistance <= 1)
        {
            onArrive?.Invoke();
            return;
        }

        navigator = GetComponent<Navigator>();
        pathProber = GetComponent<PathProber>();

        if (navigator != null)
            navigator.Pause("FlyingEffect");

        StartCoroutine(FlyCoroutine(targetPos, onArrive));
    }

    private IEnumerator FlyCoroutine(Vector3 targetPos, System.Action onArrive)
    {
        isFlying = true;
        Vector3 startPos = transform.position;

        float journeyLength = Vector3.Distance(startPos, targetPos);
        float startTime = Time.time;

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float t = Mathf.Clamp01(distanceCovered / journeyLength);

            float heightOffset = Mathf.Sin(t * Mathf.PI) * arcHeight;
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
            currentPos.y += heightOffset;

            int cell = Grid.PosToCell(currentPos);
            if (Grid.IsValidCell(cell) && Grid.Solid[cell])
            {
                float cellTop = Grid.CellToPos(cell).y + minYBuffer;
                if (currentPos.y < cellTop)
                    currentPos.y = cellTop;
            }

            transform.position = currentPos;
            yield return null;
        }

        transform.position = targetPos;
        isFlying = false;

        yield return new WaitForSeconds(0.3f);

        if (navigator != null)
        {
            navigator.Unpause("FlyingEffect");
            navigator.SetCurrentNavType(NavType.Floor);
        }

        // 检查是否卡住：1秒后位置是否还没动
        Vector3 lastPos = transform.position;
        yield return new WaitForSeconds(1f);

        if ((transform.position - lastPos).sqrMagnitude < 0.1f)
        {
            transform.position = targetPos; // 强制传送
            if (navigator != null)
            {
                navigator.Unpause("FlyingEffect");
                navigator.SetCurrentNavType(NavType.Floor);
            }
        }

        onArrive?.Invoke();

        Destroy(this);
    }
}
