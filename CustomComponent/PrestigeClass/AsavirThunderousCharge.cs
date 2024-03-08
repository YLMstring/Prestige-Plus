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
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.Blueprints.Root;
using PrestigePlus.Blueprint.PrestigeClass;

namespace PrestigePlus.Modify
{
    internal class AsavirThunderousCharge : ContextAction
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus"); 
        public override string GetCaption()
        {
            return "Thunderous Charge";
        }

        public override void RunAction()
        {
            try
            {
                //Logger.Info("start");
                UnitEntityData unit = base.Target.Unit;
                if (unit == null)
                {
                    Logger.Info("Target unit is missing");
                    return;
                }
                UnitEntityData maybeCaster = base.Context.MaybeCaster;
                if (maybeCaster == null || !maybeCaster.HasFact(BlueprintRoot.Instance.SystemMechanics.ChargeBuff))
                {
                    Logger.Info("Caster is missing or no charge");
                    return;
                }
                UnitEntityData mount = maybeCaster.GetSaddledUnit();
                if (mount == null) { Logger.Info("Caster not mount"); return; }
                int dc = mount.Stats.Strength.Bonus + 10 + maybeCaster.Descriptor.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(Asavir.ArchetypeGuid));
                //Logger.Info(dc.ToString());
                bool pass = maybeCaster.Context.TriggerRule(new RuleSavingThrow(unit, Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, dc)).Success;
                //Logger.Info("skill check end");
                if (!pass)
                {
                    //Logger.Info("not pass");
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
            catch (Exception ex) { Logger.Error("Failed to thunder.", ex); }

        }
    }
}
