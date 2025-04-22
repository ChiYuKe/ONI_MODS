using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace KModTool
{
    internal class KModAddClonedSideScreen
    {
        public static void AddClonedSideScreen<T>(string name, string originalName, Type originalType)
        {
            List<DetailsScreen.SideScreenRef> list;
            GameObject contentBody;
            bool elements = KModAddClonedSideScreen.GetElements(out list, out contentBody);
            if (elements)
            {
                SideScreenContent prefab = KModAddClonedSideScreen.Copy<T>(KModAddClonedSideScreen.FindOriginal(originalName, list), contentBody, name, originalType);
                list.Add(KModAddClonedSideScreen.NewSideScreen(name, prefab));
            }
        }
        private static bool GetElements(out List<DetailsScreen.SideScreenRef> screens, out GameObject contentBody)
        {
            Traverse traverse = Traverse.Create(DetailsScreen.Instance);
            screens = traverse.Field("sideScreens").GetValue<List<DetailsScreen.SideScreenRef>>();
            contentBody = traverse.Field("sideScreenContentBody").GetValue<GameObject>();
            return screens != null && contentBody != null;
        }
        private static SideScreenContent Copy<T>(SideScreenContent original, GameObject contentBody, string name, Type originalType)
        {
            GameObject gameObject = Util.KInstantiateUI<SideScreenContent>(original.gameObject, contentBody, false).gameObject;
            UnityEngine.Object.Destroy(gameObject.GetComponent(originalType));
            SideScreenContent sideScreenContent = gameObject.AddComponent(typeof(T)) as SideScreenContent;
            sideScreenContent.name = name.Trim();
            gameObject.SetActive(false);
            return sideScreenContent;
        }

        private static SideScreenContent FindOriginal(string name, List<DetailsScreen.SideScreenRef> screens)
        {
            SideScreenContent screenPrefab = screens.Find((DetailsScreen.SideScreenRef s) => s.name == name).screenPrefab;
            bool flag = screenPrefab == null;
            if (flag)
            {
                Debug.LogWarning("Could not find a sidescreen with the name " + name);
                Debug.LogWarning("找不到具有该名称的侧屏 " + name);
            }
            return screenPrefab;
        }
        private static DetailsScreen.SideScreenRef NewSideScreen(string name, SideScreenContent prefab)
        {
            return new DetailsScreen.SideScreenRef
            {
                name = name,
                offset = Vector2.zero,
                screenPrefab = prefab
            };
        }

        public static ToolTip AddSimpleToolTip(GameObject gameObject, string message, bool alignCenter = false, float wrapWidth = 0f)
        {
            bool flag = gameObject.GetComponent<ToolTip>() != null;
            ToolTip result;
            if (flag)
            {
                Debug.Log("GO already had a tooltip! skipping");
                Debug.Log("GO 已经有了工具提示！跳过");
                result = null;
            }
            else
            {
                ToolTip toolTip = gameObject.AddComponent<ToolTip>();
                toolTip.tooltipPivot = (alignCenter ? new Vector2(0.5f, 0f) : new Vector2(1f, 0f));
                toolTip.tooltipPositionOffset = new Vector2(0f, 20f);
                toolTip.parentPositionAnchor = new Vector2(0.5f, 0.5f);
                bool flag2 = wrapWidth > 0f;
                if (flag2)
                {
                    toolTip.WrapWidth = wrapWidth;
                    toolTip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
                }
                ToolTipScreen.Instance.SetToolTip(toolTip);
                toolTip.SetSimpleTooltip(message);
                result = toolTip;
            }
            return result;
        }
    }
}