using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using STRINGS;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace TestElement
{
    internal class ElementUtil
    {

        public static string ModPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static string AssetsPath
        {
            get
            {
                return Path.Combine(ElementUtil.ModPath, "assets");
            }
        }

        /// <summary>
        /// 深拷贝一个 Texture2D，得到一份新的贴图对象，不依赖原来的内存
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Texture2D DuplicateTexture(Texture2D source)
        {
            RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(source, temporary);
            RenderTexture active = RenderTexture.active;
            RenderTexture.active = temporary;
            Texture2D texture2D = new Texture2D(source.width, source.height);
            texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active = active;
            RenderTexture.ReleaseTemporary(temporary);
            return texture2D;
        }

        public static class TextureLoader
        {
            /// <summary>
            /// 根据名称加载 Mod 内部纹理（默认路径: assets/textures/{name}.png）
            /// </summary>
            public static Texture2D LoadTextureFromFile(string textureName)
            {
                var path = Path.Combine(ElementUtil.ModPath, "assets", "textures", textureName + ".png");

                if (TryLoadTexture(path, out var texture))
                {
                    LogUtil.Log($"成功加载纹理: {textureName}");
                    return texture;
                }

                LogUtil.Error($"无法加载纹理: {textureName} ({path})");
                return null;
            }

            /// <summary>
            /// 尝试加载指定路径的纹理
            /// </summary>
            public static bool TryLoadTexture(string path, out Texture2D texture)
            {
                texture = LoadTexture(path);
                return texture != null;
            }

            /// <summary>
            /// 从路径加载纹理（失败时返回 null）
            /// </summary>
            public static Texture2D LoadTexture(string path)
            {
                if (!File.Exists(path))
                {
                    LogUtil.Warning($"纹理文件不存在: {path}");
                    return null;
                }

                var bytes = TryReadFile(path);
                if (bytes == null || bytes.Length == 0)
                    return null;

                try
                {
                    var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    if (tex.LoadImage(bytes))
                        return tex;

                    LogUtil.Error($"纹理解码失败: {path}");
                    return null;
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"加载纹理出错: {path}, {ex}");
                    return null;
                }
            }

            /// <summary>
            /// 安全读取文件字节
            /// </summary>
            private static byte[] TryReadFile(string path)
            {
                try
                {
                    return File.ReadAllBytes(path);
                }
                catch (Exception ex)
                {
                    LogUtil.Error($"无法读取文件: {path}, 错误: {ex}");
                    return null;
                }
            }
        }






        [Obsolete("使用Test_Element.CreateTestElementMaterial,不过这个很灵活，但我现在只用改一个")]
        // 将 Texture2D 应用到材质
        public static void ApplyTextureToMaterial(Material material, string textureName, string propertyName = "_MainTex")
        {
            var texture = TextureLoader.LoadTextureFromFile(textureName);
            if (texture != null)
            {
                material.SetTexture(propertyName, texture);
                // material.mainTexture = texture;
                LogUtil.Log($"已将纹理 {textureName} 应用到材质属性 {propertyName}");
            }
        }

       
       






        public static void RegisterElementStrings(string elementId, string name, string description)
        {
            string text = elementId.ToUpper();
            Strings.Add(new string[]
            {
                "STRINGS.ELEMENTS." + text + ".NAME",
                UI.FormatAsLink(name, text)
            });
            Strings.Add(new string[]
            {
                "STRINGS.ELEMENTS." + text + ".DESC",
                description
            });
        }






        public static KAnimFile FindAnim(string name)
        {
            KAnimFile kanimFile = Assets.Anims.Find((KAnimFile anim) => anim.name == name);
            bool flag = kanimFile == null;
            if (flag)
            {
                global::LogUtil.Error("没找到动画文件: " + name);
            }
            return kanimFile;
        }



        public static void AddSubstance(Substance substance)
        {
            Assets.instance.substanceTable.GetList().Add(substance);
        }


        public static Substance CreateSubstance(string name, Element.State state, KAnimFile kanim, Material material, Color32 colour)
        {
            return ModUtil.CreateSubstance(name, state, kanim, material, colour, colour, colour);
        }

        public static Substance CreateRegisteredSubstance(string name, Element.State state, KAnimFile kanim, Material material, Color32 colour)
        {
            Substance substance = ElementUtil.CreateSubstance(name, state, kanim, material, colour);
            Traverse.Create(substance).Field("anims").SetValue(new KAnimFile[] { kanim });
            SimHashUtil.RegisterSimHash(name);
            ElementUtil.AddSubstance(substance);
            ElementLoader.FindElementByHash(substance.elementID).substance = substance;
            return substance;
        }
    }
}
