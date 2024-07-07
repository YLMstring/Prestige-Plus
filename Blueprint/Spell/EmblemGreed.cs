using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Items.Slots;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic;
using Kingmaker;
using Owlcat.QA.Validation;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;

namespace PrestigePlus.Blueprint.Spell
{
    internal class EmblemGreed
    {
        private const string EmblemGreedAbility = "NewSpell.UseEmblemGreed";
        public static readonly string EmblemGreedAbilityGuid = "{B7D3605D-22A7-4D31-B6FF-52C3BDE43CCA}";

        private const string EmblemGreedBuff = "NewSpell.EmblemGreedBuff";
        public static readonly string EmblemGreedBuffGuid = "{1034AF48-D063-43E1-935C-959D39484484}";

        internal const string DisplayName = "NewSpellEmblemGreed.Name";
        private const string Description = "NewSpellEmblemGreed.Description";

        private const string MaximName = "EmblemGreedWeapon";
        public static readonly string MaximGuid = "{3E39268B-E1E6-404B-8021-E5C03D613C6B}";
        public static void Configure()
        {
            var icon = ActivatableAbilityRefs.KineticBladeBlueFlameBlastAbility.Reference.Get().Icon;

            var glaive = ItemWeaponRefs.GlaiveFlamingPlus1.Reference.Get();

            var maxim = ItemWeaponConfigurator.New(MaximName, MaximGuid)
                .SetDisplayNameText(DisplayName)
                .SetDescriptionText(Description)
                .SetFlavorText(Description)
                .SetIcon(glaive.Icon)
                .SetVisualParameters(glaive.m_VisualParameters)
                .SetDC(1)
                .SetType(WeaponTypeRefs.Glaive.ToString())
                .AddToEnchantments(WeaponEnchantmentRefs.Flaming.ToString())
                .AddToEnchantments(WeaponEnchantmentRefs.Enhancement1.ToString())
                .Configure();

            var buff = BuffConfigurator.New(EmblemGreedBuff, EmblemGreedBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<AddGreedBlade>(c => { c.Blade = maxim; })
              .Configure();

            AbilityConfigurator.NewSpell(EmblemGreedAbility, EmblemGreedAbilityGuid, SpellSchool.Transmutation, canSpecialize: true)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
              .SetIcon(icon)
              .SetAnimation(CastAnimationStyle.SelfTouch)
              .SetRange(AbilityRange.Personal)
              .SetLocalizedDuration(AbilityRefs.EnlargePerson.Reference.Get().LocalizedDuration)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Extend, Metamagic.Quicken)
              .AddToSpellLists(level: 6, SpellList.Cleric)
              .AddToSpellLists(level: 6, SpellList.Inquisitor)
              .AddToSpellLists(level: 7, SpellList.Shaman)
              .AddToSpellLists(level: 6, SpellList.Wizard)
              .AddToSpellLists(level: 6, SpellList.Magus)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), Kingmaker.UnitLogic.Mechanics.DurationRate.Minutes), isFromSpell: true)
                  .Build())
              .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
              .Configure();
        }
    }

    public class AddGreedBlade : UnitBuffComponentDelegate<AddKineticistBladeData>, IAreaActivationHandler, IGlobalSubscriber, ISubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>
    {
        public BlueprintItemWeapon Blade;

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (evt.Initiator == Owner && evt.Weapon?.Blueprint == Blade)
            {
                int num = Buff.Context.Params.CasterLevel - Owner.Stats.BaseAttackBonus;
                ModifierDescriptor des = ModifierDescriptor.UntypedStackable;
                if (num < 0)
                {
                    des = ModifierDescriptor.Penalty;
                }
                evt.AddModifier(num, base.Fact, des);
            }
        }

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {

        }
        public override void OnActivate()
        {
            base.OnActivate();
            base.Owner.MarkNotOptimizableInSave(true);
            base.Data.Applied = this.Blade.CreateEntity<ItemEntityWeapon>();
            base.Data.Applied.MakeNotLootable();
            HandSlot handSlot = Owner.Body.PrimaryHand;
            if (!handSlot.CanInsertItem(base.Data.Applied, false))
            {
                base.Data.Applied = null;
                PFLog.Default.Error("Can't insert kineticist weapon to target hand", Array.Empty<object>());
                return;
            }
            using (ContextData<ItemsCollection.SuppressEvents>.Request())
            {
                handSlot.InsertItem(base.Data.Applied);
            }
        }

        // Token: 0x0600CC05 RID: 52229 RVA: 0x00350E00 File Offset: 0x0034F000
        public override void OnDeactivate()
        {
            base.OnDeactivate();
            if (base.Data.Applied != null)
            {
                ItemSlot holdingSlot = base.Data.Applied.HoldingSlot;
                if (holdingSlot != null)
                {
                    holdingSlot.RemoveItem(true);
                }
                using (ContextData<ItemsCollection.SuppressEvents>.Request())
                {
                    ItemsCollection collection = base.Data.Applied.Collection;
                    if (collection != null)
                    {
                        collection.Remove(base.Data.Applied);
                    }
                }
                base.Data.Applied = null;
            }
        }
        public override void ApplyValidation(ValidationContext context, int parentIndex)
        {
            base.ApplyValidation(context, parentIndex);
        }

        // Token: 0x0600CC07 RID: 52231 RVA: 0x00350ECF File Offset: 0x0034F0CF
        public override void OnTurnOn()
        {
            ItemEntityWeapon applied = base.Data.Applied;
            if (applied == null)
            {
                return;
            }
            ItemSlot holdingSlot = applied.HoldingSlot;
            if (holdingSlot == null)
            {
                return;
            }
            holdingSlot.Lock.Retain();
        }

        // Token: 0x0600CC08 RID: 52232 RVA: 0x00350EF5 File Offset: 0x0034F0F5
        public override void OnTurnOff()
        {
            ItemEntityWeapon applied = base.Data.Applied;
            if (applied == null)
            {
                return;
            }
            ItemSlot holdingSlot = applied.HoldingSlot;
            if (holdingSlot == null)
            {
                return;
            }
            holdingSlot.Lock.Release();
        }

        // Token: 0x0600CC09 RID: 52233 RVA: 0x00350F1B File Offset: 0x0034F11B
        public void OnAreaActivated()
        {
            if (base.Data.Applied == null)
            {
                this.OnActivate();
                this.OnTurnOn();
            }
        }
    }
}
