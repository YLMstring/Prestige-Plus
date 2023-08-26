using BlueprintCore.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using static UnityModManagerNet.UnityModManager.ModEntry;

namespace PrestigePlus.Modify
{
  internal class RangedAttackExtended : ContextActionRangedAttack
  {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");

        internal ActionList OnHit;

    public override void RunAction()
    {
      try
      {
        base.RunAction();
        var attack = AbilityContext.RulebookContext?.LastEvent<RuleAttackWithWeapon>();
        if (attack is null)
        {
          Logger.Info("No attack triggered");
          return;
        }

        if (attack.AttackRoll.IsHit)
          OnHit.Run();
      }
      catch (Exception e)
      {
        Logger.Error("RangedAttackExtended.RunAction", e);
      }
    }
  }
}
