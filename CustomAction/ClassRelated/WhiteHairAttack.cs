using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Items.Slots;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Abilities.Components;
using static Pathfinding.Util.RetainedGizmos;
using Kingmaker.UnitLogic;
using static Kingmaker.UnitLogic.Abilities.Components.AbilityCustomVitalStrike;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class WhiteHairAttack : ContextAction
    {
        public override string GetCaption()
        {
            return "WhiteHairAttack";
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
                RunAttackRule(maybeCaster, unit);
            }
            catch (Exception ex) { Logger.Error("Failed to storm.", ex); }
        }
        private void RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit)
        {
            ItemEntityWeapon weapon = null;
            foreach (WeaponSlot weaponSlot in maybeCaster.Body.AdditionalLimbs)
            {
                if (weaponSlot.Weapon?.Blueprint?.Type == WeaponTypeRefs.GoreType.Reference.Get())
                {
                    weapon = weaponSlot.Weapon;
                    break;
                }
            }
            if (weapon != null)
            {
                var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
                maybeCaster.View.AnimationManager.Execute(attackAnimation);
                var caster = maybeCaster;
                VitalStrikePart vitalStrikePart = null;
                if (caster.HasFact(VitalFeat))
                {
                    vitalStrikePart = caster.Ensure<VitalStrikePart>();
                    vitalStrikePart.Caster = caster;
                    vitalStrikePart.VitalStrikeMod = 2;
                    if (caster.HasFact(VitalFeat2))
                    {
                        vitalStrikePart.VitalStrikeMod = 3;
                    }
                    if (caster.HasFact(VitalFeat3))
                    {
                        vitalStrikePart.VitalStrikeMod = 4;
                    }
                    vitalStrikePart.MythicFact = caster.GetFact(VitalFeat4);
                    vitalStrikePart.Rowdy = caster.HasFact(VitalFeat5);
                    vitalStrikePart.Fact = caster.GetFact(VitalFeat);
                }
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
                if (vitalStrikePart != null)
                {
                    vitalStrikePart.SetDeferredRules(ruleAttackWithWeapon);
                    vitalStrikePart.RemoveSelfIfDone();
                }
            }
        }

        private static readonly BlueprintFeatureReference VitalFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeFeature.ToString());

        private static readonly BlueprintFeatureReference VitalFeat2 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeFeatureImproved.ToString());
        private static readonly BlueprintFeatureReference VitalFeat3 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeFeatureGreater.ToString());

        private static readonly BlueprintFeatureReference VitalFeat4 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeMythicFeat.ToString());
        private static readonly BlueprintFeatureReference VitalFeat5 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.RowdyVitalDamage.ToString());
    }
}