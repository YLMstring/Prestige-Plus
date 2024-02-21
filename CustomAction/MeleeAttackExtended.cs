using BlueprintCore.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomAction
{
    internal class MeleeAttackExtended : ContextActionMeleeAttack
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");

        internal ActionList OnHit;

        public override void RunAction()
        {
            try
            {
                var maybeCaster = Context.MaybeCaster; 
                if (maybeCaster != null)
                {
                    var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
                    maybeCaster.View.AnimationManager.Execute(attackAnimation);
                }
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
                Logger.Error("MeleeAttackExtended.RunAction", e);
            }
        }
    }
}
