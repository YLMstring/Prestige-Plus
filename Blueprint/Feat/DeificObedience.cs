using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Utility;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Enums;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;

namespace PrestigePlus.Blueprint.Feat
{
    internal class DeificObedience
    {
        private const string DeificObedienceFeat = "DeificObedience.DeificObedience";
        private static readonly string DeificObedienceGuid = "{E017281A-7AA3-4C6D-AC27-2D57C45B4A81}";

        internal const string DeificObedienceDisplayName = "DeificObedience.Name";
        private const string DeificObedienceDescription = "DeificObedience.Description";

        private const string DeificObedienceAblityRes = "DeificObedienceAblityRes";
        private static readonly string DeificObedienceAblityResGuid = "{751BD21A-A532-4939-9DC9-EE26E7FCE6BB}";
        public static void DeificObedienceConfigure()
        {
            var icon = AbilityRefs.HolyAura.Reference.Get().Icon;

            var resourse = AbilityResourceConfigurator.New(DeificObedienceAblityRes, DeificObedienceAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(6))
                .Configure();

            var feat = FeatureSelectionConfigurator.New(DeificObedienceFeat, DeificObedienceGuid)
              .SetDisplayName(DeificObedienceDisplayName)
              .SetDescription(DeificObedienceDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              //.AddToAllFeatures(BloodFeat())
              .AddPrerequisiteNoFeature(FeatureRefs.AtheismFeature.ToString())
              .AddPrerequisiteNoFeature(DeificObedienceGuid)
              .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 3)
              .AddAbilityResources(resource: resourse, restoreAmount: true)
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
        }

        private const string Ragathiel = "DeificObedience.Ragathiel";
        private static readonly string RagathielGuid = "{4DCA067D-8EEF-405A-95E7-9687742603D9}";

        internal const string RagathielDisplayName = "DeificObedienceRagathiel.Name";
        private const string RagathielDescription = "DeificObedienceRagathiel.Description";
        public static BlueprintProgression RagathielFeat()
        {
            //"RagathielFeature": "F79778D7-281C-4B9D-8E77-8F86812707AA",
            var icon = AbilityRefs.Angel3BoltOfJusticeAbility.Reference.Get().Icon;

            return ProgressionConfigurator.New(Ragathiel, RagathielGuid)
              .SetDisplayName(RagathielDisplayName)
              .SetDescription(RagathielDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature("F79778D7-281C-4B9D-8E77-8F86812707AA", group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.LawfulGood, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, Ragathiel0Feat())
              .AddToLevelEntry(2, FeatureSelectionRefs.SelectionMercy.ToString())
              .AddToLevelEntry(6, FeatureSelectionRefs.SelectionMercy.ToString())
              .AddToLevelEntry(10, FeatureSelectionRefs.SelectionMercy.ToString())
              .Configure();
        }

        private const string Ragathiel0 = "DeificObedience.Ragathiel0";
        private static readonly string Ragathiel0Guid = "{D0086A74-0DCE-42D3-A82C-047CD6777731}";

        private const string Ragathiel0Ability = "DeificObedience.Ragathiel0Ability";
        private static readonly string Ragathiel0AbilityGuid = "{B4980C7F-DDB6-4A85-87AC-8F44AC1F37A9}";

        internal const string Ragathiel0DisplayName = "DeificObedienceRagathiel0.Name";
        private const string Ragathiel0Description = "DeificObedienceRagathiel0.Description";

        private const string Ragathiel0Buff = "DeificObedience.Ragathiel0Buff";
        private static readonly string Ragathiel0BuffGuid = "{FB52A6B6-D5A2-47D9-9184-6E05A3F30049}";

        private const string Ragathiel0Buff2 = "DeificObedience.Ragathiel0Buff2";
        private static readonly string Ragathiel0Buff2Guid = "{02A4850E-8E1A-48D2-8EEF-13CA34543DB4}";
        public static BlueprintFeature Ragathiel0Feat()
        {
            var icon = AbilityRefs.Angel3BoltOfJusticeAbility.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(Ragathiel0Buff2, Ragathiel0Buff2Guid)
             .SetDisplayName(Ragathiel0DisplayName)
             .SetDescription(Ragathiel0Description)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.RemoveOnRest)
             .AddSavingThrowBonusAgainstAlignment(AlignmentComponent.Evil, 4, ModifierDescriptor.Sacred)
             .Configure();

            var action = ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff2)
                        .Build();

            var Buff = BuffConfigurator.New(Ragathiel0Buff, Ragathiel0BuffGuid)
             .SetDisplayName(Ragathiel0DisplayName)
             .SetDescription(Ragathiel0Description)
             .SetIcon(icon)
             .AddBuffActions(deactivated: action)
             .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Prolong)
             .Configure();

            var ability = AbilityConfigurator.New(Ragathiel0Ability, Ragathiel0AbilityGuid)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.SelfTouch)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(Buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Hours)).Build())
                .SetDisplayName(Ragathiel0DisplayName)
                .SetDescription(Ragathiel0Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityCasterInCombat(true)
                .Configure();

            var action2 = ActionsBuilder.New()
                        .Conditional(ConditionsBuilder.New().Alignment(AlignmentComponent.Evil).Build(), 
                        ifTrue: ActionsBuilder.New()
                            .OnContextCaster(action)
                            .Build())
                        .Build();

            return FeatureConfigurator.New(Ragathiel0, Ragathiel0Guid)
              .SetDisplayName(Ragathiel0DisplayName)
              .SetDescription(Ragathiel0Description)
              .SetIcon(icon)
              .AddInitiatorAttackWithWeaponTrigger(action: action2, onlyHit: true, reduceHPToZero: true)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
