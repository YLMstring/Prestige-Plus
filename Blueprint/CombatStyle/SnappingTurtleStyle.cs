using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomAction.GrappleThrow;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
{
    internal class SnappingTurtleStyle
    {
        private static readonly string StyleName = "SnappingTurtleStyle";
        public static readonly string StyleGuid = "{1CDF019C-DA2A-493D-8A90-695BB264DA56}";

        private static readonly string StyleDisplayName = "SnappingTurtleStyle.Name";
        private static readonly string StyleDescription = "SnappingTurtleStyle.Description";

        private const string Stylebuff = "SnappingTurtleStyle.Stylebuff";
        private static readonly string StylebuffGuid = "{F86754BC-0488-4DBE-9371-17A70CF3BAE2}";

        private const string StyleActivatableAbility = "SnappingTurtleStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{213C5AFC-1258-429C-AC1C-87B5E9479712}";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.CombatExpertiseFeature.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddComponent<TurtleSpecialAC>()
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
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 1, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddFacts(new() { ability })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }

        private static readonly string ClutchName = "SnappingTurtleClutch";
        public static readonly string ClutchGuid = "{E1EE097B-644D-4280-B4E9-0BBF470729C8}";

        private static readonly string ClutchDisplayName = "SnappingTurtleClutch.Name";
        private static readonly string ClutchDescription = "SnappingTurtleClutch.Description";

        private const string Clutchbuff = "SnappingTurtleStyle.Clutchbuff";
        private static readonly string ClutchbuffGuid = "{5D7C0ACD-C537-45F7-A5E8-97949278F72F}";

        public static void ClutchConfigure()
        {
            var icon = FeatureRefs.CombatExpertiseFeature.Reference.Get().Icon;

            BuffConfigurator.New(Clutchbuff, ClutchbuffGuid)
              .SetDisplayName(ClutchDisplayName)
              .SetDescription(ClutchDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            FeatureConfigurator.New(ClutchName, ClutchGuid, FeatureGroup.Feat)
                    .SetDisplayName(ClutchDisplayName)
                    .SetDescription(ClutchDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 3, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 3, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .AddComponent<TurtleGrapple>()
                    .Configure();
        }

        private static readonly string ShellName = "SnappingTurtleShell";
        public static readonly string ShellGuid = "{952D988B-C54C-41F9-95BB-17E6BE7E22FA}";

        private static readonly string ShellDisplayName = "SnappingTurtleShell.Name";
        private static readonly string ShellDescription = "SnappingTurtleShell.Description";

        public static void ShellConfigure()
        {
            var icon = FeatureRefs.CombatExpertiseFeature.Reference.Get().Icon;

            FeatureConfigurator.New(ShellName, ShellGuid, FeatureGroup.Feat)
                    .SetDisplayName(ShellDisplayName)
                    .SetDescription(ShellDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 5, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(ClutchGuid)
                    //.AddCriticalConfirmationACBonus(4)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }
    }
}
