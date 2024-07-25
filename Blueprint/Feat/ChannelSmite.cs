using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.CustomComponent;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class ChannelSmite
    {
        private static readonly string BasicSmiteFeatName = "ChannelSmiteBasicSmite";
        private static readonly string BasicSmiteFeatGuid = "{9094446C-FBE1-4999-9C31-054BFF066E6B}";

        private static readonly string BetterSmiteFeatName = "ChannelSmiteBetterSmite";
        private static readonly string BetterSmiteFeatGuid = "{8DA9053F-C91D-4D8E-8728-8634ADBF3A10}";

        private static readonly string MythicSmiteFeatName = "ChannelSmiteMythicSmite";
        private static readonly string MythicSmiteFeatGuid = "{B842F8FC-4E23-4903-BA1C-BE82FD828FCA}";

        private static readonly string BasicSmiteDisplayName = "ChannelSmiteBasicSmite.Name";
        private static readonly string BasicSmiteDescription = "ChannelSmiteBasicSmite.Description";

        private static readonly string BetterSmiteDisplayName = "ChannelSmiteBetterSmite.Name";
        private static readonly string BetterSmiteDescription = "ChannelSmiteBetterSmite.Description";

        private static readonly string MythicSmiteDisplayName = "ChannelSmiteMythicSmite.Name";
        private static readonly string MythicSmiteDescription = "ChannelSmiteMythicSmite.Description";

        private const string BasicSmitebuff = "ChannelSmiteTrick.BasicSmitebuff";
        public static readonly string BasicSmitebuffGuid = "{5326CA1E-7D40-4F88-8B83-67C48597B0F8}";

        private const string BasicSmitebuffreal = "ChannelSmiteTrick.BasicSmitebuffreal";
        public static readonly string BasicSmitebuffrealGuid = "{4D2A7AFE-383C-48E2-A749-954044D3C6A9}";

        private const string BasicSmitebuffcool = "ChannelSmiteTrick.BasicSmitebuffcool";
        public static readonly string BasicSmitebuffcoolGuid = "{0C294131-11F0-4B9A-80CB-36578B83F301}";

        private const string BasicSmiteActivatableAbility = "ChannelSmiteTrick.BasicSmiteActivatableAbility";
        private static readonly string BasicSmiteActivatableAbilityGuid = "{DE42E7CB-DE48-4FCC-9EF7-B9EC2939716E}";

        public static void ConfigureBasicSmite()
        {
            var icon = AbilityRefs.ChaosHammer.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(BasicSmitebuff, BasicSmitebuffGuid)
              .SetDisplayName(BasicSmiteDisplayName)
              .SetDescription(BasicSmiteDescription)
              .SetIcon(icon)
              
              
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var Buffreal = BuffConfigurator.New(BasicSmitebuffreal, BasicSmitebuffrealGuid)
              .SetDisplayName(BasicSmiteDisplayName)
              .SetDescription(BasicSmiteDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var Buffcool = BuffConfigurator.New(BasicSmitebuffcool, BasicSmitebuffcoolGuid)
              .SetDisplayName(BasicSmiteDisplayName)
              .SetDescription(BasicSmiteDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(BasicSmiteActivatableAbility, BasicSmiteActivatableAbilityGuid)
                .SetDisplayName(BasicSmiteDisplayName)
                .SetDescription(BasicSmiteDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .Configure();

            FeatureConfigurator.New(BasicSmiteFeatName, BasicSmiteFeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(BasicSmiteDisplayName)
                    .SetDescription(BasicSmiteDescription)
                    .SetIcon(icon)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddFacts([ability])
                    .AddComponent<PrerequisiteHasChannel>()
                    .Configure();

            FeatureConfigurator.New(MythicSmiteFeatName, MythicSmiteFeatGuid, FeatureGroup.MythicFeat)
                    .SetDisplayName(MythicSmiteDisplayName)
                    .SetDescription(MythicSmiteDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(BasicSmiteFeatGuid)
                    .Configure();

            FeatureConfigurator.New(BetterSmiteFeatName, BetterSmiteFeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(BetterSmiteDisplayName)
                    .SetDescription(BetterSmiteDescription)
                    .SetIcon(icon)
                    .AddComponent<PrerequisiteHasChannel>()
                    .AddPrerequisiteFeature(BasicSmiteFeatGuid)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 8)
                    .Configure();
        }
    }
}
