using System;
using UnityEngine;
using Klei.AI;
using static EternalDecay.Content.Patches.ChoreTypesPatch;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;
using EternalDecay.Content.Comps;
using static UnityEngine.GraphicsBuffer;

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
        this.SetWorkTime(10);
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
        targetGameObject.AddOrGet<KPrefabID>().AddTag("Assigned", true);
    }






    [MyCmpReq]
    public Assignable assignable;

    private Chore chore;
    private GameObject targetGameObject;
}
