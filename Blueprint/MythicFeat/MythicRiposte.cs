using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
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

            var selfbuff = BuffConfigurator.New(ExploitiveBuff, ExploitiveGuidBuff)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              //.AddContextStatBonus(StatType.AdditionalAttackBonus, ContextValues.Rank())
              .AddComponent<AddContextStatBonus>(c => { c.Stat = StatType.AdditionalAttackBonus; c.Value = ContextValues.Rank(); })
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
    }
}
