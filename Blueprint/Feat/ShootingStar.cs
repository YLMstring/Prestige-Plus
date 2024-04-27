using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;

namespace PrestigePlus.Blueprint.Feat
{
    internal class ShootingStar
    {
        private const string ShootingStarFeat = "Feat.ShootingStar";
        private static readonly string ShootingStarGuid = "{9A70B7FF-39EE-416D-8D83-240AEB0EB5D7}";

        internal const string ShootingStarDisplayName = "FeatShootingStar.Name";
        private const string ShootingStarDescription = "FeatShootingStar.Description";

        internal const string ShootingStarDisplayName2 = "FeatShootingStar2.Name";
        private const string ShootingStarDescription2 = "FeatShootingStar2.Description";

        private const string ShootingStarAblity = "Feat.UseShootingStar";
        private static readonly string ShootingStarAblityGuid = "{489E8817-5EC4-4717-AA39-DB0908D730F0}";
        public static void ShootingStarConfigure()
        {
            var icon = FeatureRefs.DesnaFeature.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(ShootingStarAblity, ShootingStarAblityGuid)
                .AllowTargeting(false, true, false, false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .SetHasFastAnimation(true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .Add<ShootingStarAttack>()
                        .Build())
                .SetDisplayName(ShootingStarDisplayName2)
                .SetDescription(ShootingStarDescription2)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Physical)
                .SetIsFullRoundAction(true)
                .AddComponent<ShootingStarRequirement>()
                .AddAbilityCasterMainWeaponCheck(WeaponCategory.Starknife)
                .Configure();

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
              .AddWeaponTypeDamageStatReplacement(WeaponCategory.Starknife, false, StatType.Charisma, true)
              .AddFacts(new() { ability })
              .AddToFeatureSelection("96f784ce-7660-40d4-9cf3-29bc289a8be5") //co+
              .Configure(delayed: true);
        }
    }
}
