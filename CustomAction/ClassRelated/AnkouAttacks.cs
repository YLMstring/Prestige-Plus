using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Root;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker;
using UnityEngine;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using UnityEngine.Serialization;
using Kingmaker.UnitLogic.Parts;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class AnkouAttacks : AbilityCustomLogic
    {
        public override bool IsEngageUnit => true;
        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
        {
            var caster = context.MaybeCaster;
            if (caster == null)
            {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                yield break;
            }
            var originalTarget = target.Unit;
            if (originalTarget == null)
            {
                PFLog.Default.Error("Can't be applied to point", Array.Empty<object>());
                yield break;
            }
            var image = caster.Get<UnitPartMirrorImage>();
            if (image == null) yield break;
            GameHelper.ApplyBuff(caster, CooldownBuff, new Rounds?(1.Rounds()));
            GameHelper.ApplyBuff(caster, KeepBuff, new Rounds?(1.Rounds()));
            int rank = caster.GetFact(CooldownBuff).GetRank();
            if (image.MechanicsImages.Count >= rank)
            {
                UnitAttack attack = new(target.Unit) { ForceFullAttack = true };
                attack.IgnoreCooldown();
                attack.Init(caster);
                caster.Commands.AddToQueueFirst(attack);
            }
        }
        public override void Cleanup(AbilityExecutionContext context)
        {
            GameHelper.RemoveBuff(context.MaybeCaster, KeepBuff);
        }

        public BlueprintBuff CooldownBuff;
        public BlueprintBuff KeepBuff;
    }
}