using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using PrestigePlus.CustomComponent.Archetype;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints.Classes.Prerequisites;

namespace PrestigePlus.Blueprint.Feat
{
    internal class Inquisition
    {
        private const string Spellkiller = "Inquisition.Spellkiller";
        private static readonly string SpellkillerGuid = "{EB9949FD-B5B5-4966-8E81-6C1F4F2E8174}";

        internal const string SpellkillerDisplayName = "InquisitionSpellkiller.Name";
        private const string SpellkillerDescription = "InquisitionSpellkiller.Description";
        public static BlueprintProgression SpellkillerFeat()
        {
            var icon = FeatureRefs.Disruptive.Reference.Get().Icon;

            return ProgressionConfigurator.New(Spellkiller, SpellkillerGuid)
              .SetDisplayName(SpellkillerDisplayName)
              .SetDescription(SpellkillerDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ProgressionRefs.AirDomainProgression.Reference.Get().m_Classes)
              .SetArchetypes(ProgressionRefs.AirDomainProgression.Reference.Get().m_Archetypes)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToGroups(FeatureGroup.Domain)
              .AddToLevelEntry(1, ChooseGoodEvilFeat())
              .Configure();
        }

        private const string ChooseGoodEvil = "Inquisition.ChooseGoodEvil";
        private static readonly string ChooseGoodEvilGuid = "{871E15F9-BEF1-4C8C-BD01-FD517A38AF41}";

        internal const string ChooseGoodEvilDisplayName = "InquisitionChooseGoodEvil.Name";
        private const string ChooseGoodEvilDescription = "InquisitionChooseGoodEvil.Description";

        public static BlueprintFeatureSelection ChooseGoodEvilFeat()
        {
            var icon = AbilityRefs.ShieldOfDawn.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ChooseGoodEvil, ChooseGoodEvilGuid)
              .SetDisplayName(ChooseGoodEvilDisplayName)
              .SetDescription(ChooseGoodEvilDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(HunterGoodFeat())
              .AddToAllFeatures(HunterEvilFeat())
              .AddFacts(new() { FeatureRefs.Disruptive.ToString() })
              .Configure();
        }

        private const string HunterGood = "Inquisition.HunterGood";
        private static readonly string HunterGoodGuid = "{089CB193-5638-4ADB-9B18-35023FD8ABA7}";

        private const string HunterGoodBuff = "Inquisition.HunterGoodBuff";
        private static readonly string HunterGoodBuffGuid = "{C1B2FB40-8EE4-455B-AEF1-AD58A4EBF98B}";

        internal const string InquisitionHunterGoodDisplayName = "InquisitionHunterGood.Name";
        private const string InquisitionHunterGoodDescription = "InquisitionHunterGood.Description";
        public static BlueprintFeature HunterGoodFeat()
        {
            var icon = AbilityRefs.ShieldOfDawn.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(HunterGoodBuff, HunterGoodBuffGuid)
              .SetDisplayName(InquisitionHunterGoodDisplayName)
              .SetDescription(InquisitionHunterGoodDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Sacred, false, StatType.AC, 2)
              .Configure();
            
            var action = ActionsBuilder.New()
                .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes), toCaster: true)
                .Build();

            return FeatureConfigurator.New(HunterGood, HunterGoodGuid)
              .SetDisplayName(InquisitionHunterGoodDisplayName)
              .SetDescription(InquisitionHunterGoodDescription)
              .SetIcon(icon)
              .AddComponent<AddOutgoingDamageTriggerTTT>(c => { c.Actions = action; c.TargetKilledByThisDamage = true; })
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.InquisitorClass.ToString(), CharacterClassRefs.ClericClass.ToString() }, min: 1))
              .AddPrerequisiteNoFeature(FeatureRefs.EvilDomainAllowed.ToString())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string HunterEvil = "Inquisition.HunterEvil";
        private static readonly string HunterEvilGuid = "{97CE4717-8D06-4C91-AAC3-B88A6035DC8A}";

        private const string HunterEvilBuff = "Inquisition.HunterEvilBuff";
        private static readonly string HunterEvilBuffGuid = "{06AFFB6A-946C-42EA-BE41-87BA13B7C1A2}";

        internal const string InquisitionHunterEvilDisplayName = "InquisitionHunterEvil.Name";
        private const string InquisitionHunterEvilDescription = "InquisitionHunterEvil.Description";
        public static BlueprintFeature HunterEvilFeat()
        {
            var icon = AbilityRefs.VampiricShadowShield.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(HunterEvilBuff, HunterEvilBuffGuid)
              .SetDisplayName(InquisitionHunterEvilDisplayName)
              .SetDescription(InquisitionHunterEvilDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Profane, false, StatType.AC, 2)
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes), toCaster: true)
                .Build();

            return FeatureConfigurator.New(HunterEvil, HunterEvilGuid)
              .SetDisplayName(InquisitionHunterEvilDisplayName)
              .SetDescription(InquisitionHunterEvilDescription)
              .SetIcon(icon)
              .AddComponent<AddOutgoingDamageTriggerTTT>(c => { c.Actions = action; c.TargetKilledByThisDamage = true; })
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.InquisitorClass.ToString(), CharacterClassRefs.ClericClass.ToString() }, min: 1))
              .AddPrerequisiteNoFeature(FeatureRefs.GoodDomainAllowed.ToString())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string Reformation = "Inquisition.Reformation";
        private static readonly string ReformationGuid = "{E0306AAC-6929-4459-811B-D2AD70878979}";

        internal const string ReformationDisplayName = "InquisitionReformation.Name";
        private const string ReformationDescription = "InquisitionReformation.Description";
        public static BlueprintProgression ReformationFeat()
        {
            var icon = FeatureRefs.FortuneRevelationFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(Reformation, ReformationGuid)
              .SetDisplayName(ReformationDisplayName)
              .SetDescription(ReformationDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ProgressionRefs.AirDomainProgression.Reference.Get().m_Classes)
              .SetArchetypes(ProgressionRefs.AirDomainProgression.Reference.Get().m_Archetypes)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToGroups(FeatureGroup.Domain)
              .AddToLevelEntry(1, InspiredRhetoricFeat())
              .AddToLevelEntry(4, BlessedCorrectionFeat())
              .Configure();
        }

        private const string BlessedCorrection = "Inquisition.BlessedCorrection";
        public static readonly string BlessedCorrectionGuid = "{39E540A1-EAB7-4B8D-BD81-851D089C4BDA}";

        internal const string BlessedCorrectionDisplayName = "InquisitionBlessedCorrection.Name";
        private const string BlessedCorrectionDescription = "InquisitionBlessedCorrection.Description";

        private const string BlessedCorrectionBuff = "Inquisition.BlessedCorrectionBuff";
        private static readonly string BlessedCorrectionBuffGuid = "{AE2ECC69-5F8E-42DB-800C-E4DD7D9DDFBF}";

        private const string BlessedCorrectionRes = "Inquisition.BlessedCorrectionRes";
        private static readonly string BlessedCorrectionResGuid = "{8BF4D53D-A59F-4C66-903E-7CC852A47F22}";

        private const string BlessedCorrectionAbility = "Inquisition.BlessedCorrectionAbility";
        private static readonly string BlessedCorrectionAbilityGuid = "{D463B68A-9CD4-4196-8A6F-B0053FB5FEA4}";
        public static BlueprintFeature BlessedCorrectionFeat()
        {
            var icon = FeatureRefs.Deceitful.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(BlessedCorrectionRes, BlessedCorrectionResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(0)
                    .IncreaseByStat(StatType.Wisdom))
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(BlessedCorrectionAbility, BlessedCorrectionAbilityGuid)
                .SetDisplayName(BlessedCorrectionDisplayName)
                .SetDescription(BlessedCorrectionDescription)
                .SetIcon(icon)
                .SetBuff(BlessedCorrectionBuffGuid)
                .SetDeactivateImmediately(true)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound)
                .Configure();

            BuffConfigurator.New(BlessedCorrectionBuff, BlessedCorrectionBuffGuid)
             .SetDisplayName(BlessedCorrectionDisplayName)
             .SetDescription(BlessedCorrectionDescription)
             .SetIcon(icon)
             .AddModifyD20(replace: false, rollsAmount: 1, takeBest: true, rule: RuleType.SkillCheck, specificSkill: true, skill: new StatType[] { StatType.CheckIntimidate, StatType.CheckDiplomacy })
             .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
             .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
             .Configure();

            return FeatureConfigurator.New(BlessedCorrection, BlessedCorrectionGuid)
              .SetDisplayName(BlessedCorrectionDisplayName)
              .SetDescription(BlessedCorrectionDescription)
              .SetIcon(icon)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string InspiredRhetoric = "Inquisition.InspiredRhetoric";
        private static readonly string InspiredRhetoricGuid = "{CC6324D1-47BB-4465-81FD-AE5EA293B9A8}";

        internal const string InquisitionInspiredRhetoricDisplayName = "InquisitionInspiredRhetoric.Name";
        private const string InquisitionInspiredRhetoricDescription = "InquisitionInspiredRhetoric.Description";
        public static BlueprintFeature InspiredRhetoricFeat()
        {
            var icon = FeatureRefs.Persuasive.Reference.Get().Icon;

            return FeatureConfigurator.New(InspiredRhetoric, InspiredRhetoricGuid)
              .SetDisplayName(InquisitionInspiredRhetoricDisplayName)
              .SetDescription(InquisitionInspiredRhetoricDescription)
              .SetIcon(icon)
              .AddReplaceStatBaseAttribute(StatType.Wisdom, true, Kingmaker.UnitLogic.Buffs.BonusMod.AsIs, StatType.CheckDiplomacy)
              .AddReplaceStatBaseAttribute(StatType.Wisdom, true, Kingmaker.UnitLogic.Buffs.BonusMod.AsIs, StatType.CheckIntimidate)
              .Configure();
        }

        private const string Tactics = "Inquisition.Tactics";
        private static readonly string TacticsGuid = "{02FD81B6-5F1D-454E-860F-A5A02A2CC13D}";

        internal const string TacticsDisplayName = "InquisitionTactics.Name";
        private const string TacticsDescription = "InquisitionTactics.Description";
        public static BlueprintProgression TacticsFeat()
        {
            var icon = FeatureRefs.TacticalLeaderFeatShareSwift.Reference.Get().Icon;

            return ProgressionConfigurator.New(Tactics, TacticsGuid)
              .SetDisplayName(TacticsDisplayName)
              .SetDescription(TacticsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ProgressionRefs.AirDomainProgression.Reference.Get().m_Classes)
              .SetArchetypes(ProgressionRefs.AirDomainProgression.Reference.Get().m_Archetypes)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToGroups(FeatureGroup.Domain)
              .AddPrerequisiteFeature(FeatureRefs.ToragFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.IroriFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.GorumFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddToLevelEntry(1, InspiredRhetoricFeat())
              .AddToLevelEntry(4, BlessedCorrectionFeat())
              .Configure();
        }
    }
}
