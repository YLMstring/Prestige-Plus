using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Craft;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Spell;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;

namespace PrestigePlus.Blueprint
{
    [TypeId("{2B304194-B7F4-4EA0-8D2B-FAEFF51C2169}")]
    internal class MasterpieceLogic : UnitFactComponentDelegate, ILevelUpCompleteUIHandler, IGlobalSubscriber, ISubscriber
    {
        public override void OnTurnOn()
        {
            
        }
        public override void OnTurnOff()
        {
            
        }
        private void Apply()
        {
            var book = Owner.GetSpellbook(CharacterClassRefs.BardClass.Reference);
            book ??= Owner.GetSpellbook(CharacterClassRefs.BardClass_Penta.Reference);
            if (book == null) return;
            var songs = new List<BlueprintUnitFact>() { };
            if (book.IsKnown(BlueprintTool.GetRef<BlueprintAbilityReference>(Masterpiece.TripleTimeAblityGuid)))
            {
                songs.Add(BlueprintTool.GetRef<BlueprintUnitFactReference>(Masterpiece.TripleTimeGuid));
            }
            if (book.IsKnown(BlueprintTool.GetRef<BlueprintAbilityReference>(Masterpiece.TwistingSteelAblityGuid)))
            {
                songs.Add(BlueprintTool.GetRef<BlueprintUnitFactReference>(Masterpiece.TwistingSteelGuid));
            }
            if (book.IsKnown(BlueprintTool.GetRef<BlueprintAbilityReference>(Masterpiece.RatQuadrilleAblityGuid)))
            {
                songs.Add(BlueprintTool.GetRef<BlueprintUnitFactReference>(Masterpiece.RatQuadrilleGuid));
            }
            if (book.IsKnown(BlueprintTool.GetRef<BlueprintAbilityReference>(Masterpiece.StoneFaceAblityGuid)))
            {
                songs.Add(BlueprintTool.GetRef<BlueprintUnitFactReference>(Masterpiece.StoneFaceGuid));
            }
            if (book.IsKnown(BlueprintTool.GetRef<BlueprintAbilityReference>(Masterpiece.PriestKingAblityGuid)))
            {
                songs.Add(BlueprintTool.GetRef<BlueprintUnitFactReference>(Masterpiece.PriestKingGuid));
            }
            if (songs.Count == 0) { return; }
            foreach (var song in songs)
            {
                if (!Owner.HasFact(song) && song is BlueprintFeature feat && feat.MeetsPrerequisites(null, Owner, null, false, false))
                {
                    Owner.AddFact(song, null, null);
                }
            }
        }

        void ILevelUpCompleteUIHandler.HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
        {
            if (unit == Owner)
            {
                Apply();
            }
        }
    }
    internal class Masterpiece
    {
        private const string BardMasterpieceCore = "BardMasterpiece.BardMasterpieceCore";
        public static readonly string BardMasterpieceCoreGuid = "{0901EA8F-1CAE-465D-A272-BAB8FEE863E4}";
        public static void CreateBardMasterpieceCore()
        {
            var icon = AbilityRefs.DeadlyPerformanceAbility.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(BardMasterpieceCore, BardMasterpieceCoreGuid)
              .SetDisplayName(BardMasterpieceDisplayName)
              .SetDescription(BardMasterpieceDescription)
              .SetIcon(icon)
              .AddToIsPrerequisiteFor(new Blueprint<BlueprintFeatureReference>[] { BardMasterpieceConfigure() })
              .AddComponent<MasterpieceLogic>()
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.BardProgression)
                .AddToLevelEntry(1, feat)
                .Configure();
        }

        private const string BardMasterpieceFeat = "BardMasterpiece.BardMasterpiece";
        public static readonly string BardMasterpieceGuid = "{22DF5EBC-A04E-4C3B-B72B-D6A032DF26BB}";

        internal const string BardMasterpieceDisplayName = "BardMasterpiece.Name";
        private const string BardMasterpieceDescription = "BardMasterpiece.Description";
        public static BlueprintFeature BardMasterpieceConfigure()
        {
            var icon = AbilityRefs.DeadlyPerformanceAbility.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(BardMasterpieceFeat, BardMasterpieceGuid)
              .SetDisplayName(BardMasterpieceDisplayName)
              .SetDescription(BardMasterpieceDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(BardMasterpieceCoreGuid)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(CreateTripleTime())
              .AddToAllFeatures(CreateTwistingSteel())
              .AddToAllFeatures(CreateStoneFace())
              .AddToAllFeatures(CreateRatQuadrille())
              .AddToAllFeatures(CreatePriestKing())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DragonLevel2FeatSelection)
                .AddToAllFeatures(feat)
                .Configure();

            return feat;
        }

        private const string TripleTime = "Masterpiece.TripleTime";
        public static readonly string TripleTimeGuid = "{EA78ACB4-D560-44C3-A6DA-2E3DDD20006B}";
        internal const string TripleTimeDisplayName = "MasterpieceTripleTime.Name";
        private const string TripleTimeDescription = "MasterpieceTripleTime.Description";

        private const string TripleTimeAblity = "Masterpiece.UseTripleTime";
        public static readonly string TripleTimeAblityGuid = "{AD42CAF0-4315-4C08-8EBB-1FB888F32F84}";

        private const string TripleTimeBuff = "Masterpiece.TripleTimeBuff";
        private static readonly string TripleTimeBuffGuid = "{025E7123-73F7-4B38-B063-8F1CB3A4B3F3}";

        public static BlueprintFeature CreateTripleTime()
        {
            var icon = FeatureRefs.ExtendSpellFeat.Reference.Get().Icon;
            var fx = AbilityRefs.ExpeditiousRetreat.Reference.Get().GetComponent<AbilitySpawnFx>();

            var buff = BuffConfigurator.New(TripleTimeBuff, TripleTimeBuffGuid)
              .SetDisplayName(TripleTimeDisplayName)
              .SetDescription(TripleTimeDescription)
              .SetIcon(icon)
              .AddBuffMovementSpeed(value: 10, descriptor: ModifierDescriptor.Enhancement)
              .Configure();

            var shoot = ActionsBuilder.New()
                .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), Kingmaker.UnitLogic.Mechanics.DurationRate.TenMinutes))
                .Build();

            var ability = AbilityConfigurator.New(TripleTimeAblity, TripleTimeAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[]{ CharacterClassRefs.BardClass.ToString() }, max: 6, min: 1))
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(TripleTimeDisplayName)
                .SetDescription(TripleTimeDescription)
              .SetIcon(icon)
              .SetRange(AbilityRange.Personal)
              .AddAbilityTargetsAround(includeDead: false, targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Ally, radius: 100.Feet(), spreadSpeed: 20.Feet())
              .AddComponent(fx)
              .SetIsFullRoundAction(true)
              .AddAbilityCasterInCombat(true)
              .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
              .AddComponent<AbilityRequirementNotSpell>()
              .AddToSpellList(1, SpellListRefs.BardSpellList.ToString())
              .Configure();

            return FeatureConfigurator.New(TripleTime, TripleTimeGuid)
              .SetDisplayName(TripleTimeDisplayName)
              .SetDescription(TripleTimeDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.SkillUseMagicDevice, 3)
              .AddFacts(new() { ability })
              .AddLearnSpells(CharacterClassRefs.BardClass.ToString(), new() { ability })
              .Configure();
        }

        private const string TwistingSteel = "Masterpiece.TwistingSteel";
        public static readonly string TwistingSteelGuid = "{C73EB87C-ED4E-42E8-82C9-C73C793B8797}";
        internal const string TwistingSteelDisplayName = "MasterpieceTwistingSteel.Name";
        private const string TwistingSteelDescription = "MasterpieceTwistingSteel.Description";

        private const string TwistingSteelAblity = "Masterpiece.UseTwistingSteel";
        public static readonly string TwistingSteelAblityGuid = "{B2C149BD-CE23-4753-9D75-9A74C642C9E3}";
        public static BlueprintFeature CreateTwistingSteel()
        {
            var icon = AbilityRefs.SwordOfEternalSquireAbility.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(TwistingSteelAblity, TwistingSteelAblityGuid)
                .CopyFrom(
                ShieldOther.ShieldOtherAbilityGuid,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx))
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(TwistingSteelDisplayName)
                .SetDescription(TwistingSteelDescription)
              .SetIcon(icon)
              .SetRange(AbilityRange.Custom)
              .SetCustomRange(5)
              .AllowTargeting(false, false, true, false)
              .SetIsFullRoundAction(true)
              .AddAbilityCasterInCombat(true)
              .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
              .AddToSpellList(2, SpellListRefs.BardSpellList.ToString())
              .AddComponent<AbilityRequirementNotSpell>()
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.BardClass.ToString() }))
              .Configure();

            return FeatureConfigurator.New(TwistingSteel, TwistingSteelGuid)
              .SetDisplayName(TwistingSteelDisplayName)
              .SetDescription(TwistingSteelDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.SkillPersuasion, 3)
              .AddFacts(new() { ability })
              .AddLearnSpells(CharacterClassRefs.BardClass.ToString(), new() { ability })
              .Configure();
        }

        private const string StoneFace = "Masterpiece.StoneFace";
        public static readonly string StoneFaceGuid = "{21606A26-8EA7-4538-92E2-3AC549548EFA}";
        internal const string StoneFaceDisplayName = "MasterpieceStoneFace.Name";
        private const string StoneFaceDescription = "MasterpieceStoneFace.Description";

        private const string StoneFaceAblity = "Masterpiece.UseStoneFace";
        public static readonly string StoneFaceAblityGuid = "{DF05FC9B-9F39-4D26-AD3A-51E21EC6AC06}";
        public static BlueprintFeature CreateStoneFace()
        {
            var icon = AbilityRefs.Stoneskin.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(StoneFaceAblity, StoneFaceAblityGuid)
                .CopyFrom(
                AbilityRefs.StoneToFlesh,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(AbilityTargetStoneToFlesh),
                typeof(AbilitySpawnFx))
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(StoneFaceDisplayName)
                .SetDescription(StoneFaceDescription)
              .SetIcon(icon)
              .SetRange(AbilityRange.Close)
              .AllowTargeting(false, false, true, false)
              .SetIsFullRoundAction(true)
              .AddAbilityCasterInCombat(true)
              .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
              .AddToSpellList(3, SpellListRefs.BardSpellList.ToString())
              .AddComponent<AbilityRequirementNotSpell>()
              .Configure();

            return FeatureConfigurator.New(StoneFace, StoneFaceGuid)
              .SetDisplayName(StoneFaceDisplayName)
              .SetDescription(StoneFaceDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.SkillPersuasion, 7)
              .AddFacts(new() { ability })
              .AddLearnSpells(CharacterClassRefs.BardClass.ToString(), new() { ability })
              .Configure();
        }

        private const string RatQuadrille = "Masterpiece.RatQuadrille";
        public static readonly string RatQuadrilleGuid = "{91850442-7324-4E20-8F00-845CF44B81C4}";
        internal const string RatQuadrilleDisplayName = "MasterpieceRatQuadrille.Name";
        private const string RatQuadrilleDescription = "MasterpieceRatQuadrille.Description";

        private const string RatQuadrilleAblity = "Masterpiece.UseRatQuadrille";
        public static readonly string RatQuadrilleAblityGuid = "{C897A757-7C0A-4E68-B733-B3C655C9EB79}";

        private const string RatQuadrilleBuff = "Masterpiece.RatQuadrilleBuff";
        private static readonly string RatQuadrilleBuffGuid = "{6CBF5363-0081-4DF0-AB48-4224F456E7A6}";

        private const string RatQuadrilleBuff2 = "Masterpiece.RatQuadrilleBuff2";
        private static readonly string RatQuadrilleBuff2Guid = "{EE7A4D24-AA8F-4735-B1E4-8368E3AC8F28}";
        public static BlueprintFeature CreateRatQuadrille()
        {
            var icon = AbilityRefs.SummonMonsterISingle.Reference.Get().Icon;
            var fx = AbilityRefs.CurseIdiocyBomb.Reference.Get().GetComponent<AbilitySpawnFx>();

            var buff = BuffConfigurator.New(RatQuadrilleBuff, RatQuadrilleBuffGuid)
              .SetDisplayName(RatQuadrilleDisplayName)
              .SetDescription(RatQuadrilleDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.AdditionalAttackBonus, value: -2)
              .AddConcentrationBonus(value: -4)
              .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion)
              .Configure();

            var buff2 = BuffConfigurator.New(RatQuadrilleBuff2, RatQuadrilleBuff2Guid)
              .SetDisplayName(RatQuadrilleDisplayName)
              .SetDescription(RatQuadrilleDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.AdditionalAttackBonus, value: -1)
              .AddConcentrationBonus(value: -2)
              .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion)
              .Configure();

            var shoot1 = ActionsBuilder.New()
                .ApplyBuff(buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.TenMinutes))
                .RemoveBuff(buff2)
                .Build();

            var shoot2 = ActionsBuilder.New()
                .ApplyBuff(buff2, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Minutes))
                .RemoveBuff(buff)
                .Build();

            var ability = AbilityConfigurator.New(RatQuadrilleAblity, RatQuadrilleAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ConditionalSaved(shoot1, shoot2)
                    .Build(), savingThrowType: SavingThrowType.Will)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.BardClass.ToString() }, max: 6, min: 1))
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(RatQuadrilleDisplayName)
                .SetDescription(RatQuadrilleDescription)
              .SetIcon(icon)
              .SetRange(AbilityRange.Long)
              .AllowTargeting(false, true, false, false)
              .SetSpellDescriptor(SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion)
              .AddComponent(fx)
              .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
              .AddToSpellList(2, SpellListRefs.BardSpellList.ToString())
              .AddComponent<AbilityRequirementNotSpell>()
              .AddComponent<CustomDC>(c => { c.classguid = CharacterClassRefs.BardClass.ToString(); c.Property = StatType.Charisma; c.halfed = true; })
              .Configure();

            return FeatureConfigurator.New(RatQuadrille, RatQuadrilleGuid)
              .SetDisplayName(RatQuadrilleDisplayName)
              .SetDescription(RatQuadrilleDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.SkillPersuasion, 4, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteStatValue(StatType.SkillUseMagicDevice, 4, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddFacts(new() { ability })
              .AddLearnSpells(CharacterClassRefs.BardClass.ToString(), new() { ability })
              .Configure();
        }

        private const string PriestKing = "Masterpiece.PriestKing";
        public static readonly string PriestKingGuid = "{2EBC866B-610A-4655-8303-2C5006F55BC3}";
        internal const string PriestKingDisplayName = "MasterpiecePriestKing.Name";
        private const string PriestKingDescription = "MasterpiecePriestKing.Description";

        private const string PriestKingAblity = "Masterpiece.UsePriestKing";
        public static readonly string PriestKingAblityGuid = "{292C8975-9FA1-4AF3-83D5-B7DBCCF72B92}";

        private const string PriestKingAblity2 = "Masterpiece.UsePriestKing2";
        private static readonly string PriestKingAblity2Guid = "{6803A5A8-5D29-4E7D-8CB0-2F5E6E293F67}";

        private const string PriestKingBuff = "Masterpiece.PriestKingBuff";
        private static readonly string PriestKingBuffGuid = "{E840AFC8-C41F-44B3-B89F-AC7DA226B2C9}";

        public static BlueprintFeature CreatePriestKing()
        {
            var icon = AbilityRefs.CavalierForTheKingAbility.Reference.Get().Icon;
            var fx = AbilityRefs.Haste.Reference.Get().GetComponent<AbilitySpawnFx>();

            var shoot2 = ActionsBuilder.New()
                .Add<ContextActionRestoreAction>()
                .RemoveBuff(PriestKingBuffGuid)
                .Build();

            var ability2 = AbilityConfigurator.New(PriestKingAblity2, PriestKingAblity2Guid)
                .AddAbilityEffectRunAction(shoot2)
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(PriestKingDisplayName)
                .SetDescription(PriestKingDescription)
              .SetIcon(icon)
              .SetRange(AbilityRange.Personal)
              .AddComponent(fx)
              .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
              .Configure();

            var buff = BuffConfigurator.New(PriestKingBuff, PriestKingBuffGuid)
              .SetDisplayName(PriestKingDisplayName)
              .SetDescription(PriestKingDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .Configure();

            var shoot = ActionsBuilder.New()
                .ApplyBuff(buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.TenMinutes))
                .Build();

            var ability = AbilityConfigurator.New(PriestKingAblity, PriestKingAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(PriestKingDisplayName)
                .SetDescription(PriestKingDescription)
              .SetIcon(icon)
              .SetRange(AbilityRange.Personal)
              .AddAbilityTargetsAround(includeDead: false, targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Ally, radius: 100.Feet(), spreadSpeed: 20.Feet())
              .AddComponent(fx)
              .SetIsFullRoundAction(true)
              .AddAbilityCasterInCombat(true)
              .AddAbilityResourceLogic(10, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
              .AddToSpellList(3, SpellListRefs.BardSpellList.ToString())
              .AddComponent<AbilityRequirementNotSpell>()
              .Configure();

            return FeatureConfigurator.New(PriestKing, PriestKingGuid)
              .SetDisplayName(PriestKingDisplayName)
              .SetDescription(PriestKingDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.SkillPersuasion, 10)
              .AddFacts(new() { ability })
              .AddLearnSpells(CharacterClassRefs.BardClass.ToString(), new() { ability })
              .Configure();
        }
    }
}
