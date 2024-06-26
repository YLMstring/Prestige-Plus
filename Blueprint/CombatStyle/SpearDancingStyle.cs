﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.TurnBasedModifiers;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owlcat.Runtime.UI.ConsoleTools.GamepadInput;
using Kingmaker.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using PrestigePlus.CustomAction.GrappleThrow;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.CustomComponent.Charge;
using PrestigePlus.Blueprint.MythicGrapple;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.Blueprints.Items.Weapons;

namespace PrestigePlus.Blueprint.CombatStyle
{
    internal class SpearDancingStyle
    {
        private static readonly string StyleName = "SpearDancingStyle";
        public static readonly string StyleGuid = "{FEA748E9-2786-465E-87DA-28D94053E0D0}";

        private static readonly string StyleDisplayName = "SpearDancingStyle.Name";
        private static readonly string StyleDescription = "SpearDancingStyle.Description";

        private const string Stylebuff = "SpearDancingStyle.Stylebuff";
        public static readonly string StylebuffGuid = "{0022BBE3-C41A-4EB4-BC26-2488F2916220}";

        private const string StyleActivatableAbility = "SpearDancingStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{D01A6ECB-D068-4AFE-BFF2-46C56C21C131}";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.HammerTheGap.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(StyleActivatableAbility, StyleActivatableAbilityGuid)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetGroup(ActivatableAbilityGroup.CombatStyle)
                .SetWeightInGroup(1)
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(StyleName, StyleGuid, FeatureGroup.Feat)
                    .SetDisplayName(StyleDisplayName)
                    .SetDescription(StyleDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 13)
                    .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.WeaponFinesse.ToString())
                    .AddComponent<SpearAntiCheat>()
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .AddFacts([ability])
                    .Configure();

        }

        private static readonly string SpiralName = "SpearDancingSpiral";
        public static readonly string SpiralGuid = "{7677FC3B-1FA6-4213-A0FD-1B1BDCAB4A27}";

        private const string Spiralbuff = "SpearDancingSpiral.Spiralbuff";
        public static readonly string SpiralbuffGuid = "{B79080E0-C908-446C-A901-0D6111677787}";

        private static readonly string SpiralDisplayName = "SpearDancingSpiral.Name";
        private static readonly string SpiralDescription = "SpearDancingSpiral.Description";

        public static void SpiralConfigure()
        {
            var icon = FeatureRefs.PurityOfBody.Reference.Get().Icon;

            var BuffSpiral = BuffConfigurator.New(Spiralbuff, SpiralbuffGuid)
              .SetDisplayName(SpiralDisplayName)
              .SetDescription(SpiralDescription)
              .SetIcon(icon)
              .AddComponent<SpearFlurryUnlock>(c => {
                  c.Flurry1 = FeatureRefs.FlurryOfBlows.Reference;
                  c.Flurry11 = FeatureRefs.FlurryOfBlowsLevel11.Reference;
                  c.SoheiArchetype = ArchetypeRefs.SoheiArchetype.Reference;
                  c.Flurry1stfact = [FeatureRefs.MonkFlurryOfBlowstUnlock.Reference, FeatureRefs.QuarterstaffMasterFlurryUnlock.Reference, FeatureRefs.SoheiFlurryOfBlowsUnlock.Reference, FeatureRefs.ZenArcherFlurryOfBlowsUnlock.Reference];
                  c.Flurry2ndfact = [FeatureRefs.MonkFlurryOfBlowstLevel11Unlock.Reference, FeatureRefs.SoheiFlurryOfBlowstLevel11Unlock.Reference, FeatureRefs.QuarterstaffMasterFlurry11Unlock.Reference, FeatureRefs.ZenArcherFlurryOfBlowsLevel11Unlock.Reference];
              })
              .AddComponent<SpearAttackStatReplacement>(c => { c.ReplacementStat = StatType.Dexterity; c.Mythic = FeatureRefs.WeaponFinesseMythicFeat.Reference; })
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            FeatureConfigurator.New(SpiralName, SpiralGuid, FeatureGroup.Feat)
                    .SetDisplayName(SpiralDisplayName)
                    .SetDescription(SpiralDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 15)
                    .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.WeaponFinesse.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddBuffExtraEffects(checkedBuff: StylebuffGuid, extraEffectBuff: BuffSpiral)
                    .AddToIsPrerequisiteFor([FeatureRefs.MonkFlurryOfBlowstUnlock.ToString(), SmashingStyle.CounterGuid, SmashingStyle.MasterGuid])
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }

        private static readonly string ReachName = "SpearDancingReach";
        public static readonly string ReachGuid = "{051201E2-7662-4852-89CD-ACC653B019A8}";

        private const string Reachbuff = "SpearDancingReach.Reachbuff";
        private static readonly string ReachbuffGuid = "{F1E0FF38-1C2A-4E52-84A8-1D42FF0C3D34}";

        private static readonly string ReachDisplayName = "SpearDancingReach.Name";
        private static readonly string ReachDescription = "SpearDancingReach.Description";

        public static void ReachConfigure()
        {
            var icon = FeatureSelectionRefs.MutationWarriorDiscoverySelection.Reference.Get().Icon;

            var BuffReach = BuffConfigurator.New(Reachbuff, ReachbuffGuid)
              .SetDisplayName(ReachDisplayName)
              .SetDescription(ReachDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddStatBonus(null, false, StatType.Reach, 5)
              .Configure();

            FeatureConfigurator.New(ReachName, ReachGuid, FeatureGroup.Feat)
                    .SetDisplayName(ReachDisplayName)
                    .SetDescription(ReachDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 17)
                    .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.WeaponFinesse.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(SpiralGuid)
                    .AddBuffExtraEffects(checkedBuff: StylebuffGuid, extraEffectBuff: BuffReach)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }

        private static readonly string DeadlyAgilityName = "PoWDeadlyAgility";
        public static readonly string DeadlyAgilityGuid = "{95278BF2-67F5-45BC-8E53-93208B27C2E4}";

        private static readonly string DeadlyAgilityDisplayName = "PoWDeadlyAgility.Name";
        private static readonly string DeadlyAgilityDescription = "PoWDeadlyAgility.Description";

        public static void DeadlyAgilityConfigure()
        {
            var icon = AbilityRefs.CoupDeGraceAbility.Reference.Get().Icon;

            FeatureConfigurator.New(DeadlyAgilityName, DeadlyAgilityGuid, FeatureGroup.Feat)
                    .SetDisplayName(DeadlyAgilityDisplayName)
                    .SetDescription(DeadlyAgilityDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Strength, 10)
                    .AddPrerequisiteFeature(FeatureRefs.WeaponFinesse.ToString())
                    .AddFacts([FeatureRefs.WeaponFinesseMythicFeat.ToString()])
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
    public class SpearAttackStatReplacement : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>
    {
        // Token: 0x0600C7A9 RID: 51113 RVA: 0x0034014C File Offset: 0x0033E34C
        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            ModifiableValueAttributeStat modifiableValueAttributeStat = base.Owner.Stats.GetStat(evt.AttackBonusStat) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat modifiableValueAttributeStat2 = base.Owner.Stats.GetStat(this.ReplacementStat) as ModifiableValueAttributeStat;
            bool flag = modifiableValueAttributeStat2 != null && (modifiableValueAttributeStat == null || modifiableValueAttributeStat2.Bonus >= modifiableValueAttributeStat.Bonus);
            if (flag && (evt.Weapon.Blueprint.FighterGroup.Contains(WeaponFighterGroup.Spears) || evt.Weapon.Blueprint.FighterGroup.Contains(WeaponFighterGroup.Polearms)))
            {
                evt.AttackBonusStat = this.ReplacementStat;
            }
        }

        // Token: 0x0600C7AA RID: 51114 RVA: 0x003401DF File Offset: 0x0033E3DF
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }

        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            ModifiableValueAttributeStat modifiableValueAttributeStat = evt.Initiator.Descriptor.Stats.GetStat(this.ReplacementStat) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat modifiableValueAttributeStat2 = (evt.DamageBonusStat != null) ? (evt.Initiator.Descriptor.Stats.GetStat(evt.DamageBonusStat.Value) as ModifiableValueAttributeStat) : null;
            bool flag = modifiableValueAttributeStat != null && (modifiableValueAttributeStat2 == null || modifiableValueAttributeStat.Bonus > modifiableValueAttributeStat2.Bonus);
            bool flag2 = evt.Weapon.Blueprint.FighterGroup.Contains(WeaponFighterGroup.Spears) || evt.Weapon.Blueprint.FighterGroup.Contains(WeaponFighterGroup.Polearms);
            if (flag && flag2 && Owner.HasFact(Mythic))
            {
                evt.OverrideDamageBonusStat(this.ReplacementStat);
                evt.TwoHandedStatReplacement = true;
            }
        }

        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            
        }

        // Token: 0x04008653 RID: 34387
        public StatType ReplacementStat;
        public BlueprintFeature Mythic;
    }
    internal class SpearAntiCheat : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber, IUnitSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.Weapon?.CanTakeOneHand(Owner) == false && Owner.Body.PrimaryHand.HasItem && Owner.Body.SecondaryHand.HasItem)
            {
                evt.AutoMiss = true;
            }
        }
        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {

        }
    }
}

