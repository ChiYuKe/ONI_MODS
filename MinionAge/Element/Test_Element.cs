using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestElement
{
    public static class Test_Element
    {
        private static Texture2D TintTextureTestElementColor(Texture sourceTexture, string name)
        {
            Texture2D texture2D = ElementUtil.DuplicateTexture(sourceTexture as Texture2D);
            Color32[] pixels = texture2D.GetPixels32(); 
            Color32 bioplasticColor = Test_Element.TESTELEMENT_COLOR; // 假设 BIOPLASTIC_COLOR 是 Color 类型

            for (int i = 0; i < pixels.Length; i++)
            {
                Color32 pixel = pixels[i];
                float grayscale = (0.299f * pixel.r + 0.587f * pixel.g + 0.114f * pixel.b) / 255f;
                float intensity = grayscale;
                Color bioplasticColorFloat = bioplasticColor;
                Color tintedColor = bioplasticColorFloat * intensity;
                tintedColor.a = pixel.a / 255f; // 将透明度从 0-255 转换为 0-1

                // 将结果转换回 Color32 并赋值
                pixels[i] = tintedColor;
            }

            texture2D.SetPixels32(pixels);
            texture2D.Apply();
            texture2D.name = name;
            return texture2D;
        }


        private static Material CreateTestElemenMaterial(Material source)
        {
            Material material = new Material(source);
            Texture2D texture2D = Test_Element.TintTextureTestElementColor(material.mainTexture, "KTestElement");
            material.mainTexture = texture2D;
            material.name = "matTestElement";
            return material;
        }

        public static void RegisterTestElemenSubstance()
        {
            Substance substance = Assets.instance.substanceTable.GetSubstance(SimHashes.Polypropylene);
            ElementUtil.CreateRegisteredSubstance("KTestElement", Element.State.Solid, ElementUtil.FindAnim("TestElement_kanim"), Test_Element.CreateTestElemenMaterial(substance.material), Test_Element.TESTELEMENT_COLOR);
        }

        public static readonly Color32 TESTELEMENT_COLOR = new Color32(201, 201, 195, byte.MaxValue);
        public const string TESTELEMENT_ID = "KTestElement";
        public static readonly SimHashes TestElemenSimHash = (SimHashes)Hash.SDBMLower("KTestElement");
    }
}
