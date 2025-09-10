using CykUtils;
using UnityEngine;

namespace EternalDecay.Content.Core
{
    internal static class EternalDecayInitializer
    {
        private static EternalDecayMain _instance;

        /// <summary>
        /// 创建并挂载 EternalDecayMain 全局对象（单例）
        /// </summary>
        public static void Initialize()
        {
            if (_instance != null)
            {
                LogUtil.Log(" 已经存在，无需重复创建");
                return;
            }

            GameObject go = new GameObject("EternalDecayMain");
            UnityEngine.Object.DontDestroyOnLoad(go);

            _instance = go.AddComponent<EternalDecayMain>();
            //LogUtil.Log(" 已创建并挂载");
        }

        /// <summary>
        /// 获取全局对象实例
        /// </summary>
        public static EternalDecayMain Instance => _instance;
    }
}
