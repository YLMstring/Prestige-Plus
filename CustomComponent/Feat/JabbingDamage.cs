using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Components;
using Owlcat.Runtime.Core.Utils;
using PrestigePlus.Blueprint.CombatStyle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class JabbingDamage : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleAttackWithWeaponResolve>, IRulebookHandler<RuleAttackWithWeaponResolve>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeaponResolve>.OnEventAboutToTrigger(RuleAttackWithWeaponResolve evt)
        {
            var caster = Buff.Context?.MaybeCaster;
            if (evt.AttackWithWeapon?.Weapon?.Blueprint.Category == Kingmaker.Enums.WeaponCategory.UnarmedStrike && evt.Initiator == caster)
            {
                int num = 1;
                if (caster.HasFact(Master))
                {
                    num = 2;
                    if (Buff.Rank >=2)
                    {
                        num = 4;
                    }
                }
                PhysicalDamage Damage = new(new ModifiableDiceFormula(new DiceFormula(num, DiceType.D6)), 0, PhysicalDamageForm.Bludgeoning);
                evt.Damage.Add(Damage);
            }
        }

        void IRulebookHandler<RuleAttackWithWeaponResolve>.OnEventDidTrigger(RuleAttackWithWeaponResolve evt)
        {
            
        }
        private static BlueprintFeatureReference Master = BlueprintTool.GetRef<BlueprintFeatureReference>(JabbingStyle.MasterGuid);
    }
}
