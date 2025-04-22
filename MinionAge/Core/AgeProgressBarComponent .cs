using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STRINGS.CODEX.CRITTERSTATUS.FERTILITY;
using UnityEngine;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

namespace MinionAge.Core
{
    internal class AgeProgressBarComponent : KMonoBehaviour
    {
        public static float MaxMinionAge = TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.MINIONAGETHRESHOLD * 600; // 复制人年龄阈值（单位：周期）
        private ProgressBar progressBar; // 进度条组件
       


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

        private void Update()
        {
            var prefabID = base.gameObject.GetComponent<KPrefabID>();
            if (prefabID.HasTag("Corpse")) 
            {
                if (progressBar != null)
                {
                    DestroyProgressBar();
                }
                return; 
            };
            InitializeAgeProgressBar();


        }
        
        private void InitializeAgeProgressBar()
        {

            if (progressBar != null)
            {
                DestroyProgressBar(); // 如果已有进度条，先销毁
            }
            // 检查进度条预制体是否加载
            if (ProgressBarsConfig.Instance == null || ProgressBarsConfig.Instance.progressBarPrefab == null)
            {
                Debug.LogError("进度条预制体未加载！");
                return;
            }

            // 创建进度条，绑定到基座本身
            progressBar = ProgressBar.CreateProgressBar(base.gameObject, () =>
            {
                float currentAgeInSeconds = MinionDataSaver.GetCurrentAgeInSeconds(base.gameObject); // 获取当前年龄
                    return currentAgeInSeconds / MaxMinionAge; // 返回当前年龄比例
            });
            if (progressBar == null)
            {
                Debug.LogError("进度条创建失败！");
                return;
            }
            // 设置进度条可见性
            progressBar.SetVisibility(true);

            // 设置进度条颜色 - 可以使用生命周期相关的颜色
            progressBar.barColor = new Color(0.2f, 0.8f, 0.2f); // 绿色



        }



        private void DestroyProgressBar()
        {
            if (progressBar != null)
            {
                Util.KDestroyGameObject(progressBar.gameObject); // 销毁进度条
                progressBar = null;
            }
        }


    }
}
