using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Kingmaker;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.RuleSystem.Rules;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class ValorousSmite : ContextAction
    {
        public override string GetCaption()
        {
            return "ValorousSmite";
        }

        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            var target = Target.Unit;
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (target == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            if (target.HasFact(Cool) || target.State.HasCondition(UnitCondition.Stunned)) { return; }
            if (target.GetFact(BuffRefs.SmiteEvilBuff.Reference) is Buff smited && smited.Context?.MaybeCaster == caster)
            {
                int dc = caster.Stats.Charisma.Bonus + 10 + caster.Progression.CharacterLevel / 2;
                bool pass = caster.Context.TriggerRule(new RuleSavingThrow(target, Kingmaker.EntitySystem.Stats.SavingThrowType.Will, dc)).Success;
                if (pass)
                {
                    GameHelper.ApplyBuff(target, Cool, new Rounds?(14400.Rounds()));
                }
                else
                {
                    int num = caster.Progression.CharacterLevel / 4 + 1;
                    GameHelper.ApplyBuff(target, BuffRefs.Stunned.Reference, new Rounds?(num.Rounds()));
                }
            }           
        }

        private static BlueprintBuffReference Cool = BlueprintTool.GetRef<BlueprintBuffReference>(DeificObedience.Iomedae2BuffGuid);
    }
}
