using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.GenericSlot;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    [TypeId("{5855D2C2-EB89-4A7E-8157-4F1F3C0E3E8D}")]
    internal class MalevolenceEnchant : ContextCondition
    {
        // Token: 0x0600CDC2 RID: 52674 RVA: 0x00355613 File Offset: 0x00353813
        public override string GetConditionCaption()
        {
            return "Check if target has weapon equipped";
        }

        // Token: 0x0600CDC3 RID: 52675 RVA: 0x0035561C File Offset: 0x0035381C
        public override bool CheckCondition()
        {
            UnitEntityData unitEntityData = base.Target.Unit;
            if (unitEntityData == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return false;
            }
            
                bool flag = this.CheckWeapon(unitEntityData.Body.PrimaryHand);
                flag = (flag || this.CheckWeapon(unitEntityData.Body.SecondaryHand));
                foreach (WeaponSlot slot in unitEntityData.Body.AdditionalLimbs)
                {
                    flag = (flag || this.CheckWeapon(slot));
                }
                return flag;
            
        }

        // Token: 0x0600CDC4 RID: 52676 RVA: 0x0035570C File Offset: 0x0035390C
        private bool CheckWeapon(WeaponSlot slot)
        {
            if (!slot.HasWeapon || slot.MaybeItem == null)
            {
                return false;
            }
            bool flag = slot.MaybeItem.HasEnchantment(BlueprintTool.GetRef<BlueprintItemEnchantmentReference>(WeaponEnchantmentRefs.Unholy.ToString()));
            return flag;
        }

       
    }
}
