using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Units;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Blueprint.RogueTalent;
using PrestigePlus.CustomComponent.Charge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;
using PrestigePlus.Blueprint.Feat;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Parts;
using UnityEngine;
using TurnBased.Controllers;
using PrestigePlus.Blueprint.CombatStyle;

namespace PrestigePlus.GrappleMechanic
{
    internal class GiganticAssaultController : BaseUnitController
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public override void TickOnUnit(UnitEntityData unit)
        {
            var turn = Game.Instance.TurnBasedCombatController?.CurrentTurn;
            if (turn?.Rider == unit && !unit.View.IsMoving() && unit.HasFact(Jab) && unit.HasFact(Dancer) && turn.HasFiveFootStep(unit) == false && turn.m_RiderMovementStats.MetersMovedByFiveFootStep > 0)
            {
                GameHelper.RemoveBuff(unit, Dancer);
                turn.m_RiderMovementStats.MetersMovedByFiveFootStep = 0.1f;
            }
            var rhino = unit.GetFact(Buff)?.MaybeContext?.MaybeCaster;
            if (rhino != null && TryAboveAttack.CheckTargetRestriction(rhino, null, out _, unit) && rhino.DistanceTo(unit) <= rhino.CombatSpeedMps * 3f)
            {
                try 
                {
                    if (CombatController.IsInTurnBasedCombat())
                    {
                        if (turn == null) { Logger.Info("wait"); return; }
                        if (!turn.IsActing && !turn.IsMoving) { return; }
                        if (turn.Rider == rhino) { turn.ForceToEnd(); return; }
                    }
                    Vector3 normalized2 = (unit.Position - rhino.Position).normalized;
                    float distance = unit.DistanceTo(rhino) - unit.Corpulence - rhino.Corpulence;
                    UnitEntityData mount = rhino.GetSaddledUnit();;
                    if (mount == null)
                    {
                        CastCharge(rhino, unit);
                        rhino.Ensure<UnitPartForceMove>().Push(normalized2, distance, true);
                    }
                }
                catch (Exception ex) { Logger.Error("fail tick", ex); }
                GameHelper.RemoveBuff(unit, Buff);
            }

        }

        public static void CastCharge(UnitEntityData caster, UnitEntityData target)
        {
            AbilityData spellData = new(Charge, caster.Descriptor);
            if (!spellData.CanTarget(target))
            {
                return;
            }
            AbilityExecutionContext abilityContext3 = caster.Context as AbilityExecutionContext;
            bool isDuplicateSpellApplied = abilityContext3 != null && abilityContext3.IsDuplicateSpellApplied;
            Rulebook.Trigger(new RuleCastSpell(spellData, target)
            {
                IsDuplicateSpellApplied = isDuplicateSpellApplied
            });
        }

        private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>(RhinoCharge.RhinoChargebuffGuid);
        private static BlueprintAbilityReference Charge = BlueprintTool.GetRef<BlueprintAbilityReference>(AerialAssault.ReleaseAbilityGuid);
        private static BlueprintFeatureReference Base = BlueprintTool.GetRef<BlueprintFeatureReference>("{D47DC15C-3A96-4358-A652-DB9E632009A7}");

        private static BlueprintBuffReference Jab = BlueprintTool.GetRef<BlueprintBuffReference>(JabbingStyle.StylebuffGuid);
        private static BlueprintBuffReference Dancer = BlueprintTool.GetRef<BlueprintBuffReference>(JabbingStyle.Stylebuff3Guid);
    }
}
