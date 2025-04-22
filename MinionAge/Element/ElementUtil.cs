using System;
using HarmonyLib;
using STRINGS;
using UnityEngine;

namespace TestElement
{
    internal class ElementUtil
    {
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
