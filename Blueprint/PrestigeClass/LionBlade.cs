using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Blueprint.RogueTalent;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Actions.Builder.ContextEx;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class LionBlade
    {
        private const string ArchetypeName = "LionBlade";
        public static readonly string ArchetypeGuid = "{F89F7611-EC5E-4829-9BC1-1F98D5B558F2}";
        internal const string ArchetypeDisplayName = "LionBlade.Name";
        private const string ArchetypeDescription = "LionBlade.Description";

        private const string ClassProgressName = "LionBladePrestige";
        private static readonly string ClassProgressGuid = "{C68888D6-4451-4860-BA90-06F576013670}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, InspirePoiseFeature(), FeatureRefs.Mobility.ToString())
                .AddToLevelEntry(2, FeatureRefs.SneakAttack.ToString())
                .AddToLevelEntry(3, FeatureRefs.AssassinHideInPlainSight.ToString())
                .AddToLevelEntry(4, FeatureRefs.FastMovement.ToString())
                .AddToLevelEntry(5, InspirePoiseGuid)
                .AddToLevelEntry(6, FeatureRefs.SneakAttack.ToString())
                .AddToLevelEntry(7, CloudMindFeature(), FeatureRefs.HunterWoodlandStride.ToString())
                .AddToLevelEntry(8)
                .AddToLevelEntry(9, InspirePoiseGuid)
                .AddToLevelEntry(10, SilentSoulFeature(), FeatureRefs.SneakAttack.ToString())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { CloudMindGuid }))
                ///.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SeekerArrowGuid, PhaseArrowGuid, HailArrowGuid, DeathArrowGuid }))
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();
            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(6)
                .SetHitDie(DiceType.D8)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(StatProgressionRefs.BABMedium.ToString())
                .SetFortitudeSave(StatProgressionRefs.SavesPrestigeLow.ToString())
                .SetReflexSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetWillSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillThievery, StatType.SkillStealth, StatType.SkillKnowledgeWorld, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteStatValue(StatType.SneakAttack, 2)
                .AddPrerequisiteFeaturesFromList(new() { FeatureRefs.InspireCompetenceFeature.ToString(), FeatureRefs.SenseiInspireCompetenceFeature.ToString(), CloyingShades.CloyingShadesGuid }, 1)
                .AddPrerequisiteStatValue(StatType.SkillStealth, 5)
                .AddPrerequisiteStatValue(StatType.SkillPersuasion, 3)
                .AddPrerequisiteStatValue(StatType.SkillUseMagicDevice, 3)
                .AddPrerequisiteFeaturesFromList(new() { FeatureRefs.Deceitful.ToString(), FeatureRefs.SkillFocusThievery.ToString(), FeatureRefs.SkillFocusDiplomacy.ToString() }, 1)
                .AddPrerequisiteFeature(FeatureRefs.Improved_Initiative.ToString())
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var LionBladeref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = LionBladeref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string FuryTraining = "FuryTraining";
        private static readonly string FuryTrainingGuid = "{F5E6DA2E-6C88-46D8-8493-379097D80EA2}";

        internal const string FuryTrainingDisplayName = "FuryTraining.Name";
        private const string FuryTrainingDescription = "FuryTraining.Description";
        public static BlueprintFeature FuryTrainingFeature()
        {
            var icon = FeatureRefs.NatureSense.Reference.Get().Icon;
            return FeatureConfigurator.New(FuryTraining, FuryTrainingGuid)
              .SetDisplayName(FuryTrainingDisplayName)
              .SetDescription(FuryTrainingDescription)
              .SetIcon(icon)
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.KiPowerResource.ToString(), ContextValues.Rank(AbilityRankType.DamageDice))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.RageResourse.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.BloodragerRageResource.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.FocusedRageResourse.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              //.AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.DemonRageResource.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.RageshaperShifterResource.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.BardicPerformanceResource.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }, type: AbilityRankType.DamageDice).WithOnePlusDiv2Progression())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }, type: AbilityRankType.ProjectilesCount).WithBonusValueProgression(0, true))
              .Configure();
        }

        private const string InspirePoise = "InspirePoise";
        private static readonly string InspirePoiseGuid = "{37B8ABCD-4064-4C40-A26D-F4809B8BDB40}";

        internal const string InspirePoiseDisplayName = "InspirePoise.Name";
        private const string InspirePoiseDescription = "InspirePoise.Description";
        public static BlueprintFeature InspirePoiseFeature()
        {
            var icon = FeatureRefs.ShifterClawsFeatureAddLevel1.Reference.Get().Icon;
            return FeatureConfigurator.New(InspirePoise, InspirePoiseGuid)
              .SetDisplayName(InspirePoiseDisplayName)
              .SetDescription(InspirePoiseDescription)
              .SetIcon(icon)
              .AddBuffAllSkillsBonusAbilityValue(ModifierDescriptor.Competence, ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(InspirePoiseGuid).WithBonusValueProgression(0, true))
              .SetRanks(10)
              .Configure();
        }

        private const string CloudMind = "CloudMind";
        public static readonly string CloudMindGuid = "{A7C73185-C581-4EDC-8A95-EEE3D4369830}";

        internal const string CloudMindDisplayName = "CloudMind.Name";
        private const string CloudMindDescription = "CloudMind.Description";
        public static BlueprintFeature CloudMindFeature()
        {
            var icon = FeatureRefs.ShiftersEdgeFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(CloudMind, CloudMindGuid)
              .SetDisplayName(CloudMindDisplayName)
              .SetDescription(CloudMindDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Insight, false, StatType.SkillStealth, 5)
              .AddFacts(new() { FeatureRefs.DivinationImmunityFeature.ToString() })
              .Configure();
        }

        private const string SilentSoul = "SilentSoul";
        private static readonly string SilentSoulGuid = "{8F05CF79-26E2-4741-96B4-AFE8FD2A57BA}";

        internal const string SilentSoulDisplayName = "SilentSoul.Name";
        private const string SilentSoulDescription = "SilentSoul.Description";
        public static BlueprintFeature SilentSoulFeature()
        {
            var icon = FeatureRefs.ShifterACBonusUnlock.Reference.Get().Icon;
            return FeatureConfigurator.New(SilentSoul, SilentSoulGuid)
              .SetDisplayName(SilentSoulDisplayName)
              .SetDescription(SilentSoulDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Circumstance, false, StatType.SkillStealth, 10)
              .AddSpellResistanceAgainstSpellDescriptor(spellDescriptor: SpellDescriptor.MindAffecting, value: 20)
              .Configure();
        }

        private const string ViciousFang = "ViciousFang";
        public static readonly string ViciousFangGuid = "{7737F897-1EDD-4631-9101-26AE64974743}";

        internal const string ViciousFangDisplayName = "ViciousFang.Name";
        private const string ViciousFangDescription = "ViciousFang.Description";
        public static BlueprintFeature ViciousFangFeature()
        {
            var icon = FeatureRefs.ShiftersEdgeFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(ViciousFang, ViciousFangGuid)
              .SetDisplayName(ViciousFangDisplayName)
              .SetDescription(ViciousFangDescription)
              .SetIcon(icon)
              .Configure();
        }

        private static readonly string DragonWarDanceName = "DragonWarDance";
        public static readonly string DragonWarDanceGuid = "{F90313BC-68AA-4105-9BD0-14D5359D33C8}";

        private static readonly string DragonWarDanceDisplayName = "DragonWarDance.Name";
        private static readonly string DragonWarDanceDescription = "DragonWarDance.Description";

        private const string WarDanceAbility = "DragonWarDance.WarDanceAbility";
        private static readonly string WarDanceAbilityGuid = "{1CE13E5A-3790-48B4-BD50-B36A4E096D69}";

        private const string WarDanceBuff = "DragonWarDance.WarDanceBuff";
        private static readonly string WarDanceBuffGuid = "{78B468A3-E7AF-49ED-B01D-B66EFD3B1906}";
        public static BlueprintFeature DragonWarDanceConfigure()
        {
            var icon = FeatureRefs.ShifterDragonFormFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(WarDanceBuff, WarDanceBuffGuid)
                .SetDisplayName(DragonWarDanceDisplayName)
                .SetDescription(DragonWarDanceDescription)
                .SetIcon(icon)
                .AddConcealment(concealment: Concealment.Partial, descriptor: ConcealmentDescriptor.Blur)
                .Configure();

            var ability = AbilityConfigurator.New(WarDanceAbility, WarDanceAbilityGuid)
                .SetDisplayName(DragonWarDanceDisplayName)
                .SetDescription(DragonWarDanceDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(DragonWarDanceName, DragonWarDanceGuid)
                    .SetDisplayName(DragonWarDanceDisplayName)
                    .SetDescription(DragonWarDanceDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private static readonly string BendwithWindName = "BendwithWind";
        public static readonly string BendwithWindGuid = "{05E46EA6-F494-4133-902C-3031E3235A93}";

        private static readonly string BendwithWindDisplayName = "BendwithWind.Name";
        private static readonly string BendwithWindDescription = "BendwithWind.Description";

        private const string BendWindAbility = "BendwithWind.BendWindAbility";
        private static readonly string BendWindAbilityGuid = "{D3A13A05-D86E-4801-BDDE-4E473B5B7DAF}";

        private const string BendWindBuff = "BendwithWind.BendWindBuff";
        private static readonly string BendWindBuffGuid = "{77915AA7-1B48-4E2E-A4B0-9806B80AB807}";
        public static BlueprintFeature BendwithWindConfigure()
        {
            var icon = AbilityRefs.FeatherStep.Reference.Get().Icon;

            var buff = BuffConfigurator.New(BendWindBuff, BendWindBuffGuid)
                .SetDisplayName(BendwithWindDisplayName)
                .SetDescription(BendwithWindDescription)
                .SetIcon(icon)
                .AddComponent<DFbindwind>()
                .Configure();

            var ability = AbilityConfigurator.New(BendWindAbility, BendWindAbilityGuid)
                .SetDisplayName(BendwithWindDisplayName)
                .SetDescription(BendwithWindDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().RemoveBuff(OuterStanceBuffGuid).RemoveBuff(WarDanceBuffGuid).ApplyBuffPermanent(buff).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(BendwithWindName, BendwithWindGuid)
                    .SetDisplayName(BendwithWindDisplayName)
                    .SetDescription(BendwithWindDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private static readonly string OuterSphereStanceName = "OuterSphereStance";
        public static readonly string OuterSphereStanceGuid = "{E7D3DD28-1714-4083-A7C4-43A7427C9D55}";

        private static readonly string OuterSphereStanceDisplayName = "OuterSphereStance.Name";
        private static readonly string OuterSphereStanceDescription = "OuterSphereStance.Description";

        private const string OuterStanceAbility = "OuterSphereStance.OuterStanceAbility";
        private static readonly string OuterStanceAbilityGuid = "{01B9D8F0-563E-4431-AF98-6F84EA9C7B92}";

        private const string OuterStanceBuff = "OuterSphereStance.OuterStanceBuff";
        private static readonly string OuterStanceBuffGuid = "{F1182AFE-66DC-4FA5-83AD-018D958DFC92}";

        private static readonly string InnerSphereStanceDisplayName = "InnerSphereStance.Name";
        private static readonly string InnerSphereStanceDescription = "InnerSphereStance.Description";

        private const string InnerStanceAbility = "InnerSphereStance.InnerStanceAbility";
        private static readonly string InnerStanceAbilityGuid = "{38C39F01-EB3E-4FF9-A318-5D76E5D2BB29}";
        public static BlueprintFeature OuterSphereStanceConfigure()
        {
            var icon = FeatureSelectionRefs.SlayerTalentSelection2.Reference.Get().Icon;
            var icon2 = AbilityRefs.WildShapeTurnBackAbility.Reference.Get().Icon;

            var buff = BuffConfigurator.New(OuterStanceBuff, OuterStanceBuffGuid)
                .SetDisplayName(OuterSphereStanceDisplayName)
                .SetDescription(OuterSphereStanceDescription)
                .SetIcon(icon)
                .AddStatBonus(ModifierDescriptor.Penalty, false, StatType.AC, -2)
                .AddComponent<ScarVicious>(c => { c.num = 1; c.checkBuff = false; })
                .Configure();

            var ability = AbilityConfigurator.New(OuterStanceAbility, OuterStanceAbilityGuid)
                .SetDisplayName(OuterSphereStanceDisplayName)
                .SetDescription(OuterSphereStanceDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().RemoveBuff(BendWindBuffGuid).RemoveBuff(WarDanceBuffGuid).ApplyBuffPermanent(buff).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            var ability2 = AbilityConfigurator.New(InnerStanceAbility, InnerStanceAbilityGuid)
                .SetDisplayName(InnerSphereStanceDisplayName)
                .SetDescription(InnerSphereStanceDescription)
                .SetIcon(icon2)
                .AddAbilityEffectRunAction(ActionsBuilder.New().RemoveBuff(OuterStanceBuffGuid).RemoveBuff(BendWindBuffGuid).RemoveBuff(WarDanceBuffGuid).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(OuterSphereStanceName, OuterSphereStanceGuid)
                    .SetDisplayName(OuterSphereStanceDisplayName)
                    .SetDescription(OuterSphereStanceDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability, ability2 })
                    .Configure();
        }
    }
}
