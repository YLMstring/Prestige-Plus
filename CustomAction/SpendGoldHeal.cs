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
            unit.RemoveFact(Constitution);
        }
        //internal const string LegendaryConstitution = "a8aab8e0-e601-4b71-82db-ecd5dfdb5953";
        private static BlueprintFeatureReference Constitution = BlueprintTool.GetRef<BlueprintFeatureReference>("a8aab8e0-e601-4b71-82db-ecd5dfdb5953");
    }
}