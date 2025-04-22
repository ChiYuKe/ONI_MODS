using PeterHan.PLib.UI;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
namespace DebuffRoulette
{
    public class BuildingBrainSideScreen : SideScreenContent
    {
        public BuildingBrainSideScreen()
        {
            titleKey = "测试界面"; // 设置默认标题键
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            // 创建并设置按钮
            PButton button = new PButton("MyButton")
            {
                Text = "点击!",
                ToolTip = "这是一个按钮",
                OnClick = OnButtonClicked,
            }.SetKleiBlueStyle();

            // 构建按钮并添加到侧屏内容容器中
            buttonObject = button.Build();

            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 50); // 设置按钮的宽度和高度

            if (ContentContainer != null)
            {
                buttonObject.transform.SetParent(ContentContainer.transform, false);
            }
            else
            {
                buttonObject.transform.SetParent(transform, false);
                Debug.LogWarning("ContentContainer 为空，默认转换.");
            }
        }

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<BuildingBrain>() != null;
        }

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);
            if (target != null)
            {
                Debug.Log($"设置目标: {target.name}");
                this.target = target.GetComponent<BuildingBrain>();
                Refresh();
            }
            else
            {
                Debug.LogWarning("目标为空，无法设置。");
            }
        }

        private void Refresh()
        {
            if (this.target == null)
            {
                Debug.Log("当前目标为空.");
                if (buttonObject != null)
                {
                    buttonObject.SetActive(false);
                }
                return;
            }

            if (buttonObject == null)
            {
                Debug.LogError("buttonObject 未被初始化.");
                return;
            }

            if (this.target.WorkComplete)
            {
                buttonObject.SetActive(true);
               
                Debug.Log("工作完成.");
            }
            else if (this.target.IsConsumed)
            {
                Debug.Log("已消耗.");
                buttonObject.SetActive(true);

                if (this.target.RechargeRequested)
                {
                    Debug.Log("请求补充.");
                }
            }
            else
            {
                Debug.Log("未消耗.");
                if (this.target.IsWorking)
                {
                    buttonObject.SetActive(false);
                    Debug.Log("正在工作.");
                }
                else
                {
                    buttonObject.SetActive(true);
                    Debug.Log("不在工作状态.");
                }
            }
        }

        private void OnButtonClicked(GameObject obj)
        {
            if (this.target == null)
            {
                Debug.LogWarning("按钮点击时目标为空.");
                return;
            }

            if (this.target.WorkComplete)
            {
                this.target.SetWorkTime(0f);
                Debug.Log("停止工作");
            }
            else if (this.target.IsConsumed) // 已经消耗
            {
                Debug.Log("开始配送大脑");
                this.target.RequestRecharge(!this.target.RechargeRequested); // 开始运送大脑
               
                Refresh();
            }
        }




        private BuildingBrain target;
        private GameObject buttonObject;
    }







    //public class MyWindow : PContainer
    //{
    //    public MyWindow(string name) : base(name) { }

    //    public override GameObject Build()
    //    {
    //        // 创建窗口 GameObject
    //        GameObject window = new GameObject(Name);
    //        SetImage(window);

    //        // 设置窗口的 RectTransform
    //        RectTransform rt = window.AddComponent<RectTransform>();
    //        rt.sizeDelta = new Vector2(800, 600); // 设置窗口大小

    //        // 创建并添加 CardLayoutGroup
    //        CardLayoutGroup cardLayoutGroup = window.AddComponent<CardLayoutGroup>();
    //        cardLayoutGroup.Margin = new RectOffset(10, 10, 10, 10); // 设置卡片间距

    //        // 创建并添加 Card 实例到 CardLayoutGroup
    //        for (int i = 0; i < 5; i++) // 添加 5 个卡片
    //        {
    //            Card card = new Card($"Card_{i}");
    //            GameObject cardObject = card.Build();
    //            cardObject.transform.SetParent(cardLayoutGroup.transform, false);
    //        }

    //        // 添加关闭按钮
    //        PButton closeButton = new PButton("CloseButton")
    //        {
    //            Text = "Close",
    //            OnClick = CloseWindow
    //        }.SetKleiBlueStyle();
    //        GameObject closeButtonObject = closeButton.Build();
    //        closeButtonObject.transform.SetParent(window.transform, false);
    //        RectTransform closeButtonRt = closeButtonObject.GetComponent<RectTransform>();
    //        closeButtonRt.anchoredPosition = new Vector2(0, -280); // 设置关闭按钮位置

    //        return window;
    //    }

    //    private void CloseWindow(GameObject obj)
    //    {
    //        UnityEngine.Object.Destroy(obj.transform.parent.gameObject); // 关闭窗口
    //    }
    //}

    //public class Card : PContainer
    //{
    //    public Card(string name) : base(name) { }

    //    public override GameObject Build()
    //    {
    //        // 创建 GameObject 实例
    //        GameObject card = new GameObject(Name);

    //        // 添加并设置 Image 组件或其他 UI 组件
    //        SetImage(card);

    //        RectTransform rt = card.AddComponent<RectTransform>();
    //        rt.sizeDelta = new Vector2(150, 100); // 设置卡片大小

    //        // 添加其他 UI 组件到卡片
    //        PButton cardButton = new PButton("CardButton")
    //        {
    //            Text = "Card Button",
    //            OnClick = OnCardButtonClicked
    //        }.SetKleiBlueStyle();
    //        GameObject cardButtonObject = cardButton.Build();
    //        cardButtonObject.transform.SetParent(card.transform, false);

    //        return card;
    //    }

    //    private void OnCardButtonClicked(GameObject obj)
    //    {
    //        Debug.Log("Card Button Clicked!");
    //    }
    //}


















    //// 新窗口类
    //public class MyPopupWindow
    //{
    //    public static void CreateAndShowWindow()
    //    {
    //        // 创建一个新的空对象作为窗口
    //        GameObject window = new GameObject("MyPopupWindow");

    //        // 为窗口添加 Canvas 组件，并设置渲染模式为屏幕空间 - 相机（可以用 Overlay 或 WorldSpace 进行测试）
    //        Canvas canvas = window.AddComponent<Canvas>();
    //        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

    //        // 禁用 Canvas 的自动调整
    //        CanvasScaler scaler = window.AddComponent<CanvasScaler>();
    //        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

    //        // 添加一个用于交互的 GraphicRaycaster
    //        window.AddComponent<GraphicRaycaster>();

    //        // 创建一个半透明的黑色背景
    //        GameObject background = new GameObject("Background");
    //        background.transform.SetParent(window.transform);
    //        Image bgImage = background.AddComponent<Image>();
    //        bgImage.color = new Color(0, 0, 0, 0.8f);  // 半透明黑色背景

    //        RectTransform bgRect = background.GetComponent<RectTransform>();
    //        bgRect.sizeDelta = new Vector2(300, 200);  // 固定大小
    //        bgRect.anchorMin = bgRect.anchorMax = new Vector2(0.5f, 0.5f);  // 中心锚点
    //        bgRect.pivot = new Vector2(0.5f, 0.5f);  // 使其居中
    //        bgRect.anchoredPosition = Vector2.zero;  // 居中

    //        // 添加一个标题文本
    //        GameObject titleText = new GameObject("TitleText");
    //        titleText.transform.SetParent(background.transform);
    //        Text text = titleText.AddComponent<Text>();
    //        text.text = "New Window";
    //        text.color = Color.white;
    //        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); // 使用内置字体
    //        text.alignment = TextAnchor.MiddleCenter;

    //        RectTransform textRect = titleText.GetComponent<RectTransform>();
    //        textRect.sizeDelta = new Vector2(280, 40);  // 适应窗口大小
    //        textRect.anchorMin = textRect.anchorMax = new Vector2(0.5f, 1);
    //        textRect.pivot = new Vector2(0.5f, 1);
    //        textRect.anchoredPosition = new Vector2(0, -10);

    //        // 添加关闭按钮
    //        PButton closeButton = new PButton("CloseButton")
    //        {
    //            Text = "Close",
    //            OnClick = obj =>
    //            {
    //                Object.Destroy(window);  // 销毁窗口
    //            }
    //        }.SetKleiPinkStyle();

    //        GameObject closeButtonObject = closeButton.Build();
    //        closeButtonObject.transform.SetParent(background.transform, false);

    //        RectTransform closeButtonRect = closeButtonObject.GetComponent<RectTransform>();
    //        closeButtonRect.sizeDelta = new Vector2(100, 40);
    //        closeButtonRect.anchorMin = closeButtonRect.anchorMax = new Vector2(0.5f, 0);
    //        closeButtonRect.pivot = new Vector2(0.5f, 0);
    //        closeButtonRect.anchoredPosition = new Vector2(0, 20);  // 按钮位于窗口底部
    //    }
    //}
}
