using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Aeromancer
    {
        private const string ArchetypeName = "Aeromancer";
        public static readonly string ArchetypeGuid = "{2BFBA710-06C5-446E-B7B3-426EE41236C9}";
        internal const string ArchetypeDisplayName = "Aeromancer.Name";
        private const string ArchetypeDescription = "Aeromancer.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.ArcanistClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .AddToRemoveFeatures(1, FeatureSelectionRefs.ArcanistExploitSelection.ToString())
            .AddToRemoveFeatures(5, FeatureSelectionRefs.ArcanistExploitSelection.ToString())
            .AddToRemoveFeatures(11, FeatureSelectionRefs.ArcanistExploitSelection.ToString())
            .AddToAddFeatures(1, CreateAirMastery())
            .AddToAddFeatures(5, CreateWindEmbrace())
            .AddToAddFeatures(11, CreateRebukingGale1(), CreateRebukingGale2())
              .Configure();
        }

        private const string WindEmbrace = "Aeromancer.WindEmbrace";
        private static readonly string WindEmbraceGuid = "{D1D806F2-3012-49F8-9BAD-66F869C5EB4C}";

        private const string WindEmbraceAbility = "Aeromancer.WindEmbraceAbility";
        private static readonly string WindEmbraceAbilityGuid = "{92259134-ABC4-497E-BC22-985A469A9981}";

        internal const string WindEmbraceDisplayName = "AeromancerWindEmbrace.Name";
        private const string WindEmbraceDescription = "AeromancerWindEmbrace.Description";
        private static BlueprintFeature CreateWindEmbrace()
        {
            var icon = FeatureRefs.WindsOfTheFallFeature.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(WindEmbraceAbility, WindEmbraceAbilityGuid)
                .SetDisplayName(WindEmbraceDisplayName)
                .SetDescription(WindEmbraceDescription)
                .SetIcon(icon)
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Personal)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .AddAbilityTargetsAround(includeDead: false, radius: 30.Feet(), spreadSpeed: 10.Feet(), targetType: TargetType.Ally)
                .AddAbilityEffectRunAction(actions: ActionsBuilder.New()
                  .ApplyBuff(BuffRefs.ProtectionFromArrowsCommunalBuff.ToString(), ContextDuration.Fixed(10))
                  .ApplyBuff(BuffRefs.FreedomOfMovementBuff.ToString(), ContextDuration.Fixed(10), toCaster: true)
                  .Build())
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: AbilityResourceRefs.ArcanistArcaneReservoirResource.ToString())
                .Configure();

            return FeatureConfigurator.New(WindEmbrace, WindEmbraceGuid)
              .SetDisplayName(WindEmbraceDisplayName)
              .SetDescription(WindEmbraceDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string RebukingGale1 = "Aeromancer.RebukingGale1";
        private static readonly string RebukingGale1Guid = "{E90BAF61-4D5F-4CB8-90C6-7D7E5B56D6BE}";

        private const string RebukingGale1Ability = "Aeromancer.RebukingGale1Ability";
        private static readonly string RebukingGale1AbilityGuid = "{F3A238DF-E13B-4128-A092-2DEEB6E55136}";

        private const string RebukingGale1Buff = "Aeromancer.RebukingGale1Buff";
        private static readonly string RebukingGale1BuffGuid = "{9D4E49A6-D5AB-4681-986D-71E54D0A64CC}";

        internal const string RebukingGale1DisplayName = "AeromancerRebukingGale1.Name";
        private const string RebukingGale1Description = "AeromancerRebukingGale1.Description";
        private static BlueprintFeature CreateRebukingGale1()
        {
            var icon = FeatureRefs.WindsOfTheFallFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(RebukingGale1Buff, RebukingGale1BuffGuid)
              .SetDisplayName(RebukingGale1DisplayName)
              .SetDescription(RebukingGale1Description)
              .SetIcon(icon)
              .AddComponent<RebukingGaleMiss>()
              .Configure();

            var ability = AbilityConfigurator.New(RebukingGale1Ability, RebukingGale1AbilityGuid)
                .SetDisplayName(RebukingGale1DisplayName)
                .SetDescription(RebukingGale1Description)
                .SetIcon(icon)
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Personal)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .AddAbilityTargetsAround(includeDead: false, radius: 20.Feet(), spreadSpeed: 10.Feet(), targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Any)
                .AddAbilityEffectRunAction(actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Fixed(1))
                  .Build())
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: AbilityResourceRefs.ArcanistArcaneReservoirResource.ToString())
                .Configure();

            return FeatureConfigurator.New(RebukingGale1, RebukingGale1Guid)
              .SetDisplayName(RebukingGale1DisplayName)
              .SetDescription(RebukingGale1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string RebukingGale2 = "Aeromancer.RebukingGale2";
        private static readonly string RebukingGale2Guid = "{E9869533-3CF4-41FE-B1C8-A861A9916E22}";

        private const string RebukingGale2Ability = "Aeromancer.RebukingGale2Ability";
        private static readonly string RebukingGale2AbilityGuid = "{979466CF-388A-4B6E-81A2-4D49FC9301FE}";

        private const string RebukingGale2Buff = "Aeromancer.RebukingGale2Buff";
        private static readonly string RebukingGale2BuffGuid = "{41F073B5-0A6F-44B4-B5B2-5F976447AAA0}";

        internal const string RebukingGale2DisplayName = "AeromancerRebukingGale2.Name";
        private const string RebukingGale2Description = "AeromancerRebukingGale2.Description";
        private static BlueprintFeature CreateRebukingGale2()
        {
            var icon = FeatureRefs.WindsOfTheFallFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(RebukingGale2Buff, RebukingGale2BuffGuid)
              .SetDisplayName(RebukingGale2DisplayName)
              .SetDescription(RebukingGale2Description)
              .SetIcon(icon)
              .AddComponent<RebukingGaleMiss>()
              .Configure();

            var ability = AbilityConfigurator.New(RebukingGale2Ability, RebukingGale2AbilityGuid)
                .SetDisplayName(RebukingGale2DisplayName)
                .SetDescription(RebukingGale2Description)
                .SetIcon(icon)
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Projectile)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .AllowTargeting(true, true, true, true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
                .AddAbilityDeliverProjectile(projectiles: new() { ProjectileRefs.NecromancyCone30Feet00.ToString() }, type: AbilityProjectileType.Cone, length: 30.Feet(), lineWidth: 5.Feet(), needAttackRoll: false)
                .AddAbilityEffectRunAction(actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Fixed(1))
                  .ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true)
                  .Build())
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: AbilityResourceRefs.ArcanistArcaneReservoirResource.ToString())
                .Configure();

            return FeatureConfigurator.New(RebukingGale2, RebukingGale2Guid)
              .SetDisplayName(RebukingGale2DisplayName)
              .SetDescription(RebukingGale2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string AirMastery = "Aeromancer.AirMastery";
        private static readonly string AirMasteryGuid = "{99EE90B9-C233-4F17-8D88-A6193175231F}";

        private const string AirMasteryBuff = "Aeromancer.AirMasteryBuff";
        private static readonly string AirMasteryBuffGuid = "{03AC401A-1CE6-4718-8FBA-B25952B8C97B}";

        private const string AirMasteryBuff2 = "Aeromancer.AirMasteryBuff2";
        private static readonly string AirMasteryBuff2Guid = "{DCFB1214-0C89-46F5-9A9C-510E8D59E56E}";

        internal const string AirMasteryDisplayName = "AeromancerAirMastery.Name";
        private const string AirMasteryDescription = "AeromancerAirMastery.Description";
        private static BlueprintFeature CreateAirMastery()
        {
            var icon = FeatureRefs.WindsOfTheFallFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(AirMasteryBuff, AirMasteryBuffGuid)
              .SetDisplayName(AirMasteryDisplayName)
              .SetDescription(AirMasteryDescription)
              .SetIcon(icon)
              
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var buff2 = BuffConfigurator.New(AirMasteryBuff2, AirMasteryBuff2Guid)
              .SetDisplayName(AirMasteryDisplayName)
              .SetDescription(AirMasteryDescription)
              .SetIcon(icon)

              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            return FeatureConfigurator.New(AirMastery, AirMasteryGuid)
              .SetDisplayName(AirMasteryDisplayName)
              .SetDescription(AirMasteryDescription)
              .SetIcon(icon)
              .AddBuffExtraEffects(BuffRefs.ArcanistArcaneReservoirCLBuff.ToString(), extraEffectBuff: buff)
              .AddBuffExtraEffects(BuffRefs.ArcanistArcaneReservoirDCBuff.ToString(), extraEffectBuff: buff2)
              .AddBuffExtraEffects(BuffRefs.ArcanistArcaneReservoirCLPotentBuff.ToString(), extraEffectBuff: buff)
              .AddBuffExtraEffects(BuffRefs.ArcanistArcaneReservoirDCPotentBuff.ToString(), extraEffectBuff: buff2)
              .AddBuffExtraEffects(BuffRefs.EldritchFontEldritchSurgeCLBuff.ToString(), extraEffectBuff: buff)
              .AddBuffExtraEffects(BuffRefs.EldritchFontEldritchSurgeDCBuff.ToString(), extraEffectBuff: buff2)
              .Configure();
        }
    }
}
