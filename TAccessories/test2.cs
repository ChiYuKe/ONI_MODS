using System;
using System.Collections.Generic;
using Database;
using Epic.OnlineServices.Logging;
using EternalDecay.Content.Comps;
using EternalDecay.Content.Patches;
using HarmonyLib;
using Klei.AI;
using TAccessories;
using UnityEngine;
using UnityEngine.UI;
using static Database.Emotes;
using static GameTags;
using static STRINGS.DUPLICANTS.MODIFIERS;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

namespace CustomButtons
{
    internal class Patches
    {



        private static MultiToggle[] CustomButtons = new MultiToggle[3];
        private static ToolTip[] CustomButtonTooltips = new ToolTip[3];



        [HarmonyPatch(typeof(TopLeftControlScreen))]
        [HarmonyPatch("OnActivate")]
        public static class Add_Custom_Buttons
        {



            public static void Postfix(TopLeftControlScreen __instance)
            {


                var sandboxToggle = Traverse.Create(__instance).Field("sandboxToggle").GetValue<MultiToggle>();
                //var KToggle = Traverse.Create(__instance).Field("sandboxToggle").GetValue<KToggle>();

                if (sandboxToggle == null)
                {
                    Debug.LogError("Failed to find sandboxToggle!");
                    return;
                }

                AddCustomButton(sandboxToggle, "icon_display_screen_properties", "效果测试", "This is Button 1", 0, OnClickButton1);
                AddCustomButton(sandboxToggle, "icon_display_screen_properties", "一键死亡", "This is Button 2", 1, OnClickButton2);
                AddCustomButton(sandboxToggle, "icon_display_screen_properties", "干饭", "This is Button 3", 2, OnClickButton3);

                UpdateCustomButtonState();
            }

            private static void AddCustomButton(MultiToggle sandboxToggle, string iconName, string buttonText, string tooltipText, int index, System.Action onClick)
            {
                Transform buttonTransform = Util.KInstantiateUI(sandboxToggle.gameObject, sandboxToggle.transform.parent.gameObject, true).transform;
                buttonTransform.SetSiblingIndex(sandboxToggle.transform.GetSiblingIndex() + 1);

                buttonTransform.Find("FG").GetComponent<Image>().sprite = Assets.GetSprite(iconName);
                buttonTransform.Find("Label").GetComponent<LocText>().text = buttonText;

                if (buttonTransform.TryGetComponent(out MultiToggle button))
                {
                    button.onClick = (System.Action)Delegate.Combine(button.onClick, onClick);
                    CustomButtons[index] = button;
                }

                if (buttonTransform.TryGetComponent(out ToolTip tooltip))
                {
                    tooltip.SetSimpleTooltip(tooltipText);
                    CustomButtonTooltips[index] = tooltip;
                }
            }

            private static void OnClickButton1() => HandleButtonClick(0, "Button 1 clicked!");
            private static void OnClickButton2() => HandleButtonClick(1, "Button 2 clicked!");
            private static void OnClickButton3() => HandleButtonClick(2, "Button 3 clicked!");

            private static void HandleButtonClick(int index, string logMessage)
            {

                if (index == 0)
                {
                    GameObject markerGO = new GameObject("CursorDurationMarker");
                    AETE_CursorDurationMarker marker = markerGO.AddComponent<AETE_CursorDurationMarker>();
                    marker.transform.parent = GameScreenManager.Instance.ssOverlayCanvas.transform;
                    marker.Show(100, Color.green);

                }
                if (index == 1)
                {

                    // 访问缓存的复制人列表
                    var minionList = EternalDecay.Content.Core.MinionEventManager.MinionCache;

                    foreach (var minionGO in minionList)
                    {
                        if (minionGO == null) continue;

                        minionGO.AddTag("NoMourning");





                        var deathMonitor = minionGO.GetSMI<DeathMonitor.Instance>();
                        deathMonitor.Kill(DeathsPatch.KDeaths.Aging);

                        var identity = minionGO.GetComponent<MinionIdentity>();
                        if (identity != null)
                        {
                            Debug.Log("复制人名字: " + identity.GetProperName());
                        }
                    }
                }
                if (index == 2)
                {
                    // 访问缓存的复制人列表
                    var minionList = EternalDecay.Content.Core.MinionEventManager.MinionCache;
                    foreach (var minionGO in minionList)
                    {
                        if (minionGO == null) continue;

                        ChoreProvider choreProvider = minionGO.GetComponent<ChoreProvider>();
                        if (choreProvider != null)
                        {
                            Db db = Db.Get();
                            Emote emote = Db.Get().Emotes.Minion.MorningStretch;

                            new EmoteChore(choreProvider, db.ChoreTypes.EmoteHighPriority, emote, 1, null);
                        }

                        //var choreProvider1 = minionGO.GetComponent<ChoreProvider>();
                        //if (choreProvider1 != null)
                        //{
                        //    new EmoteChore(
                        //        choreProvider1,
                        //        Db.Get().ChoreTypes.EmoteHighPriority,
                        //        "anim_eat_floor_kanim",                        // animFile
                        //        new HashedString[] { "eat_pre", "eat_loop", "eat_pst" } // anim sequence
                        //    );
                        //}




                    }







                }

                KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
                Debug.Log(logMessage);
                UpdateCustomButtonState();
            }






            private static void UpdateCustomButtonState()
            {
                for (int i = 0; i < CustomButtons.Length; i++)
                {
                    if (CustomButtons[i] != null)
                    {
                        CustomButtons[i].ChangeState(1);
                    }
                }
            }
        }
    }


    public class EyeReplacer : MonoBehaviour
    {

        public KAnimFile customEyeAnim = Assets.GetAnim("aete_hulk_head_kanim"); // 自定义眼睛动画文件（.kanim）
        public string eyeSymbolName = "eye_default"; // 你自定义动画里的眼睛符号名称

        public void ReplaceEyes()
        {
            // 获取 SymbolOverrideController
            SymbolOverrideController soc = GetComponent<SymbolOverrideController>();
            if (soc == null)
            {
                Debug.LogWarning("SymbolOverrideController not found!");
                return;
            }

            // 获取 Accessorizer（管理配件）
            Accessorizer accessorizer = GetComponent<Accessorizer>();
            if (accessorizer == null)
            {
                Debug.LogWarning("Accessorizer not found!");
                return;
            }

            // 1️⃣ 替换眼睛
            if (customEyeAnim != null)
            {
                KAnim.Build build = customEyeAnim.GetData().build;
                KAnim.Build.Symbol eyeSymbol = build.GetSymbol(eyeSymbolName);
                if (eyeSymbol != null)
                {
                    soc.AddSymbolOverride("snapto_eye", eyeSymbol, 1);
                }
            }

            // 2️⃣ 保留头发
            Accessory hairAcc = accessorizer.GetAccessory(Db.Get().AccessorySlots.Hair);
            if (hairAcc != null)
            {
                soc.AddSymbolOverride("snapto_hair_always", hairAcc.symbol, 1);
            }

            // 3️⃣ 保留帽子
            Accessory hatHairAcc = Db.Get().AccessorySlots.HatHair.Lookup(
                "hat_" + HashCache.Get().Get(hairAcc.symbol.hash)
            );
            if (hatHairAcc != null)
            {
                soc.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, hatHairAcc.symbol, 1);
            }

            Debug.Log("眼睛替换完成！");
        }

    }









}