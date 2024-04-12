using BlueprintCore.Utils;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;
using static UnityModManagerNet.UnityModManager.Param;

namespace PrestigePlus.Modify
{
    [TypeId("{29896BE9-4421-4B70-9A39-461444E1741C}")]
    internal class PhaseArrow : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, IInitiatorRulebookSubscriber
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            try
            {
                evt.AddModifier(-RuleCalculateAC.CalculateArmorAndShieldBonuses(evt.Target), Fact, ModifierDescriptor.Penalty);
            }
            catch (Exception e) { Logger.Error("Failed to phase arrow.", e); }
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {
            
        }
    }
}
