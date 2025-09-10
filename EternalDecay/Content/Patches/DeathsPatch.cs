using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CykUtils;
using HarmonyLib;
using static STRINGS.DUPLICANTS.MODIFIERS;
using UnityEngine;
using Database;

namespace EternalDecay.Content.Patches
{
    public class DeathsPatch
    {

        [HarmonyPatch(typeof(Deaths), MethodType.Constructor, new Type[] { typeof(ResourceSet) })]
        public class Deaths_Patch
        {
            public static void Postfix(Deaths __instance)
            {
                KDeaths.Register(__instance);
            }
        }


        public class KDeaths
        {
            public static Death Aging;

            public static void Register(Deaths deaths)
            {
                KDeaths.Aging = new Death(
                    "EDECAY_Death_Aging",
                    deaths,
                    Configs.STRINGS.DEATHS.AGING.NAME,
                    Configs.STRINGS.DEATHS.AGING.DESCRIPTION,
                    "death_suffocation",
                    "dead_on_back"
                );
                deaths.Add(KDeaths.Aging);
            }
        }



        // 阻断因为老死带来的全体哀悼DeBuff
        [HarmonyPatch(typeof(MinionModifiers), "OnDeath")]
        public static class MinionModifiers_OnDeath_Patch
        {
            public static bool Prefix(MinionModifiers __instance, object data)
            {

                GameObject deathObject = __instance.gameObject;

                if (deathObject == null)
                {
                    LogUtil.LogError("OnDeath: data 不是一个 GameObject 对象。");
                    return true;
                }
                if (deathObject.HasTag("NoMourning"))
                {

                    return false; 
                }
                return true;
            }
        }


    }
}
