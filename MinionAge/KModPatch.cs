using HarmonyLib;
using KMod;
using MinionAge;
using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;


namespace MinionAge
{
    public static class Main
    {
        public class Patch : UserMod2
        {
            public override void OnLoad(Harmony harmony)
            {
                base.OnLoad(harmony);
                //harmony.PatchAll();
                ModEntry.Initialize();
                Debug.Log("Mod - MinionAge - 已加载并初始化。");
               //  PUtil.InitLibrary(true);
                new POptions().RegisterOptions(this, typeof(ConfigurationItem));
            }
        }







        public static bool IsModLoaded(string modID)
        {
            // 打印所有 Mod 的信息
            foreach (Mod mod in Global.Instance.modManager.mods)
            {
                Console.WriteLine($"Mod ID: {mod.staticID}, Active: {mod.IsActive()}");
            }

            // 检查指定 Mod 是否加载
            foreach (Mod mod in Global.Instance.modManager.mods)
            {
                if (mod.staticID == modID && mod.IsActive())
                {
                    return true;
                }
            }
            return false;
        }



        [JsonObject(MemberSerialization.OptIn)]
        [ConfigFile("DebuffRoulette.json", true, true)]
        [RestartRequired]
        public class ConfigurationItem : SingletonOptions<ConfigurationItem>
        {

            [Option("STRINGS.CONFIGURATIONITEM.MINIONAGETHRESHOLD", "", "", Format = "F0")]
            [Limit(5.0, 3000.0)]
            [JsonProperty]
            public float minionagethreshold { get; set; }


            [Option("STRINGS.CONFIGURATIONITEM.AGE80PERCENTTHRESHOLD", "STRINGS.CONFIGURATIONITEM.AGE80PERCENTTHRESHOLDDESC", "", Format = "F1")]
            [Limit(0, 1)]
            [JsonProperty]
            public float age80percentthreshold { get; set; }

            [Option("STRINGS.CONFIGURATIONITEM.INHERITANCESUCCESSPROBABILITY", "STRINGS.CONFIGURATIONITEM.INHERITANCESUCCESSPROBABILITYDESC", "", Format = "F1")]
            [Limit(0, 1)]
            [JsonProperty]
            public float inheritanceSuccessProbability { get; set; }


            public ConfigurationItem()
            {
                this.minionagethreshold = 120;
                this.age80percentthreshold = 0.7f;
                this.inheritanceSuccessProbability = 0.8f;
            }

        }


        [HarmonyPatch(typeof(Localization))]
        [HarmonyPatch("Initialize")]
        private class Translate
        {
            public static void Postfix()
            {
               Localize(typeof(STRINGS));
            }
        }

        public static void Localize(Type root)
        {
            ModUtil.RegisterForTranslation(root);
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string modName = executingAssembly.GetName().Name;
            string translationsPath = Path.Combine(Path.GetDirectoryName(executingAssembly.Location), "translations");
            Localization.Locale locale = Localization.GetLocale();
            string languageCode = locale != null ? locale.Code : "en"; // 默认使用英语
            string poFilePath = Path.Combine(translationsPath, languageCode + ".po");
            LoadLocalizationFile(modName, poFilePath);
            // GenerateStringsTemplate(root, translationsPath);
            LocString.CreateLocStringKeys(root, "");
        }

        private static void LoadLocalizationFile(string modName, string poFilePath)
        {
            try
            {
                if (File.Exists(poFilePath))
                {
                   
                    var localizedStrings = Localization.LoadStringsFile(poFilePath, false);
                    Localization.OverloadStrings(localizedStrings);
                } else { Debug.LogWarning($"{modName}: 未在: {poFilePath} 找到本地化文件"); }
            }
            catch (Exception ex){ Debug.LogError($"{modName}: 无法加载本地化文件. Error: {ex.Message}"); }
        }

        private static void GenerateStringsTemplate(Type root, string translationsPath)
        {
            try
            {
                Localization.GenerateStringsTemplate(root, translationsPath);
            }catch (Exception ex) { Debug.LogError($"无法生成字符串模板. Error: {ex.Message}"); }
        }
    }


   







        //internal class NewText
        //{
        //    [HarmonyPatch(typeof(BuildingFacades), MethodType.Constructor, new Type[]
        //    {
        //        typeof(ResourceSet)
        //    })]
        //    public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        //    {
        //        public static void Postfix(BuildingFacades __instance)
        //        {

        //            __instance.Add("paofu", "飞鱼床", "描述", PermitRarity.Universal, "LuxuryBed", "KModelegantBed_puft_kanim", DlcManager.AVAILABLE_ALL_VERSIONS, null);
        //        }
        //    }
        //}

        public class ModifierSetPatch
    {
        [HarmonyPatch(typeof(ModifierSet), "Initialize")]
        public static class ModifierSet_Initialize_Patch
        {
            public static void Postfix(ModifierSet __instance)
            {
                // 在 ModifierSet 初始化后注册自定义效果
                KModDeBuff.Register(__instance);
            }
        }
    }

}
