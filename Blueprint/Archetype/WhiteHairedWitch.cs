using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Feats;
using PrestigePlus.Blueprint.RogueTalent;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.CustomComponent.Archetype;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Actions.Builder;
using PrestigePlus.CustomAction.GrappleThrow;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.Enums.Damage;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic;
using Kingmaker.Designers.Mechanics.Facts;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class WhiteHairedWitch
    {
        private const string ArchetypeName = "WhiteHairedWitch";
        private static readonly string ArchetypeGuid = "{7BB3E733-559A-4B5D-B48E-0FAA47495BE4}";
        internal const string ArchetypeDisplayName = "WhiteHairedWitch.Name";
        private const string ArchetypeDescription = "WhiteHairedWitch.Description";

        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        //[HarmonyBefore(new string[] { "TabletopTweaks-Base" })]
        public static void Configure()
        {
            var arc = ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.WitchClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.WitchHexSelection.ToString(), ProgressionRefs.WitchBetterHexProgression.ToString())
            .SetRemoveFeaturesEntry(2, FeatureSelectionRefs.WitchHexSelection.ToString())
            .SetRemoveFeaturesEntry(4, FeatureSelectionRefs.WitchHexSelection.ToString())
            .SetRemoveFeaturesEntry(6, FeatureSelectionRefs.WitchHexSelection.ToString())
            .SetRemoveFeaturesEntry(8, FeatureSelectionRefs.WitchHexSelection.ToString())
            .SetRemoveFeaturesEntry(10, FeatureSelectionRefs.WitchHexSelection.ToString())
            .SetRemoveFeaturesEntry(12, FeatureSelectionRefs.WitchHexSelection.ToString())
            .SetRemoveFeaturesEntry(14, FeatureSelectionRefs.WitchHexSelection.ToString())
            .SetRemoveFeaturesEntry(16, FeatureSelectionRefs.WitchHexSelection.ToString())
            .SetRemoveFeaturesEntry(18, FeatureSelectionRefs.WitchHexSelection.ToString())
            .SetRemoveFeaturesEntry(20, FeatureSelectionRefs.WitchHexSelection.ToString())
            .AddToAddFeatures(1, CreateWhiteHair())
            .AddToAddFeatures(2, CreateHairConstrict())
            .AddToAddFeatures(4, CreateHairTrip(), CreateHairReach())
            .AddToAddFeatures(6, CreateHairPull())
            .AddToAddFeatures(8, CreateHairStrangle(), HairReachGuid)
            .AddToAddFeatures(10, FeatureSelectionRefs.RogueTalentSelection.ToString(), CreateRogueTraining())
            .AddToAddFeatures(12, FeatureSelectionRefs.RogueTalentSelection.ToString(), HairReachGuid)
            .AddToAddFeatures(14, FeatureSelectionRefs.RogueTalentSelection.ToString())
            .AddToAddFeatures(16, FeatureSelectionRefs.RogueTalentSelection.ToString(), HairReachGuid)
            .AddToAddFeatures(18, FeatureSelectionRefs.RogueTalentSelection.ToString(), FeatureRefs.AdvanceTalents.ToString())
            .AddToAddFeatures(20, FeatureSelectionRefs.RogueTalentSelection.ToString(), HairReachGuid)
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.WitchProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { WhiteHairGuid, HairReachGuid })
                .Configure();
        }

        private const string WhiteHair = "WhiteHairedWitch.WhiteHair";
        private static readonly string WhiteHairGuid = "{64E7BE0E-B19E-4378-BAA3-EF261322A815}";

        private const string WhiteHairAbility = "WhiteHairedWitch.WhiteHairAbility";
        private static readonly string WhiteHairAbilityGuid = "{592470DA-B336-4B03-BF72-D40899058514}";

        internal const string WhiteHairDisplayName = "WhiteHairedWitchWhiteHair.Name";
        private const string WhiteHairDescription = "WhiteHairedWitchWhiteHair.Description";

        private static BlueprintFeature CreateWhiteHair()
        {
            var icon = AbilityRefs.MagicMissile.Reference.Get().Icon;

            var grapple = ActionsBuilder.New()
                .Add<KnotGrapple>(c => { c.isHair = true; })
                .Build();

            var abilityunlimited = AbilityConfigurator.New(WhiteHairAbility, WhiteHairAbilityGuid)
                .CopyFrom(
                AbilityRefs.MagicMissile,
                typeof(AbilityDeliverProjectile))
                .SetDisplayName(WhiteHairDisplayName)
                .SetDescription(WhiteHairDescription)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetType(AbilityType.Physical)
                .SetSpellResistance(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.Long)
                .Configure();

            return FeatureConfigurator.New(WhiteHair, WhiteHairGuid)
              .SetDisplayName(WhiteHairDisplayName)
              .SetDescription(WhiteHairDescription)
              .SetIcon(icon)
              .AddAdditionalLimb(ItemWeaponRefs.Gore1d6.ToString())
              .AddWeaponDamageOverride(formula: new DiceFormula(1, DiceType.D4), overrideDice: true, weaponTypes: new() { WeaponTypeRefs.GoreType.Cast<BlueprintWeaponTypeReference>() })
              .AddComponent<HairExtraDamage>()
              .AddComponent<ReplaceSingleCombatManeuverStat>(c => { c.Type = Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple; c.StatType = StatType.Intelligence; })
              //.AddReplaceSingleCombatManeuverStat(statType: StatType.Intelligence, type: Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple)
              .AddFacts(new() { PinAbilityGuid1, TieUpAbilityGuid, ReadyAbilityGuid, ReleaseAbilityGuid })
              .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => { c.Action = grapple; c.OnlyHit = true; c.CheckWeaponCategory = true; c.Category = WeaponCategory.Gore; c.TriggerBeforeAttack = false; c.IgnoreAutoHit = true; })
              .AddManeuverTrigger(ActionsBuilder.New().CastSpell(abilityunlimited).Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple, false)
              .Configure();
        }

        private const string RogueTraining = "WhiteHairedWitch.RogueTraining";
        private static readonly string RogueTrainingGuid = "{EDE601D1-BB61-4F95-8FEF-665B8A6C0825}";

        private static BlueprintFeature CreateRogueTraining()
        {
            return FeatureConfigurator.New(RogueTraining, RogueTrainingGuid)
              .AddClassLevelsForPrerequisites(actualClass: CharacterClassRefs.WitchClass.ToString(), fakeClass: CharacterClassRefs.SlayerClass.ToString(), modifier: 1, summand: 0)
              .SetHideInUI(true)
              .Configure();
        }

        private static readonly string PinAbilityGuid1 = "{531632AA-0E0F-402C-8A07-18E8E0D35C80}";
        private static readonly string TieUpAbilityGuid = "{DB453CF7-8799-4FDD-941B-FA33EFF5771A}";
        private static readonly string ReadyAbilityGuid = "{A5057A11-9D24-46D8-9BE6-F5C7D605EDC5}";
        private static readonly string ReleaseAbilityGuid = "{A75ED2DD-7F0D-4367-9953-4179F3E531D2}";

        private const string HairConstrict = "WhiteHairedWitch.HairConstrict";
        private static readonly string HairConstrictGuid = "{0221B55C-CEAE-4551-A6CA-94EAFC5803F7}";

        internal const string HairConstrictDisplayName = "WhiteHairedWitchHairConstrict.Name";
        private const string HairConstrictDescription = "WhiteHairedWitchHairConstrict.Description";

        private static BlueprintFeature CreateHairConstrict()
        {
            var icon = AbilityRefs.MagicMissile.Reference.Get().Icon;

            return FeatureConfigurator.New(HairConstrict, HairConstrictGuid)
              .SetDisplayName(HairConstrictDisplayName)
              .SetDescription(HairConstrictDescription)
              .SetIcon(icon)
              .AddComponent<WhiteHairConstrict>()
              .Configure();
        }

        private const string HairTrip = "WhiteHairedWitch.HairTrip";
        private static readonly string HairTripGuid = "{3B1F9701-86A3-4D70-9902-917AD1164A98}";

        internal const string HairTripDisplayName = "WhiteHairedWitchHairTrip.Name";
        private const string HairTripDescription = "WhiteHairedWitchHairTrip.Description";

        private static BlueprintFeature CreateHairTrip()
        {
            var icon = AbilityRefs.MagicMissile.Reference.Get().Icon;

            var grapple = ActionsBuilder.New()
                .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.Trip)
                .Build();

            return FeatureConfigurator.New(HairTrip, HairTripGuid)
              .SetDisplayName(HairTripDisplayName)
              .SetDescription(HairTripDescription)
              .SetIcon(icon)
              .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => { c.Action = grapple; c.OnlyHit = true; c.CheckWeaponCategory = true; c.Category = WeaponCategory.Gore; c.TriggerBeforeAttack = false; c.IgnoreAutoHit = true; })
              .Configure();
        }

        private const string HairPull = "WhiteHairedWitch.HairPull";
        private static readonly string HairPullGuid = "{B3E600B5-508F-45AB-9C82-44BD2FC73134}";

        internal const string HairPullDisplayName = "WhiteHairedWitchHairPull.Name";
        private const string HairPullDescription = "WhiteHairedWitchHairPull.Description";

        private static BlueprintFeature CreateHairPull()
        {
            var icon = AbilityRefs.MagicMissile.Reference.Get().Icon;

            return FeatureConfigurator.New(HairPull, HairPullGuid)
              .SetDisplayName(HairPullDisplayName)
              .SetDescription(HairPullDescription)
              .SetIcon(icon)
              .AddFacts(new() { GrabbingStyle.DragGuid })
              .Configure();
        }

        private const string HairStrangle = "WhiteHairedWitch.HairStrangle";
        private static readonly string HairStrangleGuid = "{67B417C5-9AB1-4582-991A-A6EB4B4BAF1F}";

        internal const string HairStrangleDisplayName = "WhiteHairedWitchHairStrangle.Name";
        private const string HairStrangleDescription = "WhiteHairedWitchHairStrangle.Description";

        private static BlueprintFeature CreateHairStrangle()
        {
            var icon = AbilityRefs.MagicMissile.Reference.Get().Icon;

            return FeatureConfigurator.New(HairStrangle, HairStrangleGuid)
              .SetDisplayName(HairStrangleDisplayName)
              .SetDescription(HairStrangleDescription)
              .SetIcon(icon)
              .AddFacts(new() { CrabKing.Feat2Guid })
              .Configure();
        }

        private const string HairReach = "WhiteHairedWitch.HairReach";
        private static readonly string HairReachGuid = "{83A85FEA-7DC5-4C5A-B233-D6918EEA62AF}";

        internal const string HairReachDisplayName = "WhiteHairedWitchHairReach.Name";
        private const string HairReachDescription = "WhiteHairedWitchHairReach.Description";

        private static BlueprintFeature CreateHairReach()
        {
            var icon = AbilityRefs.MagicMissile.Reference.Get().Icon;

            return FeatureConfigurator.New(HairReach, HairReachGuid)
              .SetDisplayName(HairReachDisplayName)
              .SetDescription(HairReachDescription)
              .SetIcon(icon)
              .SetRanks(10)
              .AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.Reach, ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(HairReachGuid).WithMultiplyByModifierProgression(5))
              .Configure();
        }
    }
}
