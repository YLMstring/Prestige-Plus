using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.TurnBasedModifiers;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owlcat.Runtime.UI.ConsoleTools.GamepadInput;
using Kingmaker.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using PrestigePlus.CustomAction.GrappleThrow;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.CustomComponent.Charge;
using PrestigePlus.Blueprint.MythicGrapple;
using Kingmaker.Enums;

namespace PrestigePlus.Blueprint.CombatStyle
{
    internal class SpearDancingStyle
    {
        private static readonly string StyleName = "SpearDancingStyle";
        public static readonly string StyleGuid = "{FEA748E9-2786-465E-87DA-28D94053E0D0}";

        private static readonly string StyleDisplayName = "SpearDancingStyle.Name";
        private static readonly string StyleDescription = "SpearDancingStyle.Description";

        private const string Stylebuff = "SpearDancingStyle.Stylebuff";
        public static readonly string StylebuffGuid = "{0022BBE3-C41A-4EB4-BC26-2488F2916220}";

        private const string StyleActivatableAbility = "SpearDancingStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{D01A6ECB-D068-4AFE-BFF2-46C56C21C131}";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.Evasion.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
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
                    .AddPrerequisiteStatValue(StatType.Dexterity, 13)
                    .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.WeaponFinesse.ToString())
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }

        private static readonly string SpiralName = "SpearDancingSpiral";
        public static readonly string SpiralGuid = "{7677FC3B-1FA6-4213-A0FD-1B1BDCAB4A27}";

        private const string Spiralbuff = "SpearDancingSpiral.Spiralbuff";
        public static readonly string SpiralbuffGuid = "{B79080E0-C908-446C-A901-0D6111677787}";

        private static readonly string SpiralDisplayName = "SpearDancingSpiral.Name";
        private static readonly string SpiralDescription = "SpearDancingSpiral.Description";

        public static void SpiralConfigure()
        {
            var icon = FeatureRefs.Evasion.Reference.Get().Icon;

            var BuffSpiral = BuffConfigurator.New(Spiralbuff, SpiralbuffGuid)
              .SetDisplayName(SpiralDisplayName)
              .SetDescription(SpiralDescription)
              .SetIcon(icon)
              .AddComponent<SpearFlurryUnlock>(c => {
                  c.Flurry1 = FeatureRefs.FlurryOfBlows.Reference;
                  c.Flurry11 = FeatureRefs.FlurryOfBlowsLevel11.Reference;
                  c.SoheiArchetype = ArchetypeRefs.SoheiArchetype.Reference;
                  c.Flurry1stfact = [FeatureRefs.MonkFlurryOfBlowstUnlock.Reference, FeatureRefs.QuarterstaffMasterFlurryUnlock.Reference, FeatureRefs.SoheiFlurryOfBlowsUnlock.Reference, FeatureRefs.ZenArcherFlurryOfBlowsUnlock.Reference];
                  c.Flurry2ndfact = [FeatureRefs.MonkFlurryOfBlowstLevel11Unlock.Reference, FeatureRefs.SoheiFlurryOfBlowstLevel11Unlock.Reference, FeatureRefs.QuarterstaffMasterFlurry11Unlock.Reference, FeatureRefs.ZenArcherFlurryOfBlowsLevel11Unlock.Reference];
              })
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            FeatureConfigurator.New(SpiralName, SpiralGuid, FeatureGroup.Feat)
                    .SetDisplayName(SpiralDisplayName)
                    .SetDescription(SpiralDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 15)
                    .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.WeaponFinesse.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddToIsPrerequisiteFor([FeatureRefs.MonkFlurryOfBlowstUnlock.ToString(), SmashingStyle.CounterGuid, SmashingStyle.MasterGuid])
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }

        private static readonly string ReachName = "SpearDancingReach";
        public static readonly string ReachGuid = "{09DE4AAC-9B10-4590-8BB0-7A19203D5582}";

        private const string Reachbuff = "SpearDancingReach.Reachbuff";
        private static readonly string ReachbuffGuid = "{09DE4AAC-9B10-4590-8BB0-7A19203D5582}";

        private static readonly string ReachDisplayName = "SpearDancingReach.Name";
        private static readonly string ReachDescription = "SpearDancingReach.Description";

        public static void ReachConfigure()
        {
            var icon = FeatureRefs.Evasion.Reference.Get().Icon;

            var BuffReach = BuffConfigurator.New(Reachbuff, ReachbuffGuid)
              .SetDisplayName(ReachDisplayName)
              .SetDescription(ReachDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddStatBonus(null, false, StatType.Reach, 5)
              .Configure();

            FeatureConfigurator.New(ReachName, ReachGuid, FeatureGroup.Feat)
                    .SetDisplayName(ReachDisplayName)
                    .SetDescription(ReachDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 17)
                    .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.WeaponFinesse.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(SpiralGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }
    }
}

