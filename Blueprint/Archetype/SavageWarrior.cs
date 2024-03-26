using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Feats;
using PrestigePlus.CustomComponent.Archetype;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class SavageWarrior
    {
        private const string ArchetypeName = "SavageWarrior";
        private static readonly string ArchetypeGuid = "{DCB94778-FEE8-44AD-86FA-D2ABA0EADFAD}";
        internal const string ArchetypeDisplayName = "SavageWarrior.Name";
        private const string ArchetypeDescription = "SavageWarrior.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.FighterClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(2, FeatureRefs.Bravery.ToString())
            .SetRemoveFeaturesEntry(5, FeatureSelectionRefs.WeaponTrainingSelection.ToString())
            .SetRemoveFeaturesEntry(6, FeatureRefs.Bravery.ToString())
            .SetRemoveFeaturesEntry(9, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.Bravery.ToString())
            .SetRemoveFeaturesEntry(13, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
            .SetRemoveFeaturesEntry(14, FeatureRefs.Bravery.ToString())
            .SetRemoveFeaturesEntry(17, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
            .SetRemoveFeaturesEntry(18, FeatureRefs.Bravery.ToString())
            .AddToAddFeatures(2, CreateSparkLife())
            .AddToAddFeatures(3, SparkLifeGuid)
            .AddToAddFeatures(4, SparkLifeGuid)
            .AddToAddFeatures(5, SparkLifeGuid)
            .AddToAddFeatures(6, SparkLifeGuid)
            .AddToAddFeatures(5, CreateNaturalSavagery())
            .AddToAddFeatures(6, NaturalSavageryGuid, FeatureRefs.LungeFeature.ToString())
            .AddToAddFeatures(7, NaturalSavageryGuid, CreateSavageCharge())
            .AddToAddFeatures(10, NaturalSavageryGuid, CreateGreaterCharge())
              .Configure();
        }

        private const string SparkLife = "SavageWarrior.SparkLife";
        private static readonly string SparkLifeGuid = "{C0F03096-F296-4B08-BAE3-17260D369487}";

        internal const string SparkLifeDisplayName = "SavageWarriorSparkLife.Name";
        private const string SparkLifeDescription = "SavageWarriorSparkLife.Description";
        private static BlueprintFeature CreateSparkLife()
        {
            var icon = AbilityRefs.BreathOfLifeCast.Reference.Get().Icon;

            return FeatureConfigurator.New(SparkLife, SparkLifeGuid)
              .SetDisplayName(SparkLifeDisplayName)
              .SetDescription(SparkLifeDescription)
              .SetIcon(icon)
              .SetRanks(5)
              .AddSavingThrowContextBonusAgainstDescriptor(spellDescriptor: SpellDescriptor.NegativeLevel | SpellDescriptor.Death, value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(SparkLifeGuid))
              .Configure();
        }

        private const string NaturalSavagery = "SavageWarrior.NaturalSavagery";
        private static readonly string NaturalSavageryGuid = "{E15AF543-AA47-47E9-917C-D696FDE92E94}";

        internal const string NaturalSavageryDisplayName = "SavageWarriorNaturalSavagery.Name";
        private const string NaturalSavageryDescription = "SavageWarriorNaturalSavagery.Description";
        private static BlueprintFeature CreateNaturalSavagery()
        {
            var icon = AbilityRefs.PrimalRegression.Reference.Get().Icon;
            var mans = new CombatManeuver[] { CombatManeuver.Grapple };

            return FeatureConfigurator.New(NaturalSavagery, NaturalSavageryGuid)
              .SetDisplayName(NaturalSavageryDisplayName)
              .SetDescription(NaturalSavageryDescription)
              .SetIcon(icon)
              .SetRanks(5)
              //.AddCMBBonusForManeuver(maneuvers: mans, value: ContextValues.Rank(), checkFact: false)
              .AddCMDBonusAgainstManeuvers(maneuvers: mans, value: ContextValues.Rank())
              .AddWeaponGroupAttackBonus(1, weaponGroup: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Natural)
              .AddWeaponGroupDamageBonus(damageBonus: 1, weaponGroup: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Natural)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(NaturalSavageryGuid))
              .Configure();
        }

        private const string SavageCharge = "SavageWarrior.SavageCharge";
        private static readonly string SavageChargeGuid = "{4F1B7462-0EE6-4867-8365-C41888CE411B}";

        private const string SavageChargeBuff = "SavageWarrior.SavageChargeBuff";
        private static readonly string SavageChargeGuidBuff = "{E4F07863-2A72-428E-A55D-D402304070C0}";

        internal const string SavageChargeDisplayName = "SavageWarriorSavageCharge.Name";
        private const string SavageChargeDescription = "SavageWarriorSavageCharge.Description";
        private static BlueprintFeature CreateSavageCharge()
        {
            var icon = FeatureRefs.ShifterWoodlandGraceFeature.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(SavageChargeBuff, SavageChargeGuidBuff)
              .SetDisplayName(SavageChargeDisplayName)
              .SetDescription(SavageChargeDescription)
              .SetIcon(icon)
              .AddComponent<SavageChargeComp>()
              .Configure();

            return FeatureConfigurator.New(SavageCharge, SavageChargeGuid)
              .SetDisplayName(SavageChargeDisplayName)
              .SetDescription(SavageChargeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.ChargeBuff.ToString(), extraEffectBuff: Buff1)
              .Configure();
        }

        private const string GreaterCharge = "SavageWarrior.GreaterCharge";
        public static readonly string GreaterChargeGuid = "{197896E4-1C28-4F06-94BF-2C411C0189DD}";

        internal const string GreaterChargeDisplayName = "SavageWarriorGreaterCharge.Name";
        private const string GreaterChargeDescription = "SavageWarriorGreaterCharge.Description";
        private static BlueprintFeature CreateGreaterCharge()
        {
            var icon = FeatureRefs.WildShapeIWolfFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(GreaterCharge, GreaterChargeGuid)
              .SetDisplayName(GreaterChargeDisplayName)
              .SetDescription(GreaterChargeDescription)
              .SetIcon(icon)
              .AddFacts(new() { StagStyle.StyleGuid })
              .Configure();
        }
    }
}
