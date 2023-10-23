using BlueprintCore.Utils;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using static UnityModManagerNet.UnityModManager.ModEntry;

namespace PrestigePlus.Modify
{

    [TypeId("{2BB0DDCD-E84B-4689-996F-06A0DC220E9E}")]
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
