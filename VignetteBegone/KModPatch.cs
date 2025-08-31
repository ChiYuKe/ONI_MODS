using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using KMod;
using UnityEngine;
using UnityEngine.UI;

namespace VignetteBegone
{
    public class KModPatch
    {

        public class Patch : UserMod2
        {
            public override void OnLoad(Harmony harmony)
            {
                base.OnLoad(harmony);
                Debug.Log("[VignetteBegone] MOD加载成功".Color("lightblue"));
            }
        }


        [HarmonyPatch(typeof(MeterScreen))]
        [HarmonyPatch("OnSpawn")]
        public static class Add_Custom_Buttons1
        {

            public static void Postfix(MeterScreen __instance)
            {
                var redAlertButton = Traverse.Create(__instance).Field("RedAlertButton").GetValue<MultiToggle>();

                if (redAlertButton == null)
                {
                    Debug.LogError("[VignetteBegone] 没有找到 RedAlertButton!");
                    return;
                }

                AddCustomButton(redAlertButton, "icon_category_lights_disabled", "切换关闭屏幕晕影", OnClickButton1);

                // 读取保存的状态
                int savedState = PlayerPrefs.GetInt("VignetteHidden", 1); 
                vignetteHidden = savedState == 1;

               
                if (vignetteHidden && Vignette.Instance != null)
                {
                    SetVignetteAlpha(0f);
                    UpdateCustomButtonState(1);
                }
                // Game.Instance.Subscribe((int)GameHashes.ExitedRedAlert, OnAlertExited);
                // Game.Instance.Subscribe((int)GameHashes.ExitedYellowAlert, OnAlertExited);
            }



            [HarmonyPatch(typeof(Vignette), nameof(Vignette.Reset))]
            public static class Vignette_Reset_Patch
            {
                public static bool Prefix(Vignette __instance)
                {
                    if (vignetteHidden)
                    {
                        SetVignetteAlpha(0f);

                        Traverse.Create(__instance).Field("showingRedAlert").SetValue(false);
                        Traverse.Create(__instance).Field("showingYellowAlert").SetValue(false);

                        var sounds = __instance.GetComponent<LoopingSounds>();
                        sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP"));
                        sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP"));

                        // Debug.Log("[VignetteBegone] 阻止 Reset，改为透明");
                        return false;
                    }

                    return true;
                }
            }


            private static MultiToggle CustomButtons;
            private static ToolTip CustomButtonTooltips;

            private static void AddCustomButton(MultiToggle redAlertButton, string iconName, string tooltipText, System.Action onClick)
            {
                Transform buttonTransform = Util.KInstantiateUI(redAlertButton.gameObject, redAlertButton.transform.parent.gameObject, true).transform;
                buttonTransform.SetSiblingIndex(redAlertButton.transform.GetSiblingIndex() + 1);

                buttonTransform.Find("FG").GetComponent<Image>().sprite = Assets.GetSprite(iconName);

                if (buttonTransform.TryGetComponent(out MultiToggle button))
                {
                    button.onClick = (System.Action)Delegate.Combine(button.onClick, onClick);
                    CustomButtons = button;

                    if (button.states != null && button.states.Length > 0)
                    {
                        var color = new Color(0.37f, 0.6f, 0.25f, 1f);
                        button.states[1].color = color;
                        button.states[1].color_on_hover = color;
                        button.states[1].use_color_on_hover = true;
                    }
                }

                if (buttonTransform.TryGetComponent(out ToolTip tooltip))
                {
                    tooltip.SetSimpleTooltip(tooltipText);
                    CustomButtonTooltips = tooltip;
                }


            }


            private static void OnClickButton1() => HandleButtonClick();
            private static bool vignetteHidden = false;

            public static void HandleButtonClick()
            {
                if (Vignette.Instance == null)
                {
                    Debug.LogWarning("[VignetteBegone] 没有找到 Vignette 实例");
                    return;
                }
                // KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));
                if (!vignetteHidden)
                {
                    // 首次点击：隐藏 Vignette（只改 alpha）
                    SetVignetteAlpha(0f);
                    vignetteHidden = true;
                    UpdateCustomButtonState(1);
                }
                else
                {
                    // 恢复默认颜色
                    // Vignette.Instance.Reset();
                    Color c = Vignette.Instance.defaultColor;
                    c.a = 0.4705882f;
                    Vignette.Instance.SetColor(c);
                    vignetteHidden = false;
                    UpdateCustomButtonState(0);
                }

                // 保存按钮状态
                PlayerPrefs.SetInt("VignetteHidden", vignetteHidden ? 1 : 0);
                PlayerPrefs.Save();

            }




            private static void UpdateCustomButtonState(int a)
            {
                CustomButtons.ChangeState(a);
            }
            private static void SetVignetteAlpha(float alpha)
            {
                if (Vignette.Instance == null) return;

                Color c = Vignette.Instance.defaultColor;
                c.a = alpha;
                Vignette.Instance.SetColor(c);
            }
            private static void OnAlertExited(object data)
            {
                if (vignetteHidden && Vignette.Instance != null)
                {
                    var alertManager = ClusterManager.Instance.activeWorld.AlertManager;
                    if (alertManager == null) return;

                    if (!alertManager.IsRedAlert() && !alertManager.IsYellowAlert())
                    {
                        SetVignetteAlpha(0f);

                        Debug.Log($"[VignetteBegone] 警报结束，恢复透明{Vignette.Instance.defaultColor.a}");
                    }
                }
            }













            // 缺氧默认黑色BG颜色  new Color(0.266f, 0.286f, 0.352f, 1f)

        }



















        //[HarmonyPatch(typeof(TopLeftControlScreen))]
        //[HarmonyPatch("OnActivate")]
        //public static class Add_Custom_Buttons
        //{

        //    public static void Postfix(TopLeftControlScreen __instance)
        //    {
        //        var kleiItemDropButton = Traverse.Create(__instance).Field("kleiItemDropButton").GetValue<MultiToggle>();

        //        if (kleiItemDropButton == null)
        //        {
        //            Debug.LogError("Failed to find kleiItemDropButton!");
        //            return;
        //        }

        //        AddCustomButton(kleiItemDropButton, "icon_category_lights_disabled", "This is Button 1", OnClickButton1);
        //        HandleButtonClick();
        //    }

        //    private static MultiToggle CustomButtons;
        //    private static ToolTip CustomButtonTooltips;

        //    private static void AddCustomButton(MultiToggle kleiItemDropButton, string iconName, string tooltipText,System.Action onClick)
        //    {
        //        Transform buttonTransform = Util.KInstantiateUI(kleiItemDropButton.gameObject, kleiItemDropButton.transform.parent.gameObject, true).transform;
        //        buttonTransform.SetSiblingIndex(kleiItemDropButton.transform.GetSiblingIndex() + 1);

        //        buttonTransform.Find("FG").GetComponent<Image>().sprite = Assets.GetSprite(iconName);


        //        if (buttonTransform.TryGetComponent(out MultiToggle button))
        //        {
        //            button.onClick = (System.Action)Delegate.Combine(button.onClick, onClick);
        //            CustomButtons = button;

        //            // 设置默认状态颜色
        //            if (button.states != null && button.states.Length > 0)
        //            {
        //                button.states[2].color = new Color(0.37f, 0.6f, 0.25f, 1f);

        //            }


        //        }


        //        if (buttonTransform.TryGetComponent(out ToolTip tooltip))
        //        {
        //            tooltip.SetSimpleTooltip(tooltipText);
        //            CustomButtonTooltips = tooltip;
        //        }
        //    }


        //    private static void OnClickButton1() => HandleButtonClick();
        //    private static bool vignetteHidden = false;

        //    public static void HandleButtonClick()
        //    {
        //        if (Vignette.Instance == null)
        //        {
        //            Debug.LogWarning("[VignetteBegone] 没有找到 Vignette 实例");
        //            return;
        //        }

        //        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click"));

        //        if (!vignetteHidden)
        //        {
        //            // 首次点击：隐藏 Vignette（只改 alpha）
        //            Color hiddenColor = Vignette.Instance.defaultColor;
        //            hiddenColor.a = 0f;
        //            Vignette.Instance.SetColor(hiddenColor);
        //            vignetteHidden = true;
        //            UpdateCustomButtonState(2);
        //        }
        //        else
        //        {
        //            // 再次点击：恢复默认颜色
        //            Vignette.Instance.Reset();  // 还原 defaultColor 和其他状态
        //            vignetteHidden = false;
        //            UpdateCustomButtonState(1);
        //        }
        //    }

        //    private static void UpdateCustomButtonState(int a)
        //    {
        //        CustomButtons.ChangeState(a); // 直接传 1，全部启用
        //    }
        //}






        //[HarmonyPatch(typeof(Vignette))]
        //[HarmonyPatch("OnSpawn")]
        //public static class Vignette_OnSpawn_Patch
        //{
        //    private static FieldInfo _imageField;
        //    private static FieldInfo _defaultColorField;

        //    public static void Postfix(Vignette __instance)
        //    {
        //        try
        //        {
        //            // 使用缓存反射字段提高性能
        //            _imageField ??= AccessTools.Field(typeof(Vignette), "image");
        //            _defaultColorField ??= AccessTools.Field(typeof(Vignette), "defaultColor");

        //            // 获取私有字段值
        //            var image = (UnityEngine.UI.Image)_imageField.GetValue(__instance);
        //            if (image == null) return;

        //            // 修改颜色
        //            var defaultColor = (Color)_defaultColorField.GetValue(__instance);
        //            defaultColor.a = 0f; // 设置完全不透明

        //            // 写回字段值
        //            _defaultColorField.SetValue(__instance, defaultColor);
        //            image.color = defaultColor;
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.LogError($"[TestVignette]修改Vignette透明度失败: {e}");
        //        }
        //    }
        //}
    }

    // 彩色日志
    public static class DebugExtensions
    {
        public static string Color(this string text, string color) =>
            $"<color={color}>{text}</color>";
    }
}
