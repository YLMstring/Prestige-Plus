using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class BullRushFeats
    {
        private static readonly string FeatName = "FeatQuickBullRush";
        private static readonly string FeatGuid = "{15C0B67C-9CB7-47D4-A763-0D494840C767}";

        private static readonly string DisplayName = "FeatQuickBullRush.Name";
        private static readonly string Description = "FeatQuickBullRush.Description";

        public static void Configure()
        {
            var icon = ActivatableAbilityRefs.DivinationSchoolForetellAidToggleAbility.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Strength, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedBullRush.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
                    .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 6)
                    .AddFacts(new() { ReplaceAttack.BullRushQuickAbilityGuid, ReplaceAttack.BullRushQuick2AbilityGuid })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }

        private static readonly string Feat2Name = "FeatKnockback";
        private static readonly string Feat2Guid = "{55DA148B-C5DA-42C2-9F29-30F2EEB75C9D}";

        private static readonly string DisplayName2 = "FeatKnockback.Name";
        private static readonly string Description2 = "FeatKnockback.Description";

        public static void Configure2()
        {
            var icon = ActivatableAbilityRefs.DivinationSchoolForetellAidToggleAbility.Reference.Get().Icon;

            FeatureConfigurator.New(Feat2Name, Feat2Guid, Kingmaker.Blueprints.Classes.FeatureGroup.RagePower)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddBuffExtraEffects(BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: ReplaceAttack.BullRushAngry2buffGuid)
                    .AddBuffExtraEffects(BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: ReplaceAttack.BullRushAngry2buffGuid)
                    .AddBuffExtraEffects(BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: ReplaceAttack.BullRushAngry2buffGuid)
                    .Configure();
        }
    }
}
