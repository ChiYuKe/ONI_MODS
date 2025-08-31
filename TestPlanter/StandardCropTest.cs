using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlanter
{
    public class StandardCropTest : StateMachineComponent<StandardCropTest.StatesInstance>
    {

        protected override void OnSpawn()
        {
            base.OnSpawn();
            base.smi.StartSM();
        }




        public class AnimSet
        {
            public string pre_grow;

            public string grow;

            public string grow_pst;

            public string idle_full;

            public string wilt_base;

            public string harvest;

            public string waning;

            public KAnim.PlayMode grow_playmode = KAnim.PlayMode.Paused;

            private string[] m_wilt;

            public void ClearWiltLevelCache()
            {
                m_wilt = null;
            }

            public string GetWiltLevel(int level)
            {
                if (m_wilt == null)
                {
                    m_wilt = new string[3];
                    for (int i = 0; i < 3; i++)
                    {
                        m_wilt[i] = wilt_base + (i + 1);
                    }
                }

                return m_wilt[level - 1];
            }

            public AnimSet()
            {
            }

            public AnimSet(AnimSet template)
            {
                pre_grow = template.pre_grow;
                grow = template.grow;
                grow_pst = template.grow_pst;
                idle_full = template.idle_full;
                wilt_base = template.wilt_base;
                harvest = template.harvest;
                waning = template.waning;
                grow_playmode = template.grow_playmode;
            }
        }





        public class States : GameStateMachine<States, StatesInstance, StandardCropTest>
        {
            public class AliveStates : PlantAliveSubState
            {
                public State pre_idle;

                public State idle;

                public State pre_fruiting;

                public State fruiting_lost;

                public State barren;

                public State fruiting;

                public State wilting;

                public State destroy;

                public State harvest;
            }

            public AliveStates alive;

            public State dead;

            public PlantAliveSubState blighted;

            public override void InitializeStates(out BaseState default_state)
            {
                base.serializable = SerializeType.Both_DEPRECATED; // 序列化类型
                default_state = alive;




                // 植物死亡
                dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead)
                    .Enter(delegate (StatesInstance smi)
                    {
                        if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
                        {
                            Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
                            Notification notification = smi.master.CreateDeathNotification();
                            notifier.Add(notification);
                        }

                        // 播放植物死亡特效
                        GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(value: true);


                        // 如果植物可以被收获且游戏调度器存在，则调度收获
                        Harvestable component = smi.master.GetComponent<Harvestable>();
                        if (component != null && component.CanBeHarvested && GameScheduler.Instance != null)
                        {
                            // 延迟0.2秒后生成果实
                            GameScheduler.Instance.Schedule("TestFood", 0.2f, smi.master.crop.SpawnConfiguredFruit);
                        }

                        // 发送植物死亡事件
                        smi.master.Trigger(1623392196);
                        // 停止并清除动画控制器
                        smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
                        // 销毁植物的动画控制器组件
                        UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
                        // 销毁植物的作物组件
                        smi.Schedule(0.5f, smi.master.DestroySelf);
                    });

                // 枯萎 waning动画状态
                blighted.InitializeStates(masterTarget, dead)
                        .PlayAnim((StatesInstance smi) => smi.master.anims.waning)
                        .ToggleMainStatusItem(Db.Get().CreatureStatusItems.Crop_Blighted) // 显示“枯萎”提示 UI
                        .TagTransition(GameTags.Blighted, alive, on_remove: true); // 当植物不再枯萎时，切换回alive状态
                      

                // 默认状态
                alive.InitializeStates(masterTarget, dead)
                     .DefaultState(alive.pre_idle) // 默认进入 pre_idle 状态
                     .ToggleComponent<Growing>() // 添加生长组件
                     .TagTransition(GameTags.Blighted, blighted); // 如果有 Blighted 就切换到枯萎状态

                // 控制植物从“刚种下”进入正常生长状态前的过渡动画行为。
                alive.pre_idle.EnterTransition(alive.idle, (StatesInstance smi) => smi.master.anims.pre_grow == null)
                     .PlayAnim((StatesInstance smi) => smi.master.anims.pre_grow)
                     .OnAnimQueueComplete(alive.idle)
                     .ScheduleGoTo(8f, alive.idle);

                // 控制植物的“正常生长”状态。
                alive.idle.EventTransition(GameHashes.Wilt, alive.wilting, (StatesInstance smi) => smi.master.wiltCondition.IsWilting())
                     .EventTransition(GameHashes.Grow, alive.pre_fruiting, (StatesInstance smi) => smi.master.growing.ReachedNextHarvest())
                     .PlayAnim((StatesInstance smi) => smi.master.anims.grow, (StatesInstance smi) => smi.master.anims.grow_playmode)
                     .Enter(RefreshPositionPercent) 
                     .Update(RefreshPositionPercent, UpdateRate.SIM_4000ms)
                     .EventHandler(GameHashes.ConsumePlant, RefreshPositionPercent);

                // 控制植物从“成熟”进入“结果”状态前的过渡动画行为。
                alive.pre_fruiting
                     .PlayAnim((StatesInstance smi) => smi.master.anims.grow_pst)   // 播放“生长结束”动画（grow_pst）
                     // .TriggerOnEnter(GameHashes.BurstEmitDisease)                   // 进入该状态时触发疾病爆发事件,这里不需要，因为这个测试植物没加病毒组件
                     .EventTransition(GameHashes.AnimQueueComplete, alive.fruiting) // 动画播完后切换到“结果”状态（alive.fruiting）
                     .EventTransition(GameHashes.Wilt, alive.wilting)               // 如果收到枯萎事件，切换到枯萎状态（alive.wilting）
                     .ScheduleGoTo(8f, alive.fruiting);                             // 8秒后自动切换到“结果”状态（alive.fruiting）

               
                alive.fruiting_lost.Enter(delegate (StatesInstance smi)
                    {
                        if (smi.master.harvestable != null)
                        {
                            smi.master.harvestable.SetCanBeHarvested(state: false); // 设置植物不可收获
                        }
                    }).GoTo(alive.idle); // 进入该状态后，立即切换回“正常生长”状态（alive.idle）


                // 控制植物的“枯萎”状态。
                alive.wilting.PlayAnim(GetWiltAnim, KAnim.PlayMode.Loop)
                     .EventTransition(GameHashes.WiltRecover, alive.idle, (StatesInstance smi) => !smi.master.wiltCondition.IsWilting())
                     .EventTransition(GameHashes.Harvest, alive.harvest);


                // 控制植物的“成熟”状态。
                alive.fruiting.PlayAnim((StatesInstance smi) => smi.master.anims.idle_full, KAnim.PlayMode.Loop) // 循环播放完全成熟的待机动画
                     .ToggleTag(GameTags.FullyGrown) // 给实体加上 FullyGrown 标签，表示植物已完全成熟
                     .Enter(delegate (StatesInstance smi)  
                     {
                         if (smi.master.harvestable != null)
                         {
                             smi.master.harvestable.SetCanBeHarvested(state: true); // 设置植物为可收获状态
                         }
                     })
                     .EventHandlerTransition(GameHashes.Wilt, alive.wilting, (StatesInstance smi, object obj) => smi.master.wiltsOnReadyToHarvest) // 如果收到枯萎事件，且植物设置为“成熟时枯萎”，则转入枯萎状态
                     .EventTransition(GameHashes.Harvest, alive.harvest) // 收获事件触发时，切换到收获状态
                     .EventTransition(GameHashes.Grow, alive.fruiting_lost, (StatesInstance smi) => !smi.master.growing.ReachedNextHarvest()); // 如果触发生长事件且未达到下一收获周期，则进入“成熟丢失”状态

                // 控制植物的“收获”状态。
                alive.harvest .PlayAnim((StatesInstance smi) => smi.master.anims.harvest)  
                     .Enter(delegate (StatesInstance smi)  // 进入状态时执行以下操作
                     {
                         if (smi.master != null)
                         {
                           
                             // 调用作物组件的方法，生成果实（传入null表示使用默认配置）
                             smi.master.crop.SpawnConfiguredFruit(null);





                         }

                         if (smi.master.harvestable != null)
                         {
                             // 将植物设置为不可收获，避免重复采摘
                             smi.master.harvestable.SetCanBeHarvested(state: false);
                         }
                     })
                     .Exit(delegate (StatesInstance smi)  // 离开该状态时触发事件
                     {
                         // 触发事件 
                         smi.Trigger(113170146);
                     })
                     .OnAnimQueueComplete(alive.idle);  // 动画播放完成后，自动转入 idle 状态
            }


            private static string GetWiltAnim(StatesInstance smi)
            {
                // 获取植物的生长百分比
                float growingPercentage = smi.master.growing.PercentOfCurrentHarvest();
                // 根据动画集和生长百分比，选择对应的枯萎动画名字
                return GetWiltAnimFromAnimSet(smi.master.anims, growingPercentage);
            }

            // .Update 函数需要的重载版本
            private static void RefreshPositionPercent(StatesInstance smi, float dt)
            {
                RefreshPositionPercent(smi);
            }


            // .事件处理函数需要的重载版本
            private static void RefreshPositionPercent(StatesInstance smi)
            {
                if (!smi.master.preventGrowPositionUpdate)
                {
                    smi.master.RefreshPositionPercent();
                }
            }
        }


        /// <summary>
        /// 刷新植物的生长百分比位置
        /// </summary>
        public void RefreshPositionPercent()
        {
            animController.SetPositionPercent(growing.PercentOfCurrentHarvest());
        }



        [MyCmpReq]
        private ReceptacleMonitor rm; // 植物的容器监控组件

        [MyCmpReq]
        private Crop crop; // 植物的作物组件


        [MyCmpReq]
        private WiltCondition wiltCondition; // 植物的枯萎条件组件

        [MyCmpReq]
        private Growing growing; // 植物的生长组件

        [MyCmpReq]
        private KAnimControllerBase animController; // 植物的动画控制器组件

        [MyCmpGet]
        private Harvestable harvestable; // 植物的收获组件


        public bool preventGrowPositionUpdate;

        public bool wiltsOnReadyToHarvest; 


        // 植物的动画集，包含各种动画状态
        public AnimSet anims = defaultAnimSet;

        public static AnimSet defaultAnimSet = new AnimSet
        {
            pre_grow = null,
            grow = "grow",
            grow_pst = "grow_pst",
            idle_full = "idle_full",
            wilt_base = "wilt",
            harvest = "harvest",
            waning = "waning"
        };


        /// <summary>
        /// 根据生长百分比获取植物的枯萎动画
        /// </summary>
        /// <param name="set"></param>
        /// <param name="growingPercentage"></param>
        /// <returns></returns>
        public static string GetWiltAnimFromAnimSet(AnimSet set, float growingPercentage)
        {
            // int level = ((growingPercentage < 0.75f) ? 1 : ((!(growingPercentage < 1f)) ? 3 : 2));
            int level;
            if (growingPercentage < 0.75f)
            {
                level = 1;
            }
            else if (growingPercentage < 1f)
            {
                level = 2;
            }
            else
            {
                level = 3;
            }

            return set.GetWiltLevel(level);
        }




        // 创建植物死亡通知
        public Notification CreateDeathNotification()
        {
            return new Notification("植物死亡", NotificationType.Bad, (List<Notification> notificationList, object data) => string.Concat("这个植物已经死亡: \n", notificationList.ReduceMessages(countNames: false)), "/t• " + base.gameObject.GetProperName());
        }

        // 销毁植物自身
        protected void DestroySelf(object callbackParam)
        {
            CreatureHelpers.DeselectCreature(base.gameObject);
            Util.KDestroyGameObject(base.gameObject);
        }


        public class StatesInstance : GameStateMachine<States, StatesInstance, StandardCropTest, object>.GameInstance
        {
            public StatesInstance(StandardCropTest master)
                : base(master)
            {
            }
        }



    }

}
