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

namespace PrestigePlus.Modify
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
                UnitEntityData unit = base.Target.Unit;
                //Logger.Info("start");
                if (unit == null)
                {
                    Logger.Info("no unit");
                    return;
                }
                UnitEntityData maybeCaster = base.Context.MaybeCaster;
                if (maybeCaster == null)
                {
                    Logger.Info("no caster");
                    return;
                }
                var weapon = maybeCaster.GetThreatHandMelee();
                if (weapon == null) { Logger.Info("no weapon"); return; }
                if (weapon.Weapon == null) { Logger.Info("no weapon2"); return; }
                if (weapon.Weapon.Blueprint.Category != Kingmaker.Enums.WeaponCategory.UnarmedStrike && !maybeCaster.HasFact(Feral)) { Logger.Info("not fist"); return; }
                var IsTargetFlatFooted = Rulebook.Trigger(new RuleCheckTargetFlatFooted(maybeCaster, unit)).IsFlatFooted;
                if (IsTargetFlatFooted || unit.State.HasCondition(UnitCondition.Dazed) || unit.State.HasCondition(UnitCondition.LoseDexterityToAC) || unit.State.HasCondition(UnitCondition.Paralyzed) || unit.State.HasCondition(UnitCondition.Staggered) || unit.State.HasCondition(UnitCondition.Stunned) || unit.State.HasCondition(UnitCondition.Unconscious))
                {
                    //Logger.Info("add buff");
                    GameHelper.ApplyBuff(maybeCaster, CasterBuff, new Rounds?(1.Rounds()));
                }
            }catch (Exception ex) { Logger.Error("Failed to medusa.", ex); }
            MeleeRua();
        }

        public void MeleeRua()
        {
            try
            {
                //Logger.Info("start rua"); 
                UnitEntityData maybeCaster = base.Context.MaybeCaster;
                WeaponSlot threatHandMelee = maybeCaster.GetThreatHandMelee(false);
                float meters = threatHandMelee.Weapon.AttackRange.Meters;
                bool selectNewTarget = true;
                TargetWrapper target = base.Target;
                UnitEntityData unitEntityData = target.Unit;
                if (unitEntityData != null)
                {
                    if (selectNewTarget)
                    {
                        //Logger.Info("start rua real");
                        int num = 0;
                        IEnumerable<UnitAttack.AttackInfo> enumerable = UnitAttack.EnumerateFullAttack(maybeCaster);
                        int attacksCount = enumerable.Count<UnitAttack.AttackInfo>();
                        using (IEnumerator<UnitAttack.AttackInfo> enumerator = enumerable.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                UnitAttack.AttackInfo attackInfo = enumerator.Current;
                                this.RunAttackRule(maybeCaster, unitEntityData, attackInfo.Hand, attackInfo.AttackBonusPenalty, num, attacksCount);
                                num++;
                            }
                            return;
                        }
                    }
                    this.RunAttackRule(maybeCaster, unitEntityData, threatHandMelee, 0, 0, 1);
                }
            }
            catch (Exception ex) { Logger.Error("Failed to rua.", ex); }
        }

        private void RunAttackRule(UnitEntityData caster, UnitEntityData target, WeaponSlot hand, int attackBonusPenalty = 0, int attackNumber = 0, int attacksCount = 1)
        {
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(caster, target, hand.Weapon, attackBonusPenalty)
            {
                Reason = base.Context,
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = true,
                AttackNumber = attackNumber,
                AttacksCount = attacksCount
            };
            base.Context.TriggerRule<RuleAttackWithWeapon>(ruleAttackWithWeapon);
        }

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{5BFEF430-F580-4A45-965B-7100D745F055}");
        //FeralCombatTrainingFeature	edb2546d1215491ca404f8dd1a0c2af3
        private static BlueprintFeatureReference Feral = BlueprintTool.GetRef<BlueprintFeatureReference>("edb2546d1215491ca404f8dd1a0c2af3");
    }
}
