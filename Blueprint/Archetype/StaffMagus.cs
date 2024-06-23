using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints.Items.Armors;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomAction.ClassRelated;
using PrestigePlus.CustomComponent.Archetype;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Items;
using Kingmaker.View.Equipment;
using Kingmaker.RuleSystem;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class StaffMagus
    {
        private const string ArchetypeName = "StaffMagus";
        private static readonly string ArchetypeGuid = "{7CBE1B26-7402-4E78-8160-0439879252A1}";
        internal const string ArchetypeDisplayName = "StaffMagus.Name";
        private const string ArchetypeDescription = "StaffMagus.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.MagusClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.MagusProficiencies.ToString())
            .SetRemoveFeaturesEntry(7, FeatureRefs.ArcaneMediumArmor.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.FighterTraining.ToString())
            .SetRemoveFeaturesEntry(13, FeatureRefs.ArcaneHeavyArmor.ToString())
            .AddToAddFeatures(1, CreateProficiencies(), QuarterstaffMasterGuid)
            .AddToAddFeatures(7, CreateQuarterstaffDefense())
            .AddToAddFeatures(10, QuarterstaffSpecializationFeat())
            .AddToAddFeatures(13, QuarterstaffDefenseGuid)
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.MagusProgression)
                .AddToUIGroups([QuarterstaffSpecializationGuid, QuarterstaffMasterGuid, QuarterstaffDefenseGuid])
                .Configure();
        }

        private const string Proficiencies = "StaffMagus.Proficiencies";
        private static readonly string ProficienciesGuid = "{85ADAF7A-26CA-4332-96A4-28B28244363E}";
        internal const string ProficienciesDisplayName = "StaffMagusProficiencies.Name";
        private const string ProficienciesDescription = "StaffMagusProficiencies.Description";

        public static BlueprintFeature CreateProficiencies()
        {
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddArcaneArmorProficiency([ArmorProficiencyGroup.Light, ArmorProficiencyGroup.Medium, ArmorProficiencyGroup.Heavy])
              .AddFacts([FeatureRefs.LightArmorProficiency.ToString(), FeatureRefs.SimpleWeaponProficiency.ToString()])
              .Configure();
        }

        private const string QuarterstaffSpecialization = "StaffMagus.QuarterstaffSpecialization";
        public static readonly string QuarterstaffSpecializationGuid = "{F5843886-C236-455C-A608-69C07298BB53}";

        internal const string QuarterstaffSpecializationDisplayName = "StaffMagusQuarterstaffSpecialization.Name";
        private const string QuarterstaffSpecializationDescription = "StaffMagusQuarterstaffSpecialization.Description";

        public static BlueprintFeature QuarterstaffSpecializationFeat()
        {
            var icon = ParametrizedFeatureRefs.WeaponSpecialization.Reference.Get().Icon;

            var focus = ParametrizedFeatureRefs.WeaponSpecialization.Reference.Get().ToReference<BlueprintParametrizedFeatureReference>();
            var kama = new AddParametrizedFeatures.FeatureData()
            {
                m_Feature = focus,
                ParamWeaponCategory = WeaponCategory.Quarterstaff,
            };

            return FeatureConfigurator.New(QuarterstaffSpecialization, QuarterstaffSpecializationGuid, FeatureGroup.Feat)
              .SetDisplayName(QuarterstaffSpecializationDisplayName)
              .SetDescription(QuarterstaffSpecializationDescription)
              .SetIcon(icon)
              .AddPrerequisiteParametrizedWeaponFeature(ParametrizedFeatureRefs.WeaponFocus.ToString(), WeaponCategory.Quarterstaff)
              .AddPrerequisiteFeature(CreateQuarterstaffMaster())
              .AddParametrizedFeatures([kama])
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string QuarterstaffMaster = "StaffMagus.QuarterstaffMaster";
        public static readonly string QuarterstaffMasterGuid = "{F8AE1B9E-AE96-484F-B905-8205C8301C73}";

        internal const string QuarterstaffMasterDisplayName = "StaffMagusQuarterstaffMaster.Name";
        private const string QuarterstaffMasterDescription = "StaffMagusQuarterstaffMaster.Description";

        private static BlueprintFeature CreateQuarterstaffMaster()
        {
            var icon = FeatureRefs.Evasion.Reference.Get().Icon;

            return FeatureConfigurator.New(QuarterstaffMaster, QuarterstaffMasterGuid, FeatureGroup.Feat)
              .SetDisplayName(QuarterstaffMasterDisplayName)
              .SetDescription(QuarterstaffMasterDescription)
              .SetIcon(icon)
              .AddPrerequisiteParametrizedWeaponFeature(ParametrizedFeatureRefs.WeaponFocus.ToString(), WeaponCategory.Quarterstaff)
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string QuarterstaffDefense = "StaffMagus.QuarterstaffDefense";
        public static readonly string QuarterstaffDefenseGuid = "{E515DF3C-B535-4F72-8DA8-ED325CA127D8}";

        internal const string QuarterstaffDefenseDisplayName = "StaffMagusQuarterstaffDefense.Name";
        private const string QuarterstaffDefenseDescription = "StaffMagusQuarterstaffDefense.Description";

        public static BlueprintFeature CreateQuarterstaffDefense()
        {
            var icon = FeatureRefs.DefensiveCombatTraining.Reference.Get().Icon;

            return FeatureConfigurator.New(QuarterstaffDefense, QuarterstaffDefenseGuid)
              .SetDisplayName(QuarterstaffDefenseDisplayName)
              .SetDescription(QuarterstaffDefenseDescription)
              .SetIcon(icon)
              .SetRanks(2)
              .AddComponent<QuarterstaffDefenseComp>()
              .Configure();
        }

        internal class QuarterstaffDefenseComp : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber
        {
            void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
            {
                int ac = 0;
                foreach (var slot in Owner.Body.EquipmentSlots)
                {
                    if (slot.Active && slot.HasItem && slot.Item is ItemEntityWeapon weapon && weapon.Blueprint.Category == WeaponCategory.Quarterstaff)
                    {
                        var rule = Rulebook.Trigger(new RuleCalculateAttackBonusWithoutTarget(Owner, weapon, 0));
                        if (rule.m_ModifiableBonus?.Modifiers?.Count() > 0)
                        {
                            foreach (var mod in rule.m_ModifiableBonus.Modifiers)
                            {
                                if (mod.Descriptor == ModifierDescriptor.Enhancement)
                                {
                                    ac = Math.Max(ac, mod.Value);
                                }
                            }
                        }
                    }
                }
                if (Fact.GetRank() > 1)
                {
                    ac += 3;
                }
                if (ac > 0)
                {
                    evt.AddModifier(ac, base.Fact, ModifierDescriptor.Shield);
                }
            }

            void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
            {

            }
        }
    }
}

