using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Kingmaker.RuleSystem;
using Kingmaker.Designers;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Items.Slots;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker.UnitLogic.Buffs;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class MedusaWrath : ContextAction
    {
        public override string GetCaption()
        {
            return "Medusa Wrath";
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public override void RunAction()
        {
            try
            {
                UnitEntityData unit = Target.Unit;
                //Logger.Info("start");
                if (unit == null)
                {
                    Logger.Info("no unit");
                    return;
                }
                UnitEntityData maybeCaster = Context.MaybeCaster;
                if (maybeCaster == null)
                {
                    Logger.Info("no caster");
                    return;
                }
                if (maybeCaster.GetFact(CasterBuff) is not Buff buff || buff.GetRank() == 10) return;
                var IsTargetFlatFooted = Rulebook.Trigger(new RuleCheckTargetFlatFooted(maybeCaster, unit)).IsFlatFooted;
                if (IsTargetFlatFooted || unit.State.HasCondition(UnitCondition.Dazed) || unit.State.HasCondition(UnitCondition.LoseDexterityToAC) || unit.State.HasCondition(UnitCondition.Paralyzed) || unit.State.HasCondition(UnitCondition.Staggered) || unit.State.HasCondition(UnitCondition.Stunned) || unit.State.HasCondition(UnitCondition.Unconscious))
                {
                    var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
                    maybeCaster.View.AnimationManager.Execute(attackAnimation);
                    buff.Rank = 10;
                    RunAttackRule(maybeCaster, unit);
                    RunAttackRule(maybeCaster, unit);
                }
            }
            catch (Exception ex) { Logger.Error("Failed to medusa.", ex); }
        }

        private static void RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit)
        {
            var weapon = maybeCaster.Body.EmptyHandWeapon;
            if (weapon == null) { return; }
            RuleAttackWithWeapon ruleAttackWithWeapon = new(maybeCaster, unit, weapon, 0)
            {
                Reason = maybeCaster.Context,
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = false,
                AttackNumber = 0,
                AttacksCount = 1
            };
            maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
        }

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{5BFEF430-F580-4A45-965B-7100D745F055}");
    }
}
