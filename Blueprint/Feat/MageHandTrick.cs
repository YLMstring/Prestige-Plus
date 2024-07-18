using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents.Prerequisites;

namespace PrestigePlus.Blueprint.Feat
{
    internal class MageHandTrick
    {
        private static readonly string MageHandMainFeatName = "MageHandMageHandMain";
        private static readonly string MageHandMainFeatGuid = "{0FF806EB-BAF4-4B36-A773-2204A9582E33}";

        private static readonly string MageHandMainDisplayName = "MageHandMageHandMain.Name";
        private static readonly string MageHandMainDescription = "MageHandMageHandMain.Description";

        public static void ConfigureMageHandMain()
        {
            var icon = AbilityRefs.DispelMagic.Reference.Get().Icon;
            ReachingHandConfigure(); ConfigureMageHandMythic();

            AbilityConfigurator.For(AbilityRefs.ExpeditiousRetreat)
                .SetSpellResistance()
                .SetIgnoreSpellResistanceForAlly()
                .Configure();

            FeatureConfigurator.New(MageHandMainFeatName, MageHandMainFeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(MageHandMainDisplayName)
                    .SetDescription(MageHandMainDescription)
                    .SetIcon(icon)
                    .AddComponent<PrerequisiteSpellKnown>(c => { c.m_Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.ExpeditiousRetreat.ToString()); c.RequireSpellbook = true; })
                    .AddPrerequisiteCasterTypeSpellLevel(true, false, 1)
                    .AddFeatureIfHasFact(FeatureRefs.ImprovedDirtyTrick.ToString(), ConfigureDirtyMagicTrick(), false)
                    .AddFeatureOnSkill(new() { ConfigureRangedAid() }, 1, StatType.BaseAttackBonus)
                    .AddFeatureIfHasFact(FeatureRefs.ImprovedUnarmedStrike.ToString(), ConfigureThrowPunch(), false)
                    .AddFeatureIfHasFact(FeatureRefs.PreciseShot.ToString(), FeatGuid, false)
                    .AddFeatureIfHasFact(FeatureRefs.ReachSpellFeat.ToString(), FeatGuid, false)
                    .AddToIsPrerequisiteFor(DirtyMagicTrickFeatGuid)
                    .AddToIsPrerequisiteFor(RangedAidFeatGuid)
                    .AddToIsPrerequisiteFor(ThrowPunchFeatGuid)
                    .AddToIsPrerequisiteFor(FeatGuid)
                    .AddToIsPrerequisiteFor(MageHandMythicFeatGuid)
                    .SetReapplyOnLevelUp(true)
                    .Configure();
        }

        private static readonly string ShieldMainFeatName = "ShieldShieldMain";
        public static readonly string ShieldMainFeatGuid = "{2428F730-3BF9-42DC-B591-FAEC7229A4D2}";

        private static readonly string ShieldMainDisplayName = "ShieldShieldMain.Name";
        private static readonly string ShieldMainDescription = "ShieldShieldMain.Description";

        public static void ConfigureShieldMain()
        {
            var icon = AbilityRefs.MageShield.Reference.Get().Icon;
            FeatureConfigurator.New(ShieldMainFeatName, ShieldMainFeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(ShieldMainDisplayName)
                    .SetDescription(ShieldMainDescription)
                    .SetIcon(icon)
                    .AddComponent<PrerequisiteSpellKnown>(c => { c.m_Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.MageShield.ToString()); c.RequireSpellbook = true; })
                    .AddPrerequisiteFeature(BodyGuard.FeatGuid)
                    .Configure();
        }

        private static readonly string MageHandMythicFeatName = "MageHandMageHandMythic";
        public static readonly string MageHandMythicFeatGuid = "{FFA0C7DA-1BA2-43C8-9242-05228125F664}";

        private static readonly string MageHandMythicDisplayName = "MageHandMageHandMythic.Name";
        private static readonly string MageHandMythicDescription = "MageHandMageHandMythic.Description";

        public static void ConfigureMageHandMythic()
        {
            var icon = AbilityRefs.DispelMagicGreater.Reference.Get().Icon;

            FeatureConfigurator.New(MageHandMythicFeatName, MageHandMythicFeatGuid, FeatureGroup.MythicFeat)
                    .SetDisplayName(MageHandMythicDisplayName)
                    .SetDescription(MageHandMythicDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(MageHandMainFeatGuid)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
                    .Configure();
        }

        private static readonly string DirtyMagicTrickFeatName = "MageHandDirtyMagicTrick";
        private static readonly string DirtyMagicTrickFeatGuid = "{F9E7A9DC-96A4-4D4E-BB71-E68809BDB9D0}";

        private static readonly string DirtyMagicTrickDisplayName = "MageHandDirtyMagicTrick.Name";
        private static readonly string DirtyMagicTrickDescription = "MageHandDirtyMagicTrick.Description";

        public static BlueprintFeature ConfigureDirtyMagicTrick()
        {
            var icon = AbilityRefs.DispelMagic.Reference.Get().Icon;

            return FeatureConfigurator.New(DirtyMagicTrickFeatName, DirtyMagicTrickFeatGuid)
                    .SetDisplayName(DirtyMagicTrickDisplayName)
                    .SetDescription(DirtyMagicTrickDescription)
                    .SetIcon(icon)
                    .AddAutoMetamagic(new() { AbilityRefs.DirtyTrickBlindnessAction.ToString(), AbilityRefs.DirtyTrickEntangleAction.ToString(), AbilityRefs.DirtyTrickSickenedAction.ToString() },
              metamagic: Metamagic.Reach, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.Any, once: false)
                    .Configure();
        }

        private static readonly string RangedAidFeatName = "MageHandRangedAid";
        private static readonly string RangedAidFeatGuid = "{1D567F1A-8883-4279-B928-22F3650EEDFF}";

        private static readonly string RangedAidDisplayName = "MageHandRangedAid.Name";
        private static readonly string RangedAidDescription = "MageHandRangedAid.Description";

        public static BlueprintFeature ConfigureRangedAid()
        {
            var icon = AbilityRefs.DispelMagic.Reference.Get().Icon;
            var AidAnotherAbility = "1eea0d80-c6b7-407a-a8a9-02f6e750ea95";
            var AidAnotherDefenceAbility = "4c2985ad-5b00-4fed-b94b-3467b80b793a";
            var AidAnotherOffenceAbility = "0b051b7c-166c-4b90-93ed-dbc42af2bb45";

            var SwiftAidAnotherAbility = "840e5398-3d98-4348-9554-f76cc764b776";
            var SwiftAidAnotherDefenceAbility = "c341e4ae-f26c-4189-af2a-1d72c994c1e3";
            var SwiftAidAnotherOffenceAbility = "eda56623-9a34-4d69-bc78-7a4fcaabc092";

            return FeatureConfigurator.New(RangedAidFeatName, RangedAidFeatGuid)
                    .SetDisplayName(RangedAidDisplayName)
                    .SetDescription(RangedAidDescription)
                    .SetIcon(icon)
                    .AddAutoMetamagic(new() { AidAnotherAbility, AidAnotherDefenceAbility, AidAnotherOffenceAbility, SwiftAidAnotherAbility, SwiftAidAnotherDefenceAbility, SwiftAidAnotherOffenceAbility },
              metamagic: Metamagic.Reach, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.Any, once: false)
                    .Configure();
        }

        private static readonly string ThrowPunchFeatName = "MageHandThrowPunch";
        private static readonly string ThrowPunchFeatGuid = "{01B800EC-BBFF-49CF-841C-0657DF53F356}";

        private static readonly string ThrowPunchDisplayName = "MageHandThrowPunch.Name";
        private static readonly string ThrowPunchDescription = "MageHandThrowPunch.Description";

        private const string ThrowPunchbuff = "MageHandTrick.ThrowPunchbuff";
        public static readonly string ThrowPunchbuffGuid = "{A6997B23-0803-42F1-84F6-4506793A888E}";

        private const string ThrowPunchActivatableAbility = "MageHandTrick.ThrowPunchActivatableAbility";
        private static readonly string ThrowPunchActivatableAbilityGuid = "{4E0D0E93-F74C-4CB3-B6D7-FF0B705B3E2C}";

        public static BlueprintFeature ConfigureThrowPunch()
        {
            var icon = AbilityRefs.DispelMagic.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(ThrowPunchbuff, ThrowPunchbuffGuid)
              .SetDisplayName(ThrowPunchDisplayName)
              .SetDescription(ThrowPunchDescription)
              .SetIcon(icon)
              .AddWeaponDamageOverride(formula: new DiceFormula(1, DiceType.D3), overrideDice: true, weaponTypes: new() { WeaponTypeRefs.Unarmed.Cast<BlueprintWeaponTypeReference>() })
              .AddComponent<ThrowPunchStuff>()
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(ThrowPunchActivatableAbility, ThrowPunchActivatableAbilityGuid)
                .SetDisplayName(ThrowPunchDisplayName)
                .SetDescription(ThrowPunchDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();

            return FeatureConfigurator.New(ThrowPunchFeatName, ThrowPunchFeatGuid)
                    .SetDisplayName(ThrowPunchDisplayName)
                    .SetDescription(ThrowPunchDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private static readonly string FeatName = "ReachingHand";
        public static readonly string FeatGuid = "{F9223ADC-494C-4EF3-B5D9-08DE2D186700}";

        private static readonly string DisplayName = "ReachingHand.Name";
        private static readonly string Description = "ReachingHand.Description";

        private const string ReachingHandAbility = "ReachingHand.ReachingHandAbility";
        private static readonly string ReachingHandAbilityGuid = "{C1E96CC4-ECA3-4B80-815C-7E593020DFB2}";

        private const string ReachingHandbuff = "ReachingHand.ReachingHandbuff";
        public static readonly string ReachingHandbuffGuid = "{672728F0-0DE2-4A3B-9500-D8CD25B6B035}";

        public static void ReachingHandConfigure()
        {
            var icon = FeatureRefs.ReachSpellFeat.Reference.Get().Icon;

            var BuffReachingHand = BuffConfigurator.New(ReachingHandbuff, ReachingHandbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .Configure();

            var abilityTrick = AbilityConfigurator.New(ReachingHandAbility, ReachingHandAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(BuffReachingHand, ContextDuration.Fixed(1))
                        .Build())
                .SetCanTargetSelf(true)
                .SetCanTargetFriends(false)
                .SetCanTargetEnemies(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddFacts(new() { abilityTrick })
                    .Configure();
        }
    }
}
