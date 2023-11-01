using BlueprintCore.Utils;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Designers;
using Kingmaker.Enums;

namespace PrestigePlus.CustomAction.OtherManeuver
{
    internal class ContextActionCombatTrickery : ContextAction
    {
        public override string GetCaption()
        {
            return "CombatTrickery";
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        // Token: 0x0600CBFF RID: 52223 RVA: 0x0034ECD0 File Offset: 0x0034CED0
        public override void RunAction()
        {
            try
            {
                UnitEntityData unit = Target.Unit;
                if (unit == null)
                {
                    PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                    return;
                }
                UnitEntityData maybeCaster = Context.MaybeCaster;
                if (maybeCaster == null)
                {
                    PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                    return;
                }
                if (unit == maybeCaster)
                {
                    PFLog.Default.Error("Unit can't sunder themselves", Array.Empty<object>());
                    return;
                }
                ItemEntityWeapon weapon;
                if (UseWeapon)
                {
                    weapon = maybeCaster.GetThreatHand()?.Weapon;
                    if (weapon == null) { weapon = maybeCaster.Body.EmptyHandWeapon; }
                }
                else
                {
                    weapon = maybeCaster.Body.EmptyHandWeapon;
                }
                var AttackBonusRule = new RuleCalculateAttackBonus(maybeCaster, unit, weapon, 0) { };
                ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule);
                RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(maybeCaster, unit, CombatManeuver.BullRush, AttackBonusRule);
                int bluff = GameHelper.TriggerSkillCheck(new RuleSkillCheck(maybeCaster, Kingmaker.EntitySystem.Stats.StatType.CheckBluff, 0)
                {
                    IgnoreDifficultyBonusToDC = maybeCaster.IsPlayersEnemy
                }, maybeCaster.Context, true).RollResult;
                ruleCombatManeuver.OverrideBonus = new int?(bluff);
                Rulebook.Trigger(ruleCombatManeuver);
            }
            catch (Exception ex) { Logger.Error("Failed to storm.", ex); }
        }

        public bool UseWeapon = false;
        public static void TriggerMRule(ref RuleCalculateAttackBonus AttackBonusRule)
        {
            Rulebook.Trigger(AttackBonusRule);
            var rule = AttackBonusRule.m_InnerRule; 
            if (rule != null) 
            {
                int bonus = 0;
                foreach (var mod in rule.m_ModifiableBonus.Modifiers)
                {
                    if (mod.Type == BonusType.SecondaryWeapon)
                    {
                        bonus -= mod.Value; 
                        break;
                    }
                }
                AttackBonusRule.AddModifier(bonus, BonusType.SecondaryWeapon, ModifierDescriptor.UntypedStackable);
                AttackBonusRule.Result += bonus;
            }    
        }
    }
}
