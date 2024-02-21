using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class GhostArmorComponent : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, ITargetRulebookSubscriber, IInitiatorRulebookHandler<RulePrepareDamage>, IRulebookHandler<RulePrepareDamage>, IInitiatorRulebookSubscriber
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            try
            {
                if (evt.Weapon.Blueprint.Type != IncorporealBite
                    && evt.Weapon.Blueprint.Type != IncorporealTouch
                    && evt.Weapon.Blueprint.Type != IncorporealTouchReach)
                    return;

                var bonus = 0;
                if (Owner.Body.SecondaryHand.HasShield)
                {
                    foreach (var mod in Owner.Stats.AC.Modifiers)
                    {
                        if (mod.ModDescriptor == ModifierDescriptor.Shield || mod.ModDescriptor == ModifierDescriptor.ShieldFocus)
                            bonus += mod.ModValue;
                    }

                    bonus += GameHelper.GetItemEnhancementBonus(Owner.Body.SecondaryHand.Shield);
                }
                else
                {
                    foreach (var mod in Owner.Stats.AC.Modifiers)
                    {
                        if (mod.ModDescriptor == ModifierDescriptor.Armor
                            || mod.ModDescriptor == ModifierDescriptor.ArmorFocus
                            || mod.ModDescriptor == ModifierDescriptor.ArmorEnhancement)
                            bonus += mod.ModValue;
                    }

                    // Unarmored enhancement bonus is applied directly to AC as ArmorEnhancement, but not if armor is equipped.
                    if (Owner.Body.Armor.HasArmor)
                    {
                        bonus += GameHelper.GetItemEnhancementBonus(Owner.Body.Armor.Armor);
                    }
                }

                //Logger.Verbose(() => $"Adding {bonus} to AC against incorporeal attack");
                evt.Target.Stats.AC.AddModifierUnique(bonus, Runtime, ModifierDescriptor.UntypedStackable);
            }
            catch (Exception e)
            {
                Logger.Error("GhostArmorComponent.OnEventAboutToTrigger", e);
            }
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            try
            {
                evt.Target.Stats.AC.RemoveModifiersFrom(Runtime);
            }
            catch (Exception e)
            {
                Logger.Error("GhostArmorComponent.OnEventDidTrigger", e);
            }
        }

        void IRulebookHandler<RulePrepareDamage>.OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            if (!onlyarmor)
            {
                evt.DamageBundle.WeaponDamage.Reality = Kingmaker.Enums.Damage.DamageRealityType.Ghost;
            }
        }

        public bool onlyarmor = true;
        void IRulebookHandler<RulePrepareDamage>.OnEventDidTrigger(RulePrepareDamage evt)
        {
            
        }

        private static BlueprintWeaponType _incorporealTouch;
        private static BlueprintWeaponType IncorporealTouch
        {
            get
            {
                _incorporealTouch ??= WeaponTypeRefs.IncoporealTouchType.Reference.Get();
                return _incorporealTouch;
            }
        }

        private static BlueprintWeaponType _incorporealTouchReach;
        private static BlueprintWeaponType IncorporealTouchReach
        {
            get
            {
                _incorporealTouchReach ??= WeaponTypeRefs.ReachIncoporealTouchType.Reference.Get();
                return _incorporealTouchReach;
            }
        }

        private static BlueprintWeaponType _incorporealBite;
        private static BlueprintWeaponType IncorporealBite
        {
            get
            {
                _incorporealBite ??= WeaponTypeRefs.IncoporealBiteType.Reference.Get();
                return _incorporealBite;
            }
        }
    }
}
