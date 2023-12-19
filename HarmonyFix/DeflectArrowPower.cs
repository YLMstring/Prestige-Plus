using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Combat;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.Designers;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.EntitySystem.EntityDataBase;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitCombatState), nameof(UnitCombatState.TryDeflectArrow))]
    internal class DeflectArrowPower
    {
        static void Postfix(ref UnitCombatState __instance, ref Projectile projectile, ref bool __result)
        {
            var maybeCaster = __instance.Unit;
            if (__result && Buff != null)
            {
                GameHelper.ApplyBuff(maybeCaster, Buff, new Rounds?(2.Rounds()));
                int max = 1;
                if (maybeCaster.HasFact(Fast))
                {
                    max += maybeCaster.GetFact(Fast).GetRank();
                }
                if (maybeCaster.HasFact(Mythic))
                {
                    max += maybeCaster.Progression.MythicLevel / 2;
                }
                int time = maybeCaster.GetFact(Buff).GetRank();
                if (time < max)
                {
                    __instance.m_LastDeflectArrowTime = default;
                }
            }
            if (__result && __instance.Unit.HasFact(Ace) && projectile.AttackRoll?.Weapon != null)
            {
                RuleAttackWithWeapon ruleAttackWithWeapon = new(maybeCaster, projectile.Launcher.Unit, projectile.AttackRoll.Weapon, 0)
                {
                    Reason = maybeCaster.Context,
                    AutoHit = false,
                    AutoCriticalThreat = false,
                    AutoCriticalConfirmation = false,
                    ExtraAttack = true,
                    IsFullAttack = false,
                    AttackNumber = 0,
                    AttacksCount = 1
                };
                maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
            }
        }

        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(Juggler.SnatchArrowsGuid);
        private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>(Juggler.FastReactionsBuffGuid);
        private static BlueprintFeatureReference Fast = BlueprintTool.GetRef<BlueprintFeatureReference>(Juggler.FastReactionsGuid);
        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(Juggler.DeflectArrowsMythicGuid);
    }
}
