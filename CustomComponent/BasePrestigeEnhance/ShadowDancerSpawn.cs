using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.BasePrestigeEnhance
{
    internal class ShadowDancerSpawn : UnitBuffComponentDelegate, ISubscriber
    {
        public override void OnActivate()
        {
            var caster = Buff.Context.MaybeCaster;
            if (caster != null)
            {
                int hit = caster.Descriptor.Stats.HitPoints / 2 - Owner.Descriptor.Stats.HitPoints;
                int bab = caster.Descriptor.Stats.BaseAttackBonus.BaseValue - Owner.Descriptor.Stats.BaseAttackBonus.BaseValue;
                int fort = caster.Descriptor.Stats.SaveFortitude.BaseValue - Owner.Descriptor.Stats.SaveFortitude.BaseValue;
                int refl = caster.Descriptor.Stats.SaveReflex.BaseValue - Owner.Descriptor.Stats.SaveReflex.BaseValue;
                int will = caster.Descriptor.Stats.SaveWill.BaseValue - Owner.Descriptor.Stats.SaveWill.BaseValue;
                Owner.Stats.HitPoints.AddModifier(hit, Runtime, ModifierDescriptor.UntypedStackable);
                Owner.Stats.BaseAttackBonus.AddModifier(bab, Runtime, ModifierDescriptor.UntypedStackable);
                Owner.Stats.SaveFortitude.AddModifier(fort, Runtime, ModifierDescriptor.UntypedStackable);
                Owner.Stats.SaveReflex.AddModifier(refl, Runtime, ModifierDescriptor.UntypedStackable);
                Owner.Stats.SaveWill.AddModifier(will, Runtime, ModifierDescriptor.UntypedStackable);
            }
        }

        // Token: 0x0600E570 RID: 58736 RVA: 0x003AB2CE File Offset: 0x003A94CE
        public override void OnDeactivate()
        {
            Owner.Stats.HitPoints.RemoveModifiersFrom(Runtime);
            Owner.Stats.BaseAttackBonus.RemoveModifiersFrom(Runtime);
            Owner.Stats.SaveFortitude.RemoveModifiersFrom(Runtime);
            Owner.Stats.SaveReflex.RemoveModifiersFrom(Runtime);
            Owner.Stats.SaveWill.RemoveModifiersFrom(Runtime);
        }
    }
}
