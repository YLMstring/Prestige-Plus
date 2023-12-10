using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.Blueprints.Classes.Selection;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.Blueprint.ManeuverFeat;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.CustomAction.ClassRelated;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class BloodConduit
    {
        private const string ArchetypeName = "BloodConduit";
        private static readonly string ArchetypeGuid = "{1482ABC3-E122-4236-A38F-50824B6B08A2}";
        internal const string ArchetypeDisplayName = "BloodConduit.Name";
        private const string ArchetypeDescription = "BloodConduit.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.BloodragerClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.BloodragerFastMovement.ToString())
            .SetRemoveFeaturesEntry(2, FeatureRefs.UncannyDodgeChecker.ToString())
            .SetRemoveFeaturesEntry(5, FeatureRefs.ImprovedUncannyDodge.ToString())
            .SetRemoveFeaturesEntry(14, FeatureRefs.BloodragerIndomitableWill.ToString())
            .AddToAddFeatures(1, BloodConduitBonusFeatConfigure())
            .AddToAddFeatures(5, CreateSpellConduit())
            .AddToAddFeatures(14, CreateReflexiveConduit())
              .Configure();
        }

        private const string BloodConduitBonusFeatFeat = "BloodConduit.BloodConduitBonusFeat";
        public static readonly string BloodConduitBonusFeatGuid = "{67422187-9865-4C49-8B26-8C0DB4A7B6AC}";

        internal const string BloodConduitBonusFeatDisplayName = "BloodConduitBonusFeat.Name";
        private const string BloodConduitBonusFeatDescription = "BloodConduitBonusFeat.Description";
        public static BlueprintFeatureSelection BloodConduitBonusFeatConfigure()
        {
            var icon = FeatureSelectionRefs.CombatTrick.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(BloodConduitBonusFeatFeat, BloodConduitBonusFeatGuid)
              .SetDisplayName(BloodConduitBonusFeatDisplayName)
              .SetDescription(BloodConduitBonusFeatDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(true)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.ImprovedBullRush.ToString())
              .AddToAllFeatures(ImprovedGrapple.StyleGuid)
              .AddToAllFeatures(KiThrow.DragGuid)
              .AddToAllFeatures(FeatureRefs.ImprovedTrip.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .Configure();
        }

        private const string SpellConduit = "BloodConduit.SpellConduit";
        public static readonly string SpellConduitGuid = "{5741A74C-21E7-47D2-A9EE-96FC56BBCE82}";

        internal const string SpellConduitDisplayName = "BloodConduitSpellConduit.Name";
        private const string SpellConduitDescription = "BloodConduitSpellConduit.Description";

        private const string SpellConduitAblity2 = "BloodConduit.UseSpellConduit2";
        public static readonly string SpellConduitAblity2Guid = "{ADD98002-1D4E-4E5A-9E76-18DE9D1CE6D0}";

        private const string SpellConduitBuff = "BloodConduit.SpellConduitBuff";
        public static readonly string SpellConduitBuffGuid = "{B4EC3444-86B2-4A94-BB33-AD1B409EF9EF}";

        private const string SpellConduitBuff2 = "BloodConduit.SpellConduitBuff2";
        public static readonly string SpellConduitBuff2Guid = "{397318CC-FA14-4894-A0A3-360121493DD5}";
        public static BlueprintFeature CreateSpellConduit()
        {
            var icon = AbilityRefs.DispelMagic.Reference.Get().Icon;

            BuffConfigurator.New(SpellConduitBuff2, SpellConduitBuff2Guid)
             .SetDisplayName(SpellConduitDisplayName)
             .SetDescription(SpellConduitDescription)
             .SetIcon(icon)
             .AddComponent<BloodConduitTouch>()
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            var buff = BuffConfigurator.New(SpellConduitBuff, SpellConduitBuffGuid)
             .SetDisplayName(SpellConduitDisplayName)
             .SetDescription(SpellConduitDescription)
             .SetIcon(icon)
             .Configure();

            var ability2 = AbilityConfigurator.New(SpellConduitAblity2, SpellConduitAblity2Guid)
              .AllowTargeting(enemies: true)
              .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
              .SetRange(AbilityRange.Touch)
              .SetType(AbilityType.Spell)
              .AddAbilityEffectRunAction(ActionsBuilder.New().Add<BloodSpellConduit>().Build())
              .AddAbilityCasterHasFacts(facts: new() { buff })
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
              .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetDisplayName(SpellConduitDisplayName)
                .SetDescription(SpellConduitDescription)
                .SetIcon(icon)
                .Configure();

            var action = ActionsBuilder.New()
                    .ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true)
                    .Build();

            return FeatureConfigurator.New(SpellConduit, SpellConduitGuid)
              .SetDisplayName(SpellConduitDisplayName)
              .SetDescription(SpellConduitDescription)
              .SetIcon(icon)
              .AddManeuverTrigger(action, Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple, true)
              .AddManeuverTrigger(action, Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush, true)
              .AddManeuverTrigger(action, Kingmaker.RuleSystem.Rules.CombatManeuver.Trip, true)
              .AddInitiatorAttackWithWeaponTrigger(action, category: WeaponCategory.UnarmedStrike, checkWeaponCategory: true, onlyHit: true)
              .AddSpontaneousSpellConversion(CharacterClassRefs.BloodragerClass.ToString(), new() { ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2 })
              .Configure();
        }

        private const string ReflexiveConduit = "BloodConduit.ReflexiveConduit";
        public static readonly string ReflexiveConduitGuid = "{CB78D35A-1559-4541-A673-6A2C410ED63A}";

        internal const string ReflexiveConduitDisplayName = "BloodConduitReflexiveConduit.Name";
        private const string ReflexiveConduitDescription = "BloodConduitReflexiveConduit.Description";

        public static BlueprintFeature CreateReflexiveConduit()
        {
            var icon = AbilityRefs.ElementalBodyIAir.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                    .ApplyBuff(SpellConduitBuffGuid, ContextDuration.Fixed(1), toCaster: true)
                    .Build();

            return FeatureConfigurator.New(ReflexiveConduit, ReflexiveConduitGuid, FeatureGroup.MagusArcana)
              .SetDisplayName(ReflexiveConduitDisplayName)
              .SetDescription(ReflexiveConduitDescription)
              .SetIcon(icon)
              .AddComponent<BloodReflexiveConduit>(c => { c.Action = action; c.ManeuverType = Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple; })
              .AddComponent<BloodReflexiveConduit>(c => { c.Action = action; c.ManeuverType = Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush; })
              .AddComponent<BloodReflexiveConduit>(c => { c.Action = action; c.ManeuverType = Kingmaker.RuleSystem.Rules.CombatManeuver.Trip; })
              .Configure();
        }
    }
}