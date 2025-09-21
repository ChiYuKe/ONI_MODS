using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CykUtils;
using EternalDecay.Content.Configs;
using EternalDecay.Content.Core;
using EternalDecay.Content.Patches;
using Klei.AI;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EternalDecay.Content.Configs.STRINGS.DUPLICANTS.MODIFIERS;
using static EternalDecay.Content.Patches.MinionPatch;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

namespace EternalDecay.Content.Core
{
    internal class EternalDecayMain : KMonoBehaviour, ISim4000ms
    {
        private bool _subscribed = false;
        // 复制人年龄相关
        public static float MinionAgeThreshold = Configs.TUNINGS.AGE.MINION_AGE_THRESHOLD; // 复制人年龄阈值（单位：周期）
        private static readonly float AgeThreshold = MinionAgeThreshold * 600f; // 年龄阈值（秒）
        private static readonly float Age80PercentThreshold = AgeThreshold * Configs.TUNINGS.AGE.AGE_80PERCENT_THRESHOLD; // 年龄80%阈值
       


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
           
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();

        }


        public void Sim4000ms(float dt)
        {
            MinionEventManager.Initialize();
            CheckMinionAges();

        }





        private static void CheckMinionAges()
        {
            var minionList = MinionEventManager.MinionCache;
            foreach (var minionGO in minionList)
            {
                var ageInstance = Db.Get().Amounts.Get("AgeAttribute").Lookup(minionGO);
                if (ageInstance == null) continue;

                float currentAgeInSeconds = ageInstance.value * 600;
                // 处理死亡
                if (currentAgeInSeconds >= AgeThreshold)
                {
                    HandleDeath(minionGO);
                    KEffects.RemoveBuff(minionGO, KEffects.ETERNALDECAY_SHUAILAO);
                    MinionDataTransfer.GenerateNewObject(minionGO, minionGO.transform.position);

                }
                // 处理衰老
                else if (currentAgeInSeconds >= Age80PercentThreshold)
                {
                    KEffects.ApplyBuff(minionGO, KEffects.ETERNALDECAY_SHUAILAO);
                    NotifyDeathApplied(minionGO);

                }
            }
        }

        private static void HandleDeath(GameObject minionGO) 
        {
           

            if (minionGO == null) return;

            var deathMonitor = minionGO.GetSMI<DeathMonitor.Instance>();
            if (deathMonitor != null)
            {
                minionGO.AddOrGet<KPrefabID>().AddTag(KGameTags.NoMourning, true);
                minionGO.AddOrGet<KPrefabID>().AddTag("DieOfOldAge", true);
                deathMonitor.Kill(DeathsPatch.KDeaths.Aging);

                
            }
        }



        






        // 通知衰老效果应用
        public static void NotifyDeathApplied(GameObject gameObject)
        {
            Notifier notifier = gameObject.AddOrGet<Notifier>();
            Notification notification = new Notification(
                Configs.STRINGS.MISC.NOTIFICATIONS.DEATHROULETTE.NAME, // 通知标题
                NotificationType.BadMinor, // 通知类型
                (notificationList, data) => notificationList.ReduceMessages(false), // 通知处理函数
                "/t• " + gameObject.GetProperName(), // 通知内容
                true, 0f, null, null, null, true, false, false
            );
            notifier.Add(notification, ""); // 添加通知
        }



        //// 遍历所有复制人
        //foreach (var minionGO in KModMinionUtils.GetAllMinionGameObjects())
        //{
        //    // 获取 MinionIdentity（可选）
        //    var identity = minionGO.GetComponent<MinionIdentity>();
        //    if (identity != null)
        //    {
        //        // 生成 PopFX 在复制人位置
        //        var popfx = PopFXManager.Instance.SpawnFX(
        //            Assets.GetSprite("crew_state_angry"),       // 图标
        //            GameUtil.GetFormattedPercent(1, GameUtil.TimeSlice.None), // 文本
        //            minionGO.transform,                          // 复制人位置
        //            1.5f,                                        // 显示时长
        //            false                                        // 是否跟随物体移动
        //        );

        //        // 调整大小
        //        if (popfx != null)
        //        {
        //            popfx.transform.localPosition += new Vector3(0f, 1.5f, 0f);
        //        }

        //    }
        //}




    }
}
