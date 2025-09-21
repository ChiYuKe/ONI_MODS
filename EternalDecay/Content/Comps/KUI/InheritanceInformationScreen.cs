using UnityEngine;
using UnityEngine.UI;
using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;


namespace EternalDecay.Content.Comps.KUI
{
    public class InheritanceInformation : KScreen
    {
        protected override void OnSpawn()
        {
            base.OnSpawn();
            ConsumeMouseScroll = true;  // 这句告诉 KScreen 阻止滚轮事件

        }

        public static GameObject Createpanel(string topText, List<(string attrName, int oldLevel, int newLevel)> attrList, string bottomButtonText)
        {
            // 外层容器
            PPanel root = new PPanel("EDPanel")
            {
                Direction = PanelDirection.Vertical,
                Alignment = TextAnchor.UpperCenter,
                BackImage = PUITuning.Images.GetSpriteByName("web_box"),
                BackColor = new Color(0.7882f, 0.7882f, 0.7882f, 1f),
                ImageMode = Image.Type.Sliced,
                FlexSize = Vector2.one
            };

            // 顶部
            var top = new PPanel("Top")
            {
                Direction = PanelDirection.Horizontal,
                Alignment = TextAnchor.MiddleCenter,
                FlexSize = new Vector2(1, 0),
                BackImage = PUITuning.Images.GetSpriteByName("web_box"),
                BackColor = PUITuning.Colors.ButtonPinkStyle.inactiveColor,
                ImageMode = Image.Type.Sliced,
            }.AddChild(new PLabel("TopLabel")
            {
                Text = topText,
                TextStyle = PUITuning.Fonts.TextLightStyle,
                Margin = new RectOffset(5, 5, 5, 5)
            });

            // 中间滚动部分
            var middleContent = new PPanel("MiddleContent")
            {
                Direction = PanelDirection.Vertical,
                Alignment = TextAnchor.UpperLeft,
                FlexSize = new Vector2(1, 0),
                Margin = new RectOffset(5, 10, 2, 2),
                Spacing = 2
            };

            int index = 0;
            foreach (var (attrName, oldLevel, newLevel) in attrList)
            {
                var card = new PPanel($"AttrCard_{index}")
                {
                    Direction = PanelDirection.Horizontal,
                    Alignment = TextAnchor.MiddleCenter,
                    Spacing = 10,
                    FlexSize = new Vector2(1, 0),
                    BackImage = PUITuning.Images.GetSpriteByName("web_rounded"),
                    ImageMode = Image.Type.Sliced,
                    BackColor = new Color(0.75f, 0.75f, 0.75f, 1f),
                    Margin = new RectOffset(5, 5, 2, 2)
                }
                // 左边：属性名
                .AddChild(new PLabel($"Name_{index}")
                {
                    Text = attrName,
                    TextStyle = PUITuning.Fonts.TextDarkStyle,
                    FlexSize = new Vector2(1, 0),
                    TextAlignment = TextAnchor.MiddleLeft
                })
                // 中间：旧等级
                .AddChild(new PLabel($"Old_{index}")
                {
                    Text = $"Lv.{oldLevel}",
                    TextStyle = PUITuning.Fonts.TextLightStyle,
                    TextAlignment = TextAnchor.MiddleCenter,
                    FlexSize = new Vector2(0, 0)
                })
                // 箭头
                .AddChild(new PLabel($"Arrow_{index}")
                {
                    Text = "→",
                    TextStyle = newLevel > oldLevel
                        ? CustomStyles.GreenText   // 升级 → 绿色
                        : PUITuning.Fonts.TextDarkStyle,     // 没变/降低
                    FlexSize = new Vector2(0, 0), 
                    TextAlignment = TextAnchor.MiddleCenter,
                    Margin = new RectOffset(5, 5, 0, 0)
                })
                // 新等级
                .AddChild(new PLabel($"New_{index}")
                {
                    Text = $"Lv.{newLevel}",
                    TextStyle = newLevel > oldLevel
                        ? CustomStyles.GreenText   // 升级 → 绿色
                        : PUITuning.Fonts.TextDarkStyle,     // 没变/降低

                    TextAlignment = TextAnchor.MiddleRight,
                    FlexSize = new Vector2(0, 0)
                });

                middleContent.AddChild(card);
                index++;
            }

            var middle = new PScrollPane("MiddleScroll")
            {
                Child = middleContent,
                ScrollVertical = true,
                ScrollHorizontal = false,
                AlwaysShowVertical = true,
                FlexSize = new Vector2(1, 1),
                TrackSize = 8f,
                BackColor = new Color(1f, 1f, 1f, 1f)
            };

            // 底部按钮
            var bottom = new PPanel("Bottom")
            {
                Direction = PanelDirection.Horizontal,
                Alignment = TextAnchor.MiddleCenter,
                FlexSize = new Vector2(1, 0),
            }.AddChild(new PButton("OkButton")
            {
                Text = bottomButtonText,
                FlexSize = new Vector2(1, 0),
                OnClick = new PUIDelegates.OnButtonPressed(close)
            });

            root.AddChild(top).AddChild(middle).AddChild(bottom);

            return root.Build();
        }

         public static TextStyleSetting TextLightStyle { get; }

        // border_basic_white web_rounded

        private static void close(GameObject _)
        {
            // Application.Quit(); 这是关闭游戏
            UnityEngine.Object.Destroy(GameObject.Find("InheritanceInfo"));

        }
    }

    // 工具类里做一次缓存，避免重复 new
    public static class CustomStyles
    {
        public static readonly TextStyleSetting GreenText;
        public static readonly TextStyleSetting RedText;

        static CustomStyles()
        {
            // 基于 PUITuning.Fonts.TextDarkStyle 派生
            GreenText = PUITuning.Fonts.TextDarkStyle.DeriveStyle(0, new Color(1f, 1f, 0f, 1f));
            RedText = PUITuning.Fonts.TextDarkStyle.DeriveStyle(0, Color.red);
        }
    }


}
