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
using PrestigePlus.CustomAction.ClassRelated;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Buffs;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UI.MVVM._VM.Other;
using Kingmaker.UnitLogic.ActivatableAbilities;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class DrunkenBrute
    {
        private const string ArchetypeName = "DrunkenBrute";
        private static readonly string ArchetypeGuid = "{F07FF589-843C-4FC8-9DE8-AB8A720015BF}";
        internal const string ArchetypeDisplayName = "DrunkenBrute.Name";
        private const string ArchetypeDescription = "DrunkenBrute.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.BarbarianClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
              .SetRemoveFeaturesEntry(1, FeatureRefs.FastMovement.ToString())
              .AddToAddFeatures(1, CreateRagingDrunk())
              .Configure();
        }

        private const string LiquidCourage = "DrunkenBrute.LiquidCourage";
        public static readonly string LiquidCourageGuid = "{B31398F4-C2A2-4098-A99C-9FF96BBBAB15}";

        private const string LiquidCourageBuff2 = "DrunkenBrute.LiquidCourageBuff2";
        private static readonly string LiquidCourageBuff2Guid = "{7B4698E4-1605-4A51-AE3F-0C9A3737CAF5}";

        internal const string LiquidCourageDisplayName = "DrunkenBruteLiquidCourage.Name";
        private const string LiquidCourageDescription = "DrunkenBruteLiquidCourage.Description";

        public static BlueprintFeature LiquidCourageFeat()
        {
            var icon = AbilityRefs.Nurah_InspirecourageAbility.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(LiquidCourageBuff2, LiquidCourageBuff2Guid)
             .SetDisplayName(LiquidCourageDisplayName)
             .SetDescription(LiquidCourageDescription)
             .SetIcon(icon)
             .AddSavingThrowBonusAgainstDescriptor(ContextValues.Rank(), modifierDescriptor: ModifierDescriptor.Morale, spellDescriptor: SpellDescriptor.MindAffecting)
             .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.BarbarianClass.ToString() }).WithDivStepProgression(4))
             .Configure();

            return FeatureConfigurator.New(LiquidCourage, LiquidCourageGuid, FeatureGroup.RagePower)
              .SetDisplayName(LiquidCourageDisplayName)
              .SetDescription(LiquidCourageDescription)
              .SetIcon(icon)
              .AddBuffExtraEffects(BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: Buff)
              .Configure();
        }

        private const string RoaringDrunk = "DrunkenBrute.RoaringDrunk";
        public static readonly string RoaringDrunkGuid = "{8D42EB74-F367-4D1F-9039-F4792D578A93}";

        private const string RoaringDrunkBuff2 = "DrunkenBrute.RoaringDrunkBuff2";
        private static readonly string RoaringDrunkBuff2Guid = "{450961C2-6347-47B3-8F21-4D757329EC06}";

        internal const string RoaringDrunkDisplayName = "DrunkenBruteRoaringDrunk.Name";
        private const string RoaringDrunkDescription = "DrunkenBruteRoaringDrunk.Description";

        public static BlueprintFeature RoaringDrunkFeat()
        {
            var icon = AbilityRefs.Nurah_InspirecourageAbility.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(RoaringDrunkBuff2, RoaringDrunkBuff2Guid)
             .SetDisplayName(RoaringDrunkDisplayName)
             .SetDescription(RoaringDrunkDescription)
             .SetIcon(icon)
             .AddIncreaseSpellContextDescriptorDC(SpellDescriptor.Fear, modifierDescriptor: ModifierDescriptor.Morale, onlyOnce: false, spellsOnly: false, value: ContextValues.Rank())
             .AddContextStatBonus(StatType.CheckIntimidate, ContextValues.Rank(), ModifierDescriptor.Morale)
             .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.BarbarianClass.ToString() }).WithDivStepProgression(4))
             .Configure();

            return FeatureConfigurator.New(RoaringDrunk, RoaringDrunkGuid, FeatureGroup.RagePower)
              .SetDisplayName(RoaringDrunkDisplayName)
              .SetDescription(RoaringDrunkDescription)
              .SetIcon(icon)
              .AddBuffExtraEffects(BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: Buff)
              .Configure();
        }

        private const string Atavism = "DrunkenBrute.Atavism";
        public static readonly string AtavismGuid = "{6E229CA1-796C-4C98-9FB9-89B003BD81EC}";

        private const string AtavismBuff2 = "DrunkenBrute.AtavismBuff2";
        private static readonly string AtavismBuff2Guid = "{BF4A421D-A546-4176-83EB-6BF2D8E2106C}";

        internal const string AtavismDisplayName = "DrunkenBruteAtavism.Name";
        private const string AtavismDescription = "DrunkenBruteAtavism.Description";

        public static BlueprintFeature AtavismFeat()
        {
            var icon = AbilityRefs.Nurah_InspirecourageAbility.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(AtavismBuff2, AtavismBuff2Guid)
             .SetDisplayName(AtavismDisplayName)
             .SetDescription(AtavismDescription)
             .SetIcon(icon)
             .AddFacts(new() { UnitFactRefs.Ferocity.ToString() })
             .Configure();

            return FeatureConfigurator.New(Atavism, AtavismGuid, FeatureGroup.RagePower)
              .SetDisplayName(AtavismDisplayName)
              .SetDescription(AtavismDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.AnimalFuryFeature.ToString())
              .AddPrerequisiteClassLevel(CharacterClassRefs.BarbarianClass.ToString(), 6)
              .AddBuffExtraEffects(BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: Buff)
              .Configure();
        }

        private const string GreaterAtavism = "DrunkenBrute.GreaterAtavism";
        public static readonly string GreaterAtavismGuid = "{C17C5BBA-78AC-4304-B079-289D617384F8}";

        private const string GreaterAtavismBuff2 = "DrunkenBrute.GreaterAtavismBuff2";
        private static readonly string GreaterAtavismBuff2Guid = "{E9EF8211-564D-4C37-B324-4DD1B47C3820}";

        internal const string GreaterAtavismDisplayName = "DrunkenBruteGreaterAtavism.Name";
        private const string GreaterAtavismDescription = "DrunkenBruteGreaterAtavism.Description";

        public static BlueprintFeature GreaterAtavismFeat()
        {
            var icon = AbilityRefs.Nurah_InspirecourageAbility.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(GreaterAtavismBuff2, GreaterAtavismBuff2Guid)
             .SetDisplayName(GreaterAtavismDisplayName)
             .SetDescription(GreaterAtavismDescription)
             .SetIcon(icon)
             .AddFacts(new() { AbilityRefs.OverrunAbility.ToString() })
             .Configure();

            return FeatureConfigurator.New(GreaterAtavism, GreaterAtavismGuid, FeatureGroup.RagePower)
              .SetDisplayName(GreaterAtavismDisplayName)
              .SetDescription(GreaterAtavismDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(AtavismGuid)
              .AddPrerequisiteClassLevel(CharacterClassRefs.BarbarianClass.ToString(), 10)
              .AddBuffExtraEffects(BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: Buff)
              .Configure();
        }

        private const string Glare = "DrunkenBrute.Glare";
        public static readonly string GlareGuid = "{F38E81C8-AB8D-4FF5-B24E-063C0B5E3504}";

        internal const string GlareDisplayName = "DrunkenBruteGlare.Name";
        private const string GlareDescription = "DrunkenBruteGlare.Description";

        private const string GlareAblity = "DrunkenBrute.UseGlare";
        public static readonly string GlareAblityGuid = "{5DFE0FCE-57B5-44FC-8279-A92A0DB501F0}";

        private const string GlareBuff2 = "DrunkenBrute.GlareBuff2";
        public static readonly string GlareBuff2Guid = "{B28F0D2F-3785-41A2-A970-01CB2CD9D2AC}";
        public static BlueprintFeature CreateGlare()
        {
            var icon = FeatureRefs.SpellStrikeFeature.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(GlareAblity, GlareAblityGuid)
              .SetDisplayName(GlareDisplayName)
              .SetDescription(GlareDescription)
              .SetIcon(icon)
              .AllowTargeting(enemies: true)
              .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
              .SetRange(AbilityRange.Custom)
              .SetCustomRange(5)
              .SetType(AbilityType.Extraordinary)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
              .AddAbilityEffectRunAction(ActionsBuilder.New().CastSpell(AbilityRefs.PersuasionUseAbility.ToString()).Build())
              .Configure();

            var Buff = BuffConfigurator.New(GlareBuff2, GlareBuff2Guid)
             .SetDisplayName(GlareDisplayName)
             .SetDescription(GlareDescription)
             .SetIcon(icon)
             .AddFacts(new() { ability })
             .Configure();

            return FeatureConfigurator.New(Glare, GlareGuid, FeatureGroup.RagePower)
              .SetDisplayName(GlareDisplayName)
              .SetDescription(GlareDescription)
              .SetIcon(icon)
              .AddBuffExtraEffects(BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: Buff)
              .Configure();
        }

        private const string RagingDrunk = "DrunkenBrute.RagingDrunk";
        public static readonly string RagingDrunkGuid = "{A3341D0B-992F-4EB7-AA5D-A913AEED6CE2}";

        internal const string RagingDrunkDisplayName = "DrunkenBruteRagingDrunk.Name";
        private const string RagingDrunkDescription = "DrunkenBruteRagingDrunk.Description";

        private const string RagingDrunkAbility = "DrunkenBrute.UseRagingDrunk";
        public static readonly string RagingDrunkAbilityGuid = "{9D0D1986-CAC8-4FCC-AFFD-4EFB35710461}";

        private const string RagingDrunkBuff1 = "DrunkenBrute.RagingDrunkBuff1";
        public static readonly string RagingDrunkBuff1Guid = "{FCAA7865-7E26-423F-84C9-1A2B414E6E31}";

        private const string RagingDrunkBuff2 = "DrunkenBrute.RagingDrunkBuff2";
        public static readonly string RagingDrunkBuff2Guid = "{E6BA4067-CD1B-4163-B938-4B779EC572A3}";

        private const string RagingDrunkBuff3 = "DrunkenBrute.RagingDrunkBuff3";
        public static readonly string RagingDrunkBuff3Guid = "{65D8B448-8660-4722-99B0-55BF323E3B45}";
        public static BlueprintFeature CreateRagingDrunk()
        {
            var icon = FeatureRefs.SpellStrikeFeature.Reference.Get().Icon;

            var Buff3 = BuffConfigurator.New(RagingDrunkBuff3, RagingDrunkBuff3Guid)
             .SetDisplayName(RagingDrunkDisplayName)
             .SetDescription(RagingDrunkDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            var Buff1 = BuffConfigurator.New(RagingDrunkBuff1, RagingDrunkBuff1Guid)
             .SetDisplayName(RagingDrunkDisplayName)
             .SetDescription(RagingDrunkDescription)
             .SetIcon(icon)
             .AddCondition(Kingmaker.UnitLogic.UnitCondition.Staggered)
             .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New()
                    .RestoreResource(AbilityResourceRefs.RageResourse.ToString(), 1)
                    .RestoreResource(AbilityResourceRefs.BloodragerRageResource.ToString(), 1)
                    .RestoreResource(AbilityResourceRefs.FocusedRageResourse.ToString(), 1)
                    .ApplyBuffPermanent(Buff3)
                    .Build())
             .Configure();

            var ability = ActivatableAbilityConfigurator.New(RagingDrunkAbility, RagingDrunkAbilityGuid)
                .SetDisplayName(RagingDrunkDisplayName)
                .SetDescription(RagingDrunkDescription)
                .SetIcon(icon)
                .AddActivatableAbilityUnitCommand(type: Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetBuff(Buff1)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetDeactivateIfOwnerDisabled(true)
                .SetDeactivateIfCombatEnded(true)
                .SetDeactivateImmediately()
                .Configure();

            var Buff2 = BuffConfigurator.New(RagingDrunkBuff2, RagingDrunkBuff2Guid)
             .SetDisplayName(RagingDrunkDisplayName)
             .SetDescription(RagingDrunkDescription)
             .SetIcon(icon)
             .AddFacts(new() { ability })
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddBuffActions(deactivated: ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New()
                        .HasFact(RagingDrunkBuff2Guid)
                        .HasFact(FeatureRefs.InternalFortitudeFeature.ToString(),true)
                        .Build(), ActionsBuilder.New().ApplyBuff(BuffRefs.Nauseated.ToString(), ContextDuration.Fixed(10)).Build())
                    .Build())
             .Configure();

            return FeatureConfigurator.New(RagingDrunk, RagingDrunkGuid, FeatureGroup.RagePower)
              .SetDisplayName(RagingDrunkDisplayName)
              .SetDescription(RagingDrunkDescription)
              .SetIcon(icon)
              .AddBuffExtraEffects(BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: Buff2)
              .AddBuffExtraEffects(BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: Buff2)
              .AddBuffExtraEffects(BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: Buff2)
              .AddToIsPrerequisiteFor(RoaringDrunkGuid)
              .AddToIsPrerequisiteFor(LiquidCourageGuid)
              .Configure();
        }
    }
}
