using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.NewEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class GreaterSunderTabletop
    {
        private const string GreaterSunderTabletopFeat = "Feat.GreaterSunderTabletop";
        public static readonly string GreaterSunderTabletopGuid = "{0D975AEE-598C-4696-BA11-14E2A6182032}";
        internal const string GreaterSunderTabletopDisplayName = "FeatGreaterSunderTabletop.Name";
        private const string GreaterSunderTabletopDescription = "FeatGreaterSunderTabletop.Description";

        public static BlueprintFeature CreateGreaterSunderTabletop()
        {
            var icon = AbilityRefs.BeastShapeIII.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .ApplyBuffPermanent(SunderStorm.SunderStormBuffGuid)
                .RemoveBuff(BuffRefs.SunderArmorBuff.ToString())
                .Conditional(ConditionsBuilder.New().TargetInMeleeRange().Build(), ifTrue: ActionsBuilder.New().MeleeAttack(autoHit: true).Build())
                .Build();

            return FeatureConfigurator.New(GreaterSunderTabletopFeat, GreaterSunderTabletopGuid, FeatureGroup.MythicAbility)
              .SetDisplayName(GreaterSunderTabletopDisplayName)
              .SetDescription(GreaterSunderTabletopDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.GreaterSunder.ToString())
              .AddManeuverTrigger(shoot, Kingmaker.RuleSystem.Rules.CombatManeuver.SunderArmor, true)
              //.AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.SunderArmor }, value: ContextValues.Constant(2))
              .Configure();
        }
    }
}
