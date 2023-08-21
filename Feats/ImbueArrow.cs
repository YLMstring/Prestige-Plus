using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using System.Drawing;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Blueprints;
using Kingmaker.UI.MVVM._VM.Other;
using PrestigePlus.Modify;

namespace PrestigePlus.Feats
{
    internal class ImbueArrow
    {
        private static readonly string FeatName = "ImbueArrow";
        public static readonly string FeatGuid = "{6EEC84C6-0845-4537-A72A-57B6B2329337}";

        private static readonly string DisplayName = "ImbueArrow.Name";
        private static readonly string Description = "ImbueArrow.Description";

        private const string ImbueArrowBuff = "ImbueArrowBuff";
        private static readonly string ImbueArrowGuidBuff = "{D6049695-75E3-46D2-B708-828612365E42}";

        private const string ImbueArrowBuff3 = "ImbueArrowBuff3";
        private static readonly string ImbueArrowGuidBuff3 = "{1AA5DD72-A6E0-43E9-B751-954ACDD3D34E}";

        public static void Configure()
        {
            var icon = AbilityRefs.HurricaneBow.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ImbueArrowBuff, ImbueArrowGuidBuff)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .Configure(); 
            
            var ability = AbilityConfigurator.New(ImbueArrowBuff3, ImbueArrowGuidBuff3)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuff(Buff1, ContextDuration.Fixed(1))
                    .Build())
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();

            foreach (var spell in AbilityRefs.All)
                AddImbueToSpell(spell.Reference.Get());
        }

        private static void AddImbueToSpell(BlueprintAbility spell)
        {
            var shoot = ActionsBuilder.New().Build();
            try
            {
                if (spell == null)
                    return;

                if (spell.Type != AbilityType.Spell)
                    return;
            }
            catch (Exception e)
            {
                LogWrapper.Get("PrestigePlus").Error("Failed to configure 11.", e);
            }
            try
            {
                bool isAOE = false;

                if (spell.GetComponent<AbilityTargetsAround>() != null)
                    isAOE = true; 
                
                if (spell.GetComponent<AbilityAoERadius>() != null)
                    isAOE = true;

                if (spell.GetComponent<AbilityDeliverProjectile>() != null)
                    if (spell.GetComponent<AbilityDeliverProjectile>().Type == AbilityProjectileType.Line)
                        isAOE = true;
                if (spell.GetComponent<AbilityDeliverProjectile>() != null)
                    if (spell.GetComponent<AbilityDeliverProjectile>().Type == AbilityProjectileType.Cone)
                        isAOE = true;

                if (isAOE == false)
                    return;
            }
            catch (Exception e)
            {
                LogWrapper.Get("PrestigePlus").Error("Failed to configure 12.", e);
            }
            try
            {
                if (spell.GetComponent<AbilityEffectRunAction>() == null)
                    return;

                if (spell.GetComponent<AbilityEffectRunAction>().Actions == null)
                    return;

                if (spell.GetComponent<AbilityEffectRunAction>().Actions.Actions == null)
                    return;
            }
            catch (Exception e)
            {
                LogWrapper.Get("PrestigePlus").Error("Failed to configure 13.", e);
            }

            try
            {
                var originalList = spell.GetComponent<AbilityEffectRunAction>().Actions;
                var original = ActionsBuilder.New()
                            .AddAll(originalList)
                            .Build();

                shoot = ActionsBuilder.New()
                        .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(ImbueArrowGuidBuff).Build(), ifFalse: original,
                        ifTrue: ActionsBuilder.New()
                            .Conditional(conditions: ConditionsBuilder.New().IsMainTarget().Build(), ifFalse: ActionsBuilder.New().Build(),
                            ifTrue: ActionsBuilder.New()
                                .RemoveBuff(ImbueArrowGuidBuff, toCaster: true)
                                .Add<RangedAttackExtended>(a => a.OnHit = ActionsBuilder.New().CastSpell(spell, false).Build())
                                .Build())
                            .Build())
                        .Build();
            }
            catch (Exception e)
            {
                LogWrapper.Get("PrestigePlus").Error("Failed to configure 2.", e);
            }
            AbilityConfigurator.For(spell)
              .EditComponent<AbilityEffectRunAction>(
                a => a.Actions.Actions = shoot.Actions)
              .Configure(delayed: true); 
            
        }
    }
}
