using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints.Classes;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.BasicEx;
using PrestigePlus.CustomAction.OtherManeuver;
using Kingmaker.Blueprints.Root;

namespace PrestigePlus.Blueprint.SpecificManeuver
{
    internal class DirtyFeats
    {
        private static readonly string FeatName = "FeatQuickDirtyTrick";
        private static readonly string FeatGuid = "{75E92D69-1A88-4B7A-A679-5FAF954CE690}";

        private static readonly string DisplayName = "FeatQuickDirtyTrick.Name";
        private static readonly string Description = "FeatQuickDirtyTrick.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.OracleRevelationManeuverMasteryDirtyTrick.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Intelligence, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedDirtyTrick.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                    .AddFacts(new() { ReplaceAttack.DirtyBlindQuick2AbilityGuid, ReplaceAttack.DirtyEntangleQuick2AbilityGuid, ReplaceAttack.DirtySickenQuick2AbilityGuid })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .Configure();
        }

        private static readonly string StyleName = "DirtyTrickMaster";
        public static readonly string StyleGuid = "{031EA33B-689C-4200-A160-3F375D220FEE}";

        private static readonly string StyleDisplayName = "DirtyTrickMaster.Name";
        private static readonly string StyleDescription = "DirtyTrickMaster.Description";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                    .Add<DirtyTrickMaster>(c => { c.buffold = BlueprintRoot.Instance.SystemMechanics.DirtyTrickEntangledBuff; c.buffnew = BuffRefs.Daze.Reference.Get(); })
                    .Build();

            var action2 = ActionsBuilder.New()
                    .Add<DirtyTrickMaster>(c => { c.buffold = BlueprintRoot.Instance.SystemMechanics.DirtyTrickSickenedBuff; c.buffnew = BuffRefs.Nauseated.Reference.Get(); })
                    .Build();

            FeatureConfigurator.New(StyleName, StyleGuid, FeatureGroup.Feat)
                    .SetDisplayName(StyleDisplayName)
                    .SetDescription(StyleDescription)
                    .SetIcon(icon)
                    .AddManeuverTrigger(action, Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickEntangle, true)
                    .AddManeuverTrigger(action2, Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickSickened, true)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 11)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedDirtyTrick.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.GreaterDirtyTrick.ToString())
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .Configure();
        }

        private static readonly string LadderName = "LadderLock";
        public static readonly string LadderGuid = "{DAAF3D32-A026-417C-B5D1-D7DED42160DC}";

        private static readonly string LadderDisplayName = "LadderLock.Name";
        private static readonly string LadderDescription = "LadderLock.Description";
        public static void LadderConfigure()
        {
            //var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            FeatureConfigurator.New(LadderName, LadderGuid, FeatureGroup.Feat)
                    .SetDisplayName(LadderDisplayName)
                    .SetDescription(LadderDescription)
                    //.SetIcon(icon)
                    .AddComponent<LadderLock>()
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedDirtyTrick.ToString())
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
