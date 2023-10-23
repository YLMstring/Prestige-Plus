using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomAction.GrappleThrow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents;

namespace PrestigePlus.Feats
{
    internal class GrabbingStyle
    {
        private static readonly string StyleName = "GrabbingStyle";
        public static readonly string StyleGuid = "{0A972A01-84B4-45AB-9097-D5E7CB8CC8AA}";

        private static readonly string StyleDisplayName = "GrabbingStyle.Name";
        private static readonly string StyleDescription = "GrabbingStyle.Description";

        private const string Stylebuff = "GrabbingStyle.Stylebuff";
        private static readonly string StylebuffGuid = "{133A887D-5353-465D-B06B-9FC40BB29040}";

        private const string StyleActivatableAbility = "GrabbingStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{293BE690-393A-46B0-B3F3-51DD24C4FD0B}";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
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
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddFacts(new() { ability })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }

        private static readonly string DragName = "GrabbingDrag";
        public static readonly string DragGuid = "{8E2A1EA6-74EF-475D-9B06-3ABAC08F53F1}";

        private static readonly string DragDisplayName = "GrabbingDrag.Name";
        private static readonly string DragDescription = "GrabbingDrag.Description";

        private const string StyleAbility = "GrabbingDrag.StyleAbility";
        private static readonly string StyleAbilityGuid = "{48955E1F-7821-47F0-AF75-D355A0E6BFA6}";
        public static void DragConfigure()
        {
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            var grab = ActionsBuilder.New()
                .Add<PPActionDrag>()
                .Build();

            var grapple = ActionsBuilder.New()
                .ProvokeAttackOfOpportunity(true)
                .CombatManeuver(onSuccess: grab, type: Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple)
                .Build();

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(grapple)
                .SetType(AbilityType.Physical)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetRange(AbilityRange.Close)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .Configure();

            FeatureConfigurator.New(DragName, DragGuid, FeatureGroup.Feat)
                    .SetDisplayName(DragDisplayName)
                    .SetDescription(DragDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 8)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 3)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private static readonly string MasterName = "GrabbingMaster";
        public static readonly string MasterGuid = "{E6382367-C2B3-4A72-8B3E-C5C7C5841874}";

        private static readonly string MasterDisplayName = "GrabbingMaster.Name";
        private static readonly string MasterDescription = "GrabbingMaster.Description";

        public static void MasterConfigure()
        {
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            FeatureConfigurator.New(MasterName, MasterGuid, FeatureGroup.Feat)
                    .SetDisplayName(MasterDisplayName)
                    .SetDescription(MasterDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 12)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 6)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 2)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(DragGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }
    }
}
