using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class ImprovedWalk
    {
        public static void Patch()
        {
            BuffConfigurator.For(BuffRefs.WalkThroughSpaceBuff)
                .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.GetUpWithoutAttackOfOpportunity)
                .Configure();

            Configure();

            //var feat = BlueprintTool.GetRef<BlueprintFeatureReference>("5110905a-4911-4916-a39c-9cda7a67eca0")?.Get();
            FeatureConfigurator.For("5110905a-4911-4916-a39c-9cda7a67eca0")
                .AddFacts(new() { FeatGuid })
                .AddPrerequisiteFeature(ShadowDancer.ShadowJumpGuidEx, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(HorizonWalker.DominanceAstralGuid, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(MenhirSavant.WalkLinesGuid, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();
        }

        private static readonly string FeatName = "FeatWalkThroughSpace";
        public static readonly string FeatGuid = "{72253385-5EDA-4EF0-A572-392FA8C62785}";

        private static readonly string BuffName = "FeatWalkThroughSpace";
        public static readonly string BuffGuid = "{A61EECF3-9FE5-4C07-9331-4857FE0D222D}";

        private static readonly string DisplayName = "FeatWalkThroughSpace.Name";
        private static readonly string Description = "FeatWalkThroughSpace.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.WalkThroughSpace.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(BuffName, BuffGuid)
             .SetDisplayName(DisplayName)
             .SetDescription(Description)
             .SetIcon(icon)
             .AddFacts(new() { AbilityRefs.DimensionDoorBase.ToString() })
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddBuffExtraEffects(BuffRefs.WalkThroughSpaceBuff.ToString(), extraEffectBuff: Buff1)
                    .Configure();
        }
    }
}
