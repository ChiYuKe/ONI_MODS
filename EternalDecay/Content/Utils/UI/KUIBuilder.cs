using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CykUtils
{
    public class UIBuilder
    {
        public GameObject GameObject { get; private set; }
        public RectTransform Rect => GameObject.GetComponent<RectTransform>();
        public UIBuilder Parent { get; private set; }


        // 支持隐式转换
        public static implicit operator GameObject(UIBuilder builder) => builder.GameObject;

        public UIBuilder(string name, Transform parent = null, Vector2? size = null, UIBuilder parentBuilder = null)
        {
            GameObject = new GameObject(name);
            if (parent != null)
                GameObject.transform.SetParent(parent, false);

            var rect = GameObject.AddComponent<RectTransform>();
            rect.sizeDelta = size ?? Vector2.zero;
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            Parent = parentBuilder;

            Debug.Log($"[UIBuilder] 创建 GameObject: {name}, size: {rect.sizeDelta}");
        }

        // 返回父节点，链式结束
        public UIBuilder End() => Parent ?? this;
        public UIBuilder End(int levels = 1)
        {
            UIBuilder current = this;
            for (int i = 0; i < levels && current.Parent != null; i++)
                current = current.Parent;
            return current;
        }


        public UIBuilder EndToRoot()
        {
            UIBuilder current = this;
            while (current.Parent != null)
                current = current.Parent;
            return current;
        }


        public UIBuilder SetSize(Vector2 size)
        {
            Rect.sizeDelta = size;
            return this;
        }

        public UIBuilder SetAnchoredPosition(Vector2 pos)
        {
            Rect.anchoredPosition = pos;
            return this;
        }

        // Image 自动添加到本 GameObject
        public UIBuilder AddImage(Color color, string sprite = null, Image.Type type = Image.Type.Simple)
        {
            var img = GameObject.GetComponent<Image>() ?? GameObject.AddComponent<Image>();
            img.color = color;
            img.sprite = sprite != null ? Assets.GetSprite(sprite) : img.sprite;
            img.type = type;

            Debug.Log($"[UIBuilder] AddImage: color={color}, sprite={sprite}, type={type}");
            return this;
        }

        // Text 自动创建子物体
        public UIBuilder AddText(string text, int fontSize = 14, TextAlignmentOptions align = TextAlignmentOptions.Center, Color? color = null)
        {
            var textGO = new GameObject("Text");
            textGO.transform.SetParent(GameObject.transform, false);

            var tmp = textGO.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.alignment = align;
            tmp.color = color ?? Color.white;
            tmp.raycastTarget = false;

            if (tmp.font == null)
                tmp.font = TMPro.TMP_Settings.defaultFontAsset
                           ?? Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");

            Debug.Log($"[UIBuilder] AddText: '{text}', fontSize={fontSize}, align={align}, color={tmp.color}");
            return this;
        }

        // Button 自动创建子物体 Image
        public UIBuilder AddButton(System.Action onClick, Color? bgColor = null, string sprite = null , Image.Type type = Image.Type.Sliced)
        {
            // 如果没有 Image，则自动创建一个子 Image
            var img = GameObject.GetComponent<Image>();
            if (img == null)
                AddImage(bgColor ?? new Color(0.243f, 0.263f, 0.341f), sprite, type);

            var btn = GameObject.GetComponent<KButton>() ?? GameObject.AddComponent<KButton>();
            btn.OnClick += () => onClick?.Invoke();

            Debug.Log($"[UIBuilder] AddButton: bgColor={bgColor}, sprite={sprite}");
            return this;
        }

        public UIBuilder AddVerticalLayout(float spacing = 0)
        {
            var layout = GameObject.GetComponent<VerticalLayoutGroup>() ?? GameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = spacing;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            return this;
        }

        public UIBuilder AddHorizontalLayout(float spacing = 0)
        {
            var layout = GameObject.GetComponent<HorizontalLayoutGroup>() ?? GameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            return this;
        }

        public UIBuilder AddLayoutElement(float preferredWidth = -1, float preferredHeight = -1, float flexibleWidth = 0, float flexibleHeight = 0)
        {
            var le = GameObject.GetComponent<LayoutElement>() ?? GameObject.AddComponent<LayoutElement>();
            if (preferredWidth >= 0) le.preferredWidth = preferredWidth;
            if (preferredHeight >= 0) le.preferredHeight = preferredHeight;
            le.flexibleWidth = flexibleWidth;
            le.flexibleHeight = flexibleHeight;
            return this;
        }

        public UIBuilder AddScrollRect(UIBuilder content, bool horizontal = false, bool vertical = true)
        {
            var scroll = GameObject.GetComponent<ScrollRect>() ?? GameObject.AddComponent<ScrollRect>();
            scroll.content = content.Rect;
            scroll.horizontal = horizontal;
            scroll.vertical = vertical;

            var mask = GameObject.GetComponent<Mask>() ?? GameObject.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            if (GameObject.GetComponent<Image>() == null)
                AddImage(Color.clear);

            return this;
        }

        // 添加子物体并返回子 UIBuilder
        public UIBuilder AddChild(string name, Vector2? size = null)
        {
            return new UIBuilder(name, GameObject.transform, size, this);
        }
    }
}
