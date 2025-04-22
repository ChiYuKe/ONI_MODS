using System;
using HarmonyLib;
using UnityEngine;
using Klei.AI;
using System.Reflection;

namespace MinionAge.Core
{
    internal class AgeTableScreenPatch
    {
        public static float MaxMinionAge = TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.MINIONAGETHRESHOLD;

        [HarmonyPatch(typeof(VitalsTableScreen), "OnActivate")]
        public static class VitalsTableScreen_OnActivate_Patch
        {
            private static MethodInfo _getWidgetRowMethod;
            private static MethodInfo _getWidgetColumnMethod;

            // 秒数转换为周期（600秒=1周期）
            private static float SecondsToCycles(float seconds) => seconds / 600f;

            private static float SecondsToDisplayCycles(float seconds)
            {
                return (seconds / 600f) + 0.4f; // 手搓偏移
            }

            public static void Postfix(VitalsTableScreen __instance)
            {
                try
                {
                    // 缓存反射方法
                    _getWidgetRowMethod = typeof(TableScreen).GetMethod("GetWidgetRow",
                        BindingFlags.Instance | BindingFlags.NonPublic)
                        ?? throw new MissingMethodException("GetWidgetRow方法未找到");

                    _getWidgetColumnMethod = typeof(TableScreen).GetMethod("GetWidgetColumn",
                        BindingFlags.Instance | BindingFlags.NonPublic)
                        ?? throw new MissingMethodException("GetWidgetColumn方法未找到");

                    // 获取AddLabelColumn方法
                    var addLabelColumnMethod = typeof(TableScreen).GetMethod(
                        "AddLabelColumn",
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new Type[] {
                            typeof(string),
                            typeof(Action<IAssignableIdentity, GameObject>),
                            typeof(Func<IAssignableIdentity, GameObject, string>),
                            typeof(Comparison<IAssignableIdentity>),
                            typeof(Action<IAssignableIdentity, GameObject, ToolTip>),
                            typeof(Action<IAssignableIdentity, GameObject, ToolTip>),
                            typeof(int),
                            typeof(bool)
                        },
                        null) ?? throw new MissingMethodException("AddLabelColumn方法未找到");

                    Action<IAssignableIdentity, GameObject> onLoadAge = (identity, widget_go) =>
                        OnLoadAge(__instance, identity, widget_go);

                    // 添加年龄列
                    addLabelColumnMethod.Invoke(__instance, new object[] {
                        "年龄",
                        onLoadAge,
                        new Func<IAssignableIdentity, GameObject, string>(GetValueAgeLabel),
                        new Comparison<IAssignableIdentity>(CompareRowsAge),
                        new Action<IAssignableIdentity, GameObject, ToolTip>(OnTooltipAge),
                        new Action<IAssignableIdentity, GameObject, ToolTip>(OnTooltipSortAge),
                        70,  // 列宽度
                        true // 可排序
                    });
                }
                catch (Exception e)
                {
                    Debug.LogError($"初始化年龄列失败: {e}");
                }
            }

            private static void OnLoadAge(VitalsTableScreen instance, IAssignableIdentity identity, GameObject widget_go)
            {
                try
                {
                    var widgetRow = _getWidgetRowMethod.Invoke(instance, new object[] { widget_go }) as TableRow;
                    var label = widget_go.GetComponentInChildren<LocText>(true);

                    if (identity is MinionIdentity minion)
                    {
                        var column = _getWidgetColumnMethod.Invoke(instance, new object[] { widget_go }) as LabelTableColumn;
                        label.text = column?.get_value_action?.Invoke(minion, widget_go) ?? "年龄";
                    }
                    else
                    {
                        label.text = widgetRow?.isDefault == true ? "" : "年龄";
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"加载年龄显示失败: {e}");
                }
            }


            private static string GetValueAgeLabel(IAssignableIdentity identity, GameObject widget_go)
            {
                if (identity is MinionIdentity minion)
                {
                    float currentAgeSeconds = MinionDataSaver.GetCurrentAgeInSeconds(minion.gameObject);
                    if (currentAgeSeconds >= 0)
                    {
                        float currentAgeCycles = SecondsToCycles(currentAgeSeconds);
                        return $"{currentAgeCycles:F1}/{MaxMinionAge:F0}";
                    }
                    return "不支持";
                }
                return "--";
            }

            private static int CompareRowsAge(IAssignableIdentity a, IAssignableIdentity b)
            {
                float ageA = GetAgeValue(a);
                float ageB = GetAgeValue(b);
                return ageA.CompareTo(ageB);
            }

            private static float GetAgeValue(IAssignableIdentity identity)
            {
                if (identity is MinionIdentity minion)
                {
                    float currentAgeSeconds = MinionDataSaver.GetCurrentAgeInSeconds(minion.gameObject);
                    if (currentAgeSeconds >= 0)
                    {
                        return SecondsToCycles(currentAgeSeconds);
                    }
                }
                return -1f;
            }

            private static void OnTooltipAge(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
            {
                if (identity is MinionIdentity minion)
                {
                    float currentAgeSeconds = MinionDataSaver.GetCurrentAgeInSeconds(minion.gameObject);
                    if (currentAgeSeconds >= 0)
                    {
                        float currentAgeCycles = SecondsToCycles(currentAgeSeconds);
                        tooltip.SetSimpleTooltip(
                            $"当前年龄: {currentAgeCycles:F1} 周期\n" +
                            $"最大年龄: {MaxMinionAge:F0} 周期\n" +
                            $"(相当于 {currentAgeSeconds} 秒)");
                        return;
                    }
                }
                tooltip.SetSimpleTooltip("年龄数据不可用");
            }

            private static void OnTooltipSortAge(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
            {
                tooltip.SetSimpleTooltip("按当前年龄排序（周期单位）");
            }
        }
    }
}