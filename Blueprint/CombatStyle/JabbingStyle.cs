using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.CustomComponent.OtherManeuver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.CombatStyle
{
    internal class JabbingStyle
    {
        private static readonly string StyleName = "JabbingStyle";
        public static readonly string StyleGuid = "{1E5EC86C-6EE4-4843-945F-F733D8A2539A}";

        private static readonly string StyleDisplayName = "JabbingStyle.Name";
        private static readonly string StyleDescription = "JabbingStyle.Description";

        private const string Stylebuff = "JabbingStyle.Stylebuff";
        public static readonly string StylebuffGuid = "{F8E2C7B3-53E0-4087-8BD9-5A45492AA7D3}";

        private const string StyleActivatableAbility = "JabbingStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{9F134F21-A28B-4CBE-9FD7-C0707D405F53}";

        private const string Stylebuff2 = "JabbingStyle.Stylebuff2";
        private static readonly string StylebuffGuid2 = "{43E034B5-71EC-4E8B-802E-706659AC5179}";

        public static void StyleConfigure()
        {
            var icon = FeatureRefs.ImprovedUnarmedStrike.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(Stylebuff2, StylebuffGuid2)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddComponent<JabbingDamage>()
              .SetRanks(2)
              .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Rank)
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(Buff2, ContextDuration.Fixed(1))
                .Build();

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddInitiatorAttackWithWeaponTrigger(action, category: Kingmaker.Enums.WeaponCategory.UnarmedStrike, checkWeaponCategory: true, onlyHit: true)
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
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 1)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddFacts(new() { ability })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }

        private static readonly string MasterName = "JabbingMaster";
        public static readonly string MasterGuid = "{DF43D731-B3C9-4294-8AB9-0369CB9A4851}";

        private static readonly string MasterDisplayName = "JabbingMaster.Name";
        private static readonly string MasterDescription = "JabbingMaster.Description";

        public static void MasterConfigure()
        {
            var icon = FeatureRefs.Evasion.Reference.Get().Icon;

            FeatureConfigurator.New(MasterName, MasterGuid, FeatureGroup.Feat)
                    .SetDisplayName(MasterDisplayName)
                    .SetDescription(MasterDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 12)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 8)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(DancerGuid)
                    .AddPrerequisiteFeature(FeatureRefs.Dodge.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.Mobility.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }

        private static readonly string DancerName = "JabbingDancer";
        public static readonly string DancerGuid = "{017CD697-652B-48C7-A0C3-C21F77FB6C5F}";

        private static readonly string DancerDisplayName = "JabbingDancer.Name";
        private static readonly string DancerDescription = "JabbingDancer.Description";

        public static void DancerConfigure()
        {
            var icon = FeatureRefs.Evasion.Reference.Get().Icon;

            FeatureConfigurator.New(DancerName, DancerGuid, FeatureGroup.Feat)
                    .SetDisplayName(DancerDisplayName)
                    .SetDescription(DancerDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 9)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 5)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(FeatureRefs.Dodge.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.Mobility.ToString())
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }
    }
}
