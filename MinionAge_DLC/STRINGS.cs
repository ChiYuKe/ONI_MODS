using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MinionAge_DLC
{
    public class STRINGS
    {
        public class BUILDINGS
        {
            public class ENERGYDISPERSIONTABLECONIFG
            {
                public static LocString NAME = "散能台";
                public static LocString DESC = "什么？你这走的是科技路线？";
                public static LocString EFFECT = "散发其中的神秘能量";
            }
        }
        public class ELEMENTS
        {

            public class TESTELEMENT
            {
                public static LocString NAME = UI.FormatAsLink("紫金", "TestElement");
                public static LocString DESC = UI.FormatAsBold("此元素为 MinionAge 模组独有元素") + " \n\n 紫金是一种多功能实用性消耗物质";
               
            }

        }

        public class KFOOD
        {
            public class XIAOHUANGYA
            {
                public static readonly LocString NAME = "小黄鸭紫莓包";
                public static readonly LocString DESC = "感觉不如原神";
                public static readonly LocString RECIPEDESC = "感觉不如原神。。。画质";
            }
        }








        public class MISC
        {

            public class NOTIFICATIONS
            {
                public class DEBUFFROULETTE
                {
                    public static LocString NAME = "被给予Debuff";
                }

            }
        }

        public class UI
        {

            public static string FormatAsHotkey(string text)
            {
                return "<b><color=#F44A4A>" + text + "</b></color>";
            }

            public static string FormatAsBold(string text)
            {
                return "<b>" + text + "</b>";
            }
            public static string FormatAsLink(string text, string linkID)
            {
                text = StripLinkFormatting(text);
                linkID = CodexCache.FormatLinkID(linkID);
                return "<link=\"" + linkID + "\">" + text + "</link>";
            }

        }
        public static string StripLinkFormatting(string text)
        {
            string text2 = text;
            try
            {
                while (text2.Contains("<link="))
                {
                    int num = text2.IndexOf("</link>");
                    if (num > -1)
                    {
                        text2 = text2.Remove(num, 7);
                    }
                    else
                    {
                        Debug.LogWarningFormat("String has no closing link tag: {0}", text);
                    }

                    int num2 = text2.IndexOf("<link=");
                    if (num2 != -1)
                    {
                        int num3 = text2.IndexOf("\">", num2);
                        if (num3 != -1)
                        {
                            text2 = text2.Remove(num2, num3 - num2 + 2);
                            continue;
                        }

                        text2 = text2.Remove(num2, "<link=".Length);
                        Debug.LogWarningFormat("String has no open link closure: {0}", text);
                    }
                    else
                    {
                        Debug.LogWarningFormat("String has no open link tag: {0}", text);
                    }
                }
            }
            catch
            {
                Debug.Log("STRIP LINK FORMATTING FAILED ON: " + text);
                text2 = text;
            }

            return text2;
        }
    }
}
