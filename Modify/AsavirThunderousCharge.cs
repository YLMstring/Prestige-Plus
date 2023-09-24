using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Kingmaker.Designers;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Utility;
using static Pathfinding.Util.RetainedGizmos;
using System.Security.AccessControl;
using PrestigePlus.PrestigeClasses;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.Blueprints.Root;

namespace PrestigePlus.Modify
{
    internal class AsavirThunderousCharge : ContextAction
    {
        public override string GetCaption()
        {
            return "Thunderous Charge";
        }

        public override void RunAction()
        {
            UnitEntityData unit = base.Target.Unit;
            if (unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null || !maybeCaster.HasFact(BlueprintRoot.Instance.SystemMechanics.ChargeBuff))
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            UnitEntityData mount = maybeCaster.GetSaddledUnit();
            if (mount != null) return;
            int dc = mount.Stats.Strength.Bonus + 10 + maybeCaster.Descriptor.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(Asavir.ArchetypeGuid));
            bool pass = GameHelper.TriggerSkillCheck(new RuleSkillCheck(unit, Kingmaker.EntitySystem.Stats.StatType.SaveReflex, dc)
            {
                IgnoreDifficultyBonusToDC = mount.IsPlayersEnemy
            }, unit.Context, true).Success;
            if (!pass)
            {
                if (unit.CanBeKnockedOff())
                {
                    unit.Descriptor.State.Prone.ShouldBeActive = true;
                    EventBus.RaiseEvent(delegate (IKnockOffHandler h)
                    {
                        h.HandleKnockOff(maybeCaster, unit);
                    }, true);
                }
            }


        }
    }
}
