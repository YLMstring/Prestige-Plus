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
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Blueprint.PrestigeClass;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Properties;

namespace PrestigePlus.Blueprint.Spell
{
    internal class ShieldOther
    {
        private const string ShieldOtherAbility = "NewSpell.UseShieldOther";
        public static readonly string ShieldOtherAbilityGuid = "{58B06F82-B02A-44C1-96CF-C508C7EFA983}";

        private const string ShieldOtherBuff = "NewSpell.ShieldOtherBuff";
        private static readonly string ShieldOtherBuffGuid = "{4B0415F2-D08D-45A5-9C65-EA59DAA71B7E}";

        private const string ShieldOtherAbility2 = "NewSpell.ShieldOtherAbility2";
        private static readonly string ShieldOtherAbility2Guid = "{551AE36E-8772-4F7B-B9F4-D01F831031A6}";

        internal const string DisplayName = "NewSpellShieldOther.Name";
        private const string Description = "NewSpellShieldOther.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.ShieldOfLaw.Reference.Get().Icon;

            var buff = BuffConfigurator.New(ShieldOtherBuff, ShieldOtherBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<ScarSacrifice>()
              .AddRemoveBuffIfCasterIsMissing(removeOnCasterDeath: true)
              .AddFacts(new() { ShieldOtherAbility2Guid })
              .AddContextStatBonus(StatType.AC, value: 1, descriptor: ModifierDescriptor.Deflection)
              .AddBuffAllSavesBonus(descriptor: ModifierDescriptor.Resistance, value: 1)
              .Configure();

            var ability = AbilityConfigurator.New(ShieldOtherAbility2, ShieldOtherAbility2Guid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetType(AbilityType.Special)
                .SetRange(AbilityRange.Personal)
                .SetActionType(CommandType.Free)
                .SetAnimation(CastAnimationStyle.Self)
                .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .RemoveBuff(buff)
                  .Build())
                .Configure();

            AbilityConfigurator.NewSpell(
                ShieldOtherAbility, ShieldOtherAbilityGuid, SpellSchool.Abjuration, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(false, false, true, false)
              .SetAnimation(CastAnimationStyle.Point)
              .SetRange(AbilityRange.Close)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Extend)
              .AddToSpellLists(level: 2, SpellList.Cleric)
              .AddToSpellLists(level: 2, SpellList.Warpriest)
              .AddToSpellLists(level: 2, SpellList.Paladin)
              .AddToSpellLists(level: 2, SpellList.Inquisitor)
              .AddToSpellLists(level: 2, SpellList.CommunityDomain)
              .AddToSpellLists(level: 2, SpellList.ProtectionDomain)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), DurationRate.Hours), isFromSpell: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();
        }
    }
}
