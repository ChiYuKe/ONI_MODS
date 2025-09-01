using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using KMod;

namespace ElementExpansion
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






        [HarmonyPatch(typeof(Assets), "SubstanceListHookup")]
        public class ElementLoader_Load_Patch
        {

            private static void Postfix()
            {
                Elements.RegisterAllElements();
            }


        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
           
            public static void Postfix()
            {
                Localize(typeof(STRINGS));
               //  ElementUtil.RegisterElementStrings("TestElement", "测试元素", "这是一个测试元素的描述" );
            }
        }

        public static void RegisterForTranslation(Type locstring_tree_root)
        {
            Localization.RegisterForTranslation(locstring_tree_root);
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            // Localization.GenerateStringsTemplate(locstring_tree_root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
            Localization.GenerateStringsTemplate(locstring_tree_root, Path.Combine(Path.GetDirectoryName(executingAssembly.Location), "translations"));

        }


        public static class DebugAnims
        {
            public static void PrintAllAnims()
            {
                if (Assets.Anims == null)
                {
                    LogUtil.LogWarning("Assets.Anims 为空！");
                    return;
                }

                foreach (var kanim in Assets.Anims)
                {
                    if (kanim != null)
                        LogUtil.Log($"Anim: {kanim.name}");
                }

                LogUtil.Log($"总共 {Assets.Anims.Count} 个 anim 文件");
            }
        }


        public static void Localize(Type root)
        {
            // ModUtil.RegisterForTranslation(root);
            RegisterForTranslation(root);
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
                }
                else { LogUtil.LogWarning($"{modName}: 未在: {poFilePath} 找到本地化文件"); }
            }
            catch (Exception ex) { LogUtil.LogError($"{modName}: 无法加载本地化文件. Error: {ex.Message}"); }
        }

        private static void GenerateStringsTemplate(Type root, string translationsPath)
        {
            try
            {
                Localization.GenerateStringsTemplate(root, translationsPath);
            }
            catch (Exception ex) { LogUtil.LogError($"无法生成字符串模板. Error: {ex.Message}"); }
        }
    }







}
