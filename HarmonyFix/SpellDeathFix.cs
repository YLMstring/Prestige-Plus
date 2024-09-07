using HarmonyLib;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic;
using PrestigePlus.CustomComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.EntitySystem;
using Owlcat.Runtime.Core.Utils;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.HarmonyFix
{
    internal class SpellDeathFix
    {
        [HarmonyPatch(typeof(BuffCollection), nameof(BuffCollection.UpdateOnDeadOwner))]
        internal class SpellDeathFix1
        {
            static bool Prefix(ref BuffCollection __instance)
            {
                if (!ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("endure")) || __instance.Owner.Unit.IsPlayersEnemy) 
                {
                    __instance.m_IsOwnerAlive = !__instance.Owner.State.IsDead;
                    return true; 
                }
                if (__instance.m_IsOwnerAlive && __instance.Owner.State.IsDead)
                {
                    List<EntityFact> list = ListPool<EntityFact>.Claim();
                    foreach (Buff buff in __instance.RawFacts)
                    {
                        if (!buff.Blueprint.StayOnDeath && buff.Context?.MaybeCaster?.HasFact(FeatureRefs.EnduringSpells.Reference) != true)
                        {
                            list.Add(buff);
                        }
                    }
                    foreach (EntityFact fact in list)
                    {
                        __instance.RemoveFact(fact);
                    }
                    ListPool<EntityFact>.Release(list);
                }
                __instance.m_IsOwnerAlive = !__instance.Owner.State.IsDead;
                return false;
            }
        }
    }
}
