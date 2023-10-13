using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class SurprisingStrategy
    {
        public static readonly string FeatName = "SurprisingStrategy";
        public static readonly string FeatGuid = "{7C7164B9-2F5B-4F39-A8B0-8CAB1C160BC9}";

        public static readonly string DisplayName = "SurprisingStrategy.Name";
        public static readonly string Description = "SurprisingStrategy.Description";

        public const string SurprisingAbility = "SurprisingStrategy.SurprisingStrategyAbility2";
        public static readonly string SurprisingAbilityGuid = "{7B2F7D14-1CB4-4955-AAC5-E3257D4225F0}";

        public const string SurprisingDirtyBlindAbility = "SurprisingDirtyBlind.SurprisingDirtyBlindAbility";
        public static readonly string SurprisingDirtyBlindAbilityGuid = "{E4621562-FF01-4BB0-9370-14B78E600316}";

        public const string SurprisingDirtyBlindbuff = "SurprisingDirtyBlind.SurprisingDirtyBlindbuff";
        public static readonly string SurprisingDirtyBlindbuffGuid = "{38CDB49B-3412-42C6-956B-37FFACF17416}";

        public const string SurprisingDirtyEntangleAbility = "SurprisingDirtyEntangle.SurprisingDirtyEntangleAbility";
        public static readonly string SurprisingDirtyEntangleAbilityGuid = "{47FF2240-816A-41DF-9E66-E723642499AA}";

        public const string SurprisingDirtyEntanglebuff = "SurprisingDirtyEntangle.SurprisingDirtyEntanglebuff";
        public static readonly string SurprisingDirtyEntanglebuffGuid = "{41FCA336-CD5F-41BF-AFC3-2008A6CE6301}";

        public const string SurprisingDirtySickenAbility = "SurprisingDirtySicken.SurprisingDirtySickenAbility";
        public static readonly string SurprisingDirtySickenAbilityGuid = "{6DFF524D-3C15-42D9-86ED-802F000C4CDC}";

        public const string SurprisingDirtySickenbuff = "SurprisingDirtySicken.SurprisingDirtySickenbuff";
        public static readonly string SurprisingDirtySickenbuffGuid = "{2376F3E9-EB70-4443-8D9A-641DD1E22B74}";

        public const string SurprisingDisarmAbility = "SurprisingDisarm.SurprisingDisarmAbility";
        public static readonly string SurprisingDisarmAbilityGuid = "{5E2C4462-F63B-4E44-A72B-EFB7B9E292BC}";

        public const string SurprisingDisarmbuff = "SurprisingDisarm.SurprisingDisarmbuff";
        public static readonly string SurprisingDisarmbuffGuid = "{8F80CD8E-20C2-49BF-B0E3-4866D5DF872E}";

        public const string SurprisingSunderAbility = "SurprisingSunder.SurprisingSunderAbility";
        public static readonly string SurprisingSunderAbilityGuid = "{45D986DF-56F1-48AB-927E-9066C6C478E0}";

        public const string SurprisingSunderbuff = "SurprisingSunder.SurprisingSunderbuff";
        public static readonly string SurprisingSunderbuffGuid = "{CFA3BC1E-E96D-4907-BB41-BC41E3CB184A}";

        public static void Configure()
        {
            var icon = FeatureRefs.HuntersSurprise.Reference.Get().Icon;

            var BuffSurprisingDirtyBlind = BuffConfigurator.New(SurprisingDirtyBlindbuff, SurprisingDirtyBlindbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDirtyBlind = ActivatableAbilityConfigurator.New(SurprisingDirtyBlindAbility, SurprisingDirtyBlindAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.DirtyTrickBlindnessAction.Reference.Get().Icon)
                .SetBuff(BuffSurprisingDirtyBlind)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false).SetActionBarAutoFillIgnored(true)
                .Configure();

            var BuffSurprisingDirtyEntangle = BuffConfigurator.New(SurprisingDirtyEntanglebuff, SurprisingDirtyEntanglebuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDirtyEntangle = ActivatableAbilityConfigurator.New(SurprisingDirtyEntangleAbility, SurprisingDirtyEntangleAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.DirtyTrickEntangleAction.Reference.Get().Icon)
                .SetBuff(BuffSurprisingDirtyEntangle)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false).SetActionBarAutoFillIgnored(true)
                .Configure();

            var BuffSurprisingDirtySicken = BuffConfigurator.New(SurprisingDirtySickenbuff, SurprisingDirtySickenbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDirtySicken = ActivatableAbilityConfigurator.New(SurprisingDirtySickenAbility, SurprisingDirtySickenAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.DirtyTrickSickenedAction.Reference.Get().Icon)
                .SetBuff(BuffSurprisingDirtySicken)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false).SetActionBarAutoFillIgnored(true)
                .Configure();

            var BuffSurprisingDisarm = BuffConfigurator.New(SurprisingDisarmbuff, SurprisingDisarmbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDisarm = ActivatableAbilityConfigurator.New(SurprisingDisarmAbility, SurprisingDisarmAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.DisarmAction.Reference.Get().Icon)
                .SetBuff(BuffSurprisingDisarm)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false).SetActionBarAutoFillIgnored(true)
                .Configure();

            var BuffSurprisingSunder = BuffConfigurator.New(SurprisingSunderbuff, SurprisingSunderbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilitySunder = ActivatableAbilityConfigurator.New(SurprisingSunderAbility, SurprisingSunderAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.SunderAction.Reference.Get().Icon)
                .SetBuff(BuffSurprisingSunder)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false).SetActionBarAutoFillIgnored(true)
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(SurprisingAbility, SurprisingAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .AddActivationDisable()
                .SetDeactivateImmediately()
                .SetActivationType(AbilityActivationType.Immediately)
                .AddActivatableAbilityVariants(variants: new() { abilityDirtyBlind, abilityDirtyEntangle, abilityDirtySicken, abilityDisarm, abilitySunder })
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.DuelistRiposte.ToString())
                    .AddFacts(new() { ability, abilityDirtyBlind, abilityDirtyEntangle, abilityDirtySicken, abilityDisarm, abilitySunder })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
