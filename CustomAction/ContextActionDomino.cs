using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Maneuvers;
using Kingmaker.Designers;
using Kingmaker.Utility;

namespace PrestigePlus.CustomAction
{
    internal class ContextActionDomino : ContextAction
    {
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public override string GetCaption()
        {
            return "StickFightingManeuver";
        }

        public override void RunAction()
        {
            ActManeuver(Context.MaybeCaster, Target.Unit);
        }

        public static void ActManeuver(UnitEntityData caster, UnitEntityData target)
        {
            if (target == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            var maneuver = CombatManeuver.Trip;
            ItemEntityWeapon weapon = caster.Body.EmptyHandWeapon;
            var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, weapon, 0) { };
            int penalty;
            var rank = caster.GetFact(CasterBuff);
            if (rank == null) 
            {
                penalty = - 4; 
            }
            else
            {
                penalty = - 4 - 4 * rank.GetRank();
            }
            AttackBonusRule.AddModifier(penalty, descriptor: Kingmaker.Enums.ModifierDescriptor.Penalty);
            Rulebook.Trigger(AttackBonusRule);
            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(caster, target, maneuver, AttackBonusRule);
            ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
            int num = caster.Progression.MythicLevel / 2;
            if (ruleCombatManeuver.Success)
            {
                GameHelper.ApplyBuff(caster, CasterBuff2, new Rounds?(1.Rounds()));
                caster.CombatState.PreventAttacksOfOpporunityNextFrame = true;
                caster.Position = target.Position;
            }
            else
            {
                GameHelper.RemoveBuff(caster, CasterBuff2);
                GameHelper.RemoveBuff(caster, CasterBuffCore);
            }
            if (penalty + 4 * num < 0)
            {
                GameHelper.RemoveBuff(caster, CasterBuff2);
                GameHelper.RemoveBuff(caster, CasterBuffCore);
            }
        }

        private static BlueprintUnitFactReference CasterBuff = BlueprintTool.GetRef<BlueprintUnitFactReference>(DownLikeDominoes.StyleBuff2Guid);
        private static BlueprintBuffReference CasterBuff2 = BlueprintTool.GetRef<BlueprintBuffReference>(DownLikeDominoes.StyleBuff2Guid);
        private static BlueprintBuffReference CasterBuffCore = BlueprintTool.GetRef<BlueprintBuffReference>(DownLikeDominoes.StyleBuffGuid);
    }
}