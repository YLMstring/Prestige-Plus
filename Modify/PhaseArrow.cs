using BlueprintCore.Utils;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
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
    internal class PhaseArrow : BrilliantEnergy, IInitiatorRulebookHandler<RuleCalculateAC>
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            Logger.Info("before AC");
            if (evt != null)
                if (evt.m_ModifiableBonus != null)
                    if (evt.m_ModifiableBonus.Modifiers != null)
                        foreach (var mod in evt.m_ModifiableBonus.Modifiers)
                            Logger.Info(mod.ToString());

            if (evt != null)
                if (evt.BrilliantEnergy == null)
                {
                    Logger.Info("modify AC");
                    evt.AddModifier(-RuleCalculateAC.CalculateArmorAndShieldBonuses(evt.Target), evt.BrilliantEnergy, ModifierDescriptor.UntypedStackable);
                }
            base.OnEventAboutToTrigger(evt);
            if (evt != null)
                if (evt.m_ModifiableBonus != null)
                    if (evt.m_ModifiableBonus.Modifiers != null)
                        foreach (var mod in evt.m_ModifiableBonus.Modifiers)
                            Logger.Info(mod.ToString());
            Logger.Info("after AC");
        }
    }
}
