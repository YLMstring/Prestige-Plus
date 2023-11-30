using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.CustomComponent.Spell;

namespace PrestigePlus.Blueprint.Spell
{
    internal class LitanyRighteousness
    {
        private const string LitanyRighteousnessAbility = "NewSpell.UseLitanyRighteousness";
        public static readonly string LitanyRighteousnessAbilityGuid = "{42F00777-3689-4E27-A696-81F5F1CF1342}";

        private const string LitanyRighteousnessBuff = "NewSpell.LitanyRighteousnessBuff";
        private static readonly string LitanyRighteousnessBuffGuid = "{481F07A7-19D4-45DF-82F5-D377BCDCA2EE}";

        internal const string DisplayName = "NewSpellLitanyRighteousness.Name";
        private const string Description = "NewSpellLitanyRighteousness.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.SlayerStudyTargetAbility.Reference.Get().Icon;
            var buff = BuffConfigurator.New(LitanyRighteousnessBuff, LitanyRighteousnessBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<LitanyRighteousnessDamage>()
              .Configure();

            AbilityConfigurator.NewSpell(
                LitanyRighteousnessAbility, LitanyRighteousnessAbilityGuid, SpellSchool.Evocation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(enemies: true)
              .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
              .AddAbilityTargetAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.Evil)
              .SetAnimation(CastAnimationStyle.Special)
              .SetActionType(CommandType.Swift)
              .SetRange(AbilityRange.Close)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Reach, Metamagic.Heighten)
              .AddToSpellLists(level: 2, SpellList.Paladin)
              .AddToSpellLists(level: 3, SpellList.Inquisitor)
              .SetSpellDescriptor(SpellDescriptor.Good)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuff(buff, ContextDuration.Fixed(1))
                        .Build())
                  .Build(), savingThrowType: SavingThrowType.Will)
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.Will,
                spellType: CraftSpellType.Debuff)
              .Configure();
        }
    }
}
