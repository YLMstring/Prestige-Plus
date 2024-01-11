using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class EnergyDrainFalseLife : ContextAction
    {
        public override string GetCaption()
        {
            return "EnergyDrainFalseLife";
        }

        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            if (caster == null || Target.Unit == null) { return; }
            int rank = 1;
            if (caster.Progression.GetClassLevel(soul) >=5) { rank++; }
            bool isCrit = ContextData<ContextAttackData>.Current?.AttackRoll?.IsCriticalConfirmed ?? false;
            if (isCrit) { rank *= 2; }
            RuleDrainEnergy rule = new(caster, Target.Unit, EnergyDrainType.Permanent, null, DiceFormula.Zero, rank);
            rule = Rulebook.Trigger(rule);
            rank = rule.Result;
            var round = new Rounds?(600.Rounds());
            if (rank >= 4)
            {
                GameHelper.ApplyBuff(caster, Buff4, round);
            }
            else if (rank == 3)
            {
                GameHelper.ApplyBuff(caster, Buff3, round);
            }
            else if (rank == 2)
            {
                GameHelper.ApplyBuff(caster, Buff2, round);
            }
            else if (rank == 1)
            {
                GameHelper.ApplyBuff(caster, Buff1, round);
            }
        }

        private static BlueprintBuffReference Buff1 = BlueprintTool.GetRef<BlueprintBuffReference>(Souldrinker.EnergyDrainBuffGuid);
        private static BlueprintBuffReference Buff2 = BlueprintTool.GetRef<BlueprintBuffReference>(Souldrinker.EnergyDrainBuff2Guid);
        private static BlueprintBuffReference Buff3 = BlueprintTool.GetRef<BlueprintBuffReference>(Souldrinker.EnergyDrainBuff3Guid);
        private static BlueprintBuffReference Buff4 = BlueprintTool.GetRef<BlueprintBuffReference>(Souldrinker.EnergyDrainBuff4Guid);
        private static BlueprintCharacterClassReference soul = BlueprintTool.GetRef<BlueprintCharacterClassReference>(Souldrinker.ArchetypeGuid);
    }
}
