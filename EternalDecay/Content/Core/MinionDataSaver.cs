using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace EternalDecay.Content.Core
{
    public static class MinionDataSaver
    {
        [Serializable]
        public class MinionData
        {
            public int minionInstanceID;
            public float ageInSeconds;
        }

        private static readonly string FilePath = Path.Combine(Path.GetDirectoryName(SaveLoader.GetActiveSaveFilePath()), "minionData.json");



        // 新增方法：修改指定 minion 的年龄数据
        public static void UpdateMinionAge(GameObject minion, float newAgeInSeconds)
        {
            if (minion == null)
            {
                Debug.LogWarning("传入的 minion 为 null，无法更新年龄数据");
                return;
            }

            // 获取 minion 的实例 ID
            int instanceID = minion.GetComponent<KPrefabID>().InstanceID;

            // 加载保存的数据
            List<MinionData> minionDataList = LoadMinionData();

            // 查找并更新对应的 minion 数据
            bool found = false;
            foreach (var data in minionDataList)
            {
                if (data.minionInstanceID == instanceID)
                {
                    data.ageInSeconds = newAgeInSeconds;
                    found = true;
                    Debug.Log($"更新 minion (InstanceID: {instanceID}) 的年龄数据为 {newAgeInSeconds} 秒");
                    break;
                }
            }

            // 如果未找到对应的 minion 数据，则添加新数据
            if (!found)
            {
                MinionData newData = new MinionData
                {
                    minionInstanceID = instanceID,
                    ageInSeconds = newAgeInSeconds
                };
                minionDataList.Add(newData);
                //  Debug.Log($"添加 minion (InstanceID: {instanceID}) 的年龄数据为 {newAgeInSeconds} 秒");
            }

            // 保存更新后的数据
            SaveToFile(minionDataList);
        }


        // 同步所有 minion 的数据
        public static void SynchronizeAllMinionData(IEnumerable<GameObject> cachedMinionGameObjects)
        {
            if (cachedMinionGameObjects == null) return;

            // 加载保存的数据
            List<MinionData> minionDataList = LoadMinionData();

            // 用于快速查找保存的数据
            Dictionary<int, MinionData> savedDataMap = new Dictionary<int, MinionData>();
            foreach (var data in minionDataList)
            {
                savedDataMap[data.minionInstanceID] = data;
            }

            // 遍历所有 minion，同步数据
            foreach (GameObject minion in cachedMinionGameObjects)
            {
                if (minion == null) continue;

                int instanceID = minion.GetComponent<KPrefabID>().InstanceID;
                float currentAgeInSeconds = GetCurrentAgeInSeconds(minion);

                if (savedDataMap.TryGetValue(instanceID, out MinionData savedData))
                {
                    // 如果文件中的数据与游戏中的数据不一致，则将文件中的数据同步到游戏
                    if (Mathf.Abs(savedData.ageInSeconds - currentAgeInSeconds) > 650f)
                    {
                        SetAgeInGame(minion, savedData.ageInSeconds);
                        //  Debug.Log($"将文件中的数据同步到游戏：minion (InstanceID: {instanceID})，年龄 = {savedData.ageInSeconds} 秒");
                    }
                    else
                    {
                        // 如果误差未超过阈值，将游戏中的数据保存到文件
                        savedData.ageInSeconds = currentAgeInSeconds;
                        // Debug.Log($"将游戏中的数据保存到文件：minion (InstanceID: {instanceID})，年龄 = {currentAgeInSeconds} 秒");
                    }
                }
                else
                {
                    // 如果保存的数据中没有当前 minion 的数据，则添加新数据
                    MinionData newData = new MinionData
                    {
                        minionInstanceID = instanceID,
                        ageInSeconds = currentAgeInSeconds
                    };
                    minionDataList.Add(newData);
                    // Debug.Log($"添加 minion (InstanceID: {instanceID}) 的年龄数据");
                }
            }

            // 清理无效的数据
            RemoveNonMatchingData(minionDataList, cachedMinionGameObjects);

            // 保存更新后的数据
            SaveToFile(minionDataList);
        }

        // 获取当前 minion 的年龄（以秒为单位）
        public static float GetCurrentAgeInSeconds(GameObject minion)
        {
            var ageInstance = Db.Get().Amounts.Get("AgeAttribute").Lookup(minion);
            return ageInstance != null ? ageInstance.value * 600 : 0f;
        }

        // 将文件中的年龄数据应用到游戏中
        private static void SetAgeInGame(GameObject minion, float ageInSeconds)
        {
            var ageInstance = Db.Get().Amounts.Get("AgeAttribute").Lookup(minion);
            if (ageInstance != null)
            {
                // 将秒转换为游戏中的年龄单位
                float ageInGameUnits = ageInSeconds / 600f;
                ageInstance.SetValue(ageInGameUnits);
            }
        }

        // 移除无效的 MinionData（即保存的数据中没有对应的游戏对象）
        private static void RemoveNonMatchingData(List<MinionData> minionDataList, IEnumerable<GameObject> cachedMinionGameObjects)
        {
            if (minionDataList == null || cachedMinionGameObjects == null) return;

            // 获取当前所有游戏中的 minion 实例 ID
            HashSet<int> activeMinionIDs = new HashSet<int>();
            foreach (GameObject minion in cachedMinionGameObjects)
            {
                if (minion != null)
                {
                    int instanceID = minion.GetComponent<KPrefabID>().InstanceID;
                    activeMinionIDs.Add(instanceID);
                }
            }

            // 移除无效的数据
            int removedCount = minionDataList.RemoveAll(data => !activeMinionIDs.Contains(data.minionInstanceID));

            if (removedCount > 0)
            {
                Debug.Log($"移除 {removedCount} 条无效的 MinionData 数据");
            }
        }

        // 从文件加载数据
        private static List<MinionData> LoadMinionData()
        {
            if (!File.Exists(FilePath))
            {
                return new List<MinionData>();
            }

            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<MinionData>>(json) ?? new List<MinionData>();
        }

        // 保存数据到文件
        private static void SaveToFile(List<MinionData> minionDataList)
        {
            string json = JsonConvert.SerializeObject(minionDataList, Formatting.Indented);
            File.WriteAllText(FilePath, json);
            Debug.Log($"复制人年龄数据已经保存到: {FilePath} ");
        }
    }
}