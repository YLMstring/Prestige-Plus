using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
{
    internal class MeatShield
    {
        private static readonly string FeatName = "MeatShield";
        public static readonly string FeatGuid = "{03817C1A-8432-47BC-8079-336229547CA0}";

        private static readonly string DisplayName = "MeatShield.Name";
        private static readonly string Description = "MeatShield.Description";

        private const string MeatShieldAbility = "MeatShield.MeatShieldAbility";
        private static readonly string MeatShieldAbilityGuid = "{A23ECC60-3498-4D65-B850-1F41DC7CBEA6}";

        private const string MeatShieldbuff = "MeatShield.MeatShieldbuff";
        private static readonly string MeatShieldbuffGuid = "{A1FF983E-7EB4-47DB-B7B8-B86B8E5B69DD}";

        private const string MeatShieldbuff2 = "MeatShield.MeatShieldbuff2";
        private static readonly string MeatShieldbuff2Guid = "{0F01108E-59B5-4355-82C2-E7DE1552D48A}";
        public static void Configure()
        {
            var icon = FeatureRefs.ShieldBashFeature.Reference.Get().Icon;

            var BuffMeatShield2 = BuffConfigurator.New(MeatShieldbuff2, MeatShieldbuff2Guid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var BuffMeatShield = BuffConfigurator.New(MeatShieldbuff, MeatShieldbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<MeatShieldMechanic>(c => { c.CooldownBuff = BuffMeatShield2; })
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(MeatShieldAbility, MeatShieldAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetBuff(BuffMeatShield)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddFacts(new() { abilityTrick })
                    .Configure();
        }
    }
}
