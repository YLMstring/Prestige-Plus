using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class UnholyFortitudeLogic : UnitFactComponentDelegate, IOwnerGainLevelHandler, IUnitSubscriber, ISubscriber
    {
        public override void OnTurnOn()
        {
            Apply();
        }

        // Token: 0x0600E570 RID: 58736 RVA: 0x003AB2CE File Offset: 0x003A94CE
        public override void OnTurnOff()
        {
            Owner.Stats.HitPoints.RemoveModifiersFrom(Runtime);
        }

        // Token: 0x0600E571 RID: 58737 RVA: 0x003AB2EB File Offset: 0x003A94EB
        public void HandleUnitGainLevel()
        {
            Apply();
        }

        // Token: 0x0600E572 RID: 58738 RVA: 0x003AB2F4 File Offset: 0x003A94F4
        private void Apply()
        {
            Owner.Stats.HitPoints.RemoveModifiersFrom(Runtime);
            if (Owner.HasFact(Undead)) return;
            int num = Owner.Progression.CharacterLevel;
            int con = Owner.Descriptor.Stats.Constitution.BaseValue;
            int cha = Owner.Descriptor.Stats.Charisma.BaseValue;
            int value = Math.Max(num * (cha - con) / 2, 0);
            Owner.Stats.HitPoints.AddModifier(value, Runtime, ModifierDescriptor.UntypedStackable);
        }

        private static BlueprintFeatureReference Undead = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.UndeadImmunities.ToString());
    }
}
