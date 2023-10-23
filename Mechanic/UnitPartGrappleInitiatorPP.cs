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
using BlueprintCore.Utils;
using Kingmaker.Blueprints;

namespace PrestigePlus.Grapple
{
    internal class UnitPartGrappleInitiatorPP : UnitPartGrappleBase
    {
        [JsonProperty]
        public UnitReference Target { get; private set; }

        // Token: 0x0600BBD3 RID: 48083 RVA: 0x00311110 File Offset: 0x0030F310
        public void Init(UnitReference target, BlueprintBuff buff, MechanicsContext parentContext)
        {
            if (this.m_Initialized)
            {
                PFLog.Default.Error("Already initialized!", Array.Empty<object>());
                return;
            }
            this.m_Initialized = true;
            base.InitializeStartPosition();
            this.Target = target;
            if (buff)
            {
                this.m_Buff = base.Owner.AddBuff(buff, parentContext, null);
            }
            base.Owner.State.Features.RotationForbidden.Retain();
            base.Owner.State.AddCondition(UnitCondition.MovementBan, null, null);
            base.Owner.State.AddCondition(UnitCondition.DisableAttacksOfOpportunity, null, null);
        }

        // Token: 0x0600BBD4 RID: 48084 RVA: 0x003111C8 File Offset: 0x0030F3C8
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
            TryClearPinning();
        }

        public bool IsPinning { get; set; }
        public void TrySetPinning()
        {
            if (this.IsPinning)
            {
                return;
            }
            this.IsPinning = true;
            if (Owner.HasFact(GrabbingStyle))
            {
                return;
            }
            base.Owner.State.AddCondition(UnitCondition.LoseDexterityToAC, null, null);
        }

        // Token: 0x0600BBE1 RID: 48097 RVA: 0x003113DC File Offset: 0x0030F5DC
        public void TryClearPinning()
        {
            if (!this.IsPinning)
            {
                return;
            }
            this.IsPinning = false;
            base.Owner.State.RemoveCondition(UnitCondition.LoseDexterityToAC, null);
        }

        // Token: 0x04007F41 RID: 32577
        [JsonProperty]
        private Buff m_Buff;
        private static BlueprintBuffReference GrabbingStyle = BlueprintTool.GetRef<BlueprintBuffReference>("{133A887D-5353-465D-B06B-9FC40BB29040}");

        // Token: 0x04007F42 RID: 32578
        [JsonProperty]
        private bool m_Initialized;
    }
}

