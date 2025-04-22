using Klei.AI;
using KModTool;
using OxygenConsumingPlant.Tool;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static Crop;
using CREATURES = STRINGS.CREATURES;

namespace OxygenConsumingPlant
{
    public class OxygenConsumingPlant : StateMachineComponent<OxygenConsumingPlant.StatesInstance>, ISingleSliderControl, ISliderControl
    {
        public Notification CreateDeathNotification()
        {
            return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
        }


        public void RefreshPositionPercent()
        {
            this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
        }


        private void MonitorPlantHeating(OxygenConsumingPlant.StatesInstance smi, float deltaTime)
        {
            // 分配用于管理网格单元和场景分区条目的池化列表
            ListPool<int, OxygenConsumingPlant>.PooledList nonSolidCells = ListPool<int, OxygenConsumingPlant>.Allocate();
            ListPool<ScenePartitionerEntry, OxygenConsumingPlant>.PooledList pickupableEntries = ListPool<ScenePartitionerEntry, OxygenConsumingPlant>.Allocate();
            ListPool<ScenePartitionerEntry, OxygenConsumingPlant>.PooledList plantEntries = ListPool<ScenePartitionerEntry, OxygenConsumingPlant>.Allocate();

            RangeVisualizer rangeVisualizer = GetComponent<RangeVisualizer>();

            // 获取当前对象位置对应的网格单元
            int currentCell = Grid.PosToCell(base.transform.GetPosition());

            // 获取非固体网格单元
            GameUtil.GetNonSolidCells(currentCell, this.radius, nonSolidCells);

            List<int> blockedCells = new List<int>();

            // 检查每个网格单元是否未被遮挡视线
            for (int i = 0; i < nonSolidCells.Count; i++)
            {
                int targetCell = nonSolidCells[i];
                bool isBlocked = KModGridUtilities.IsLineOfSightBlocked(currentCell, targetCell, rangeVisualizer);
                if (isBlocked)
                {
                    blockedCells.Add(targetCell);
                }
            }

            // 对未被遮挡视线的网格单元进行处理
            for (int j = 0; j < blockedCells.Count; j++)
            {
                int blockedCell = blockedCells[j];

                int cellX;
                int cellY;
                Grid.CellToXY(blockedCell, out cellX, out cellY);

                // 收集该坐标范围内的拾取物和植物
                GameScenePartitioner.Instance.GatherEntries(cellX, cellY, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pickupableEntries);
                GameScenePartitioner.Instance.GatherEntries(cellX, cellY, 1, 1, GameScenePartitioner.Instance.plants, plantEntries);

                //枪斗术
                Emote radiationItchEmote = Db.Get().Emotes.Minion.FingerGuns;

                Miscellaneous.PerformEmoteActions(pickupableEntries,10, radiationItchEmote,false);

                // 处理拾取物
                for (int k = 0; k < pickupableEntries.Count; k++)
                {
                    ScenePartitionerEntry entry = pickupableEntries[k];
                    object entryObject = entry.obj;
                    GameObject gameObject = entryObject as GameObject;
                    if (gameObject == null)
                    {
                        Pickupable pickupable = entryObject as Pickupable;
                        if (pickupable != null)
                        {
                            gameObject = pickupable.gameObject;
                        }
                    }

                    if (gameObject != null)
                    {
                        // 获取对象的Effects组件和MinionBrain组件
                        Effects effectsComponent = gameObject.GetComponent<Effects>();
                        MinionBrain minionBrainComponent = gameObject.GetComponent<MinionBrain>();
                        if (effectsComponent != null && minionBrainComponent != null)
                        {
                            // 添加效果到组件中
                            effectsComponent.Add("wajue", true);
                            effectsComponent.Add("huifu", true);
                        }
                    }
                }
                // 清空池化列表
                pickupableEntries.Clear();
            }

            // 回收植物监视单元
            this.RecyclePlantMonitorCells();

            // 更新植物监视单元列表
            this.PlantMonitorCells = plantEntries;

            // 回收池化列表
            nonSolidCells.Recycle();
            pickupableEntries.Recycle();

            this.AdjustTemperature(smi);
        }


        private void RecyclePlantMonitorCells()
        {
           
            bool hasPlantMonitorCells = this.PlantMonitorCells != null;
            if (hasPlantMonitorCells)
            {
                // 尝试将 PlantMonitorCells 转换为 PooledList
                ListPool<ScenePartitionerEntry, OxygenConsumingPlant>.PooledList pooledList = this.PlantMonitorCells as ListPool<ScenePartitionerEntry, OxygenConsumingPlant>.PooledList;

                // 检查转换是否成功
                bool isPooledList = pooledList != null;
                if (isPooledList)
                {
                    // 如果是 PooledList，则进行回收
                    pooledList.Recycle();
                }
                else
                {
                    // 如果不是 PooledList，则清空 PlantMonitorCells 列表
                    this.PlantMonitorCells.Clear();
                }
            }
        }



        public string SliderTitleKey => "温度调整";
        public string SliderUnits => "°C";

        public int SliderDecimalPlaces(int index) => 1;

        public float GetSliderMin(int index) => 0f;
        public float GetSliderMax(int index) => 100f;

        public float GetSliderValue(int index) => targetTemperature;

        public void SetSliderValue(float value, int index)
        {
            OxygenConsumingPlant.targetTemperature = value;
        }

        public string GetSliderTooltipKey(int index) => "调整目标温度.";

        public string GetSliderTooltip(int index) => $"{targetTemperature} °C";




        private void AdjustTemperature(StatesInstance statesInstance)
        {
            // 将目标温度转换为开尔文
            float targetTemperatureKelvin = 273.15f + targetTemperature;

            // 定义温度调整参数
            float temperatureAdjustmentRate = 2f;
            float temperatureAdjustmentFactor = 1f;//区间

            // 定义植物的Tag
            Tag plantTag = new Tag("KModOxygenPlantTag");

            try
            {
                // 调整植物的温度
                Miscellaneous.AdjustTemperature(statesInstance.master.gameObject, this.PlantMonitorCells, targetTemperatureKelvin, temperatureAdjustmentRate, temperatureAdjustmentFactor, plantTag);
            }
            catch (Exception exception)
            {
                // 记录调用 AdjustTemperature 方法时的异常信息
                Debug.LogError("调用 AdjustTemperature 方法时发生异常: " + exception.Message);
            }

            // 检查目标温度是否超过80摄氏度
            bool isTemperatureAboveThreshold = targetTemperature > 80f;
            if (isTemperatureAboveThreshold)
            {
                Debug.Log("当前温度已经大于80度了");
            }
        }


        protected void DestroySelf(object callbackParam)
        {
            CreatureHelpers.DeselectCreature(base.gameObject);
            Util.KDestroyGameObject(base.gameObject);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            base.smi.StartSM();
          
        }


        private static string ToolTipResolver(List<Notification> notificationList, object data)
        {
            string text = "";
            for (int i = 0; i < notificationList.Count; i++)
            {
                Notification notification = notificationList[i];
                text += (string)notification.tooltipData;
                bool flag = i < notificationList.Count - 1;
                if (flag)
                {
                    text += "\n";
                }
            }
            return string.Format(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP, text);
        }

        public static AnimSet defaultAnimSet = new AnimSet
        {
            grow = "grow",
            grow_pst = "grow_pst",
            idle_full = "idle_full",
            wilt_base = "wilt",
            harvest = "harvest",
            waning = "waning"
        };

        public AnimSet anims = defaultAnimSet;

        private const int WILT_LEVELS = 3;

        public int radius = 4;

        [SerializeField]
        private List<ScenePartitionerEntry> entriesPlant = new List<ScenePartitionerEntry>();

        private List<ScenePartitionerEntry> PlantMonitorCells = new List<ScenePartitionerEntry>();

        [MyCmpReq]
        private KAnimControllerBase animController;

        [MyCmpReq]
        private Crop crop;

        [MyCmpReq]
        private Growing growing;

        [MyCmpGet]
        private Harvestable harvestable;

        [MyCmpReq]
        private ReceptacleMonitor rm;

        [MyCmpReq]
        private WiltCondition wiltCondition;

        private static float targetTemperature = 25f;

        private float _targetTemperature = 25f;

        public class AnimSet
        {
            public string GetWiltLevel(int level)
            {
                bool flag = this.m_wilt == null;
                if (flag)
                {
                    this.m_wilt = new string[3];
                    for (int i = 0; i < 3; i++)
                    {
                        this.m_wilt[i] = this.wilt_base + (i + 1).ToString();
                    }
                }
                return this.m_wilt[level - 1];
            }
            public string GetIdleFull(int level)
            {
                bool flag = this.m_idlefull == null;
                if (flag)
                {
                    this.m_idlefull = new string[2];
                    for (int i = 0; i < 2; i++)
                    {
                        this.m_idlefull[i] = this.idle_full + (i + 1).ToString();
                    }
                }
                return this.m_idlefull[level - 1];
            }
            public string GetGrowPst(int level)
            {
                bool flag = this.m_grow_pst == null;
                if (flag)
                {
                    this.m_grow_pst = new string[2];
                    for (int i = 0; i < 2; i++)
                    {
                        this.m_grow_pst[i] = this.grow_pst + (i + 1).ToString();
                    }
                }
                return this.m_grow_pst[level - 1];
            }
            // Token: 0x04000025 RID: 37
            public string grow;

            // Token: 0x04000026 RID: 38
            public string grow_pst;

            // Token: 0x04000027 RID: 39
            public string harvest;

            // Token: 0x04000028 RID: 40
            public string idle_full;

            // Token: 0x04000029 RID: 41
            public string waning;

            // Token: 0x0400002A RID: 42
            public string wilt_base;

            // Token: 0x0400002B RID: 43
            private string[] m_wilt;
            private string[] m_idlefull;
            private string[] m_grow_pst;
        }

       
        public class States : GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant>
        {
           
            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
                default_state = this.alive;
                this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate (OxygenConsumingPlant.StatesInstance smi)
                {
                    bool flag = smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
                    if (flag)
                    {
                        Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
                        Notification notification = smi.master.CreateDeathNotification();
                        notifier.Add(notification, "");
                    }
                    GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
                    Harvestable component = smi.master.GetComponent<Harvestable>();
                    bool flag2 = component != null && component.CanBeHarvested && GameScheduler.Instance != null;
                    if (flag2)
                    {
                        GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
                    }
                    smi.master.Trigger(1623392196, null);
                    smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
                    global::UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
                    smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
                });
                this.blighted.InitializeStates(this.masterTarget, this.dead).PlayAnim((OxygenConsumingPlant.StatesInstance smi) => smi.master.anims.waning, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Crop_Blighted, null)
                    .TagTransition(GameTags.Blighted, this.alive, true);
                this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle).ToggleComponent<Growing>(false)
                    .TagTransition(GameTags.Blighted, this.blighted, false);
                this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (OxygenConsumingPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Grow, this.alive.pre_fruiting, (OxygenConsumingPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).EventTransition(GameHashes.CropSleep, this.alive.sleeping, new StateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.Transition.ConditionCallback(this.IsSleeping))
                    .PlayAnim((OxygenConsumingPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Paused)
                    .Enter(new StateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State.Callback(OxygenConsumingPlant.States.RefreshPositionPercent))
                    .Update(new Action<OxygenConsumingPlant.StatesInstance, float>(OxygenConsumingPlant.States.RefreshPositionPercent), UpdateRate.SIM_4000ms, false)
                    .EventHandler(GameHashes.ConsumePlant, new StateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State.Callback(OxygenConsumingPlant.States.RefreshPositionPercent));

                this.alive.pre_fruiting.PlayAnim(
                    new Func<StatesInstance, string>(GetGrowPstAnim), KAnim.PlayMode.Once)
                    .TriggerOnEnter(GameHashes.BurstEmitDisease, null)
                    .EventTransition(GameHashes.AnimQueueComplete, this.alive.fruiting, null);


                this.alive.fruiting_lost.Enter(delegate (StatesInstance smi)
                {
                    bool flag3 = smi.master.harvestable != null;
                    if (flag3)
                    {
                        smi.master.harvestable.SetCanBeHarvested(false);
                    }
                }).GoTo(this.alive.idle);

                this.alive.wilting.PlayAnim(
                    new Func<StatesInstance, string>(GetWiltAnim), KAnim.PlayMode.Loop)
                    .EventTransition(GameHashes.WiltRecover, this.alive.idle, (StatesInstance smi) => !smi.master.wiltCondition.IsWilting())
                    .EventTransition(GameHashes.Harvest, this.alive.harvest, null);

                this.alive.sleeping.PlayAnim((OxygenConsumingPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Once).EventTransition(GameHashes.CropWakeUp, this.alive.idle, GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.Not(new StateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.Transition.ConditionCallback(this.IsSleeping))).EventTransition(GameHashes.Harvest, this.alive.harvest, null)
                    .EventTransition(GameHashes.Wilt, this.alive.wilting, null);
                this.alive.fruiting.PlayAnim(
                   new Func<StatesInstance, string>(GetIdleFullAnim), KAnim.PlayMode.Loop)
                   .Enter(delegate (StatesInstance smi)
                   {
                       bool flag4 = smi.master.harvestable != null;
                       if (flag4)
                       {
                           smi.master.harvestable.SetCanBeHarvested(true);
                       }
                   })
                   .EventTransition(GameHashes.Wilt, this.alive.wilting, null)
                   .EventTransition(GameHashes.Harvest, this.alive.harvest, null)
                   .EventTransition(GameHashes.Grow, this.alive.fruiting_lost, (StatesInstance smi) => !smi.master.growing.ReachedNextHarvest());
                  
                this.alive.harvest.PlayAnim((OxygenConsumingPlant.StatesInstance smi) => smi.master.anims.harvest, KAnim.PlayMode.Once).Enter(delegate (OxygenConsumingPlant.StatesInstance smi)
                {
                    bool isValidMaster = smi.master != null;
                    if (isValidMaster)
                    {
                        var primaryElement = smi.master.gameObject.GetComponent<PrimaryElement>();
                        float temperature = primaryElement.Temperature;

                        // 根据自己温度来确定生成什么果实
                        string cropId = temperature > 273.15f + 50f ? "KModOxygenTreeFruit_R" : "KModOxygenTreeFruit_G";
                        Crop.CropVal selectedCrop = CROPS.CROP_TYPES.Find(crop => crop.cropId == cropId);

                        
                        smi.master.crop.Configure(selectedCrop);
                        smi.master.crop.SpawnConfiguredFruit(null);

                        primaryElement.Temperature = 298.15f;
                    }

                    bool flag6 = smi.master.harvestable != null;
                    if (flag6)
                    {
                        smi.master.harvestable.SetCanBeHarvested(false);
                    }
                }).Exit(delegate (StatesInstance smi)
                {
                    smi.Trigger(113170146, null);
                })
                    .OnAnimQueueComplete(this.alive.idle);
            }

            public bool IsSleeping(OxygenConsumingPlant.StatesInstance smi)
            {
                CropSleepingMonitor.Instance smi2 = smi.master.GetSMI<CropSleepingMonitor.Instance>();
                return smi2 != null && smi2.IsSleeping();
            }

            // Token: 0x06000049 RID: 73 RVA: 0x00004680 File Offset: 0x00002880
            private static string GetWiltAnim(OxygenConsumingPlant.StatesInstance smi)
            {
                float num = smi.master.growing.PercentOfCurrentHarvest();
                bool flag = num < 0.75f;
                int num2;
                if (flag)
                {
                    num2 = 1;
                }
                else
                {
                    bool flag2 = num < 1f;
                    if (flag2)
                    {
                        num2 = 2;
                    }
                    else
                    {
                        num2 = 3;
                    }
                }
                return smi.master.anims.GetWiltLevel(num2);
            }
            private static string GetIdleFullAnim(StatesInstance smi)
            {
                // 获取植物的温度
                var primaryElement = smi.master.gameObject.GetComponent<PrimaryElement>();
                float temperature = primaryElement.Temperature;

                // 确定空闲级别
                int ldlefull = temperature > 323.15f ? 1 : 2; // 323.15 K = 50°C

                // 获取并返回空闲标识符
                return smi.master.anims.GetIdleFull(ldlefull);
            }
            private static string GetGrowPstAnim(StatesInstance smi)
            {
                // 获取植物的温度
                var primaryElement = smi.master.gameObject.GetComponent<PrimaryElement>();
                float temperature = primaryElement.Temperature;

                // 确定空闲级别
                int growpst = temperature > 323.15f ? 1 : 2; // 323.15 K = 50°C

                // 获取并返回空闲标识符
                return smi.master.anims.GetGrowPst(growpst);
            }


            private static void RefreshPositionPercent(OxygenConsumingPlant.StatesInstance smi, float dt)
            {
                smi.master.RefreshPositionPercent();
                smi.master.MonitorPlantHeating(smi, dt);
            }

            private static void RefreshPositionPercent(OxygenConsumingPlant.StatesInstance smi)
            {
                smi.master.RefreshPositionPercent();
            }

            public OxygenConsumingPlant.States.AliveStates alive;

            // Token: 0x0400002D RID: 45
            public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.PlantAliveSubState blighted;

            // Token: 0x0400002E RID: 46
            public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State dead;

            // Token: 0x02000021 RID: 33
            public class AliveStates : GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.PlantAliveSubState
            {
                // Token: 0x04000037 RID: 55
                public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State barren;

                // Token: 0x04000038 RID: 56
                public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State destroy;

                // Token: 0x04000039 RID: 57
                public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State fruiting;

                // Token: 0x0400003A RID: 58
                public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State fruiting_lost;

                // Token: 0x0400003B RID: 59
                public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State harvest;

                // Token: 0x0400003C RID: 60
                public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State idle;

                // Token: 0x0400003D RID: 61
                public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State pre_fruiting;

                // Token: 0x0400003E RID: 62
                public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State sleeping;

                // Token: 0x0400003F RID: 63
                public GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.State wilting;
            }
        }

        // Token: 0x02000015 RID: 21
        public class StatesInstance : GameStateMachine<OxygenConsumingPlant.States, OxygenConsumingPlant.StatesInstance, OxygenConsumingPlant, object>.GameInstance
        {
            // Token: 0x0600004D RID: 77 RVA: 0x00004713 File Offset: 0x00002913
            public StatesInstance(OxygenConsumingPlant master)
                : base(master)
            {
            }
        }
    }
}