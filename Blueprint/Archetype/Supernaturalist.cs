using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Utils;
using PrestigePlus.Blueprint.Spell;
using PrestigePlus.CustomComponent.Spell;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem.Rules;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Actions.Builder;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using PrestigePlus.CustomAction.OtherFeatRelated;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.Designers;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using static Kingmaker.GameModes.GameModeType;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class EldritchBotanistComp : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            bool flag = evt.Reason.Context != null && (evt.Reason.Context.SpellDescriptor & des) != SpellDescriptor.None;
            if (flag && evt.StatType == StatType.SaveWill && Owner.Stats.SaveFortitude > Owner.Stats.SaveWill)
            {
                evt.StatType = StatType.SaveFortitude;
            }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            
        }

        public SpellDescriptorWrapper des;
    }
    internal class Supernaturalist
    {
        private const string ArchetypeName = "Supernaturalist";
        private static readonly string ArchetypeGuid = "{FE10CC0C-6F52-4892-9760-A8748B0BB26B}";
        internal const string ArchetypeDisplayName = "Supernaturalist.Name";
        private const string ArchetypeDescription = "Supernaturalist.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.DruidClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.NatureSense.ToString(), FeatureRefs.DruidProficiencies.ToString())
            .SetRemoveFeaturesEntry(4, FeatureRefs.WildShapeIWolfFeature.ToString(), FeatureRefs.ResistNaturesLure.ToString())
            .SetRemoveFeaturesEntry(6, FeatureRefs.WildShapeIILeopardFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalSmallFeature.ToString())
            .SetRemoveFeaturesEntry(8, FeatureRefs.WildShapeIVBearFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalFeatureAddMedium.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.WildShapeIIISmilodonFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalFeatureAddLarge.ToString(), FeatureRefs.WildShapeIVShamblingMoundFeature.ToString())
            .SetRemoveFeaturesEntry(12, FeatureRefs.WildShapeElementaHugeFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString())
            .SetRemoveFeaturesEntry(14, FeatureRefs.WildShapeExtraUse.ToString())
            .SetRemoveFeaturesEntry(16, FeatureRefs.WildShapeExtraUse.ToString())
            .SetRemoveFeaturesEntry(18, FeatureRefs.WildShapeExtraUse.ToString())
            .AddToAddFeatures(1, CreateProficiencies())
            .AddToAddFeatures(3, CreateParanormalScholar3())
            .AddToAddFeatures(6, CreateParanormalScholar6())
            .AddToAddFeatures(9, CreateParanormalScholar9(), PackLeaderConfigure())
            .AddToAddFeatures(12, CreateParanormalScholar12())
            .AddToAddFeatures(15, CreateParanormalScholar15())
            .AddToAddFeatures(18, CreateParanormalScholar18())
            .AddToAddFeatures(4, CreateSpiritBonus(), CreateTotemTransformation())
            .AddToAddFeatures(7, SpiritBonusGuid)
            .AddToAddFeatures(11, SpiritBonusGuid)
            .AddToAddFeatures(15, SpiritBonusGuid)
            .AddToAddFeatures(19, SpiritBonusGuid)
            .AddToAddFeatures(14, WildstrikeFeat())
            .AddToAddFeatures(20, WildHeartFeat())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.DruidProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { ParanormalScholar3Guid, ParanormalScholar6Guid, ParanormalScholar9Guid, ParanormalScholar12Guid, ParanormalScholar15Guid, ParanormalScholar18Guid })
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { TotemTransformationGuid, PackLeaderGuid, WildstrikeGuid, WildHeartGuid })
                .Configure();

            EldritchBotanistConfigure();
            EditedPlantTypeConfigure();
        }

        private const string EditedPlantTypeFeat = "Supernaturalist.EditedPlantType";
        public static readonly string EditedPlantTypeGuid = "{C91C9F56-F394-478F-BAFE-6424C96D4BBE}";
        public static void EditedPlantTypeConfigure()
        {
            var feat = FeatureConfigurator.New(EditedPlantTypeFeat, EditedPlantTypeGuid)
              .CopyFrom(FeatureRefs.ImmunityToMindAffecting)
              .AddBuffDescriptorImmunity(false, SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.Charm | SpellDescriptor.Sleep | SpellDescriptor.Stun | SpellDescriptor.Paralysis, ignoreFeature: ProficienciesGuid)
              .AddSpellImmunityToSpellDescriptor(ProficienciesGuid, SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.Charm | SpellDescriptor.Sleep | SpellDescriptor.Stun | SpellDescriptor.Paralysis)
              .AddComponent<EldritchBotanistComp>(c => { c.des = SpellDescriptor.MindAffecting; })
              .Configure();

            var facts = FeatureRefs.PlantType.Reference.Get().GetComponent<AddFacts>();
            if (facts?.m_Facts?.Count() == 6)
            {
                facts.m_Facts = new BlueprintUnitFactReference[] { feat.ToReference<BlueprintUnitFactReference>(), FeatureRefs.ImmunityToPoison.Reference.Get().ToReference<BlueprintUnitFactReference>(), FeatureRefs.ImmunityToPolymorph.Reference.Get().ToReference<BlueprintUnitFactReference>() };
                Main.Logger.Info("editing plant immunity");
            }
            else
            {
                Main.Logger.Info("not edited plant immunity");
            }
        }

        private const string EldritchBotanistFeat = "Supernaturalist.EldritchBotanist";
        public static readonly string EldritchBotanistGuid = "{7A563862-2DC2-4061-8D0A-FFC10C8B98A1}";

        internal const string EldritchBotanistDisplayName = "SupernaturalistEldritchBotanist.Name";
        private const string EldritchBotanistDescription = "SupernaturalistEldritchBotanist.Description";
        public static void EldritchBotanistConfigure()
        {
            var icon = AbilityRefs.SickeningEntanglement.Reference.Get().Icon;
            //"TreesingerPlantBondSelection": "c0b7042d-7c29-4682-b555-dbcc531236b6",
            var feat = FeatureSelectionConfigurator.New(EldritchBotanistFeat, EldritchBotanistGuid)
              .SetDisplayName(EldritchBotanistDisplayName)
              .SetDescription(EldritchBotanistDescription)
              .SetIcon(icon)
              .AddPrerequisiteArchetypeLevel(ArchetypeGuid, CharacterClassRefs.DruidClass.ToString())
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures("c0b7042d-7c29-4682-b555-dbcc531236b6")
              .SetHideNotAvailibleInUI(true)
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DruidBondSelection)
                .AddToAllFeatures(feat)
                .Configure();
        }

        private const string Proficiencies = "Supernaturalist.Proficiencies";
        private static readonly string ProficienciesGuid = "{D46EAF7C-1690-4F41-B2B6-85B8655F1073}";
        internal const string ProficienciesDisplayName = "SupernaturalistProficiencies.Name";
        private const string ProficienciesDescription = "SupernaturalistProficiencies.Description";
        public static BlueprintFeature CreateProficiencies()
        {
            var icon = AbilityRefs.SickeningEntanglement.Reference.Get().Icon;
            var dProficiencies = FeatureRefs.DruidProficiencies.Reference.Get();
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .SetIcon(icon)
              .AddComponent(dProficiencies.GetComponent<AddProficiencies>())
              .AddFacts(new() { FeatureRefs.ShieldsProficiency.ToString() })
              .AddToIsPrerequisiteFor(EldritchBotanistGuid)
              .Configure();
        }

        private const string ParanormalScholar3 = "Supernaturalist.ParanormalScholar3";
        public static readonly string ParanormalScholar3Guid = "{27BF8095-31F9-499F-8D6A-277D46378418}";

        internal const string ParanormalScholar3DisplayName = "SupernaturalistParanormalScholar3.Name";
        private const string ParanormalScholar3Description = "SupernaturalistParanormalScholar3.Description";

        public static BlueprintFeature CreateParanormalScholar3()
        {
            var icon = AbilityRefs.BestowGraceCast.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar3, ParanormalScholar3Guid)
              .SetDisplayName(ParanormalScholar3DisplayName)
              .SetDescription(ParanormalScholar3Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(HermeanPotential.HermeanPotentialAbilityGuid);
                  c.level = 1;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar6 = "Supernaturalist.ParanormalScholar6";
        public static readonly string ParanormalScholar6Guid = "{9868BE72-1794-432D-9593-B40609A2CC81}";

        internal const string ParanormalScholar6DisplayName = "SupernaturalistParanormalScholar6.Name";
        private const string ParanormalScholar6Description = "SupernaturalistParanormalScholar6.Description";

        public static BlueprintFeature CreateParanormalScholar6()
        {
            var icon = AbilityRefs.CacophonousCall.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar6, ParanormalScholar6Guid)
              .SetDisplayName(ParanormalScholar6DisplayName)
              .SetDescription(ParanormalScholar6Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(DebilitatingPain.DebilitatingPainAbilityGuid);
                  c.level = 3;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar9 = "Supernaturalist.ParanormalScholar9";
        public static readonly string ParanormalScholar9Guid = "{64182E9A-27FB-42FA-9308-7E2A5AA0AB90}";

        internal const string ParanormalScholar9DisplayName = "SupernaturalistParanormalScholar9.Name";
        private const string ParanormalScholar9Description = "SupernaturalistParanormalScholar9.Description";

        public static BlueprintFeature CreateParanormalScholar9()
        {
            var icon = AbilityRefs.CacophonousCallMass.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar9, ParanormalScholar9Guid)
              .SetDisplayName(ParanormalScholar9DisplayName)
              .SetDescription(ParanormalScholar9Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(DebilitatingPainMass.DebilitatingPainMassAbilityGuid);
                  c.level = 5;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar12 = "Supernaturalist.ParanormalScholar12";
        public static readonly string ParanormalScholar12Guid = "{90986E5A-0A0F-449E-8F2E-75FEB5E72825}";

        internal const string ParanormalScholar12DisplayName = "SupernaturalistParanormalScholar12.Name";
        private const string ParanormalScholar12Description = "SupernaturalistParanormalScholar12.Description";

        public static BlueprintFeature CreateParanormalScholar12()
        {
            var icon = AbilityRefs.ConstrictingCoils.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar12, ParanormalScholar12Guid)
              .SetDisplayName(ParanormalScholar12DisplayName)
              .SetDescription(ParanormalScholar12Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(PhantomLimb.PhantomLimbAbilityGuid);
                  c.level = 6;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar15 = "Supernaturalist.ParanormalScholar15";
        public static readonly string ParanormalScholar15Guid = "{1ECE9A0E-609C-4B18-969B-082298C61C26}";

        internal const string ParanormalScholar15DisplayName = "SupernaturalistParanormalScholar15.Name";
        private const string ParanormalScholar15Description = "SupernaturalistParanormalScholar15.Description";

        public static BlueprintFeature CreateParanormalScholar15()
        {
            var icon = AbilityRefs.TrueSeeingCommunal.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar15, ParanormalScholar15Guid)
              .SetDisplayName(ParanormalScholar15DisplayName)
              .SetDescription(ParanormalScholar15Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(GlimpseAkashic.GlimpseAkashicAbilityGuid);
                  c.level = 8;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar18 = "Supernaturalist.ParanormalScholar18";
        public static readonly string ParanormalScholar18Guid = "{C20564B2-B1B7-44D0-A613-23B4E12343DB}";

        internal const string ParanormalScholar18DisplayName = "SupernaturalistParanormalScholar18.Name";
        private const string ParanormalScholar18Description = "SupernaturalistParanormalScholar18.Description";

        public static BlueprintFeature CreateParanormalScholar18()
        {
            var icon = AbilityRefs.BrilliantInspiration.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar18, ParanormalScholar18Guid)
              .SetDisplayName(ParanormalScholar18DisplayName)
              .SetDescription(ParanormalScholar18Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(AkashicForm.AkashicFormAbilityGuid);
                  c.level = 9;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string SpiritBonus = "Supernaturalist.SpiritBonus";
        private static readonly string SpiritBonusGuid = "{7D72C6EA-8612-4B2D-96AA-EEE6878F1F5F}";

        internal const string SpiritBonusDisplayName = "SupernaturalistSpiritBonus.Name";
        private const string SpiritBonusDescription = "SupernaturalistSpiritBonus.Description";
        private static BlueprintFeature CreateSpiritBonus()
        {
            var icon = AbilityRefs.NaturesExile.Reference.Get().Icon;

            return FeatureConfigurator.New(SpiritBonus, SpiritBonusGuid)
              .SetDisplayName(SpiritBonusDisplayName)
              .SetDescription(SpiritBonusDescription)
              .SetIcon(icon)
              .SetRanks(5)
              .AddStatBonus(stat: StatType.SkillLoreNature, value: 1)
              .AddWeaponGroupAttackBonus(1, weaponGroup: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Natural)
              .AddWeaponGroupDamageBonus(damageBonus: 1, weaponGroup: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Natural)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(SpiritBonusGuid))
              .Configure();
        }

        private const string TotemTransformation = "Supernaturalist.TotemTransformation";
        private static readonly string TotemTransformationGuid = "{DCD3C31D-565A-4DC5-994B-395919B8A8D7}";

        internal const string TotemTransformationDisplayName = "SupernaturalistTotemTransformation.Name";
        private const string TotemTransformationDescription = "SupernaturalistTotemTransformation.Description";

        private const string TotemTransformationBuff = "Supernaturalist.TotemTransformationBuff";
        private static readonly string TotemTransformationBuffGuid = "{2D34A28C-D755-45E5-831B-264F8AEF2C4F}";

        private const string TotemTransformationAbility = "Supernaturalist.TotemTransformationAbility";
        private static readonly string TotemTransformationAbilityGuid = "{2BB6FFB4-EA29-4B66-AD11-82F1DAF8D2C7}";
        private static BlueprintFeature CreateTotemTransformation()
        {
            var icon = AbilityRefs.WildShapeIWolfAbillity.Reference.Get().Icon;

            var ability = ActivatableAbilityConfigurator.New(TotemTransformationAbility, TotemTransformationAbilityGuid)
                .SetDisplayName(TotemTransformationDisplayName)
                .SetDescription(TotemTransformationDescription)
                .SetIcon(icon)
                .SetBuff(TotemTransformationBuffGuid)
                .SetIsOnByDefault(true)
                .SetDeactivateImmediately(true)
                .Configure();

            BuffConfigurator.New(TotemTransformationBuff, TotemTransformationBuffGuid)
             .SetDisplayName(TotemTransformationDisplayName)
             .SetDescription(TotemTransformationDescription)
             .SetIcon(icon)
             .AddEmptyHandWeaponOverride(isMonkUnarmedStrike: false, isPermanent: false, weapon: ItemWeaponRefs.Claw1d4.ToString())
             .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
             .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
             .Configure();

            return FeatureConfigurator.New(TotemTransformation, TotemTransformationGuid)
              .SetDisplayName(TotemTransformationDisplayName)
              .SetDescription(TotemTransformationDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.ArmorFocus, false, StatType.AC, 1)
              .AddAdditionalLimb(ItemWeaponRefs.Bite1d6.ToString())
              .AddBuffMovementSpeed(value: 20, descriptor: ModifierDescriptor.Enhancement)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string PackLeader = "Supernaturalist.PackLeader";
        public static readonly string PackLeaderGuid = "{8050D709-2C9A-4D96-A87E-9DFBF7F1BF08}";

        private const string PackLeaderFeat = "Supernaturalist.UsePackLeader";
        private static readonly string PackLeaderFeatGuid = "{447CD9F4-3FD3-4727-8652-9613F5CEF198}";

        internal const string PackLeaderDisplayName = "SupernaturalistPackLeader.Name";
        private const string PackLeaderDescription = "SupernaturalistPackLeader.Description";
        public static BlueprintFeature PackLeaderConfigure()
        {
            var icon = AbilityRefs.BullsStrengthMass.Reference.Get().Icon;

            var buff = BuffConfigurator.New(PackLeaderFeat, PackLeaderFeatGuid)
              .SetDisplayName(PackLeaderDisplayName)
              .SetDescription(PackLeaderDescription)
              .SetIcon(icon)
              .AddStatBonus(stat: StatType.AdditionalAttackBonus, value: 2)
              .AddStatBonus(stat: StatType.AdditionalDamage, value: 2)
              .AddBuffAllSavesBonus(value: 2)
              .Configure();

            return FeatureConfigurator.New(PackLeader, PackLeaderGuid)
              .SetDisplayName(PackLeaderDisplayName)
              .SetDescription(PackLeaderDescription)
              .SetIcon(icon)
              .AddOnSpawnBuff(buff, checkSummonedUnitFact: true, ifHaveFact: PackLeaderGuid, ifSummonHaveFact: FeatureRefs.AnimalType.ToString(), isInfinity: true)
              .Configure();
        }

        private const string Wildstrike = "Supernaturalist.Wildstrike";
        private static readonly string WildstrikeGuid = "{26DC90BF-2721-4CCD-B34B-F5B9FA845257}";

        internal const string WildstrikeDisplayName = "SupernaturalistWildstrike.Name";
        private const string WildstrikeDescription = "SupernaturalistWildstrike.Description";

        private const string WildstrikeAbility = "Supernaturalist.WildstrikeAbility";
        private static readonly string WildstrikeAbilityGuid = "{49143210-6E57-4DC7-9AE6-4420A183A5F2}";

        private const string WildstrikeRes = "Supernaturalist.WildstrikeRes";
        private static readonly string WildstrikeResGuid = "{0858C4CF-9CE5-4758-954D-E2DD88C7E5A4}";
        public static BlueprintFeature WildstrikeFeat()
        {
            var icon = AbilityRefs.CatsGraceMass.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(WildstrikeRes, WildstrikeResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(3))
                .Configure();

            var ability = AbilityConfigurator.New(WildstrikeAbility, WildstrikeAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .Add<ContextWildstrike>()
                        .Build())
                .SetDisplayName(WildstrikeDisplayName)
                .SetDescription(WildstrikeDescription)
                .SetIcon(icon)
                .AllowTargeting(false, false, true, false)
                .AddAbilityTargetHasFact(new() { FeatureRefs.AnimalType.ToString(), FeatureRefs.PlantType.ToString(), FeatureRefs.PlantTypeFake.ToString() })
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Extraordinary)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(Wildstrike, WildstrikeGuid)
              .SetDisplayName(WildstrikeDisplayName)
              .SetDescription(WildstrikeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string WildHeart = "Supernaturalist.WildHeart";
        private static readonly string WildHeartGuid = "{93CD6818-0582-404D-8C67-78972376C756}";

        internal const string WildHeartDisplayName = "SupernaturalistWildHeart.Name";
        private const string WildHeartDescription = "SupernaturalistWildHeart.Description";

        private const string WildHeartAbility = "Supernaturalist.WildHeartAbility";
        private static readonly string WildHeartAbilityGuid = "{C617C930-5662-42F1-82BB-50D5B7642742}";

        private const string WildHeartRes = "Supernaturalist.WildHeartRes";
        private static readonly string WildHeartResGuid = "{6A1A74BF-C847-4226-BE6D-FCFF02BE5B7F}";
        public static BlueprintFeature WildHeartFeat()
        {
            var icon = AbilityRefs.SummonNaturesAllyIX.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(WildHeartRes, WildHeartResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(WildHeartAbility, WildHeartAbilityGuid)
                .CopyFrom(
                AbilityRefs.SummonNaturesAllyIX,
                typeof(AbilityVariants),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(ContextRankConfig),
                typeof(AbilityCasterHasNoFacts))
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(WildHeart, WildHeartGuid)
              .SetDisplayName(WildHeartDisplayName)
              .SetDescription(WildHeartDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSavingThrowBonusAgainstDescriptor(value: 4, spellDescriptor: SpellDescriptor.MindAffecting, modifierDescriptor: ModifierDescriptor.NaturalArmorForm, bonus: 0)
              .AddSavingThrowBonusAgainstSchool(value: 4, school: SpellSchool.Enchantment, modifierDescriptor: ModifierDescriptor.NaturalArmorForm)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }
    }
}
