using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomComponent.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using BlueprintCore.Actions.Builder.ContextEx;

namespace PrestigePlus.Blueprint.Spell
{
    internal class ShieldOther
    {
        private const string ShieldOtherAbility = "NewSpell.UseShieldOther";
        public static readonly string ShieldOtherAbilityGuid = "{42F00777-3689-4E27-A696-81F5F1CF1342}";

        private const string ShieldOtherBuff = "NewSpell.ShieldOtherBuff";
        private static readonly string ShieldOtherBuffGuid = "{481F07A7-19D4-45DF-82F5-D377BCDCA2EE}";

        internal const string DisplayName = "NewSpellShieldOther.Name";
        private const string Description = "NewSpellShieldOther.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.AuraOfJusticeFeature.Reference.Get().Icon;
            var buff = BuffConfigurator.New(ShieldOtherBuff, ShieldOtherBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)

              .Configure();

            AbilityConfigurator.NewSpell(
                ShieldOtherAbility, ShieldOtherAbilityGuid, SpellSchool.Evocation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(enemies: true)
              .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
              .AddAbilityTargetAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.Evil)
              .SetAnimation(CastAnimationStyle.Directional)
              .SetActionType(CommandType.Swift)
              .SetRange(AbilityRange.Close)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Reach, Metamagic.Heighten)
              .AddToSpellLists(level: 2, SpellList.Paladin)
              .AddToSpellLists(level: 3, SpellList.Inquisitor)
              .SetSpellDescriptor(SpellDescriptor.Good)
              .SetSpellResistance()
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuff(buff, ContextDuration.Fixed(1))
                        .Build())
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();
        }
    }
}
