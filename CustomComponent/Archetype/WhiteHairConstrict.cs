using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Entities;
using PrestigePlus.Blueprint.GrappleFeat;
using Kingmaker.Items;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.Items.Slots;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class WhiteHairConstrict : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {

        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (evt.Target == null) return;
            if (!evt.Success) return;
            if (evt.Type != CombatManeuver.Grapple) return;
            foreach (WeaponSlot weaponSlot in Owner.Body.AdditionalLimbs)
            {
                if (weaponSlot.Weapon?.Blueprint?.Type == WeaponTypeRefs.GoreType.Reference.Get())
                {
                    RunAttackRule(Owner, evt.Target, weaponSlot.Weapon);
                    break;
                }
            }

        }

        private static void RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit, ItemEntityWeapon weapon)
        {
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(maybeCaster, unit, weapon, 0)
            {
                Reason = maybeCaster.Context,
                AutoHit = true,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = false,
                AttackNumber = 0,
                AttacksCount = 1
            };
            maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
        }
    }
}