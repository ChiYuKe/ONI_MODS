using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

namespace TestMod
{
    public class MinionManager : KMonoBehaviour,ISim4000ms
    {

        public static void Initialize() 
        {
            // 在游戏对象上添加MinionManager组件
            GameObject MinionManager = new GameObject("MinionManager");
            UnityEngine.Object.DontDestroyOnLoad(MinionManager);
            MinionManager.AddComponent<MinionManager>();

        }

       

         void Update()
        {

           
        }

        public void Sim4000ms(float dt)
        {
            List<GameObject> allMinionGameObjects = KModMinionUtils.GetAllMinionGameObjects();
            KModMinionUtils.PrintNavigatorInfo(allMinionGameObjects[0]);
           
        }
    }

      
}
