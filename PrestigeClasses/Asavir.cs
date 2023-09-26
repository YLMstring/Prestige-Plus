using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.RuleSystem;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Utils;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic;
using BlueprintCore.Blueprints.CustomConfigurators;
using Pathfinding.Voxels;

namespace PrestigePlus.PrestigeClasses
{
    internal class Asavir
    {
        private const string ArchetypeName = "Asavir";
        public static readonly string ArchetypeGuid = "{A29AE95E-ADB4-469D-A996-FA9B913165CC}";
        internal const string ArchetypeDisplayName = "Asavir.Name";
        private const string ArchetypeDescription = "Asavir.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "AsavirPrestige";
        private static readonly string ClassProgressGuid = "{C45A4085-D218-439A-8695-BCB2D8CBCB14}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, EquineBondFeat(), SummonAnimalSelection(), FeatureRefs.PaladinProficiencies.ToString())
                .AddToLevelEntry(2, ShaitanFeat(), ThunderousChargeFeat())
                .AddToLevelEntry(3, FeatureRefs.AuraOfCourageFeature.ToString(), FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString())
                .AddToLevelEntry(4, DjinniFeat(), CreateControllCharge())
                .AddToLevelEntry(5)
                .AddToLevelEntry(6, MaridFeat(), SummonTrampleSelection())
                .AddToLevelEntry(7, AllEyesFeat())
                .AddToLevelEntry(8, DjinniBlessFeat(), MaridBlessFeat())
                .AddToLevelEntry(9, InspiringLeaderFeat(), ShaitanBlessFeat())
                .AddToLevelEntry(10, JanniBlessFeat())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { DjinniGuid, ShaitanGuid, MaridGuid, MaridBlessGuid, JanniBlessGuid }))
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
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillLoreNature, StatType.SkillMobility, StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 4)
                .AddPrerequisiteStatValue(StatType.SkillMobility, 5)
                .AddPrerequisiteStatValue(StatType.SkillLoreNature, 5)
                .AddPrerequisiteFeature(FeatureRefs.MountedCombat.ToString())
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var Asavirref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = Asavirref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string EquineBond = "Asavir.EquineBond";
        private static readonly string EquineBondGuid = "{25CE5060-A728-45CE-8FEE-6F22438E19E7}";

        internal const string EquineBondDisplayName = "AsavirEquineBond.Name";
        private const string EquineBondDescription = "AsavirEquineBond.Description";
        //"CavalierMobilityFeature": "272aa4cc-a738-4a69-92da-395f4fae3d22",
        public static BlueprintFeature EquineBondFeat()
        {
            var icon = FeatureRefs.CompanionBoon.Reference.Get().Icon;

            return FeatureConfigurator.New(EquineBond, EquineBondGuid)
              .SetDisplayName(EquineBondDisplayName)
              .SetDescription(EquineBondDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddCompanionBoon(2, FeatureRefs.AnimalCompanionRank.ToString())
              .AddFeatureToPet(FeatureRefs.LightBardingProficiency.ToString())
              .AddFacts(new() { "272aa4cc-a738-4a69-92da-395f4fae3d22" }) ///ttt
              .Configure();
        }

        private const string SummonBigAnimal2 = "Asavir.SummonBigAnimal2";
        private static readonly string SummonBigAnimalGuid2 = "{E871B792-7B27-4584-A042-A48DA1D91C32}";

        private const string ClassProgressName2 = "AsavirPrestige2";
        private static readonly string ClassProgressGuid2 = "{F255E51D-8CC2-4426-9F77-B9ED81F77F2A}";
        public static BlueprintFeatureSelection SummonAnimalSelection()
        {
            //var icon = AbilityRefs.SummonElvenSpirits.Reference.Get().Icon;

            var progression =
                ProgressionConfigurator.New(ClassProgressName2, ClassProgressGuid2)
                .CopyFrom(ProgressionRefs.SylvanSorcererAnimalCompanionProgression, typeof(LevelEntry), typeof(UIGroup))
                .SetClasses(ArchetypeGuid)
                .Configure();

            //FeatureSelectionConfigurator.For(FeatureSelectionRefs.DeitySelection.ToString())
                //.AddFacts(new() { FeatureRefs.MountTargetFeature.ToString() })
                //.Configure();

            return FeatureSelectionConfigurator.New(SummonBigAnimal2, SummonBigAnimalGuid2)
              .CopyFrom(
                FeatureSelectionRefs.AnimalCompanionSelectionSylvanSorcerer)
              .AddFeatureOnApply(progression)
              .AddFeatureOnApply(FeatureRefs.AnimalCompanionRank.ToString())
              .AddFeatureOnApply(FeatureRefs.MountTargetFeature.ToString())
              .AddFeatureOnApply(FeatureSelectionRefs.AnimalCompanionArchetypeSelection.ToString())
              .Configure(delayed: true);
        }

        private const string Shaitan = "Asavir.Shaitan";
        private static readonly string ShaitanGuid = "{76669252-6914-476F-9026-93371FD9AF7E}";

        internal const string ShaitanDisplayName = "AsavirShaitan.Name";
        private const string ShaitanDescription = "AsavirShaitan.Description";

        private const string EquineBond4 = "Asavir.EquineBond4";
        private static readonly string EquineBond4Guid = "{C862E371-C7DB-43AE-B7AD-63170A480D6A}";
        public static BlueprintFeature ShaitanFeat()
        {
            var icon = AbilityRefs.RemoveFear.Reference.Get().Icon;

            var feat4 = FeatureConfigurator.New(EquineBond4, EquineBond4Guid)
              .SetDisplayName(ShaitanDisplayName)
              .SetDescription(ShaitanDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Constant(2), spellDescriptor: SpellDescriptor.MindAffecting, modifierDescriptor: ModifierDescriptor.Racial)
              .Configure();

            return FeatureConfigurator.New(Shaitan, ShaitanGuid)
              .SetDisplayName(ShaitanDisplayName)
              .SetDescription(ShaitanDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat4)
              .Configure();
        }

        private const string ShaitanBless = "Asavir.ShaitanBless";
        private static readonly string ShaitanBlessGuid = "{BAC51D6D-D3CF-4CC3-B5AF-E959F9491939}";

        private const string EquineBond5 = "Asavir.EquineBond5";
        private static readonly string EquineBond5Guid = "{37CB53E9-6C4C-49CB-9EF0-1691459DFCE0}";
        public static BlueprintFeature ShaitanBlessFeat()
        {
            var icon = AbilityRefs.RemoveFear.Reference.Get().Icon;

            var feat5 = FeatureConfigurator.New(EquineBond5, EquineBond5Guid)
              .SetDisplayName(ShaitanDisplayName)
              .SetDescription(ShaitanDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Constant(2), spellDescriptor: SpellDescriptor.MindAffecting, modifierDescriptor: ModifierDescriptor.Racial)
              .Configure();

            return FeatureConfigurator.New(ShaitanBless, ShaitanBlessGuid)
              .SetDisplayName(ShaitanDisplayName)
              .SetDescription(ShaitanDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat5)
              .SetHideInUI(true)
              .Configure();
        }

        private const string Djinni = "Asavir.Djinni";
        private static readonly string DjinniGuid = "{D2806EDF-9FE1-4748-A189-0EF330E61E2F}";

        private const string DjinniBuff = "Asavir.DjinniBuff";
        private static readonly string DjinniBuffGuid = "{DE2DBC9B-77AC-448C-A26F-F438C6EF7D72}";

        private const string DjinniAbility = "Asavir.DjinniAbility";
        private static readonly string DjinniAbilityGuid = "{0A106946-66F9-4EA4-B177-86FFC2142D61}";

        internal const string DjinniDisplayName = "AsavirDjinni.Name";
        private const string DjinniDescription = "AsavirDjinni.Description";

        private const string EquineBond6 = "Asavir.EquineBond6";
        private static readonly string EquineBond6Guid = "{9FF96DBE-FA76-486A-A0B0-41A7788862BA}";
        public static BlueprintFeature DjinniFeat()
        {
            var icon = FeatureRefs.MountedCombat.Reference.Get().Icon;

            var feat6 = FeatureConfigurator.New(EquineBond6, EquineBond6Guid)
              .SetDisplayName(DjinniDisplayName)
              .SetDescription(DjinniDescription)
              .SetIcon(icon)
              .AddBuffMovementSpeed(value: 10)
              .Configure();

            var Buff1 = BuffConfigurator.New(DjinniBuff, DjinniBuffGuid)
             .SetDisplayName(DjinniDisplayName)
             .SetDescription(DjinniDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddFacts(new() { FeatureRefs.MountTargetFeature.ToString() })
             .Configure();

            var ability = AbilityConfigurator.New(DjinniAbility, DjinniAbilityGuid)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(Buff1).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .RemoveBuff(Buff1)
                        .Build())
                    .Build())
                .SetDisplayName(DjinniDisplayName)
                .SetDescription(DjinniDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .SetCanTargetPoint(false)
                .SetCanTargetSelf(false)
                .Configure();

            return FeatureConfigurator.New(Djinni, DjinniGuid)
              .SetDisplayName(DjinniDisplayName)
              .SetDescription(DjinniDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat6)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string DjinniBless = "Asavir.DjinniBless";
        private static readonly string DjinniBlessGuid = "{851E3E0F-C29F-4DDE-BFC4-DCD361A0E7C5}";

        private const string EquineBond7 = "Asavir.EquineBond7";
        private static readonly string EquineBond7Guid = "{C3332C59-0E83-4F9B-9ABD-ED903D3A6827}";
        public static BlueprintFeature DjinniBlessFeat()
        {
            var icon = FeatureRefs.MountedCombat.Reference.Get().Icon;

            var feat7 = FeatureConfigurator.New(EquineBond7, EquineBond7Guid)
              .SetDisplayName(DjinniDisplayName)
              .SetDescription(DjinniDescription)
              .SetIcon(icon)
              .AddBuffMovementSpeed(value: 10)
              .Configure();

            return FeatureConfigurator.New(DjinniBless, DjinniBlessGuid)
              .SetDisplayName(DjinniDisplayName)
              .SetDescription(DjinniDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat7)
              .SetHideInUI(true)
              .Configure();
        }

        private const string Marid = "Asavir.Marid";
        private static readonly string MaridGuid = "{FC0EA33B-49F1-456F-9B05-F7532D069CCF}";

        internal const string MaridDisplayName = "AsavirMarid.Name";
        private const string MaridDescription = "AsavirMarid.Description";

        private const string EquineBond8 = "Asavir.EquineBond8";
        private static readonly string EquineBond8Guid = "{F413EB1D-CBEC-4BA0-8F44-8AD7654B8B19}";
        public static BlueprintFeature MaridFeat()
        {
            var icon = FeatureRefs.MountedShield.Reference.Get().Icon;

            var feat8 = FeatureConfigurator.New(EquineBond8, EquineBond8Guid)
              .SetDisplayName(MaridDisplayName)
              .SetDescription(MaridDescription)
              .SetIcon(icon)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Racial, false, stat: StatType.SaveReflex, 2)
              .Configure();

            return FeatureConfigurator.New(Marid, MaridGuid)
              .SetDisplayName(MaridDisplayName)
              .SetDescription(MaridDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat8)
              .AddConcentrationBonus(false, value: ContextValues.Constant(2))
              .Configure();
        }

        private const string MaridBless = "Asavir.MaridBless";
        private static readonly string MaridBlessGuid = "{BC1E7BC2-F70B-4DFF-94FE-0234A3CF2389}";

        internal const string EfreetiDisplayName = "AsavirEfreeti.Name";
        private const string EfreetiDescription = "AsavirEfreeti.Description";

        private const string EquineBond9 = "Asavir.EquineBond9";
        private static readonly string EquineBond9Guid = "{1DD92609-BE11-4F83-9B99-D35F094B26AC}";
        public static BlueprintFeature MaridBlessFeat()
        {
            var icon = AbilityRefs.AngelPhoenixGift.Reference.Get().Icon;

            var feat9 = FeatureConfigurator.New(EquineBond9, EquineBond9Guid)
              .SetDisplayName(EfreetiDisplayName)
              .SetDescription(EfreetiDescription)
              .SetIcon(icon)
              .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Constant(5), type: Kingmaker.Enums.Damage.DamageEnergyType.Fire)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Flaming.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Flaming.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Flaming.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.AdditionalLimb)
              .Configure();

            return FeatureConfigurator.New(MaridBless, MaridBlessGuid)
              .SetDisplayName(EfreetiDisplayName)
              .SetDescription(EfreetiDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat9)
              .Configure();
        }

        private const string JanniBless = "Asavir.JanniBless";
        private static readonly string JanniBlessGuid = "{880C69F1-FB49-43DE-AF1A-9C9C40FB5EA8}";

        private const string EquineBond1 = "Asavir.EquineBond1";
        private static readonly string EquineBond1Guid = "{4EFE06D3-91E0-474D-A22E-1220010A7F7B}";

        internal const string JanniDisplayName = "AsavirJanni.Name";
        private const string JanniDescription = "AsavirJanni.Description";
        public static BlueprintFeature JanniBlessFeat()
        {
            var icon = AbilityRefs.Bless.Reference.Get().Icon;

            var feat1 = FeatureConfigurator.New(EquineBond1, EquineBond1Guid)
              .SetDisplayName(JanniDisplayName)
              .SetDescription(JanniDescription)
              .SetIcon(icon)
              .AddBuffAllSavesBonus(Kingmaker.Enums.ModifierDescriptor.Luck, 1)
              .Configure();

            return FeatureConfigurator.New(JanniBless, JanniBlessGuid)
              .SetDisplayName(JanniDisplayName)
              .SetDescription(JanniDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat1)
              .AddFacts(new() { FeatureRefs.PerfectStrikeFeature.ToString() })
              .AddBuffAllSavesBonus(Kingmaker.Enums.ModifierDescriptor.Luck, 1)
              .Configure();
        }

        private const string ControllCharge = "Asavir.ControllCharge";
        private static readonly string ControllChargeGuid = "{17385463-4190-430A-B9A8-E47F9B2A786B}";

        private const string ControllChargeBuff = "Asavir.ControllChargeBuff";
        private static readonly string ControllChargeGuidBuff = "{93AF4428-1085-4B01-908E-3502CE6783D3}";

        internal const string ControllChargeDisplayName = "AsavirControllCharge.Name";
        private const string ControllChargeDescription = "AsavirControllCharge.Description";
        private static BlueprintFeature CreateControllCharge()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ControllChargeBuff, ControllChargeGuidBuff)
              .SetDisplayName(ControllChargeDisplayName)
              .SetDescription(ControllChargeDescription)
              .SetIcon(icon)
              .AddAttackBonusConditional(bonus: ContextValues.Constant(2), descriptor: ModifierDescriptor.UntypedStackable)
              .AddACBonusAgainstAttacks(armorClassBonus: 2)
              .Configure();

            var feat = FeatureConfigurator.New(ControllCharge, ControllChargeGuid)
              .SetDisplayName(ControllChargeDisplayName)
              .SetDescription(ControllChargeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .Configure();

            var action = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(BuffRefs.MountedBuff.ToString()).CasterHasFact(feat).Build(), ifTrue: ActionsBuilder.New()
                    .ApplyBuff(buff: Buff1, durationValue: ContextDuration.Fixed(1))
                    .Build())
                .Build();

            BuffConfigurator.For(BuffRefs.ChargeBuff)
            .EditComponent<AddFactContextActions>(
                a => a.Activated.Actions = CommonTool.Append(a.Activated.Actions, action.Actions))
              .Configure();

            return feat;
        }

        private const string AllEyes = "Asavir.AllEyes";
        private static readonly string AllEyesGuid = "{6247701B-7845-4DAA-9EEB-708441B963BD}";

        internal const string AllEyesDisplayName = "AsavirAllEyes.Name";
        private const string AllEyesDescription = "AsavirAllEyes.Description";

        private const string AllEyesAbility = "Asavir.AllEyesAbility";
        private static readonly string AllEyesAbilityGuid = "{E7C9E1C1-A20D-4DF2-9759-A93FF1419D2B}";

        private const string AllEyesAbilityRes = "Asavir.AllEyesAbilityRes";
        private static readonly string AllEyesAbilityResGuid = "{16650270-7BAE-4258-9D8F-E850F2253F0A}";

        private const string AllEyesBuff = "Asavir.AllEyesBuff";
        private static readonly string AllEyesBuffGuid = "{220E113B-5204-4A41-986D-6A0FB3E2BAE8}";

        public static BlueprintFeature AllEyesFeat()
        {
            var icon = AbilityRefs.Glitterdust.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(AllEyesAbilityRes, AllEyesAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(3))
                .Configure();

            var Buff = BuffConfigurator.New(AllEyesBuff, AllEyesBuffGuid)
              .SetDisplayName(AllEyesDisplayName)
              .SetDescription(AllEyesDescription)
              .SetIcon(icon)
              .AddSpellDescriptorComponent(descriptor: SpellDescriptor.MindAffecting)
              .AddSpellDescriptorComponent(descriptor: SpellDescriptor.GazeAttack)
              .AddComponent<AllEyesonMe>()
              .Configure();

            var ability = AbilityConfigurator.New(AllEyesAbility, AllEyesAbilityGuid)
                .CopyFrom(
                AbilityRefs.DazzlingDisplayAction,
                typeof(AbilitySpawnFx))
                //.SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Will, useDCFromContextSavingThrow: true,
                    onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(Buff, ContextDuration.Fixed(1)).Build()).Build())
                    .Build())
                .SetDisplayName(AllEyesDisplayName)
                .SetDescription(AllEyesDescription)
                .SetIcon(icon)
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Enemy, radius: 30.Feet(), spreadSpeed: 40.Feet())
                .AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Charisma; })
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddSpellDescriptorComponent(descriptor: SpellDescriptor.MindAffecting)
                .AddSpellDescriptorComponent(descriptor: SpellDescriptor.GazeAttack)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(AllEyes, AllEyesGuid)
              .SetDisplayName(AllEyesDisplayName)
              .SetDescription(AllEyesDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string InspiringLeader = "Asavir.InspiringLeader";
        private static readonly string InspiringLeaderGuid = "{966E91DD-89BD-46A8-ABB7-57317972AAF1}";

        internal const string InspiringLeaderDisplayName = "AsavirInspiringLeader.Name";
        private const string InspiringLeaderDescription = "AsavirInspiringLeader.Description";

        private const string InspiringLeaderAbility = "Asavir.InspiringLeaderAbility";
        private static readonly string InspiringLeaderAbilityGuid = "{8A41955E-9D7B-4971-89E6-60B5BD3B1219}";

        private const string InspiringLeaderAbilityRes = "Asavir.InspiringLeaderAbilityRes";
        private static readonly string InspiringLeaderAbilityResGuid = "{7391923E-72D5-4413-92A2-45C0F0FD59D3}";

        private const string InspiringLeaderBuff = "Asavir.InspiringLeaderBuff";
        private static readonly string InspiringLeaderBuffGuid = "{826BA996-7424-45F1-8DBA-0A7DBF49F750}";

        public static BlueprintFeature InspiringLeaderFeat()
        {
            var icon = FeatureRefs.InspireHeroicsFeature.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(InspiringLeaderAbilityRes, InspiringLeaderAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(3))
                .Configure();

            var Buff = BuffConfigurator.New(InspiringLeaderBuff, InspiringLeaderBuffGuid)
              .SetDisplayName(InspiringLeaderDisplayName)
              .SetDescription(InspiringLeaderDescription)
              .SetIcon(icon)
              .AddSpellDescriptorComponent(descriptor: SpellDescriptor.MindAffecting)
              .AddComponent<AsavirInspiringLeader>()
              .Configure();

            var ability = AbilityConfigurator.New(InspiringLeaderAbility, InspiringLeaderAbilityGuid)
                //.SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().IsCaster().Build(), ifFalse: ActionsBuilder.New()
                        .ApplyBuff(Buff, ContextDuration.Fixed(1))
                        .Build())
                    .Build())
                .SetDisplayName(InspiringLeaderDisplayName)
                .SetDescription(InspiringLeaderDescription)
                .SetIcon(icon)
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Ally, radius: 60.Feet(), spreadSpeed: 40.Feet())
                //.AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Charisma; })
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .AddSpellDescriptorComponent(descriptor: SpellDescriptor.MindAffecting)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(InspiringLeader, InspiringLeaderGuid)
              .SetDisplayName(InspiringLeaderDisplayName)
              .SetDescription(InspiringLeaderDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string ThunderousCharge = "Asavir.ThunderousCharge";
        private static readonly string ThunderousChargeGuid = "{45C5B9B9-3C0E-47A5-84AF-07A62A86AF2F}";

        internal const string ThunderousChargeDisplayName = "AsavirThunderousCharge.Name";
        private const string ThunderousChargeDescription = "AsavirThunderousCharge.Description";

        private const string ThunderousChargeAbility = "Asavir.ThunderousChargeAbility";
        private static readonly string ThunderousChargeAbilityGuid = "{24C220B0-2B0E-4D5A-97E4-865CFF4B9674}";

        private const string ThunderousChargeAbilityRes = "Asavir.ThunderousChargeAbilityRes";
        private static readonly string ThunderousChargeAbilityResGuid = "{C26EADB2-B1DE-4D8E-85DC-EE7BD390DEA2}";

        private const string ThunderousChargeAbility1 = "Asavir.ThunderousChargeAbility1";
        private static readonly string ThunderousChargeAbility1Guid = "{1F870299-BF72-41B6-B9BC-9C0D6D658455}";

        private const string ThunderousChargeAbility2 = "Asavir.ThunderousChargeAbility2";
        private static readonly string ThunderousChargeAbility2Guid = "{07174E7F-EC67-4ABE-8DCE-14D0DFA10A88}";

        private const string ThunderousChargeAbility3 = "Asavir.ThunderousChargeAbility3";
        private static readonly string ThunderousChargeAbility3Guid = "{D895EF5B-54D9-44BB-9C5D-17158AC40AD7}";

        private const string ThunderousChargeBuff = "Asavir.ThunderousChargeBuff";
        private static readonly string ThunderousChargeBuffGuid = "{8ABCBA5E-E12A-4962-AB5A-0AC43102DAB2}";

        public static BlueprintFeature ThunderousChargeFeat()
        {
            var icon = FeatureRefs.CavalierMightyCharge.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                .Conditional(ConditionsBuilder.New().CharacterClass(true, ArchetypeGuid, 1, false).CharacterClass(true, ArchetypeGuid, 6, true).Build(), ifTrue: ActionsBuilder.New().CastSpell(ThunderousChargeAbility1Guid).Build())
                .Conditional(ConditionsBuilder.New().CharacterClass(true, ArchetypeGuid, 6, false).CharacterClass(true, ArchetypeGuid, 10, true).Build(), ifTrue: ActionsBuilder.New().CastSpell(ThunderousChargeAbility2Guid).Build())
                .Conditional(ConditionsBuilder.New().CharacterClass(true, ArchetypeGuid, 10, false).Build(), ifTrue: ActionsBuilder.New().CastSpell(ThunderousChargeAbility3Guid).Build())
                .RemoveSelf()
                .Build();

            var abilityresourse = AbilityResourceConfigurator.New(ThunderousChargeAbilityRes, ThunderousChargeAbilityResGuid)
                 .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 1, bonusPerStep: 1))
                .Configure();

            var Buff = BuffConfigurator.New(ThunderousChargeBuff, ThunderousChargeBuffGuid)
              .SetDisplayName(ThunderousChargeDisplayName)
              .SetDescription(ThunderousChargeDescription)
              .SetIcon(icon)
              .AddInitiatorAttackWithWeaponTrigger(action, true, triggerBeforeAttack: true)
              .Configure();

            var ability = AbilityConfigurator.New(ThunderousChargeAbility, ThunderousChargeAbilityGuid)
                .CopyFrom(
                AbilityRefs.DazzlingDisplayAction,
                typeof(AbilitySpawnFx))
                //.SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(Buff, ContextDuration.Fixed(1)).Build())
                .SetDisplayName(ThunderousChargeDisplayName)
                .SetDescription(ThunderousChargeDescription)
                .SetIcon(icon)
                //.AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Ally, radius: 60.Feet(), spreadSpeed: 40.Feet())
                //.AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Charisma; })
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            var ability1 = AbilityConfigurator.New(ThunderousChargeAbility1, ThunderousChargeAbility1Guid)
                .CopyFrom(
                AbilityRefs.DazzlingDisplayAction,
                typeof(AbilitySpawnFx))
                //.SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().Add<AsavirThunderousCharge>().Build())
                .SetDisplayName(ThunderousChargeDisplayName)
                .SetDescription(ThunderousChargeDescription)
                .SetIcon(icon)
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Enemy, radius: 10.Feet(), spreadSpeed: 5.Feet())
                //.AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Charisma; })
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .Configure();

            var ability2 = AbilityConfigurator.New(ThunderousChargeAbility2, ThunderousChargeAbility2Guid)
                .CopyFrom(
                AbilityRefs.DazzlingDisplayAction,
                typeof(AbilitySpawnFx))
                //.SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().Add<AsavirThunderousCharge>().Build())
                .SetDisplayName(ThunderousChargeDisplayName)
                .SetDescription(ThunderousChargeDescription)
                .SetIcon(icon)
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Enemy, radius: 15.Feet(), spreadSpeed: 5.Feet())
                //.AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Charisma; })
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .Configure();

            var ability3 = AbilityConfigurator.New(ThunderousChargeAbility3, ThunderousChargeAbility3Guid)
                .CopyFrom(
                AbilityRefs.DazzlingDisplayAction,
                typeof(AbilitySpawnFx))
                //.SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().Add<AsavirThunderousCharge>().Build())
                .SetDisplayName(ThunderousChargeDisplayName)
                .SetDescription(ThunderousChargeDescription)
                .SetIcon(icon)
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Enemy, radius: 25.Feet(), spreadSpeed: 5.Feet())
                //.AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Charisma; })
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .Configure();

            return FeatureConfigurator.New(ThunderousCharge, ThunderousChargeGuid)
              .SetDisplayName(ThunderousChargeDisplayName)
              .SetDescription(ThunderousChargeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string SummonBigTrample2 = "Asavir.SummonBigTrample2";
        private static readonly string SummonBigTrampleGuid2 = "{D26E55CB-5979-44AE-BDE2-DDBCE3C6BAA5}";

        internal const string SummonBigTrample2DisplayName = "AsavirSummonBigTrample2.Name";
        private const string SummonBigTrample2Description = "AsavirSummonBigTrample2.Description";

        private const string SummonBigTrample = "Asavir.SummonBigTrample";
        private static readonly string SummonBigTrampleGuid = "{275FE548-1359-40C9-85E9-610BCC892392}";

        private const string SummonBigTrample1 = "Asavir.SummonBigTrample1";
        private static readonly string SummonBigTrample1Guid = "{03FECF1C-4B49-4DC7-96C2-FC97F9849E82}";

        internal const string SummonBigTrampleDisplayName = "AsavirSummonBigTrample.Name";
        private const string SummonBigTrampleDescription = "AsavirSummonBigTrample.Description";

        public static BlueprintFeatureSelection SummonTrampleSelection()
        {
            var icon = FeatureRefs.Trample.Reference.Get().Icon;

            var feat1 = FeatureConfigurator.New(SummonBigTrample1, SummonBigTrample1Guid)
              .SetDisplayName(SummonBigTrampleDisplayName)
              .SetDescription(SummonBigTrampleDescription)
              .SetIcon(icon)
              .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Overrun }, value: ContextValues.Constant(4))
              .Configure();

            var feat = FeatureConfigurator.New(SummonBigTrample, SummonBigTrampleGuid)
              .SetDisplayName(SummonBigTrampleDisplayName)
              .SetDescription(SummonBigTrampleDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.Trample.ToString())
              .AddFeatureToPet(feat1)
              .Configure();

            return FeatureSelectionConfigurator.New(SummonBigTrample2, SummonBigTrampleGuid2)
              .SetDisplayName(SummonBigTrample2DisplayName)
              .SetDescription(SummonBigTrample2Description)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.Trample.ToString())
              .AddToAllFeatures(feat)
              .Configure();
        }
    }
}
