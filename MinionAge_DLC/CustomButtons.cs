//using System;
//using Epic.OnlineServices.Logging;
//using HarmonyLib;
//using PeterHan.PLib.UI;
//using UnityEngine;
//using UnityEngine.UI;

//namespace CustomButtons
//{
//    internal class Patches
//    {



//        private static MultiToggle[] CustomButtons = new MultiToggle[3];
//        private static ToolTip[] CustomButtonTooltips = new ToolTip[3];

//        [HarmonyPatch(typeof(TopLeftControlScreen))]
//        [HarmonyPatch("OnActivate")]
//        public static class Add_Custom_Buttons
//        {



//            public static void Postfix(TopLeftControlScreen __instance)
//            {
//                var sandboxToggle = Traverse.Create(__instance).Field("sandboxToggle").GetValue<MultiToggle>();

//                if (sandboxToggle == null)
//                {
//                    Debug.LogError("Failed to find sandboxToggle!");
//                    return;
//                }

//                AddCustomButton(sandboxToggle, "icon_display_screen_properties", "Button 1", "This is Button 1", 0, OnClickButton1);
//                AddCustomButton(sandboxToggle, "icon_display_screen_properties", "Button 2", "This is Button 2", 1, OnClickButton2);
//                AddCustomButton(sandboxToggle, "icon_display_screen_properties", "Button 3", "This is Button 3", 2, OnClickButton3);

//                UpdateCustomButtonState();
//            }

//            private static void AddCustomButton(MultiToggle sandboxToggle, string iconName, string buttonText, string tooltipText, int index, System.Action onClick)
//            {
//                Transform buttonTransform = Util.KInstantiateUI(sandboxToggle.gameObject, sandboxToggle.transform.parent.gameObject, true).transform;
//                buttonTransform.SetSiblingIndex(sandboxToggle.transform.GetSiblingIndex() + 1);

//                buttonTransform.Find("FG").GetComponent<Image>().sprite = Assets.GetSprite(iconName);
//                buttonTransform.Find("Label").GetComponent<LocText>().text = buttonText;

//                if (buttonTransform.TryGetComponent(out MultiToggle button))
//                {
//                    button.onClick = (System.Action)Delegate.Combine(button.onClick, onClick);
//                    CustomButtons[index] = button;
//                }

//                if (buttonTransform.TryGetComponent(out ToolTip tooltip))
//                {
//                    tooltip.SetSimpleTooltip(tooltipText);
//                    CustomButtonTooltips[index] = tooltip;
//                }
//            }

//            private static void OnClickButton1() => HandleButtonClick(0, "Button 1 clicked!");
//            private static void OnClickButton2() => HandleButtonClick(1, "Button 2 clicked!");
//            private static void OnClickButton3() => HandleButtonClick(2, "Button 3 clicked!");

//            private static void HandleButtonClick(int index, string logMessage)
//            {

//                if (index == 0)
//                {
//                }
             
//                KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
//                Debug.Log(logMessage);
//                UpdateCustomButtonState();
//            }


//            private static void UpdateCustomButtonState()
//            {
//                for (int i = 0; i < CustomButtons.Length; i++)
//                {
//                    if (CustomButtons[i] != null)
//                    {
//                        CustomButtons[i].ChangeState(1); 
//                    }
//                }
//            }
//        }
//    }
//}