using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.CustomComponent;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents;

namespace PrestigePlus.Feats
{
    internal class BlackSeraphStyle
    {
        private static readonly string StyleName = "BlackSeraphStyle";
        public static readonly string StyleGuid = "{EA626222-196D-4CF0-A405-65A872E824AD}";

        private static readonly string StyleDisplayName = "BlackSeraphStyle.Name";
        private static readonly string StyleDescription = "BlackSeraphStyle.Description";

        private const string Stylebuff = "BlackSeraphStyle.Stylebuff";
        private static readonly string StylebuffGuid = "{B2CD75FD-3DF5-4B1D-88F4-221F01782FC1}";

        private const string StyleActivatableAbility = "BlackSeraphStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{CA378D34-1E4B-419F-94BE-EBD59896BDDD}";
        public static void StyleConfigure()
        {
            var icon = AbilityRefs.Destruction.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
                    .AddACContextBonusAgainstFactOwner(bonus: ContextValues.Rank(), checkedFact: BuffRefs.Shaken.ToString(), descriptor: ModifierDescriptor.Profane)
                    .AddACContextBonusAgainstFactOwner(bonus: ContextValues.Rank(), checkedFact: BuffRefs.ShakenSystemCondition.ToString(), descriptor: ModifierDescriptor.Profane)
                    .AddACContextBonusAgainstFactOwner(bonus: ContextValues.Rank(), checkedFact: BuffRefs.Frightened.ToString(), descriptor: ModifierDescriptor.Profane)
                    .AddACContextBonusAgainstFactOwner(bonus: ContextValues.Rank(), checkedFact: BuffRefs.CowerBuff.ToString(), descriptor: ModifierDescriptor.Profane)
                    .AddACContextBonusAgainstFactOwner(bonus: ContextValues.Rank(), checkedFact: BuffRefs.CoweringCommonBuff.ToString(), descriptor: ModifierDescriptor.Profane)
                    .AddComponent<ContextSavingThrowBonusAgainstFact>(c => { c.Bonus = ContextValues.Rank(); c.CheckedFact =  BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.Shaken.ToString()); c.Descriptor = ModifierDescriptor.Profane; c.Value = 0; })
                    .AddComponent<ContextSavingThrowBonusAgainstFact>(c => { c.Bonus = ContextValues.Rank(); c.CheckedFact =  BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.ShakenSystemCondition.ToString()); c.Descriptor = ModifierDescriptor.Profane; c.Value = 0; })
                    .AddComponent<ContextSavingThrowBonusAgainstFact>(c => { c.Bonus = ContextValues.Rank(); c.CheckedFact =  BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.Frightened.ToString()); c.Descriptor = ModifierDescriptor.Profane; c.Value = 0; })
                    .AddComponent<ContextSavingThrowBonusAgainstFact>(c => { c.Bonus = ContextValues.Rank(); c.CheckedFact =  BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.CowerBuff.ToString()); c.Descriptor = ModifierDescriptor.Profane; c.Value = 0; })
                    .AddComponent<ContextSavingThrowBonusAgainstFact>(c => { c.Bonus = ContextValues.Rank(); c.CheckedFact =  BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.CoweringCommonBuff.ToString()); c.Descriptor = ModifierDescriptor.Profane; c.Value = 0; })
                    .AddContextRankConfig(ContextRankConfigs.CharacterLevel().WithCustomProgression((4, 1), (9, 2), (14, 3), (19, 4), (20, 5)))
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(StyleActivatableAbility, StyleActivatableAbilityGuid)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetGroup(ActivatableAbilityGroup.CombatStyle)
                .SetWeightInGroup(1)
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(StyleName, StyleGuid, FeatureGroup.Feat)
                    .SetDisplayName(StyleDisplayName)
                    .SetDescription(StyleDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.SkillPersuasion, 3, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(ParametrizedFeatureRefs.WeaponFocus.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddFacts(new() { ability })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }

        private static readonly string MalevolenceName = "BlackSeraphMalevolence";
        public static readonly string MalevolenceGuid = "{D307E1C7-BD6A-4A54-9163-906C0582E208}";

        private static readonly string MalevolenceDisplayName = "BlackSeraphMalevolence.Name";
        private static readonly string MalevolenceDescription = "BlackSeraphMalevolence.Description";

        public static void MalevolenceConfigure()
        {
            var icon = AbilityRefs.Destruction.Reference.Get().Icon;

            FeatureConfigurator.New(MalevolenceName, MalevolenceGuid, FeatureGroup.Feat)
                    .SetDisplayName(MalevolenceDisplayName)
                    .SetDescription(MalevolenceDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.SkillPersuasion, 7)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .AddBuffActions(newRound: ActionsBuilder.New()
                        .Conditional(ConditionsBuilder.New().Add<MalevolenceEnchant>().Build(), ifTrue: ActionsBuilder.New()
                            .EnchantWornItem(ContextDuration.Fixed(1), WeaponEnchantmentRefs.Holy.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
                            .EnchantWornItem(ContextDuration.Fixed(1), WeaponEnchantmentRefs.Holy.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
                            .Build())
                        .Build())
                    .Configure();

        }

        private static readonly string AnnihilationName = "BlackSeraphAnnihilation";
        public static readonly string AnnihilationGuid = "{AE9AD4D6-AF47-4BAD-95EB-89AE73C6D9FD}";

        private static readonly string AnnihilationDisplayName = "BlackSeraphAnnihilation.Name";
        private static readonly string AnnihilationDescription = "BlackSeraphAnnihilation.Description";

        private const string AuraBuff = "BlackSeraphStyle.Annihilationbuff";
        private static readonly string AuraBuffGuid = "{CC9F690E-0F1B-4947-8904-689876D7B96F}";

        private const string AuraBuff2 = "BlackSeraphStyle.Annihilationbuff2";
        private static readonly string AuraBuff2Guid = "{D1F58182-5055-49AF-9D62-63D4AF98A69E}";

        private const string AnnihilationAura = "BlackSeraphStyle.AnnihilationAura";
        private static readonly string AnnihilationAuraGuid = "{2FC689CD-5F2F-4029-88DD-3A3DA136E05F}";

        public static void AnnihilationConfigure()
        {
            var icon = AbilityRefs.Destruction.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AuraBuff2, AuraBuff2Guid)
              .SetDisplayName(AnnihilationDisplayName)
              .SetDescription(AnnihilationDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<SpellDescriptorImmunityIgnore>(c => {
                  c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened;
              })
              .AddComponent<BuffDescriptorImmunityIgnore>(c => {
                  c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.MindAffecting | SpellDescriptor.Emotion | SpellDescriptor.NegativeEmotion;
              })
              .Configure();

            var area = AbilityAreaEffectConfigurator.New(AnnihilationAura, AnnihilationAuraGuid)
                .AddAbilityAreaEffectBuff(Buff2, checkConditionEveryRound: true, condition: ConditionsBuilder.New().IsCaster(true).Build())
                .SetAggroEnemies(true)
                .SetAffectEnemies(true)
                .SetAffectDead(true)
                .SetTargetType(BlueprintAbilityAreaEffect.TargetType.Any)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(FeetExtension.Feet(33))
                .Configure();
            
            var Buff1 = BuffConfigurator.New(AuraBuff, AuraBuffGuid)
              .SetDisplayName(AnnihilationDisplayName)
              .SetDescription(AnnihilationDescription)
              .SetIcon(icon)
              .AddAreaEffect(area)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            FeatureConfigurator.New(AnnihilationName, AnnihilationGuid, FeatureGroup.Feat)
                    .SetDisplayName(AnnihilationDisplayName)
                    .SetDescription(AnnihilationDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.SkillPersuasion, 11)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(MalevolenceGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .AddAuraFeatureComponent(Buff1)
                    .Configure();

        }
    }
}
