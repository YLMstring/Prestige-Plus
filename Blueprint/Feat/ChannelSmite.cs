using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using PrestigePlus.Blueprint.CombatStyle;
using PrestigePlus.CustomAction.OtherManeuver;
using PrestigePlus.CustomComponent;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.CustomComponent.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class ChannelSmite
    {
        private static readonly string BasicSmiteFeatName = "ChannelSmiteBasicSmite";
        private static readonly string BasicSmiteFeatGuid = "{9094446C-FBE1-4999-9C31-054BFF066E6B}";

        private static readonly string BetterSmiteFeatName = "ChannelSmiteBetterSmite";
        public static readonly string BetterSmiteFeatGuid = "{8DA9053F-C91D-4D8E-8728-8634ADBF3A10}";

        private static readonly string MythicSmiteFeatName = "ChannelSmiteMythicSmite";
        private static readonly string MythicSmiteFeatGuid = "{B842F8FC-4E23-4903-BA1C-BE82FD828FCA}";

        private static readonly string DemonSmiteFeatName = "ChannelSmiteDemonSmite";
        public static readonly string DemonSmiteFeatGuid = "{4DE50F32-A959-49DE-B3F0-2E7A20357A9D}";

        private static readonly string BasicSmiteDisplayName = "ChannelSmiteBasicSmite.Name";
        private static readonly string BasicSmiteDescription = "ChannelSmiteBasicSmite.Description";

        private static readonly string BetterSmiteDisplayName = "ChannelSmiteBetterSmite.Name";
        private static readonly string BetterSmiteDescription = "ChannelSmiteBetterSmite.Description";

        private static readonly string MythicSmiteDisplayName = "ChannelSmiteMythicSmite.Name";
        private static readonly string MythicSmiteDescription = "ChannelSmiteMythicSmite.Description";

        private static readonly string DemonSmiteDisplayName = "ChannelSmiteDemonSmite.Name";
        private static readonly string DemonSmiteDescription = "ChannelSmiteDemonSmite.Description";

        private const string BasicSmitebuff = "ChannelSmiteTrick.BasicSmitebuff";
        public static readonly string BasicSmitebuffGuid = "{5326CA1E-7D40-4F88-8B83-67C48597B0F8}";

        private const string BasicSmitebuffreal = "ChannelSmiteTrick.BasicSmitebuffreal";
        public static readonly string BasicSmitebuffrealGuid = "{4D2A7AFE-383C-48E2-A749-954044D3C6A9}";

        private const string BasicSmitebuffcool = "ChannelSmiteTrick.BasicSmitebuffcool";
        public static readonly string BasicSmitebuffcoolGuid = "{0C294131-11F0-4B9A-80CB-36578B83F301}";

        private const string BasicSmiteActivatableAbility = "ChannelSmiteTrick.BasicSmiteActivatableAbility";
        private static readonly string BasicSmiteActivatableAbilityGuid = "{DE42E7CB-DE48-4FCC-9EF7-B9EC2939716E}";

        public static void ConfigureBasicSmite()
        {
            var icon = AbilityRefs.ChaosHammer.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(BasicSmitebuff, BasicSmitebuffGuid)
              .SetDisplayName(BasicSmiteDisplayName)
              .SetDescription(BasicSmiteDescription)
              .SetIcon(icon)
              
              
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var Buffreal = BuffConfigurator.New(BasicSmitebuffreal, BasicSmitebuffrealGuid)
              .SetDisplayName(BasicSmiteDisplayName)
              .SetDescription(BasicSmiteDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var Buffcool = BuffConfigurator.New(BasicSmitebuffcool, BasicSmitebuffcoolGuid)
              .SetDisplayName(BasicSmiteDisplayName)
              .SetDescription(BasicSmiteDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(BasicSmiteActivatableAbility, BasicSmiteActivatableAbilityGuid)
                .SetDisplayName(BasicSmiteDisplayName)
                .SetDescription(BasicSmiteDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .Configure();

            FeatureConfigurator.New(BasicSmiteFeatName, BasicSmiteFeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(BasicSmiteDisplayName)
                    .SetDescription(BasicSmiteDescription)
                    .SetIcon(icon)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddFacts([ability])
                    .AddComponent<PrerequisiteHasChannel>()
                    .Configure();

            FeatureConfigurator.New(MythicSmiteFeatName, MythicSmiteFeatGuid, FeatureGroup.MythicFeat)
                    .SetDisplayName(MythicSmiteDisplayName)
                    .SetDescription(MythicSmiteDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(BasicSmiteFeatGuid)
                    .Configure();

            FeatureConfigurator.New(BetterSmiteFeatName, BetterSmiteFeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(BetterSmiteDisplayName)
                    .SetDescription(BetterSmiteDescription)
                    .SetIcon(icon)
                    .AddComponent<PrerequisiteHasChannel>()
                    .AddPrerequisiteFeature(BasicSmiteFeatGuid)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 8)
                    .Configure();

            FeatureConfigurator.New(DemonSmiteFeatName, DemonSmiteFeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(DemonSmiteDisplayName)
                    .SetDescription(DemonSmiteDescription)
                    .SetIcon(icon)
                    .AddComponent<PrerequisiteHasChannel>()
                    .Configure();
        }
    }
    internal class ChannelSmiteStuff : UnitFactComponentDelegate<ChannelSmiteStuff.ComponentData>, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleAttackWithWeaponResolve>
    {
        public class ComponentData
        {
            public Ability channel;
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            GetChannel(Owner, evt.Target);
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {

        }

        public Ability GetChannel(UnitEntityData caster, UnitEntityData target)
        {
            try
            {
                if (HasChannelTarget(caster, target, Data.channel)) return Data.channel;
                if (!caster.HasSwiftAction()) return null;
                foreach (var ability in caster.Abilities.RawFacts)
                {
                    if (HasChannelTarget(caster, target, ability))
                    {
                        Data.channel = ability;
                        caster.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift, false, 0f);
                        return ability;
                    }
                }
                return null;
            }
            catch (Exception ex) { Main.Logger.Error("Failed to CheckHasChannel.", ex); return null; }
        }

        public static bool HasChannelTarget(UnitEntityData caster, UnitEntityData target, Ability ability)
        {
            if (ability == null) return false;
            if (!ability.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.ChannelNegativeHarm)
            && !ability.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.ChannelPositiveHarm))
            {
                return false;
            }
            if (caster.HasFact(Demon) && target.HasFact(FeatureRefs.SubtypeEvil.Reference)) return true;
            if (ability.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.ChannelNegativeHarm))
            {
                return !target.HasFact(FeatureRefs.NegativeEnergyAffinity.Reference);
            }
            if (ability.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.ChannelPositiveHarm))
            {
                return target.HasFact(FeatureRefs.NegativeEnergyAffinity.Reference);
            }
            return false;
        }

        void IRulebookHandler<RuleAttackWithWeaponResolve>.OnEventAboutToTrigger(RuleAttackWithWeaponResolve evt)
        {
            if (!evt.AttackRoll.IsHit) return;
            if (Data.channel == null) return;
            DamageEnergyType energy;
            if (Data.channel.Blueprint.SpellDescriptor.HasFlag(SpellDescriptor.ChannelPositiveHarm))
            {
                energy = DamageEnergyType.PositiveEnergy;
            }
            else
            {
                energy = DamageEnergyType.NegativeEnergy;
            }
            int damage = Data.channel.Blueprint.GetComponent<ContextRankConfig>(f => f.Type == AbilityRankType.Default)?.GetValue(Owner.Context) ?? 0;
            int bonus = Data.channel.Blueprint.GetComponent<ContextRankConfig>(f => f.Type == AbilityRankType.DamageBonus)?.GetValue(Owner.Context) ?? 0;
            evt.Damage.m_DamageBundle.m_Chunks.Add(new EnergyDamage(new DiceFormula(damage, DiceType.D6), bonus, energy) { SourceFact = this.Fact });
        }

        void IRulebookHandler<RuleAttackWithWeaponResolve>.OnEventDidTrigger(RuleAttackWithWeaponResolve evt)
        {
            
        }

        private static BlueprintFeatureReference Demon = BlueprintTool.GetRef<BlueprintFeatureReference>(ChannelSmite.DemonSmiteFeatGuid);
        private static BlueprintFeatureReference Great = BlueprintTool.GetRef<BlueprintFeatureReference>(ChannelSmite.BetterSmiteFeatGuid);
    }
}
