using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using PrestigePlus.CustomComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class DawnflowerDervish
    {
        private const string ArchetypeName = "DawnflowerDervish";
        private static readonly string ArchetypeGuid = "{86B44107-9338-4D51-8138-D00396F3CE82}";
        internal const string ArchetypeDisplayName = "DawnflowerDervish.Name";
        private const string ArchetypeDescription = "DawnflowerDervish.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.FighterClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(3, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(7, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(11, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(15, FeatureRefs.ArmorTraining.ToString())
            .AddToAddFeatures(3, CreateBurst())
            .AddToAddFeatures(7, BurstGuid, FeatureRefs.HunterWoodlandStride.ToString())
            .AddToAddFeatures(11, RapidAttackFeat())
            .AddToAddFeatures(15, LightningFeat())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.FighterProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { LightningGuid, BurstGuid, RapidAttackGuid })
                .Configure();
        }

        private const string Burst = "DawnflowerDervish.Burst";
        private static readonly string BurstGuid = "{2B13A7D3-2DA9-4B6E-9373-5F343782FF62}";

        private const string BurstBuff = "DawnflowerDervish.BurstBuff";
        private static readonly string BurstGuidBuff = "{BFA256EB-4531-4E76-92D8-9696461D93E8}";

        internal const string BurstDisplayName = "DawnflowerDervishBurst.Name";
        private const string BurstDescription = "DawnflowerDervishBurst.Description";
        private static BlueprintFeature CreateBurst()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(BurstBuff, BurstGuidBuff)
              .SetDisplayName(BurstDisplayName)
              .SetDescription(BurstDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddContextStatBonus(StatType.AC, value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(BurstGuid))
              .Configure();

            return FeatureConfigurator.New(Burst, BurstGuid)
              .SetDisplayName(BurstDisplayName)
              .SetDescription(BurstDescription)
              .SetIcon(icon)
              .SetRanks(2)
              .SetIsClassFeature(true)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.ChargeBuff.ToString(), extraEffectBuff: Buff1)
              .Configure();
        }

        private const string RapidAttack = "DawnflowerDervish.RapidAttack";
        public static readonly string RapidAttackGuid = "{DA485182-E9BB-4089-9246-F8A106BFEBF8}";

        internal const string RapidAttackDisplayName = "DawnflowerDervishRapidAttack.Name";
        private const string RapidAttackDescription = "DawnflowerDervishRapidAttack.Description";

        private const string RapidAttackBuff = "DawnflowerDervish.RapidAttackBuff";
        public static readonly string RapidAttackBuffGuid = "{8E6B3FA6-3BEA-4619-AEEF-93B5CC662FDF}";

        private const string RapidAttackAbility = "DawnflowerDervish.RapidAttackAbility";
        private static readonly string RapidAttackAbilityGuid = "{B5286362-8AF2-42C1-A0C0-678DEC7B1A5E}";
        public static BlueprintFeature RapidAttackFeat()
        {
            var icon = FeatureRefs.AmuletOfQuickDrawFeature.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(RapidAttackBuff, RapidAttackBuffGuid)
              .SetDisplayName(RapidAttackDisplayName)
              .SetDescription(RapidAttackDescription)
              .SetIcon(icon)
              .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.Pounce)
              .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.BaseAttackBonus, value: -5)
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(RapidAttackAbility, RapidAttackAbilityGuid)
                .SetDisplayName(RapidAttackDisplayName)
                .SetDescription(RapidAttackDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateImmediately(true)
                .Configure();

            return FeatureConfigurator.New(RapidAttack, RapidAttackGuid)
              .SetDisplayName(RapidAttackDisplayName)
              .SetDescription(RapidAttackDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Lightning = "DawnflowerDervish.Lightning";
        public static readonly string LightningGuid = "{7DA757DD-1FFC-465D-825C-FC9F09291B9E}";

        internal const string LightningDisplayName = "DawnflowerDervishLightning.Name";
        private const string LightningDescription = "DawnflowerDervishLightning.Description";

        private const string LightningBuff = "DawnflowerDervish.LightningBuff";
        private static readonly string LightningBuffGuid = "{227CF740-4C0A-40CA-B515-DE35F22048AD}";

        private const string LightningAbility = "DawnflowerDervish.LightningAbility";
        private static readonly string LightningAbilityGuid = "{17DFD8F3-BAE5-47E0-92BA-92AFCC515296}";
        public static BlueprintFeature LightningFeat()
        {
            var icon = AbilityRefs.CallLightningStorm.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(LightningBuff, LightningBuffGuid)
              .SetDisplayName(LightningDisplayName)
              .SetDescription(LightningDescription)
              .SetIcon(icon)
              .AddBuffExtraAttack(false, number: 1)
              .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.AdditionalAttackBonus, value: -2)
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(LightningAbility, LightningAbilityGuid)
                .SetDisplayName(LightningDisplayName)
                .SetDescription(LightningDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetIsOnByDefault(true)
                .SetDeactivateImmediately(true)
                .Configure();

            return FeatureConfigurator.New(Lightning, LightningGuid)
              .SetDisplayName(LightningDisplayName)
              .SetDescription(LightningDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
