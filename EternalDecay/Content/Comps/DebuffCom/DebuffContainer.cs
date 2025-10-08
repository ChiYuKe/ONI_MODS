using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EternalDecay.Content.Comps.DebuffCom
{
    // 这是一个空组件，用作容器
    public class DebuffContainer : KMonoBehaviour
    {
        // 可选：控制整个容器是否启用
        public bool IsEnabled = true;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            // 确保只添加一次 AbyssophobiaDebuff
            if (this.gameObject.GetComponent<AbyssophobiaDebuff>() == null)
                this.gameObject.AddComponent<AbyssophobiaDebuff>();



        }


    }
}
