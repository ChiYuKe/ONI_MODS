using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

namespace DebuffRoulette
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

        public static void SaveMinionData(GameObject minion, float currentAgeInSeconds)
        {
            if (minion == null) return;

            int instanceID = minion.GetComponent<KPrefabID>().InstanceID;

            // 读取现有的数据
            List<MinionData> minionDataList = LoadMinionData();

            // 查找是否已有相同的记录
            MinionData existingData = minionDataList.Find(data => data.minionInstanceID == instanceID);

            if (existingData != null)
            {
                // 更新已有记录的 ageInSeconds
                existingData.ageInSeconds = currentAgeInSeconds;
            }
            else
            {
                // 添加新的记录
                MinionData minionData = new MinionData
                {
                    minionInstanceID = instanceID,
                    ageInSeconds = currentAgeInSeconds
                };
                minionDataList.Add(minionData);
            }

            // 保存数据到文件
            SaveToFile(minionDataList);
        }
        public static bool IsAgeDifferenceExceedingThreshold(GameObject minion, float currentAgeInSeconds, float threshold = 600f)
        {
            if (minion == null) return false;

            int instanceID = minion.GetComponent<KPrefabID>().InstanceID;

            // 读取现有的数据
            List<MinionData> minionDataList = LoadMinionData();

            // 查找是否已有相同的记录
            MinionData existingData = minionDataList.Find(data => data.minionInstanceID == instanceID);

            if (existingData != null)
            {
                // 计算误差
                float ageDifference = Mathf.Abs(existingData.ageInSeconds - currentAgeInSeconds);
               
                return ageDifference > threshold;
            }

            return false;
        }

        public static float GetAgeByInstance(GameObject minion)
        {
            if (minion == null) return -1f;

            int instanceID = minion.GetComponent<KPrefabID>().InstanceID;
            // 读取现有的数据
            List<MinionData> minionDataList = LoadMinionData();

            // 查找对应的记录
            MinionData data = minionDataList.Find(d => d.minionInstanceID == instanceID);

            return data != null ? data.ageInSeconds / 600f : -1f / 600f; // 返回 -1 表示未找到
        }

        public static void RemoveMinionData(GameObject minion)
        {
            if (minion == null) return;

            int instanceID = minion.GetComponent<KPrefabID>().InstanceID;

            // 读取现有的数据
            List<MinionData> minionDataList = LoadMinionData();

            // 移除指定的 MinionData
            minionDataList.RemoveAll(data => data.minionInstanceID == instanceID);

            // 保存数据到文件
            SaveToFile(minionDataList);
        }
        public static void RemoveNonMatchingData(GameObject minion)
        {
            if (minion == null) return;

            // 获取传入 minion 的 instanceID
            int instanceID = minion.GetComponent<KPrefabID>().InstanceID;

            // 读取现有的数据
            List<MinionData> minionDataList = LoadMinionData();

            // 过滤出匹配的 MinionData
            List<MinionData> filteredDataList = minionDataList.FindAll(data => data.minionInstanceID == instanceID);

            // 如果存在不匹配的数据
            if (filteredDataList.Count == 0)
            {
               
                // 移除不匹配的数据
                RemoveMinionData(minion);
            }
        }


        private static List<MinionData> LoadMinionData()
        {
            if (!File.Exists(FilePath))
            {
                return new List<MinionData>();
            }

            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<MinionData>>(json) ?? new List<MinionData>();
        }

        private static void SaveToFile(List<MinionData> minionDataList)
        {
            string json = JsonConvert.SerializeObject(minionDataList, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
    }
}
