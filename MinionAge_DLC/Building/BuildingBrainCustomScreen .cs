using PeterHan.PLib.UI;
using UnityEngine;
using UnityEngine.UI;


namespace DebuffRoulette
{
    public class CustomSideScreen : SideScreenContent
    {
        private PButton myButton; // 用于存储按钮
       



        public CustomSideScreen()
        {
            titleKey = "DefaultTitleKey"; // 设置默认标题键
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
          
            CreateButton(); // 添加按钮
        }

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<MinionBrainCounter>() != null;
        }


        public override void SetTarget(GameObject target)
        {
            
        }

        private void Refresh()
        {
        }


        // 用于创建和配置按钮
        private void CreateButton()
        {
            // 创建一个按钮实例
            myButton = new PButton("MyButton")
            {
                Text = "点击!",
                Color = PUITuning.Colors.ButtonPinkStyle,
                OnClick = OnButtonClick
            }.SetKleiBlueStyle(); // 配置按钮样式

            // 创建按钮并将其添加到容器中
            GameObject buttonObject = myButton.Build();
            RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(200, 500); // 设置按钮的宽度和高度

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

        // 按钮点击事件处理
        private void OnButtonClick(GameObject button)
        {
            // TODO: 添加按钮点击时的逻辑
            Debug.Log("Button clicked!");
        }


    }

}
