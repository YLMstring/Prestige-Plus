using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.PrestigeClass;
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
            var icon = FeatureRefs.ArcanistExploitSwiftConsumeFeature.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.LoremasterGreaterLore.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.RangedLegerdemainFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(ExaltedEvangelist.ExaltedDomainPlusGuid, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
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

        private static readonly string Feat2Name = "SecretAkashic2";
        private static readonly string Feat2Guid = "{16050278-9442-4FB1-ABDB-3AE7D5D2943A}";

        private static readonly string DisplayName2 = "MythicSecretAkashic2.Name";
        private static readonly string Description2 = "MythicSecretAkashic2.Description";

        public static void Configure2()
        {
            var icon = AbilityRefs.PhantasmalKiller.Reference.Get().Icon;

            FeatureConfigurator.New(Feat2Name, Feat2Guid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.LoremasterLore.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.RangedLegerdemainFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(ExaltedEvangelist.ExaltedDomainPlusGuid, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
                    .AddComponent<PPLearnSpell>(c =>
                    {
                        c.Spell = AbilityRefs.PhantasmalKiller.Reference;
                        c.level = 4;
                    })
                    .AddComponent<PPLearnSpell>(c =>
                    {
                        c.Spell = AbilityRefs.PhantasmalWeb.Reference;
                        c.level = 5;
                    })
                    .AddComponent<PPLearnSpell>(c =>
                    {
                        c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(PhantomLimb.PhantomLimbAbilityGuid);
                        c.level = 6;
                    })
                    .Configure();
        }
    }
}
