using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using PrestigePlus.CustomComponent.BasePrestigeEnhance;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class MetaphysicalSneak
    {
        private static readonly string FeatName = "MetaphysicalSneak";
        private static readonly string FeatGuid = "{DBE92411-1961-40B9-B069-B7C0F8BA58DA}";

        private static readonly string DisplayName = "MetaphysicalSneak.Name";
        private static readonly string Description = "MetaphysicalSneak.Description";

        public static void Configure()
        {
            var icon = ActivatableAbilityRefs.ImpromptuSneakAttackToggleAbility.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.StudentOfWarClass.ToString(), 1)
                    .AddPrerequisiteFeature(FeatureRefs.SneakAttack.ToString())
                    .AddComponent<MetaphysicalSneakFix>()
                    .Configure();
        }
    }
}
