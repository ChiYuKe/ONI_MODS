using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace AutomaticHarvest
{
    public class KModPatch
    {


        public static class Buildpatch
        {
            [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
            public static class ThermalBlock_F_1LoadGeneratedBuildings_Patch
            {

                public static void Prefix()
                {
                    ModUtil.AddBuildingToPlanScreen("Base", AutomaticHarvestConfig.ID, "Tiles");  // 添加到建筑菜单 ，"Base"是基础菜单，"Tiles"是子菜单 具体可定位到 TUNING.BUILDING.PLANORDER 查看其结构
                    Db.Get().Techs.Get("HighTempForging").unlockedItemIDs.Add(AutomaticHarvestConfig.ID); // 使其可研究
                    KModStringUtils.Add_New_BuildStrings(AutomaticHarvestConfig.ID, STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.NAME, STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.DESC,STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.EFFECT);
                }
            }
        }





        [HarmonyPatch(typeof(RangeVisualizerEffect), "OnPostRender")]
        public static class RangeVisualizerEffectPatch
        {
            public static void Prefix(RangeVisualizerEffect __instance)
            {
                // 1. 确定目标 GameObject
                GameObject targetGo = null;

                // 检查是否有选中的对象
                if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
                {
                    targetGo = SelectTool.Instance.selected.gameObject;
                }
                // 如果没有选中对象，检查是否有建造预览对象
                else if (BuildTool.Instance != null && BuildTool.Instance.visualizer != null)
                {
                    targetGo = BuildTool.Instance.visualizer;
                }

                // 2. 定义您要检查的目标 Tag
                Tag myTargetTag = "testRed"; // 示例：判断是否为食物 Tag

                // 默认颜色
                Color newColor = new Color(0f, 1f, 0.8f, 1f); 

                // 3. 检查目标对象是否存在且拥有 KPrefabID 组件
                if (targetGo != null && targetGo.TryGetComponent<KPrefabID>(out var kPrefabID))
                {
                    if (kPrefabID.HasTag(myTargetTag))
                    {
                      
                        newColor = new Color(1f, 0.1f, 0.3f, 1f); 
                    }
                    // else 保持默认色
                }

                // 4. 设置 __instance.highlightColor，并在 Material 上更新颜色
                __instance.highlightColor = newColor;

                // 设置 Material 颜色
                FieldInfo materialField = typeof(RangeVisualizerEffect).GetField("material", BindingFlags.Instance | BindingFlags.NonPublic);
                if (materialField != null)
                {
                    Material material = (Material)materialField.GetValue(__instance);
                    if (material != null)
                    {
                        // 确保在 OnPostRender 实际渲染前，Material 上的颜色已更新
                        material.SetColor("_HighlightColor", __instance.highlightColor);
                    }
                }
            }
        }




    }
}

