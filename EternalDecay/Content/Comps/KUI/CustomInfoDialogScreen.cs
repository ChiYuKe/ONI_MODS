using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;
using CykUtils;

namespace EternalDecay.Content.Comps.KUI
{
    // 这个类用于封装 UI 元素的创建和设置
    public class SimpleDialogScreen : KScreen
    {
        // 私有引用，确保外部无法直接修改 UI 元素
        private GameObject layout;
        private GameObject header;
        private GameObject content;
        private GameObject titleLabel;
        private GameObject closeButton;
        private GameObject cancelButton;

        private TextMeshProUGUI titleTextComponent;



        private GameObject contentArea;
        private GameObject textContainer;

        // UI 常量
        private const float BackgroundWidth = 400f;
        private const float BackgroundHeight = 300f;
        private const float HeaderHeight = 24f;
        private const float ButtonSize = 24f;
        private const float LabelWidth = 380f;
        private const float LabelHeight = 40f;
        private const float IconSize = 15f;


        Color blackColor = new Color(0.1226f, 0.1337f, 0.1698f, 1f);
        Color redColor = new Color(0.498f, 0.2392f, 0.3686f, 1f);
        Color greyColor = new Color(0.2431f, 0.2627f, 0.3412f, 1f);


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            gameObject.name = "SimpleDialogScreen";

            // 调用私有方法，按顺序创建所有 UI 元素
            CreateUIElements();
        }

        // 核心创建方法，负责所有 UI 元素的实例化
        private void CreateUIElements()
        {

            string aaa = "aewrsgqwregqw rwertergtwerg ewgrqw3r gt3wr tgresg ewg erwgesg ewrg gerwg w4thtjytrjr65 jy yru6tyj 6yrjrt yjr5ty jtr5y jrt5yg jrye5u hert ert eryt yrtet5 yertyrteyertyhu rtyr5e yer5 jrtjuer 5ywe45rtyw35y er5t6ujrtyk uk,yutkyuj rtehreh wr5ty ewty wey wrtyesry wety ";

            var layout = new UIBuilder("layout", this.transform, new Vector2(400, 300))
                .AddVerticalLayout(0)
               

                .AddChild("Header", new Vector2(400, 24))
                    .AddLayoutElement(preferredHeight: 24, preferredWidth:400)
                    .AddImage(redColor, "web_box", Image.Type.Sliced)
                    .AddText("标题", 14, TextAlignmentOptions.Center, Color.white)
                .End()

                .AddChild("Content", new Vector2(398, 276))
                    .AddVerticalLayout(4)
                    .AddImage(blackColor, "web_box", Image.Type.Sliced)
                    .AddText(aaa, 18, TextAlignmentOptions.Center, Color.white)

                .AddChild("InnerButton", new Vector2(100, 30))
                    .AddLayoutElement(preferredHeight: 30, preferredWidth: 400)
                    .AddButton(() => Deactivate(), null, "web_button")
                        .AddText("内部按钮", 20, TextAlignmentOptions.Center, Color.white)
                    .End(2)



                .AddChild("CancelButton", new Vector2(100, 30))
                    .AddLayoutElement(preferredHeight: 30, preferredWidth: 400)
                    .AddButton(() => Deactivate(), null, "web_button")
                        .AddText("取消", 20, TextAlignmentOptions.Center, Color.white)
                    .End();













            //layout = CreateUIObject("Layout", this.transform, new Vector2(400f, 300f));
            //SetImageProperties(layout, blackColor, "web_box", Image.Type.Sliced);
            //layout.AddComponent<VerticalLayoutGroup>().spacing = 12;

            //layout.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;



            //header = CreateUIObject("Header", layout.transform, new Vector2(400f, 24f));
            //SetImageProperties(header, redColor, "web_box", Image.Type.Sliced);
            //header.AddComponent<LayoutElement>().preferredHeight = 24f;



            //content = CreateUIObject("Content", layout.transform, new Vector2(400f, 276f));
            //content.AddComponent<VerticalLayoutGroup>();

            //cancelButton = CreateUIObject("CancelButton", layout.transform, new Vector2(100f, 30f), new Vector2(1f, 0.5f), new Vector2(-10, -10));
            //cancelButton.AddComponent<LayoutElement>().preferredHeight = 30f;
            //SetImageProperties(cancelButton, greyColor, "web_button", Image.Type.Sliced);

            //// 创建按钮的文字子物体
            //var textGO = CreateUIObject("Text", cancelButton.transform, Vector2.zero);
            //var tmp = textGO.AddComponent<TextMeshProUGUI>();
            //tmp.text = "取消";
            //tmp.fontSize = 20;
            //tmp.alignment = TextAlignmentOptions.Center;


            //CykUtils.KButton kButton1 = cancelButton.AddComponent<CykUtils.KButton>();
            //kButton1.OnClick += () => Deactivate();







            //titleLabel = CreateUIObject("Label", header.transform, new Vector2(LabelWidth, LabelHeight));
            //titleTextComponent = titleLabel.AddComponent<TextMeshProUGUI>();
            //titleTextComponent.text = "测试标题";
            //titleTextComponent.fontSize = 14;
            //titleTextComponent.alignment = TextAlignmentOptions.Center;
            //titleTextComponent.color = Color.white;

            //closeButton = CreateUIObject("CloseButton", header.transform, new Vector2(ButtonSize, ButtonSize), new Vector2(1f, 0.5f), new Vector2(-12, 0));
            //SetImageProperties(closeButton, greyColor, "web_button", Image.Type.Sliced);

            //// 创建关闭按钮图标
            //GameObject closeIcon = CreateUIObject("CloseIcon", closeButton.transform, new Vector2(IconSize, IconSize));
            //SetImageProperties(closeIcon, Color.white, "web_x");

            //// 添加点击事件
            //FButton kButton = closeButton.AddComponent<FButton>();
            //kButton.OnClick += () => Deactivate();


            //// 在内容区域内创建文本对象
            //GameObject textObject = CreateUIObject("TextObject", content.transform, new Vector2(BackgroundWidth, BackgroundHeight));  // 调整大小
            //TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();

            //// 设置文本内容
            //textComponent.text = "这里是一可以可以显示更多内容这里是一些大以显示更多内容这里是一些大段文本，您可以显示更多内容这里是一些大段文本，您可以显示更多内容";

            //// 设置字体大小、颜色等
            //textComponent.fontSize = 20;
            //textComponent.alignment = TextAlignmentOptions.TopLeft;
            //textComponent.color = Color.white;





            // 如果需要显示更多文本，可以设置内容区域的大小
            // textObject.GetComponent<RectTransform>().sizeDelta = new Vector2(BackgroundWidth - 40, 200);  // 设置高度和宽度







            //// 在 contentArea 区域内填充本地图片
            //GameObject imageObject = CreateUIObject("ImageInContentArea", contentArea.transform, new Vector2(contentArea.GetComponent<RectTransform>().rect.width, contentArea.GetComponent<RectTransform>().rect.height));

            //// 为图像添加 Image 组件
            //Image image = imageObject.AddComponent<Image>();


            //Texture2D texture = LoadTextureFromFile("1111");  // 加载纹理
            //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));  // 创建 Sprite

            //image.sprite = sprite;
            //image.type = Image.Type.Simple;  // 设置图片显示方式
            //image.preserveAspect = true;  // 保持图片比例









        }

        // --- 辅助方法 ---


        // 加载本地图片并返回一个 Texture2D 对象
        private Texture2D LoadTextureFromFile(string TexttureName)
        {
            var path = Path.Combine(Path.Combine(KUtils.AssetsPath,"textures"), $"{TexttureName}.png");
            if (File.Exists(path))
            {
                byte[] fileData = File.ReadAllBytes(path);  // 读取文件
                Texture2D texture = new Texture2D(2, 2);  // 创建一个新的纹理对象
                texture.LoadImage(fileData);  // 使用 LoadImage 方法加载图片数据
                return texture;
            }
            else
            {
                Debug.LogError("文件不存在：" + path);
                return null;
            }
        }



        /// <summary>
        /// 创建一个带有 RectTransform 的 GameObject
        /// </summary>
        private GameObject CreateUIObject(string name, Transform parent, Vector2 size, Vector2 anchor = default, Vector2 anchoredPosition = default)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent, false);

            RectTransform rect = obj.AddComponent<RectTransform>();
            rect.sizeDelta = size;
            rect.anchorMin = rect.anchorMax = anchor != default ? anchor : new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;

            return obj;
        }

        /// <summary>
        /// 为 GameObject 添加并设置 Image 组件
        /// </summary>
        private void SetImageProperties(GameObject obj, Color color, string spriteName, Image.Type type = Image.Type.Simple)
        {
            Image image = obj.AddComponent<Image>();
            image.color = color;
            image.sprite = Assets.GetSprite(spriteName);
            image.type = type;
        }

        // --- 公共方法 ---

        /// <summary>
        /// 设置弹窗的标题文本
        /// </summary>
        public void SetTitle(string title)
        {
            if (titleTextComponent != null)
            {
                titleTextComponent.text = title;
            }
        }

        /// <summary>
        /// 设置背景颜色
        /// </summary>
        public void SetBackgroundColor(Color color)
        {
            if (content != null)
                content.GetComponent<Image>().color = color;
        }

        /// <summary>
        /// 设置 Header 颜色
        /// </summary>
        public void SetHeaderColor(Color color)
        {
            if (header != null)
                header.GetComponent<Image>().color = color;
        }

        public GameObject GetBackground() => content;
        public GameObject GetHeader() => header;
        public GameObject GetLabel() => titleLabel;
        public GameObject GetCloseButton() => closeButton;
    }
}
