using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.Enums;
using PrestigePlus.Modify;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Utility;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using static Kingmaker.Blueprints.Area.FactHolder;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Abilities;
using static Dreamteck.Splines.SplineMesh;
using System.IO.Ports;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.Blueprints.Classes.Prerequisites;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class InheritorCrusader
    {
        private const string ArchetypeName = "InheritorCrusader";
        public static readonly string ArchetypeGuid = "{772F4078-A60A-4D0F-B06C-61E56C4688D7}";
        internal const string ArchetypeDisplayName = "InheritorCrusader.Name";
        private const string ArchetypeDescription = "InheritorCrusader.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "InheritorCrusaderPrestige";
        private static readonly string ClassProgressGuid = "{812439A7-D49F-4335-9D2E-529E43630964}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, ICspellupgrade(), ICChampionHonor(), ICChooseAura(), FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString())
                .AddToLevelEntry(2, spellupgradeGuid, ICDestroyTyranny())
                .AddToLevelEntry(3, spellupgradeGuid, CreateSwordAgainstInjustice())
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
                .SetBaseAttackBonus(BABFull)
                .SetFortitudeSave(SavesPrestigeHigh)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillLoreReligion, StatType.SkillKnowledgeWorld, StatType.SkillPersuasion, StatType.SkillPerception })
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 5)
                .AddPrerequisiteStatValue(StatType.SkillPerception, 5)
                .AddPrerequisiteProficiency(weaponProficiencies: new WeaponCategory[] { WeaponCategory.Longsword }, armorProficiencies: new ArmorProficiencyGroup[] { })
                .AddPrerequisiteFeature(FeatureRefs.IronWill.ToString(), group: Prerequisite.GroupType.All)
                .AddPrerequisiteAlignment(AlignmentMaskType.Lawful, checkInProgression: true)
                .AddPrerequisiteClassLevel(ArchetypeGuid, 3, not: true)
                .AddPrerequisiteFeature(FeatureRefs.ChannelEnergyFeature.ToString(), group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.ChannelEnergyHospitalerFeature.ToString(), group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.ChannelEnergyPaladinFeature.ToString(), group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.ChannelEnergyEmpyrealFeature.ToString(), group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.ShamanLifeSpiritChannelEnergyFeature.ToString(), group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.OracleRevelationChannelFeature.ToString(), group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.WarpriestChannelEnergyFeature.ToString(), group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.HexChannelerChannelPositiveFeature.ToString(), group: Prerequisite.GroupType.Any)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var InheritorCrusaderref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = InheritorCrusaderref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string spellupgrade = "InheritorCrusader.spellupgrade";
        private static readonly string spellupgradeGuid = "{8D91B2D5-5DFE-4D76-912B-7DBD46EF2A97}";

        internal const string InheritorCrusaderspellupgradeDisplayName = "InheritorCrusaderspellupgrade.Name";
        private const string InheritorCrusaderspellupgradeDescription = "InheritorCrusaderspellupgrade.Description";
        public static BlueprintFeatureSelection ICspellupgrade()
        {
            return FeatureSelectionConfigurator.New(spellupgrade, spellupgradeGuid)
              .SetDisplayName(InheritorCrusaderspellupgradeDisplayName)
              .SetDescription(InheritorCrusaderspellupgradeDescription)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(SpellbookLevelUp.AngelfireApostleGuid)
              .AddToAllFeatures(SpellbookLevelUp.ClericGuid)
              .AddToAllFeatures(SpellbookLevelUp.CrusaderGuid)
              .AddToAllFeatures(SpellbookLevelUp.PaladinGuid)
              .AddToAllFeatures(SpellbookLevelUp.WarpriestGuid)
              .Configure();
        }

        private const string ChampionHonor = "InheritorCrusader.ChampionHonor";
        private static readonly string ChampionHonorGuid = "{568439A6-B542-4A2C-BB6D-E3E885C3044B}";

        internal const string InheritorCrusaderChampionHonorDisplayName = "InheritorCrusaderChampionHonor.Name";
        private const string InheritorCrusaderChampionHonorDescription = "InheritorCrusaderChampionHonor.Description";
        public static BlueprintFeature ICChampionHonor()
        {
            var icon = AbilityRefs.SmiteEvilAbility.Reference.Get().Icon;
            return FeatureConfigurator.New(ChampionHonor, ChampionHonorGuid)
              .SetDisplayName(InheritorCrusaderChampionHonorDisplayName)
              .SetDescription(InheritorCrusaderChampionHonorDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.SmiteEvilAdditionalUse.ToString() })
              .AddDamageBonusAgainstFactOwner(bonus: ContextValues.Rank(), checkedFact: BuffRefs.SmiteEvilBuff.ToString())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }))
              .Configure();
        }

        private const string GreaterAura = "InheritorCrusader.GreaterAura";
        private static readonly string GreaterAuraGuid = "{F19E0F66-A991-4735-920D-00A2E281B274}";

        private const string ChooseAura = "InheritorCrusader.ChooseAura";
        private static readonly string ChooseAuraGuid = "{72D003C2-0FD8-451F-B49B-27A538617F0A}";

        private const string AuraBuff = "InheritorCrusader.AuraBuff";
        private static readonly string AuraGuidBuff = "{EC942BEA-EA00-4CD5-A69B-C98E4732B374}";

        private const string CourageAura = "InheritorCrusader.CourageAura";
        private static readonly string CourageAuraGuid = "{1A77BEA1-3387-4243-A5A0-80E0A4BC064C}";

        internal const string InheritorCrusaderGreaterAuraDisplayName = "InheritorCrusaderGreaterAura.Name";
        private const string InheritorCrusaderGreaterAuraDescription = "InheritorCrusaderGreaterAura.Description";
        public static BlueprintFeature ICGreaterAura()
        {
            var icon = AbilityRefs.AuraOfGreaterCourage.Reference.Get().Icon;

            var area = AbilityAreaEffectConfigurator.New(CourageAura, CourageAuraGuid)
                .CopyFrom(AbilityAreaEffectRefs.AuraOfCourageArea, typeof(AbilityAreaEffectBuff))
                .SetSize(23.Feet())
                .Configure();

            var Buff1 = BuffConfigurator.New(AuraBuff, AuraGuidBuff)
              .SetDisplayName(InheritorCrusaderGreaterAuraDisplayName)
              .SetDescription(InheritorCrusaderGreaterAuraDescription)
              .SetIcon(icon)
              .AddAreaEffect(area)
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            return FeatureConfigurator.New(GreaterAura, GreaterAuraGuid)
              .SetDisplayName(InheritorCrusaderGreaterAuraDisplayName)
              .SetDescription(InheritorCrusaderGreaterAuraDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.AuraOfCourageFeature.ToString())
              .AddAuraFeatureComponent(Buff1)
              .Configure();
        }

        public static BlueprintFeatureSelection ICChooseAura()
        {
            var icon = FeatureRefs.AuraOfCourageFeature.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ChooseAura, ChooseAuraGuid)
              .SetDisplayName(InheritorCrusaderGreaterAuraDisplayName)
              .SetDescription(InheritorCrusaderGreaterAuraDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.AuraOfCourageFeature.ToString())
              .AddToAllFeatures(ICGreaterAura())
              .Configure();
        }

        private const string DestroyTyranny = "InheritorCrusader.DestroyTyranny";
        private static readonly string DestroyTyrannyGuid = "{31406035-968C-470F-A5FA-539C77082A78}";

        internal const string DestroyTyrannyDisplayName = "DestroyTyranny.Name";
        private const string DestroyTyrannyDescription = "DestroyTyranny.Description";

        public static BlueprintFeature ICDestroyTyranny()
        {
            var icon = AbilityRefs.BreakEnchantment.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(DestroyTyranny, DestroyTyrannyGuid)
              .SetDisplayName(DestroyTyrannyDisplayName)
              .SetDescription(DestroyTyrannyDescription)
              .SetIcon(icon)
              .Configure();

            foreach (var bp in SpellbookLevelUp.ChannelPositiveHeal)
                AddPurifyToChannel(bp.Get());

            AddPurifyToChannel(AbilityRefs.LayOnHandsSelf.Reference.Get());
            AddPurifyToChannel(AbilityRefs.LayOnHandsSelfOrTroth.Reference.Get());
            AddPurifyToChannel(AbilityRefs.LayOnHandsOthers.Reference.Get());
            AddPurifyToChannel(AbilityRefs.WarpriestFervorPositiveAbility.Reference.Get());
            AddPurifyToChannel(AbilityRefs.WarpriestFervorPositiveAbilitySelf.Reference.Get());
            return feat;
        }

        private static void AddPurifyToChannel(BlueprintAbility channel)
        {
            if (channel == null)
                return;

            var purify = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(DestroyTyrannyGuid).Build(), ifTrue: ActionsBuilder.New()
                    .DispelMagic(buffType: Kingmaker.UnitLogic.Mechanics.Actions.ContextActionDispelMagic.BuffType.All, checkType: Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.CasterLevel, maxSpellLevel: ContextValues.Constant(9), checkSchoolOrDescriptor: false, descriptor: SpellDescriptor.Charm)
                    .DispelMagic(buffType: Kingmaker.UnitLogic.Mechanics.Actions.ContextActionDispelMagic.BuffType.All, checkType: Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.CasterLevel, maxSpellLevel: ContextValues.Constant(9), checkSchoolOrDescriptor: false, descriptor: SpellDescriptor.Compulsion)
                    .Build())
                .Build();

            AbilityConfigurator.For(channel)
              .EditComponent<AbilityEffectRunAction>(
                a => a.Actions.Actions = CommonTool.Append(a.Actions.Actions, purify.Actions))
              .Configure();
        }

        private const string SwordAgainstInjustice = "InheritorCrusader.SwordAgainstInjustice";
        private static readonly string SwordAgainstInjusticeGuid = "{F9324DC1-0901-4E1F-85EA-1C149118CD26}";
        internal const string SwordAgainstInjusticeDisplayName = "InheritorCrusaderSwordAgainstInjustice.Name";
        private const string SwordAgainstInjusticeDescription = "InheritorCrusaderSwordAgainstInjustice.Description";

        private const string SwordAgainstInjusticeAblity = "InheritorCrusader.UseSwordAgainstInjustice";
        private static readonly string SwordAgainstInjusticeAblityGuid = "{74683258-6DDC-4049-8AC3-CA36F4DA187B}";

        private const string SwordAgainstInjusticeAblity2 = "InheritorCrusader.UseSwordAgainstInjustice2";
        private static readonly string SwordAgainstInjusticeAblityGuid2 = "{A8EB847D-4B61-4F56-8BFA-DEFF61AA8E31}";

        private const string SwordAgainstInjusticeAblity3 = "InheritorCrusader.UseSwordAgainstInjustice3";
        private static readonly string SwordAgainstInjusticeAblityGuid3 = "{BAAC6749-05BF-4654-A2D3-C81807BA3CCE}";

        private const string SwordAgainstInjusticeAblity4 = "InheritorCrusader.UseSwordAgainstInjustice4";
        private static readonly string SwordAgainstInjusticeAblityGuid4 = "{60C4A7E6-8866-4C00-9E1B-931BBE87613E}";

        private const string SwordAgainstInjusticeAblityRes = "InheritorCrusader.SwordAgainstInjusticeRes";
        private static readonly string SwordAgainstInjusticeAblityResGuid = "{C152D07C-EE59-4B7F-9FFF-2D729D011336}";

        private static BlueprintFeature CreateSwordAgainstInjustice()
        {
            var icon = AbilityRefs.AngelArmyOfHeaven.Reference.Get().Icon;

            var sword = ActionsBuilder.New()
                .MeleeAttack(autoHit: true, extraAttack: true)
                .Build();

            var sword2 = ActionsBuilder.New()
                .MeleeAttack(autoHit: true, extraAttack: true)
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(BuffRefs.Fatigued.ToString()).Build(), ifTrue: ActionsBuilder.New()
                    .ApplyBuffPermanent(buff: BuffRefs.Exhausted.ToString(), toCaster: true)
                    .Build())
                .ApplyBuffPermanent(buff: BuffRefs.Fatigued.ToString(), toCaster: true)
                .Build();

            var abilityresourse = AbilityResourceConfigurator.New(SwordAgainstInjusticeAblityRes, SwordAgainstInjusticeAblityResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .SetUseMax()
                .SetMax(1)
                .Configure();

            var ability1perday = AbilityConfigurator.New(SwordAgainstInjusticeAblity, SwordAgainstInjusticeAblityGuid)
                .AddAbilityEffectRunAction(sword)
                .SetType(AbilityType.Physical)
                .SetDisplayName(SwordAgainstInjusticeDisplayName)
                .SetDescription(SwordAgainstInjusticeDescription)
                .AddInPowerDismemberComponent()
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace)
                .SetRange(AbilityRange.Weapon)
                .AddAbilityRequirementHasItemInHands(type: AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
                .AddAbilityRequirementHasCondition(conditions: new[] { UnitCondition.Exhausted }, not: true)
                .AddAbilityTargetHasFact(new() { BuffRefs.MindBlankBuff.ToString() }, inverted: true)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: SwordAgainstInjusticeAblityResGuid)
                .Configure();

            var ability1paladin = AbilityConfigurator.New(SwordAgainstInjusticeAblity3, SwordAgainstInjusticeAblityGuid3)
                .AddAbilityEffectRunAction(sword)
                .SetType(AbilityType.Physical)
                .SetDisplayName(SwordAgainstInjusticeDisplayName)
                .SetDescription(SwordAgainstInjusticeDescription)
                .AddInPowerDismemberComponent()
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace)
                .SetRange(AbilityRange.Weapon)
                .AddAbilityRequirementHasItemInHands(type: AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
                .AddAbilityRequirementHasCondition(conditions: new[] { UnitCondition.Exhausted }, not: true)
                .AddAbilityTargetHasFact(new() { BuffRefs.MindBlankBuff.ToString() }, inverted: true)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.LayOnHandsResource.ToString())
                .Configure();

            var ability1warpriest = AbilityConfigurator.New(SwordAgainstInjusticeAblity4, SwordAgainstInjusticeAblityGuid4)
                .AddAbilityEffectRunAction(sword)
                .SetType(AbilityType.Physical)
                .SetDisplayName(SwordAgainstInjusticeDisplayName)
                .SetDescription(SwordAgainstInjusticeDescription)
                .AddInPowerDismemberComponent()
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace)
                .SetRange(AbilityRange.Weapon)
                .AddAbilityRequirementHasItemInHands(type: AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
                .AddAbilityRequirementHasCondition(conditions: new[] { UnitCondition.Exhausted }, not: true)
                .AddAbilityTargetHasFact(new() { BuffRefs.MindBlankBuff.ToString() }, inverted: true)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.WarpriestFervorResource.ToString())
                .Configure();

            var abilityunlimited = AbilityConfigurator.New(SwordAgainstInjusticeAblity2, SwordAgainstInjusticeAblityGuid2)
                .AddAbilityEffectRunAction(sword2)
                .SetType(AbilityType.Physical)
                .SetDisplayName(SwordAgainstInjusticeDisplayName)
                .SetDescription(SwordAgainstInjusticeDescription)
                .AddInPowerDismemberComponent()
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace)
                .SetRange(AbilityRange.Weapon)
                .AddAbilityRequirementHasItemInHands(type: AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
                .AddAbilityRequirementHasCondition(conditions: new[] { UnitCondition.Exhausted }, not: true)
                .AddAbilityTargetHasFact(new() { BuffRefs.MindBlankBuff.ToString() }, inverted: true)
                .Configure();

            return FeatureConfigurator.New(SwordAgainstInjustice, SwordAgainstInjusticeGuid)
              .SetDisplayName(SwordAgainstInjusticeDisplayName)
              .SetDescription(SwordAgainstInjusticeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability1perday, abilityunlimited, ability1paladin, ability1warpriest })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }
    }
}

