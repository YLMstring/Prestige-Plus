using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.GrappleFeat
{
    internal class SavageSlam
    {
        private static readonly string FeatName = "SavageSlam";
        public static readonly string FeatGuid = "{B7EFC5BC-49DD-4822-8813-402071BB17B8}";

        private static readonly string DisplayName = "SavageSlam.Name";
        private static readonly string Description = "SavageSlam.Description";

        private const string SavageSlamAbility = "SavageSlam.SavageSlamAbility";
        private static readonly string SavageSlamAbilityGuid = "{DEB23624-AB4A-46D9-848B-8917501833F9}";

        private const string SavageSlambuff = "SavageSlam.SavageSlambuff";
        private static readonly string SavageSlambuffGuid = "{0A0ABEDD-D762-4148-8948-272FFCDC336D}";
        public static void Configure()
        {
            var icon = FeatureRefs.ImprovedTrip.Reference.Get().Icon;

            var BuffSavageSlam = BuffConfigurator.New(SavageSlambuff, SavageSlambuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(SavageSlamAbility, SavageSlamAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetBuff(BuffSavageSlam)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 3, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 1, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddFacts(new() { abilityTrick })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
