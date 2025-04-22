using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MinionAge
{
    public class MinionBrainCounter : MonoBehaviour
    {
        // 用于存储组件实例的计数
        [Serialize]
        private static int MinionBrainCount = 0;

        [Serialize]
        private static List<GameObject> MinionBrainobjectList = new List<GameObject>();

        // 在组件启用时增加计数
        void OnEnable()
        {
            MinionBrainCount++;
            MinionBrainobjectList.Add(gameObject);
        }

        // 在组件禁用或销毁时减少计数
        void OnDisable()
        {
            MinionBrainCount--;
            MinionBrainobjectList.Remove(gameObject);
           

        }
        // 获取所有使用此组件的对象
        public static List<GameObject> GetAllObjects()
        {
            return new List<GameObject>(MinionBrainobjectList);
        }


        public int GetMinionCount()
        {
           
            return MinionBrainCount;
        }



    }
}
