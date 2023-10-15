using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.AreaLogic;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.Modify;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities;

namespace PrestigePlus.PrestigeClasses
{
    internal class AnchoriteofDawn
    {
        private const string ArchetypeName = "AnchoriteofDawn";
        public static readonly string ArchetypeGuid = "{CE227602-D387-4B37-A382-852EAF2D9F9B}";
        internal const string ArchetypeDisplayName = "AnchoriteofDawn.Name";
        private const string ArchetypeDescription = "AnchoriteofDawn.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "AnchoriteofDawnPrestige";
        private static readonly string ClassProgressGuid = "{27930DC5-8896-4017-AB49-985340E6328C}";

        public static void Configure()
        {
            string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";
            CredenceFeat();
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, spellupgradeGuid, SASolarInvocation())
                .AddToLevelEntry(2, CredenceGuid)
                .AddToLevelEntry(3, SABaskRadiance())
                .AddToLevelEntry(4, CredenceGuid)
                .AddToLevelEntry(5, SASolarMove())
                .AddToLevelEntry(6, CredenceGuid)
                .AddToLevelEntry(7, CreateSunbeam())
                .AddToLevelEntry(8, CredenceGuid)
                .AddToLevelEntry(9)
                .AddToLevelEntry(10, SASolarSwift(), DawnFeat(), CredenceGuid)
                .SetUIGroups(UIGroupBuilder.New()
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { spellupgradeGuid, ShadowChainsGuid, ShadowChains2Guid, ShadowChains3Guid })
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { CredenceGuid, SolarGuid, Solar2Guid, Solar3Guid })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] {  }))
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();
            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(4)
                .SetHitDie(DiceType.D8)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABMedium)
                .SetFortitudeSave(SavesPrestigeLow)
                .SetReflexSave(SavesPrestigeHigh)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion, StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillPerception })
                .AddPrerequisiteStatValue(StatType.SkillLoreNature, 5)
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 5)
                .AddPrerequisiteAlignment(AlignmentMaskType.Good, checkInProgression: true)
                .AddPrerequisiteCasterTypeSpellLevel(true, false, 2, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteCasterTypeSpellLevel(false, false, 2, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var AnchoriteofDawnref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = AnchoriteofDawnref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure();
        }

        private const string Credence = "AnchoriteofDawn.Credence";
        private static readonly string CredenceGuid = "{702A2959-B2EC-4149-A677-67BF30A7B363}";

        internal const string CredenceDisplayName = "AnchoriteofDawnCredence.Name";
        private const string CredenceDescription = "AnchoriteofDawnCredence.Description";

        public static void CredenceFeat()
        {
            var icon = AbilityRefs.Thoughtsense.Reference.Get().Icon;
            //"DervishDance": "bdaf6052-e215-4eec-9ad5-c1b3f380cae0",
            FeatureSelectionConfigurator.New(Credence, CredenceGuid)
              .SetDisplayName(CredenceDisplayName)
              .SetDescription(CredenceDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures("bdaf6052-e215-4eec-9ad5-c1b3f380cae0")
              .Configure(delayed: true);
        }

        private const string SolarInvocation = "AnchoriteofDawn.SolarInvocation";
        private static readonly string SolarInvocationGuid = "{51D06D50-859A-460A-B191-E178EA181899}";

        private const string FreeSolarInvocation = "AnchoriteofDawn.FreeSolarInvocation";
        private static readonly string FreeSolarInvocationGuid = "{ADAD9870-6E8C-49F6-B56B-6EED7C3EF8C6}";

        internal const string AnchoriteofDawnSolarInvocationDisplayName = "AnchoriteofDawnSolarInvocation.Name";
        private const string AnchoriteofDawnSolarInvocationDescription = "AnchoriteofDawnSolarInvocation.Description";

        internal const string FreeInvocationDisplayName = "FreeSolarInvocation.Name";
        private const string FreeInvocationDescription = "FreeSolarInvocation.Description";

        private const string SolarAbility = "AnchoriteStyle.SolarAbility";
        private static readonly string SolarAbilityGuid = "{45FA4862-FC2B-40E3-9688-90F3F7ABE8EC}";

        private const string SolarAbilityRes = "AnchoriteStyle.SolarAbilityRes";
        private static readonly string SolarAbilityResGuid = "{04B41671-24D9-4598-BF98-4928F118F3E2}";

        private const string SolarBuff = "AnchoriteStyle.Gazebuff";
        private static readonly string SolarBuffGuid = "{E6FAC7DB-0DF8-488C-91C5-CAD7247FFB1A}";

        private const string SolarBuff2 = "AnchoriteStyle.Gazebuff2";
        private static readonly string SolarBuffGuid2 = "{92537510-0C34-48DE-A78E-F4AC8791605A}";

        private const string SolarBuff3 = "AnchoriteStyle.Gazebuff3";
        private static readonly string SolarBuffGuid3 = "{6EB3BA91-3A21-41ED-AF79-91A4D90A8F99}";

        private const string SolarBuff4 = "AnchoriteStyle.Gazebuff4";
        public static readonly string SolarBuffGuid4 = "{973B8E27-94EE-4E9E-9C86-8D8F17C88A44}";

        private const string SolarBuff5 = "AnchoriteStyle.Gazebuff5";
        public static readonly string SolarBuffGuid5 = "{1E228A48-776D-4035-99DB-BCC08F6762FB}";

        private const string GazeAura = "AnchoriteStyle.GazeAura";
        private static readonly string GazeAuraGuid = "{048B72FD-2E5E-4346-9B4D-C5C1B6944599}";
        public static BlueprintFeature SASolarInvocation()
        {
            var icon = FeatureRefs.DomainMastery.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(SolarAbilityRes, SolarAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevel(classes: new string[] { ArchetypeGuid }, 2)
                        .IncreaseByStat(StatType.Charisma))
                .Configure();

            var Buff2 = BuffConfigurator.New(SolarBuff2, SolarBuffGuid2)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddComponent<SolarInvocationComp>()
              .AddIncreaseAllSpellsDC(spellsOnly: true, value: 1)
              .Configure();

            var givebuff = ActionsBuilder.New().ApplyBuffPermanent(Buff2).Build();
            var retrivebuff = ActionsBuilder.New().RemoveBuff(Buff2).Build();

            var area = AbilityAreaEffectConfigurator.New(GazeAura, GazeAuraGuid)
                .SetAffectEnemies(false)
                .SetTargetType(BlueprintAbilityAreaEffect.TargetType.Ally)
                .SetAffectDead(false)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(FeetExtension.Feet(33))
                .AddAbilityAreaEffectBuff(Buff2, condition: ConditionsBuilder.New().CasterHasFact(BaskRadianceGuid).Build())
                .Configure();

            var Buff3 = BuffConfigurator.New(SolarBuff3, SolarBuffGuid3)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddAreaEffect(area)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var Buff5 = BuffConfigurator.New(SolarBuff5, SolarBuffGuid5)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .Configure();

            var Buff1 = BuffConfigurator.New(SolarBuff, SolarBuffGuid)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddComponent<SolarInvocationComp>()
              .AddIncreaseAllSpellsDC(spellsOnly: true, value: 1)
              .AddBuffActions(activated: ActionsBuilder.New()
                .OnPets(givebuff)
                .Conditional(ConditionsBuilder.New()
                    .HasBuff(Buff5).Build(),
                    ifFalse: ActionsBuilder.New().RemoveSelf().Build())
                .Build(),
                deactivated: ActionsBuilder.New().OnPets(retrivebuff).Build())
              .AddAuraFeatureComponent(Buff3)
              .Configure();

            BuffConfigurator.New(SolarBuff4, SolarBuffGuid4)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddComponent<SolarInvocationComp>()
              .AddIncreaseAllSpellsDC(spellsOnly: true, value: 1)
              .AddBuffActions(activated: ActionsBuilder.New()
                .OnPets(givebuff)
                .Build(), 
                deactivated: ActionsBuilder.New().OnPets(retrivebuff).Build())
              .AddAuraFeatureComponent(Buff3)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(SolarAbility, SolarAbilityGuid)
                .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
                .SetDescription(AnchoriteofDawnSolarInvocationDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateIfCombatEnded(true)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound, freeBlueprint: FreeSolarInvocationGuid)
                .SetDeactivateImmediately()
                .Configure();

            FeatureConfigurator.New(FreeSolarInvocation, FreeSolarInvocationGuid)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .Configure();

            var action = ActionsBuilder.New().ApplyBuff(Buff5, durationValue: ContextDuration.Fixed(3)).Build();

            return FeatureConfigurator.New(SolarInvocation, SolarInvocationGuid)
              .SetDisplayName(FreeInvocationDisplayName)
              .SetDescription(FreeInvocationDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddAbilityUseTrigger(action: action, checkSpellSchool: true, isSpellSchool: SpellSchool.Evocation)
              .Configure();
        }

        private const string BaskRadiance = "Anchorite.BaskRadiance";
        private static readonly string BaskRadianceGuid = "{B566CA3D-C3F2-4CDD-B33E-F3E3D1257A62}";

        internal const string AnchoriteBaskRadianceDisplayName = "AnchoriteBaskRadiance.Name";
        private const string AnchoriteBaskRadianceDescription = "AnchoriteBaskRadiance.Description";
        public static BlueprintFeature SABaskRadiance()
        {
            var icon = FeatureRefs.AngelHaloArchonsAuraFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(BaskRadiance, BaskRadianceGuid)
              .SetDisplayName(AnchoriteBaskRadianceDisplayName)
              .SetDescription(AnchoriteBaskRadianceDescription)
              .SetIcon(icon)
              .Configure();
        }

        private const string SolarMove = "Anchorite.SolarMove";
        private static readonly string SolarMoveGuid = "{95811779-A38E-40AF-AF88-494B8C70F74D}";

        internal const string AnchoriteSolarMoveDisplayName = "AnchoriteSolarMove.Name";
        private const string AnchoriteSolarMoveDescription = "AnchoriteSolarMove.Description";
        public static BlueprintFeature SASolarMove()
        {
            var icon = FeatureRefs.DomainMastery.Reference.Get().Icon;
            return FeatureConfigurator.New(SolarMove, SolarMoveGuid)
              .SetDisplayName(AnchoriteSolarMoveDisplayName)
              .SetDescription(AnchoriteSolarMoveDescription)
              .SetIcon(icon)
              .AddAbilityResources(resource: SolarAbilityResGuid, restoreAmount: true)
              .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(SolarAbilityGuid); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
              .AddRemoveFeatureOnApply(SolarInvocationGuid)
              .Configure();
        }

        private const string SolarSwift = "Anchorite.SolarSwift";
        private static readonly string SolarSwiftGuid = "{81157D97-7D4D-4910-B25A-F4129CFAAE3E}";

        internal const string AnchoriteSolarSwiftDisplayName = "AnchoriteSolarSwift.Name";
        private const string AnchoriteSolarSwiftDescription = "AnchoriteSolarSwift.Description";
        public static BlueprintFeature SASolarSwift()
        {
            var icon = FeatureRefs.DomainMastery.Reference.Get().Icon;
            return FeatureConfigurator.New(SolarSwift, SolarSwiftGuid)
              .SetDisplayName(AnchoriteSolarSwiftDisplayName)
              .SetDescription(AnchoriteSolarSwiftDescription)
              .SetIcon(icon)
              .AddAutoMetamagic(new() { SolarAbilityGuid }, metamagic: Metamagic.Quicken)
              .Configure();
        }

        private const string Sunbeam = "Anchorite.Sunbeam";
        private static readonly string SunbeamGuid = "{7A85CDC3-35C5-4FA6-A83F-812C19B91676}";
        internal const string SunbeamDisplayName = "AnchoriteSunbeam.Name";
        private const string SunbeamDescription = "AnchoriteSunbeam.Description";

        private const string SunbeamAblity = "Anchorite.UseSunbeam";
        private static readonly string SunbeamAblityGuid = "{92AFBDAE-29CD-4576-A812-7A169C308835}";

        private const string SunbeamAblityRes = "Anchorite.SunbeamRes";
        private static readonly string SunbeamAblityResGuid = "{3109B0BB-F006-4988-9FFB-1AA846B87AAC}";

        private static BlueprintFeature CreateSunbeam()
        {
            var icon = AbilityRefs.Sunbeam.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(SunbeamAblityRes, SunbeamAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 3, bonusPerStep: 1, startingLevel: 7))
                .SetUseMax()
                .SetMax(2)
                .Configure();
            var ability = AbilityConfigurator.New(SunbeamAblity, SunbeamAblityGuid)
                .CopyFrom(
                AbilityRefs.Sunbeam,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent))
                .SetType(AbilityType.SpellLike)
                .AddAbilityCasterInCombat(true)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .AddReplaceAbilityDC(stat: StatType.Charisma)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: SunbeamAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(Sunbeam, SunbeamGuid)
              .SetDisplayName(SunbeamDisplayName)
              .SetDescription(SunbeamDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string Dawn = "Anchorite.Dawn";
        private static readonly string DawnGuid = "{DD8D398C-A81C-4E7F-A5EB-291787FAF4B0}";

        private const string DawnAblity = "Anchorite.UseDawn";
        private static readonly string DawnAblityGuid = "{BBE7E7B3-A634-42FF-B31F-4FD997BE848B}";

        private const string DawnAblityRes = "Anchorite.UseDawnRes";
        private static readonly string DawnAblityResGuid = "{8C824C12-30C4-412B-9ED1-1011FFD0C385}";

        internal const string DawnDisplayName = "AnchoriteDawn.Name";
        private const string DawnDescription = "AnchoriteDawn.Description";
        public static BlueprintFeature DawnFeat()
        {
            var icon = FeatureRefs.DawnOfLifeFeature.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(DawnAblityRes, DawnAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(DawnAblity, DawnAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuff(SolarBuffGuid4, ContextDuration.Variable(ContextValues.Constant(10)))
                    .Build())
                .SetDisplayName(DawnDisplayName)
                .SetDescription(DawnDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(Dawn, DawnGuid)
              .SetDisplayName(DawnDisplayName)
              .SetDescription(DawnDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddAutoMetamagic(new() { ability }, metamagic: Kingmaker.UnitLogic.Abilities.Metamagic.Quicken)
              .Configure();
        }
    }
}
