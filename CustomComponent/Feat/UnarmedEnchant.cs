using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.Enums;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class UnarmedEnchant : UnitBuffComponentDelegate<UnarmedEnchant.ComponentData>
    {
        public BlueprintItemEnchantment Enchantment;
        public override void OnActivate()
        {
            base.Data.Enchantment = Owner.Body.EmptyHandWeapon.AddEnchantment(this.Enchantment, base.Context, null);
        }
        public override void OnDeactivate()
        {
            ItemEnchantment enchantment = base.Data.Enchantment;
            if (enchantment != null)
            {
                ItemEntity owner = enchantment.Owner;
                owner?.RemoveEnchantment(base.Data.Enchantment);
            }
            base.Data.Enchantment = null;
        }
        public class ComponentData
        {
            public ItemEnchantment Enchantment;
        }
    }
}
