using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using static STRINGS.BUILDINGS.PREFABS;

namespace CykUtils
{
    // Token: 0x02000301 RID: 769
    public class ScreenUtil : KScreen
    {
        // Token: 0x06000981 RID: 2433 RVA: 0x0002ABD0 File Offset: 0x00028DD0
        protected override void OnPrefabInit()
        {
            this.SetObjects();
            this.activateOnSpawn = true;
            base.gameObject.SetActive(true);
        }

        // Token: 0x06000982 RID: 2434 RVA: 0x0002ABEC File Offset: 0x00028DEC
        public virtual void SetObjects()
        {
            Transform transform = base.transform.Find("SettingsDialogData");
            Text text;
            if (transform != null && transform.gameObject.TryGetComponent<Text>(out text))
            {
                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(text.text);
                this.cancelButton = this.SetButton("cancel", dictionary);
                this.confirmButton = this.SetButton("apply", dictionary);
                this.XButton = this.SetButton("close", dictionary);
                this.SteamButton = this.SetButton("steam", dictionary);
                this.GithubButton = this.SetButton("github", dictionary);
                global::UnityEngine.Object.Destroy(transform.gameObject);
            }
        }

        // Token: 0x06000983 RID: 2435 RVA: 0x0002AC98 File Offset: 0x00028E98
        private ButtonUtil SetButton(string key, Dictionary<string, string> buttonRefs)
        {
            string text;
            if (buttonRefs.TryGetValue(key, out text))
            {
                Transform transform = base.transform.Find(text);
                if (transform != null)
                {
                    return transform.gameObject.AddComponent<ButtonUtil>();
                }
            }
            return null;
        }

        // Token: 0x06000984 RID: 2436 RVA: 0x0002ACD4 File Offset: 0x00028ED4
        public virtual void ShowDialog()
        {
            if (base.transform.parent.GetComponent<Canvas>() == null && base.transform.parent.parent != null)
            {
                base.transform.SetParent(base.transform.parent.parent);
            }
            base.transform.SetAsLastSibling();
            if (this.cancelButton != null)
            {
                this.cancelButton.OnClick += this.OnClickCancel;
            }
            if (this.XButton != null)
            {
                this.XButton.OnClick += this.OnClickCancel;
            }
            if (this.confirmButton != null)
            {
                this.confirmButton.OnClick += this.OnClickApply;
            }
            if (this.GithubButton != null)
            {
                this.GithubButton.OnClick += this.OnClickGithub;
            }
            if (this.SteamButton != null)
            {
                this.SteamButton.OnClick += this.OnClickSteam;
            }
        }

        // Token: 0x06000985 RID: 2437 RVA: 0x0002ADF3 File Offset: 0x00028FF3
        public void OnClickGithub()
        {
            Application.OpenURL("https://github.com/aki-art/ONI-Mods");
        }

        // Token: 0x06000986 RID: 2438 RVA: 0x0002ADFF File Offset: 0x00028FFF
        public void OnClickSteam()
        {
            Application.OpenURL("https://steamcommunity.com/id/akisnothere/myworkshopfiles/?appid=457140");
        }

        // Token: 0x06000987 RID: 2439 RVA: 0x0002AE0B File Offset: 0x0002900B
        public virtual void OnClickCancel()
        {
            this.Reset();
            this.Deactivate();
        }

        // Token: 0x06000988 RID: 2440 RVA: 0x00007404 File Offset: 0x00005604
        public virtual void Reset()
        {
        }

        // Token: 0x06000989 RID: 2441 RVA: 0x00007404 File Offset: 0x00005604
        public virtual void OnClickApply()
        {
        }

        // Token: 0x0600098A RID: 2442 RVA: 0x0002AE19 File Offset: 0x00029019
        protected override void OnCmpEnable()
        {
            base.OnCmpEnable();
            if (CameraController.Instance != null)
            {
                CameraController.Instance.DisableUserCameraControl = true;
            }
        }

        // Token: 0x0600098B RID: 2443 RVA: 0x0002AE39 File Offset: 0x00029039
        protected override void OnCmpDisable()
        {
            base.OnCmpDisable();
            if (CameraController.Instance != null)
            {
                CameraController.Instance.DisableUserCameraControl = false;
            }
            base.Trigger(476357528, null);
        }

        // Token: 0x0600098C RID: 2444 RVA: 0x0001DCFA File Offset: 0x0001BEFA
        public override bool IsModal()
        {
            return true;
        }

        // Token: 0x0600098D RID: 2445 RVA: 0x0002AE65 File Offset: 0x00029065
        public override float GetSortKey()
        {
            return 300f;
        }

        // Token: 0x0600098E RID: 2446 RVA: 0x0002AE6C File Offset: 0x0002906C
        protected override void OnActivate()
        {
            this.OnShow(true);
        }

        // Token: 0x0600098F RID: 2447 RVA: 0x0002AE75 File Offset: 0x00029075
        protected override void OnDeactivate()
        {
            this.OnShow(false);
        }

        // Token: 0x06000990 RID: 2448 RVA: 0x0002AE80 File Offset: 0x00029080
        protected override void OnShow(bool show)
        {
            base.OnShow(show);
            if (this.pause && SpeedControlScreen.Instance != null)
            {
                if (show && !this.shown)
                {
                    SpeedControlScreen.Instance.Pause(false, false);
                }
                else if (!show && this.shown)
                {
                    SpeedControlScreen.Instance.Unpause(false);
                }
                this.shown = show;
            }
        }

        // Token: 0x06000991 RID: 2449 RVA: 0x0002AEDF File Offset: 0x000290DF
        public override void OnKeyDown(KButtonEvent e)
        {
            if (e.TryConsume(global::Action.Escape))
            {
                this.OnClickCancel();
                return;
            }
            base.OnKeyDown(e);
        }

        // Token: 0x06000992 RID: 2450 RVA: 0x0002AEF8 File Offset: 0x000290F8
        public override void OnKeyUp(KButtonEvent e)
        {
            if (!e.Consumed)
            {
                KScrollRect componentInChildren = base.GetComponentInChildren<KScrollRect>();
                if (componentInChildren != null)
                {
                    componentInChildren.OnKeyUp(e);
                }
            }
            e.Consumed = true;
        }

        // Token: 0x04000893 RID: 2195
        public const float SCREEN_SORT_KEY = 300f;

        // Token: 0x04000894 RID: 2196
        private new bool ConsumeMouseScroll = true;

        // Token: 0x04000895 RID: 2197
        private bool shown;

        // Token: 0x04000896 RID: 2198
        public bool pause = true;

        // Token: 0x04000897 RID: 2199
        public ButtonUtil cancelButton;

        // Token: 0x04000898 RID: 2200
        public ButtonUtil confirmButton;

        // Token: 0x04000899 RID: 2201
        public ButtonUtil XButton;

        // Token: 0x0400089A RID: 2202
        public ButtonUtil SteamButton;

        // Token: 0x0400089B RID: 2203
        public ButtonUtil GithubButton;
    }
}
