using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class ChangeAreaLocation : ContextAction
    {
        public override string GetCaption()
        {
            return "ChangeAreaLocation";
        }

        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            var data = GetData(caster);
            if (data != null)
            {
                data.Position = Target.Point;
            }
        }

        private AreaEffectEntityData GetData(UnitEntityData caster)
        {
            foreach (AreaEffectEntityData areaEffectEntityData in Game.Instance.State.AreaEffects)
            {
                if (areaEffectEntityData?.Blueprint == area)
                {
                    AbilityExecutionContext abilityExecutionContext = GetContext(areaEffectEntityData.Context);
                    if (abilityExecutionContext != null && abilityExecutionContext.MaybeCaster == caster)
                    {
                        return areaEffectEntityData;
                    }
                }
            }
            return null;
        }
        private AbilityExecutionContext GetContext(MechanicsContext mechanicsContext)
		{
            while (mechanicsContext != null)
			{
                if (mechanicsContext.ParentContext is AbilityExecutionContext abilityExecutionContext)
                {
                    return abilityExecutionContext;
                }
                mechanicsContext = mechanicsContext.ParentContext;
			}
            return null;
		}

        public BlueprintAbilityAreaEffect area;
    }
}
