using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.CustomComponent.BasePrestigeEnhance;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Kingmaker.RuleSystem.Rules.Damage;
using BlueprintCore.Actions.Builder.BasicEx;
using PrestigePlus.CustomAction;
using Kingmaker.UI.MVVM._VM.Other;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class HeritorKnight
    {
        private const string ArchetypeName = "HeritorKnight";
        public static readonly string ArchetypeGuid = "{9D66FDCD-1EFF-47AE-821C-6F269EC3E589}";
        internal const string ArchetypeDisplayName = "HeritorKnight.Name";
        private const string ArchetypeDescription = "HeritorKnight.Description";

        private const string ClassProgressName = "HeritorKnightPrestige";
        private static readonly string ClassProgressGuid = "{038CAF66-AC84-490F-AA92-D8BBA2F71A33}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, SkillHeartFeat())
                .AddToLevelEntry(2, HeritorHonorFeature(), CreateWitchesWoe())
                .AddToLevelEntry(3, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
                .AddToLevelEntry(4, CreateWraithwall())
                .AddToLevelEntry(5, RedeemerUndeathConfigure())
                .AddToLevelEntry(6, MightyStrikeConfigure())
                .AddToLevelEntry(7, CreateImageDivinity())
                .AddToLevelEntry(8, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
                .AddToLevelEntry(9, CreateFreedBlood())
                .AddToLevelEntry(10, SkyStrideFeature())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { }))
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
                .SetSkillPoints(2)
                .SetHitDie(DiceType.D10)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(StatProgressionRefs.BABFull.ToString())
                .SetFortitudeSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetReflexSave(StatProgressionRefs.SavesPrestigeLow.ToString())
                .SetWillSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillMobility, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
                .AddPrerequisiteFeature(FeatureRefs.IomedaeFeature.ToString())
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 5)
                .AddPrerequisiteFeature(FeatureRefs.IronWill.ToString())
                .AddPrerequisiteFeature(FeatureRefs.SkillFocusLoreReligion.ToString())
                .AddPrerequisiteParametrizedWeaponFeature(ParametrizedFeatureRefs.WeaponFocus.ToString(), WeaponCategory.Longsword)
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        internal const string SkillHeartDisplayName = "HeritorKnightSkillHeart.Name";
        private const string SkillHeartDescription = "HeritorKnightSkillHeart.Description";

        private const string SkillHeart = "HeritorKnight.SkillHeart";
        private static readonly string SkillHeartGuid = "{B47E5743-10B1-4790-A6AB-AA3310F48F4B}";
        public static BlueprintFeatureSelection SkillHeartFeat()
        {
            var icon = FeatureRefs.FighterWeaponMastery.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(SkillHeart, SkillHeartGuid)
              .SetDisplayName(SkillHeartDisplayName)
              .SetDescription(SkillHeartDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(AsOneFeature())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string AsOne = "HeritorKnightAsOne";
        private static readonly string AsOneGuid = "{858E0DED-4212-490B-A482-F88C8C679D02}";
        public static BlueprintFeature AsOneFeature()
        {
            var icon = FeatureRefs.FighterWeaponMastery.Reference.Get().Icon;

            return FeatureConfigurator.New(AsOne, AsOneGuid)
              .SetDisplayName(SkillHeartDisplayName)
              .SetDescription(SkillHeartDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureSelectionRefs.WeaponTrainingSelection.ToString())
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.FighterClass.ToString(), modifier: 1, summand: 0)
              .Configure();
        }

        private const string HeritorHonor = "HeritorKnightHeritorHonor";
        private static readonly string HeritorHonorGuid = "{DB369DA9-FF1F-42D6-AE31-ACDE9F644FDE}";

        internal const string HeritorHonorDisplayName = "HeritorKnightHeritorHonor.Name";
        private const string HeritorHonorDescription = "HeritorKnightHeritorHonor.Description";
        public static BlueprintFeature HeritorHonorFeature()
        {
            var icon = FeatureRefs.LayOnHandsFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(HeritorHonor, HeritorHonorGuid)
              .SetDisplayName(HeritorHonorDisplayName)
              .SetDescription(HeritorHonorDescription)
              .SetIcon(icon)
              .AddDerivativeStatBonus(StatType.Charisma, StatType.SaveWill)
              .AddRecalculateOnStatChange(stat: StatType.Charisma, useKineticistMainStat: false)
              .Configure();
        }

        private const string WitchesWoe = "HeritorKnight.WitchesWoe";
        public static readonly string WitchesWoeGuid = "{87456535-6DFF-4D75-8A59-A36FBF066210}";
        internal const string WitchesWoeDisplayName = "HeritorKnightWitchesWoe.Name";
        private const string WitchesWoeDescription = "HeritorKnightWitchesWoe.Description";

        private const string WitchesWoeAblity2 = "HeritorKnight.UseWitchesWoe2";
        private static readonly string WitchesWoeAblity2Guid = "{331E8D21-D73C-4500-A803-FD65ADD660D8}";

        private const string WitchesWoeAblityRes = "HeritorKnight.WitchesWoeRes";
        private static readonly string WitchesWoeAblityResGuid = "{0868A010-F81E-4638-900B-D6B9AC67A2A6}";
        private static BlueprintFeature CreateWitchesWoe()
        {
            var icon = AbilityRefs.RemoveCurse.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(WitchesWoeAblityRes, WitchesWoeAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ability2 = AbilityConfigurator.New(WitchesWoeAblity2, WitchesWoeAblity2Guid)
                .CopyFrom(
                AbilityRefs.RemoveCurse,
                typeof(SpellComponent),
                typeof(AbilityEffectRunAction),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 3)
                .SetType(AbilityType.SpellLike)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(WitchesWoe, WitchesWoeGuid)
              .SetDisplayName(WitchesWoeDisplayName)
              .SetDescription(WitchesWoeDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Rank(), spellDescriptor: SpellDescriptor.Hex | SpellDescriptor.Curse)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithDiv2Progression())
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string SkyStride = "HeritorKnightSkyStride";
        private static readonly string SkyStrideGuid = "{C161AF72-C193-443D-B9BC-0FB2F750506B}";

        internal const string SkyStrideDisplayName = "HeritorKnightSkyStride.Name";
        private const string SkyStrideDescription = "HeritorKnightSkyStride.Description";
        public static BlueprintFeature SkyStrideFeature()
        {
            var icon = AbilityRefs.WalkThroughSpace.Reference.Get().Icon;
            return FeatureConfigurator.New(SkyStride, SkyStrideGuid)
              .SetDisplayName(SkyStrideDisplayName)
              .SetDescription(SkyStrideDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.FeatureWingsAngel.ToString() })
              .Configure();
        }

        private const string Wraithwall = "HeritorKnight.Wraithwall";
        public static readonly string WraithwallGuid = "{7581089F-9992-4610-9CB7-243E1315F6DD}";
        internal const string WraithwallDisplayName = "HeritorKnightWraithwall.Name";
        private const string WraithwallDescription = "HeritorKnightWraithwall.Description";

        private const string WraithwallAblity2 = "HeritorKnight.UseWraithwall2";
        private static readonly string WraithwallAblity2Guid = "{9CED1644-D9DE-480C-AE1D-66562340E76E}";

        private const string WraithwallAblityRes = "HeritorKnight.WraithwallRes";
        private static readonly string WraithwallAblityResGuid = "{B9041DBA-C3B7-4976-8DBC-BFBC9A82B28D}";

        private const string WraithwallBuff = "HeritorKnight.WraithwallBuff";
        private static readonly string WraithwallBuffGuid = "{1348676D-063B-4ADD-B819-A44A888C6A5C}";
        private static BlueprintFeature CreateWraithwall()
        {
            var icon = AbilityRefs.Repulsion.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(WraithwallAblityRes, WraithwallAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var Buff = BuffConfigurator.New(WraithwallBuff, WraithwallBuffGuid)
              .SetDisplayName(WraithwallDisplayName)
              .SetDescription(WraithwallDescription)
              .SetIcon(icon)
              .AddComponent<GhostArmorComponent>()
              .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: ModifierDescriptor.Sacred, spellDescriptor: SpellDescriptor.NegativeLevel | SpellDescriptor.StatDebuff)
              .Configure();

            var ability2 = AbilityConfigurator.New(WraithwallAblity2, WraithwallAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuff(Buff, ContextDuration.Fixed(1, DurationRate.Minutes))
                    .Build())
                .SetDisplayName(WraithwallDisplayName)
                .SetDescription(WraithwallDescription)
                .SetIcon(icon)
                .AddComponent(AbilityRefs.Bless.Reference.Get().GetComponent<AbilitySpawnFx>())
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Ally, radius: 30.Feet(), spreadSpeed: 40.Feet())
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(Wraithwall, WraithwallGuid)
              .SetDisplayName(WraithwallDisplayName)
              .SetDescription(WraithwallDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string FreedBlood = "HeritorKnight.FreedBlood";
        public static readonly string FreedBloodGuid = "{AF324393-6CC6-4AB7-8DBD-F146426852E0}";
        internal const string FreedBloodDisplayName = "HeritorKnightFreedBlood.Name";
        private const string FreedBloodDescription = "HeritorKnightFreedBlood.Description";

        private const string FreedBloodAblity2 = "HeritorKnight.UseFreedBlood2";
        private static readonly string FreedBloodAblity2Guid = "{D6576E9D-07D9-47AC-B34C-055D555204B2}";

        private const string FreedBloodAblityRes = "HeritorKnight.FreedBloodRes";
        private static readonly string FreedBloodAblityResGuid = "{F5B3A70D-AE84-42D6-8616-09BBB749C561}";

        private const string FreedBloodBuff = "HeritorKnight.FreedBloodBuff";
        private static readonly string FreedBloodBuffGuid = "{9D631947-9309-4AF7-AB01-89A0DDE70557}";
        private static BlueprintFeature CreateFreedBlood()
        {
            var icon = FeatureRefs.BleedingInfusionFeature.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(FreedBloodAblityRes, FreedBloodAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(3))
                .Configure();

            var Buff = BuffConfigurator.New(FreedBloodBuff, FreedBloodBuffGuid)
              .SetDisplayName(FreedBloodDisplayName)
              .SetDescription(FreedBloodDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(9, modifierDescriptor: ModifierDescriptor.Sacred, spellDescriptor: SpellDescriptor.Death | SpellDescriptor.Paralysis | SpellDescriptor.Compulsion)
              .Configure();

            var selfdamage = ActionsBuilder.New()
                .DealDamage(value: ContextDice.Value(DiceType.Zero, 0, 9), damageType: DamageTypes.Direct())
                .Build();

            var ability2 = AbilityConfigurator.New(FreedBloodAblity2, FreedBloodAblity2Guid)
                .AllowTargeting(false, false, true, true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .OnContextCaster(selfdamage)
                    .DispelMagic(buffType: Kingmaker.UnitLogic.Mechanics.Actions.ContextActionDispelMagic.BuffType.All, checkType: Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.CasterLevel, maxSpellLevel: ContextValues.Constant(9), checkSchoolOrDescriptor: false, descriptor: SpellDescriptor.Death | SpellDescriptor.Paralysis | SpellDescriptor.Compulsion)
                    .ApplyBuff(Buff, ContextDuration.Fixed(1))
                    .Build())
                .SetDisplayName(FreedBloodDisplayName)
                .SetDescription(FreedBloodDescription)
                .SetIcon(icon)
                .AddComponent(AbilityRefs.BloodBlastAbility.Reference.Get().GetComponent<AbilitySpawnFx>())
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(FreedBlood, FreedBloodGuid)
              .SetDisplayName(FreedBloodDisplayName)
              .SetDescription(FreedBloodDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private static readonly string MightyStrikeName = "HeritorKnightMightyStrike";
        public static readonly string MightyStrikeGuid = "{048CCF0B-97B8-4F6C-8EED-1461FD2D601F}";

        private static readonly string MightyStrikeDisplayName = "HeritorKnightMightyStrike.Name";
        private static readonly string MightyStrikeDescription = "HeritorKnightMightyStrike.Description";

        private const string MightyStrikeBuff = "MightyStrike.MightyStrikeBuff";
        public static readonly string MightyStrikeBuffGuid = "{853E1768-A1B6-41BB-B2C0-C6F9BDEA9493}";
        public static BlueprintFeature MightyStrikeConfigure()
        {
            var icon = AbilityRefs.RighteousMight.Reference.Get().Icon;

            var buff = BuffConfigurator.New(MightyStrikeBuff, MightyStrikeBuffGuid)
                .SetDisplayName(MightyStrikeDisplayName)
                .SetDescription(MightyStrikeDescription)
                .SetIcon(icon)
                .AddComponent<HeritorMightyStrike>()
                .Configure();

            var applyAdvancedBuff = ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true);

            return FeatureConfigurator.New(MightyStrikeName, MightyStrikeGuid)
                    .SetDisplayName(MightyStrikeDisplayName)
                    .SetDescription(MightyStrikeDescription)
                    .SetIcon(icon)
                    .AddAbilityUseTrigger(ability: AbilityRefs.CleaveAction.ToString(), action: applyAdvancedBuff, actionsOnTarget: false)
                    .AddAbilityUseTrigger(ability: RedeemerUndeathAbilityGuid, action: applyAdvancedBuff, actionsOnTarget: false)
                    .AddFacts(new() { FeatureRefs.VitalStrikeFeature.ToString(), FeatureRefs.VitalStrikeFeatureImproved.ToString() })
                    .Configure();
        }

        private static readonly string RedeemerUndeathName = "HeritorKnightRedeemerUndeath";
        public static readonly string RedeemerUndeathGuid = "{46F6D5FF-95A0-4699-9E4C-BB52EE629455}";

        private static readonly string RedeemerUndeathDisplayName = "HeritorKnightRedeemerUndeath.Name";
        private static readonly string RedeemerUndeathDescription = "HeritorKnightRedeemerUndeath.Description";

        private const string RedeemerUndeathAbility = "HeritorKnight.RedeemerUndeathAbility";
        private static readonly string RedeemerUndeathAbilityGuid = "{59DA3FA3-4506-48FD-95CA-E4267EBB932E}";

        private const string RedeemerUndeathBuff = "HeritorKnight.RedeemerUndeathBuff";
        private static readonly string RedeemerUndeathBuffGuid = "{FD1F9EF2-AFE1-49FA-8886-29D705679A47}";
        public static BlueprintFeature RedeemerUndeathConfigure()
        {
            var icon = AbilityRefs.UndeathToDeath.Reference.Get().Icon;

            var buff1 = BuffConfigurator.New(RedeemerUndeathBuff, RedeemerUndeathBuffGuid)
                .SetDisplayName(RedeemerUndeathDisplayName)
                .SetDescription(RedeemerUndeathDescription)
                .SetIcon(icon)
                .Configure();

            var shoot = ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Will, useDCFromContextSavingThrow: true,
            onResult: ActionsBuilder.New()
                        .ConditionalSaved(failed: ActionsBuilder.New().Kill(dismember: Kingmaker.UnitLogic.UnitState.DismemberType.InPower).Build(),
                                    succeed: ActionsBuilder.New().ApplyBuff(buff1, ContextDuration.Fixed(1, DurationRate.Days)).Build())
                        .Build())
                    .Build();

            var action2 = ActionsBuilder.New()
                            .Conditional(ConditionsBuilder.New().HasFact(buff1).Build(), ifFalse: shoot)
                            .Build();

            var action = ActionsBuilder.New()
                            .Conditional(ConditionsBuilder.New().HasFact(FeatureRefs.UndeadType.ToString()).Build(), ifTrue: action2)
                            .Build();

            var ability = AbilityConfigurator.New(RedeemerUndeathAbility, RedeemerUndeathAbilityGuid)
                .SetDisplayName(RedeemerUndeathDisplayName)
                .SetDescription(RedeemerUndeathDescription)
                .SetIcon(icon)
                .AllowTargeting(false, true, false, false)
                .AddAbilityEffectRunAction(ActionsBuilder.New().Add<MeleeAttackExtended>(c => { c.OnHit = action; }).Build())
                .AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Charisma; })
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Weapon)
                .AddAbilityCasterMainWeaponCheck(WeaponCategory.Longsword)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .AddHideDCFromTooltip()
                .Configure();

            return FeatureConfigurator.New(RedeemerUndeathName, RedeemerUndeathGuid)
                    .SetDisplayName(RedeemerUndeathDisplayName)
                    .SetDescription(RedeemerUndeathDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private const string ImageDivinity = "HeritorKnight.ImageDivinity";
        public static readonly string ImageDivinityGuid = "{F02BEFC0-7A21-4F9E-BD73-7A72B0E12E7F}";
        internal const string ImageDivinityDisplayName = "HeritorKnightImageDivinity.Name";
        private const string ImageDivinityDescription = "HeritorKnightImageDivinity.Description";

        private const string ImageDivinityAblity2 = "HeritorKnight.UseImageDivinity2";
        private static readonly string ImageDivinityAblity2Guid = "{333041CE-09E6-48F2-8039-8731BB66CE47}";

        private const string ImageDivinityAblityRes = "HeritorKnight.ImageDivinityRes";
        private static readonly string ImageDivinityAblityResGuid = "{D8C6D998-1FBE-4EB5-AA60-D852E9767619}";

        internal const string ImageDivinityDisplayName2 = "HeritorKnightImageDivinity2.Name";
        private const string ImageDivinityDescription2 = "HeritorKnightImageDivinity2.Description";
        private static BlueprintFeature CreateImageDivinity()
        {
            var icon = AbilityRefs.AngelicAspectGreater.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(ImageDivinityAblityRes, ImageDivinityAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(3))
                .Configure();

            var selfheal = ActionsBuilder.New()
                .HealTarget(value: ContextDice.Value(DiceType.D6, ContextValues.Rank()))
                .CastSpell(AbilityRefs.Restoration.ToString(), overrideSpellLevel: 4)
                .Build();

            var selfdamage = ActionsBuilder.New()
                .DealDamage(value: ContextDice.Value(DiceType.D6, ContextValues.Rank()), damageType: DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Divine))
                .SavingThrow(type: SavingThrowType.Fortitude, useDCFromContextSavingThrow: true,
                onResult: ActionsBuilder.New()
                            .ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Staggered.ToString(), ContextDuration.Variable(ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true))).Build())
                            .Build())
                .Build();

            var ability2 = AbilityConfigurator.New(ImageDivinityAblity2, ImageDivinityAblity2Guid)
                .AllowTargeting(false, true, true, true)
                .AddAbilityDeliverTouch(false, null, ComponentMerge.Fail, ItemWeaponRefs.TouchItem.ToString())
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().Alignment(AlignmentComponent.Good).Build(), ifTrue: selfheal)
                    .Conditional(ConditionsBuilder.New().Alignment(AlignmentComponent.Evil).Build(), ifTrue: selfdamage)
                    .Build())
                .SetDisplayName(ImageDivinityDisplayName2)
                .SetDescription(ImageDivinityDescription2)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Touch)
                .SetType(AbilityType.Supernatural)
                .AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Charisma; })
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }))
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
                .AddHideDCFromTooltip()
                .Configure();

            return FeatureConfigurator.New(ImageDivinity, ImageDivinityGuid)
              .SetDisplayName(ImageDivinityDisplayName)
              .SetDescription(ImageDivinityDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddComponent<ChangePortrait>(c => { c.Portrait = UnitRefs.Iomedae.Reference.Get().PortraitSafe; })
              .AddReplaceUnitPrefab(prefab: UnitRefs.Iomedae.Reference.Get().Prefab.AssetId)
              .Configure();
        }
    }
}
