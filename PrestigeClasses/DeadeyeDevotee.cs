using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using PrestigePlus.Feats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Actions.Builder.ContextEx;
using PrestigePlus.Modify;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Designers.Mechanics.Buffs;

namespace PrestigePlus.PrestigeClasses
{
    internal class DeadeyeDevotee
    {
        private const string ArchetypeName = "DeadeyeDevotee";
        public static readonly string ArchetypeGuid = "{41E63096-56F9-499A-B8B5-FC0B20E03F20}";
        internal const string ArchetypeDisplayName = "DeadeyeDevotee.Name";
        private const string ArchetypeDescription = "DeadeyeDevotee.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "DeadeyeDevoteePrestige";
        private static readonly string ClassProgressGuid = "{94E31306-2ADD-440F-820F-24170E58609F}";

        private static readonly string ProficienciesGuid = "{3486544B-AD52-436A-A0BE-E5B8C70FDE47}";
        private static readonly string EnhanceArrows1Guid = "{6D9BF4E9-4370-49B8-A510-1E0DD286CE52}";
        private static readonly string EnhanceArrows2Guid = "{E264D79C-1965-4EBD-8093-EAC731E7067F}";
        private static readonly string EnhanceArrows3Guid = "{D59B7809-D002-414D-A09C-0374CD858A18}";
        private static readonly string EnhanceArrows4Guid = "{1DA01231-4001-46F3-8A31-E748AE8DFBF3}";
        private static readonly string PhaseArrowGuid = "{247B5788-5D3A-41D3-914C-1BC3DE1B9114}";
        private static readonly string HailArrowGuid = "{188337A9-939D-4AF2-9030-34C0AD102F46}";
        private static readonly string DeathArrowGuid = "{8656CF6B-810D-4493-8898-51991ABC73DE}";
        public static void Configure()
        {
            string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";

            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, ProficienciesGuid, spellupgradeGuid, EnhanceArrows1Guid)
                .AddToLevelEntry(2, ImbueArrow.FeatGuid, FeatureRefs.ReachSpellFeat.ToString())
                .AddToLevelEntry(3, EnhanceArrows2Guid)
                .AddToLevelEntry(4, CreateEnergyArrow())
                .AddToLevelEntry(5, EnhanceArrows4Guid)
                .AddToLevelEntry(6, PhaseArrowGuid)
                .AddToLevelEntry(7, EnhanceArrows2Guid)
                .AddToLevelEntry(8, HailArrowGuid)
                .AddToLevelEntry(9, EnhanceArrows3Guid)
                .AddToLevelEntry(10, DeathArrowGuid)
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { EnhanceArrows1Guid, EnhanceArrows2Guid, EnhanceArrows3Guid, EnhanceArrows4Guid }))
                ///.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { EnergyArrowGuid, PhaseArrowGuid, HailArrowGuid, DeathArrowGuid }))
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
                .AddSkipLevelsForSpellProgression(new int[] { 5, 9 })
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillLoreReligion, StatType.SkillLoreNature, StatType.SkillPerception, StatType.SkillStealth })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                .AddPrerequisiteFeature(FeatureRefs.ErastilFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.GreenFaithFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.GreenFaithCameliaFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.PreciseShot.ToString())
                .AddPrerequisiteParametrizedWeaponFeature(ParametrizedFeatureRefs.WeaponFocus.ToString(), WeaponCategory.Longbow)
                .AddPrerequisiteCasterTypeSpellLevel(false, false, 1)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var DeadeyeDevoteeref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = DeadeyeDevoteeref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string EnergyArrow = "DeadeyeDevotee.EnergyArrow";
        private static readonly string EnergyArrowGuid = "{1ADBDA73-9FD1-44EB-B85C-AC5E74B1D968}";
        internal const string EnergyArrowDisplayName = "DeadeyeDevoteeEnergyArrow.Name";
        private const string EnergyArrowDescription = "DeadeyeDevoteeEnergyArrow.Description";

        private const string EnergyArrowAblity = "DeadeyeDevotee.UseEnergyArrow";
        private static readonly string EnergyArrowAblityGuid = "{7FF90659-2D53-4E36-BD34-934FEC5ABFE2}";

        private const string EnergyArrowBuff = "DeadeyeDevotee.EnergyArrowBuff";
        private static readonly string EnergyArrowBuffGuid = "{77304B28-00D0-4AA8-A195-6394C6AC2729}";

        private const string EnergyArrowBuff2 = "DeadeyeDevotee.EnergyArrowBuff2";
        private static readonly string EnergyArrowBuff2Guid = "{58E2FD86-EEE5-44FC-BA49-31E142997391}";

        private const string EnergyArrowBuff3 = "DeadeyeDevotee.EnergyArrowBuff3";
        private static readonly string EnergyArrowBuff3Guid = "{9531A1A7-8F1B-4BF3-BC24-28D85C26E309}";

        private static BlueprintFeature CreateEnergyArrow()
        {
            var icon = AbilityRefs.ArrowOfLaw.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(EnergyArrowBuff, EnergyArrowBuffGuid)
             .SetDisplayName(EnergyArrowDisplayName)
             .SetDescription(EnergyArrowDescription)
             .SetIcon(icon)
             .AddComponent<EnergyArrow>()
             .Configure();

            var Buff2 = BuffConfigurator.New(EnergyArrowBuff2, EnergyArrowBuff2Guid)
             .SetDisplayName(EnergyArrowDisplayName)
             .SetDescription(EnergyArrowDescription)
             .SetIcon(icon)
             .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RemoveBuff(Buff1).Build(), actionsOnInitiator: false, triggerBeforeAttack: false)
             .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RemoveSelf().Build(), actionsOnInitiator: true, triggerBeforeAttack: false)
             .Configure();

            var Buff3 = BuffConfigurator.New(EnergyArrowBuff3, EnergyArrowBuff3Guid)
             .SetDisplayName(EnergyArrowDisplayName)
             .SetDescription(EnergyArrowDescription)
             .SetIcon(icon)
             .Configure();

            var shoot = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(Buff3).Build()
                    ,ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Fixed(1), toCaster: false)
                        .ApplyBuff(Buff2, ContextDuration.Fixed(1), toCaster: true)
                        .RangedAttack(autoHit: true, extraAttack: false)
                        .RemoveBuff(Buff3, toCaster: true)
                        .Build())
                .Build();

            var action = ActionsBuilder.New()
                .ApplyBuff(Buff3, ContextDuration.Fixed(1), toCaster: true)
                .Build();

            var ability = AbilityConfigurator.New(EnergyArrowAblity, EnergyArrowAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(EnergyArrowDisplayName)
                .SetDescription(EnergyArrowDescription)
                .SetIcon(icon)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .SetCanTargetSelf(false)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.Weapon)
                .Configure();

            return FeatureConfigurator.New(EnergyArrow, EnergyArrowGuid)
              .SetDisplayName(EnergyArrowDisplayName)
              .SetDescription(EnergyArrowDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityUseTrigger(abilities: SpellbookLevelUp.CureOrInflict, action: action, checkAbilityType: true, type: AbilityType.Spell, forMultipleSpells: true)
              .AddAutoMetamagic(abilities: SpellbookLevelUp.CureOrInflict, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.SpellOnly, checkSpellbook: false, metamagic: Kingmaker.UnitLogic.Abilities.Metamagic.Reach, once: false)
              .Configure();
        }
    }
}
