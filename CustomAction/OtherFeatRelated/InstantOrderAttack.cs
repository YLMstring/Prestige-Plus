using BlueprintCore.Actions.Builder;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Designers;
using Kingmaker.Utility;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.CombatStyle;
using PrestigePlus.Blueprint.Archetype;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class InstantOrderAttack : ContextAction
    {
        public override string GetCaption()
        {
            return "InstantOrder";
        }

        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            var target = Target.Unit;
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (target == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            foreach (UnitGroupMemory.UnitInfo unitInfo in target.Memory.Enemies)
            {
                UnitEntityData unit = unitInfo.Unit;
                var buff = unit.GetFact(BuffRefs.CavalierChallengeBuffTarget.Reference) as Buff;
                if (target.CombatState.EngagedUnits.Contains(unit) && buff?.Context?.MaybeCaster == caster)
                {
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(target, unit, false);
                    GameHelper.ApplyBuff(target, Daze, new Rounds?(1.Rounds()));
                    return;
                }
            }
        }

        private static BlueprintBuffReference Daze = BlueprintTool.GetRef<BlueprintBuffReference>(Constable.InstantOrderBuffGuid);
    }
}
