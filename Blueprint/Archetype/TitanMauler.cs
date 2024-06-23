using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using PrestigePlus.CustomComponent;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Linq;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs;
using BlueprintCore.Utils;
using PrestigePlus.Blueprint.CombatStyle;
using Kingmaker.Controllers.Units;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Buffs.Components;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class TitanMauler
    {
        private const string ArchetypeName = "TitanMauler";
        private static readonly string ArchetypeGuid = "{C5529944-971C-4069-83D1-9F55B7D169AA}";
        internal const string ArchetypeDisplayName = "TitanMauler.Name";
        private const string ArchetypeDescription = "TitanMauler.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.BarbarianClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
              .SetRemoveFeaturesEntry(1, FeatureRefs.FastMovement.ToString())
              .SetRemoveFeaturesEntry(2, FeatureRefs.UncannyDodgeChecker.ToString())
              .SetRemoveFeaturesEntry(3, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(6, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(9, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(12, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(15, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(18, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(5, FeatureRefs.ImprovedUncannyDodge.ToString())
              .SetRemoveFeaturesEntry(14, FeatureRefs.IndomitableWill.ToString())
              .AddToAddFeatures(1, CreateBigGameHunter())
              .AddToAddFeatures(2, "38323bb032d740ab9045f1086705b0c7")
              .AddToAddFeatures(3, "f0235447f6f3430fa4a98d15642b849e")
              .AddToAddFeatures(6, "f0235447f6f3430fa4a98d15642b849e")
              .AddToAddFeatures(9, "f0235447f6f3430fa4a98d15642b849e")
              .AddToAddFeatures(12, "f0235447f6f3430fa4a98d15642b849e")
              .AddToAddFeatures(15, "f0235447f6f3430fa4a98d15642b849e")
              .AddToAddFeatures(18, "f0235447f6f3430fa4a98d15642b849e")
              .AddToAddFeatures(5, FeatureRefs.LungeFeature.ToString())
              .AddToAddFeatures(14, CreateTitanicRage())
              .Configure();

            var feat = BlueprintTool.GetRef<BlueprintFeatureReference>("f0235447f6f3430fa4a98d15642b849e")?.Get();
            if (feat != null)
            {
                feat.Ranks = 20;
            }
        }

        private const string BigGameHunter = "TitanMauler.BigGameHunter";
        private static readonly string BigGameHunterGuid = "{ADE525E2-915C-4B3B-8437-ACE3C9FCDEF9}";

        internal const string BigGameHunterDisplayName = "TitanMaulerBigGameHunter.Name";
        private const string BigGameHunterDescription = "TitanMaulerBigGameHunter.Description";
        private static BlueprintFeature CreateBigGameHunter()
        {
            var icon = FeatureRefs.MasterHunter.Reference.Get().Icon;

            return FeatureConfigurator.New(BigGameHunter, BigGameHunterGuid)
              .SetDisplayName(BigGameHunterDisplayName)
              .SetDescription(BigGameHunterDescription)
              .SetIcon(icon)
              .AddComponent<BigGameHunterComp>()
              .Configure();
        }

        private const string TitanicRage = "TitanMauler.TitanicRage";
        private static readonly string TitanicRageGuid = "{9D51A7F1-B651-47D0-A66E-B4C8FF67E49E}";

        private const string TitanicRage0 = "TitanMauler.TitanicRage0";
        public static readonly string TitanicRageGuid0 = "{85E6C0EA-01CD-469C-9AE5-7569D61EADBE}";

        internal const string TitanicRageDisplayName = "TitanMaulerTitanicRage.Name";
        private const string TitanicRageDescription = "TitanMaulerTitanicRage.Description";
        private static BlueprintFeature CreateTitanicRage()
        {
            var icon = AbilityRefs.EnlargePerson.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(TitanicRage0, TitanicRageGuid0)
                .CopyFrom(
                BuffRefs.EnlargePersonBuff,
                typeof(ChangeUnitSize),
                typeof(AddGenericStatBonus))
              .SetDisplayName(TitanicRageDisplayName)
              .SetDescription(TitanicRageDescription)
              .SetIcon(icon)
              .RemoveFromFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.IsFromSpell)
              .Configure();

            return FeatureConfigurator.New(TitanicRage, TitanicRageGuid)
              .SetDisplayName(TitanicRageDisplayName)
              .SetDescription(TitanicRageDescription)
              .SetIcon(icon)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.DemonRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.InspiredRageEffectBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(BuffRefs.InspiredRageEffectBuffMythic.ToString(), extraEffectBuff: Buff)
              .Configure();
        }
    }

    internal class BigGameHunterComp : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            if (Owner.CombatState.EngagedUnits.Contains(evt.Initiator) && evt.Initiator.State.Size > Owner.State.Size)
            {
                evt.AddModifier(1, base.Fact, ModifierDescriptor.Dodge);
            }
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            if (Owner.CombatState.EngagedUnits.Contains(evt.Target) && evt.Target.State.Size > Owner.State.Size)
            {
                evt.AddModifier(1, base.Fact);
            }
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
            
        }
    }
}
