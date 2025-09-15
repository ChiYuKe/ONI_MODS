using System;
using System.Collections.Generic;
using UnityEngine;
using Klei.AI;
using CykUtils;

namespace EternalDecay.Content.Core
{
    public static class MinionEventManager
    {
        private static bool _initialized = false;
        private static readonly List<GameObject> _minionCache = new List<GameObject>();

        public static IReadOnlyList<GameObject> MinionCache => _minionCache;

        public static void Initialize()
        {
            if (_initialized) return;

            if (GameClock.Instance == null)
            {
                LogUtil.Log("GameClock 未就绪，延迟初始化");
                return;
            }

            // 订阅夜晚事件
            GameClock.Instance.Subscribe((int)GameHashes.Nighttime, OnNight);

            // 初始化缓存
            foreach (var minionGO in KModMinionUtils.GetAllMinionGameObjects())
            {
                AddMinion(minionGO);
            }

            // 新复制人生成时订阅
            Components.LiveMinionIdentities.OnAdd += identity =>
            {
                if (identity != null)
                {
                    var prefabID = identity.GetComponent<KPrefabID>();
                    // 跳过带有 Bionic 标签的对象
                    if (prefabID == null || !prefabID.HasTag(GameTags.Minions.Models.Bionic))
                    {
                        AddMinion(identity.gameObject);
                        OnNight(null); // 立即同步新复制人的数据

                    }
                }
            };

            // 复制人死亡时移除
            Components.LiveMinionIdentities.OnRemove += identity =>
            {
                OnNight(null);
                RemoveMinion(identity.gameObject);
                
            };

            _initialized = true;
            LogUtil.Log("初始化完成，已缓存所有复制人");
        }

        public static void OnNight(object obj)
        {
            // 访问缓存的复制人列表
            var minionList = MinionEventManager.MinionCache;
            try
            {
                MinionDataSaver.SynchronizeAllMinionData(minionList);
            }
            catch (Exception ex)
            {
                Debug.LogError($"数据同步失败: {ex.Message}");
            }
        }

        private static void AddMinion(GameObject minionGO)
        {
            if (minionGO == null || _minionCache.Contains(minionGO)) return;

            _minionCache.Add(minionGO);
            var identity = minionGO.GetComponent<MinionIdentity>();
           
            LogUtil.Log($"添加复制人并订阅事件: {identity?.GetProperName()}");
        }

        private static void RemoveMinion(GameObject minionGO)
        {
            if (minionGO == null) return;
            _minionCache.Remove(minionGO);
            LogUtil.Log("移除复制人缓存: " + minionGO.name);
        }

    }
}
