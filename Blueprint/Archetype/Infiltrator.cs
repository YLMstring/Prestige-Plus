using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Classes.Prerequisites;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Infiltrator
    {
        private const string ArchetypeName = "Infiltrator";
        private static readonly string ArchetypeGuid = "{1445CD8D-3330-45E8-9425-DBB7932581A9}";
        internal const string ArchetypeDisplayName = "Infiltrator.Name";
        private const string ArchetypeDescription = "Infiltrator.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.RangerClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(3, FeatureSelectionRefs.FavoriteTerrainSelection.ToString())
            .SetRemoveFeaturesEntry(8, FeatureSelectionRefs.FavoriteTerrainSelection.ToString(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
            .SetRemoveFeaturesEntry(13, FeatureSelectionRefs.FavoriteTerrainSelection.ToString(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
            .SetRemoveFeaturesEntry(18, FeatureSelectionRefs.FavoriteTerrainSelection.ToString(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
            .SetRemoveFeaturesEntry(12, FeatureRefs.Camouflage.ToString())
            .AddToAddFeatures(3, CreateAdaptation())
            .AddToAddFeatures(8, AdaptationGuid)
            .AddToAddFeatures(13, AdaptationGuid)
            .AddToAddFeatures(18, AdaptationGuid)
              .Configure();
        }


        private const string AdaptationAblityRes = "InfiltratorAdaptationAblityRes";
        private static readonly string AdaptationAblityResGuid = "{7840CAE5-3AB8-4186-9E81-7D0ADC5B6611}";

        private const string Adaptation = "Infiltrator.Adaptation";
        private static readonly string AdaptationGuid = "{7AA31804-1D6F-47BB-ADCF-D800F9FF902B}";

        internal const string AdaptationDisplayName = "InfiltratorAdaptation.Name";
        private const string AdaptationDescription = "InfiltratorAdaptation.Description";

        private static BlueprintFeatureSelection CreateAdaptation()
        {
            var icon = FeatureRefs.AnimalBlessingMajorFeature.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(AdaptationAblityRes, AdaptationAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevel(new string[] { CharacterClassRefs.RangerClass.ToString() }))
                .Configure();

            return FeatureSelectionConfigurator.New(Adaptation, AdaptationGuid)
              .SetDisplayName(AdaptationDisplayName)
              .SetDescription(AdaptationDescription)
              .SetIcon(icon)
              .AddToAllFeatures(AdaptationAnimalsFeat())
              .AddToAllFeatures(AdaptationDragonFeat())
              .AddToAllFeatures(AdaptationDwarfsFeat())
              .AddToAllFeatures(AdaptationElfsFeat())
              .AddToAllFeatures(AdaptationGiantsFeat())
              .AddToAllFeatures(AdaptationGnomeFeat())
              .AddToAllFeatures(AdaptationGoblinsFeat())
              .AddToAllFeatures(AdaptationHalflingFeat())
              .AddToAllFeatures(AdaptationOutsiderFeat())
              .AddToAllFeatures(AdaptationVerminFeat())
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string AdaptationDragon = "Infiltrator.AdaptationDragon";
        public static readonly string AdaptationDragonGuid = "{FC4401A1-85B0-40B5-BAB8-293F29168663}";

        private const string AdaptationDragonAblity = "Infiltrator.UseAdaptationDragon";
        private static readonly string AdaptationDragonAblityGuid = "{405E6BE8-6D1D-4BE0-B611-FEC8A8398CA6}";

        private const string AdaptationDragonBuff2 = "Infiltrator.AdaptationDragonBuff2";
        private static readonly string AdaptationDragonBuff2Guid = "{D1B1C153-F5A3-4EB4-8199-E9E2C312A66C}";

        internal const string AdaptationDragonDisplayName = "InfiltratorAdaptationDragon.Name";
        private const string AdaptationDragonDescription = "InfiltratorAdaptationDragon.Description";

        public static BlueprintFeature AdaptationDragonFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyDragons.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationDragonBuff2, AdaptationDragonBuff2Guid)
             .SetDisplayName(AdaptationDragonDisplayName)
             .SetDescription(AdaptationDragonDescription)
             .SetIcon(icon)
             .AddFacts(new() { FeatureRefs.GreatFortitude.ToString(), FeatureRefs.IronWill.ToString(), FeatureRefs.LightningReflexes.ToString(), FeatureRefs.LungeFeature.ToString() })
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationDragonAblity, AdaptationDragonAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationDragonDisplayName)
                .SetDescription(AdaptationDragonDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationDragon, AdaptationDragonGuid)
              .SetDisplayName(AdaptationDragonDisplayName)
              .SetDescription(AdaptationDragonDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyDragons.ToString())
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AdaptationOutsider = "Infiltrator.AdaptationOutsider";
        public static readonly string AdaptationOutsiderGuid = "{5DC80F65-5BCC-46E8-B9C4-CC210F36149A}";

        private const string AdaptationOutsiderAblity = "Infiltrator.UseAdaptationOutsider";
        private static readonly string AdaptationOutsiderAblityGuid = "{A08E9085-F119-4C0F-A87F-A627A781E04B}";

        private const string AdaptationOutsiderBuff2 = "Infiltrator.AdaptationOutsiderBuff2";
        private static readonly string AdaptationOutsiderBuff2Guid = "{73678AF1-59F0-42BA-846C-71554E392374}";

        internal const string AdaptationOutsiderDisplayName = "InfiltratorAdaptationOutsider.Name";
        private const string AdaptationOutsiderDescription = "InfiltratorAdaptationOutsider.Description";

        public static BlueprintFeature AdaptationOutsiderFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyOutsider.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationOutsiderBuff2, AdaptationOutsiderBuff2Guid)
             .SetDisplayName(AdaptationOutsiderDisplayName)
             .SetDescription(AdaptationOutsiderDescription)
             .SetIcon(icon)
             .AddFacts(new() { FeatureRefs.GreatFortitude.ToString(), FeatureRefs.IronWill.ToString(), FeatureRefs.LightningReflexes.ToString() })
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationOutsiderAblity, AdaptationOutsiderAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationOutsiderDisplayName)
                .SetDescription(AdaptationOutsiderDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationOutsider, AdaptationOutsiderGuid)
              .SetDisplayName(AdaptationOutsiderDisplayName)
              .SetDescription(AdaptationOutsiderDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyOutsider.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyDemonOfMagic.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyDemonOfSlaughter.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyDemonOfStrength.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyFey.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyHuman.ToString(), group: Prerequisite.GroupType.Any)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AdaptationHalfling = "Infiltrator.AdaptationHalfling";
        public static readonly string AdaptationHalflingGuid = "{BDAE24EC-D503-4954-B497-6B441665C658}";

        private const string AdaptationHalflingAblity = "Infiltrator.UseAdaptationHalfling";
        private static readonly string AdaptationHalflingAblityGuid = "{565432CC-856B-43B4-B40A-4B5AC9AAF190}";

        private const string AdaptationHalflingBuff2 = "Infiltrator.AdaptationHalflingBuff2";
        private static readonly string AdaptationHalflingBuff2Guid = "{634AC548-39DC-4D58-9E74-A0124242B9DE}";

        internal const string AdaptationHalflingDisplayName = "InfiltratorAdaptationHalfling.Name";
        private const string AdaptationHalflingDescription = "InfiltratorAdaptationHalfling.Description";

        public static BlueprintFeature AdaptationHalflingFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyHalfling.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationHalflingBuff2, AdaptationHalflingBuff2Guid)
             .SetDisplayName(AdaptationHalflingDisplayName)
             .SetDescription(AdaptationHalflingDescription)
             .SetIcon(icon)
             .AddFacts(new() { FeatureRefs.Fearless.ToString(), FeatureRefs.HalflingLuck.ToString(), FeatureRefs.SureFooted.ToString(), FeatureRefs.KeenSenses.ToString() })
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationHalflingAblity, AdaptationHalflingAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationHalflingDisplayName)
                .SetDescription(AdaptationHalflingDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationHalfling, AdaptationHalflingGuid)
              .SetDisplayName(AdaptationHalflingDisplayName)
              .SetDescription(AdaptationHalflingDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyHalfling.ToString())
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AdaptationDwarfs = "Infiltrator.AdaptationDwarfs";
        public static readonly string AdaptationDwarfsGuid = "{AA22BDB9-3A8E-44B6-B0EB-DCBF634F9BD5}";

        private const string AdaptationDwarfsAblity = "Infiltrator.UseAdaptationDwarfs";
        private static readonly string AdaptationDwarfsAblityGuid = "{F7B54C1F-B111-4146-8BBF-A40BDCE5B7CF}";

        private const string AdaptationDwarfsBuff2 = "Infiltrator.AdaptationDwarfsBuff2";
        private static readonly string AdaptationDwarfsBuff2Guid = "{2D425D4E-F4CA-4B41-805D-B3E299882E2E}";

        internal const string AdaptationDwarfsDisplayName = "InfiltratorAdaptationDwarfs.Name";
        private const string AdaptationDwarfsDescription = "InfiltratorAdaptationDwarfs.Description";

        public static BlueprintFeature AdaptationDwarfsFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyDwarfs.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationDwarfsBuff2, AdaptationDwarfsBuff2Guid)
             .SetDisplayName(AdaptationDwarfsDisplayName)
             .SetDescription(AdaptationDwarfsDescription)
             .SetIcon(icon)
             .AddFacts(new() { FeatureRefs.Stability.ToString(), FeatureRefs.Hardy.ToString(), FeatureRefs.DwarfDefensiveTrainingGiants.ToString(), FeatureRefs.HatredGoblinoidOrc.ToString() })
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationDwarfsAblity, AdaptationDwarfsAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationDwarfsDisplayName)
                .SetDescription(AdaptationDwarfsDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationDwarfs, AdaptationDwarfsGuid)
              .SetDisplayName(AdaptationDwarfsDisplayName)
              .SetDescription(AdaptationDwarfsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyDwarfs.ToString())
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AdaptationElfs = "Infiltrator.AdaptationElfs";
        public static readonly string AdaptationElfsGuid = "{E404130A-856E-44DE-B3B4-658D4D1C9B07}";

        private const string AdaptationElfsAblity = "Infiltrator.UseAdaptationElfs";
        private static readonly string AdaptationElfsAblityGuid = "{A93E28B5-CB5C-4337-BDE7-E176C823353D}";

        private const string AdaptationElfsBuff2 = "Infiltrator.AdaptationElfsBuff2";
        private static readonly string AdaptationElfsBuff2Guid = "{A7691110-0A4C-4D1B-A4A3-C025F6F72BD0}";

        internal const string AdaptationElfsDisplayName = "InfiltratorAdaptationElfs.Name";
        private const string AdaptationElfsDescription = "InfiltratorAdaptationElfs.Description";

        public static BlueprintFeature AdaptationElfsFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyElfs.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationElfsBuff2, AdaptationElfsBuff2Guid)
             .SetDisplayName(AdaptationElfsDisplayName)
             .SetDescription(AdaptationElfsDescription)
             .SetIcon(icon)
             .AddFacts(new() { FeatureRefs.ElvenImmunities.ToString(), FeatureRefs.ElvenMagic.ToString(), FeatureRefs.ElvenArcaneFocus.ToString(), FeatureRefs.KeenSenses.ToString() })
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationElfsAblity, AdaptationElfsAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationElfsDisplayName)
                .SetDescription(AdaptationElfsDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationElfs, AdaptationElfsGuid)
              .SetDisplayName(AdaptationElfsDisplayName)
              .SetDescription(AdaptationElfsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyElfs.ToString())
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AdaptationGnome = "Infiltrator.AdaptationGnome";
        public static readonly string AdaptationGnomeGuid = "{944EF4FD-C79B-49AA-AD87-7F87CE192C0C}";

        private const string AdaptationGnomeAblity = "Infiltrator.UseAdaptationGnome";
        private static readonly string AdaptationGnomeAblityGuid = "{3BB40560-D0B1-4788-A5BC-D233A7A681E4}";

        private const string AdaptationGnomeBuff2 = "Infiltrator.AdaptationGnomeBuff2";
        private static readonly string AdaptationGnomeBuff2Guid = "{180EE748-B8D0-4404-9767-108F004C06A6}";

        internal const string AdaptationGnomeDisplayName = "InfiltratorAdaptationGnome.Name";
        private const string AdaptationGnomeDescription = "InfiltratorAdaptationGnome.Description";

        public static BlueprintFeature AdaptationGnomeFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyGnome.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationGnomeBuff2, AdaptationGnomeBuff2Guid)
             .SetDisplayName(AdaptationGnomeDisplayName)
             .SetDescription(AdaptationGnomeDescription)
             .SetIcon(icon)
             .AddFacts(new() { FeatureRefs.KeenSenses.ToString(), FeatureRefs.GnomeMagic.ToString(), FeatureRefs.GnomeDefensiveTrainingGiants.ToString(), FeatureRefs.HatredReptilian.ToString(), FeatureRefs.IllusionResistance.ToString(), FeatureRefs.Obsessive.ToString() })
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationGnomeAblity, AdaptationGnomeAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationGnomeDisplayName)
                .SetDescription(AdaptationGnomeDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationGnome, AdaptationGnomeGuid)
              .SetDisplayName(AdaptationGnomeDisplayName)
              .SetDescription(AdaptationGnomeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyGnome.ToString())
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AdaptationAnimals = "Infiltrator.AdaptationAnimals";
        public static readonly string AdaptationAnimalsGuid = "{7202A539-B2EC-4432-9C11-2C01D47F8B65}";

        private const string AdaptationAnimalsAblity = "Infiltrator.UseAdaptationAnimals";
        private static readonly string AdaptationAnimalsAblityGuid = "{B5EA4170-75C2-4B01-89D0-F5A4FE59EFED}";

        private const string AdaptationAnimalsBuff2 = "Infiltrator.AdaptationAnimalsBuff2";
        private static readonly string AdaptationAnimalsBuff2Guid = "{25F3018A-A8B9-4767-83DE-A1C568451ADA}";

        internal const string AdaptationAnimalsDisplayName = "InfiltratorAdaptationAnimals.Name";
        private const string AdaptationAnimalsDescription = "InfiltratorAdaptationAnimals.Description";

        public static BlueprintFeature AdaptationAnimalsFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyAnimals.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationAnimalsBuff2, AdaptationAnimalsBuff2Guid)
             .SetDisplayName(AdaptationAnimalsDisplayName)
             .SetDescription(AdaptationAnimalsDescription)
             .SetIcon(icon)
             .AddFacts(new() { FeatureRefs.Intimidating.ToString(), FeatureRefs.HalfOrcFerocity.ToString() })
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationAnimalsAblity, AdaptationAnimalsAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationAnimalsDisplayName)
                .SetDescription(AdaptationAnimalsDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationAnimals, AdaptationAnimalsGuid)
              .SetDisplayName(AdaptationAnimalsDisplayName)
              .SetDescription(AdaptationAnimalsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyAnimals.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyAberrations.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyMagicalBeasts.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyMonstrousHumanoid.ToString(), group: Prerequisite.GroupType.Any)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AdaptationGoblins = "Infiltrator.AdaptationGoblins";
        public static readonly string AdaptationGoblinsGuid = "{0332F810-AD71-4C0A-981D-C8FF9BE8C05B}";

        private const string AdaptationGoblinsAblity = "Infiltrator.UseAdaptationGoblins";
        private static readonly string AdaptationGoblinsAblityGuid = "{EA82727B-BF93-479F-A537-29B3452397B4}";

        private const string AdaptationGoblinsBuff2 = "Infiltrator.AdaptationGoblinsBuff2";
        private static readonly string AdaptationGoblinsBuff2Guid = "{25BF112F-FFEA-490F-B279-F8221A1B1375}";

        internal const string AdaptationGoblinsDisplayName = "InfiltratorAdaptationGoblins.Name";
        private const string AdaptationGoblinsDescription = "InfiltratorAdaptationGoblins.Description";

        public static BlueprintFeature AdaptationGoblinsFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyGoblins.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationGoblinsBuff2, AdaptationGoblinsBuff2Guid)
             .SetDisplayName(AdaptationGoblinsDisplayName)
             .SetDescription(AdaptationGoblinsDescription)
             .SetIcon(icon)
             .AddStatBonus(ModifierDescriptor.NaturalArmor, false, StatType.AC, 2)
             .AddBuffMovementSpeed(value: 10)
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationGoblinsAblity, AdaptationGoblinsAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationGoblinsDisplayName)
                .SetDescription(AdaptationGoblinsDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationGoblins, AdaptationGoblinsGuid)
              .SetDisplayName(AdaptationGoblinsDisplayName)
              .SetDescription(AdaptationGoblinsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyGoblins.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyReptilian.ToString(), group: Prerequisite.GroupType.Any)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AdaptationGiants = "Infiltrator.AdaptationGiants";
        public static readonly string AdaptationGiantsGuid = "{298A61DF-1449-405C-ACE8-46885A002D20}";

        private const string AdaptationGiantsAblity = "Infiltrator.UseAdaptationGiants";
        private static readonly string AdaptationGiantsAblityGuid = "{5D8083E0-E9A0-48D0-855A-6FF609331EF5}";

        private const string AdaptationGiantsBuff2 = "Infiltrator.AdaptationGiantsBuff2";
        private static readonly string AdaptationGiantsBuff2Guid = "{2213112F-0B88-4E7D-9841-B5E34C420253}";

        internal const string AdaptationGiantsDisplayName = "InfiltratorAdaptationGiants.Name";
        private const string AdaptationGiantsDescription = "InfiltratorAdaptationGiants.Description";

        public static BlueprintFeature AdaptationGiantsFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyGiants.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationGiantsBuff2, AdaptationGiantsBuff2Guid)
             .SetDisplayName(AdaptationGiantsDisplayName)
             .SetDescription(AdaptationGiantsDescription)
             .SetIcon(icon)
             .AddFacts(new() { FeatureRefs.LungeFeature.ToString(), FeatureRefs.ThrowAnything.ToString() })
             .AddStatBonus(ModifierDescriptor.NaturalArmor, false, StatType.AC, 2)
             .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Constant(5), type: Kingmaker.Enums.Damage.DamageEnergyType.Fire)
             .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Constant(5), type: Kingmaker.Enums.Damage.DamageEnergyType.Cold)
             .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Constant(5), type: Kingmaker.Enums.Damage.DamageEnergyType.Electricity)
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationGiantsAblity, AdaptationGiantsAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationGiantsDisplayName)
                .SetDescription(AdaptationGiantsDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationGiants, AdaptationGiantsGuid)
              .SetDisplayName(AdaptationGiantsDisplayName)
              .SetDescription(AdaptationGiantsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyGiants.ToString())
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AdaptationVermin = "Infiltrator.AdaptationVermin";
        public static readonly string AdaptationVerminGuid = "{D4E856B8-0D31-4FFF-AE1D-2E6C0534A79B}";

        private const string AdaptationVerminAblity = "Infiltrator.UseAdaptationVermin";
        private static readonly string AdaptationVerminAblityGuid = "{5D26C3E8-63F1-4CEE-ADBC-309DBD47742B}";

        private const string AdaptationVerminBuff2 = "Infiltrator.AdaptationVerminBuff2";
        private static readonly string AdaptationVerminBuff2Guid = "{321AA3E4-93A1-4F79-8170-D719FBCD1AE2}";

        internal const string AdaptationVerminDisplayName = "InfiltratorAdaptationVermin.Name";
        private const string AdaptationVerminDescription = "InfiltratorAdaptationVermin.Description";

        public static BlueprintFeature AdaptationVerminFeat()
        {
            var icon = FeatureRefs.FavoriteEnemyVermin.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AdaptationVerminBuff2, AdaptationVerminBuff2Guid)
             .SetDisplayName(AdaptationVerminDisplayName)
             .SetDescription(AdaptationVerminDescription)
             .SetIcon(icon)
             .AddFacts(new() { FeatureRefs.GreatFortitude.ToString(), FeatureRefs.IronWill.ToString() })
             .AddStatBonus(ModifierDescriptor.NaturalArmor, false, StatType.AC, 2)
             .Configure();

            var ability = AbilityConfigurator.New(AdaptationVerminAblity, AdaptationVerminAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(AdaptationVerminDisplayName)
                .SetDescription(AdaptationVerminDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AdaptationAblityResGuid)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(AdaptationVermin, AdaptationVerminGuid)
              .SetDisplayName(AdaptationVerminDisplayName)
              .SetDescription(AdaptationVerminDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyVermin.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyConstructs.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyPlant.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyUndead.ToString(), group: Prerequisite.GroupType.Any)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
