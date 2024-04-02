using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using Kingmaker;
using PrestigePlus.CustomComponent.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;
using Kingmaker.UnitLogic;
using Kingmaker.Enums.Damage;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class ReapingStrikeComp : UnitFactComponentDelegate<VexGiantComp.ComponentData>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>
    {
        void IRulebookHandler<RuleCalculateDamage>.OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (Data.LastUseTime + 1.Rounds().Seconds > Game.Instance.TimeController.GameTime)
            {
                return;
            }
            DamageEnergyType type = DamageEnergyType.NegativeEnergy;
            if (evt.Target.HasFact(FeatureRefs.NegativeEnergyAffinity.Reference))
            {
                type = DamageEnergyType.PositiveEnergy;
            }
            evt.ParentRule.Add(new EnergyDamage(new DiceFormula(Fact.GetRank(), DiceType.D6), 0, type));
            Data.LastUseTime = Game.Instance.TimeController.GameTime;
        }

        void IRulebookHandler<RuleCalculateDamage>.OnEventDidTrigger(RuleCalculateDamage evt)
        {

        }
        public class ComponentData
        {
            public TimeSpan LastUseTime;
        }
    }
}
