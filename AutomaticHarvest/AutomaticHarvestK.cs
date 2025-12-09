using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSerialization;
using UnityEngine;

namespace AutomaticHarvest
{
    public class AutomaticHarvestK : KMonoBehaviour, IUserControlledCapacity
    {
        // Token: 0x0600373F RID: 14143 RVA: 0x0013721B File Offset: 0x0013541B
        protected override void OnPrefabInit()
        {
            this.Initialize(false);
        }

        // Token: 0x06003740 RID: 14144 RVA: 0x00137224 File Offset: 0x00135424
        protected void Initialize(bool use_logic_meter)
        {
            base.OnPrefabInit();
            this.log = new LoggerFS("AutomaticHarvestK", 35);
            ChoreType choreType = Db.Get().ChoreTypes.Get(this.choreTypeID);
            this.filteredStorage = new FilteredStorage(this, null, (global::IUserControlledCapacity)this, use_logic_meter, choreType);
            base.Subscribe<AutomaticHarvestK>(-905833192, AutomaticHarvestK.OnCopySettingsDelegate);
        }

        // Token: 0x06003741 RID: 14145 RVA: 0x00137280 File Offset: 0x00135480
        protected override void OnSpawn()
        {
            this.filteredStorage.FilterChanged();
            if (this.nameable != null && !this.lockerName.IsNullOrWhiteSpace())
            {
                this.nameable.SetName(this.lockerName);
            }
            base.Trigger(-1683615038, null);
        }

        // Token: 0x06003742 RID: 14146 RVA: 0x001372D0 File Offset: 0x001354D0
        protected override void OnCleanUp()
        {
            this.filteredStorage.CleanUp();
        }

        // Token: 0x06003743 RID: 14147 RVA: 0x001372E0 File Offset: 0x001354E0
        private void OnCopySettings(object data)
        {
            GameObject gameObject = (GameObject)data;
            if (gameObject == null)
            {
                return;
            }
            AutomaticHarvestK component = gameObject.GetComponent<AutomaticHarvestK>();
            if (component == null)
            {
                return;
            }
            this.UserMaxCapacity = component.UserMaxCapacity;
        }

        // Token: 0x06003744 RID: 14148 RVA: 0x0013731B File Offset: 0x0013551B
        public void UpdateForbiddenTag(Tag game_tag, bool forbidden)
        {
            if (forbidden)
            {
                this.filteredStorage.RemoveForbiddenTag(game_tag);
                return;
            }
            this.filteredStorage.AddForbiddenTag(game_tag);
        }

        public bool ControlEnabled()
        {
            throw new NotImplementedException();
        }

        // Token: 0x17000399 RID: 921
        // (get) Token: 0x06003745 RID: 14149 RVA: 0x00137339 File Offset: 0x00135539
        // (set) Token: 0x06003746 RID: 14150 RVA: 0x00137351 File Offset: 0x00135551
        public virtual float UserMaxCapacity
        {
            get
            {
                return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
            }
            set
            {
                this.userMaxCapacity = value;
                this.filteredStorage.FilterChanged();
            }
        }

        // Token: 0x1700039A RID: 922
        // (get) Token: 0x06003747 RID: 14151 RVA: 0x00137365 File Offset: 0x00135565
        public float AmountStored
        {
            get
            {
                return base.GetComponent<Storage>().MassStored();
            }
        }

        // Token: 0x1700039B RID: 923
        // (get) Token: 0x06003748 RID: 14152 RVA: 0x00137372 File Offset: 0x00135572
        public float MinCapacity
        {
            get
            {
                return 0f;
            }
        }

        // Token: 0x1700039C RID: 924
        // (get) Token: 0x06003749 RID: 14153 RVA: 0x00137379 File Offset: 0x00135579
        public float MaxCapacity
        {
            get
            {
                return base.GetComponent<Storage>().capacityKg;
            }
        }

        // Token: 0x1700039D RID: 925
        // (get) Token: 0x0600374A RID: 14154 RVA: 0x00137386 File Offset: 0x00135586
        public bool WholeValues
        {
            get
            {
                return false;
            }
        }

        // Token: 0x1700039E RID: 926
        // (get) Token: 0x0600374B RID: 14155 RVA: 0x00137389 File Offset: 0x00135589
        public LocString CapacityUnits
        {
            get
            {
                return GameUtil.GetCurrentMassUnit(false);
            }
        }

        // Token: 0x040021AF RID: 8623
        private LoggerFS log;

        // Token: 0x040021B0 RID: 8624
        [Serialize]
        private float userMaxCapacity = float.PositiveInfinity;

        // Token: 0x040021B1 RID: 8625
        [Serialize]
        public string lockerName = "";

        // Token: 0x040021B2 RID: 8626
        protected FilteredStorage filteredStorage;

        // Token: 0x040021B3 RID: 8627
        [MyCmpGet]
        private UserNameable nameable;

        // Token: 0x040021B4 RID: 8628
        public string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

        // Token: 0x040021B5 RID: 8629
        private static readonly EventSystem.IntraObjectHandler<AutomaticHarvestK> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AutomaticHarvestK>(delegate (AutomaticHarvestK component, object data)
        {
            component.OnCopySettings(data);
        });
    }
}
