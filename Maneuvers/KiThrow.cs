using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction;
using PrestigePlus.Grapple;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class KiThrow
    {
        private static readonly string DragName = "KiThrow";
        public static readonly string DragGuid = "{AC5ECA78-2D85-4894-985F-4850F95333FC}";

        private static readonly string DragDisplayName = "KiThrow.Name";
        private static readonly string DragDescription = "KiThrow.Description";

        private const string StyleAbility = "KiThrow.StyleAbility";
        private static readonly string StyleAbilityGuid = "{930A6A3E-E11B-4AA5-A6D9-215B244ED426}";

        private const string StyleBuff = "KiThrow.StyleBuff";
        private static readonly string StyleBuffGuid = "{98558112-717B-456D-838F-EFF5061F4D38}";
        public static void DragConfigure()
        {
            var icon = FeatureRefs.CraneStyleFeat.Reference.Get().Icon;

            var SetThrow = ActionsBuilder.New()
                .Add<ContextActionSetKiThrow>()
                .Build();

            var DoThrow = ActionsBuilder.New()
                .Add<ContextActionDoKiThrow>()
                .Build();

            var ReleaseThrow = ActionsBuilder.New()
                .Add<ContextActionCancelKiThrow>()
                .Build();

            var buff = BuffConfigurator.New(StyleBuff, StyleBuffGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .AddBuffActions(deactivated: ReleaseThrow)
                .Configure();

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(DoThrow)
                .SetType(AbilityType.Physical)
                .SetCanTargetEnemies(false)
                .SetCanTargetSelf(false)
                .SetCanTargetFriends(false)
                .SetCanTargetPoint(true)
                .SetRange(AbilityRange.Weapon)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .Configure();

            FeatureConfigurator.New(DragName, DragGuid, FeatureGroup.Feat)
                    .SetDisplayName(DragDisplayName)
                    .SetDescription(DragDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddManeuverTrigger(SetThrow, Kingmaker.RuleSystem.Rules.CombatManeuver.Trip, true)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToFeatureSelection(FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
                    .AddFacts(new() { ability })
                    .Configure();

            FeatureConfigurator.New(DragName2, DragGuid2, FeatureGroup.MythicAbility)
                    .SetDisplayName(DragDisplayName2)
                    .SetDescription(DragDescription2)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(DragGuid, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(FeatureRefs.KiPowerFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddManeuverTrigger(SetThrow, Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickBlind, true)
                    .AddManeuverTrigger(SetThrow, Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickEntangle, true)
                    .AddManeuverTrigger(SetThrow, Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickSickened, true)
                    .AddManeuverTrigger(SetThrow, Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, true)
                    .AddManeuverTrigger(SetThrow, Kingmaker.RuleSystem.Rules.CombatManeuver.SunderArmor, true)
                    .AddComponent<MythicKiThrowBonus>()
                    .Configure();
        }

        private static readonly string DragName2 = "MythicKiThrow";
        public static readonly string DragGuid2 = "{D2FF8435-DB27-4EE6-AC0A-3AEE7863909A}";

        private static readonly string DragDisplayName2 = "MythicKiThrow.Name";
        private static readonly string DragDescription2 = "MythicKiThrow.Description";
    }
}
