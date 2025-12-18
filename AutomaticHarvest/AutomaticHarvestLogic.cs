using KSerialization;
using STRINGS;
using UnityEngine;



public class AutomaticHarvestLogic : KMonoBehaviour, IActivationRangeTarget, ISim200ms
{
    [MyCmpGet]
    private Storage storage;

    [MyCmpGet]
    private Operational operational;

    [Serialize]
    private int activateValue;

    [Serialize]
    private int deactivateValue = 100;

    [Serialize]
    public bool activated;

    [MyCmpGet]
    private LogicPorts logicPorts;

    // 获取固体分配器组件
    [MyCmpGet]
    private SolidConduitDispenser dispenser;

    [MyCmpAdd]
    private CopyBuildingSettings copyBuildingSettings;

    private MeterController logicMeter;

    public static readonly HashedString PORT_ID = "AutomaticHarvestLogicLogicPort";

    private static readonly EventSystem.IntraObjectHandler<AutomaticHarvestLogic> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AutomaticHarvestLogic>(delegate (AutomaticHarvestLogic component, object data)
    {
        component.OnCopySettings(data);
    });

    private static readonly EventSystem.IntraObjectHandler<AutomaticHarvestLogic> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<AutomaticHarvestLogic>(delegate (AutomaticHarvestLogic component, object data)
    {
        component.OnLogicValueChanged(data);
    });

    private static readonly EventSystem.IntraObjectHandler<AutomaticHarvestLogic> UpdateLogicCircuitDelegate = new EventSystem.IntraObjectHandler<AutomaticHarvestLogic>(delegate (AutomaticHarvestLogic component, object data)
    {
        component.UpdateLogicCircuit(data);
    });

    public float PercentFull => storage.MassStored() / storage.Capacity();

    public float ActivateValue
    {
        get
        {
            return deactivateValue;
        }
        set
        {
            deactivateValue = (int)value;
            UpdateLogicCircuit(null);
        }
    }

    public float DeactivateValue
    {
        get
        {
            return activateValue;
        }
        set
        {
            activateValue = (int)value;
            UpdateLogicCircuit(null);
        }
    }

    public float MinValue => 0f;

    public float MaxValue => 100f;

    public bool UseWholeNumbers => true;

    public string ActivateTooltip => AutomaticHarvest.STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.ACTIVATE_TOOLTIP;

    public string DeactivateTooltip => AutomaticHarvest.STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.DEACTIVATE_TOOLTIP;

    public string ActivationRangeTitleText => AutomaticHarvest.STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.SIDESCREEN_TITLE;

    public string ActivateSliderLabelText => AutomaticHarvest.STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.SIDESCREEN_ACTIVATE;

    public string DeactivateSliderLabelText => AutomaticHarvest.STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.SIDESCREEN_DEACTIVATE;

    protected override void OnSpawn()
    {
        base.OnSpawn();
        Subscribe(-801688580, OnLogicValueChangedDelegate);
        Subscribe(-592767678, UpdateLogicCircuitDelegate);
    }

    protected override void OnPrefabInit()
    {
        base.OnPrefabInit();
        Subscribe(-905833192, OnCopySettingsDelegate);
    }

    public void Sim200ms(float dt)
    {
        UpdateLogicCircuit(null);
    }

    private void UpdateLogicCircuit(object data)
    {
        float num = PercentFull * 100f;

        // 当前状态是激活（绿色信号）
        if (activated)
        {
            // 只有降到低阈值 (activateValue) 时，才去激活 = false (红色信号)
            if (num <= (float)activateValue) // 注意：这里使用了 activateValue
            {
                activated = false;
            }
        }
        // 当前状态是非激活（红色信号）
        else if (num >= (float)deactivateValue) // 注意：这里使用了 deactivateValue
        {
            // 只有升到高阈值 (deactivateValue) 时，才去激活 = true (绿色信号)
            activated = true;
        }



        logicPorts.SendSignal(PORT_ID, activated ? 1 : 0);
    }

    private void OnLogicValueChanged(object data)
    {
        LogicValueChanged logicValueChanged = (LogicValueChanged)data;
        if (logicValueChanged.portID == PORT_ID)
        {
            SetLogicMeter(LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue));
        }
    }

    private void OnCopySettings(object data)
    {
        AutomaticHarvestLogic component = ((GameObject)data).GetComponent<AutomaticHarvestLogic>();
        if (component != null)
        {
            ActivateValue = component.ActivateValue;
            DeactivateValue = component.DeactivateValue;
        }
    }

    public void SetLogicMeter(bool on)
    {
        if (logicMeter != null)
        {
            logicMeter.SetPositionPercent(on ? 1f : 0f);
        }
    }
}