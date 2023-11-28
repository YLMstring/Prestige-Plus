using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class MythicRiposte
    {
        private static readonly string FeatName = "MythicRiposte";
        private static readonly string FeatGuid = "{655910EE-C044-4A18-A725-84946B8B1110}";

        private static readonly string DisplayName = "MythicRiposte.Name";
        private static readonly string Description = "MythicRiposte.Description";

        private const string ExploitiveBuff = "MythicRiposte.ExploitiveBuff";
        private static readonly string ExploitiveGuidBuff = "{2682CE40-4F60-482C-909B-90C991BFEFAC}";

        public static void Configure()
        {
            var icon = FeatureRefs.DuelistParryFeature.Reference.Get().Icon;

            BuffConfigurator.New(ExploitiveBuff, ExploitiveGuidBuff)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              //.AddAttackBonusConditional(bonus: ContextValues.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
              .AddContextStatBonus(StatType.AdditionalAttackBonus, ContextValues.Rank())
              .AddCMBBonus(value: ContextValues.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
              .AddContextRankConfig(ContextRankConfigs.MythicLevel())
              .AddInitiatorAttackRollTrigger(ActionsBuilder.New().RemoveBuff(ExploitiveGuidBuff, toCaster: true).Build(), onOwner: true)
              .AddManeuverTrigger(ActionsBuilder.New().RemoveBuff(ExploitiveGuidBuff, toCaster: true).Build(), onlySuccess: false)
              .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.DuelistClass.ToString(), 2)
                    .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
                    .Configure();
        }

        private static readonly string Feat2Name = "ImprovedParry";
        public static readonly string Feat2Guid = "{0B65D7B4-CF40-467F-888D-A46966DB6DB1}";

        private static readonly string DisplayName2 = "ImprovedParry.Name";
        private static readonly string Description2 = "ImprovedParry.Description";

        private const string ExploitiveBuff2 = "ImprovedParry.ExploitiveBuff";
        public static readonly string ExploitiveGuidBuff2 = "{D5752A55-FA6A-4CB4-87AD-9BD44378DAB8}";

        public static void Configure2()
        {
            var icon = FeatureRefs.DuelistParryFeature.Reference.Get().Icon;

            BuffConfigurator.New(ExploitiveBuff2, ExploitiveGuidBuff2)
              .SetDisplayName(DisplayName2)
              .SetDescription(Description2)
              .SetIcon(icon)
              .AddComponent<ParryFlat>()
              .Configure();

            FeatureConfigurator.New(Feat2Name, Feat2Guid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Intelligence, 13)
                    .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.DuelistParryFeature.ToString())
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
