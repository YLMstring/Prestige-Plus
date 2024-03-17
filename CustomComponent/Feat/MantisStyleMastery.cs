using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using PrestigePlus.Blueprint.Feat;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class MantisStyleMastery : UnitFactComponentDelegate, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RulePrepareDamage>, IRulebookHandler<RulePrepareDamage>
    {
        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            if (this.ConditionsChecker(evt))
            {
                BaseDamage weaponDamage = evt.DamageBundle.WeaponDamage;
                if (weaponDamage == null)
                {
                    return;
                }
                int bonus = 0;
                foreach (var damage in evt.ParentRule.DamageBundle)
                {
                    if (damage != null && damage.Sneak)
                    {
                        bonus += damage.Dice.BaseFormula.Rolls;
                    }
                }
                if (bonus > 0)
                {
                    BaseDamage baseDamage = weaponDamage.CreateTypeDescription().CreateDamage(new DiceFormula(0, DiceType.Zero), bonus);
                    baseDamage.Sneak = true;
                    baseDamage.Precision = true;
                    evt.Add(baseDamage);
                }
            }
        }

        // Token: 0x0600E422 RID: 58402 RVA: 0x003ABC0C File Offset: 0x003A9E0C
        public void OnEventDidTrigger(RulePrepareDamage evt)
        {
        }

        // Token: 0x0600E423 RID: 58403 RVA: 0x003ABC10 File Offset: 0x003A9E10
        private bool ConditionsChecker(RulePrepareDamage evt)
        {
            ItemEntityWeapon weapon = evt.DamageBundle.Weapon;
            if (weapon?.Blueprint.Category == WeaponCategory.Falcata)
            {
                return true;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(RuleCalculateAttackBonusWithoutTarget), nameof(RuleCalculateAttackBonusWithoutTarget.OnTrigger))]
    internal class MantisStyleFix
    {
        static void Prefix(ref RuleCalculateAttackBonusWithoutTarget __instance)
        {
            var evt = __instance;
            var Owner = evt.Initiator;
            ItemEntityWeapon maybeWeapon = Owner.Body.PrimaryHand.MaybeWeapon;
            ItemEntityWeapon maybeWeapon2 = Owner.Body.SecondaryHand.MaybeWeapon;
            var fact = Owner.GetFact(Master);
            if (fact == null) { return; }
            int bonus = 0;
            if (maybeWeapon?.Blueprint.Category == WeaponCategory.Falcata && maybeWeapon2?.Blueprint.Category == WeaponCategory.Falcata && evt.m_ModifiableBonus?.Modifiers?.First() != null)
            {
                foreach (var mod in evt.m_ModifiableBonus.Modifiers)
                {
                    if (mod.Fact?.Blueprint?.GetComponent<TwoWeaponFightingAttackPenalty>() != null)
                    {
                        bonus += mod.Value;
                    }
                }
                evt.AddModifier(-bonus, fact, ModifierDescriptor.UntypedStackable);
            }
        }

        private static BlueprintFeatureReference Master = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Achaekek3Guid);
    }
}
