using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.ModReferences;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Parts;
using System.Threading;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs;

namespace PrestigePlus.Blueprint.Spell
{
    internal class ToothHunt
    {
        private const string ToothHuntAbility = "NewSpell.UseToothHunt";
        public static readonly string ToothHuntAbilityGuid = "{561B6EBB-9444-4B32-A140-8AE3FF7C0515}";

        private const string ToothHuntBuff = "NewSpell.ToothHuntBuff";
        private static readonly string ToothHuntBuffGuid = "{9DF7EC63-2A6D-4364-BB2A-B8EDB5924E0A}";

        internal const string DisplayName = "NewSpellToothHunt.Name";
        private const string Description = "NewSpellToothHunt.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.BlackDragonsFangAbility.Reference.Get().Icon;

            var monster = UnitRefs.DLC4_TotemFey.Reference.Get();
            var balor = BuffRefs.DemonicFormIVBalorBuff.Reference.Get().GetComponent<Polymorph>();

            var buff = BuffConfigurator.New(ToothHuntBuff, ToothHuntBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
              .AddReplaceAsksList(monster.Visual.Barks)
              .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.NaturalSpell)
              .AddPolymorph([ItemWeaponRefs.Bite3d8.ToString()], false, 0, 8, balor.m_EnterTransition, balor.m_ExitTransition, 
                [AbilityRefs.TurnBackAbilityStandart.ToString(), FeatureRefs.DemonicFormWeaponEnchantFact.ToString(), FeatureRefs.ShifterGrabTiger.ToString(), AbilityRefs.Disintegrate.ToString()],
                false, ItemWeaponRefs.ClawHuge3d6.ToString(), null, BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge.Fail, 10, ItemWeaponRefs.ClawHuge3d6.ToString(),
                monster.PortraitSafe, monster.Prefab, monster.Prefab, null, null, null, true, Kingmaker.Enums.Size.Huge, SpecialDollType.None, 12, balor.m_TransitionExternal, true)
              .Configure();

            AbilityConfigurator.NewSpell(ToothHuntAbility, ToothHuntAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
              .SetIcon(icon)
              .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.SelfTouch)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetLocalizedDuration(AbilityRefs.EnlargePerson.Reference.Get().LocalizedDuration)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Extend, Metamagic.Quicken)
              .AddToSpellLists(level: 6, SpellList.Alchemist)
              .AddToSpellLists(level: 6, SpellList.Druid)
              .AddToSpellLists(level: 6, SpellList.Shaman)
              .AddToSpellLists(level: 7, SpellList.Wizard)
              .AddToSpellLists(level: 7, SpellList.Witch)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), Kingmaker.UnitLogic.Mechanics.DurationRate.Minutes), isFromSpell: true)
                  .Build())
              .Configure();
        }
    }
}
