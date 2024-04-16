using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.Spell;
using PrestigePlus.CustomComponent.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.MythicFeat
{
    internal class SecretAkashic
    {
        private static readonly string FeatName = "SecretAkashic";
        private static readonly string FeatGuid = "{9B8CA340-96B2-4465-BC66-595279EB8382}";

        private static readonly string DisplayName = "MythicSecretAkashic.Name";
        private static readonly string Description = "MythicSecretAkashic.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.TrueSeeing.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(ProgressionRefs.KineticBlastProgression.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.LoremasterGreaterLore.ToString())
                    .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
                    .AddComponent<PPLearnSpell>(c =>
                    {
                        c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(GlimpseAkashic.GlimpseAkashicAbilityGuid);
                        c.level = 8;
                    })
                    .AddComponent<PPLearnSpell>(c =>
                    {
                        c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(AkashicForm.AkashicFormAbilityGuid);
                        c.level = 9;
                    })
                    .Configure();
        }
    }
}
