using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker;
using Kingmaker.UnitLogic.Parts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Enums;

namespace PrestigePlus.Grapple
{
    internal class UnitPartGrappleTargetPP : UnitPartGrappleBase
    {
        [JsonProperty]
        public UnitReference Initiator { get; private set; }

        // Token: 0x17001CAA RID: 7338
        // (get) Token: 0x0600BBD8 RID: 48088 RVA: 0x00311258 File Offset: 0x0030F458
        // (set) Token: 0x0600BBD9 RID: 48089 RVA: 0x00311260 File Offset: 0x0030F460
        [JsonProperty]
        public MechanicsContext Context { get; private set; }

        // Token: 0x17001CAB RID: 7339
        // (get) Token: 0x0600BBDA RID: 48090 RVA: 0x00311269 File Offset: 0x0030F469
        // (set) Token: 0x0600BBDB RID: 48091 RVA: 0x00311271 File Offset: 0x0030F471
        [JsonProperty]
        public TimeSpan LastBreakFreeAttempt { get; set; }

        // Token: 0x17001CAC RID: 7340
        // (get) Token: 0x0600BBDC RID: 48092 RVA: 0x0031127A File Offset: 0x0030F47A
        // (set) Token: 0x0600BBDD RID: 48093 RVA: 0x00311282 File Offset: 0x0030F482
        [JsonProperty]
        public bool IsPinned { get; private set; }
        public bool IsTiedUp { get; private set; }

        // Token: 0x0600BBDE RID: 48094 RVA: 0x0031128C File Offset: 0x0030F48C
        public void Init(UnitReference initiator, BlueprintBuff buff, MechanicsContext parentContext)
        {
            if (this.m_Initialized)
            {
                PFLog.Default.Error("Already initialized!", Array.Empty<object>());
                return;
            }
            this.m_Initialized = true;
            base.InitializeStartPosition();
            this.Initiator = initiator;
            this.Context = parentContext;
            if (buff)
            {
                this.m_Buff = base.Owner.AddBuff(buff, parentContext, null);
            }
            base.Owner.State.Features.RotationForbidden.Retain();
            base.Owner.State.AddCondition(UnitCondition.MovementBan, null, null);
            base.Owner.State.AddCondition(UnitCondition.DisableAttacksOfOpportunity, null, null);
            //this.LastBreakFreeAttempt = Game.Instance.TimeController.GameTime;
        }

        // Token: 0x0600BBDF RID: 48095 RVA: 0x0031134C File Offset: 0x0030F54C
        public override void OnDispose()
        {
            base.OnDispose();
            Buff buff = this.m_Buff;
            if (buff != null)
            {
                buff.Remove();
            }
            base.Owner.State.Features.RotationForbidden.Release();
            base.Owner.State.RemoveCondition(UnitCondition.MovementBan, null);
            base.Owner.State.RemoveCondition(UnitCondition.DisableAttacksOfOpportunity, null);
            this.TryClearPinned();
            this.TryClearTiedUp();
        }

        // Token: 0x0600BBE0 RID: 48096 RVA: 0x003113B6 File Offset: 0x0030F5B6
        public void TrySetPinned()
        {
            if (this.IsPinned)
            {
                return;
            }
            this.IsPinned = true;
            base.Owner.State.AddCondition(UnitCondition.CantUseStandardActions, null, null);
            base.Owner.State.AddCondition(UnitCondition.LoseDexterityToAC, null, null);
        }

        // Token: 0x0600BBE1 RID: 48097 RVA: 0x003113DC File Offset: 0x0030F5DC
        private void TryClearPinned()
        {
            if (!this.IsPinned)
            {
                return;
            }
            this.IsPinned = false;
            base.Owner.State.RemoveCondition(UnitCondition.CantUseStandardActions, null);
            base.Owner.State.RemoveCondition(UnitCondition.LoseDexterityToAC, null);
        }

        public void TrySetTiedUp()
        {
            if (this.IsTiedUp)
            {
                return;
            }
            this.IsTiedUp = true;
            
            //base.Owner.State.AddCondition(UnitCondition.Paralyzed, null, null);
        }

        // Token: 0x0600BBE1 RID: 48097 RVA: 0x003113DC File Offset: 0x0030F5DC
        private void TryClearTiedUp()
        {
            if (!this.IsTiedUp)
            {
                return;
            }
            this.IsTiedUp = false;
            //base.Owner.State.RemoveCondition(UnitCondition.Paralyzed, null);
        }

        // Token: 0x04007F45 RID: 32581
        [JsonProperty]
        private Buff m_Buff;

        // Token: 0x04007F46 RID: 32582
        [JsonProperty]
        private bool m_Initialized;
    }
}

