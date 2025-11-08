using UnityEngine;
using UnityEngine.UI;
using PeterHan.PLib.UI;
using System;
using System.Collections.Generic;
using KMod;


namespace EternalDecay.Content.Comps.KUI
{
    public class InheritanceInformation : KScreen
    {
        protected override void OnSpawn()
        {
            base.OnSpawn();
            ConsumeMouseScroll = true; 

        }

        public static GameObject Createpanel(
            List<(string attrName, int oldLevel, int newLevel)> attrList, 
            List<(string attrName, int oldLevel, int newLevel)> skillList,
            List<(string attrName, int oldLevel, int newLevel)> traitList)
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
                Text = Configs.STRINGS.UI.INHERITANCEINFORMATION.TOPTEXT,
                TextStyle = PUITuning.Fonts.TextLightStyle,
                Margin = new RectOffset(5, 5, 5, 5)
            });

            // 中间滚动部分
            var middleContent = new PPanel("MiddleContent")
            {
                Direction = PanelDirection.Vertical,
                Spacing = 10,
                FlexSize = new Vector2(1, 0)
            };
            middleContent.AddChild(CreateAttributeBlock(attrList));
            middleContent.AddChild(CreateSkillBlock(skillList));
            middleContent.AddChild(CreateTraitBlock(traitList));

            var middle = new PScrollPane("MiddleScroll")
            {
                Child = middleContent,
                ScrollVertical = true,
                ScrollHorizontal = false,
                AlwaysShowVertical = true,
                FlexSize = new Vector2(1, 1),
                TrackSize = 8f,
                BackColor = Color.white
            };

            // 底部按钮
            var bottom = new PPanel("Bottom")
            {
                Direction = PanelDirection.Horizontal,
                Alignment = TextAnchor.MiddleCenter,
                FlexSize = new Vector2(1, 0),
            }.AddChild(new PButton("OkButton")
            {
                Text = Configs.STRINGS.UI.INHERITANCEINFORMATION.BUTTONTEXT,
                FlexSize = new Vector2(1, 0),
                OnClick = new PUIDelegates.OnButtonPressed(close)
            });

            root.AddChild(top).AddChild(middle).AddChild(bottom);

            return root.Build();
        }





        /// <summary>
        /// 创建“属性”区块 (标题 + 属性卡片列表)
        /// </summary>
        private static PPanel CreateAttributeBlock(List<(string attrName, int oldLevel, int newLevel)> attrList)
        {
            // 外层容器：标题 + 内容
            var wrapper = new PPanel("AttrWrapper")
            {
                Direction = PanelDirection.Vertical,
                Alignment = TextAnchor.UpperLeft,
                FlexSize = new Vector2(1, 0),
                Margin = new RectOffset(5, 10, 5, 5),
                Spacing = 4,
                BackImage = PUITuning.Images.GetSpriteByName("web_box"),
                ImageMode = Image.Type.Sliced,
                BackColor = new Color(0.24f, 0.3f, 0.4072f, 1f)
            };

            // 标题
            wrapper.AddChild(new PLabel("AttrTitle")
            {
                Text = Configs.STRINGS.UI.INHERITANCEINFORMATION.ATTRIBUTESTITLE.NAME,
                TextStyle = CustomStyles.BigLightStyle,

                ToolTip = Configs.STRINGS.UI.INHERITANCEINFORMATION.ATTRIBUTESTITLE.DESC,
                TextAlignment = TextAnchor.UpperCenter,
                Margin = new RectOffset(2, 2, 2, 4)
            });

            // 内容区域：放卡片
            var content = new PPanel("AttrContent")
            {
                Direction = PanelDirection.Vertical,
                Alignment = TextAnchor.UpperLeft,
                FlexSize = new Vector2(1, 0),
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
                // 属性名
                .AddChild(new PLabel($"Name_{index}")
                {
                    Text = attrName,
                    TextStyle = PUITuning.Fonts.TextDarkStyle,
                    FlexSize = new Vector2(1, 0),
                    TextAlignment = TextAnchor.MiddleLeft
                })
                // 旧等级
                .AddChild(new PLabel($"Old_{index}")
                {
                    Text = $"Lv.{oldLevel}",
                    TextStyle = PUITuning.Fonts.TextLightStyle,
                    TextAlignment = TextAnchor.MiddleCenter
                })
                // 箭头
                .AddChild(new PLabel($"Arrow_{index}")
                {
                    Text = "→",
                    TextStyle = newLevel > oldLevel ? CustomStyles.GreenText : PUITuning.Fonts.TextDarkStyle,
                    TextAlignment = TextAnchor.MiddleCenter,
                    Margin = new RectOffset(5, 5, 0, 0)
                })
                // 新等级
                .AddChild(new PLabel($"New_{index}")
                {
                    Text = $"Lv.{newLevel}",
                    TextStyle = newLevel > oldLevel ? CustomStyles.GreenText : PUITuning.Fonts.TextDarkStyle,
                    TextAlignment = TextAnchor.MiddleRight
                });

                content.AddChild(card);
                index++;
            }

            wrapper.AddChild(content);
            return wrapper;
        }


        // 技能列表
        private static PPanel CreateSkillBlock(List<(string attrName, int oldLevel, int newLevel)> skillList) 
        {
            var wrapper = new PPanel("SkillWrapper")
            {
                Direction = PanelDirection.Vertical,
                Alignment = TextAnchor.UpperLeft,
                FlexSize = new Vector2(1, 0),
                Margin = new RectOffset(5, 10, 5, 5),
                Spacing = 4,
                BackImage = PUITuning.Images.GetSpriteByName("web_box"),
                ImageMode = Image.Type.Sliced,
                BackColor = new Color(0.24f, 0.3f, 0.4072f, 1f)
            };

            // 标题
            wrapper.AddChild(new PLabel("SkillTitle")
            {
                Text = Configs.STRINGS.UI.INHERITANCEINFORMATION.SKILLSTITLE.NAME,
                ToolTip = Configs.STRINGS.UI.INHERITANCEINFORMATION.SKILLSTITLE.DESC,

                TextStyle = CustomStyles.BigLightStyle,
                TextAlignment = TextAnchor.UpperCenter,
                Margin = new RectOffset(2, 2, 2, 4)
            });

            return wrapper;


        }

        // 特质列表
        private static PPanel CreateTraitBlock(List<(string attrName, int oldLevel, int newLevel)> traitList)
        {
            var wrapper = new PPanel("TraitWrapper")
            {
                Direction = PanelDirection.Vertical,
                Alignment = TextAnchor.UpperLeft,
                FlexSize = new Vector2(1, 0),
                Margin = new RectOffset(5, 10, 5, 5),
                Spacing = 4,
                BackImage = PUITuning.Images.GetSpriteByName("web_box"),
                ImageMode = Image.Type.Sliced,
                BackColor = new Color(0.24f, 0.3f, 0.4072f, 1f)
            };
            // 标题
            wrapper.AddChild(new PLabel("TraitTitle")
            {
                Text = Configs.STRINGS.UI.INHERITANCEINFORMATION.TRAITSTITLE.NAME,
                ToolTip = Configs.STRINGS.UI.INHERITANCEINFORMATION.TRAITSTITLE.DESC,

                TextStyle = CustomStyles.BigLightStyle,
                TextAlignment = TextAnchor.UpperCenter,
                Margin = new RectOffset(2, 2, 2, 4)
            });
            return wrapper;
            }



        // border_basic_white  web_rounded

        private static void close(GameObject _)
        {
            // Application.Quit(); 这是关闭游戏
            UnityEngine.Object.Destroy(GameObject.Find("InheritanceInfo"));

        }

    }

    // 缓存，避免重复 new
    public static class CustomStyles
    {
        public static readonly TextStyleSetting GreenText;
        public static readonly TextStyleSetting RedText;
        public static readonly TextStyleSetting BigLightStyle;

        static CustomStyles()
        {
            // 基于 PUITuning.Fonts.TextDarkStyle 派生
            GreenText = PUITuning.Fonts.TextDarkStyle.DeriveStyle(0, new Color(1f, 1f, 0f, 1f));
            RedText = PUITuning.Fonts.TextDarkStyle.DeriveStyle(0, Color.red);
            BigLightStyle = PUITuning.Fonts.TextLightStyle.DeriveStyle(
                size: 20,   
                newColor: new Color(1f, 1f, 1f, 1f)  
            );

        }

    }

}
