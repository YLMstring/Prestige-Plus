using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Grapple
{
    internal class ConditionTwoFreeHand : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {

        private static bool CheckCondition(UnitEntityData unit)
        {
            var secondaryHand = unit.Body.CurrentHandsEquipmentSet.SecondaryHand;
            var primaryHand = unit.Body.CurrentHandsEquipmentSet.PrimaryHand;
            bool hasFreeHand = true;
            if (secondaryHand.HasShield)
            {
                var maybeShield = secondaryHand.MaybeShield;
                hasFreeHand &= maybeShield.Blueprint.Type.ProficiencyGroup == ArmorProficiencyGroup.Buckler;
            }
            else if (secondaryHand.HasWeapon && secondaryHand.MaybeWeapon != unit.Body.EmptyHandWeapon)
            {
                hasFreeHand = false;
            }
            if (primaryHand.HasWeapon && primaryHand.MaybeWeapon != unit.Body.EmptyHandWeapon)
            {
                hasFreeHand = false;
            }
            return hasFreeHand;
        }

        public static bool HasFreeHand(UnitEntityData unit)
        {
            var secondaryHand = unit.Body.CurrentHandsEquipmentSet.SecondaryHand;
            var primaryHand = unit.Body.CurrentHandsEquipmentSet.PrimaryHand;
            bool hasFreeHand = true;
            if (secondaryHand.HasShield)
            {
                var maybeShield = secondaryHand.MaybeShield;
                hasFreeHand &= maybeShield.Blueprint.Type.ProficiencyGroup == ArmorProficiencyGroup.Buckler;
            }
            else if (secondaryHand.HasWeapon && secondaryHand.MaybeWeapon != unit.Body.EmptyHandWeapon)
            {
                hasFreeHand = false;
            }
            if (primaryHand.HasWeapon)
            {
                hasFreeHand &= !primaryHand.MaybeWeapon.HoldInTwoHands;
            }
            return hasFreeHand;
        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
            if (evt.Type != CombatManeuver.Grapple) { return; }
            if (Owner.HasFact(GrabbingStyle) || Owner.HasFact(Bear) || Owner.HasFact(Tiger) || Owner.HasFact(Lizard) || Owner.HasFact(Griffon))
            {
                return;
            }
            if (CheckCondition(Owner)) { return; }
            evt.AddModifier(-4, Fact, descriptor: Kingmaker.Enums.ModifierDescriptor.Penalty);
        }

        //private static BlueprintBuffReference HamatulaStrike = BlueprintTool.GetRef<BlueprintBuffReference>("{2AF7906A-C641-4596-B6A7-DF1F0CDA8758}");
        private static BlueprintBuffReference GrabbingStyle = BlueprintTool.GetRef<BlueprintBuffReference>("{133A887D-5353-465D-B06B-9FC40BB29040}");

        private static BlueprintFeatureReference Bear = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ShifterGrabBear.ToString());
        private static BlueprintFeatureReference Tiger = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ShifterGrabTiger.ToString());
        private static BlueprintFeatureReference Lizard = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ShifterGrabLizard.ToString());
        private static BlueprintFeatureReference Griffon = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ShifterGrabGriffon.ToString());
        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {

        }
    }
}