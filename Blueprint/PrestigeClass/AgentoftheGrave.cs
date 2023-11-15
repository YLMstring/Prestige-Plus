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
using Kingmaker.UnitLogic.Alignments;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.ElementsSystem;
using BlueprintCore.Utils.Types;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.BasicEx;
using Kingmaker.RuleSystem.Rules.Damage;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Designers.Mechanics.Facts;
using PrestigePlus.Feats;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Patch;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class AgentoftheGrave
    {
        private const string ArchetypeName = "AgentoftheGrave";
        public static readonly string ArchetypeGuid = "{36FF0597-88F2-4FE1-9BC9-7FDDCB546CF8}";
        internal const string ArchetypeDisplayName = "AgentoftheGrave.Name";
        private const string ArchetypeDescription = "AgentoftheGrave.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "AgentoftheGravePrestige";
        private static readonly string ClassProgressGuid = "{C7BA87F4-64B1-4F3F-AF2F-974A3D1AF180}";
        public static void Configure()
        {
            string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, spellupgradeGuid, CreateLichTouch(), CreateUnholyFortitude())
                .AddToLevelEntry(2, FeatureRefs.BloodlineUndeadArcana.ToString())
                .AddToLevelEntry(3, CreateDesecrate())
                .AddToLevelEntry(4, FeatureRefs.NegativeEnergyAffinityDhampir.ToString(), CreateDeathShroud())
                .AddToLevelEntry(5, ExtraFeat())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.Evasion.Reference.Get().ToReference<BlueprintFeatureBaseReference>(), FeatureRefs.ImprovedEvasion.Reference.Get().ToReference<BlueprintFeatureBaseReference>() })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.UncannyDodge.Reference.Get().ToReference<BlueprintFeatureBaseReference>(), FeatureRefs.ImprovedUncannyDodge.Reference.Get().ToReference<BlueprintFeatureBaseReference>() }))
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
                .SetBaseAttackBonus(BABLow)
                .SetFortitudeSave(SavesPrestigeHigh)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeWorld, StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillUseMagicDevice, StatType.SkillPersuasion })
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeArcana, 5)
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 5)
                .AddComponent<PrerequisiteSpellKnown>(c => { c.m_Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AnimateDead.ToString()); c.RequireSpellbook = true; })
                .AddPrerequisiteAlignment(AlignmentMaskType.Evil, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.LawfulNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var AgentoftheGraveref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = AgentoftheGraveref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string LichTouch = "AgentoftheGrave.LichTouch";
        private static readonly string LichTouchGuid = "{2192E2B1-1D10-4764-BC97-B0DCD27D17C1}";
        internal const string LichTouchDisplayName = "AgentoftheGraveLichTouch.Name";
        private const string LichTouchDescription = "AgentoftheGraveLichTouch.Description";

        private const string LichTouchAblity = "AgentoftheGrave.UseLichTouch";
        private static readonly string LichTouchAblityGuid = "{8F27B6E7-A10D-41A1-A187-9479A3F17820}";

        private const string LichTouchAblityRes = "AgentoftheGrave.LichTouchRes";
        private static readonly string LichTouchAblityResGuid = "{59FE69D1-5D96-49D0-927D-BF446B141CEC}";

        private static BlueprintFeature CreateLichTouch()
        {
            var icon = AbilityRefs.BloodlineUndeadGraspOfTheDeadAbility.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(LichTouchAblityRes, LichTouchAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(3)
                        .IncreaseByStat(StatType.Charisma))
                .SetUseMax()
                .Configure();

            var ability = AbilityConfigurator.New(LichTouchAblity, LichTouchAblityGuid)
                .CopyFrom(
                AbilityRefs.WarpriestFervorNegativeAbility,
                typeof(AbilityDeliverTouch))
                .SetDisplayName(LichTouchDisplayName)
                .SetDescription(LichTouchDescription)
                .SetIcon(icon)
                .SetCanTargetSelf(true)
                .SetType(AbilityType.Supernatural)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().HasFact(FeatureRefs.NegativeEnergyAffinity.ToString()).Build(),
                    ifTrue: ActionsBuilder.New()
                    .HealTarget(ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Rank()))
                    .Build(),
                    ifFalse: ActionsBuilder.New()
                    .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Rank()), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.NegativeEnergy))
                    .Build())
                .Build())
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }, max: 5, min: 1))
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: LichTouchAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(LichTouch, LichTouchGuid)
              .SetDisplayName(LichTouchDisplayName)
              .SetDescription(LichTouchDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string UnholyFortitude = "AgentoftheGrave.UnholyFortitude";
        private static readonly string UnholyFortitudeGuid = "{FF69F897-5884-48D5-9CF0-5F616ECA618C}";
        internal const string UnholyFortitudeDisplayName = "AgentoftheGraveUnholyFortitude.Name";
        private const string UnholyFortitudeDescription = "AgentoftheGraveUnholyFortitude.Description";

        private static BlueprintFeature CreateUnholyFortitude()
        {
            var icon = AbilityRefs.BloodlineUndeadGraveTouchAbility.Reference.Get().Icon;

            FeatureConfigurator.For(FeatureRefs.UndeadImmunities)
                .AddRemoveFeatureOnApply(UnholyFortitudeGuid)
                .Configure();

            return FeatureConfigurator.New(UnholyFortitude, UnholyFortitudeGuid)
              .SetDisplayName(UnholyFortitudeDisplayName)
              .SetDescription(UnholyFortitudeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              //.AddRaiseStatToMinimum(stat: StatType.Constitution, targetValue: ContextValues.Rank())
              //.AddContextRankConfig(ContextRankConfigs.BaseStat(StatType.Charisma))
              .AddComponent<UnholyFortitudeLogic>()
              .Configure();
        }
        private const string DeathShroud = "AgentoftheGrave.DeathShroud";
        private static readonly string DeathShroudGuid = "{722CBC3E-0B7B-481E-9B45-70E0654B5185}";
        internal const string DeathShroudDisplayName = "AgentoftheGraveDeathShroud.Name";
        private const string DeathShroudDescription = "AgentoftheGraveDeathShroud.Description";

        private static BlueprintFeature CreateDeathShroud()
        {
            return FeatureConfigurator.New(DeathShroud, DeathShroudGuid)
              .SetDisplayName(DeathShroudDisplayName)
              .SetDescription(DeathShroudDescription)
              .SetIsClassFeature(true)
              .AddUndetectableAlignment()
              .Configure();
        }

        private const string Desecrate = "AgentoftheGrave.Desecrate";
        private static readonly string DesecrateGuid = "{78CDE1D4-A480-42B8-A5C4-20ABF7906221}";
        internal const string DesecrateDisplayName = "AgentoftheGraveDesecrate.Name";
        private const string DesecrateDescription = "AgentoftheGraveDesecrate.Description";

        private const string DesecrateAblity = "AgentoftheGrave.UseDesecrate";
        private static readonly string DesecrateAblityGuid = "{9368D400-0086-4AF1-9122-12DDE6F7C98D}";

        private const string DesecrateAblityRes = "AgentoftheGrave.DesecrateRes";
        private static readonly string DesecrateAblityResGuid = "{0B3D4292-41B3-46CB-829A-E298F00C2AD8}";

        private const string DesecrateBuff = "DesecrateBuff";
        private static readonly string DesecrateGuidBuff = "{93C68F9F-515B-4F4E-BD55-7EB855F51E7D}";

        private const string DesecrateBuff2 = "DesecrateBuff2";
        private static readonly string DesecrateGuidBuff2 = "{C235ABC0-33EA-4E5F-ABDC-0233C1BB8293}";

        private const string DesecrateAuraBuff = "DesecrateAuraBuff";
        private static readonly string DesecrateAuraGuidBuff = "{5A914FA4-5236-4FBF-BECD-090A99B594A8}";

        private const string DesecrateAura = "DesecrateAura";
        private static readonly string DesecrateAuraGuid = "{E3D52E2A-E0E8-4DC8-A7AC-75EC9029C6D7}";

        private static BlueprintFeature CreateDesecrate()
        {
            var icon = AbilityRefs.ProfaneBendeictionAbility.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(DesecrateBuff, DesecrateGuidBuff)
              .SetDisplayName(DesecrateDisplayName)
              .SetDescription(DesecrateDescription)
              .SetIcon(icon)
              //.AddAttackBonusConditional(bonus: ContextValues.Constant(1), descriptor: ModifierDescriptor.Profane)
              .AddContextStatBonus(StatType.AdditionalAttackBonus, 1, ModifierDescriptor.Profane)
              .AddDamageBonusConditional(bonus: ContextValues.Constant(1), descriptor: ModifierDescriptor.Profane)
              //.AddSavingThrowBonusAgainstAbilityType(bonus: ContextValues.Constant(1), modifierDescriptor: ModifierDescriptor.Profane)
              .AddBuffAllSavesBonus(ModifierDescriptor.Profane, 1)
              .Configure();

            var Buff2 = BuffConfigurator.New(DesecrateBuff2, DesecrateGuidBuff2)
              .SetDisplayName(DesecrateDisplayName)
              .SetDescription(DesecrateDescription)
              .SetIcon(icon)
              .AddStatBonusAbilityValue(ModifierDescriptor.Profane, StatType.HitPoints, ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.CasterLevel())
              .Configure();

            var abilityresourse = AbilityResourceConfigurator.New(DesecrateAblityRes, DesecrateAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .SetUseMax()
                .Configure();

            var area = AbilityAreaEffectConfigurator.New(DesecrateAura, DesecrateAuraGuid)
                .SetAffectEnemies(false)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(23.Feet())
                .AddAbilityAreaEffectBuff(buff: Buff, checkConditionEveryRound: true,
                    condition: ConditionsBuilder.New().HasFact(FeatureRefs.UndeadType.ToString()).Build())
                .Configure();

            var Aura = BuffConfigurator.New(DesecrateAuraBuff, DesecrateAuraGuidBuff)
              .SetDisplayName(DesecrateDisplayName)
              .SetDescription(DesecrateDescription)
              .SetIcon(icon)
              .AddAreaEffect(area)
              .Configure();

            var ability = AbilityConfigurator.New(DesecrateAblity, DesecrateAblityGuid)
                .CopyFrom(
                AbilityRefs.AuraOfGreaterCourage,
                typeof(AbilitySpawnFx))
                .SetType(AbilityType.Supernatural)
                .SetIcon(icon)
                .SetDisplayName(DesecrateDisplayName)
                .SetDescription(DesecrateDescription)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuff(Aura, ContextDuration.Variable(rate: Kingmaker.UnitLogic.Mechanics.DurationRate.TenMinutes, value: ContextValues.Rank()))
                    .Build())
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: DesecrateAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(Desecrate, DesecrateGuid)
              .SetDisplayName(DesecrateDisplayName)
              .SetDescription(DesecrateDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddOnSpawnBuff(Buff2, ifSummonHaveFact: FeatureRefs.UndeadType.ToString(), checkSummonedUnitFact: true, isInfinity: true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string Ghoul = "AgentoftheGrave.Ghoul";
        private static readonly string GhoulGuid = "{DC99A999-022B-40E1-A258-F1CA3848F028}";
        internal const string GhoulDisplayName = "AgentoftheGraveGhoul.Name";
        private const string GhoulDescription = "AgentoftheGraveGhoul.Description";

        private static BlueprintFeature CreateGhoul()
        {
            var icon = AbilityRefs.ArmySummonGhoul.Reference.Get().Icon;

            return FeatureConfigurator.New(Ghoul, GhoulGuid)
              .SetDisplayName(GhoulDisplayName)
              .SetDescription(GhoulDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteNoFeature(FeatureRefs.MythicLichSkeletonCompanionArcher.ToString())
              .AddPrerequisiteNoFeature(FeatureRefs.MythicLichSkeletonCompanionDualWielder.ToString())
              .AddPrerequisiteNoFeature(FeatureRefs.MythicLichSkeletonCompanionTank.ToString())
              .AddPrerequisiteNoFeature(FeatureRefs.MythicLichSkeletonCompanionTwoHanded.ToString())
              .AddFacts(new() { FeatureRefs.UndeadType.ToString(), FeatureRefs.GhastParalysisFeature.ToString(), FeatureRefs.DLC4_Ghast_StenchFeature.ToString(), FeatureRefs.GhoulDiseaseFeature.ToString(), FeatureRefs.ChannelResistance2.ToString() })
              .AddAdditionalLimb(ItemWeaponRefs.Bite1d6.ToString())
              .AddEmptyHandWeaponOverride(isMonkUnarmedStrike: false, isPermanent: true, weapon: ItemWeaponRefs.Claw1d6.ToString())
              .Configure();
        }

        private const string Vampire = "AgentoftheGrave.Vampire";
        private static readonly string VampireGuid = "{2D4B53FF-CFC9-49B6-A820-B11C1166252E}";
        internal const string VampireDisplayName = "AgentoftheGraveVampire.Name";
        private const string VampireDescription = "AgentoftheGraveVampire.Description";

        private static BlueprintFeature CreateVampire()
        {
            var icon = AbilityRefs.VampiricTouch.Reference.Get().Icon;

            return FeatureConfigurator.New(Vampire, VampireGuid)
              .SetDisplayName(VampireDisplayName)
              .SetDescription(VampireDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteNoFeature(FeatureRefs.MythicLichSkeletonCompanionArcher.ToString())
              .AddPrerequisiteNoFeature(FeatureRefs.MythicLichSkeletonCompanionDualWielder.ToString())
              .AddPrerequisiteNoFeature(FeatureRefs.MythicLichSkeletonCompanionTank.ToString())
              .AddPrerequisiteNoFeature(FeatureRefs.MythicLichSkeletonCompanionTwoHanded.ToString())
              .AddPrerequisiteFeature(FeatureRefs.BloodDrinker.ToString())
              .AddFacts(new() { FeatureRefs.UndeadType.ToString(), FeatureRefs.VampireSpawnFeatures.ToString() })
              .AddEmptyHandWeaponOverride(isMonkUnarmedStrike: false, isPermanent: true, weapon: ItemWeaponRefs.Slam1d6.ToString())
              .AddOutgoingPhysicalDamageProperty(addMagic: true, naturalAttacks: true)
              .Configure();
        }

        private const string UndeathInitiate = "UndeathInitiate";
        private static readonly string UndeathInitiateGuid = "{522579B1-2041-4A91-8C44-BB42D066B40C}";

        internal const string UndeathInitiateDisplayName = "UndeathInitiate.Name";
        private const string UndeathInitiateDescription = "UndeathInitiate.Description";
        public static BlueprintFeatureSelection ExtraFeat()
        {
            var icon = AbilityRefs.AnimateDead.Reference.Get().Icon;

            FeatureConfigurator.For(FeatureRefs.MythicLichSkeletonCompanionDualWielder.ToString())
                .AddPrerequisiteNoFeature(GhoulGuid)
                .AddPrerequisiteNoFeature(VampireGuid)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.MythicLichSkeletonCompanionArcher.ToString())
                .AddPrerequisiteNoFeature(GhoulGuid)
                .AddPrerequisiteNoFeature(VampireGuid)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.MythicLichSkeletonCompanionTank.ToString())
                .AddPrerequisiteNoFeature(GhoulGuid)
                .AddPrerequisiteNoFeature(VampireGuid)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.MythicLichSkeletonCompanionTwoHanded.ToString())
                .AddPrerequisiteNoFeature(GhoulGuid)
                .AddPrerequisiteNoFeature(VampireGuid)
                .Configure();

            return FeatureSelectionConfigurator.New(UndeathInitiate, UndeathInitiateGuid)
              .SetDisplayName(UndeathInitiateDisplayName)
              .SetDescription(UndeathInitiateDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(GraveSpellList.SecretDeathGuid)
              .AddToAllFeatures(CreateGhoul())
              .AddToAllFeatures(CreateVampire())
              .Configure(delayed: true);
        }
    }
}
