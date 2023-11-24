using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Rest;
using Kingmaker.GameModes;
using Kingmaker.PubSubSystem;
using Kingmaker.UI;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class MadBrew : ContextAction
    {
        public override string GetCaption()
        {
            return "MadBrew";
        }

        public override void RunAction()
        {
            if (RestHelper.CanSkipTime())
            {
                var action = ActionsBuilder.New()
                    .DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.Wisdom, ContextDice.Value(Kingmaker.RuleSystem.DiceType.D4, 1, 0))
                    .RestoreResource(AbilityResourceRefs.MutagenResource.ToString(), 1)
                    .Build();
                action.Run();
                Game.Instance.AdvanceGameTime(TimeSpan.FromHours(1), false);
                if (Game.Instance.IsModeActive(GameModeType.GlobalMap) || Game.Instance.IsModeActive(GameModeType.CutsceneGlobalMap))
                {
                    Game.Instance.KingdomController.Tick();
                }
                else
                {
                    Game.Instance.MatchTimeOfDay(null);
                }
            }
            else
            {
                EventBus.RaiseEvent(delegate (IWarningNotificationUIHandler h)
                {
                    h.HandleWarning(WarningNotificationType.RestOnThisZoneImpossible, true);
                }, true);
            }
        }
    }
}
