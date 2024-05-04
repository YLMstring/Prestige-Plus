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
            bool unarmed = false;
            if (evt.AttackWithWeapon?.Weapon?.Blueprint.Category == Kingmaker.Enums.WeaponCategory.UnarmedStrike)
            {
                unarmed = true;
            }
            else if (caster.HasFact(Feral) && evt.AttackWithWeapon?.Weapon?.Blueprint.IsNatural == true)
            {
                unarmed = true;
            }
            if (unarmed && evt.Initiator == caster)
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
        //FeralCombatTrainingFeature	edb2546d1215491ca404f8dd1a0c2af3	Kingmaker.Blueprints.Classes.BlueprintFeature
        private static BlueprintFeatureReference Feral = BlueprintTool.GetRef<BlueprintFeatureReference>("edb2546d1215491ca404f8dd1a0c2af3");
    }
}
