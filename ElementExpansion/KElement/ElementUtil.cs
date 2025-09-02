using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using STRINGS;
using UnityEngine;

namespace ElementExpansion
{
    public static class ElementUtil
    {
        #region Path Utilities

        public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string AssetsPath => Path.Combine(ModPath, "assets");
        public static string TexturesPath => Path.Combine(AssetsPath, "textures");

        #endregion

        #region Texture Utilities

        public static Texture2D DuplicateTexture(Texture2D source)
        {
            var renderTex = RenderTexture.GetTemporary(source.width, source.height, 0,
                RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture.active = renderTex;

            var newTexture = new Texture2D(source.width, source.height);
            newTexture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            newTexture.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTex);

            return newTexture;
        }

        public static Texture2D LoadTexture(string textureName, bool warnIfFailed = true)
        {
            var path = Path.Combine(TexturesPath, $"{textureName}.png");

            if (!File.Exists(path))
            {
                if (warnIfFailed) LogUtil.LogWarning($"纹理文件不存在: {path}");
                return null;
            }

            try
            {
                var bytes = File.ReadAllBytes(path);
                var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

                if (texture.LoadImage(bytes))
                {
                    texture.name = textureName;
                    return texture;
                }

                LogUtil.LogError($"纹理解码失败: {path}");
                return null;
            }
            catch (Exception ex)
            {
                LogUtil.LogError($"加载纹理出错: {path}, {ex}");
                return null;
            }
        }

        public static Texture2D AdjustTextureBrightness(Texture2D texture, float brightnessMultiplier)
        {
            if (texture == null) return null;

            var adjustedTexture = DuplicateTexture(texture);
            var pixels = adjustedTexture.GetPixels32();

            for (int i = 0; i < pixels.Length; i++)
            {
                var pixel = pixels[i];
                float r = Mathf.Clamp01(pixel.r / 255f * brightnessMultiplier);
                float g = Mathf.Clamp01(pixel.g / 255f * brightnessMultiplier);
                float b = Mathf.Clamp01(pixel.b / 255f * brightnessMultiplier);

                pixels[i] = new Color32(
                    (byte)(r * 255),
                    (byte)(g * 255),
                    (byte)(b * 255),
                    pixel.a
                );
            }

            adjustedTexture.SetPixels32(pixels);
            adjustedTexture.Apply();
            return adjustedTexture;
        }

        #endregion

        #region Animation Utilities

        public static KAnimFile FindAnim(string name)
        {
            var anim = Assets.Anims.Find(a => a.name == name);
            if (anim == null) LogUtil.LogError($"动画文件未找到: {name}");
            return anim;
        }

        #endregion


        #region Element Creation Helpers

        public static Substance CreateSolidElement(string id, string anim, Color color, string texture = null, float brightness = 1f)
        {
            var material = Assets.instance.substanceTable.GetSubstance(SimHashes.Algae).material;
            return ElementUtil.CreateAndRegisterSubstance(
                id, Element.State.Solid, anim, material, color, texture, brightness
            );
        }

        public static Substance CreateLiquidElement(string id, Color color, string texture = null)
        {
            var material = Assets.instance.substanceTable.liquidMaterial;
            return ElementUtil.CreateAndRegisterSubstance(
                id, Element.State.Liquid, "liquid_tank_kanim", material, color, texture
            );
        }

        public static Substance CreateGasElement(string id, Color color)
        {
            var material = Assets.instance.substanceTable.liquidMaterial; // 气体通常使用液体材质
            return ElementUtil.CreateAndRegisterSubstance(
                id, Element.State.Gas, "gas_tank_kanim", material, color
            );
        }

        #endregion






        #region Substance Creation

        public static Substance CreateSubstance(
            string elementId,
            Element.State state,
            KAnimFile anim,
            Material material,
            Color color,
            Color uiColor,
            Color conduitColor)
        {
            return ModUtil.CreateSubstance(elementId, state, anim, material, color, uiColor, conduitColor);
        }

        public static Substance CreateAndRegisterSubstance(
         string elementId,
         Element.State state,
         string animName,
         Material baseMaterial,
         Color color,
         string textureName = null,
         float brightnessMultiplier = 1f)
        {
            LogUtil.Log($"开始创建物质: {elementId}");

            // 查找动画
            var anim = FindAnim(animName);
            if (anim == null)
            {
                LogUtil.LogError($"无法创建物质 {elementId}: 动画 {animName} 未找到");
                return null;
            }

            // 创建材质
            var material = new Material(baseMaterial);
            material.name = $"mat{elementId}";

            // 应用纹理（如果提供）
            if (!string.IsNullOrEmpty(textureName))
            {
                var texture = LoadTexture(textureName);
                if (texture != null)
                {
                    if (brightnessMultiplier != 1f)
                    {
                        texture = AdjustTextureBrightness(texture, brightnessMultiplier);
                    }
                    material.mainTexture = texture;
                    LogUtil.Log($"已应用纹理: {textureName}");
                }
            }

            // 创建物质
            var substance = CreateSubstance(
                elementId, state, anim, material, color, color, color
            );

            if (substance == null)
            {
                LogUtil.LogError($"物质创建失败: {elementId}");
                return null;
            }

            // 设置动画 - 这是关键步骤！
            try
            {
                var animsField = Traverse.Create(substance).Field("anims");
                if (animsField.FieldExists())
                {
                    animsField.SetValue(new[] { anim });
                    LogUtil.Log($"成功设置动画: {animName}");
                }
                else
                {
                    LogUtil.LogError($"物质中未找到 anims 字段: {elementId}");
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogError($"设置动画时出错: {ex}");
            }

            // 注册 SimHash
            RegisterSimHash(elementId);

            // 添加到物质表
            try
            {
                var substanceList = Assets.instance.substanceTable.GetList();
                substanceList.Add(substance);
                ElementLoader.FindElementByHash(substance.elementID).substance = substance;//这个非常非常非常关键
                LogUtil.Log($"物质已添加到物质表: {elementId}");
            }
            catch (Exception ex)
            {
                LogUtil.LogError($"添加到物质表失败: {ex}");
            }

            return substance;
        }

        #endregion

        #region SimHash Registration

        public static void RegisterSimHash(string name)
        {
            var simHash = (SimHashes)Hash.SDBMLower(name);
            SimHashRegistry.NameToHash[name] = simHash;
            SimHashRegistry.HashToName[simHash] = name;
        }

        public static class SimHashRegistry
        {
            public static readonly Dictionary<string, SimHashes> NameToHash = new Dictionary<string, SimHashes>();
            public static readonly Dictionary<SimHashes, string> HashToName = new Dictionary<SimHashes, string>();
        }

        #endregion

        #region Localization 这是我测试用的

        public static void RegisterElementStrings(string elementId, string name, string description)
        {
            var upperId = elementId.ToUpper();

            Strings.Add($"STRINGS.ELEMENTS.{upperId}.NAME", UI.FormatAsLink(name, elementId));
            Strings.Add($"STRINGS.ELEMENTS.{upperId}.DESC", description);
        }

        #endregion
    }
}