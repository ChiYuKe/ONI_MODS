using System;
using UnityEngine;
using Klei.AI;
using static EternalDecay.Content.Patches.ChoreTypesPatch;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;
using EternalDecay.Content.Comps;
using static UnityEngine.GraphicsBuffer;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;
using CykUtils;
using EternalDecay.Content.Comps.DebuffCom;
using System.IO;
using STRINGS;
using EternalDecay.Content.Comps.KUI;
using static Satsuma.CompleteBipartiteGraph;
using Color = UnityEngine.Color;
using EternalDecay.Content.Configs;


public class Accepttheinheritance : Workable
{






    protected override void OnPrefabInit()
    {
        base.OnPrefabInit();
        this.synchronizeAnims = false;
        this.showProgressBar = true;
        this.resetProgressOnStop = true;      
        this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
        this.assignable.OnAssign += this.Assign;

        this.overrideAnims = new KAnimFile[] { Assets.GetAnim("anim_interacts_hqbase_skill_upgrade_kanim") };
        this.workAnims = new HashedString[] { "upgrade" };
        this.workingPstComplete = null;
        this.workingPstFailed = null;

    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        this.SetWorkTime(8);
    }



    private void Assign(IAssignableIdentity new_assignee)
    {
        this.CancelChore();
        if (new_assignee != null)
        {
            this.ActivateChore();
        }

        if (!CanProcessInheritance(new_assignee, out GameObject source)) return;
        TryGetActualTargetObject(source, out GameObject target);

    }






    private bool CanProcessInheritance(IAssignableIdentity currentAssignee, out GameObject sourceObject)
    {
        sourceObject = null;  // 初始化输出参数为空

        if (currentAssignee == null) // 如果当前分配对象为空
        {
            return false; // 不能处理
        }

        var soleOwner = currentAssignee.GetSoleOwner(); // 获取分配对象的唯一所有者（SoleOwner）
        if (soleOwner == null) // 如果没有所有者
        {
            return false; // 不能处理
        }

        sourceObject = soleOwner.gameObject; // 将所有者的 GameObject 赋给输出参数
        return sourceObject != null; // 如果 GameObject 不为空，则可以处理，返回 true
    }

    private bool TryGetActualTargetObject(GameObject sourceObject, out GameObject targetObject)
    {
        targetObject = sourceObject;

        var proxy = sourceObject.GetComponent<MinionAssignablesProxy>();
        if (proxy != null)
        {
            targetObject = proxy.GetTargetGameObject();
            this.targetGameObject = targetObject;
        }

        return targetObject != null;
    }








    private void ActivateChore()
    {
        Debug.Assert(this.chore == null);
        this.chore = new WorkChore<Accepttheinheritance>(AddNewChorePatch.Accepttheinheritance, this, null, true, delegate (Chore o)
        {
            this.CompleteChore();
        }, null, null, true, null, false, false, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.compulsory, 5, false, true);
    }


    private void CancelChore()
    {
        if (this.chore == null)
        {
            return;
        }
        this.chore.Cancel("User cancelled");
        this.chore = null;
    }


    private void CompleteChore()
    {
        this.chore.Cleanup();
        this.chore = null;

        GameObject.Destroy(this.gameObject);

        var popfx = PopFXManager.Instance.SpawnFX(
            Assets.GetSprite("icon_action_upgrade"),       // 图标
            "继承成功", // 文本
            targetGameObject.transform,                          // 复制人位置
            1.5f,                                        // 显示时长
            false                                        // 是否跟随物体移动
          );
        targetGameObject.AddOrGet<KPrefabID>().AddTag("Assigned", true);// 有这个tag的不会显示在那个罐中脑列表里面
    }



    protected override void OnAbortWork(WorkerBase worker)
    {
        base.OnAbortWork(worker);
        LogUtil.Log("工作中止");
    }
    protected override void OnStartWork(WorkerBase worker)
    {
        base.OnStartWork(worker);
        LogUtil.Log("开始工作");
       
    }

    protected override void OnCompleteWork(WorkerBase worker)
    {
        base.OnCompleteWork(worker);
        GameObject minion = worker.gameObject;

        if (minion != null && minion.GetComponent<ColorfulPulsatingLight2D>() == null)
        {
            var light = minion.AddComponent<ColorfulPulsatingLight2D>();
            light.PulseSpeed = 1.5f;
            light.MinIntensity = 800f;
            light.MaxIntensity = 2000f;
            light.MinRadius = 4f;
            light.MaxRadius = 8f;
            light.Colors = new Color[] { Color.red, Color.yellow, Color.green };
            LogUtil.Log($"{minion.name} 已添加 ColorfulPulsatingLight2D 组件");

           


        }
        minion.AddOrGet<AbyssophobiaDebuff>();

        ShowInheritanceInfo(minion);


    }


    public void ShowInheritanceInfo(GameObject minion)
    {
        var oldAttributes = minion.GetComponent<AttributeLevels>();
        var newAttributes = this.gameObject.GetComponent<AttributeLevels>();

        if (oldAttributes == null)
        {
            Debug.LogWarning("旧角色未找到 AttributeLevels 组件。");
            return;
        }

        HashSet<string> filteredAttributes = new HashSet<string>
        {
            "Toggle","LifeSupport","Immunity","FarmTinker","PowerTinker"
        };

      
        List<(string attrName, int oldLevel, int newLevel)> attrList = new();

        foreach (var oldAttrLevel in oldAttributes)
        {
            var attribute = oldAttrLevel.attribute.Attribute;
            if (filteredAttributes.Contains(attribute.Id))
                continue;

            string attrName = attribute.Name;
            int oldLevel = oldAttrLevel.GetLevel();

            var newAttrLevel = newAttributes.GetAttributeLevel(attribute.Id);
            int newLevel = oldLevel;
            if (newAttrLevel != null)
            {
                newLevel += newAttrLevel.GetLevel();
                if (newLevel > TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.ATTRIBUTEMAXLEVEL)
                    newLevel = TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.ATTRIBUTEMAXLEVEL;
            }

            attrList.Add((attrName, oldLevel, newLevel));
        }

        // 创建并显示面板
        var screenGO = new GameObject("InheritanceInfo");
        var rectTransform = screenGO.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300f, 500f);

        KScreen kscreen = screenGO.AddComponent<InheritanceInformation>();
        GameObject panel = InheritanceInformation.Createpanel(
            "继承详情",
            attrList, 
            "关闭窗口"
        );
        panel.transform.SetParent(kscreen.transform, false);

        screenGO.transform.SetParent(GameObject.Find("ScreenSpaceOverlayCanvas").transform, false);
        kscreen.Activate();
    }







    [MyCmpReq]
    public Assignable assignable;

    private Chore chore;
    private GameObject targetGameObject;
}
