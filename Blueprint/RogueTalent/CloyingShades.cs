using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.Abilities.Components;
using PrestigePlus.Modify;
using PrestigePlus.CustomComponent.Feat;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using BlueprintCore.Conditions.Builder.ContextEx;

namespace PrestigePlus.Blueprint.RogueTalent
{
    internal class CloyingShades
    {
        private const string CloyingShadesPower = "Rogue.CloyingShades";
        public static readonly string CloyingShadesGuid = "{41C482BD-9727-40E7-A967-626253AA67D6}";

        internal const string CloyingShadesDisplayName = "RogueCloyingShades.Name";
        private const string CloyingShadesDescription = "RogueCloyingShades.Description";

        private const string CloyingShadesAbility = "Rogue.CloyingShadesAbility";
        public static readonly string CloyingShadesAbilityGuid = "{52D16341-7463-46BE-B64A-032DAA2E0B70}";

        private const string CloyingShadesAbility2 = "Rogue.CloyingShadesAbility2";
        public static readonly string CloyingShadesAbility2Guid = "{E3A26B9E-133D-4294-A002-3F65AA1EDCA1}";

        private const string CloyingShadesBuff = "Rogue.CloyingShadesBuff";
        public static readonly string CloyingShadesBuffGuid = "{B3E304A2-B72F-4C3D-B257-C2E7E89E3325}";

        public static BlueprintFeature CloyingShadesFeat()
        {
            var icon = AbilityRefs.WavesOfExhaustion.Reference.Get().Icon;

            AbilityConfigurator.New(CloyingShadesAbility, CloyingShadesAbilityGuid)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().IsCaster(true).Build(), ifTrue: 
                        ActionsBuilder.New()
                        .ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(BuffRefs.EntangleBuff.ToString(), ContextDuration.Fixed(1)).Build())
                        .Build())
                    .Build(), savingThrowType: Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex)
                .SetDisplayName(CloyingShadesDisplayName)
                .SetDescription(CloyingShadesDescription)
                .SetIcon(icon)
                .AddAbilityTargetsAround(includeDead: false, radius: 5.Feet(), targetType: TargetType.Any)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddComponent<CustomDC>(c => { c.Property = Kingmaker.EntitySystem.Stats.StatType.Intelligence; c.Property2 = Kingmaker.EntitySystem.Stats.StatType.Charisma; c.classguid = CharacterClassRefs.RogueClass.ToString(); c.halfed = true; c.isRogue = true; })
                .Configure();

            AbilityConfigurator.New(CloyingShadesAbility2, CloyingShadesAbility2Guid)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .RemoveBuff(CloyingShadesBuffGuid)
                    .Build())
                .SetDisplayName(CloyingShadesDisplayName)
                .SetDescription(CloyingShadesDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .Configure();

            BuffConfigurator.New(CloyingShadesBuff, CloyingShadesBuffGuid)
              .SetDisplayName(CloyingShadesDisplayName)
              .SetDescription(CloyingShadesDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddBuffActions(deactivated: ActionsBuilder.New().CastSpell(CloyingShadesAbilityGuid).Build())
              .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().RemoveSelf().Build(), true, triggerBeforeAttack: true)
              .AddFacts(new() { CloyingShadesAbility2Guid })
              .Configure();

            return FeatureConfigurator.New(CloyingShadesPower, CloyingShadesGuid)
              .SetDisplayName(CloyingShadesDisplayName)
              .SetDescription(CloyingShadesDescription)
              .SetIcon(icon)
              .AddComponent<CastCloyingShade>()
              .AddToGroups(FeatureGroup.RogueTalent)
              .Configure();
        }
    }
}
