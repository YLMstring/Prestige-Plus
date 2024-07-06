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
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using PrestigePlus.CustomComponent.Spell;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using BlueprintCore.Utils.Assets;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.Visual;
using UnityEngine;
using Kingmaker.UnitLogic;

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
            var icon = FeatureRefs.WitchHexRestlessSlumberFeature.Reference.Get().Icon;

            var monster = UnitRefs.DLC5_CR10_RagedNymphStorasta.Reference.Get();
            var balor = BuffRefs.BeastShapeIVWyvernBuff.Reference.Get().GetComponent<Polymorph>();
            var port = BlueprintTool.GetRef<BlueprintPortraitReference>("e95da21465774e04d8149de74ad5850e");
            var assetId = "{1029D29B-8E4E-44D9-8241-00C882FCC65E}"; // New GUID identifying your prefab
            var sourceAssetId = monster.Prefab.AssetId; 
            AssetTool.RegisterDynamicPrefabLink(assetId, sourceAssetId, prefab => prefab.transform.localScale = new(0.125f, 0.125f, 0.125f));
            var prefab2 = new PrefabLink() { AssetId = assetId };

            var buff = BuffConfigurator.New(ToothHuntBuff, ToothHuntBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
              .AddReplaceAsksList(monster.Visual.Barks)
              .AddStatBonus(ModifierDescriptor.Size, false, Kingmaker.EntitySystem.Stats.StatType.Strength, -4)
              .AddStatBonus(ModifierDescriptor.Size, false, Kingmaker.EntitySystem.Stats.StatType.Dexterity, 8)
              .AddFacts([AbilityRefs.TurnBackAbilityStandart.ToString(), FeatureRefs.ShifterGriffonWingsFeature.ToString(), FeatureRefs.GriffonheartShifterGriffonShapeFakeFeature.ToString(), AbilityRefs.GriffonDeathFromAboveAbility.ToString()])
              .AddAdditionalLimb(ItemWeaponRefs.Bite2d6.ToString())
              .AddReplaceUnitPrefab(prefab: prefab2)
              .AddChangeUnitSize(size: Size.Diminutive, type: Kingmaker.Designers.Mechanics.Buffs.ChangeUnitSize.ChangeType.Value)
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

    [HarmonyPatch(typeof(WeaponSizeExtension), nameof(WeaponSizeExtension.Shift))]
    internal class FixTinyLimit
    {
        static void Prefix(ref Size minSize)
        {
            try
            {
                if (minSize == Size.Tiny)
                {
                    minSize = Size.Fine;
                }
            }
            catch (Exception ex) { Main.Logger.Error("Failed to FixTinyLimit", ex); }
        }
    }
}
