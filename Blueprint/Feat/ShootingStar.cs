using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;

namespace PrestigePlus.Blueprint.Feat
{
    internal class ShootingStar
    {
        private const string ShootingStarFeat = "Feat.ShootingStar";
        private static readonly string ShootingStarGuid = "{9A70B7FF-39EE-416D-8D83-240AEB0EB5D7}";

        internal const string ShootingStarDisplayName = "FeatShootingStar.Name";
        private const string ShootingStarDescription = "FeatShootingStar.Description";
        public static void ShootingStarConfigure()
        {
            var icon = FeatureRefs.DesnaFeature.Reference.Get().Icon;

            FeatureConfigurator.New(ShootingStarFeat, ShootingStarGuid)
              .SetDisplayName(ShootingStarDisplayName)
              .SetDescription(ShootingStarDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.DesnaFeature.ToString())
              .AddComponent<AttackStatReplacement>(c =>
              {
                  c.ReplacementStat = StatType.Charisma; c.SubCategory = WeaponSubCategory.Finessable; c.CheckWeaponTypes = true;
                  c.m_WeaponTypes = new BlueprintWeaponTypeReference[] { WeaponTypeRefs.Starknife.Reference.Get().ToReference<BlueprintWeaponTypeReference>() };
              })
              .AddToFeatureSelection("96f784ce-7660-40d4-9cf3-29bc289a8be5") //co+
              .Configure();
        }
    }
}
