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
              .AddToAllFeatures(VitalityGoodFeat())
              .AddToAllFeatures(VitalityEvilFeat())
              .AddFacts(new() { FeatureRefs.Disruptive.ToString() })
              .Configure();
        }

        private const string VitalityGood = "Inquisition.VitalityGood";
        private static readonly string VitalityGoodGuid = "{089CB193-5638-4ADB-9B18-35023FD8ABA7}";

        private const string VitalityGoodBuff = "Inquisition.VitalityGoodBuff";
        private static readonly string VitalityGoodBuffGuid = "{C1B2FB40-8EE4-455B-AEF1-AD58A4EBF98B}";

        internal const string InquisitionVitalityGoodDisplayName = "InquisitionVitalityGood.Name";
        private const string InquisitionVitalityGoodDescription = "InquisitionVitalityGood.Description";
        public static BlueprintFeature VitalityGoodFeat()
        {
            var icon = AbilityRefs.ShieldOfDawn.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(VitalityGoodBuff, VitalityGoodBuffGuid)
              .SetDisplayName(InquisitionVitalityGoodDisplayName)
              .SetDescription(InquisitionVitalityGoodDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Sacred, false, StatType.AC, 2)
              .Configure();
            
            var action = ActionsBuilder.New()
                .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes), toCaster: true)
                .Build();

            return FeatureConfigurator.New(VitalityGood, VitalityGoodGuid)
              .SetDisplayName(InquisitionVitalityGoodDisplayName)
              .SetDescription(InquisitionVitalityGoodDescription)
              .SetIcon(icon)
              .AddComponent<AddOutgoingDamageTriggerTTT>(c => { c.Actions = action; c.TargetKilledByThisDamage = true; })
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.InquisitorClass.ToString(), CharacterClassRefs.ClericClass.ToString() }, min: 1))
              .AddPrerequisiteNoFeature(FeatureRefs.EvilDomainAllowed.ToString())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string VitalityEvil = "Inquisition.VitalityEvil";
        private static readonly string VitalityEvilGuid = "{97CE4717-8D06-4C91-AAC3-B88A6035DC8A}";

        private const string VitalityEvilBuff = "Inquisition.VitalityEvilBuff";
        private static readonly string VitalityEvilBuffGuid = "{06AFFB6A-946C-42EA-BE41-87BA13B7C1A2}";

        internal const string InquisitionVitalityEvilDisplayName = "InquisitionVitalityEvil.Name";
        private const string InquisitionVitalityEvilDescription = "InquisitionVitalityEvil.Description";
        public static BlueprintFeature VitalityEvilFeat()
        {
            var icon = AbilityRefs.VampiricShadowShield.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(VitalityEvilBuff, VitalityEvilBuffGuid)
              .SetDisplayName(InquisitionVitalityEvilDisplayName)
              .SetDescription(InquisitionVitalityEvilDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Profane, false, StatType.AC, 2)
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes), toCaster: true)
                .Build();

            return FeatureConfigurator.New(VitalityEvil, VitalityEvilGuid)
              .SetDisplayName(InquisitionVitalityEvilDisplayName)
              .SetDescription(InquisitionVitalityEvilDescription)
              .SetIcon(icon)
              .AddComponent<AddOutgoingDamageTriggerTTT>(c => { c.Actions = action; c.TargetKilledByThisDamage = true; })
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.InquisitorClass.ToString(), CharacterClassRefs.ClericClass.ToString() }, min: 1))
              .AddPrerequisiteNoFeature(FeatureRefs.GoodDomainAllowed.ToString())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }
    }
}
