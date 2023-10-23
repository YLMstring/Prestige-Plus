using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    [TypeId("{A681EAE8-B2D8-490C-9188-D3452A18DDE4}")]
    internal class PainfulPurification : UnitFactComponentDelegate, IUnitSubscriber, ISubscriber, IUnitRestHandler, IGlobalSubscriber
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public void HandleUnitRest(UnitEntityData unit)
        {
            try
            {
                if (unit == base.Owner)
                {
                    IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                    if (factContextOwner == null)
                    {
                        return;
                    }
                    base.Owner.Stats.HitPoints.RemoveModifiersFrom(base.Runtime);
                    factContextOwner.RunActionInContext(this.Action, base.Owner);
                }
            }
            catch (Exception e) { Logger.Error("Failed to RemoveModifiers", e); }
        }

        public override void OnActivate()
        {
            base.OnActivate();
            try
            {
                int value = base.Owner.Progression.CharacterLevel;
                base.Owner.Stats.HitPoints.AddModifier(-value, base.Runtime, ModifierDescriptor.UntypedStackable);
            }
            catch (Exception e) { Logger.Error("Failed to AddModifiers", e); }
        }

        public ActionList Action;
    }
}
