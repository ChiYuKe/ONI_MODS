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



        // 加载本地图片为 Texture2D
        public static Texture2D LoadTextureFromFile(string textureName)
        {
            // 拼接图片路径
            var path = Path.Combine(ElementUtil.ModPath, "assets", "textures", textureName + ".png");

            // 尝试加载图片
            if (TryLoadTexture(path, out var texture))
            {
                Debug.Log($"成功加载纹理: {textureName}");
                return texture;
            }
            else
            {
                Debug.LogError($"无法加载纹理: {textureName}");
                return null;
            }
        }




        // 将 Texture2D 应用到材质
        public static void ApplyTextureToMaterial(Material material, string textureName, string propertyName = "_MainTex")
        {
            var texture = LoadTextureFromFile(textureName);
            if (texture != null)
            {
                material.SetTexture(propertyName, texture);
                Debug.Log($"已将纹理 {textureName} 应用到材质属性 {propertyName}");
            }
        }










        public static bool TryLoadTexture(string path, out Texture2D texture)
        {
            texture = LoadTexture(path, false);
            return texture != null;
        }

        // Token: 0x06000667 RID: 1639 RVA: 0x0001C558 File Offset: 0x0001A758
        public static Texture2D LoadTexture(string path, bool warnIfFailed = true)
        {
            Texture2D texture2D = null;
            if (File.Exists(path))
            {
                byte[] array = TryReadFile(path);
                texture2D = new Texture2D(1, 1);
                texture2D.LoadImage(array);
            }
            else if (warnIfFailed)
            {
                Debug.LogWarning(new object[] { "无法在路径上加载纹理 " + path + "." });
            }
            return texture2D;
        }
        public static byte[] TryReadFile(string texFile)
        {
            byte[] array;
            try
            {
                array = File.ReadAllBytes(texFile);
            }
            catch (Exception ex)
            {
                object[] array2 = new object[1];
                int num = 0;
                string text = "无法读取文件: ";
                Exception ex2 = ex;
                array2[num] = text + ((ex2 != null) ? ex2.ToString() : null);
                Debug.Log(array2);
                array = null;
            }
            return array;
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
                global::Debug.LogError("Failed to find KAnim: " + name);
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
