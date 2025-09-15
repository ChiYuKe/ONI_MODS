using HarmonyLib;
using System;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Database;
using System.Reflection;
using MinionAge.Core;

namespace MinionAge
{

    [HarmonyPatch(typeof(BaseMinionConfig))]
    public static class AddMinionAmountsPatch
    {
        
        [HarmonyPostfix]
        [HarmonyPatch("AddMinionAmounts")]
        public static void Postfix(Modifiers modifiers)
        {
           
            if (Db.Get() != null && Db.Get().Amounts != null && Db.Get().Amounts.Age != null)
            {
                if (modifiers.initialAmounts != null)
                {
                    modifiers.initialAmounts.Add(Db.Get().Amounts.Age.Id);
                  
                }
            }
        }


        [HarmonyPostfix]
        [HarmonyPatch("BaseMinion")]
        public static void Postfix_BaseMinion(GameObject __result)
        {
            __result.AddComponent<AgeProgressBarComponent>();

        }


    }





    // 阻断因为老死带来的全体哀悼DeBuff
    [HarmonyPatch(typeof(MinionModifiers), "OnDeath")]
    public static class MinionModifiers_OnDeath_Patch
    {
        static bool Prefix(MinionModifiers __instance, object data)
        {
           
            GameObject deathObject = __instance.gameObject;
           
            if (deathObject == null)
            {
                Debug.LogError("OnDeath: data 不是一个 GameObject 对象。");
                return true;
            }   
            if (deathObject.HasTag(TUNINGS.NoMourning))
            {
               
                return false; // 返回 false 跳过原方法
            }
            return true;
        }
    }





    [HarmonyPatch(typeof(MinionConfig))]
    public static class AddMonionTraitsPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("GetTraits")]
        public static void Postfix(ref AttributeModifier[] __result)
        {
            // 获取 Db 实例
            var db = Db.Get();
            if (db == null || db.Attributes == null || db.Amounts == null || db.Amounts.Age == null)
            {
                Debug.LogWarning("Db 或 Amounts.Age 为 null，无法添加新的特质");
                return;
            }

            if (__result.Any(modifier => modifier.AttributeId == db.Amounts.Age.maxAttribute.Id))
            {
                Debug.LogWarning("特质 'maxAttribute' 已经存在，无法重复添加");
                return;
            }

            float ageThreshold = RandomDebuffTimerManager.MinionAgeThreshold;
            var newModifiers = __result.ToList();

            newModifiers.Add(new AttributeModifier(
                db.Amounts.Age.maxAttribute.Id,     // 目标属性：Age.maxAttribute 的 Id
                ageThreshold,                       // 特质影响值：使用 MinionAgeThreshold 值
                STRINGS.MISC.NOTIFICATIONS.CTCLEROULETTE.NAME,                               // 特质名称（description）
                false,                              // 不是乘法因子
                false,                              // 不是 UI 仅可见
                true                                // 是只读特质
            ));

            newModifiers.Add(new AttributeModifier(
                db.Amounts.Age.deltaAttribute.Id,   // 目标属性：Age.deltaAttribute 的 Id
                1 / 600f,                           // 每秒增量的特质影响值（假设是 1/600）
                STRINGS.MISC.NOTIFICATIONS.CTCLEROULETTE.NAME,                               // 特质名称（description）
                false,                              // 不是乘法因子
                false,                              // 不是 UI 仅可见
                true                                // 是只读特质
            ));

            // 更新 __result 数组
            __result = newModifiers.ToArray();
        }
    }













    [HarmonyPatch(typeof(ChoreTypes))]
    public static class AddNewChorePatch
    {
        public static ChoreType Accepttheinheritance;

        [HarmonyPostfix]
        [HarmonyPatch(MethodType.Constructor, new Type[] { typeof(ResourceSet) })]
        public static void Postfix(ChoreTypes __instance)
        {
            if (__instance != null)
            {
                MethodInfo addMethod = typeof(ChoreTypes).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);

                if (addMethod != null)
                {

                    object[] parameters = new object[]
                    {
                            "Accepttheinheritance",
                            new string[0],
                            "",
                            new string[0],
                            "顷刻炼化大脑",
                            "顷刻炼化大脑",
                            "这个复制人正在接受记忆传承！！",
                            false,
                            -1,
                            null
                    };
                    Accepttheinheritance = (ChoreType)addMethod.Invoke(__instance, parameters);



                }
            }
        }
    }






    [HarmonyPatch(typeof(Deaths))]
    [HarmonyPriority(Priority.First)]
    public static class AddNewDeathPatch
    {
        public static Death customDeath;

        [HarmonyPostfix]
        [HarmonyPatch(MethodType.Constructor, new Type[] { typeof(ResourceSet) })]
        public static void Postfix(Deaths __instance)
        {

            if (__instance != null)
            {

                string id = "CustomDeath";
                string name = STRINGS.DUPLICANTS.DEATHS.NAME;
                string description = STRINGS.DUPLICANTS.DEATHS.DESCRIPTION;
                string animation1 = "dead_on_back";
                string animation2 = "dead_on_back";

                customDeath = new Death(id, __instance, name, description, animation1, animation2);
                __instance.Add(customDeath);
            
            }
            else
            {
                Debug.LogWarning("Deaths 实例为空");
            }
        }
    }



    [HarmonyPatch(typeof(Database.Amounts), "Load")]
    [HarmonyPriority(Priority.First)]
    public static class AddAmountPatch
    {
        public static Amount MiniAgeAmount; 

        [HarmonyPostfix]
        public static void Postfix(Database.Amounts __instance)
        {
            if (__instance != null)
            {
                string id = "MiniAge";
                float min = 0f;
                float max = 100f;
                bool showMax = true;
                Units units = Units.Flat;
                float deltaThreshold = 0.1675f;
                bool showInUI = true;
                string stringRoot = "STRINGS.CREATURES";
                string uiSprite = "ui_icon_age";

                MiniAgeAmount = __instance.CreateAmount(id, min, max, showMax, units, deltaThreshold, showInUI, stringRoot, uiSprite);
             
            }
            else
            {
                Debug.LogWarning("Amounts 对象为空");
            }
        }
    }


    [HarmonyPatch(typeof(MinionConfig))]
    public static class AddMinionTagPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("CreatePrefab")] 
        public static void Postfix(GameObject __result)
        {
            if (__result != null)
            {
                Tag modTag = new Tag("ShowModifiedAge");
                // 添加标签，为了确保年龄描述正常显示
                __result.AddOrGet<KPrefabID>().AddTag(modTag, true);
            }
        }
    }


    [HarmonyPatch(typeof(MinionVitalsPanel))]
    [HarmonyPatch("Refresh")]
    public static class MinionVitalsPanelRefreshPatch
    {
        public static void Postfix(MinionVitalsPanel __instance, GameObject selectedEntity)
        {
            var amountsLines = Traverse.Create(__instance).Field("amountsLines").GetValue<List<MinionVitalsPanel.AmountLine>>();
            if (amountsLines == null || selectedEntity == null) return;

            Klei.AI.Amounts amounts = selectedEntity.GetAmounts();
            if (amounts == null) return;

            KPrefabID prefabID = selectedEntity.GetComponent<KPrefabID>();
            bool hasShowModifiedAgeTag = prefabID != null && prefabID.HasTag("ShowModifiedAge");

            //不是复制人就跳过文本替换
            if (!hasShowModifiedAgeTag) return;


            string ageKeyword = "年龄";
            string ageTooltipText = "这只小动物在<style=\"KKeyword\">年龄</style>到达物种寿命上限时就会死去";
           
            // 根据系统语言切换文本
            if (Localization.GetCurrentLanguageCode() == "en")
            {
                ageKeyword = "Age";
                ageTooltipText = "This critter will die when its <style=\"KKeyword\">Age</style> reaches its species' maximum lifespan";
            }
            foreach (var amountLine in amountsLines)
            {
                if (amountLine.amount.Id == "Age")
                {
                    AmountInstance ageInstance = amounts.Get(amountLine.amount);
                    if (ageInstance != null)
                    {
                        
                        string customAgeText = amountLine.amount.GetDescription(ageInstance).Replace(ageKeyword, MISSING.STRINGS.CREATURES.ATTRIBUTES.MINIAGEDELTA.NAME);
                        string customAgeTooltip = amountLine.toolTipFunc(ageInstance).Replace(
                            ageTooltipText,
                           MISSING.STRINGS.CREATURES.ATTRIBUTES.MINIAGEDELTA.DESC);

                        amountLine.locText.SetText(customAgeText); 
                        amountLine.toolTip.toolTip = customAgeTooltip;
                    }
                }
            }
        }
    }
    
}
