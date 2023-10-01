using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class MythicWings
    {
        private static readonly string FeatName = "MythicWings";
        private static readonly string FeatGuid = "{E107DC01-E07A-4902-B23B-248BBE5E7F10}";

        private static readonly string DisplayName = "MythicWings.Name";
        private static readonly string Description = "MythicWings.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.Fear.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddAdditionalLimb(ItemWeaponRefs.Wing1d4.ToString())
                    .AddAdditionalLimb(ItemWeaponRefs.Wing1d4.ToString())
                    .AddPrerequisiteClassLevel(CharacterClassRefs.DragonDiscipleClass.ToString(), 5, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteArchetypeLevel(ArchetypeRefs.DragonheirScionArchetype.ToString(), level: 10, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any, characterClass: CharacterClassRefs.FighterClass.ToString())
                    .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
                    .Configure();
        }
    }
}
