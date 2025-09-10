using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using CykUtils;
using EternalDecay.Content.Core.Utils;
using EternalDecay.Content.Patches;
using Klei.AI;
using MinionAge.Core;
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
        public static float MinionAgeThreshold = 3f; // 复制人年龄阈值（单位：周期）
        private static readonly float AgeThreshold = MinionAgeThreshold * 600f; // 年龄阈值（秒）
        private static readonly float Age80PercentThreshold = AgeThreshold * 0.8f; // 年龄80%阈值
        public static readonly float DebuffTimeThreshold = AgeThreshold - Age80PercentThreshold; // 衰老效果阈值




        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            LogUtil.Log($"[{this.GetType().Name}] 初始化完成");

        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
           
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            LogUtil.Log("");
        }


        public void Sim4000ms(float dt)
        {
            // EventSubscriber.SubscribeAllEvents();
            // 事件订阅
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
                }
            }
        }

        private static void HandleDeath(GameObject minionGO) 
        {
            if (minionGO == null) return;

            var deathMonitor = minionGO.GetSMI<DeathMonitor.Instance>();
            if (deathMonitor != null)
            {
                minionGO.AddOrGet<KPrefabID>().AddTag("NoMourning", true);
                minionGO.AddOrGet<KPrefabID>().AddTag("DieOfOldAge", true);
                deathMonitor.Kill(DeathsPatch.KDeaths.Aging);

                
            }
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
