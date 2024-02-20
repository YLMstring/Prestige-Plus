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
                .AddToLevelEntry(1, EnchantingTouchFeature(), SkillHeartFeat())
                .AddToLevelEntry(2, HeritorHonorFeature(), CreateWitchesWoe())
                .AddToLevelEntry(3, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
                .AddToLevelEntry(4, CreateWraithwall())
                .AddToLevelEntry(5)
                .AddToLevelEntry(6, MightyStrikeConfigure())
                .AddToLevelEntry(7)
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

            DeludingTouchFeature();

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
            return FeatureConfigurator.New(AsOne, AsOneGuid)
              .SetDisplayName(SkillHeartDisplayName)
              .SetDescription(SkillHeartDescription)
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.FighterClass.ToString(), modifier: 1, summand: 0)
              .Configure();
        }

        private const string HeritorHonor = "HeritorKnightHeritorHonor";
        private static readonly string HeritorHonorGuid = "{DB369DA9-FF1F-42D6-AE31-ACDE9F644FDE}";

        internal const string HeritorHonorDisplayName = "HeritorKnightHeritorHonor.Name";
        private const string HeritorHonorDescription = "HeritorKnightHeritorHonor.Description";
        public static BlueprintFeature HeritorHonorFeature()
        {
            var icon = FeatureRefs.DivineGrace.Reference.Get().Icon;
            return FeatureConfigurator.New(HeritorHonor, HeritorHonorGuid)
              .SetDisplayName(HeritorHonorDisplayName)
              .SetDescription(HeritorHonorDescription)
              .SetIcon(icon)
              .AddDerivativeStatBonus(StatType.Charisma, StatType.SaveWill)
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
            var icon = FeatureRefs.OathOfTheSkyFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(SkyStride, SkyStrideGuid)
              .SetDisplayName(SkyStrideDisplayName)
              .SetDescription(SkyStrideDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.WingsFeature.ToString() })
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
            var icon = AbilityRefs.UndeathToDeath.Reference.Get().Icon;

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
            var icon = AbilityRefs.BloodBoilerAbility.Reference.Get().Icon;

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
                .Configure();

            return FeatureConfigurator.New(FreedBlood, FreedBloodGuid)
              .SetDisplayName(FreedBloodDisplayName)
              .SetDescription(FreedBloodDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string HiddenMind = "HeritorKnightHiddenMind";
        public static readonly string HiddenMindGuid = "{01661653-5877-4A6B-B4DF-EE8D9C26AD47}";

        internal const string HiddenMindDisplayName = "HeritorKnightHiddenMind.Name";
        private const string HiddenMindDescription = "HeritorKnightHiddenMind.Description";
        public static BlueprintFeature HiddenMindFeature()
        {
            var icon = AbilityRefs.MindBlank.Reference.Get().Icon;
            return FeatureConfigurator.New(HiddenMind, HiddenMindGuid)
              .SetDisplayName(HiddenMindDisplayName)
              .SetDescription(HiddenMindDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(8, modifierDescriptor: ModifierDescriptor.Resistance, spellDescriptor: SpellDescriptor.MindAffecting)
              .AddFacts(new() { FeatureRefs.DivinationImmunityFeature.ToString() })
              .Configure();
        }

        private const string EnchantingTouch = "HeritorKnightEnchantingTouch";
        public static readonly string EnchantingTouchGuid = "{EBB40FD1-AB07-4894-B691-4E85CB90DC96}";

        internal const string EnchantingTouchDisplayName = "HeritorKnightEnchantingTouch.Name";
        private const string EnchantingTouchDescription = "HeritorKnightEnchantingTouch.Description";
        public static BlueprintFeature EnchantingTouchFeature()
        {
            var icon = AbilityRefs.HolyWhisper.Reference.Get().Icon;
            return FeatureConfigurator.New(EnchantingTouch, EnchantingTouchGuid)
              .SetDisplayName(EnchantingTouchDisplayName)
              .SetDescription(EnchantingTouchDescription)
              .SetIcon(icon)
              .AddComponent<EnchantingTouchComp>()
              .Configure();
        }

        private const string DeludingTouch = "HeritorKnightDeludingTouch";
        public static readonly string DeludingTouchGuid = "{604769DB-A407-4F43-87E9-066B66FAB9F7}";

        internal const string DeludingTouchDisplayName = "HeritorKnightDeludingTouch.Name";
        private const string DeludingTouchDescription = "HeritorKnightDeludingTouch.Description";
        public static BlueprintFeature DeludingTouchFeature()
        {
            var icon = AbilityRefs.HolyWhisper.Reference.Get().Icon;
            return FeatureConfigurator.New(DeludingTouch, DeludingTouchGuid, FeatureGroup.MythicAbility)
              .SetDisplayName(DeludingTouchDisplayName)
              .SetDescription(DeludingTouchDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(EnchantingTouchGuid)
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

            var applyAdvancedBuff = ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1));

            return FeatureConfigurator.New(MightyStrikeName, MightyStrikeGuid)
                    .SetDisplayName(MightyStrikeDisplayName)
                    .SetDescription(MightyStrikeDescription)
                    .SetIcon(icon)
                    .AddAbilityUseTrigger(ability: AbilityRefs.CleaveAction.ToString(), action: applyAdvancedBuff, actionsOnTarget: false)
                    .AddFacts(new() { FeatureRefs.VitalStrikeFeature.ToString(), FeatureRefs.VitalStrikeFeatureImproved.ToString() })
                    .Configure();
        }

        private static readonly string EcstasyName = "HeritorKnightEcstasy";
        public static readonly string EcstasyGuid = "{49AC4C66-95CD-45AA-B923-0CE563C2CFB5}";

        private static readonly string EcstasyDisplayName = "HeritorKnightEcstasy.Name";
        private static readonly string EcstasyDescription = "HeritorKnightEcstasy.Description";

        private static readonly string Ecstasy1DisplayName = "HeritorKnightEcstasy1.Name";
        private static readonly string Ecstasy1Description = "HeritorKnightEcstasy1.Description";

        private static readonly string Ecstasy2DisplayName = "HeritorKnightEcstasy2.Name";
        private static readonly string Ecstasy2Description = "HeritorKnightEcstasy2.Description";

        private const string EcstasyAbility = "HeritorKnight.EcstasyAbility";
        private static readonly string EcstasyAbilityGuid = "{19DA9656-42E1-4572-98F5-722060B5ECB9}";

        private const string EcstasyBuff = "HeritorKnight.EcstasyBuff";
        private static readonly string EcstasyBuffGuid = "{ED5C140F-9C2C-4119-8427-AFC5C7FD3675}";

        private const string EcstasyBuff2 = "HeritorKnight.EcstasyBuff2";
        private static readonly string EcstasyBuff2Guid = "{1C21BC0A-7FE4-4630-81AB-F6F766E4A666}";
        public static BlueprintFeature EcstasyConfigure()
        {
            var icon = AbilityRefs.WavesOfEctasy.Reference.Get().Icon;

            var buff1 = BuffConfigurator.New(EcstasyBuff, EcstasyBuffGuid)
                .SetDisplayName(Ecstasy1DisplayName)
                .SetDescription(Ecstasy1Description)
                .SetIcon(icon)
                .AddComponent<SuppressBuffs>(c => { c.Descriptor = SpellDescriptor.NegativeEmotion; })
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion)
                .AddFacts(new() { BuffRefs.Stunned.ToString() })
                .Configure();

            var buff2 = BuffConfigurator.New(EcstasyBuff2, EcstasyBuff2Guid)
                .SetDisplayName(Ecstasy2DisplayName)
                .SetDescription(Ecstasy2Description)
                .SetIcon(icon)
                .AddComponent<SuppressBuffs>(c => { c.Descriptor = SpellDescriptor.NegativeEmotion; })
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion)
                .AddFacts(new() { BuffRefs.Staggered.ToString() })
                .Configure();

            var shoot = ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Fortitude, customDC: ContextValues.Rank(), useDCFromContextSavingThrow: true,
            onResult: ActionsBuilder.New()
                        .ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(buff1, ContextDuration.FixedDice(DiceType.D4)).Build(),
                                    succeed: ActionsBuilder.New().ApplyBuff(buff2, ContextDuration.FixedDice(DiceType.D4)).Build())
                        .Build())
                    .Build();

            var ability = AbilityConfigurator.New(EcstasyAbility, EcstasyAbilityGuid)
                .SetDisplayName(EcstasyDisplayName)
                .SetDescription(EcstasyDescription)
                .SetIcon(icon)
                .AddComponent<AbilityTargetMayPrecise>()
                .AllowTargeting(false, true, true, true)
                .AddAbilityDeliverTouch(false, null, ComponentMerge.Fail, ItemWeaponRefs.TouchItem.ToString())
                .AddAbilityEffectRunAction(shoot)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(stat: StatType.Wisdom).WithBonusValueProgression(20, false))
                .AddHideDCFromTooltip()
                .SetType(AbilityType.Extraordinary)
                .SetRange(AbilityRange.Touch)
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion)
                .Configure();

            return FeatureConfigurator.New(EcstasyName, EcstasyGuid)
                    .SetDisplayName(EcstasyDisplayName)
                    .SetDescription(EcstasyDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
