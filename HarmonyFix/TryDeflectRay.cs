using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Combat;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitCombatState), nameof(UnitCombatState.CannotBeDeflected))]
    internal class TryDeflectRay
    {
        static void Postfix(ref UnitCombatState __instance, ref TimeSpan now, ref Projectile projectile, ref UnitEntityData attacker, ref bool __result)
        {
            if (__result && __instance.Unit.HasFact(Mythic))
            {
                __result = !__instance.Unit.Descriptor.State.Features.DeflectArrows || now - __instance.m_LastDeflectArrowTime < 1.Rounds().Seconds || projectile.AttackRoll == null || projectile.AttackRoll.Weapon.Blueprint.IsNatural || !projectile.AttackRoll.IsHit || (attacker != null && Rulebook.Trigger(new RuleCheckTargetFlatFooted(attacker, __instance.Unit)).IsFlatFooted);
            }
        }

        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(Juggler.DeflectArrowsMythicGuid);
    }
}