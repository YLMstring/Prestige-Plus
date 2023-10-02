using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.RuleSystem.Rules.RuleCalculateAttacksCount;
using static Kingmaker.Visual.Animation.IKController;

namespace PrestigePlus.Modify
{
    internal class ImbueAdvanced : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCastSpell>
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");

        UnitEntityData IInitiatorRulebookSubscriber.GetSubscribingUnit()
        {
            throw new NotImplementedException();
        }

        void IRulebookHandler<RuleCastSpell>.OnEventAboutToTrigger(RuleCastSpell evt)
        {
            try
            {
                if (evt.Spell == null)
                {
                    Logger.Info("No abilitydata");
                    return;
                }
                if (evt.Spell.Blueprint == null)
                {
                    Logger.Info("no blueprint");
                    return;
                }
                if (evt.Spell.Blueprint.Type != AbilityType.Spell)
                {
                    Logger.Info("not spell");
                    return;
                }
                if (!evt.Spell.IsAOE)
                {
                    Logger.Info("not aoe");
                    return;
                }
                if (evt.SpellTarget == null)
                {
                    Logger.Info("No target");
                    return;
                }
                if (!evt.SpellTarget.IsUnit)
                {
                    Logger.Info("Not unit");
                    return;
                }
                if (evt.SpellTarget.Unit == null)
                {
                    Logger.Info("unit is null");
                    return;
                }
                UnitEntityData maybeCaster = base.Context.MaybeCaster;
                if (maybeCaster == null)
                {
                    Logger.Info("Caster is missing");
                    return;
                }
                WeaponSlot threatHandRanged = maybeCaster.GetThreatHandRanged();
                if (threatHandRanged == null)
                {
                    Logger.Info("Caster can't make range attack");
                    return;
                }
                var attack = this.RunAttackRule(maybeCaster, evt.SpellTarget.Unit, threatHandRanged);
                if (attack is null)
                {
                    Logger.Info("No attack triggered");
                    evt.SpellFailureChance = 100;
                    return;
                }
                if (!attack.AttackRoll.IsHit)
                {
                    Logger.Info("attack missed");
                    evt.SpellFailureChance = 100;
                    return;
                }
            }
            catch (Exception e) { Logger.Error("Failed to imbue", e);  }
            //Logger.Info("attack hit");
            return;
        }

        private RuleAttackWithWeapon RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit, WeaponSlot threatHandRanged)
        {
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(maybeCaster, unit, threatHandRanged.Weapon, 0)
            {
                Reason = base.Context,
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = false,
                AttackNumber = 0,
                AttacksCount = 1
            };
            return base.Context.TriggerRule<RuleAttackWithWeapon>(ruleAttackWithWeapon);
        }

        void IRulebookHandler<RuleCastSpell>.OnEventDidTrigger(RuleCastSpell evt)
        {
            
        }
    }
}
