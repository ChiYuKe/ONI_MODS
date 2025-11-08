using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EternalDecay.Content.Patches;
using UnityEngine;

namespace EternalDecay.Content.Comps
{
    // KAggressiveChore：用于实现复制人的愤怒行为（打墙、破坏物体）
    public class KAggressiveChore : Chore<KAggressiveChore.StatesInstance>
    {
        // 构造函数，传入目标对象和完成回调
        public KAggressiveChore(IStateMachineTarget target, Action<Chore> on_complete = null): base(
                  ChoreTypesPatch.AddNewChorePatch.BreakStuff, // Chore类型：皮痒
                  target,                              // Chore目标对象
                  target.GetComponent<ChoreProvider>(),// 目标的ChoreProvider组件
                  false, on_complete, null, null,
                  PriorityScreen.PriorityClass.compulsory, // 优先级：强制
                  5, false, true, 0, false,
                  ReportManager.ReportType.WorkTime)       // 报告类型
        {
            // 初始化状态机实例
            base.smi = new KAggressiveChore.StatesInstance(this, target.gameObject);
        }

        // 清理行为
        public override void Cleanup()
        {
            base.Cleanup();
        }

        // 每tick对墙壁造成伤害
        public void PunchWallDamage(float dt)
        {
            // 检查墙是否存在且耐久小于100
            if (Grid.Solid[base.smi.sm.wallCellToBreak] && Grid.StrengthInfo[base.smi.sm.wallCellToBreak] < 100)
            {
                // 施加伤害
                WorldDamage.Instance.ApplyDamage(
                    base.smi.sm.wallCellToBreak,
                    0.06f * dt,
                    base.smi.sm.wallCellToBreak,
                    "你复制人爹想手痒想拆点东西",
                    "你是好人，我帮你拆");
            }
        }

        public class StatesInstance : GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.GameInstance
        {
            public StatesInstance(KAggressiveChore master, GameObject breaker): base(master)
            {
                base.sm.breaker.Set(breaker, base.smi, false);
            }

            // 查找可破坏的对象
            public void FindBreakable()
            {
                Navigator navigator = base.GetComponent<Navigator>();
                int minCost = int.MaxValue;
                Breakable breakable = null;

                // 概率随机挑选一个Breakable
                if (UnityEngine.Random.Range(0, 100) >= 2)
                {
                    foreach (Breakable b in Components.Breakables.Items)
                    {
                        if (b != null && !b.IsInvincible && !b.isBroken())
                        {
                            int cost = navigator.GetNavigationCost(b);
                            if (cost != -1 && cost < minCost)
                            {
                                minCost = cost;
                                breakable = b;
                            }
                        }
                    }
                }

                // 如果没找到，寻找最近可触碰墙面的位置
                if (breakable == null)
                {
                    int cell = GameUtil.FloodFillFind<object>(
                        (int c, object arg) => !Grid.Solid[c] && navigator.CanReach(c) &&
                        ((Grid.IsValidCell(Grid.CellLeft(c)) && Grid.Solid[Grid.CellLeft(c)]) ||
                        (Grid.IsValidCell(Grid.CellRight(c)) && Grid.Solid[Grid.CellRight(c)]) ||
                        (Grid.IsValidCell(Grid.OffsetCell(c, 1, 1)) && Grid.Solid[Grid.OffsetCell(c, 1, 1)]) ||
                        (Grid.IsValidCell(Grid.OffsetCell(c, -1, 1)) && Grid.Solid[Grid.OffsetCell(c, -1, 1)])),
                        null,
                        Grid.PosToCell(navigator.gameObject),
                        128,
                        true,
                        true);
                    base.sm.moveToWallTarget.Set(cell, base.smi, false);
                    this.GoTo(base.sm.move_notarget);
                    return;
                }

                // 设置目标Breakable并切换到移动状态
                base.sm.breakable.Set(breakable, base.smi);
                this.GoTo(base.sm.move_target);
            }
        }

        // 状态机定义
        public class States : GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore>
        {
            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = this.findbreakable; // 默认状态为寻找可破坏对象
                base.Target(this.breaker);

                // 根状态动画
                this.root.ToggleAnims("anim_loco_destructive_kanim", 0f);

                // 没有目标状态：停止状态机
                this.noTarget.Enter(smi => smi.StopSM("complete/no more food"));

                // 查找可破坏对象状态
                this.findbreakable.Enter(smi => smi.FindBreakable());

                // 移动到墙状态
                this.move_notarget.MoveTo(smi => smi.sm.moveToWallTarget.Get(smi), this.breaking_wall, this.noTarget, false);

                // 移动到Breakable对象
                this.move_target.InitializeStates(this.breaker, this.breakable, this.breaking, this.findbreakable, null, null)
                    .ToggleStatusItem(Db.Get().DuplicantStatusItems.LashingOut, null);

                // 破坏墙体状态
                this.breaking_wall.DefaultState(this.breaking_wall.Pre)
                    .Enter(smi =>
                    {
                        int pos = Grid.PosToCell(smi.master.gameObject);
                        // 检测墙体方向并设置动画覆盖
                        if (Grid.Solid[Grid.OffsetCell(pos, 1, 0)])
                        {
                            smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_low_kanim"), 0f);
                            this.wallCellToBreak = Grid.OffsetCell(pos, 1, 0);
                        }
                        else if (Grid.Solid[Grid.OffsetCell(pos, -1, 0)])
                        {
                            smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_low_kanim"), 0f);
                            this.wallCellToBreak = Grid.OffsetCell(pos, -1, 0);
                        }
                        else if (Grid.Solid[Grid.OffsetCell(pos, 1, 1)])
                        {
                            smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_high_kanim"), 0f);
                            this.wallCellToBreak = Grid.OffsetCell(pos, 1, 1);
                        }
                        else if (Grid.Solid[Grid.OffsetCell(pos, -1, 1)])
                        {
                            smi.sm.masterTarget.Get<KAnimControllerBase>(smi).AddAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_high_kanim"), 0f);
                            this.wallCellToBreak = Grid.OffsetCell(pos, -1, 1);
                        }
                        smi.master.GetComponent<Facing>().Face(Grid.CellToPos(this.wallCellToBreak));
                    })
                    .Exit(smi =>
                    {
                        // 移除动画覆盖
                        smi.sm.masterTarget.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_high_kanim"));
                        smi.sm.masterTarget.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(Assets.GetAnim("anim_out_of_reach_destructive_low_kanim"));
                    });

                // 破坏墙体子状态：Pre -> Loop -> Post
                this.breaking_wall.Pre.PlayAnim("working_pre").OnAnimQueueComplete(this.breaking_wall.Loop);

                // Loop状态，持续打墙
                this.breaking_wall.Loop
                    .ScheduleGoTo(26f, this.breaking_wall.Pst) // 26秒后进入Post
                    .Update("PunchWallDamage", (smi, dt) => smi.master.PunchWallDamage(dt), UpdateRate.SIM_1000ms, false)
                    .Enter(smi => smi.Play("working_loop", KAnim.PlayMode.Loop))
                    .Update((smi, dt) =>
                    {
                        // 如果墙已不存在，跳到Post
                        if (!Grid.Solid[smi.sm.wallCellToBreak])
                            smi.GoTo(this.breaking_wall.Pst);
                    }, UpdateRate.SIM_200ms, false);

                // Post状态，播放完成动画后回到noTarget
                this.breaking_wall.Pst.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.noTarget);

                // Breaking状态绑定工作对象
                this.breaking.ToggleWork<Breakable>(this.breakable, null, null, null);
            }

            // 状态机参数
            public StateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.TargetParameter breaker;
            public StateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.TargetParameter breakable;
            public StateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.IntParameter moveToWallTarget;
            public int wallCellToBreak;

            // 子状态
            public GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.ApproachSubState<Breakable> move_target;
            public GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.State move_notarget;
            public GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.State findbreakable;
            public GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.State noTarget;
            public GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.State breaking;
            public KAggressiveChore.States.BreakingWall breaking_wall;

            // 破坏墙体子状态类
            public class BreakingWall : GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.State
            {
                public GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.State Pre;
                public GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.State Loop;
                public GameStateMachine<KAggressiveChore.States, KAggressiveChore.StatesInstance, KAggressiveChore, object>.State Pst;
            }
        }
    }
}
