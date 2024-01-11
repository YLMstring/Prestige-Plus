using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    [TypeId("{6A9DFE50-27EE-473A-A0AB-7533DDB6CDAB}")]
    internal class EnergyDrainLife : UnitBuffComponentDelegate<EnergyDrainLife.ComponentData>, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, ITargetRulebookSubscriber, IUnitBuffHandler, IGlobalSubscriber
    {
        void IRulebookHandler<RuleDealDamage>.OnEventAboutToTrigger(RuleDealDamage evt)
        {
            
        }

        void IRulebookHandler<RuleDealDamage>.OnEventDidTrigger(RuleDealDamage evt)
        {
            if (base.Data.Modifier?.AppliedTo == null || Data.Modifier.ModValue < 1)
            {
                base.Buff.Remove();
            }
        }

        public override void OnActivate()
        {
            base.Data.Modifier = base.Owner.Stats.TemporaryHitPoints.AddModifier(value, base.Runtime);
        }

        // Token: 0x0600EBA9 RID: 60329 RVA: 0x003C32D4 File Offset: 0x003C14D4
        public override void OnDeactivate()
        {
            ModifiableValue.Modifier modifier = base.Data.Modifier;
            if (modifier != null)
            {
                modifier.Remove();
            }
            base.Data.Modifier = null;
        }

        void IUnitBuffHandler.HandleBuffDidAdded(Buff buff)
        {
            if (buff.Owner != base.Owner || buff.Blueprint == Context.AssociatedBlueprint) return;
            int point;
            if (buff.Blueprint == Buff1.Get())
            {
                point = 5;
            }
            else if (buff.Blueprint == Buff2.Get())
            {
                point = 10;
            }
            else if (buff.Blueprint == Buff3.Get())
            {
                point = 15;
            }
            else if (buff.Blueprint == Buff4.Get())
            {
                point = 20;
            }
            else { return; }
            if (point > Data.Modifier.ModValue)
            {
                Buff.Remove();
            }
            else
            {
                Buff.SetDuration(new TimeSpan(1, 0, 0));
                buff.Remove();
            }
        }

        void IUnitBuffHandler.HandleBuffDidRemoved(Buff buff)
        {
            
        }

        public int value;
        public class ComponentData
        {
            public ModifiableValue.Modifier Modifier;
        }

        private static BlueprintBuffReference Buff1 = BlueprintTool.GetRef<BlueprintBuffReference>(Souldrinker.EnergyDrainBuffGuid);
        private static BlueprintBuffReference Buff2 = BlueprintTool.GetRef<BlueprintBuffReference>(Souldrinker.EnergyDrainBuff2Guid);
        private static BlueprintBuffReference Buff3 = BlueprintTool.GetRef<BlueprintBuffReference>(Souldrinker.EnergyDrainBuff3Guid);
        private static BlueprintBuffReference Buff4 = BlueprintTool.GetRef<BlueprintBuffReference>(Souldrinker.EnergyDrainBuff4Guid);
    }
}
