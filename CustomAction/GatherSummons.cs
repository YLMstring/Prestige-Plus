using Kingmaker.AreaLogic.SummonPool;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using static Pathfinding.Util.RetainedGizmos;
using Owlcat.Runtime.Core.Utils;
using Kingmaker.UnitLogic;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Buffs;

namespace PrestigePlus.CustomAction
{
    internal class GatherSummons : ContextAction
    {
        public override string GetCaption()
        {
            return "GatherSummons";
        }

        // Token: 0x0600D174 RID: 53620 RVA: 0x00366B08 File Offset: 0x00364D08
        public override void RunAction()
        {
            if (Context.MaybeCaster?.IsDirectlyControllable != true) { return; }
            var pools = Game.Instance.SummonPools.m_Pools;
            if (pools == null || pools.Count == 0)
            {
                return;
            }
            foreach (SummonPool pool in pools.Values.ToTempList())
            {
                if (pool.Units == null || pool.Units.Count() == 0) { continue; }
                foreach (UnitEntityData unitEntityData in pool.Units.ToTempList())
                {
                    if (unitEntityData.GetFact(BuffRefs.SummonedUnitBuff.Reference.Get()) is Buff buff)
                    {
                        if (buff.Context.MaybeCaster == Context.MaybeCaster)
                        {
                            unitEntityData.CombatState.PreventAttacksOfOpporunityNextFrame = true;
                            unitEntityData.Position = Context.MaybeCaster.Position;
                        }
                    }
                }
            }
        }
    }
}