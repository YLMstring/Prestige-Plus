using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint;
using Kingmaker.UI.Common;
using Kingmaker.EntitySystem.Stats;

namespace PrestigePlus.CustomAction
{
    internal class SpendGoldHeal : ContextAction
    {
        public override string GetCaption()
        {
            return "SpendGoldHeal";
        }

        public override void RunAction()
        {
            var unit = Target?.Unit;
            if (unit == null || !unit.IsDirectlyControllable) { return; }
            var hurt = unit.Descriptor.Damage;
            if (Game.Instance.Player.SpendMoney(hurt * 3 / 2))
            {
                unit.Descriptor.Damage = 0;
            }
            List<ModifiableValue.Modifier> list = new List<ModifiableValue.Modifier>();
            foreach (var mod in unit.Stats.AdditionalAttackBonus.ModifierList) 
            {
                //UIUtility.SendWarning(unit.CharacterName + mod.Key.ToString());
                if (mod.Key == Kingmaker.Enums.ModifierDescriptor.UntypedStackable)
                {
                    foreach (var modifier in mod.Value)
                    {
                        //UIUtility.SendWarning(unit.CharacterName + modifier.Source?.ToString());
                        if (modifier.Source == null && modifier.ModValue > 0)
                        {
                            list.Add(modifier);
                            UIUtility.SendWarning(unit.CharacterName + " removes weird attack bonus " + modifier.ModValue.ToString());
                        }
                    }
                }
            }
            foreach (var modifier in list)
            {
                modifier.Remove();
            }
        }
    }
}