using CykUtils;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Options;

namespace WireAnywhere
{
    public class Patch : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            new POptions().RegisterOptions(this, typeof(Config));
            LogUtil.Log("MOD加载成功");

        }



        // 本地化补丁，初始化时注册并加载翻译字符串。//
        [HarmonyPatch(typeof(Localization), "Initialize")]

        private class Translate_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(WireAnywhere.STRINGS), true);
            }
        }






    }
}
