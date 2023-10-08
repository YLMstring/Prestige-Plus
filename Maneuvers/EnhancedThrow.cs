using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class EnhancedThrow
    {
        private static readonly string DragName = "EnhancedThrow";
        public static readonly string DragGuid = "{C5A057E2-CAA1-4AD5-9C63-B4BBD57CCC49}";

        private static readonly string DragDisplayName = "EnhancedThrow.Name";
        private static readonly string DragDescription = "EnhancedThrow.Description";

        private const string StyleAbility = "EnhancedThrow.StyleAbility";
        private static readonly string StyleAbilityGuid = "{AAF910E4-BB24-4134-983E-7F14CB2CDCBC}";

        private const string StyleBuff = "EnhancedThrow.StyleBuff";
        private static readonly string StyleBuffGuid = "{36DD4519-6A34-4A90-85A5-D77A2309A20D}";
        public static void DragConfigure()
        {
            var icon = AbilityRefs.KiExtraAttack.Reference.Get().Icon;

            var buff = BuffConfigurator.New(StyleBuff, StyleBuffGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .Configure();

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: AbilityResourceRefs.KiPowerResource.ToString())
                .Configure();

            FeatureConfigurator.New(DragName, DragGuid, FeatureGroup.Feat)
                    .SetDisplayName(DragDisplayName)
                    .SetDescription(DragDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.KiPowerFeature.ToString())
                    .AddPrerequisiteFeature(KiThrow.DragGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
