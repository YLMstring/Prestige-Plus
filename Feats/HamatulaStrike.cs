using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Mechanics.Components;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
{
    internal class HamatulaStrike
    {
        private static readonly string FeatName = "HamatulaStrike";
        public static readonly string FeatGuid = "{3DB2B560-2F66-45F5-8942-14B826055F7C}";

        private static readonly string DisplayName = "HamatulaStrike.Name";
        private static readonly string Description = "HamatulaStrike.Description";

        private const string HamatulaStrikeAbility = "HamatulaStrike.HamatulaStrikeAbility";
        private static readonly string HamatulaStrikeAbilityGuid = "{0CE2F344-3440-4194-8696-AEF4A65BE50E}";

        private const string HamatulaStrikebuff = "HamatulaStrike.HamatulaStrikebuff";
        private static readonly string HamatulaStrikebuffGuid = "{2AF7906A-C641-4596-B6A7-DF1F0CDA8758}";
        public static void Configure()
        {
            var icon = FeatureRefs.PiranhaStrikeFeature.Reference.Get().Icon;

            var grapple = ActionsBuilder.New()
                .Add<HamatulaStrikeGrab>()
                .Build();

            var BuffHamatulaStrike = BuffConfigurator.New(HamatulaStrikebuff, HamatulaStrikebuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddInitiatorAttackWithWeaponTrigger(action: grapple, onlyHit: true)
              .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c => { c.Action = grapple; c.OnlyHit = true; c.CheckWeaponRangeType = true; c.RangeType = Kingmaker.Enums.WeaponRangeType.Melee; c.TriggerBeforeAttack = false;  })
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(HamatulaStrikeAbility, HamatulaStrikeAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetBuff(BuffHamatulaStrike)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Strength, 13)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 7)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddFacts(new() { abilityTrick })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
