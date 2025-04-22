using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ExplosionHandler")]
public class ExplosionHandler : KMonoBehaviour
{
    // 爆炸参数
    public float emitInterval = 0.3f; // 发射碎片的间隔时间（秒）
    public float emitMassPerInterval = 10f; // 每次发射的碎片质量
    public float totalMassToEmit = 200f; // 总共要发射的碎片质量
    public float emitVelocity = 30f; // 碎片的发射速度
    public float emitTemperature = 5000f; // 发射物质的温度
    public SimHashes emitSubstance = SimHashes.NuclearWaste; // 发射的物质类型
    public string cometPrefabID = "NuclearWasteComet"; // 发射的碎片预制体ID

    // 爆炸方向选项
    public enum DirectionOption
    {
        Up,        // 向上
        Down,      // 向下
        Left,      // 向左
        Right,     // 向右
        Random     // 随机方向
    }

    public DirectionOption directionOption = DirectionOption.Random; // 当前方向选项

    // 内部状态
    private float timeSinceLastEmit; // 从上一次发射到现在的时间累积
    private float remainingMassToEmit; // 剩余要发射的碎片质量

    // 初始化爆炸
    public void StartExplosion()
    {
        Debug.Log("开炮开炮!");
        remainingMassToEmit = totalMassToEmit; // 初始化剩余质量
        timeSinceLastEmit = 0f; // 重置时间累积
    }

    // 每帧更新爆炸逻辑
    private void Update()
    {
        // 如果还有剩余质量需要发射，则调用 UpdateExplosion
        if (remainingMassToEmit > 0f)
        {
            UpdateExplosion(Time.deltaTime);
        }
    }

    // 更新爆炸逻辑
    public void UpdateExplosion(float dt)
    {
        // 如果没有剩余质量需要发射，则直接返回
        if (remainingMassToEmit <= 0f)
            return;

        // 累积时间
        timeSinceLastEmit += dt;

        // 如果累积时间超过发射间隔，则发射碎片
        if (timeSinceLastEmit > emitInterval)
        {
            timeSinceLastEmit -= emitInterval; // 重置时间累积
            float massToEmit = Mathf.Min(remainingMassToEmit, emitMassPerInterval); // 计算本次发射的质量
            remainingMassToEmit -= massToEmit; // 减少剩余质量

            EmitFragments(massToEmit); // 发射碎片
        }
    }

    // 发射碎片
    private void EmitFragments(float mass)
    {
        Debug.Log("Emitting fragments...");

        // 发射核废料碎片
        for (int i = 0; i < 3; i++)
        {
            // 如果剩余质量足够发射一个碎片
            if (mass >= NuclearWasteCometConfig.MASS)
            {
                Debug.Log("Creating comet fragment...");

                // 实例化碎片预制体
                GameObject fragment = Util.KInstantiate(Assets.GetPrefab(cometPrefabID), transform.position + Vector3.up * 1f, Quaternion.identity, null, null, true, 0);
                fragment.SetActive(true);

                // 获取 Comet 组件并设置参数
                Comet comet = fragment.GetComponent<Comet>();
                comet.ignoreObstacleForDamage.Set(GetComponent<KPrefabID>()); // 忽略障碍物伤害
                comet.addTiles = 1; // 添加瓦片

                // 根据选定的方向来确定发射角度
                float angle = 0f;
                switch (directionOption)
                {
                    case DirectionOption.Up:
                        angle = 90f;
                        break;
                    case DirectionOption.Down:
                        angle = 270f;
                        break;
                    case DirectionOption.Left:
                        angle = 0f;
                        break;
                    case DirectionOption.Right:
                        angle = 180f;
                        break;
                    case DirectionOption.Random:
                        angle = UnityEngine.Random.Range(0, 360); // 随机角度
                        break;
                }

                float radians = angle * Mathf.PI / 180f;

                // 设置碎片的速度和旋转
                comet.Velocity = new Vector2(-Mathf.Cos(radians) * emitVelocity, Mathf.Sin(radians) * emitVelocity);
                comet.GetComponent<KBatchedAnimController>().Rotation = -angle - 90f;

                // 减少剩余质量
                mass -= NuclearWasteCometConfig.MASS;
            }
        }

        // 发射核废料气体
        for (int j = 0; j < 3; j++)
        {
            // 如果剩余质量足够发射气体
            if (mass >= 0.001f)
            {
                Debug.Log("Emitting nuclear waste gas...");

                // 在指定位置生成核废料气体
                SimMessages.AddRemoveSubstance(
                    Grid.PosToCell(transform.position + Vector3.up * 3f + Vector3.right * j * 2f), // 位置
                    emitSubstance, // 物质类型
                    CellEventLogger.Instance.ElementEmitted, // 事件记录器
                    mass / 3f, // 质量
                    emitTemperature, // 温度
                    Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.Id), // 疾病索引
                    Mathf.RoundToInt(50f * (mass / 3f)), // 疾病数量
                    true, // 是否广播事件
                    -1 // 优先级
                );
            }
        }
    }
}
