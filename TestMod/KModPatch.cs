using HarmonyLib;
using KMod;
using PeterHan.PLib.Options;
using System;
using System.IO;
using System.Reflection;

namespace TestMod
{
    public static class Main
    {
        public class Patch : UserMod2
        {
            public override void OnLoad(Harmony harmony)
            {
                base.OnLoad(harmony);
                Debug.Log("开始初始化");
                MinionManager.Initialize();

            }
        }
    }
}