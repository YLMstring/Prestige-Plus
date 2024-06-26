using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomComponent.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using PrestigePlus.CustomAction.ClassRelated;
using HarmonyLib;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic;
using BlueprintCore.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using static Kingmaker.GameModes.GameModeType;
using Kingmaker.Designers;
using Kingmaker.UnitLogic.Mechanics.Actions;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.Blueprint.Spell
{
    internal class VirtuosoPerformance
    {
        private const string VirtuosoPerformanceAbility = "NewSpell.UseVirtuosoPerformance";
        public static readonly string VirtuosoPerformanceAbilityGuid = "{31E5B2F7-B4AC-4B8C-BE0F-B9824EDE6FA7}";

        private const string VirtuosoPerformanceBuff = "NewSpell.VirtuosoPerformanceBuff";
        public static readonly string VirtuosoPerformanceBuffGuid = "{FA73F1B7-79E4-423D-919D-91665E1D4BED}";

        internal const string DisplayName = "NewSpellVirtuosoPerformance.Name";
        private const string Description = "NewSpellVirtuosoPerformance.Description";
        public static void Configure()
        {
            var icon = FeatureRefs.FascinateFeature.Reference.Get().Icon;
            var fx = AbilityRefs.Serenity.Reference.Get().GetComponent<AbilitySpawnFx>();

            var action = ActionsBuilder.New()
                .Add<DeactivateAbilityFromGroup>(c => { c.group = ActivatableAbilityGroup.BardicPerformance; c.num_abilities_activated = 1; })
                .Build();

            var action2 = ActionsBuilder.New()
                .Add<VirtuosoSpend>(c => { c.ResGuid = AbilityResourceRefs.BardicPerformanceResource.ToString(); })
                .Build();

            var buff = BuffConfigurator.New(VirtuosoPerformanceBuff, VirtuosoPerformanceBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddBuffActions(deactivated: action, newRound: action2)
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.BardicPerformance)
              .Configure();

            AbilityConfigurator.NewSpell(
                VirtuosoPerformanceAbility, VirtuosoPerformanceAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetLocalizedDuration(AbilityRefs.Haste.Reference.Get().LocalizedDuration)
              .SetAnimation(CastAnimationStyle.SelfTouch)
              .AddComponent(fx)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Extend)
              .AddToSpellLists(level: 4, SpellList.Bard)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank()), isFromSpell: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();
        }
    }

    public class DeactivateAbilityFromGroup : ContextAction
    {
        public ActivatableAbilityGroup group;
        public int num_abilities_activated;
        public override string GetCaption()
        {
            return $"Deactivate Ability From Group {group} if more than {num_abilities_activated}.";
        }
        public override void RunAction()
        {
            var unit = this.Target?.Unit;
            if (unit == null)
            {
                return;
            }
            int num_activated = 0;
            foreach (var a in unit.ActivatableAbilities)
            {
                if (a.Blueprint.Group == group && a.IsOn)
                {
                    if (num_activated < num_abilities_activated)
                    {
                        num_activated++;
                    }
                    else
                    {
                        a.Stop();
                    }
                }

            }
        }
    }

    [HarmonyPatch(typeof(ActivatableAbility), nameof(ActivatableAbility.ReapplyBuff))]
    internal static class VirtuosoPerformancePatch
    {
        private static bool Prefix(ref ActivatableAbility __instance)
        {
            try
            {
                if (__instance.Owner.HasFact(virt) && __instance.Blueprint.Group == ActivatableAbilityGroup.BardicPerformance)
                {
                    if (!__instance.IsOn || !__instance.IsStarted)
                    {
                        return false;
                    }
                    using (ContextData<ActivatableAbility.ReapplyBuffContextData>.Request().Setup(__instance))
                    {
                        if (__instance.m_AppliedBuff != null)
                        {
                            Buff appliedBuff = __instance.m_AppliedBuff;
                            __instance.m_AppliedBuff = null;
                            appliedBuff.Remove();
                        }
                        if (__instance.Blueprint.Buff)
                        {
                            foreach (ActivatableAbility activatableAbility in __instance.Owner.ActivatableAbilities.RawFacts)
                            {
                                if (activatableAbility.Blueprint.Group == ActivatableAbilityGroup.BardicPerformance && !activatableAbility.IsStarted && !activatableAbility.IsOn)
                                {
                                    GameHelper.RemoveBuff(__instance.Owner, activatableAbility.Blueprint.Buff);
                                }
                            }
                            MechanicsContext parentContext = new(__instance.Owner.Unit, null, __instance.Blueprint, null, null);
                            __instance.m_AppliedBuff = __instance.Owner.AddBuff(__instance.Blueprint.Buff, parentContext, null);
                            if (__instance.m_AppliedBuff != null)
                            {
                                __instance.m_AppliedBuff.IsNotDispelable = true;
                            }
                        }
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex) { Main.Logger.Error("Failed to VirtuosoPerformancePatch.", ex); return true; }
        }

        private static BlueprintBuffReference virt = BlueprintTool.GetRef<BlueprintBuffReference>(VirtuosoPerformance.VirtuosoPerformanceBuffGuid);
    }
}
