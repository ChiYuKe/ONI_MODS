using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CykUtils
{
    // Token: 0x020002FD RID: 765
    public class ButtonUtil : KMonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x0600096B RID: 2411 RVA: 0x0002A71C File Offset: 0x0002891C
        // (remove) Token: 0x0600096C RID: 2412 RVA: 0x0002A754 File Offset: 0x00028954
        public event global::System.Action OnClick;

        // Token: 0x0600096D RID: 2413 RVA: 0x0002A789 File Offset: 0x00028989
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.material = this.image.material;
            this.interactable = true;
        }

        // Token: 0x0600096E RID: 2414 RVA: 0x0002A7AC File Offset: 0x000289AC
        public void SetInteractable(bool interactable)
        {
            if (interactable == this.interactable)
            {
                return;
            }
            this.interactable = interactable;
            if (this.button == null)
            {
                this.image.color = (interactable ? this.normalColor : this.disabledColor);
                return;
            }
            this.button.interactable = interactable;
        }

        // Token: 0x0600096F RID: 2415 RVA: 0x0002A801 File Offset: 0x00028A01
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!this.interactable)
            {
                return;
            }
            if (KInputManager.isFocused)
            {
                KInputManager.SetUserActive();
               //  KMonoBehaviour.PlaySound(UISoundHelper.ClickOpen);
                global::System.Action onClick = this.OnClick;
                if (onClick == null)
                {
                    return;
                }
                onClick();
            }
        }

        // Token: 0x06000970 RID: 2416 RVA: 0x0002A832 File Offset: 0x00028A32
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!this.interactable)
            {
                return;
            }
            if (KInputManager.isFocused)
            {
                if (this.button == null)
                {
                    this.image.color = this.hoverColor;
                }
                KInputManager.SetUserActive();
               //  KMonoBehaviour.PlaySound(UISoundHelper.MouseOver);
            }
        }

        // Token: 0x06000971 RID: 2417 RVA: 0x0002A872 File Offset: 0x00028A72
        public void OnPointerExit(PointerEventData eventData)
        {
            if (this.button == null)
            {
                this.image.color = this.normalColor;
            }
        }

        // Token: 0x04000881 RID: 2177
        private bool interactable;

        // Token: 0x04000882 RID: 2178
        private Material material;

        // Token: 0x04000883 RID: 2179
        [MyCmpReq]
        private Image image;

        // Token: 0x04000884 RID: 2180
        [MyCmpGet]
        private Button button;

        // Token: 0x04000885 RID: 2181
        [SerializeField]
        public Color disabledColor = new Color(0.78f, 0.78f, 0.78f);

        // Token: 0x04000886 RID: 2182
        [SerializeField]
        public Color normalColor = new Color(0.243f, 0.263f, 0.341f);

        // Token: 0x04000887 RID: 2183
        [SerializeField]
        public Color hoverColor = new Color(0.345f, 0.373f, 0.702f);
    }
}
