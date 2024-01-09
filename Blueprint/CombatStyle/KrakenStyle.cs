using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using Kingmaker.Blueprints.TurnBasedModifiers;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Feat;
using BlueprintCore.Utils.Types;

namespace PrestigePlus.Feats
{
    internal class KrakenStyle
    {
        private static readonly string StyleName = "KrakenStyle";
        public static readonly string StyleGuid = "{6271C0E4-5AC2-436C-BAAE-0C88D7ABA115}";

        private static readonly string StyleDisplayName = "KrakenStyle.Name";
        private static readonly string StyleDescription = "KrakenStyle.Description";

        private const string Stylebuff = "KrakenStyle.Stylebuff";
        private static readonly string StylebuffGuid = "{8F08E0D5-9A2B-487A-8CEC-CD3E600D7984}";

        private const string StyleActivatableAbility = "KrakenStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{8A32733B-7926-46B5-B831-BD04DA41F436}";
        public static void StyleConfigure()
        {
            var icon = AbilityRefs.HungryPit.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddComponent<KrakenDamage>()
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
                    .AddPrerequisiteStatValue(StatType.Wisdom, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 3, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 3, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddFacts(new() { ability })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }

        private static readonly string WrathName = "KrakenWrath";
        public static readonly string WrathGuid = "{3D8D9373-F843-4507-B70E-1D8F751257E1}";

        private static readonly string WrathDisplayName = "KrakenWrath.Name";
        private static readonly string WrathDescription = "KrakenWrath.Description";

        private const string StyleAbility = "KrakenStyle.StyleAbility";
        private static readonly string StyleAbilityGuid = "{923ACBBA-D202-4CD1-8441-8BC91D0E0349}";

        private const string StyleAbilitybuff = "KrakenStyle.StyleAbilitybuff";
        private static readonly string StyleAbilitybuffGuid = "{5BFEF430-F580-4A45-965B-7100D745F055}";
        public static void WrathConfigure()
        {
            var icon = AbilityRefs.HungryPit.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<MedusaWrath>()
                    .Build())
                .SetDisplayName(WrathDisplayName)
                .SetDescription(WrathDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetRange(AbilityRange.Weapon)
                .SetType(AbilityType.Physical)
                .Configure();

            FeatureConfigurator.New(WrathName, WrathGuid, FeatureGroup.Feat)
                    .SetDisplayName(WrathDisplayName)
                    .SetDescription(WrathDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 11)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(WrackGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New()
                        .ApplyBuff(StyleAbilitybuffGuid, ContextDuration.Fixed(1))
                        .Build(), true, category: Kingmaker.Enums.WeaponCategory.UnarmedStrike, checkWeaponCategory: true,
                        onlyOnFullAttack: true)
                    .AddToFeatureSelection(FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
                    .Configure();

            BuffConfigurator.New(StyleAbilitybuff, StyleAbilitybuffGuid)
              .SetDisplayName(WrathDisplayName)
              .SetDescription(WrathDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private static readonly string WrackName = "KrakenWrack";
        public static readonly string WrackGuid = "{527797B0-A313-453B-8558-A3F30C657623}";

        private static readonly string WrackDisplayName = "KrakenWrack.Name";
        private static readonly string WrackDescription = "KrakenWrack.Description";

        public static void WrackConfigure()
        {
            var icon = AbilityRefs.HungryPit.Reference.Get().Icon;

            var BuffPin = BuffConfigurator.New(Pinbuff, PinbuffGuid)
              .SetDisplayName(PinDisplayName)
              .SetDescription(PinDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityPin = ActivatableAbilityConfigurator.New(PinAbility, PinAbilityGuid)
                .SetDisplayName(PinDisplayName)
                .SetDescription(PinDescription)
                .SetIcon(FeatureRefs.GreaterSunder.Reference.Get().Icon)
                .SetBuff(BuffPin)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(WrackName, WrackGuid, FeatureGroup.Feat)
                    .SetDisplayName(WrackDisplayName)
                    .SetDescription(WrackDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Wisdom, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 7, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 7, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .AddFacts(new() { abilityPin })
                    .Configure();

        }

        private const string PinAbility = "KrakenWrack.PinAbility";
        private static readonly string PinAbilityGuid = "{70B4C0B3-5D46-4006-8F3B-6A53B9D37D0A}";

        private static readonly string PinDisplayName = "KrakenWrackPin.Name";
        private static readonly string PinDescription = "KrakenWrackPin.Description";

        private const string Pinbuff = "KrakenWrack.Pinbuff";
        private static readonly string PinbuffGuid = "{E355834F-3B1F-4790-A8CA-01F66683550A}";

    }
}
