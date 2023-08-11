using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.Configurators;
using Kingmaker.Blueprints.Items.Armors;

namespace PrestigePlus.PrestigeClasses
{
    internal class ArcaneArcher
    {
        private const string ArchetypeName = "ArcaneArcher";
        private static readonly string ArchetypeGuid = "{F6E9E165-5187-483B-8EDB-810E47876A4E}";
        internal const string ArchetypeDisplayName = "ArcaneArcher.Name";
        private const string ArchetypeDescription = "ArcaneArcher.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "ArcaneArcherPrestige";
        private static readonly string ClassProgressGuid = "{B3D3D512-F9A0-41E7-99EF-8898898C67BF}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, CreateProficiencies())
                .AddToLevelEntry(2)
                .AddToLevelEntry(3)
                .AddToLevelEntry(4)
                .AddToLevelEntry(5)
                .AddToLevelEntry(6)
                .AddToLevelEntry(7)
                .AddToLevelEntry(8)
                .AddToLevelEntry(9)
                .AddToLevelEntry(10)
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.Evasion.Reference.Get().ToReference<BlueprintFeatureBaseReference>(), FeatureRefs.ImprovedEvasion.Reference.Get().ToReference<BlueprintFeatureBaseReference>() })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.UncannyDodge.Reference.Get().ToReference<BlueprintFeatureBaseReference>(), FeatureRefs.ImprovedUncannyDodge.Reference.Get().ToReference<BlueprintFeatureBaseReference>() }))
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName(ArchetypeDisplayName)
                .SetDescription(ArchetypeDescription)
                .Configure();
            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(4)
                .SetHitDie(DiceType.D10)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABFull)
                .SetFortitudeSave(SavesPrestigeHigh)
                .SetReflexSave(SavesPrestigeHigh)
                .SetWillSave(SavesPrestigeLow)
                .SetProgression(progression)
                .SetIsArcaneCaster(true)
                .SetIsDivineCaster(false)
                .AddSkipLevelsForSpellProgression(new int[] { 5, 9 })
                .SetClassSkills(new StatType[] { StatType.SkillStealth, StatType.SkillLoreNature, StatType.SkillMobility, StatType.SkillPerception })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                .AddPrerequisiteCasterTypeSpellLevel(true, false, 1)
                .AddPrerequisiteFeature(FeatureRefs.PointBlankShot.ToString())
                .AddPrerequisiteFeature(FeatureRefs.PreciseShot.ToString())
                .AddPrerequisiteFeature(FeatureRefs.WeaponFocusLongbow.ToString())
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var ArcaneArcherref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = ArcaneArcherref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }
        private const string Proficiencies = "ArcaneArcher.Proficiencies";
        private static readonly string ProficienciesGuid = "{CAFEF397-8F87-47EB-AD7C-D102D03871A1}";
        internal const string ProficienciesDisplayName = "ArcaneArcherProficiencies.Name";
        private const string ProficienciesDescription = "ArcaneArcherProficiencies.Description";

        private const string ShadowJump = "ArcaneArcher.ShadowJump";
        private static readonly string ShadowJumpGuid = "5565506D-04DE-47EC-8545-C19ABCEA40B2";
        internal const string ShadowJumpDisplayName = "ArcaneArcherShadowJump.Name";
        private const string ShadowJumpDescription = "ArcaneArcherShadowJump.Description";

        private const string ShadowJumpAblity = "ArcaneArcher.UseShadowJump";
        private static readonly string ShadowJumpAblityGuid = "E1A86DF3-38E6-4E78-82B9-4EF7A6436BA8";

        private const string ShadowJumpAblity2 = "ArcaneArcher.UseShadowJump2";
        private static readonly string ShadowJumpAblityGuid2 = "{8ED8B86D-A5DB-46FE-B88A-05098198041A}";

        private const string ShadowJumpAblityRes = "ArcaneArcher.ShadowJumpRes";
        private static readonly string ShadowJumpAblityResGuid = "E96F5710-5344-495F-A3C7-A4859C1ABF81";

        private static BlueprintFeature CreateProficiencies()
        {
            var rangerProficiencies = FeatureRefs.RangerProficiencies.Reference.Get();
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddComponent(rangerProficiencies.GetComponent<AddFacts>())
              .AddProficiencies(armorProficiencies: new ArmorProficiencyGroup[] { ArmorProficiencyGroup.TowerShield })
              .Configure();
        }

    }
}
