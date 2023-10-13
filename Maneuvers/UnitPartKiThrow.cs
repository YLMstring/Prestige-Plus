using BlueprintCore.Utils;
using Kingmaker.Blueprints;
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
using Kingmaker.Utility;
using HarmonyLib;

namespace PrestigePlus.Maneuvers
{
    internal class UnitPartKiThrow : UnitPartGrappleBase
    {
        [JsonProperty]
        public List<UnitReference> Target = new List<UnitReference>();

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
            Target.Add(target);
            if (buff)
            {
                var duration = new Rounds?(1.Rounds());
                TimeSpan? duration2 = (duration != null) ? new TimeSpan?(duration.Value.Seconds) : null;
                this.m_Buff = base.Owner.AddBuff(buff, parentContext, duration2);
            }
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
        }

        // Token: 0x04007F41 RID: 32577
        [JsonProperty]
        private Buff m_Buff;

        // Token: 0x04007F42 RID: 32578
        [JsonProperty]
        private bool m_Initialized;
    }
}
