using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static STRINGS.MISC.STATUSITEMS.HEALTHSTATUS;

namespace AutomaticHarvest
{
    public class Reservoir : KMonoBehaviour
    {
        private MeterController fillMeter; // 储存容量进度
        private MeterController lightMeter; // 状态灯

        public enum HstatusLight
        {
            Red,
            Yellow,
            Green
        }

        [MyCmpGet]
        private Storage storage;

        private static readonly EventSystem.IntraObjectHandler<Reservoir> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Reservoir>(delegate (Reservoir component, object data)
        {
            component.OnStorageChange(data);
        });

        /// <summary>
        /// 游戏对象生成时调用，初始化进度条和订阅事件
        /// </summary>
        protected override void OnSpawn()
        {
            base.OnSpawn();

            fillMeter = new MeterController(
                GetComponent<KBatchedAnimController>(),
                "meter_target",
                "meter",
                Meter.Offset.Infront,
                Grid.SceneLayer.NoLayer,
                "meter_fill", "meter_OL");

            Subscribe(-1697596308, OnStorageChangeDelegate);
            OnStorageChange(null);
        }

        /// <summary>
        /// 刷新状态灯显示
        /// </summary>
        /// <param name="hstatus">状态灯颜色</param>
        public void RefreshHstatusLight(HstatusLight hstatus)
        {
            if (lightMeter == null)
            {
                lightMeter = new MeterController(
                    GetComponent<KBatchedAnimController>(),
                    "opening",
                    "status_light",
                    Meter.Offset.Infront,
                    Grid.SceneLayer.NoLayer,
                    "meter_fill", "meter_OL");
            }

            // 将枚举转换为对应帧数（0=红, 1=黄, 2=绿）
            float frame = hstatus switch
            {
                HstatusLight.Red => 0f,
                HstatusLight.Yellow => 1f,
                HstatusLight.Green => 2f,
                _ => 0f
            };

            lightMeter.SetPositionPercent(frame / 3f);
        }

        /// <summary>
        /// 存储变化事件回调，更新进度条显示
        /// </summary>
        /// <param name="data">事件数据（未使用）</param>
        private void OnStorageChange(object data)
        {
            fillMeter.SetPositionPercent(Mathf.Clamp01(storage.MassStored() / storage.capacityKg));
        }
    }
}
