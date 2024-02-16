using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Designers.Mechanics.Facts;
using PrestigePlus.CustomComponent.Archetype;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction.OtherFeatRelated;
using TabletopTweaks.Core.NewComponents;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.RuleSystem;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class WeaponMaster
    {
        private const string ArchetypeName = "WeaponMaster";
        private static readonly string ArchetypeGuid = "{8F3BF8BB-EEC3-44A3-94DD-FB5A39492005}";
        internal const string ArchetypeDisplayName = "WeaponMaster.Name";
        private const string ArchetypeDescription = "WeaponMaster.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.FighterClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(2, FeatureRefs.Bravery.ToString())
            .SetRemoveFeaturesEntry(3, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(5, FeatureSelectionRefs.WeaponTrainingSelection.ToString())
            .SetRemoveFeaturesEntry(6, FeatureRefs.Bravery.ToString())
            .SetRemoveFeaturesEntry(7, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(9, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.Bravery.ToString())
            .SetRemoveFeaturesEntry(11, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(13, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
            .SetRemoveFeaturesEntry(14, FeatureRefs.Bravery.ToString())
            .SetRemoveFeaturesEntry(15, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(17, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
            .SetRemoveFeaturesEntry(18, FeatureRefs.Bravery.ToString())
            .AddToAddFeatures(2, CreateWeaponGuard())
            .AddToAddFeatures(6, WeaponGuardGuid)
            .AddToAddFeatures(10, WeaponGuardGuid)
            .AddToAddFeatures(14, WeaponGuardGuid)
            .AddToAddFeatures(18, WeaponGuardGuid)
            .AddToAddFeatures(3, FeatureSelectionRefs.WeaponTrainingSelection.ToString())
            .AddToAddFeatures(7, FeatureSelectionRefs.WeaponTrainingSelection.ToString())
            .AddToAddFeatures(11, FeatureSelectionRefs.WeaponTrainingSelection.ToString())
            .AddToAddFeatures(15, FeatureSelectionRefs.WeaponTrainingSelection.ToString())
            .AddToAddFeatures(5, FeatureRefs.PerfectStrikeFeature.ToString())
            .AddToAddFeatures(13, FeatureRefs.CriticalFocus.ToString())
            .AddToAddFeatures(17, FeatureRefs.CriticalMastery.ToString())
            .AddToAddFeatures(9, MirrorMoveFeat())
            .AddToAddFeatures(19, UnstoppableStrikeFeat())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.FighterProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.PerfectStrikeFeature.ToString(), FeatureRefs.CriticalFocus.ToString(), FeatureRefs.CriticalMastery.ToString(), UnstoppableStrikeGuid, MirrorMoveGuid })
                .Configure();
        }

        private const string WeaponGuard = "WeaponMaster.WeaponGuard";
        private static readonly string WeaponGuardGuid = "{59B7A808-3012-4FEA-A8E7-CF9E538E41E8}";

        internal const string WeaponGuardDisplayName = "WeaponMasterWeaponGuard.Name";
        private const string WeaponGuardDescription = "WeaponMasterWeaponGuard.Description";
        private static BlueprintFeature CreateWeaponGuard()
        {
            var icon = FeatureRefs.SwordlordDefensiveParryFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(WeaponGuard, WeaponGuardGuid)
              .SetDisplayName(WeaponGuardDisplayName)
              .SetDescription(WeaponGuardDescription)
              .SetIcon(icon)
              .SetRanks(5)
              .SetIsClassFeature(true)
              .AddToIsPrerequisiteFor(CreateMasterTraining())
              .AddCMDBonusAgainstManeuvers(maneuvers: new CombatManeuver[] { CombatManeuver.Disarm, CombatManeuver.SunderArmor }, value: ContextValues.Rank())
              //.AddContextRankConfig(ContextRankConfigs.FeatureRank(WeaponGuardGuid))
              .Configure();
        }

        private const string MasterTraining = "WeaponMaster.MasterTraining";
        private static readonly string MasterTrainingGuid = "{46B3A353-20E7-4AFC-88C8-2FF81DFD74E4}";

        internal const string MasterTrainingDisplayName = "WeaponMasterMasterTraining.Name";
        private const string MasterTrainingDescription = "WeaponMasterMasterTraining.Description";
        private static BlueprintFeatureSelection CreateMasterTraining()
        {
            var icon = FeatureSelectionRefs.WeaponTrainingSelection.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(MasterTraining, MasterTrainingGuid)
              .CopyFrom(
                FeatureSelectionRefs.AdvancedWeaponTraining4,
                typeof(FeatureTagsComponent))
              .SetDisplayName(MasterTrainingDisplayName)
              .SetDescription(MasterTrainingDescription)
              .SetIcon(icon)
              .AddPrerequisiteArchetypeLevel(ArchetypeGuid, CharacterClassRefs.FighterClass.ToString(), level: 4)
              .SetHideNotAvailibleInUI(true)
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.FighterFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();

            return feat;
        }

        private const string MirrorMove = "WeaponMaster.MirrorMove";
        public static readonly string MirrorMoveGuid = "{49B7E309-C339-493C-8F38-A4E4499C1035}";

        internal const string MirrorMoveDisplayName = "WeaponMasterMirrorMove.Name";
        private const string MirrorMoveDescription = "WeaponMasterMirrorMove.Description";
        public static BlueprintFeature MirrorMoveFeat()
        {
            var icon = AbilityRefs.MirrorImage.Reference.Get().Icon;

            return FeatureConfigurator.New(MirrorMove, MirrorMoveGuid)
              .SetDisplayName(MirrorMoveDisplayName)
              .SetDescription(MirrorMoveDescription)
              .SetIcon(icon)
              .AddComponent<WeaponMirrorMoveAC>()
              .Configure();
        }

        private const string UnstoppableStrike = "WeaponMaster.UnstoppableStrike";
        public static readonly string UnstoppableStrikeGuid = "{FE8EB3DE-6212-43B2-B57A-B670EBB84D5C}";

        private const string UnstoppableStrikeAblity = "WeaponMaster.UseUnstoppableStrike";
        private static readonly string UnstoppableStrikeAblityGuid = "{B015C367-68EB-48B7-98E7-52653057E304}";

        private const string UnstoppableStrikeBuff2 = "WeaponMaster.UnstoppableStrikeBuff2";
        public static readonly string UnstoppableStrikeBuff2Guid = "{B5D90088-2338-4132-A0C0-F8621F6041CF}";

        internal const string UnstoppableStrikeDisplayName = "WeaponMasterUnstoppableStrike.Name";
        private const string UnstoppableStrikeDescription = "WeaponMasterUnstoppableStrike.Description";
        public static BlueprintFeature UnstoppableStrikeFeat()
        {
            var icon = FeatureRefs.TwoHandedFighterBackswing.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(UnstoppableStrikeBuff2, UnstoppableStrikeBuff2Guid)
             .SetDisplayName(UnstoppableStrikeDisplayName)
             .SetDescription(UnstoppableStrikeDescription)
             .SetIcon(icon)
             .AddAttackTypeChange(false, false, AttackType.Touch, AttackType.Melee)
             .AddAttackTypeChange(false, false, AttackType.RangedTouch, AttackType.Ranged)
             .AddIgnoreTargetDR(true)
             .Configure();

            var ability = AbilityConfigurator.New(UnstoppableStrikeAblity, UnstoppableStrikeAblityGuid)
                .AllowTargeting(enemies: true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(1), toCaster: true)
                        .Add<OneTouchAttack>(c => { c.anyweapon = true; })
                        .Build())
                .SetDisplayName(UnstoppableStrikeDisplayName)
                .SetDescription(UnstoppableStrikeDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Weapon)
                .SetType(AbilityType.Extraordinary)
                .Configure();

            return FeatureConfigurator.New(UnstoppableStrike, UnstoppableStrikeGuid)
              .SetDisplayName(UnstoppableStrikeDisplayName)
              .SetDescription(UnstoppableStrikeDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
