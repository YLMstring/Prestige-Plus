using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Actions.Builder.ContextEx;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomComponent.Spell;

namespace PrestigePlus.Blueprint.Spell
{
    internal class TelekineticManeuver
    {
        private const string TelekineticManeuverAbility = "NewSpell.UseTelekineticManeuver";
        public static readonly string TelekineticManeuverAbilityGuid = "{7BBDA215-8045-4D69-8D48-A6AE290910FB}";

        private const string TelekineticManeuverBuff = "NewSpell.TelekineticManeuverBuff";
        private static readonly string TelekineticManeuverBuffGuid = "{D8D87291-1B53-4654-95E3-7202762248E6}";

        private const string TelekineticManeuverBuff2 = "NewSpell.TelekineticManeuverBuff2";
        private static readonly string TelekineticManeuverBuff2Guid = "{7F311C10-43AD-4E20-97C1-2644ECB4F6FC}";

        internal const string DisplayName = "NewSpellTelekineticManeuver.Name";
        private const string Description = "NewSpellTelekineticManeuver.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.TelekineticFist.Reference.Get().Icon;

            var buff2 = BuffConfigurator.New(TelekineticManeuverBuff2, TelekineticManeuverBuff2Guid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddFacts(new() { AbilityRefs.BullRushAction.ToString(), AbilityRefs.DisarmAction.ToString(), AbilityRefs.TripAction.ToString(), ImprovedGrapple.StyleAbilityGuid })
              .AddAutoMetamagic(new() { AbilityRefs.BullRushAction.ToString(), AbilityRefs.DisarmAction.ToString(), AbilityRefs.TripAction.ToString() },
              metamagic: Metamagic.Reach, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.Any, once: false)
              .AddComponent<TelekineticManeuverComp>()
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(buff2, ContextDuration.Fixed(1), isFromSpell: true)
                .Build();

            var buff = BuffConfigurator.New(TelekineticManeuverBuff, TelekineticManeuverBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddNewRoundTrigger(newRoundActions: action)
              .AddFacts(new() { PinAbilityGuid1, TieUpAbilityGuid, ReadyAbilityGuid, ReleaseAbilityGuid })
              .Configure();

            AbilityConfigurator.NewSpell(
                TelekineticManeuverAbility, TelekineticManeuverAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetLocalizedDuration(AbilityRefs.Haste.Reference.Get().LocalizedDuration)
              .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
              .SetSpellResistance(true)
              //.SetIgnoreSpellResistanceForAlly(true)
              .SetAnimation(CastAnimationStyle.Omni)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Extend)
              .AddToSpellLists(level: 4, SpellList.Wizard)
              .AddToSpellLists(level: 4, SpellList.Magus)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff2, ContextDuration.Fixed(1), isFromSpell: true)
                  .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank()), isFromSpell: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();
        }

        private static readonly string PinAbilityGuid1 = "{531632AA-0E0F-402C-8A07-18E8E0D35C80}";
        private static readonly string TieUpAbilityGuid = "{DB453CF7-8799-4FDD-941B-FA33EFF5771A}";
        private static readonly string ReadyAbilityGuid = "{A5057A11-9D24-46D8-9BE6-F5C7D605EDC5}";
        private static readonly string ReleaseAbilityGuid = "{A75ED2DD-7F0D-4367-9953-4179F3E531D2}";
    }
}
