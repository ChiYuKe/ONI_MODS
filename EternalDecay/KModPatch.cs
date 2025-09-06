
using CykUtils;
using HarmonyLib;
using KMod;

namespace EternalDecay
{
    public class KModPatch
    {
        public class Patch : UserMod2 
        {
            public override void OnLoad(Harmony harmony)
            {
                base.OnLoad(harmony);
                LogUtil.Log("MOD加载成功");
            }























        }
    }
}
