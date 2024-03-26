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

namespace PrestigePlus.Blueprint.Spell
{
    internal class VirtuosoPerformance
    {
        //don't work because of how lingering performance is done and when it ends things don't get removed, anyway too complicated
        
        private const string VirtuosoPerformanceAbility = "NewSpell.UseVirtuosoPerformance";
        public static readonly string VirtuosoPerformanceAbilityGuid = "{31E5B2F7-B4AC-4B8C-BE0F-B9824EDE6FA7}";

        private const string VirtuosoPerformanceBuff = "NewSpell.VirtuosoPerformanceBuff";
        private static readonly string VirtuosoPerformanceBuffGuid = "{FA73F1B7-79E4-423D-919D-91665E1D4BED}";

        internal const string DisplayName = "NewSpellVirtuosoPerformance.Name";
        private const string Description = "NewSpellVirtuosoPerformance.Description";
        public static void Configure()
        {
            var icon = FeatureRefs.FascinateFeature.Reference.Get().Icon;
            var fx = AbilityRefs.Serenity.Reference.Get().GetComponent<AbilitySpawnFx>();

            var action = ActionsBuilder.New()
                .Add<VirtuosoSpend>(c => { c.ResGuid = AbilityResourceRefs.BardicPerformanceResource.ToString(); })
                .Build();

            var buff = BuffConfigurator.New(VirtuosoPerformanceBuff, VirtuosoPerformanceBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddNewRoundTrigger(newRoundActions: action)
              .AddIncreaseActivatableAbilityGroupSize(Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityGroup.BardicPerformance)
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
              .AddToSpellLists(level: 1, SpellList.Bard)
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
}
