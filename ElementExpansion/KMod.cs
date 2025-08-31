using HarmonyLib;
using KMod;

namespace ElementExpansion
{
    public class KMod
    {
        public class Patch : UserMod2
        {
            public override void OnLoad(Harmony harmony)
            {
                base.OnLoad(harmony);
                Debug.Log("[TestPlanter] MOD加载成功");
            }
        }
    }
}
