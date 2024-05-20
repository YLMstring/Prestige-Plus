using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using PrestigePlus.CustomComponent.Archetype;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints.Classes.Prerequisites;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.CustomAction.OtherFeatRelated;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.Maneuvers;
using Kingmaker.AreaLogic;
using System.Drawing;
using Kingmaker.UnitLogic.FactLogic;

namespace PrestigePlus.Blueprint.Feat
{
    internal class Inquisition
    {
        private const string Spellkiller = "Inquisition.Spellkiller";
        private static readonly string SpellkillerGuid = "{EB9949FD-B5B5-4966-8E81-6C1F4F2E8174}";

        internal const string SpellkillerDisplayName = "InquisitionSpellkiller.Name";
        private const string SpellkillerDescription = "InquisitionSpellkiller.Description";
        public static BlueprintProgression SpellkillerFeat()
        {
            var icon = AbilityRefs.VampiricShadowShield.Reference.Get().Icon;

            var pro = ProgressionConfigurator.New(Spellkiller, SpellkillerGuid)
              .SetDisplayName(SpellkillerDisplayName)
              .SetDescription(SpellkillerDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ProgressionRefs.AirDomainProgression.Reference.Get().m_Classes)
              .SetArchetypes(ProgressionRefs.AirDomainProgression.Reference.Get().m_Archetypes)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToGroups(FeatureGroup.Domain)
              .AddToLevelEntry(1, ChooseGoodEvilFeat())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DomainsSelection)
                .AddToAllFeatures(pro)
                .Configure();

            return pro;
        }

        private const string ChooseGoodEvil = "Inquisition.ChooseGoodEvil";
        private static readonly string ChooseGoodEvilGuid = "{871E15F9-BEF1-4C8C-BD01-FD517A38AF41}";

        internal const string ChooseGoodEvilDisplayName = "InquisitionChooseGoodEvil.Name";
        private const string ChooseGoodEvilDescription = "InquisitionChooseGoodEvil.Description";

        public static BlueprintFeatureSelection ChooseGoodEvilFeat()
        {
            var icon = AbilityRefs.ShieldOfDawn.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ChooseGoodEvil, ChooseGoodEvilGuid)
              .SetDisplayName(ChooseGoodEvilDisplayName)
              .SetDescription(ChooseGoodEvilDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(HunterGoodFeat())
              .AddToAllFeatures(HunterEvilFeat())
              .AddFacts(new() { FeatureRefs.Disruptive.ToString() })
              .Configure();
        }

        private const string HunterGood = "Inquisition.HunterGood";
        private static readonly string HunterGoodGuid = "{089CB193-5638-4ADB-9B18-35023FD8ABA7}";

        private const string HunterGoodBuff = "Inquisition.HunterGoodBuff";
        private static readonly string HunterGoodBuffGuid = "{C1B2FB40-8EE4-455B-AEF1-AD58A4EBF98B}";

        internal const string InquisitionHunterGoodDisplayName = "InquisitionHunterGood.Name";
        private const string InquisitionHunterGoodDescription = "InquisitionHunterGood.Description";
        public static BlueprintFeature HunterGoodFeat()
        {
            var icon = AbilityRefs.ShieldOfDawn.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(HunterGoodBuff, HunterGoodBuffGuid)
              .SetDisplayName(InquisitionHunterGoodDisplayName)
              .SetDescription(InquisitionHunterGoodDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Sacred, false, StatType.AC, 2)
              .Configure();
            
            var action = ActionsBuilder.New()
                .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes), toCaster: true)
                .Build();

            return FeatureConfigurator.New(HunterGood, HunterGoodGuid)
              .SetDisplayName(InquisitionHunterGoodDisplayName)
              .SetDescription(InquisitionHunterGoodDescription)
              .SetIcon(icon)
              .AddComponent<AddOutgoingDamageTriggerTTT>(c => { c.Actions = action; c.TargetKilledByThisDamage = true; })
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.InquisitorClass.ToString(), CharacterClassRefs.ClericClass.ToString() }, min: 1))
              .AddPrerequisiteNoFeature(FeatureRefs.EvilDomainAllowed.ToString())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string HunterEvil = "Inquisition.HunterEvil";
        private static readonly string HunterEvilGuid = "{97CE4717-8D06-4C91-AAC3-B88A6035DC8A}";

        private const string HunterEvilBuff = "Inquisition.HunterEvilBuff";
        private static readonly string HunterEvilBuffGuid = "{06AFFB6A-946C-42EA-BE41-87BA13B7C1A2}";

        internal const string InquisitionHunterEvilDisplayName = "InquisitionHunterEvil.Name";
        private const string InquisitionHunterEvilDescription = "InquisitionHunterEvil.Description";
        public static BlueprintFeature HunterEvilFeat()
        {
            var icon = AbilityRefs.VampiricShadowShield.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(HunterEvilBuff, HunterEvilBuffGuid)
              .SetDisplayName(InquisitionHunterEvilDisplayName)
              .SetDescription(InquisitionHunterEvilDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Profane, false, StatType.AC, 2)
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Minutes), toCaster: true)
                .Build();

            return FeatureConfigurator.New(HunterEvil, HunterEvilGuid)
              .SetDisplayName(InquisitionHunterEvilDisplayName)
              .SetDescription(InquisitionHunterEvilDescription)
              .SetIcon(icon)
              .AddComponent<AddOutgoingDamageTriggerTTT>(c => { c.Actions = action; c.TargetKilledByThisDamage = true; })
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.InquisitorClass.ToString(), CharacterClassRefs.ClericClass.ToString() }, min: 1))
              .AddPrerequisiteNoFeature(FeatureRefs.GoodDomainAllowed.ToString())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string Reformation = "Inquisition.Reformation";
        private static readonly string ReformationGuid = "{E0306AAC-6929-4459-811B-D2AD70878979}";

        internal const string ReformationDisplayName = "InquisitionReformation.Name";
        private const string ReformationDescription = "InquisitionReformation.Description";
        public static BlueprintProgression ReformationFeat()
        {
            var icon = FeatureRefs.CavalierBannerGreater.Reference.Get().Icon;

            var pro = ProgressionConfigurator.New(Reformation, ReformationGuid)
              .SetDisplayName(ReformationDisplayName)
              .SetDescription(ReformationDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ProgressionRefs.AirDomainProgression.Reference.Get().m_Classes)
              .SetArchetypes(ProgressionRefs.AirDomainProgression.Reference.Get().m_Archetypes)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToGroups(FeatureGroup.Domain)
              .AddToLevelEntry(1, InspiredRhetoricFeat())
              .AddToLevelEntry(4, BlessedCorrectionFeat())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DomainsSelection)
                .AddToAllFeatures(pro)
                .Configure();

            return pro;
        }

        private const string BlessedCorrection = "Inquisition.BlessedCorrection";
        public static readonly string BlessedCorrectionGuid = "{39E540A1-EAB7-4B8D-BD81-851D089C4BDA}";

        internal const string BlessedCorrectionDisplayName = "InquisitionBlessedCorrection.Name";
        private const string BlessedCorrectionDescription = "InquisitionBlessedCorrection.Description";

        private const string BlessedCorrectionBuff = "Inquisition.BlessedCorrectionBuff";
        private static readonly string BlessedCorrectionBuffGuid = "{AE2ECC69-5F8E-42DB-800C-E4DD7D9DDFBF}";

        private const string BlessedCorrectionRes = "Inquisition.BlessedCorrectionRes";
        private static readonly string BlessedCorrectionResGuid = "{8BF4D53D-A59F-4C66-903E-7CC852A47F22}";

        private const string BlessedCorrectionAbility = "Inquisition.BlessedCorrectionAbility";
        private static readonly string BlessedCorrectionAbilityGuid = "{D463B68A-9CD4-4196-8A6F-B0053FB5FEA4}";
        public static BlueprintFeature BlessedCorrectionFeat()
        {
            var icon = FeatureRefs.Deceitful.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(BlessedCorrectionRes, BlessedCorrectionResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(0)
                    .IncreaseByStat(StatType.Wisdom))
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(BlessedCorrectionAbility, BlessedCorrectionAbilityGuid)
                .SetDisplayName(BlessedCorrectionDisplayName)
                .SetDescription(BlessedCorrectionDescription)
                .SetIcon(icon)
                .SetBuff(BlessedCorrectionBuffGuid)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound)
                .Configure();

            BuffConfigurator.New(BlessedCorrectionBuff, BlessedCorrectionBuffGuid)
             .SetDisplayName(BlessedCorrectionDisplayName)
             .SetDescription(BlessedCorrectionDescription)
             .SetIcon(icon)
             .AddModifyD20(replace: false, rollsAmount: 1, takeBest: true, rule: RuleType.SkillCheck, specificSkill: true, skill: new StatType[] { StatType.CheckIntimidate, StatType.CheckDiplomacy })
             .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
             .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
             .Configure();

            return FeatureConfigurator.New(BlessedCorrection, BlessedCorrectionGuid)
              .SetDisplayName(BlessedCorrectionDisplayName)
              .SetDescription(BlessedCorrectionDescription)
              .SetIcon(icon)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string InspiredRhetoric = "Inquisition.InspiredRhetoric";
        private static readonly string InspiredRhetoricGuid = "{CC6324D1-47BB-4465-81FD-AE5EA293B9A8}";

        internal const string InquisitionInspiredRhetoricDisplayName = "InquisitionInspiredRhetoric.Name";
        private const string InquisitionInspiredRhetoricDescription = "InquisitionInspiredRhetoric.Description";
        public static BlueprintFeature InspiredRhetoricFeat()
        {
            var icon = FeatureRefs.Persuasive.Reference.Get().Icon;

            return FeatureConfigurator.New(InspiredRhetoric, InspiredRhetoricGuid)
              .SetDisplayName(InquisitionInspiredRhetoricDisplayName)
              .SetDescription(InquisitionInspiredRhetoricDescription)
              .SetIcon(icon)
              .AddReplaceStatBaseAttribute(StatType.Wisdom, true, Kingmaker.UnitLogic.Buffs.BonusMod.AsIs, StatType.SkillPersuasion)
              .Configure();
        }

        private const string Tactics = "Inquisition.Tactics";
        private static readonly string TacticsGuid = "{02FD81B6-5F1D-454E-860F-A5A02A2CC13D}";

        internal const string TacticsDisplayName = "InquisitionTactics.Name";
        private const string TacticsDescription = "InquisitionTactics.Description";
        public static BlueprintProgression TacticsFeat()
        {
            var icon = FeatureRefs.TacticalLeaderFeatShareSwift.Reference.Get().Icon;

            var pro = ProgressionConfigurator.New(Tactics, TacticsGuid)
              .SetDisplayName(TacticsDisplayName)
              .SetDescription(TacticsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ProgressionRefs.AirDomainProgression.Reference.Get().m_Classes)
              .SetArchetypes(ProgressionRefs.AirDomainProgression.Reference.Get().m_Archetypes)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToGroups(FeatureGroup.Domain)
              .AddPrerequisiteFeature(FeatureRefs.ToragFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.IroriFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.GorumFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddToLevelEntry(1, DirectionFeat())
              .AddToLevelEntry(8, GrantInitiativeFeat())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DomainsSelection)
                .AddToAllFeatures(pro)
                .Configure();

            return pro;
        }

        private static readonly string GrantInitiativeName = "InquisitionGrantInitiative";
        public static readonly string GrantInitiativeGuid = "{66020E49-93C3-491D-9CAB-D1DD71244682}";

        private static readonly string GrantInitiativeDisplayName = "InquisitionGrantInitiative.Name";
        private static readonly string GrantInitiativeDescription = "InquisitionGrantInitiative.Description";

        private const string AuraBuff = "Inquisition.GrantInitiativebuff";
        private static readonly string AuraBuffGuid = "{42F325F7-6A58-45B1-B239-118D2BD0EBBF}";

        private const string AuraBuff2 = "Inquisition.GrantInitiativebuff2";
        private static readonly string AuraBuff2Guid = "{25B529C5-8566-4FD5-A8AB-1BD6A7831BD8}";

        private const string GrantInitiativeAura = "Inquisition.GrantInitiativeAura";
        private static readonly string GrantInitiativeAuraGuid = "{7CC9637F-8DD8-4BD1-8AFB-DFDEDEFD5FA1}";

        public static BlueprintFeature GrantInitiativeFeat()
        {
            var icon = AbilityRefs.FeatherStepMass.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AuraBuff2, AuraBuff2Guid)
              .SetDisplayName(GrantInitiativeDisplayName)
              .SetDescription(GrantInitiativeDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.Initiative, ContextValues.Property(UnitProperty.StatBonusWisdom, true))
              .Configure();

            var area = AbilityAreaEffectConfigurator.New(GrantInitiativeAura, GrantInitiativeAuraGuid)
                .SetTargetType(BlueprintAbilityAreaEffect.TargetType.Ally)
                .SetAffectDead(false)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(33.Feet())
                .AddAbilityAreaEffectBuff(Buff2, false, ConditionsBuilder.New().TargetIsYourself(true).Build())
                .Configure();

            var Buff1 = BuffConfigurator.New(AuraBuff, AuraBuffGuid)
              .SetDisplayName(GrantInitiativeDisplayName)
              .SetDescription(GrantInitiativeDescription)
              .SetIcon(icon)
              .AddComponent<RecalculateEachRound>()
              .AddAreaEffect(area)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            return FeatureConfigurator.New(GrantInitiativeName, GrantInitiativeGuid)
                    .SetDisplayName(GrantInitiativeDisplayName)
                    .SetDescription(GrantInitiativeDescription)
                    .SetIcon(icon)
                    .AddAuraFeatureComponent(Buff1)
                    .AddFacts(new() { FeatureRefs.CunningInitiative.ToString() })
                    .Configure();
        }

        private const string Direction = "Inquisition.Direction";
        private static readonly string DirectionGuid = "{4AF21068-0395-4009-8AA3-34494BAA9C78}";

        internal const string DirectionDisplayName = "InquisitionDirection.Name";
        private const string DirectionDescription = "InquisitionDirection.Description";

        private const string DirectionAbility = "Inquisition.DirectionAbility";
        private static readonly string DirectionAbilityGuid = "{F11F68EA-53CA-4A69-8070-E314E42AAB93}";

        private const string DirectionAbilityRes = "Inquisition.DirectionAbilityRes";
        private static readonly string DirectionAbilityResGuid = "{614F6273-9730-4070-BFB6-5E53C0172F08}";

        private const string DirectionCooldownBuff = "Inquisition.DirectionCooldownBuff";
        private static readonly string DirectionCooldownBuffGuid = "{2E9F847F-CCA9-4A5F-A765-9F15EC897DA9}";
        public static BlueprintFeature DirectionFeat()
        {
            var icon = FeatureRefs.FreebootersBondFeature.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(DirectionAbilityRes, DirectionAbilityResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(3).IncreaseByStat(StatType.Wisdom))
                .Configure();

            var CooldownBuff = BuffConfigurator.New(DirectionCooldownBuff, DirectionCooldownBuffGuid)
                .SetDisplayName(DirectionDisplayName)
                .SetDescription(DirectionDescription)
                .SetIcon(icon)
                .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
                .AddCondition(Kingmaker.UnitLogic.UnitCondition.DisableAttacksOfOpportunity)
                .Configure();

            var ability = AbilityConfigurator.New(DirectionAbility, DirectionAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .Conditional(ConditionsBuilder.New().HasBuff(BuffRefs.SlowBuff.ToString()).Build(),
                        ifTrue: ActionsBuilder.New().RemoveBuff(BuffRefs.SlowBuff.ToString()).Build(),
                        ifFalse: ActionsBuilder.New().ApplyBuff(BuffRefs.HasteBuff.ToString(), ContextDuration.Fixed(1)).Build())
                        .ApplyBuff(CooldownBuff, ContextDuration.Fixed(1), toCaster: true)
                        .Build())
                .SetDisplayName(DirectionDisplayName)
                .SetDescription(DirectionDescription)
                .SetIcon(icon)
                .AddComponent(AbilityRefs.Haste.Reference.Get().GetComponent<AbilitySpawnFx>())
                .AllowTargeting(false, false, true, false)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Supernatural)
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(Direction, DirectionGuid)
              .SetDisplayName(DirectionDisplayName)
              .SetDescription(DirectionDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string Hammer = "Inquisition.Hammer";
        private static readonly string HammerGuid = "{8D5E3BA4-F6FC-4367-9C54-3A1D824F738C}";

        internal const string HammerDisplayName = "InquisitionHammer.Name";
        private const string HammerDescription = "InquisitionHammer.Description";
        public static BlueprintProgression HammerFeat()
        {
            var icon = AbilityRefs.ChaosHammer.Reference.Get().Icon;

            var pro = ProgressionConfigurator.New(Hammer, HammerGuid)
              .SetDisplayName(HammerDisplayName)
              .SetDescription(HammerDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ProgressionRefs.CavalierOrderOfTheLionProgression.Reference.Get().m_Classes)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, HammerChallengeFeat(), HammerSkillFeat())
              .AddToLevelEntry(2, FeatureRefs.ImprovedUnarmedStrike.ToString())
              .AddToLevelEntry(8, BodyGuard.GreaterUnarmedStrikeGuid)
              .AddToLevelEntry(15, HammerFlexFeat())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.CavalierOrderSelection)
                .AddToAllFeatures(pro)
                .Configure();

            return pro;
        }

        private const string HammerSkill = "Inquisition.HammerSkill";
        private static readonly string HammerSkillGuid = "{338CE0C4-74BC-4B69-9E90-C958AF5F64AE}";

        internal const string InquisitionHammerSkillDisplayName = "InquisitionHammerSkill.Name";
        private const string InquisitionHammerSkillDescription = "InquisitionHammerSkill.Description";
        public static BlueprintFeature HammerSkillFeat()
        {
            //var icon = FeatureRefs.Persuasive.Reference.Get().Icon;

            return FeatureConfigurator.New(HammerSkill, HammerSkillGuid)
              .SetDisplayName(InquisitionHammerSkillDisplayName)
              .SetDescription(InquisitionHammerSkillDescription)
              //.SetIcon(icon)
              .AddComponent(FeatureRefs.CavalierLionSkills.Reference.Get().GetComponent<AddClassSkill>())
              .Configure();
        }

        private static readonly string HammerFlexName = "InquisitionHammerFlex";
        public static readonly string HammerFlexGuid = "{5FF16D00-11CC-4A97-836F-05371AA06BEA}";

        private static readonly string HammerFlexDisplayName = "InquisitionHammerFlex.Name";
        private static readonly string HammerFlexDescription = "InquisitionHammerFlex.Description";

        private const string FlexAuraBuff = "Inquisition.HammerFlexbuff";
        private static readonly string FlexAuraBuffGuid = "{45D16FEA-2FE4-4C43-8FE7-DF8F52E14CAD}";

        private const string FlexAuraBuff2 = "Inquisition.HammerFlexbuff2";
        private static readonly string FlexAuraBuff2Guid = "{3A32602E-BF34-4F91-AD4C-68C0FC3F3547}";

        private const string HammerFlexFlexAura = "Inquisition.HammerFlexFlexAura";
        private static readonly string HammerFlexFlexAuraGuid = "{FADB840F-DEA9-4381-9B6B-76D7EA03AA95}";

        private const string HammerFlexAbility = "Inquisition.HammerFlexAbility";
        private static readonly string HammerFlexAbilityGuid = "{48056153-9B20-4ECB-B979-F2BD4376FDC4}";

        private const string HammerFlexAbilityRes = "Inquisition.HammerFlexAbilityRes";
        private static readonly string HammerFlexAbilityResGuid = "{92B9FA8E-C0FA-4CB8-841F-465D7E33C5FD}";

        public static BlueprintFeature HammerFlexFeat()
        {
            var icon = AbilityRefs.HeroNeverSurrender.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(FlexAuraBuff2, FlexAuraBuff2Guid)
              .SetDisplayName(HammerFlexDisplayName)
              .SetDescription(HammerFlexDescription)
              .SetIcon(icon)
              .AddAttackTypeAttackBonus(attackBonus: 4, descriptor: ModifierDescriptor.Morale, type: WeaponRangeType.Melee)
              .AddCMBBonus(descriptor: ModifierDescriptor.Morale, value: 4)
              .AddStatBonus(ModifierDescriptor.Morale, false, StatType.SaveFortitude, 4)
              .AddStatBonus(ModifierDescriptor.Morale, false, StatType.SkillAthletics, 4)
              .SetFxOnStart(BuffRefs.HeroismBuff.Reference.Get().FxOnStart)
              .Configure(); 
            
            var area = AbilityAreaEffectConfigurator.New(HammerFlexFlexAura, HammerFlexFlexAuraGuid)
                .SetTargetType(BlueprintAbilityAreaEffect.TargetType.Ally)
                .SetAffectDead(false)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(33.Feet())
                .AddAbilityAreaEffectBuff(BuffRefs.SpellResistanceBuff.ToString())
                .AddContextCalculateAbilityParams(replaceCasterLevel: true, casterLevel: ContextValues.Property(UnitProperty.Level))
                .Configure();

            var Buff1 = BuffConfigurator.New(FlexAuraBuff, FlexAuraBuffGuid)
              .SetDisplayName(HammerFlexDisplayName)
              .SetDescription(HammerFlexDescription)
              .SetIcon(icon)
              .AddAreaEffect(area)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityresourse = AbilityResourceConfigurator.New(HammerFlexAbilityRes, HammerFlexAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { CharacterClassRefs.CavalierClass.ToString() }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1))
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(HammerFlexAbility, HammerFlexAbilityGuid)
                .SetDisplayName(HammerFlexDisplayName)
                .SetDescription(HammerFlexDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateIfCombatEnded(true)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound)
                .Configure();

            return FeatureConfigurator.New(HammerFlexName, HammerFlexGuid)
                    .SetDisplayName(HammerFlexDisplayName)
                    .SetDescription(HammerFlexDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .Configure();
        }

        private const string HammerChallenge = "Inquisition.HammerChallenge";
        public static readonly string HammerChallengeGuid = "{B51FD722-D931-4AC8-82FF-B4B1CA72449E}";

        internal const string InquisitionHammerChallengeDisplayName = "InquisitionHammerChallenge.Name";
        private const string InquisitionHammerChallengeDescription = "InquisitionHammerChallenge.Description";

        private const string ChallengeAuraBuff = "Inquisition.HammerChallengebuff";
        public static readonly string ChallengeAuraBuffGuid = "{406ED37D-681B-40F8-A06A-74BA78312080}";
        public static BlueprintFeature HammerChallengeFeat()
        {
            //var icon = FeatureRefs.Persuasive.Reference.Get().Icon;
            BuffConfigurator.New(ChallengeAuraBuff, ChallengeAuraBuffGuid)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            return FeatureConfigurator.New(HammerChallenge, HammerChallengeGuid)
              .SetDisplayName(InquisitionHammerChallengeDisplayName)
              .SetDescription(InquisitionHammerChallengeDescription)
              //.SetIcon(icon)
              .AddFacts(new() { SeizetheOpportunity.ManeuverGuid })
              .Configure();
        }
    }
}
