﻿using BlueprintCore.Actions.Builder;
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
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using PrestigePlus.CustomComponent.Spell;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace PrestigePlus.Blueprint.Spell
{
    internal class ToothHunt
    {
        private const string ToothHuntAbility = "NewSpell.UseToothHunt";
        public static readonly string ToothHuntAbilityGuid = "{64A98883-B889-4B55-8BAE-F573D7142589}";

        private const string ToothHuntBuff = "NewSpell.ToothHuntBuff";
        private static readonly string ToothHuntBuffGuid = "{AB1820E8-B07C-43C6-B83D-949A5C9EFAA7}";

        internal const string DisplayName = "NewSpellToothHunt.Name";
        private const string Description = "NewSpellToothHunt.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.BlackDragonsFangAbility.Reference.Get().Icon;

            var monster = UnitRefs.CR25_Areshkagal_RetrieverBoss.Reference.Get();
            var balor = BuffRefs.DemonicFormIVBalorBuff.Reference.Get().GetComponent<Polymorph>();

            var buff = BuffConfigurator.New(ToothHuntBuff, ToothHuntBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
              .AddReplaceAsksList(monster.Visual.Barks)
              .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.NaturalSpell)
              .AddPolymorph([ItemWeaponRefs.Bite3d8.ToString()], false, 0, 8, balor.m_EnterTransition, balor.m_ExitTransition, 
                [AbilityRefs.TurnBackAbilityStandart.ToString(), FeatureRefs.ShifterGriffonWingsFeature.ToString(), FeatureRefs.GriffonheartShifterGriffonShapeFakeFeature.ToString(), AbilityRefs.GriffonDeathFromAboveAbility.ToString()],
                true, null, null, BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge.Fail, 0, null,
                monster.PortraitSafe, monster.Prefab, monster.Prefab, null, null, null, false, Kingmaker.Enums.Size.Diminutive, 
                SpecialDollType.None, -4, balor.m_TransitionExternal, true)
              .AddDamageResistancePhysical(isStackable: true, value: 2, material: Kingmaker.Enums.Damage.PhysicalDamageMaterial.ColdIron, bypassedByMaterial: true)
              .Configure();

            AbilityConfigurator.NewSpell(ToothHuntAbility, ToothHuntAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
              .SetIcon(icon)
              .SetAnimation(CastAnimationStyle.SelfTouch)
              .SetRange(AbilityRange.Personal)
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
              .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
              .Configure();
        }
    }
}
