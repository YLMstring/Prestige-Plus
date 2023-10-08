using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class ImprovedKiThrow
    {
        private static readonly string DragName = "ImprovedKiThrow";
        public static readonly string DragGuid = "{EDEDDD81-1913-4E57-96A2-B07694DADCFA}";

        private static readonly string DragDisplayName = "ImprovedKiThrow.Name";
        private static readonly string DragDescription = "ImprovedKiThrow.Description";

        private const string StyleAbility = "ImprovedKiThrow.StyleAbility";
        private static readonly string StyleAbilityGuid = "{E05343FB-0131-496C-8174-D1E811F30652}";

        public static void DragConfigure()
        {
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            var DoThrow = ActionsBuilder.New()
                .Add<ContextActionDoKiThrow>()
                .Build();

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(DoThrow)
                .SetType(AbilityType.Physical)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetCanTargetFriends(false)
                .SetCanTargetPoint(false)
                .SetRange(AbilityRange.Weapon)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .Configure();

            FeatureConfigurator.New(DragName, DragGuid, FeatureGroup.Feat)
                    .SetDisplayName(DragDisplayName)
                    .SetDescription(DragDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedBullRush.ToString())
                    .AddPrerequisiteFeature(KiThrow.DragGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToFeatureSelection(FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
