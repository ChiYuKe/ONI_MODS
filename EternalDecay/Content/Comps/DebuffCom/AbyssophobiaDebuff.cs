using CykUtils;
using UnityEngine;

namespace EternalDecay.Content.Comps.DebuffCom
{
    public class AbyssophobiaDebuff : KMonoBehaviour, ISim4000ms
    {
        // 场景中建筑的 ID（优先使用第一个 ID）
        public string buildingID = "HeadquartersComplete";
        public string buildingID2 = "ExobaseHeadquarters";

        private GameObject building;  // 场景中的某个建筑对象

        // debuff 的基础值、增幅值和阈值
        public float debuffBaseValue = 1f;
        public float debuffMultiplier = 1f;

        // 设定每个 Y 坐标差异区间与对应的 debuff 等级
        private readonly (float threshold, int level)[] debuffThresholds = new (float, int)[]
        {
            (5f, 0),    
            (25f, 1),   
            (45f, 2),   
            (65f, 3),   
            (85f, 4),   
            (105f, 5),  
        };

        private int debuffLevel = 0;  // 当前的 debuff 级别

        protected override void OnSpawn()
        {
            base.OnSpawn();

            building = GetBuildingByID(buildingID) ?? GetBuildingByID(buildingID2);

            if (building == null)
            {
                Debug.LogWarning("无法找到建筑！");
            }
        }

        void ISim4000ms.Sim4000ms(float dt)
        {
            if (building == null || this.gameObject == null) return;

            float targetY = this.gameObject.transform.position.y;
            float buildingY = building.transform.position.y;

            // 计算 Y 坐标差值
            float yDifference = Mathf.Abs(buildingY - targetY);

            // 输出调试信息
            LogUtil.Log($"目标Y坐标: {targetY}, 建筑Y坐标: {buildingY}");
            LogUtil.Log($"Y 坐标差异 (yDifference): {yDifference}");

            // 通过差异找到对应的 debuff 级别
            debuffLevel = GetDebuffLevel(yDifference);

          

            // 应用 debuff
            ApplyAbyssophobiaDebuff(debuffLevel);
        }

        // 通过建筑 ID 查找建筑对象
        private GameObject GetBuildingByID(string buildingID)
        {
            return GameObject.Find(buildingID);  // 根据实际场景修改
        }

        // 计算 debuff 级别
        private int GetDebuffLevel(float yDifference)
        {
            // 遍历预设的阈值和级别
            foreach (var (threshold, level) in debuffThresholds)
            {
                if (yDifference <= threshold)
                {
                    return level;
                }
            }

            // 如果差异大于最大阈值，返回最高级别 5
            return 5;
        }

        // 应用 Abyssophobia debuff
        private void ApplyAbyssophobiaDebuff(float debuffLevel)
        {
            // 示例代码：你可以根据游戏逻辑来应用 debuff
            LogUtil.Log($"目标对象 {this.gameObject.name} 得到的深渊恐惧症 debuff 强度: {debuffLevel}");
        }
    }
}
