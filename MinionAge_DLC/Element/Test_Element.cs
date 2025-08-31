using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestElement
{
    public static class Test_Element
    {

        // 加载本地图片并设置名字
        private static Texture2D LoadTexture(string textureName)
        {
            // 调用 ElementUtil 的 LoadTextureFromFile 方法
            var texture = ElementUtil.TextureLoader.LoadTextureFromFile(textureName);
            if (texture == null)
            {
                Debug.LogError($"无法加载纹理: {textureName}");
                return null;
            }

            texture.name = textureName; // 设置纹理名称
            return texture;
        }

        // 克隆一个材质并使用本地图片作为texture
        private static Material CreateTestElementMaterial(Material source, string textureName)
        {
            Material material = new Material(source);
            Texture2D texture2D = LoadTexture(textureName); 
            if (texture2D != null)
            {
                // 调整纹理亮度
                texture2D = AdjustTextureBrightness(texture2D, 2);
                material.mainTexture = texture2D; // 将本地图片设置为材质的主纹理
            }
            material.name = "matTestElement";
            return material;
        }

        // 注册 TestElement 物质
        public static void RegisterTestElemenSubstance()
        {
            //Substance substance = Assets.instance.substanceTable.GetSubstance(SimHashes.Polypropylene);
            //ElementUtil.CreateRegisteredSubstance("TestElement", Element.State.Solid, ElementUtil.FindAnim("Test_element_kanim"), Test_Element.CreateTestElemenMaterial(substance.material), Test_Element.TESTELEMENT_COLOR);

            Substance substance = Assets.instance.substanceTable.GetSubstance(SimHashes.Algae);
            Material material = CreateTestElementMaterial(substance.material, "TestElementTexture"); // 使用本地图片
            ElementUtil.CreateRegisteredSubstance(
                "TestElement",
                Element.State.Solid,
                ElementUtil.FindAnim("Test_element_kanim"),
                material,
                Test_Element.TESTELEMENT_COLOR
            );
        }



        // 调整纹理亮度
        private static Texture2D AdjustTextureBrightness(Texture2D texture, float brightnessMultiplier)
        {
            Texture2D adjustedTexture = ElementUtil.DuplicateTexture(texture);
            Color32[] pixels = adjustedTexture.GetPixels32();

            for (int i = 0; i < pixels.Length; i++)
            {
                Color32 pixel = pixels[i];
                float r = pixel.r / 255f;
                float g = pixel.g / 255f;
                float b = pixel.b / 255f;

                // 调整亮度
                r *= brightnessMultiplier;
                g *= brightnessMultiplier;
                b *= brightnessMultiplier;

                // 确保颜色值在 0-1 范围内
                r = Mathf.Clamp01(r);
                g = Mathf.Clamp01(g);
                b = Mathf.Clamp01(b);

                // 将调整后的颜色值转换回 Color32
                pixels[i] = new Color32((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), pixel.a);
            }

            adjustedTexture.SetPixels32(pixels);
            adjustedTexture.Apply();
            return adjustedTexture;
        }

        public static readonly Color32 TESTELEMENT_COLOR = new Color32(255, 255, 255, byte.MaxValue);
        public const string TESTELEMENT_ID = "TestElement";
        public static readonly SimHashes TestElemenSimHash = (SimHashes)Hash.SDBMLower("TestElement");
    }
}
