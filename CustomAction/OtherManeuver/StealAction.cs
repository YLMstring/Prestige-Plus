using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker;
using PrestigePlus.Blueprint.SpecificManeuver;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Kingmaker.Items.Slots;
using Kingmaker.Designers;
using Kingmaker.PubSubSystem;

namespace PrestigePlus.CustomAction.OtherManeuver
{
    internal class StealAction : ContextAction
    {
        public override string GetCaption()
        {
            return "StealAction";
        }

        public override void RunAction()
        {
            ActManeuver(Context.MaybeCaster, Target.Unit);
        }

        public void ActManeuver(UnitEntityData caster, UnitEntityData target)
        {
            if (target == null || !target.IsPlayersEnemy)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            var weapon = target.GetThreatHand();
            if (weapon?.Weapon.Blueprint.IsNatural == true && weapon.Weapon.Blueprint.Category != Kingmaker.Enums.WeaponCategory.UnarmedStrike)
            {
                List<ItemSlot> list = new();
                foreach (var slot in target.Body.EquipmentSlots)
                {
                    if (slot.HasItem)
                    {
                        list.Add(slot);
                    }
                }
                if (list.Count > 0)
                {
                    var stolen = list.Random();
                    stolen.MaybeItem.Identify();
                    GameHelper.GetPlayerCharacter().Inventory.Add(stolen.MaybeItem);
                    if (stolen.Active)
                    {
                        stolen.MaybeItem.OnWillUnequip();
                    }
                    stolen.MaybeItem.HoldingSlot = null;
                    stolen.m_ItemRef = null;
                    stolen.Owner.Body.OnItemRemoved(stolen.MaybeItem);
                    EventBus.RaiseEvent(delegate (IRemoveItemFromSlotHandler h)
                    {
                        h.HandleItemRemovedFromSlot(stolen, stolen.MaybeItem);
                    }, true);
                }
            }
        }
    }
}
