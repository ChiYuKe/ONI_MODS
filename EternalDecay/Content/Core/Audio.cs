using System.IO;
using CykUtils;

namespace EternalDecay.Content.Core
{
    public class ModAssets
    {

        public static float GetSFXVolume()
        {
            return KPlayerPrefs.GetFloat("Volume_SFX") * KPlayerPrefs.GetFloat("Volume_Master");
        }


        public static void LoadAll()
        {
           
            var path = KUtils.AssetsPath;
            AudioUtil.LoadSound(ModAssets.Sounds.NMSHJ, Path.Combine(path, "你骂谁罕见啊你骂谁狗罕见.wav"), false, false);
            AudioUtil.LoadSound(ModAssets.Sounds.XWYSZMB, Path.Combine(path, "想玩原神怎么办原神隐犯了.wav"), false, false);
            AudioUtil.LoadSound(ModAssets.Sounds.WW, Path.Combine(path, "喔喔.wav"), false, false);
        }
        public static class Sounds
        {
            
            public static int NMSHJ = Hash.SDBMLower("KMOD_NMSHJ");

            public static int XWYSZMB = Hash.SDBMLower("KMOD_XWYSZMB");

            public static int WW = Hash.SDBMLower("KMOD_WOWO");

        }
    }
}
