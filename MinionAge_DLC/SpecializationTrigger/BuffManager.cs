using Database;
using Klei.AI;
using KModTool;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MinionAge_DLC
{
   
    internal class KModDeBuff
    {

        // 添加负面效果
        public static void ApplyDebuff(GameObject gameObject, string BuffName)
        {
            Effects effectsComponent = gameObject.GetComponent<Effects>();

            effectsComponent.GetTimeLimitedEffects();
            if (effectsComponent != null && !effectsComponent.HasEffect(BuffName)) // 如果没有当前debuff
            {
                effectsComponent.Add(BuffName, true); // 添加效果
               
            }
        }



        public static float GetEffectRemainingTime(GameObject gameObject, string effect_id)
        {
            // 获取 Effects 组件
            Effects effectsComponent = gameObject.GetComponent<Effects>();

            // 如果没有找到 Effects 组件，返回 -1
            if (effectsComponent == null)
            {
                return -1f;
            }

            // 获取目标效果实例
            EffectInstance targetEffect = effectsComponent.Get(effect_id);

            // 如果没有找到效果实例，返回 -1
            if (targetEffect == null)
            {
                return -1f;
            }
           
            return targetEffect.timeRemaining;
        }


        public static void RemoveEffect(GameObject gameObject, string effect_id)
        {
            // 获取 Effects 组件
            Effects effectsComponent = gameObject.GetComponent<Effects>();

            // 如果没有找到 Effects 组件，返回
            if (effectsComponent == null)
            {
                return;
            }

            // 使用 effect_id 移除效果
            effectsComponent.Remove(effect_id);
        }
























        public static void ApplyRandomDebuff(HashSet<GameObject> cachedMinionGameObjects)
        {
            List<string> debuffTypes = new List<string> { "debuff1", "debuff2", "debuff3", "debuff4" };
            int minionCount = cachedMinionGameObjects.Count;
            int numToSelect = 3;

            if (minionCount >= numToSelect)
            {
                // 打乱小人列表，随机选择前 numToSelect 个小人
                List<GameObject> selectedMinions = cachedMinionGameObjects.OrderBy(x => UnityEngine.Random.value).Take(numToSelect).ToList();

                foreach (GameObject gameObject in selectedMinions)
                {
                    if (gameObject == null) continue;

                    Klei.AI.Effects effectsComponent = gameObject.GetComponent<Klei.AI.Effects>();
                    if (effectsComponent == null) continue;

                    // 随机整一个 Debuff给小人
                    string randomDebuff = debuffTypes[UnityEngine.Random.Range(0, debuffTypes.Count)];

                    if (!effectsComponent.HasEffect(randomDebuff))
                    {
                        effectsComponent.Add(randomDebuff, true);
                        NotifyDebuffApplied1(gameObject); // 通知玩家谁被添加了DeBuff

                    }
                }
            }
            else
            {
                Debug.Log("复制人数不足3人，不添加 Debuff。");
            }
        }


        private static void NotifyDebuffApplied1(GameObject gameObject)
        {
            Notifier notifier = gameObject.AddOrGet<Notifier>();
            Notification notification = new Notification(
                MinionAge_DLC.STRINGS.MISC.NOTIFICATIONS.DEBUFFROULETTE.NAME,
                NotificationType.BadMinor,
                (notificationList, data) => notificationList.ReduceMessages(false),
                "/t• " + gameObject.GetProperName(), true, 0f, null, null, null, true, false, false
            );
            notifier.Add(notification, "");
        }


       

    }
}